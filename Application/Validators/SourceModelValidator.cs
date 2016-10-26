using Application.Models;

namespace Application.Validators {
    public class SourceModelValidator {
        public bool Validate(SourceModel model) {
            return model != null;
        }
    }
}