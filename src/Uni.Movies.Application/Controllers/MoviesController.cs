using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uni.Movies.Application.Models.ViewModels;
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

        public async Task<IActionResult> Index()
        {
            return View(await _moviesRepository.GetMoviesWithGenre());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _moviesRepository.GetMovieWithGenre(id.Value);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieViewModel movieViewModel)
        {
            if (!ModelState.IsValid) return View(movieViewModel);
            if (id != movieViewModel.Id) return NotFound();
            var movie = await _moviesRepository.FindById(id);

            movieViewModel.Image = movie.Image;

            if(movieViewModel.ImageUpload != null)
            {
                var imgPrefix = Guid.NewGuid() + "_" + movieViewModel.Image;
                if (!await UploadFile(movieViewModel.ImageUpload, imgPrefix))
                {
                    return View(movieViewModel);
                }
                movie.Image = imgPrefix + movieViewModel.ImageUpload.FileName;
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

            if (id == null) return NotFound();
            var movie = await _moviesRepository.GetMovieWithGenre(id.Value);
            if (movie == null) return NotFound();
            
            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _moviesRepository.FindById(id);
            if (movie == null) return NotFound();
            await _moviesRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }




        // UTILS
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
