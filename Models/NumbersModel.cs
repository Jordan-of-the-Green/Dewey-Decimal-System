// Jordan Green. st10083222. PROG7312. POE Part 3

using System.Collections.Generic;

namespace Jordan_Green._st10083222._PROG7312._POE
{
    // Model class for representing the data structure
    public class NumbersModel
    {
        public List<Category> Categories { get; set; }
    }

    // Class representing a category
    public class Category
    {
        public string Name { get; set; }
        public List<Subcategory> Subcategories { get; set; }
    }

    // Class representing a subcategory, with a parent-child relationship
    public class Subcategory
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public List<Subcategory> Children { get; set; } // Added property for children subcategories
    }
}