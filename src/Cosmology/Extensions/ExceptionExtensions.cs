namespace Cosmology.Extensions;

public static class ExceptionExtensions
{
    public static bool IsOfType<TException>(this Exception e) =>
        e.GetType() == typeof(TException);
}