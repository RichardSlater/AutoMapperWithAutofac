using Application.Infrastructure;
using Autofac;

namespace Application {
    internal class Program {
        private static void Main() {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.ConfigureAutoMapper();
        }
    }
}