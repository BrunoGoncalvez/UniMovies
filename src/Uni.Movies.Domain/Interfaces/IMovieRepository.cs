using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uni.Movies.Domain.Entities;

namespace Uni.Movies.Domain.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {

        Task<Movie> GetMovieWithGenre(int id);
        Task<IEnumerable<Movie>> GetMoviesWithGenre();
        Task<IEnumerable<Movie>> SearchMovie(string searchMovie);

    }
}
