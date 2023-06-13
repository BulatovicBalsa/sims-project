using System.Collections.Generic;

namespace Hospital.Filter;

public interface IFilter<T>
{
    List<T> Filter(T persons, ISpecification<T> specification);
}