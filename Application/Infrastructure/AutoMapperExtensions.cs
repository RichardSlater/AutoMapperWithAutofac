using System;
using System.Collections.Generic;
using Application.Models;
using Application.Validators;
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
            builder.RegisterType<SourceModelValidator>().As<IValidator<SourceModel>>();

            builder.Register(c => {
                var profiles = c.Resolve<IEnumerable<Profile>>();
                var context = c.Resolve<IComponentContext>();
                return new MapperConfiguration(x => {
                    foreach (var profile in profiles) x.AddProfile(profile);
                    x.ConstructServicesUsing(context.Resolve);                   
                });
            }).SingleInstance().AsSelf();

            builder.Register(c => {
                var context = c.Resolve<IComponentContext>();
                var config = context.Resolve<MapperConfiguration>();
                return config.CreateMapper();
            }).As<IMapper>();

            return builder;
        }
    }
}