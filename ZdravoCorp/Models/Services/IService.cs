using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models.Services
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        void Update(T entity);
        void Delete(T entity);
    }
}
