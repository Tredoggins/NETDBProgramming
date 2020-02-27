using System;
using System.Collections.Generic;

namespace MovieLibrary
{
    public class Movie
    {
        public UInt64 movieId { get; set; }
        public string title { get; set; }
        public List<string> genres { get; set; }


        // constructor
        public Movie()
        {
            genres = new List<string>();
        }
        public string Display()
        {
            return $"Id: {movieId}\nTitle: {title}\nGenres: {string.Join(", ", genres)}\n";
        }
    }
}
