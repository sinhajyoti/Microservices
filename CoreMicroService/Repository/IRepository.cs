using CoreMicroService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMicroService.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Add(T item);
        void Remove(string id);
        void Update(T item);
        T FindByID(string id);
        IEnumerable<T> FindAll();
    }
}
