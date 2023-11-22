// Jordan Green. st10083222. PROG7312. POE Part 2

/***************************************************************************************
 *    Title: <C# Dictionary with examples>
 *    Author: <ankita_saini>
 *    Date Published: <n.d>
 *    Date Retrieved: <24 September 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://www.geeksforgeeks.org/c-sharp-dictionary-with-examples/>
 *
 ***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Jordan_Green._st10083222._PROG7312._POE.Models
{
    public class ShuffleModel
    {
        public Dictionary<string, string> CallNumberDescriptions { get; set; }

        public ShuffleModel()
        {
            // Initialize the dictionary with call numbers and descriptions.
            CallNumberDescriptions = new Dictionary<string, string>
            {
                { "000-099", "General Knowledge" },
                { "100-199", "Philosophy & Physchology" },
                { "Religion", "200-299" },
                { "Social Sciences", "300-399" },
            };
        }

        // GetDescription method to retrieve a description for a given call number.
        public string GetDescription(string callNumber)
        {
            if (CallNumberDescriptions.ContainsKey(callNumber))
            {
                // Return the description for the provided call number.
                return CallNumberDescriptions[callNumber];
            }
            else
            {
                // In this if else, we return a message indicating that the call number is not in the dictionary.
                return "Call number not found.";
            }
        }
    }
}
