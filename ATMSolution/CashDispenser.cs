using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMSolution
{
    /// <summary>
    /// Contains all the logic for monitoring and dispensing cash
    /// </summary>
    internal class CashDispenser
    {
        private const int DEFAULT_CURRENCY_STOCK = 10;
        private readonly int _maxCurrencyStock;
        private Dictionary<string, CurrencyDrawer> _currencyDrawers;

        public CashDispenser()
        {
            _maxCurrencyStock = DEFAULT_CURRENCY_STOCK;
            InitializeCurrencyDrawers(_maxCurrencyStock);
        }

        private void InitializeCurrencyDrawers(int maxCurrencyStock)
        {
            _currencyDrawers = new Dictionary<string, CurrencyDrawer>
            {
                { "$100", new(100, maxCurrencyStock) },
                { "$50", new(50, maxCurrencyStock) },
                { "$20", new(20, maxCurrencyStock) },
                { "$10", new(10, maxCurrencyStock) },
                { "$5", new(5, maxCurrencyStock) },
                { "$1", new(1, maxCurrencyStock) }
            };
        }

        internal List<string> GetDenomonationInventory(string keys)
        {
            string[] denomonations = keys.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> denomonationInventory = new List<string>();
            foreach(string value in denomonations)
            {
                denomonationInventory.Add(GetDenomonationInfo(value));
            }

            return denomonationInventory;
        }

        private string GetDenomonationInfo(string key)
        {
            string denomonationInfo;

            if (_currencyDrawers.ContainsKey(key))
            {
                denomonationInfo = _currencyDrawers.Where(d => d.Key == key).Select(v => v.Value).Single().ToString();
            }
            else
            {
                denomonationInfo = string.Empty;
            }

            return denomonationInfo;
        }
    }
}
