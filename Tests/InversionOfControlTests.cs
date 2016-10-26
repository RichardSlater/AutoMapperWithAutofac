using Application.Infrastructure;
using Autofac;
using AutoMapper;
using Should;
using Xunit;

namespace Tests {
    public class InversionOfControlTests {
        [Fact]
        public void ShouldResolveTypeConverter() {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.ConfigureAutoMapper();
            var container = containerBuilder.Build();
            var mapper = container.Resolve<IMapper>();
            mapper.ShouldNotBeNull();
        }
    }
}