using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.repository
{
    public interface IRepository<T, Tkey> where T : class
    {
        T GetById (Tkey id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Remove (Tkey id);
    }
}
