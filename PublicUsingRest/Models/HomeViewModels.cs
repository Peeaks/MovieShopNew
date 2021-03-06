﻿using System.Collections.Generic;
using DLL.Entities;

namespace PublicUsingRest.Models {
    public class HomeIndexViewModel {
        public List<Movie> Movies { get; set; }
        public int MaxPages { get; set; }
        public int CurrentPage { get; set; }
        public List<Genre> Genres { get; set; }
    }

    public class HomeSearchViewModel {
        public HomeIndexViewModel HomeIndexViewModel { get; set; }
        public string Search { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class HomeGenreSearchViewModel {
        public HomeIndexViewModel HomeIndexViewModel { get; set; }
        public Genre Genre { get; set; }
    }

}