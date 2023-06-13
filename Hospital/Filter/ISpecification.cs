namespace Hospital.Filter;

public interface ISpecification<in T>
{
    bool IsSatisfied(T item);
}