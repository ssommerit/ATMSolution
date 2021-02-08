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

        #region Withdrawal Logic
        // method for main withdrawl logic
        // returns a tuple(bool, string) indicating if the withdrawal was successful and a corresponding message
        internal (bool, string) WithdrawalFunds(string requestedAmount)
        {
            string message;
            (bool canWithdraw, List<KeyValuePair<string, CurrencyDrawer>> dispenserDrawers) = TryWithdrawal(requestedAmount);

            if (canWithdraw)
            {
                ExecuteWithdrawal(dispenserDrawers);
                message = $"Success: Dispensed {requestedAmount}";
            }
            else
            {
                message = "Failure: insufficient funds";
            }

            return (canWithdraw, message);
        }

        // Checks to see if an withdrawl can be executed
        // returns tuple of (bool, List<CurrencyDrawer>) indicating if a withdrawl can be completed with the current currency stock levels
        // and the required quantity of each denomonation of currency
        private (bool, List<KeyValuePair<string, CurrencyDrawer>>) TryWithdrawal(string request)
        {
            bool canWithdraw = false;
            List<KeyValuePair<string, CurrencyDrawer>> dispenserDrawers = InitializeDispenserDrawers();

            bool canParse = int.TryParse(request.Trim(new char[] { ' ', '\t', '$' }), out int amount);

            if(canParse && amount > 0)
            {
                int remainder = amount;
                
                foreach(KeyValuePair<string, CurrencyDrawer> drawer in dispenserDrawers)
                {
                    if(remainder >= drawer.Value.CurrencyValue)
                    {
                        int billCount = Convert.ToInt32(Math.Floor((decimal)remainder / drawer.Value.CurrencyValue));
                        var availableQuantity = _currencyDrawers.Where(d => d.Key == drawer.Key).Select(q => q.Value.Quantity).Single();

                        // if the quantity of bills in the machine is greater or equal to the bills needed for this denomonation add the billCount to the dispenserDrawer
                        // if the quantity of bills available is less than needed, take what is available and add them to the dispenserDrawer
                        drawer.Value.AddCurrency(availableQuantity >= billCount ? billCount : availableQuantity);
                        remainder -= drawer.Value.CurrencyValue * drawer.Value.Quantity;
                    }
                }
                if(remainder == 0)
                {
                    canWithdraw = true;
                }
            }

            return (canWithdraw, dispenserDrawers);
        }

        // method for removing cash from the ATM stock
        private void ExecuteWithdrawal(List<KeyValuePair<string, CurrencyDrawer>> dispenserDrawers)
        {
            foreach(KeyValuePair<string, CurrencyDrawer> drawer in dispenserDrawers)
            {
                _currencyDrawers.Where(c => c.Key == drawer.Key).Select(v => v.Value).Single().RemoveCurrency(drawer.Value.Quantity);
            }
        }
        #endregion

        #region CurrencyDrawer initialization
        // Initializes the ATM's currency drawers to maximum stock levels
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

        // returns List of CurrencyDrawers in order from highest to lowest CurrencyValue with quantities set to 0
        // returned list is used to hold the quantity of each currency denomonation to be removed from the ATM on withdrawl
        private List<KeyValuePair<string, CurrencyDrawer>> InitializeDispenserDrawers()
        {
            List<KeyValuePair<string, CurrencyDrawer>> emptyDrawers = new List<KeyValuePair<string, CurrencyDrawer>>();

            foreach (KeyValuePair<string, CurrencyDrawer> pair in _currencyDrawers.OrderByDescending(x => x.Value.CurrencyValue))
            {
                emptyDrawers.Add(new KeyValuePair<string, CurrencyDrawer>(pair.Key, new CurrencyDrawer(pair.Value.CurrencyValue, 0)));
            }

            return emptyDrawers;
        }
        #endregion

        #region Inventory Methods
        // Gets quantity from all currencyDrawers in the ATM
        internal List<string> GetDispenserInventory()
        {
            List<string> dispenserInventory = new List<string>();

            foreach(KeyValuePair<string, CurrencyDrawer> pair in _currencyDrawers.OrderByDescending(x => x.Value.CurrencyValue))
            {
                dispenserInventory.Add(GetDenomonationInfo(pair.Key));
            }

            return dispenserInventory;
        }

        // Gets quantity from specified currency drawers 
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

        // Gets currency value and quantity from the specified CurrencyDrawer using the CurrencyDrawer.ToString() method
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
        #endregion

        #region Restock logic
        internal void RestockCurrency()
        {
            foreach(KeyValuePair<string, CurrencyDrawer> drawer in _currencyDrawers)
            {
                drawer.Value.Restock(_maxCurrencyStock);
            }
        }
        #endregion
    }
}
