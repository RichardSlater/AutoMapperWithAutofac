using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;

namespace Application.Infrastructure {
    public static class AutoMapperExtensions {
        public static ContainerBuilder ConfigureAutoMapper(this ContainerBuilder builder) {
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .AsClosedTypesOf(typeof(ITypeConverter<,>))
                .AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(AutoMapperExtensions).Assembly)
                .AssignableTo<Profile>().As<Profile>();

            return builder;
        }
    }
}
