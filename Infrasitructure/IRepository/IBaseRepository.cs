using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasitructure.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<List<T>> GetAll();
        public T GetById(int id);
        public void Add(T entity,Action<string> LogAction);
        public void Update(T entity, Action<string> LogAction);
        public void Delete(int id, Action<string> LogAction);
    }
}
