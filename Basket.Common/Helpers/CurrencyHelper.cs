using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Basket.Common.Helpers
{
    public class CurrencyHelper
    {
        public static async Task<decimal> GetCurrencyRate(string currency)
        {
            if (_cache.ContainsKey(currency) && DateTime.Today == _cache[currency].acquiredAt.Date)
                return _cache[currency].rate;

            string serviceUrl = "http://www.tcmb.gov.tr/kurlar/today.xml";

            var xmlDoc = new XmlDocument();
            await Task.Run(() => xmlDoc.Load(serviceUrl));

            decimal currencyRate = decimal.Parse(xmlDoc.SelectSingleNode($"Tarih_Date/Currency[@Kod='{ currency }']/BanknoteSelling").InnerXml);

            (decimal rate, DateTime acquiredAt) tuple = (currencyRate, DateTime.Today);
            _cache[currency] = tuple;

            return currencyRate;
        }

        // Merkez bankasından bir önceki kapanış verileri alındığından dolayı anlık olarak değişim gösteren bir veri dönmeyecektir
        // Bu nedenle oluşturulan local bir cache mantığı ile harici servise aynı veri için sorgu atılması engellenmiş olmaktadır
        // Aynı gün içerisinde sorgulanan CurrencyRate cache'de tutularak servise bağlanmadan dönüş yapması sağlanmıştır

        private static Dictionary<string, (decimal rate, DateTime acquiredAt)> _cache = new Dictionary<string, (decimal rate, DateTime acquiredAt)>();
    }
}

