using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATMSolution
{
    /// <summary>
    /// This class handles input and output in the Console
    /// Contains all methods for outputting informatin to the console
    /// Handles string parsing of console input
    /// </summary>
    internal static class ATMIO
    {
        /// <summary>
        /// Parses the input string into a tuple
        /// </summary>
        /// <param name="input">A string.</param>
        /// <returns>
        /// A tuple of (char, string) representing the requested command and any passed arguments
        /// </returns>
        internal static (char, string) ParseInput(string input)
        {
            char cmd = string.IsNullOrWhiteSpace(input) ? ' ' : Char.ToUpper(input[0]);
            string args = input.Any(x => Char.IsWhiteSpace(x)) ? input.Split(new char[] { ' ', '\t' }, 2)[1] : string.Empty;

            var parsed = (Command: cmd, Args: args);
            return parsed;
        }

        internal static void PrintMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
