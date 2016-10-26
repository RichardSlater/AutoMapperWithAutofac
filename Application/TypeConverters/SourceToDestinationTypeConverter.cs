using System.Linq;
using Application.Models;
using Application.Validators;
using AutoMapper;

namespace Application.TypeConverters {
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
}