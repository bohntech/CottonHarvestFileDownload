/****************************************************************************
The MIT License(MIT)

Copyright(c) 2016 Bohn Technology Solutions, LLC

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using CottonHarvestDataTransferApp.Data;

namespace CottonHarvestDataTransferApp.EF
{
    /// <summary>
    /// Base class implementation of generic repository using
    /// Entity Framework and MS SQL Local Database for data 
    /// storage.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public class GenericRepository<TObject> where TObject: class
    {
        protected AppDBContext _context = null;

        public GenericRepository(AppDBContext context) {
            _context = context;
        }

        /// <summary>
        /// Gets all entities of type 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TObject> GetAll()
        {
            return _context.Set<TObject>().ToList();
        }

        /// <summary>
        /// Finds a single entity matching predict.  If no match returns null.
        /// If multiple matches an exception is thrown.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TObject FindSingle(Expression<Func<TObject, bool>> predicate)
        {
            return _context.Set<TObject>().SingleOrDefault(predicate);
        }

        /// <summary>
        /// Returns all entities that match predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TObject> FindMatching(Expression<Func<TObject, bool>> predicate)
        {
            return _context.Set<TObject>().Where(predicate);
        }

        /// <summary>
        /// Adds an entity to the context to be inserted on Save
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(TObject entity)
        {
            _context.Set<TObject>().Add(entity);
        }

        /// <summary>
        /// Marks an entity for removal on next save
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TObject entity)
        {
            _context.Set<TObject>().Remove(entity);
        }

        /// <summary>
        /// Marks an entity for update on next save
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TObject entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        
    }
}
