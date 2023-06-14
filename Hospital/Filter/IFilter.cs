using System.Collections.Generic;

namespace Hospital.Filter;

public interface IFilter<T>
{
    List<T> Filter(List<T> itemsToFilter, ISpecification<T> specification);
}