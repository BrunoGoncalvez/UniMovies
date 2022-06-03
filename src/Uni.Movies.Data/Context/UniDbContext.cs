using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Uni.Movies.Domain.Entities;

namespace Uni.Movies.Data.Context
{
    public class UniDbContext : DbContext
    {

        public UniDbContext(DbContextOptions<UniDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

    }
}
