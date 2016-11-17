using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DLL.Entities;
using Newtonsoft.Json.Linq;

namespace DLL.Gateways {
    public class CurrencyGateway : ICurrencyGateway {
        private const string URL = "http://api.fixer.io/";

        public double ConvertDKKToEUR(double amount) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("latest?base=DKK&symbols=EUR").Result;
                if (response.IsSuccessStatusCode) {
                    var responseJson = response.Content.ReadAsStringAsync().Result;
                    var jObject = JObject.Parse(responseJson);
                    double rate = jObject.SelectToken(@"rates.EUR").Value<double>();
                    return amount * rate;
                }

                throw new HttpException("Rate not found");
            }
        }

        public double ConvertDKKToUSD(double amount) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("latest?base=DKK&symbols=USD").Result;
                if (response.IsSuccessStatusCode) {
                    var responseJson = response.Content.ReadAsStringAsync().Result;
                    var jObject = JObject.Parse(responseJson);
                    double rate = jObject.SelectToken(@"rates.USD").Value<double>();
                    return amount * rate;
                }

                throw new HttpException("Rate not found");
            }
        }
    }
}
