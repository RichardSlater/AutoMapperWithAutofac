using Application.Models;

namespace Application.Validators {
    public class SourceModelValidator : IValidator<SourceModel> {
        public bool Validate(SourceModel ob) {
            return ob != null;
        }
    }
}