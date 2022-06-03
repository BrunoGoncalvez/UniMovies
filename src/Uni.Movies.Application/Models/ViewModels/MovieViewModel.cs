using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Uni.Movies.Domain.Entities;

namespace Uni.Movies.Application.Models.ViewModels
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string Director { get; set; }
        public string Resume { get; set; }
        public string Image { get; set; }
        public IFormFile ImageUpload { get; set; }
        public Genre Genre { get; set; }

        [Display(Name="Genres")]
        public int GenreId { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

    }
}
