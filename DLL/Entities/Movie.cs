using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.Gateways;

namespace DLL.Entities {
    public class Movie : AbstractEntity {
        [Required]
        public string Title { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public double Price { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Trailer URL")]
        public string TrailerUrl { get; set; }

        public Genre Genre { get; set; }

        public string GetPriceCurrency(string currency) {
            ICurrencyGateway currencyGateway = new CurrencyGateway();
            if (currency == null) {
                return Price + "kr";
            }
            if (currency.Equals("EUR")) {
                return currencyGateway.ConvertDKKToEUR(Price) + "€";
            } else if (currency.Equals("USD")) {
                return currencyGateway.ConvertDKKToUSD(Price) + "$";
            }

            return Price + "kr";
        }
    }
}