using System.Diagnostics.CodeAnalysis;
using Application.Models;
using Application.Validators;
using Should;
using Xunit;

namespace Tests {
    public class SourceModelValidatorTests {
        [Fact]
        [SuppressMessage("ReSharper", "ExpressionIsAlwaysNull")]
        public void ShouldFailValidationOnNullModel() {
            var validator = new SourceModelValidator();
            SourceModel model = null;
            var result = validator.Validate(model);
            result.ShouldBeFalse();
        }

        [Fact]
        public void ShouldPassValidationOnNullModel() {
            var validator = new SourceModelValidator();
            var model = new SourceModel {Items = new[] {"One", "Two"}};
            var result = validator.Validate(model);
            result.ShouldBeTrue();
        }
    }
}