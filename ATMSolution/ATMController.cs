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

        internal void Run()
        {
            while (true)
            {
                (char Command, string args) = ATMIO.ParseInput(Console.ReadLine().Trim());

                switch (Command)
                {
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
