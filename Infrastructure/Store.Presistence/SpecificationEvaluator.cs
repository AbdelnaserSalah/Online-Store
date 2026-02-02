using Microsoft.EntityFrameworkCore;
using Store.Domain.Contracts;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presistence
{
    public static class SpecificationEvaluator 
    {
      //  _context.Products.Include(p=>p.Brand).Include(P=>P.Type).ToListAsync() as IEnumerable<TEntity>
        public static IQueryable<TEntity> GetQuery<TKey,TEntity>(IQueryable<TEntity> inputQuery,ISpecifications<TKey,TEntity> spec) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery; //_context.Products
            // Apply criteria
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);// _context.Products.Where(spec.Criteria)
            }


            if(spec.Orderby is not null)
            {
                query = query.OrderBy(spec.Orderby);
            }
            else if(spec.OrderbyDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderbyDescending);
            }
            else if (spec.OrderbyDescendingName is not null)
            {
                query = query.OrderByDescending(spec.OrderbyDescendingName);
            }


            if(spec.IsPagination)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            // Apply includes
            // _context.Products.Where(spec.Criteria).Include(p=>p.Brand).Include(P=>P.Type)
            query = spec.Includes.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));


            return query;
        }
    }
}
