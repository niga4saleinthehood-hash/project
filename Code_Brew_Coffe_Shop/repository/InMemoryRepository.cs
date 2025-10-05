using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.repository
{
    public class InMemoryRepository<T , Tkey>: IRepository<T, Tkey> where T : class
    {
        public readonly Dictionary<Tkey, T> _store = new Dictionary<Tkey, T>();
        public readonly Func<T, Tkey> _keySelector;

        public InMemoryRepository(Func<T, Tkey> keySelector)
        {
            if(keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            _keySelector = keySelector;
        }

        public void Add(T entity)
        {
            var key = _keySelector(entity);
            _store[key] = entity;  
        }

        public void Update(T entity)
        {
            var key = _keySelector(entity);
            if (!_store.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Entity with key {key} not found.");
            }
            _store[key] = entity;
        }

        public IEnumerable<T> GetAll()
        {
            return _store.Values;
        }
        public T GetById(Tkey id)
        {
            T value;
            return _store.TryGetValue(id, out value) ? value : null;
        }

        public void Remove(Tkey id)
        {
            _store.Remove(id);
        }



    }
}
