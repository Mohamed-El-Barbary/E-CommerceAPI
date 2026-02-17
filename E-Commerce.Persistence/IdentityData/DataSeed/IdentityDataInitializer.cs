using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace E_Commerce.Persistence.IdentityData.DataSeed;

public class IdentityDataInitializer : IDataInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<IdentityDataInitializer> _logger;

    public IdentityDataInitializer(
        UserManager<ApplicationUser> userManager ,
        RoleManager<IdentityRole> roleManager,
        ILogger<IdentityDataInitializer> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }
    
    public async Task InitializeAsync()
    {

        try
        {

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!_userManager.Users.Any())
            {
                var user01 = new ApplicationUser()
                {
                    DisplayName = "Mohamed Elbarbary",
                    UserName = "MohamedElbarbary",
                    Email = "mohamedelbarbary511@gmail.com",
                    PhoneNumber = "01092814027"
                };
                var user02 = new ApplicationUser()
                {
                    DisplayName = "Mohamed Ali",
                    UserName = "MohamedAli",
                    Email = "MohamedAli511@gmail.com",
                    PhoneNumber = "01212859631"
                };
                
                await _userManager.CreateAsync(user01,"P@ssw0rd");
                await _userManager.CreateAsync(user02,"P@ssw0rd");
                
                await _userManager.AddToRoleAsync(user01, "SuperAdmin");
                await _userManager.AddToRoleAsync(user02, "Admin");
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error While Seeding Identity Database : Message = {ex.Message} ");
        }
        
    }
}