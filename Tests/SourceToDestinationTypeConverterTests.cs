using Application.Models;
using Application.TypeConverters;
using Application.Validators;
using Moq;
using Should;
using Xunit;

namespace Tests {
    public class SourceToDestinationTypeConverterTests {
        private static SourceToDestinationTypeConverter CreateConverter(bool isValid) {
            var mockValidator = new Mock<IValidator<SourceModel>>();
            mockValidator.Setup(x => x.Validate(It.IsAny<SourceModel>())).Returns(isValid);
            var converter = new SourceToDestinationTypeConverter(mockValidator.Object);
            return converter;
        }

        [Fact]
        public void ShouldReturnEqualValuesIfListOfOne() {
            var converter = CreateConverter(true);
            var source = new SourceModel {Items = new[] {"One"}};
            var destination = converter.Convert(source, null, null);
            destination.FirstItem.ShouldEqual("One");
            destination.LastItem.ShouldEqual("One");
        }

        [Fact]
        public void ShouldReturnNullObjectIfNullList() {
            var converter = CreateConverter(false);
            var source = new SourceModel {Items = null};
            var destination = converter.Convert(source, null, null);
            destination.ShouldBeNull();
        }
    }
}