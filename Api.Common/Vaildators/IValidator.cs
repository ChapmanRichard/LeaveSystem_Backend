namespace Api.Common.Vaildators
{
    public interface IValidator<T>
    {
        void ValidateObject(T instance);
    }
}
