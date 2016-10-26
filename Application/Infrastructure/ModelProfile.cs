using Application.Models;
using Application.TypeConverters;
using AutoMapper;

namespace Application.Infrastructure {
    public class ModelProfile : Profile {
        public ModelProfile() {
            CreateMap<SourceModel, DestinationModel>()
                .ConvertUsing<SourceToDestinationTypeConverter>();
        }
    }
}