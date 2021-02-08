using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMSolution
{
    /// <summary>
    /// Implementation of currency drawers for the CashDispenser class
    /// </summary>
    internal class CurrencyDrawer
    {
        public int CurrencyValue { get; private set; }
        public int Quantity { get; private set; }

        public CurrencyDrawer(int value, int quantity)
        {
            this.CurrencyValue = value;
            this.Quantity = quantity;
        }

        public override string ToString()
        {
            return $"${CurrencyValue} - {Quantity}";
        }
    }
}
