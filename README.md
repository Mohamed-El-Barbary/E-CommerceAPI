# E-Commerce API (.NET)

A production-style e-commerce backend built with **ASP.NET Core** using **Onion Architecture**, featuring JWT authentication with refresh tokens, Redis caching/session management, and Stripe payment processing.

---

## 📝 Overview

This project is a RESTful e-commerce backend that handles the full customer journey — from registering an account and logging in, through browsing products, managing a cart, and completing a paid order via Stripe.

It's built around **Onion Architecture** to keep core business rules (what a valid order looks like, how pricing is calculated, etc.) completely decoupled from frameworks and infrastructure. That means the database, cache, and payment provider are all swappable implementation details, not things the business logic depends on.

Two supporting concerns drive most of the infrastructure design:
- **Auth & sessions** — stateless JWT access tokens for speed, paired with server-tracked refresh tokens in Redis for security and instant revocation.
- **Payments** — Stripe PaymentIntents with webhook-driven order confirmation, so order state is never trusted from the client.

---

## ✨ Project Features

**Auth & Users**
- User registration with hashed passwords (BCrypt / `PasswordHasher<T>`)
- Login issuing short-lived JWT access tokens + Redis-backed refresh tokens
- Refresh token rotation with reuse detection
- Logout / "logout everywhere" via Redis token revocation
- Role-based authorization (e.g. `Customer` vs `Admin`)

**Products & Catalog**
- CRUD for products (admin-only write access)
- Redis-cached product listings/detail for fast reads
- Search & filter (category, price range)

**Cart**
- Add/remove/update items in cart
- Persistent cart for logged-in users (DB), ephemeral cart for guests (Redis with TTL)
- Server-side price recalculation at checkout (never trusts client-sent totals)

