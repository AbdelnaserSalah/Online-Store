using Store.Domain.Contracts;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifcations
{
    public class BaseSpecifications<TKey, TEntity> : ISpecifications<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }=new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, bool>>? Criteria { get ; set ; }
        public Expression<Func<TEntity, object>>? Orderby { get; set ; }
        public Expression<Func<TEntity, object>>? OrderbyDescending { get; set ; }
        public Expression<Func<TEntity, object>>? OrderbyDescendingName { get ; set; }
        public int Skip { get; set ; }
        public int Take { get ; set; }
        public bool IsPagination { get; set; }

        public BaseSpecifications(Expression<Func<TEntity, bool>>? expression)
        {
            Criteria= expression;
        }



        public void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            Orderby = orderByExpression;
        }
        public void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderbyDescending = orderByDescExpression;
        }

        public void ApplyPaging(int pagenum, int pagesize)
        {
            IsPagination = true;
            Skip = (pagenum - 1) * pagesize;
            Take = pagesize;
        }

    }
}
