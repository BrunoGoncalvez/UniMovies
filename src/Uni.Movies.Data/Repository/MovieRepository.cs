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
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {

        public MovieRepository(UniDbContext context) : base(context)
        {
        }

        public async Task<Movie> GetMovieWithGenre(int id)
        {
            return await _context.Movies.AsNoTracking().Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id) ;
        }

        public async Task<IEnumerable<Movie>> GetMoviesWithGenre()
        {
            return await _context.Movies.AsNoTracking().Include(m => m.Genre).OrderBy(m => m.Name).ToListAsync();
        }


    }
}
