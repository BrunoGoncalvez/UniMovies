using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Uni.Movies.Domain.Entities;

namespace Uni.Movies.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {

        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(int id);

        Task<IEnumerable<TEntity>> FindAll();

        Task<TEntity> FindById(int id);


    }
}