**Orders & Payments**
- Checkout flow that creates an `Order` and a Stripe `PaymentIntent`
- Stripe webhook handling with signature verification
- Idempotent webhook processing via Redis (handles Stripe's at-least-once delivery)
- Order status lifecycle: `Pending → Paid / Failed → (Shipped → Delivered, optional)`
- Inventory decrement on successful payment

**Cross-cutting**
- Global exception handling middleware with consistent error responses
- Rate limiting on sensitive endpoints (login, checkout)
- Request validation via FluentValidation
- Swagger/OpenAPI documentation
- Structured logging (Serilog recommended)

---

## 🧰 Tech Stack

- **Framework:** ASP.NET Core 8 Web API (C#)
- **Architecture:** Onion / Clean Architecture
- **Database:** SQL Server / PostgreSQL via **EF Core**
- **Cache / Session Store:** **Redis** (`StackExchange.Redis`)
- **Payments:** **Stripe** (`Stripe.net`)
- **Auth:** JWT (Access + Refresh tokens) via `Microsoft.AspNetCore.Authentication.JwtBearer`
- **Mediator/CQRS:** MediatR
- **Validation:** FluentValidation
- **Mapping:** AutoMapper
- **Testing:** xUnit + Moq + Testcontainers
- **Docs:** Swagger / Swashbuckle

---

## 🔐 Authentication Flow (JWT + Refresh Tokens)

1. **Register** — `POST /api/auth/register`
   User submits credentials → password hashed with `PasswordHasher` (BCrypt.Net or ASP.NET Core Identity's `PasswordHasher<T>`) → user saved via `IUserRepository`.

2. **Login** — `POST /api/auth/login`
   - Credentials validated against the stored hash.
   - On success, two tokens are issued by `JwtTokenService`:
     - **Access Token** (JWT, short-lived, ~15 min, signed with `SymmetricSecurityKey`) — returned in the response body, sent as `Authorization: Bearer <token>` on subsequent requests.
     - **Refresh Token** (opaque random string or JWT, long-lived, ~7 days) — stored in **Redis** (`StackExchange.Redis`, keyed by user ID/token ID) and set as an `HttpOnly`, `Secure`, `SameSite=Strict` cookie.
   - Storing the refresh token server-side in Redis (instead of trusting a stateless JWT) enables **instant revocation**.

3. **Access protected routes**
   Standard ASP.NET Core `[Authorize]` attribute + JWT Bearer middleware (configured in `Program.cs`) validates signature, issuer, audience, and expiry on every request.

4. **Refresh** — `POST /api/auth/refresh`
   - Client sends the refresh token (via cookie).
   - `RefreshTokenCommandHandler` checks Redis to confirm the token is still valid (not revoked/rotated).
   - If valid: issue a new access token **and rotate** the refresh token — delete the old Redis entry, store a new one. This is **refresh token rotation**, preventing replay attacks if a token is stolen.
   - If invalid/expired/reused: return `401`, force re-login (optionally flag the whole token family as compromised).

5. **Logout** — `POST /api/auth/logout`
   Deletes the refresh token entry from Redis and clears the cookie — the token becomes immediately unusable.

```
Login ──► Access Token (JWT, 15m) + Refresh Token (Redis + HttpOnly cookie, 7d)
              │
              ▼
[Authorize] Request ──► JwtBearer middleware verifies JWT ──► 200 OK
              │
              ▼ (access token expired → 401)
POST /auth/refresh ──► check Redis ──► rotate token ──► new Access Token
```

### Program.cs — JWT setup (reference)
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:AccessSecret"]!))
        };
    });

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(config["Redis:ConnectionString"]!));
```

### Why Redis for refresh tokens?
- O(1) lookup/invalidation via `StackExchange.Redis`.
- Native key TTL — set expiry to match the refresh token lifetime so Redis auto-expires stale entries.
- Enables "logout everywhere" by scanning/deleting all keys under `refresh:{userId}:*`.

---

## 🧠 Redis Usage Summary

| Use Case | Key Pattern | TTL |
|---|---|---|
| Refresh token storage | `refresh:{userId}:{tokenId}` | 7 days |
| Access token blacklist (optional, on logout) | `blacklist:{jti}` | matches token `exp` |
| Product catalog cache (`IDistributedCache` / `IDatabase`) | `product:{productId}` | 5–10 min |
| Cart session (guest users) | `cart:{sessionId}` | 24 hrs |
| Rate limiting (login attempts, API throttling) | `ratelimit:{ip}:{route}` | rolling window |
| Idempotency keys (Stripe webhooks) | `stripe:event:{eventId}` | 24 hrs |

---

## 💳 Order & Payment Flow (Stripe)

1. **Add to cart** — `POST /api/cart` — items stored per user (EF Core) or per session (Redis for guests).
2. **Checkout initiation** — `POST /api/orders/checkout`
   - Server recalculates the cart total server-side (never trust client-sent prices).
   - Creates an `Order` entity in `Pending` state via `CreateOrderCommandHandler`.
   - Calls Stripe via `IPaymentService` → `StripePaymentService` (`Stripe.net`'s `PaymentIntentService.CreateAsync`).
   - Returns `client_secret` to the frontend.
3. **Frontend confirms payment** using Stripe.js / Stripe Elements with the `client_secret`.
4. **Stripe Webhook** — `POST /api/payments/webhook`
   - Stripe sends `payment_intent.succeeded` (or `.failed`).
   - Signature verified with `EventUtility.ConstructEvent(json, signatureHeader, webhookSecret)`.
   - The event ID is checked/stored in Redis to guarantee **idempotency** (Stripe may redeliver the same event).
   - On success: order status → `Paid`, inventory decremented, confirmation email queued.
   - On failure: order status → `Failed`.
5. **Order confirmation** — `GET /api/orders/{id}` — client polls or is notified once the webhook updates the order.

```
Cart ──► Checkout ──► Create Order (Pending) ──► Stripe PaymentIntent
                                                        │
                                              client confirms payment
                                                        │
                                                        ▼
                                    Stripe Webhook ──► verify signature
                                                    ──► check Redis idempotency
                                                    ──► update Order (Paid/Failed)
```

**Important:** Order state is only ever finalized via the **webhook**, never a client-side redirect callback — this prevents inconsistent state if the user closes their browser mid-payment, and prevents spoofed "payment succeeded" calls from the frontend.

---

### Stripe Webhook (local testing)
```bash
stripe listen --forward-to https://localhost:5001/api/payments/webhook
```

Swagger UI available at `https://localhost:5001/swagger` in development.

---

## 📄 License

MIT
