using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstarct
{
    public interface IGeneric<T> where T : class
    {
        void Create(T item);
        void Delete(T item);
        void Update(T item);
        List<T> GetList();
        T GetById(int id);
    }
}
