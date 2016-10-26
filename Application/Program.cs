using System;
using System.Linq;
using Application.Infrastructure;
using Application.Models;
using Autofac;
using AutoMapper;

namespace Application {
    internal class Program {
        private static void Main(string[] args) {
            if (!args.Any()) args = new[] {"DummyOne", "DummyTwo", "DummyThree"};
            var containerBuilder = new ContainerBuilder();
            containerBuilder.ConfigureAutoMapper();
            var container = containerBuilder.Build();
            var input = new SourceModel {Items = args};
            var mapper = container.Resolve<IMapper>();
            var output = mapper.Map<SourceModel, DestinationModel>(input);
            Console.WriteLine(output);
            Console.Write("Press any key to continue...");
            Console.ReadLine();
        }
    }
}