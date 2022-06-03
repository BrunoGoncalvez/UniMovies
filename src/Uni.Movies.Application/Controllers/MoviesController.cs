using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Uni.Movies.Application.Models.ViewModels;
using Uni.Movies.Data.Context;
using Uni.Movies.Data.Repository;
using Uni.Movies.Domain.Entities;
using Uni.Movies.Domain.Interfaces;

namespace Uni.Movies.Application.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _moviesRepository;
        private readonly IGenreRepository _genreRepository;

        public MoviesController(IMovieRepository moviesRepository, IGenreRepository genreRepository)
        {
            _moviesRepository = moviesRepository;
            _genreRepository = genreRepository;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            return View(await _moviesRepository.FindAll());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _moviesRepository.FindById(id.Value);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        public async Task<IActionResult> Create()
        {
            var movieViewModel = await PopulateGenres(new MovieViewModel());
            return View(movieViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel movieViewModel)
        {
            if (!ModelState.IsValid) return View(movieViewModel);

            var imgPrefixo = Guid.NewGuid() + "_";

            if (!await UploadFile(movieViewModel.ImageUpload, imgPrefixo))
                return View(movieViewModel);

            var movie = new Movie
            {
                Name = movieViewModel.Name,
                Image = imgPrefixo + movieViewModel.ImageUpload.FileName,
                Resume = movieViewModel.Resume,
                Director = movieViewModel.Director,
                GenreId = movieViewModel.GenreId,
                Genre = movieViewModel.Genre,
                Year = movieViewModel.Year
            };

            await _moviesRepository.Add(movie);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _moviesRepository.FindById(id.Value);

            MovieViewModel movieViewModel = new MovieViewModel
            {
                Id = movie.Id,
                Name = movie.Name,
                Resume = movie.Resume,
                Director = movie.Director,
                Year = movie.Year,
                GenreId = movie.GenreId,
                Image = movie.Image,
                Genres = await _genreRepository.FindAll()
            };
            if (movie == null) return NotFound();

            return View(movieViewModel);
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieViewModel movieViewModel)
        {
            if (!ModelState.IsValid) return View(movieViewModel);
            if (id != movieViewModel.Id) return NotFound();
            var movie = await _moviesRepository.FindById(id);

            if(movieViewModel.ImageUpload != null)
            {
                var imgName = Guid.NewGuid() + "_" + movieViewModel.Image;
                movie.Image = imgName;
            }

            movie.Name = movieViewModel.Name;
            movie.Resume = movieViewModel.Resume;
            movie.Director = movieViewModel.Director;
            movie.Year = movieViewModel.Year;
            movie.GenreId = movieViewModel.GenreId;

            await _moviesRepository.Update(movie);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var movie = await _context.Movies
            //    .Include(m => m.Genre)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (movie == null)
            //{
            //    return NotFound();
            //}

            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var movie = await _context.Movies.FindAsync(id);
            //_context.Movies.Remove(movie);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private async Task<MovieViewModel> PopulateGenres(MovieViewModel movie)
        {
            movie.Genres = await _genreRepository.FindAll();
            return movie;
        }

        private async Task<bool> UploadFile(IFormFile file, string prefixo)
        {
            if (file.Length <= 0) return false;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", prefixo + file.FileName);
            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "A file with this name already exists.");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return true;

        }

    }
}
