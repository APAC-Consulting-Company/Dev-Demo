using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

namespace Demo1.BL
{
    public static class AutomapperConfig
    {
        public static void Configure()
        {
            ConfigureInternal();
        }

        private static void ConfigureInternal()
        {
            Mapper.AddProfile(new Features.Users.MappingProfile());
        }
    }
}
