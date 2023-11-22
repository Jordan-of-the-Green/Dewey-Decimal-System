// Jordan Green. st10083222. PROG7312. POE Part 3

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

/***************************************************************************************
 *    Title: <Random Class>
 *    Author: <Microsoft>
 *    Date Published: <2023>
 *    Date Retrieved: <19 November 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-7.0>
 *
 ***************************************************************************************/

/***************************************************************************************
 *    Title: <Read JSON file using C# in ASP.Net MVC>
 *    Author: <Microsoft>
 *    Date Published: <2023>
 *    Date Retrieved: <17 November 2023>
 *    Code version: <1.0.0>
 *    Availability: <https://www.aspsnippets.com/Articles/3937/Read-JSON-file-using-C-in-ASPNet-MVC/>
 *
 ***************************************************************************************/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // ILogger
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jordan_Green._st10083222._PROG7312._POE.Controllers
{
    public class NumbersController : Controller
    {
        private NumbersModel _model;
        private List<Subcategory> _currentOptions;
        private int _level;
        private int _timer;
        private const int MaxTimePerQuestion = 10;

        private readonly ILogger<NumbersController> _logger; // Logger for logging information

        // Constructor with dependency injection of logger
        public NumbersController(ILogger<NumbersController> logger)
        {
            // Initialize the logger
            _logger = logger;
            try
            {
                _model = LoadDataFromJson();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading data from JSON: {ex.Message}");
                throw;
            }
            _currentOptions = GetRandomOptions();
            _level = 1;
            _timer = MaxTimePerQuestion;
        }

        // Action method for handling the answer submission
        [HttpPost]
        public IActionResult CheckAnswer(int selectedCode)
        {
            var correctOption = _currentOptions.First();
            var correctCode = correctOption.Code;
            var correctAnswer = correctOption.Name;

            // Finding the selected option based on the selectedCode
            var selectedOption = _currentOptions.FirstOrDefault(option => option.Code == selectedCode);
            var selectedAnswer = selectedOption != null ? selectedOption.Name : "Unknown";

            // Assigning values to ViewBag for rendering in the view
            ViewBag.CorrectCode = correctCode;
            ViewBag.CorrectAnswer = correctAnswer;
            ViewBag.SelectedCode = selectedCode;
            ViewBag.SelectedAnswer = selectedAnswer;

            // Logging selected code and available options for debugging
            _logger.LogInformation($"Selected Code: {selectedCode}");
            _logger.LogInformation("Available Options:");
            foreach (var option in _currentOptions)
            {
                _logger.LogInformation($"Code: {option.Code}, Name: {option.Name}");
            }

            // Checking if the selected code is correct and updating the view accordingly
            if (selectedCode == correctCode)
            {
                _level++;
                _currentOptions = GetRandomOptions(correctCode);
                ViewBag.ProgressColor = "bg-success"; // Set color to green for correct answer
            }
            else
            {
                ViewBag.WrongAnswer = true;
                ViewBag.ProgressColor = "bg-danger"; // Set color to red for incorrect answer
            }

            // Updating ViewBag values and resetting timer and progress bar
            ViewBag.Level = _level;
            ResetTimerAndProgressBar();

            ViewBag.Question = GetRandomQuestion();
            ViewBag.Options = _currentOptions;

            return View("Index");
        }

        // Action method for rendering the main view
        public IActionResult Index()
        {
            ViewBag.Level = _level;
            ViewBag.Question = GetRandomQuestion();
            ViewBag.Options = _currentOptions;
            ViewBag.Timer = _timer;
            ViewBag.MaxTime = MaxTimePerQuestion;
            ViewBag.Progress = (_level - 1) / (double)_model.Categories.Count * 100; // Progress calculation

            return View();
        }

        // Helper method to reset the timer and progress bar
        private void ResetTimerAndProgressBar()
        {
            _timer = MaxTimePerQuestion;
            ViewBag.Timer = _timer;
            ViewBag.Progress = (_level - 1) / (double)_model.Categories.Count * 100;
        }

        // Helper method to load data from a JSON file with error handling
        private NumbersModel LoadDataFromJson()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "categories.json");
            try
            {
                var jsonData = System.IO.File.ReadAllText(jsonPath);
                var numbersModel = JsonConvert.DeserializeObject<NumbersModel>(jsonData);
                return numbersModel;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError($"JSON file not found: {jsonPath}");
                throw new Exception("Data file not found", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error deserializing JSON: {ex.Message}");
                throw new Exception("Error deserializing JSON", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        // Helper method to get a random question
        private Subcategory GetRandomQuestion()
        {
            var random = new Random();
            var randomCategory = _model.Categories[random.Next(_model.Categories.Count)];
            var randomSubcategory = randomCategory.Subcategories[random.Next(randomCategory.Subcategories.Count)];

            return randomSubcategory;
        }

        // Helper method to get random options based on a parent code
        private List<Subcategory> GetRandomOptions(int parentCode = 0)
        {
            var random = new Random();
            var allCategories = _model.Categories.ToList();

            // Select a random third-level entry for the question
            var correctSubcategories = allCategories
                .SelectMany(c => c.Subcategories)
                .SelectMany(s => s.Children ?? new List<Subcategory>())
                .Where(s => s.Code / 100 == parentCode / 100)
                .ToList();

            var selectedThirdLevelOption = correctSubcategories.Count > 0
                ? correctSubcategories[random.Next(correctSubcategories.Count)]
                : null;

            // Display only the description of the selected third-level option for the question
            var question = selectedThirdLevelOption != null
                ? new Subcategory { Name = selectedThirdLevelOption.Name }
                : null;

            // Display four top-level options for the answers
            var topLevelOptions = allCategories
                .OrderBy(c => c.Name) // Order top-level categories numerically
                .Take(4)
                .ToList();

            // Display both the call number and description for the answer options
            var options = new List<Subcategory>();
            foreach (var option in topLevelOptions)
            {
                options.Add(new Subcategory
                {
                    Code = option.Subcategories.First().Code,
                    Name = option.Subcategories.First().Name
                });
            }

            // Shuffle the options and insert the question at a random position
            options = options.OrderBy(x => random.Next()).ToList();
            if (selectedThirdLevelOption != null)
            {
                options.Insert(random.Next(options.Count), question);
            }

            return options;
        }


    }
}