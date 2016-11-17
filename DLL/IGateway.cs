using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DLL {
    public interface IGateway<T, R> {
        T Create(T element);
        T Read(R id);
        List<T> Read();
        T Update(T element);
        bool Delete(R id);
    }
}
