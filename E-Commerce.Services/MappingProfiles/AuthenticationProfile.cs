using AutoMapper;
using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.MappingProfiles
{
    internal class AuthenticationProfile : Profile
    {

        public AuthenticationProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
        }

    }
}
