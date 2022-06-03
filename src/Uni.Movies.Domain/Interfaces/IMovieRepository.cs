using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uni.Movies.Domain.Entities;

namespace Uni.Movies.Domain.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {

        Task<IEnumerable<Movie>> FindByName(string name);

    }
}
