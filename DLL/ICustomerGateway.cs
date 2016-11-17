using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DLL.Entities;
using DLL.Models;
using PublicUsingRest.Models;

namespace DLL {
    public interface ICustomerGateway {
        HttpResponseMessage Register(RegisterViewModel model);
        HttpResponseMessage Login(string userName, string password);
        HttpResponseMessage LogOut();
        HttpResponseMessage ChangePassword(ChangePasswordViewModel model);
        Customer GetUserLoggedIn();
    }
}
