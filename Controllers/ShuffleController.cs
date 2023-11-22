// Jordan Green. st10083222. PROG7312. POE Part 2

/***************************************************************************************
 *    Title: <C# If ... Else>
 *    Author: <W3Schools>
 *    Date Published: <2023>
 *    Date Retrieved: <31 September 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://www.w3schools.com/cs/cs_conditions.php>
 *
 ***************************************************************************************/

/***************************************************************************************
 *    Title: <Logging in .NET Core and ASP.NET Core>
 *    Author: <Microsoft>
 *    Date Published: <09 May 2023>
 *    Date Retrieved: <19 November 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://www.w3schools.com/cs/cs_conditions.php>
 *
 ***************************************************************************************/

using Jordan_Green._st10083222._PROG7312._POE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jordan_Green._st10083222._PROG7312._POE.Controllers
{
    public class ShuffleController : Controller
    {
        private readonly ShuffleModel _model;
        private readonly ILogger<ShuffleController> _logger;

        public ShuffleController(ILogger<ShuffleController> logger)
        {
            // Initialize the ShuffleModel instance
            _model = new ShuffleModel();
            // Initialize the logger
            _logger = logger;
        }

        // Action method for Index
        public ActionResult Index()
        {
            try
            {
                // Shuffle the pairs using a random order
                var shuffledPairs = _model.CallNumberDescriptions.OrderBy(x => Guid.NewGuid()).ToDictionary(item => item.Key, item => item.Value);

                // Return the shuffled pairs as the view model
                return View(shuffledPairs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the ShuffleController while shuffling pairs");
                ViewBag.ErrorMessage = "An unexpected error occurred while shuffling pairs. Please try again later or contact support.";

                return View("Error");
            }
        }

        // Action method for CheckAnswer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckAnswer(Dictionary<string, string> userAnswers)
        {
            // Check if the model state is valid before processing user input
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Invalid user input. Please check your answers and try again.";
                return View("Error");
            }

            try
            {
                int correctAnswers = 0;

                foreach (var userAnswer in userAnswers)
                {
                    string callNumber = userAnswer.Key;
                    string userDescription = userAnswer.Value;

                    if (_model.CallNumberDescriptions.TryGetValue(callNumber, out string correctDescription))
                    {
                        // Compare the user's answer with the correct answer, ignoring case
                        if (string.Equals(userDescription, correctDescription, StringComparison.OrdinalIgnoreCase))
                        {
                            correctAnswers++;
                        }
                    }
                }

                // Calculate the score as a percentage
                int totalQuestions = _model.CallNumberDescriptions.Count;
                double score = totalQuestions > 0 ? (double)correctAnswers / totalQuestions * 100 : 0;

                // Pass the number of correct answers, total questions, and the score to the result view
                ViewBag.CorrectAnswers = correctAnswers;
                ViewBag.TotalQuestions = totalQuestions;
                ViewBag.Score = score;

                return View("Result");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the ShuffleController while checking answers");
                ViewBag.ErrorMessage = "An unexpected error occurred while checking answers. Please try again later or contact support.";

                return View("Error");
            }
        }
    }
}
