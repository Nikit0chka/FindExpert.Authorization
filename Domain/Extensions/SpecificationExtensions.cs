using Ardalis.Specification;

namespace Domain.Extensions;

public static class SpecificationExtensions
{
    public static ISpecification<T> AsNoTracking<T>(this ISpecification<T> spec)
        where T : class
    {
        spec.Query.AsNoTracking();
        return spec;
    }

    public static ISpecification<T, TResult> AsNoTracking<T, TResult>(this ISpecification<T, TResult> spec)
        where T : class
    {
        spec.Query.AsNoTracking();
        return spec;
    }

    public static ISingleResultSpecification<T> AsNoTracking<T>(this ISingleResultSpecification<T> spec)
        where T : class
    {
        spec.Query.AsNoTracking();
        return spec;
    }

    public static ISingleResultSpecification<T, TResult> AsNoTracking<T, TResult>(this ISingleResultSpecification<T, TResult> spec)
        where T : class
    {
        spec.Query.AsNoTracking();
        return spec;
    }
}