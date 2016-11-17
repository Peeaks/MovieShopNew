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
    class OrderGateway : IOrderGateway {
        private const string URL = "http://movieshoprest.azurewebsites.net/";

        public Order Create(Order element) {
            using (var client = new HttpClient()) {
                SetupClient(client);
                AddAuthorizationHeader(client);

                HttpResponseMessage response = client.PostAsJsonAsync("api/orders", element).Result;
                return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<Order>().Result : null;
            }
        }

        public Order Read(int id) {
            using (var client = new HttpClient()) {
                SetupClient(client);
                AddAuthorizationHeader(client);

                HttpResponseMessage response = client.GetAsync($"api/orders/{id}").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<Order>().Result;
                }
                return null;
            }
        }  

        public List<Order> Read() {
            using (var client = new HttpClient()) {
                SetupClient(client);
                AddAuthorizationHeader(client);

                var response = client.GetAsync("/api/orders").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<List<Order>>().Result;
                }
            }
            return new List<Order>();
        }

        public List<Order> ReadOrdersFromUser() {
            using (var client = new HttpClient()) {
                SetupClient(client);
                AddAuthorizationHeader(client);

                var response = client.GetAsync("/api/orders/GetOrdersByUser").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<List<Order>>().Result;
                }
            }
            return new List<Order>();
        }

        public Order Update(Order element) {
            throw new NotImplementedException();
        }

        public bool Delete(int id) {
            throw new NotImplementedException();
        }

        private void SetupClient(HttpClient client) {
            client.BaseAddress = new Uri(URL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void AddAuthorizationHeader(HttpClient client) {
            if (HttpContext.Current.Session["token"] != null) {
                string token = HttpContext.Current.Session["token"].ToString();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }

    }
}
