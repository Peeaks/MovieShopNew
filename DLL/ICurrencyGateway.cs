using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL {
    public interface ICurrencyGateway {
        double ConvertDKKToEUR(double amount);
        double ConvertDKKToUSD(double amount);
    }
}
