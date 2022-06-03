using System;
using System.Collections.Generic;
using System.Text;
using Uni.Movies.Data.Context;
using Uni.Movies.Domain.Entities;
using Uni.Movies.Domain.Interfaces;

namespace Uni.Movies.Data.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {


        public GenreRepository(UniDbContext context) : base(context)
        {
        }


    }
}
