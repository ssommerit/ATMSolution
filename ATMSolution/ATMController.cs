using System;
using System.Collections.Generic;
using System.Text;

namespace ATMSolution
{
    /// <summary>
    /// The main controller for the ATM machine
    /// </summary>
    internal class ATMController
    {
        private readonly CashDispenser _cashDispenser = new CashDispenser();

        internal void Run()
        {
            while (true)
            {
                (char Command, string Args) = ATMIO.ParseInput(Console.ReadLine().Trim());

                switch (Command)
                {
                    case 'I':
                        // print inventory by denomonation
                        ATMIO.PrintDenomonationInfo(_cashDispenser.GetDenomonationInventory(Args));
                        break;
                    case 'Q':
                        Environment.Exit(0);
                        break;
                    default:
                        ATMIO.PrintMessage("Failure: Invalid Command");
                        break;
                }
            }
        }
    }
}
