using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRegistration.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string MovieTitle { get; set; }
        public string Genre { get; set; }
        public DateTime Year { get; set; }
        public string Actors { get; set; }
        public string Directors { get; set; }
        public Movie()
        {

        }
        public Movie(int ID, string MovieTitle, string Genre, DateTime Year, string Actors, string Directors)
        {
            this.ID = ID;
            this.MovieTitle = MovieTitle;
            this.Genre = Genre;
            this.Year = Year;
            this.Actors = Actors;
            this.Directors = Directors;
        }
    }
}
