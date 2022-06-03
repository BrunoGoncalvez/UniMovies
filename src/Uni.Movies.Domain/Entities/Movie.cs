using System;
using System.Collections.Generic;
using System.Text;

namespace Uni.Movies.Domain.Entities
{
    public class Movie : Entity
    {

        public string Name { get; set; }
        public string Year { get; set; }
        public string Director { get; set; }
        public string Resume { get; set; }
        public string Image { get; set; }
        public Genre Genre { get; set; }
        public int GenreId { get; set; }

    }
}
