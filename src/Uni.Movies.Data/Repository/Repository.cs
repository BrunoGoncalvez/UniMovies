using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni.Movies.Data.Context;
using Uni.Movies.Domain.Entities;
using Uni.Movies.Domain.Interfaces;

namespace Uni.Movies.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly UniDbContext _context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(UniDbContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task Add(TEntity entity)
        {
            DbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            DbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var model = await DbSet.FindAsync(id);
            DbSet.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<TEntity> FindById(int id)
        {
            return await DbSet.FindAsync(id);
        }

    }
}
