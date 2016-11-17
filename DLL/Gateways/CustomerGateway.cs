using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using DLL.Entities;
using DLL.Models;
using Newtonsoft.Json.Linq;
using PublicUsingRest.Models;

namespace DLL.Gateways {
    class CustomerGateway : ICustomerGateway, IGateway<Customer, string> {
        public HttpResponseMessage Register(RegisterViewModel model) {
            using (var client = new HttpClient()) {
                string baseAddress = WebConfigurationManager.AppSettings["RestApiURL"];
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsJsonAsync("api/account/register", model).Result;
                return response;
            }
        }

        public HttpResponseMessage Login(string userName, string password) {
            using (var client = new HttpClient()) {
                string baseAddress = WebConfigurationManager.AppSettings["RestApiURL"];
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //setup login data
                var formContent =
                    new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", userName),
                        new KeyValuePair<string, string>("password", password)
                    });

                //Request token
                var response = client.PostAsync("/token", formContent).Result;

                if (response.IsSuccessStatusCode) {
                    var responseJson = response.Content.ReadAsStringAsync().Result;
                    var jObject = JObject.Parse(responseJson);
                    string token = jObject.GetValue("access_token").ToString();
                    HttpContext.Current.Session["token"] = token;
                    var user = GetUserLoggedIn();
                    if ((user.Roles.FirstOrDefault(x => x.RoleId == "1") != null)) {
                        HttpContext.Current.Session["role"] = "admin";
                    } else {
                        HttpContext.Current.Session["role"] = "user";
                    }
                }

                return response;
            }
        }

        public HttpResponseMessage LogOut() {
            using (var client = new HttpClient()) {
                string baseAddress = WebConfigurationManager.AppSettings["RestApiURL"];
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AddAuthorizationHeader(client);

                HttpContext.Current.Session["token"] = null;
                HttpContext.Current.Session["role"] = null;

                return new HttpResponseMessage();
            }
        }

        public HttpResponseMessage ChangePassword(ChangePasswordViewModel model) {
            using (var client = new HttpClient()) {
                string baseAddress = WebConfigurationManager.AppSettings["RestApiURL"];
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AddAuthorizationHeader(client);

                var response = client.PostAsJsonAsync("api/account/changepassword", model).Result;
                return response;
            }
        }

        public Customer GetUserLoggedIn() {
            using (var client = new HttpClient()) {
                string baseAddress = WebConfigurationManager.AppSettings["RestApiURL"];
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AddAuthorizationHeader(client);

                var response = client.GetAsync("api/account/GetUserLoggedIn").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<Customer>().Result;
                }
                return null;
            }
        }


        public Customer Create(Customer element) {
            throw new NotImplementedException();
        }

        public Customer Read(string id) {
            throw new NotImplementedException();
        }

        public List<Customer> Read() {
            using (var client = new HttpClient()) {
                string baseAddress = WebConfigurationManager.AppSettings["RestApiURL"];
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AddAuthorizationHeader(client);

                var response = client.GetAsync("/api/account/GetAllCustomers").Result;
                if (response.IsSuccessStatusCode) {
                    return response.Content.ReadAsAsync<List<Customer>>().Result;
                }
            }
            return new List<Customer>();
        }

        public Customer Update(Customer element) {
            using (var client = new HttpClient()) {
                string baseAddress = WebConfigurationManager.AppSettings["RestApiURL"];
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AddAuthorizationHeader(client);

                var response = client.PutAsJsonAsync($"api/customer/{element.Id}", element).Result;

                return element;
            }
        }

        public bool Delete(string id) {
            throw new NotImplementedException();
        }

        private void AddAuthorizationHeader(HttpClient client) {
            if (HttpContext.Current.Session["token"] != null) {
                string token = HttpContext.Current.Session["token"].ToString();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }
    }
}