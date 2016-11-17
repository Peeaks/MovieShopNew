using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using DLL;
using DLL.Models;
using PublicUsingRest.Models;
using RegisterViewModel = DLL.Models.RegisterViewModel;

namespace PublicUsingRest.Controllers {
    //[Authorize]
    public class AccountController : Controller {
        private ICustomerGateway _customerGateway => new DllFacade().GetUserGateway();

        //
        // GET: /Account/Login
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl) {
            if (ModelState.IsValid) {
                HttpResponseMessage response = _customerGateway.Login(model.Email, model.Password);
                if (response.StatusCode == HttpStatusCode.OK) {
                    if (Url.IsLocalUrl(returnUrl)) {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                } else
                    ModelState.AddModelError("", "Invalid login attempt!");
            }

            return View(model);
        }

        //
        // GET: /Account/Register
        public ActionResult Register() {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model) {
            if (ModelState.IsValid) {
                HttpResponseMessage response = _customerGateway.Register(model);

                if (response.IsSuccessStatusCode) {
                    _customerGateway.Login(model.Email, model.Password);
                    return RedirectToAction("Index", "Home");
                } else
                    ModelState.AddModelError("", response.Content.ReadAsStringAsync().Result);
            }

            return View(model);
        }

        public ActionResult Logout() {
            if (ModelState.IsValid) {
                HttpResponseMessage response = _customerGateway.LogOut();
            }
            return RedirectToAction("Index", "Home");
        }

    }
}