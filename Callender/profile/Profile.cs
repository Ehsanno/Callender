using AutoMapper;
using Callender.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Callender.profile
{
    public class ProFile : Profile
    {
        public ProFile()
        {
            CreateMap<AccountCreateDto, User>();
            CreateMap<User, UserRole>();
            CreateMap<SetSuggest, Suggest>();
            CreateMap<SetUserCarrier, UserCarrier>();
        }
    }
}
