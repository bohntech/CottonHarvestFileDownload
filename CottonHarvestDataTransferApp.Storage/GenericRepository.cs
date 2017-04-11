using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using CottonHarvestDataTransferApp.Data;

namespace CottonHarvestDataTransferApp.Storage
{
    public class GenericRepository<TObject> where TObject: class
    {
        protected AppDBEntities _context = null;

        public GenericRepository(AppDBEntities context) {
            _context = context;
        }

        public IQueryable<TObject> GetAll()
        {
            return _context.Set<TObject>();
        }

        public TObject FindSingle(Expression<Func<TObject, bool>> predicate)
        {
            return _context.Set<TObject>().SingleOrDefault(predicate);
        }

        public IQueryable<TObject> FindMatching(Expression<Func<TObject, bool>> predicate)
        {
            return _context.Set<TObject>().Where(predicate);
        }

        public virtual void Add(TObject entity)
        {
            _context.Set<TObject>().Add(entity);
        }

        public virtual void Delete(TObject entity)
        {
            _context.Set<TObject>().Remove(entity);
        }

        public virtual void Update(TObject entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        
    }
}
