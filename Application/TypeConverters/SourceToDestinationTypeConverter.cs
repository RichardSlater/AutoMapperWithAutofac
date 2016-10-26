using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using AutoMapper;

namespace Application.TypeConverters {
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
}
