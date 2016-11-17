using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DLL;
using DLL.Entities;
using Microsoft.AspNet.Identity;
using PublicUsingRest.Models;

namespace PublicUsingRest.Controllers {
    public class ManageController : Controller {
        private readonly IGateway<Customer, string> _customerGateway = new DllFacade().GetCustomerGateway();
        private readonly IOrderGateway _orderGateway = new DllFacade().GetOrderGateway();
        private readonly ICustomerGateway _userGateway = new DllFacade().GetUserGateway();

        //
        // GET: /Manage/Index
        public ActionResult Index(string message) {
            var userLoggedIn = _userGateway.GetUserLoggedIn();
            if (userLoggedIn != null) {
                //Found orders for a user
                var orders = _orderGateway.ReadOrdersFromUser();
                return View(new IndexViewModel {Customer = userLoggedIn, Message = message, Orders = orders});
            } else {
                return RedirectToAction("Login", "Account");
            }
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword() {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var result = _userGateway.ChangePassword(model);
            if (result.IsSuccessStatusCode) {
                var user = _userGateway.GetUserLoggedIn();
                if (user != null) {
                    _userGateway.Login(user.Email, model.NewPassword);
                }
                return RedirectToAction("Index", new {Message = "Succesfully changed password"});
            }
            return View(model);
        }

        public ActionResult ChangeName() {
            var userFound = _userGateway.GetUserLoggedIn();

            return View(new ChangeNameViewModel {FirstName = userFound.FirstName, LastName = userFound.LastName});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeName(ChangeNameViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var userFound = _userGateway.GetUserLoggedIn();
            userFound.FirstName = model.FirstName;
            userFound.LastName = model.LastName;
            _customerGateway.Update(userFound);

            return RedirectToAction("Index", new {Message = "Succesfully changed name"});
        }

        public ActionResult ChangeAddress() {
            var userFound = _userGateway.GetUserLoggedIn();

            return View(userFound.Address);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAddress(Address address) {
            if (!ModelState.IsValid) {
                return View(address);
            }
            var userFound = _userGateway.GetUserLoggedIn();
            userFound.Address = address;
            _customerGateway.Update(userFound);

            return RedirectToAction("Index", new {Message = "Succesfully changed address"});
        }
    }
}