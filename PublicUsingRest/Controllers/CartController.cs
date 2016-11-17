using System.Collections.Generic;
using System.Web.Mvc;
using DLL;
using DLL.Entities;

namespace PublicUsingRest.Controllers {
    public class CartController : Controller {
        private IGateway<Movie, int> MovieManager => new DllFacade().GetMovieGateway();
        private ICartGateway CartManager => new DllFacade().GetCartGateway(this.HttpContext);


        public ActionResult Index() {
            var cart = CartManager.Read();
            if (cart == null) {
                cart = new Cart {Movies = new List<Movie>(), PromoCode = new PromoCode()};
            }
            return View(cart);
        }

        public ActionResult AddPromoCode(string promocode) {
            CartManager.AddPromoToCart(promocode);

            return RedirectToAction("Index");
        }


        public ActionResult AddToCart(int id) {
            var movie = MovieManager.Read(id);

            CartManager.AddToCart(movie);

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id) {
            string movieName = MovieManager.Read(id).Title;

            CartManager.RemoveFromCart(id);

            var message = movieName + " has been removed from your shopping cart.";
            return RedirectToAction("Index");
        }

    }
}