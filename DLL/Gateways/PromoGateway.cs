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
    class PromoGateway : IGateway<PromoCode, int> {
        private const string URL = "http://movieshoprest.azurewebsites.net/";

        public PromoCode Create(PromoCode element) {
            using (var client = new HttpClient()) {
                SetupClient(client);
                AddAuthorizationHeader(client);

                HttpResponseMessage response = client.PostAsJsonAsync("api/promocodes", element).Result;
                return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<PromoCode>().Result : null;
            }
        }

        public PromoCode Read(int id) {
            using (var client = new HttpClient()) {
                SetupClient(client);
                AddAuthorizationHeader(client);

                HttpResponseMessage response = client.GetAsync($"api/promocodes/{id}").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<PromoCode>().Result;
                }
                return null;
            }
        }

        public List<PromoCode> Read() {
            using (var client = new HttpClient()) {
                SetupClient(client);
                AddAuthorizationHeader(client);

                var response = client.GetAsync("api/promocodes").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<List<PromoCode>>().Result;
                }
            }
            return new List<PromoCode>();
        }

        public PromoCode Update(PromoCode element) {
            using (var client = new HttpClient()) {
                SetupClient(client);
                AddAuthorizationHeader(client);

                var response = client.PutAsJsonAsync($"api/promocodes/{element.Id}", element).Result;

                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<PromoCode>().Result;
                }
                return null;
            }
        }

        public bool Delete(int id) {
            using (var client = new HttpClient()) {
                SetupClient(client);
                AddAuthorizationHeader(client);

                var response = client.DeleteAsync($"api/promocodes/{id}").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<PromoCode>().Result != null;
                }
                return false;
            }
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
