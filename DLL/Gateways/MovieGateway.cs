using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DLL.Entities;

namespace DLL.Gateways {
    class MovieGateway : IGateway<Movie, int> {
        private const string URL = "http://movieshoprest.azurewebsites.net/";

        public Movie Create(Movie element) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync("api/movies", element).Result;
                return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<Movie>().Result : null;
            }
        }

        public Movie Read(int id) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"api/movies/{id}").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<Movie>().Result;
                }
                return null;
            }
        }

        public List<Movie> Read() {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("/api/movies").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<List<Movie>>().Result;
                }
            }
            return new List<Movie>();
        }

        public Movie Update(Movie element) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PutAsJsonAsync($"/api/movies/{element.Id}", element).Result;

                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<Movie>().Result;
                }
                return null;
            }
        }

        public bool Delete(int id) {
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.DeleteAsync($"/api/movies/{id}").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<Movie>().Result != null;
                }
                return false;
            }
        }
    }
}
