// Jordan Green. st10083222. PROG7312. POE Part 1

/***************************************************************************************
 *    Title: <C# | List Class>
 *    Author: <Kirti_Mangal>
 *    Date Published: <25 November 2022>
 *    Date Retrieved: <27 August 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://www.geeksforgeeks.org/c-sharp-list-class/>
 *
 ***************************************************************************************/


// CallNumber.cs
using System;
using System.Collections.Generic;

namespace Jordan_Green._st10083222._PROG7312._POE.Models
{
    public class CallNumber
    {
        // Generate a list of random call numbers
        public List<int> GenerateCallNumbers()
        {
            try
            {
                List<int> callNumbers = new List<int>();
                Random random = new Random();

                for (int i = 0; i < 10; i++)
                {
                    callNumbers.Add(random.Next(100, 1000));
                }

                return callNumbers;
            }
            catch (Exception ex)
            {
                // If an exception occurs, rethrow it with a custom error message
                throw new Exception("Error generating call numbers: " + ex.Message);
            }
        }

        // Check if the provided call numbers are in ascending order
        public bool CheckOrder(List<int> callNumbers)
        {
            try
            {
                // Check if the input list is null or empty
                if (callNumbers == null || callNumbers.Count == 0)
                {
                    // Handle the case where the input is invalid
                    return false;
                }

                for (int i = 1; i < callNumbers.Count; i++)
                {
                    if (callNumbers[i] < callNumbers[i - 1])
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // If an exception occurs, rethrow it with a custom error message
                throw new Exception("Error checking call number order: " + ex.Message);
            }
        }

    }
}
