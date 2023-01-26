using Microsoft.EntityFrameworkCore;
using PartTracking.Context.Models.Models;
using PartTracking.Service.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace PartTracking.Service.Service
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly PartMgtContext _context;
        public GenericRepository(PartMgtContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public T AddAndReturn(T entity)
        {
            _context.Set<T>().Add(entity);            
            _context.SaveChanges();
            return entity;
        }    
    }
}
