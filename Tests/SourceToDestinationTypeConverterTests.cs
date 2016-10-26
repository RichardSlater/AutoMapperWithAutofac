using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.TypeConverters;
using Should;
using Xunit;

namespace Tests {
    public class SourceToDestinationTypeConverterTests {
        [Fact]
        public void ShouldReturnNullObjectIfNullList() {
            var converter = new SourceToDestinationTypeConverter();
            var source = new SourceModel { Items = null };
            var destination = converter.Convert(source, null, null);
            destination.ShouldBeNull();
        }

        [Fact]
        public void ShouldReturnEqualValuesIfListOfOne() {
            var converter = new SourceToDestinationTypeConverter();
            var source = new SourceModel { Items = new [] { "One" } };
            var destination = converter.Convert(source, null, null);
            destination.FirstItem.ShouldEqual("One");
            destination.LastItem.ShouldEqual("One");
        }
    }
}
