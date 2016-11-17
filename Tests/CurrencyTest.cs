using System;
using DLL;
using DLL.Gateways;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    class CurrencyTest {
        [Test]
        public void TestDKKtoEUR() {
            ICurrencyGateway currencyGateway = new CurrencyGateway();
            double dkk = 100;
            var result = currencyGateway.ConvertDKKToEUR(dkk);
            Assert.IsInstanceOf<double>(result);
            Assert.IsTrue(dkk/7 > result);
            Assert.IsTrue(dkk/8 < result);
        }

        [Test]
        public void TestDKKtoUSD() {
            ICurrencyGateway currencyGateway = new CurrencyGateway();
            double dkk = 100;
            var result = currencyGateway.ConvertDKKToUSD(dkk);
            Assert.IsInstanceOf<double>(result);
            Assert.IsTrue(dkk / 6 > result);
            Assert.IsTrue(dkk / 7 < result);
        }
    }
}