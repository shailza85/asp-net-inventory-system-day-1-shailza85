using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_4Point1.Models.Exceptions
{
    public class ProductValidationException : Exception
    {
        // A list of exceptions to be thrown as one.
        public List<Exception> SubExceptions { get; set; } = new List<Exception>();

        // Override our message with a summary.
        public override string Message => $"There are {SubExceptions.Count} exceptions.";

        // When we construct this exception without a messsage, we get an empty sub-list which we can populate.
        public ProductValidationException() : base()
        { }

        public ProductValidationException(string message) : base()
        {
            // When we construct this exception with a message, it gets added to the subexceptions list.
            SubExceptions.Add(new Exception(message));
        }
    }
}

// Code borrowed @link:https://github.com/TECHCareers-by-Manpower/4.1-MVC/blob/e9c88722d326733c92e8e8c10c85edc53637fabc/MVC_4Point1/Models/Exceptions/PersonValidationException.cs