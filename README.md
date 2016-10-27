# Configuring AutoMapper to fulfil ITypeConverter<,> constructor dependecies with Autofac

My first time working with Autofac to inject AutoMapper's `IMapper` interface into classes that have an object mapping requirement.  I have made some progress, [with a little help][so-33980760], getting the various dependencies added to AutoMapper's register using Assembly Scanning:

<!-- language: lang-cs -->

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

This works perfectly for an `ITypeConverter<,>` that doesn't have any injected dependencies:

<!-- language: lang-cs -->

    public class SourceToDestinationTypeConverter : ITypeConverter<SourceModel, DestinationModel> {
        public DestinationModel Convert(SourceModel source, DestinationModel destination, ResolutionContext context) {
            if (source.Items == null) {
                return null;
            }

            return new DestinationModel {
                FirstItem = source.Items.FirstOrDefault(),
                LastItem = source.Items.LastOrDefault()
            };
        }
    }

However from the moment I add a dependency, in this contrived example, n validator:

<!-- language: lang-cs -->

    public class SourceToDestinationTypeConverter : ITypeConverter<SourceModel, DestinationModel> {
        private readonly IValidator<SourceModel> _validator;

        public SourceToDestinationTypeConverter(IValidator<SourceModel> validator) {
            _validator = validator;
        }

        public DestinationModel Convert(SourceModel source, DestinationModel destination, ResolutionContext context) {
            if (!_validator.Validate(source)) return null;

            return new DestinationModel {
                FirstItem = source.Items.FirstOrDefault(),
                LastItem = source.Items.LastOrDefault()
            };
        }
    }

The following exception is thrown:

> `Application.TypeConverters.SourceToDestinationTypeConverter` needs to have a constructor with 0 args or only optional args

It seems clear to me that AutoMapper **needs to be told to use Autofac** to fulfil the dependencies. However, I haven't been able to find out how to tell it to do so.

The full solution is [available on GitHub][github-repo] if further clarification of the error is required.  This question was [asked on StackOverflow][so-40293597] on 2016-10-27. 

  [github-repo]: https://github.com/RichardSlater/AutoMapperWithAutofac
  [so-33980760]: http://stackoverflow.com/questions/33980760/how-to-inject-automapper-with-autofac
  [so-40293597]: http://stackoverflow.com/questions/40293597/configuring-automapper-to-fulfil-itypeconverter-constructor-dependecies-with
