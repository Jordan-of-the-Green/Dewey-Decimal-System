// Jordan Green. st10083222. PROG7312. POE Part 1

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

namespace Jordan_Green._st10083222._PROG7312._POE.Controllers
{
    public class HomeController : Controller
    {
        private const int MaxScore = 100;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            // Initialize the logger
            _logger = logger;
        }

        // Action method for the default view (Index)
        public ActionResult Index()
        {
            return View();
        }

        // Action method for generating and displaying call numbers
        public ActionResult CallNumbers()
        {
            try
            {
                CallNumber model = new CallNumber();
                List<int> callNumbers = model.GenerateCallNumbers();

                ViewBag.CallNumbers = callNumbers;

                // Gamification features
                ViewBag.MaxScore = MaxScore;
                ViewBag.Score = 0;
                ViewBag.Timer = 60; // Set the timer to 60 seconds

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating call numbers.");
                ViewBag.ErrorMessage = "An error occurred while generating call numbers. Please try again.";
                return View("Error");
            }
        }

        // Action method for checking the order of call numbers
        [HttpPost]
        public ActionResult Result(List<int> orderedCallNumbers, int score, int timer)
        {
            try
            {
                CallNumber model = new CallNumber();
                bool isCorrectOrder = model.CheckOrder(orderedCallNumbers);

                // Update the score based on correctness and time remaining
                score = CalculateScore(isCorrectOrder, score, timer);

                ViewBag.IsCorrectOrder = isCorrectOrder;
                ViewBag.Score = score;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the result.");
                ViewBag.ErrorMessage = "An error occurred while processing the result. Please try again.";
                return View("Error");
            }
        }

        private int CalculateScore(bool isCorrectOrder, int currentScore, int timer)
        {
            if (isCorrectOrder)
            {
                // Increase the score if the order is correct
                currentScore += (int)Math.Round((double)MaxScore / timer);
            }

            return currentScore;
        }
    }
}
