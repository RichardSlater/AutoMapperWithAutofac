# Configuring AutoMapper to fulfil ITypeConverter<,> constructor dependecies with Autofac

## The Question

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

However from the moment I add a dependency, in this contrived example, a validator:

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

## The Answer

Travis Illig has provided a [hollistic answer to the question][so-40306029] which I am marking as the answer as it answers the question in a broad and generic way.  However, I also wanted to document the **specific solution** to my question.

You need to be fairly careful of how you wire up the dependency resolver to AutoMapper, to be precice you must resolve the component context within the closure - failing to do so will result in the context being disposed before AutoMapper ever gets a chance to resolve it's dependencies.

### Solution #1

In my example, the following code block that registers the `IMapper` using the previously defined `MapperConfiguration`:

<!-- language: lang-cs -->

    builder.Register(c => {
        var context = c.Resolve<IComponentContext>();
        var config = context.Resolve<MapperConfiguration>();
        return config.CreateMapper();
    }).As<IMapper>();

Can be trivially adapted by using an overload of `MapperConfiguration.CreateMapper()` that accepts a `Func<Type, object>` as an argument named `serviceCtor` that AutoMapper will use to construct dependencies:

<!-- language: lang-cs -->

    builder.Register(c => {
        var context = c.Resolve<IComponentContext>();
        var config = context.Resolve<MapperConfiguration>();
        return config.CreateMapper(context.Resolve);
    }).As<IMapper>();

It's essential that the Component Context `context` is used as it is declared within the closure, attempting to use `c` will result in the following exception:

> This resolve operation has already ended. When registering components using lambdas, the `IComponentContext` '`c`' parameter to the lambda cannot be stored. Instead, either resolve `IComponentContext` again from '`c`', or resolve a `Func<>`` based factory to create subsequent components from.

  [github-repo]: https://github.com/RichardSlater/AutoMapperWithAutofac
  [so-33980760]: http://stackoverflow.com/questions/33980760/how-to-inject-automapper-with-autofac
  [so-40293597]: http://stackoverflow.com/questions/40293597/configuring-automapper-to-fulfil-itypeconverter-constructor-dependecies-with
  [so-40306029]: http://stackoverflow.com/a/40306029/74302
