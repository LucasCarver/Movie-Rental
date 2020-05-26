using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRegistration.Models;


namespace MovieRegistration.Controllers
{
    public class MovieController : Controller
    {
        List<Movie> movieList = new List<Movie>();
        List<Movie> cartList = new List<Movie>();

        public IActionResult Index()
        {
            return View();
        }

        public void InitializeList()
        {
            movieList.Add(new Movie(1, "Paul Blart: Mall Cop 2", "Comedy", DateTime.Parse("04/17/2015"), "Kevin James", "Andy Fickman"));
            movieList.Add(new Movie(2, "Sonic the Hedgehog", "Animation", DateTime.Parse("02/14/2020"), "James Marsden", "Jeff Fowler"));
            movieList.Add(new Movie(3, "The Matrix Reloaded", "Sci-Fi", DateTime.Parse("05/16/2003"), "Keanu Reeves", "Lana Wachowski, Lilly Wachowski"));
            movieList.Add(new Movie(4, "The Matrix", "Sci-Fi", DateTime.Parse("03/31/1999"), "Keanu Reeves", "Lana Wachowski, Lilly Wachowski"));
            movieList.Add(new Movie(5, "The Matrix Revolutions", "Sci-Fi", DateTime.Parse("11/05/2003"), "Keanu Reeves", "Lana Wachowski, Lilly Wachowski"));
        }

        public IActionResult MovieRegistration()
        {
            return View();
        }

        public IActionResult MovieRegister(Movie m)
        {
            string movieString = JsonSerializer.Serialize(m);
            HttpContext.Session.SetString("MovieSession", movieString);
            return View(m);
        }

        public IActionResult AddMovieToList()
        {
            Movie movie = JsonSerializer.Deserialize<Movie>(HttpContext.Session.GetString("MovieSession"));
            string movieListString = HttpContext.Session.GetString("MovieList");

            if (movieListString == null)
            {
                InitializeList();
                movieListString = JsonSerializer.Serialize(movieList);
                HttpContext.Session.SetString("MovieList", movieListString);
            }

            if (movieListString != null)
            {
                movieList = JsonSerializer.Deserialize<List<Movie>>(movieListString);
            }

            movieList.Add(movie);
            movieListString = JsonSerializer.Serialize(movieList);
            HttpContext.Session.SetString("MovieList", movieListString);

            return RedirectToAction("ListMovies");
        }

        public IActionResult ListMovies()
        {
            string movieListString = HttpContext.Session.GetString("MovieList");
            if (movieListString != null)
            {
                movieList = JsonSerializer.Deserialize<List<Movie>>(HttpContext.Session.GetString("MovieList"));
            }
            return View(movieList);
        }

        public IActionResult SelectMovie(string titleKey)
        {
            string movieListString = HttpContext.Session.GetString("MovieList");
            if (movieListString != null)
            {
                movieList = JsonSerializer.Deserialize<List<Movie>>(movieListString);
            }
            Movie foundMovie = movieList.Where(x => x.MovieTitle == titleKey).First();

            string cartListString = HttpContext.Session.GetString("CartList");
            if (cartListString != null)
            {
                cartList = JsonSerializer.Deserialize<List<Movie>>(cartListString);
            }
            foreach (Movie m in cartList)
            {
                if (foundMovie.ID == m.ID)
                {
                    return RedirectToAction("ErrorInCart", foundMovie);
                }
            }
            cartList.Add(foundMovie);
            cartListString = JsonSerializer.Serialize(cartList);
            HttpContext.Session.SetString("CartList", cartListString);

            return View(foundMovie);
        }

        public IActionResult ErrorInCart(Movie m)
        {
            return View(m);
        }

        public IActionResult MovieRental()
        {
            string movieListString = HttpContext.Session.GetString("MovieList");
            if (movieListString == null)
            {
                InitializeList();
                movieListString = JsonSerializer.Serialize(movieList);
                HttpContext.Session.SetString("MovieList", movieListString);
            }
            return RedirectToAction("ListMovies");
        }

        public IActionResult RentalCart()
        {
            string cartListString = HttpContext.Session.GetString("CartList");
            if (cartListString != null)
            {
                cartList = JsonSerializer.Deserialize<List<Movie>>(cartListString);
            }
            return View(cartList);
        }

        public IActionResult RemoveMovie(string titleKey)
        {

            string cartListString = HttpContext.Session.GetString("CartList");
            if (cartListString != null)
            {
                cartList = JsonSerializer.Deserialize<List<Movie>>(cartListString);
            }

            Movie foundMovie = cartList.Where(x => x.MovieTitle == titleKey).First();
            cartList.Remove(foundMovie);

            cartListString = JsonSerializer.Serialize(cartList);
            HttpContext.Session.SetString("CartList", cartListString);

            return View(foundMovie);
        }

        public IActionResult Receipt()
        {
            string cartListString = HttpContext.Session.GetString("CartList");
            if (cartListString != null)
            {
                cartList = JsonSerializer.Deserialize<List<Movie>>(cartListString);
            }
            return View(cartList);
        }
    }
}