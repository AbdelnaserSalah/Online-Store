using Store.Domain.Contracts;
using Store.Domain.Entities;
using Store.Presistence.Data.Contexts;
using Store.Presistence.Repostories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presistence
{
    public class Unitofwork(StoreDbContext context) : IUnitofWork
    {
        private Dictionary<string, object> _repositories = new Dictionary<string, object>();
        public IGenericRepository<Tkey, TEntity> GetRepository<Tkey, TEntity>() where TEntity : BaseEntity<Tkey>
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new GenericRepository<Tkey, TEntity>(context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepository<Tkey, TEntity>)_repositories[type];
        }

        public Task<int> SaveChangesAsync()
        {
          return  context.SaveChangesAsync();
        }
    }
}
