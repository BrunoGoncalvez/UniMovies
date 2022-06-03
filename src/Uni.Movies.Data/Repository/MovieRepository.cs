using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Uni.Movies.Data.Context;
using Uni.Movies.Domain.Entities;
using Uni.Movies.Domain.Interfaces;

namespace Uni.Movies.Data.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {

        public MovieRepository(UniDbContext context) : base(context)
        {
        }

        public Task<IEnumerable<Movie>> FindByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
