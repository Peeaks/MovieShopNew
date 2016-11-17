using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DLL.Entities;
using DLL.Gateways;

//using DLL.Managers;

namespace DLL {
    public class DllFacade {
        public IGateway<Movie, int> GetMovieGateway() {
            return new MovieGateway();
        }
        public IGateway<Genre, int> GetGenreGateway() {
            return new GenreGateway();
        }
        public IOrderGateway GetOrderGateway() {
            return new OrderGateway();
        }
        public IGateway<PromoCode, int> GetPromoCodeGateway() {
            return new PromoGateway();
        }
        public ICustomerGateway GetUserGateway() {
            return new CustomerGateway();
        }

        public IGateway<Customer, String> GetCustomerGateway() {
            return new CustomerGateway();
        }


        public ICartGateway GetCartGateway(HttpContextBase httpContext) {
            return new CartGateway(httpContext);
        }
    }
}