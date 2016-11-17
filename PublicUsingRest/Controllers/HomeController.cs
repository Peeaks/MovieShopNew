using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AuthTest.Models;
using DLL;
using DLL.Entities;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using PublicUsingRest.Models;

namespace PublicUsingRest.Controllers {
    public class HomeController : Controller {
        private IGateway<Movie, int> MovieManager => new DllFacade().GetMovieGateway();
        private IOrderGateway OrderManager => new DllFacade().GetOrderGateway();
        private IGateway<Genre, int> GenreManager => new DllFacade().GetGenreGateway();
        private ICustomerGateway CustomerManager => new DllFacade().GetUserGateway();
        private ICartGateway CartManager => new DllFacade().GetCartGateway(this.HttpContext);

        // GET: Movie
        public ActionResult Index(int? page) {
            var allMovies = MovieManager.Read();

            if (page == null) {
                page = 1;
            }

            return View(GetPage(allMovies, page.Value));
        }

        public ActionResult Search(int? page, string search) {
            if (search.IsNullOrWhiteSpace()) {
                return RedirectToAction("Index");
            }

            var allMovies = MovieManager.Read();

            if (page == null) {
                page = 1;
            }

            var returnMovies = new List<Movie>();

            foreach (var movie in allMovies) {
                if (movie.Title.ToLower().Contains(search.ToLower())) {
                    returnMovies.Add(movie);
                }
            }

            if (!returnMovies.Any()) {
                return View(new HomeSearchViewModel {Search = search, ErrorMessage = "No results were found"});
            }

            return
                View(new HomeSearchViewModel {HomeIndexViewModel = GetPage(returnMovies, page.Value), Search = search});
        }

        public ActionResult GenreSearch(int? page, int? genreSearch) {
            if (genreSearch == null) {
                return RedirectToAction("Index");
            }

            var allMovies = MovieManager.Read();
            var genreFound = GenreManager.Read(genreSearch.Value);

            if (genreFound == null) {
                return RedirectToAction("Index");
            }

            if (page == null) {
                page = 1;
            }

            var returnMovies = new List<Movie>();

            foreach (var movie in allMovies) {
                if (movie.Genre.Id == genreFound.Id) {
                    returnMovies.Add(movie);
                }
            }

            return
                View(new HomeGenreSearchViewModel {
                    Genre = genreFound,
                    HomeIndexViewModel = GetPage(returnMovies, page.Value)
                });
        }

        private HomeIndexViewModel GetPage(List<Movie> movies, int page) {
            var moviesPerPage = 12;
            var endIndex = page*moviesPerPage;

            var returnMovies = new List<Movie>();

            for (int i = endIndex - moviesPerPage; i < endIndex; i++) {
                if (i < movies.Count) {
                    returnMovies.Add(movies[i]);
                }
            }
            return new HomeIndexViewModel {
                Movies = returnMovies,
                MaxPages = (movies.Count + 9)/12,
                CurrentPage = page,
                Genres = GenreManager.Read()
            };
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = MovieManager.Read(id.Value);
            if (movie == null) {
                return HttpNotFound();
            }
            return View(movie);
        }

        public ActionResult BuyPage() {
            if (System.Web.HttpContext.Current.Session["token"] == null) {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid) {
                Order order;

                var cart = CartManager.Read();

                if (cart.PromoCode == null) {
                    order = new Order {Customer = CustomerManager.GetUserLoggedIn(), Movies = cart.Movies};
                } else {
                    order = new Order {
                        Customer = CustomerManager.GetUserLoggedIn(),
                        Movies = cart.Movies,
                        PromoCode = cart.PromoCode
                    };
                }
                return View("Confirm", order);
            }
            return RedirectToAction("Index", "Cart");
        }

        [ValidateAntiForgeryToken]
        public ActionResult Confirm([Bind(Include = "Id, Movies, PromoCode, Customer")] Order order) {
            if (System.Web.HttpContext.Current.Session["token"] == null) {
                return RedirectToAction("Login", "Account");
            }

            CartManager.EmptyCart();

            order = OrderManager.Create(order);

            //const string subject = "Order receipt from Movie Shop";
            //var body = new EmailTemplate.EmailTemplate().Receipt(order);
            //await UserManager.SendEmailAsync(User.Identity.GetUserId(), subject, body);

            return View("ThankYou", order);
        }

        public ActionResult Currency(Currency currency) {
            System.Web.HttpContext.Current.Session["currency"] = currency;

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }


    }
}