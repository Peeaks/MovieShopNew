using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Admin.Views.Movies;
using Admin.Views.PromoCodes;
using DLL;
using DLL.Entities;

namespace PublicUsingRest.Controllers {
    public class AdminController : Controller {
        IGateway<Movie, int> MovieGateway => new DllFacade().GetMovieGateway();
        ICustomerGateway UserGateway => new DllFacade().GetUserGateway();
        IGateway<Genre, int> GenreGateway => new DllFacade().GetGenreGateway();
        IGateway<PromoCode, int> PromoGateway => new DllFacade().GetPromoCodeGateway();
        IOrderGateway OrderGateway => new DllFacade().GetOrderGateway();
        IGateway<Customer, string> CustomerGateway => new DllFacade().GetCustomerGateway();

        // GET: Admin/Movies
        public ActionResult Movies() {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            return View(MovieGateway.Read());
        }

        // GET: Admin/MovieCreate
        public ActionResult MovieCreate() {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            var viewModel = new CreateMovieViewModel { Genres = GenreGateway.Read() };
            return View(viewModel);
        }

        // POST: Admin/MovieCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MovieCreate([Bind(Include = "Id,Title,Year,Price,ImageUrl,TrailerUrl, Genre")] Movie movie) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid) {
                MovieGateway.Create(movie);
                return RedirectToAction("Movies");
            }

            return View(new CreateMovieViewModel {Genres = GenreGateway.Read(), Movie = movie});
        }

        // GET: Admin/MovieEdit/5
        public ActionResult MovieEdit(int? id) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = MovieGateway.Read(id.Value);
            if (movie == null) {
                return HttpNotFound();
            }
            var viewModel = new EditMovieViewModel { Genres = GenreGateway.Read(), Movie = movie };
            return View(viewModel);
        }

        // POST: Admin/MovieEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MovieEdit([Bind(Include = "Id,Title,Year,Price,ImageUrl,TrailerUrl, Genre")] Movie movie) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid) {
                MovieGateway.Update(movie);
                return RedirectToAction("Movies");
            }
            var viewModel = new EditMovieViewModel { Genres = GenreGateway.Read(), Movie = movie };
            return View(viewModel);
        }

        // GET: Admin/MovieDelete/5
        public ActionResult MovieDelete(int? id) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = MovieGateway.Read(id.Value);
            if (movie == null) {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Admin/MovieDelete/5
        [HttpPost, ActionName("MovieDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult MovieDeleteConfirmed(int id) {
            MovieGateway.Delete(id);
            return RedirectToAction("Movies");
        }





        // GET: Admin/Genres
        public ActionResult Genres() {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            return View(GenreGateway.Read());
        }

        // GET: Admin/GenreCreate
        public ActionResult GenreCreate() {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Admin/GenreCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenreCreate([Bind(Include = "Id,Name")] Genre genre) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid) {
                GenreGateway.Create(genre);
                return RedirectToAction("Genres");
            }

            return View(genre);
        }

        // GET: Admin/GenreEdit/5
        public ActionResult GenreEdit(int? id) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = GenreGateway.Read(id.Value);
            if (genre == null) {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: Admin/GenreEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenreEdit([Bind(Include = "Id,Name")] Genre genre) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid) {
                GenreGateway.Update(genre);
                return RedirectToAction("Genres");
            }
            return View(genre);
        }

        // GET: Admin/GenreDelete/5
        public ActionResult GenreDelete(int? id) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Genre genre = GenreGateway.Read(id.Value);
            if (genre == null) {
                return HttpNotFound();
            }
            return View(genre);
        }

        // POST: Admin/GenreDelete/5
        [HttpPost, ActionName("GenreDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult GenreDeleteConfirmed(int id) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            GenreGateway.Delete(id);
            return RedirectToAction("Genres");
        }







        // GET: Admin/Promos
        public ActionResult Promos() {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            return View(PromoGateway.Read());
        }

        // GET: Admin/PromoCreate
        public ActionResult PromoCreate() {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Admin/PromoCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PromoCreate([Bind(Include = "Id,Code,Discount, IsValid")] PromoCode promoCode) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid) {
                PromoGateway.Create(promoCode);
                return RedirectToAction("Promos");
            }

            return View(promoCode);
        }

        // GET: Admin/PromoEdit/5
        public ActionResult PromoEdit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromoCode promoCode = PromoGateway.Read(id.Value);
            if (promoCode == null) {
                return HttpNotFound();
            }
            return View(promoCode);
        }

        // POST: Admin/PromoEdit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PromoEdit([Bind(Include = "Id,Code,Discount, IsValid")] PromoCode promoCode) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid) {
                PromoGateway.Update(promoCode);
                return RedirectToAction("Promos");
            }
            return View(promoCode);
        }

        // GET: Admin/PromoDelete/5
        public ActionResult PromoDelete(int? id) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PromoCode promoCode = PromoGateway.Read(id.Value);
            if (promoCode == null) {
                return HttpNotFound();
            }
            return View(new PromoCodeDeleteViewModel { PromoCode = promoCode, ShitWentWrong = false });
        }

        // POST: Admin/PromoDelete/5
        [HttpPost, ActionName("PromoDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult PromoDeleteConfirmed(int id) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            try {
                PromoGateway.Delete(id);
            } catch (DbUpdateException) {
                return View(new PromoCodeDeleteViewModel { PromoCode = PromoGateway.Read(id), ShitWentWrong = true });
            }
            return RedirectToAction("Promos");
        }






        // GET: Admin/Users
        public ActionResult Users() {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            return View(CustomerGateway.Read());
        }






        // GET: Admin/Orders
        public ActionResult Orders() {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            return View(OrderGateway.Read());
        }

        // GET: Admin/OrderDetails/5
        public ActionResult OrderDetails(int? id) {
            if (HasAccess()) {
                return RedirectToAction("Index", "Home");
            }

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = OrderGateway.Read(id.Value);
            if (order == null) {
                return HttpNotFound();
            }
            return View(order);
        }



        private bool HasAccess() {
            return (UserGateway.GetUserLoggedIn()?.Roles.FirstOrDefault(x => x.RoleId == "1") == null);
        }

    }
}