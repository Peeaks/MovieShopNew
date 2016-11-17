using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLL.Entities;

namespace DLL {
    public interface IOrderGateway {
        Order Create(Order element);
        Order Read(int id);
        List<Order> Read();
        List<Order> ReadOrdersFromUser();
        Order Update(Order element);
        bool Delete(int id);
    }
}