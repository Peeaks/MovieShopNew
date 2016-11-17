using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DLL.Entities;

namespace DLL.Gateways {
    class CartGateway : ICartGateway {
        private const string URL = "http://movieshoprest.azurewebsites.net/";

        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public CartGateway(HttpContextBase httpContext) {
            ShoppingCartId = GetCartId(httpContext);
        }

        public Cart Read() {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"api/carts/{ShoppingCartId}").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<Cart>().Result;
                }
                return null;
            }
        }

        public void AddPromoToCart(string code) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var cart = Read();
                if (cart == null) {
                    cart = new Cart {
                        Id = ShoppingCartId,
                        Movies = new List<Movie>(),
                        PromoCode = new PromoCode {Code = code}
                    };
                    var response = client.PostAsJsonAsync("api/carts", cart).Result;
                } else {
                    var response =
                        client.PostAsJsonAsync($"api/carts/AddPromoToCart?id={ShoppingCartId}&code={code}", code).Result;
                }
            }
        }

        public void AddToCart(Movie movie) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var cart = Read();
                if (cart == null) {
                    cart = new Cart {
                        Id = ShoppingCartId,
                        Movies = new List<Movie>(),
                        PromoCode = new PromoCode {Code = "1234"}
                    };
                    cart.Movies.Add(movie);
                    var response = client.PostAsJsonAsync("api/carts", cart).Result;
                } else {
                    var response = client.PostAsJsonAsync($"api/carts/AddToCart?id={ShoppingCartId}", movie).Result;
                }
            }
        }

        public void RemoveFromCart(int id) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response =
                    client.PostAsJsonAsync($"api/carts/RemoveFromCart?id={ShoppingCartId}&movieId={id}", id).Result;
            }
        }

        public void EmptyCart() {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync($"api/carts/ClearCart?id={ShoppingCartId}", new Movie()).Result;
            }
        }

        private string GetCartId(HttpContextBase context) {
            if (context.Session[CartSessionKey] == null) {
                // Generate a new random GUID using System.Guid class
                Guid tempCartId = Guid.NewGuid();
                // Send tempCartId back to client as a cookie
                context.Session[CartSessionKey] = tempCartId.ToString();
            }
            return context.Session[CartSessionKey].ToString();
        }
    }
}