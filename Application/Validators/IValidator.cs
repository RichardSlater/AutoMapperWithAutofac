namespace Application.Validators {
    public interface IValidator<in T> {
        bool Validate(T obj);
    }
}