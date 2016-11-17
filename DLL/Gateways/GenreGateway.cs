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
    class GenreGateway : IGateway<Genre, int> {
        private const string URL = "http://movieshoprest.azurewebsites.net/";

        public Genre Create(Genre element) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AddAuthorizationHeader(client);

                HttpResponseMessage response = client.PostAsJsonAsync("api/genres", element).Result;
                return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<Genre>().Result : null;
            }
        }

        public Genre Read(int id) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"api/genres/{id}").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<Genre>().Result;
                }
                return null;
            }
        }

        public List<Genre> Read() {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("/api/genres").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<List<Genre>>().Result;
                }
            }
            return new List<Genre>();
        }

        public Genre Update(Genre element) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AddAuthorizationHeader(client);

                var response = client.PutAsJsonAsync($"/api/genres/{element.Id}", element).Result;

                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<Genre>().Result;
                }
                return null;
            }
        }

        public bool Delete(int id) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AddAuthorizationHeader(client);

                var response = client.DeleteAsync($"/api/genres/{id}").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<Genre>().Result != null;
                }
                return false;
            }
        }

        private void AddAuthorizationHeader(HttpClient client) {
            if (HttpContext.Current.Session["token"] != null) {
                string token = HttpContext.Current.Session["token"].ToString();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }

    }
}