using System;
using System.Collections.Generic;
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

            builder.Register(context => {
                var profiles = context.Resolve<IEnumerable<Profile>>();
                return new MapperConfiguration(x => {
                    foreach (var profile in profiles) x.AddProfile(profile);
                });
            }).SingleInstance().AutoActivate().AsSelf();

            builder.Register(context => {
                var componentContext = context.Resolve<IComponentContext>();
                var config = componentContext.Resolve<MapperConfiguration>();
                return config.CreateMapper();
            }).As<IMapper>();

            return builder;
        }
    }
}