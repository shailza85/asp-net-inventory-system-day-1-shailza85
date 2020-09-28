using System;
using Microsoft.AspNetCore.Mvc;
using InventorySystem.Controllers;
using MVC_4Point1.Models.Exceptions;
using System.Linq;
using InventorySystem.Models;
using System.Collections.Generic;

namespace InventorySystem.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class AdminAPIController : ControllerBase
    {
        /* Create an HttpPost “AddProduct” endpoint that allows the user to add a product to the database*/
        
        [HttpPost("Product/AddProduct")]
        public ActionResult AddProduct_POST(string name, string quantity)
        {
            ActionResult response;
            try
            {
                int newID = new AdminController().AddProduct(name, quantity);

                // Just for fun:
                // (It's also an example of how to throw a code that doesn't have a method built-in)
                if (name.Trim().ToUpper() == "TEAPOT" && quantity.Trim().ToUpper() == "COFFEE")
                {
                    response = StatusCode(418, new { message = $"Successfully created teapot but it does not want to brew coffee." });
                }
                else
                {
                    // This should really be a Create() that provides the API endpoint for the GET to retrieve the created object.
                    response = Created($"API/AdminAPI/Product/ID/{newID}", new { message = $"Successfully created product {name} {quantity} at ID {newID}." });
                }
            }
            catch (ProductValidationException e)
            {
                response = UnprocessableEntity(new { errors = e.SubExceptions.Select(x => x.Message) });
            }


            return response;
        }

        /*
         * Create an HttpPut “DiscontinueProduct” endpoint that allows the user to discontinue a product
            Accepts the product ID as a parameter
            If Product ID is invalid  - respond with Http Status 403 and include a descriptive message in the Content
            Set IsDiscontinued to false
         * */

        [HttpPut("Product/DiscontinueProduct")]
        public ActionResult DiscontinueProduct_PUT(string id, string isdiscontinued)
        {
            ActionResult response;
            try
            {
                new AdminController().DiscontinueProduct(id, isdiscontinued);

                // Semantically, we should be including a copy of the object (or at least a DTO rendering of it) in the Ok response.
                // For our purposes, a message with the fields will suffice.
                response = Ok(new { message = $"Successfully updated product at ID {id} that is to be discontinued {isdiscontinued}." });
            }
            catch (ProductValidationException e)
            {
                // If it couldn't find the entity to update, that's the primary concern, so discard the other subexceptions and just return NotFound().
                if (e.SubExceptions.Any(x => x.GetType() == typeof(NullReferenceException)))
                {
                    response = StatusCode(403, new { message = $"No entity exists at ID {id}." });
                    
                }
                // If there's no NullReferenceException, but there's still an exception, return the list of problems.
                else
                {
                    response = UnprocessableEntity(new { errors = e.SubExceptions.Select(x => x.Message) });
                }
            }


            return response;
        }


        /*
        * Create an HttpPut “AddQuantityProduct” endpoint that allows the user to add to a product’s quantity
           Accepts the product ID as a parameter
           If Product ID is invalid  - respond with Http Status 403 and include a descriptive message in the Content
           AmountAdded must be a positive integer
           If integer invalid  - respond with Http Status 403 and include a descriptive message in the Content
           User cannot add to a product that has been discontinued
           If Product is discontinued - respond with Http Status 403 and include a descriptive message in the Content

         */

        [HttpPut("Product/AddQuantity")]
        public ActionResult AddQuantityProduct_PUT(string id, string quantity)
        {
            ActionResult response;
            try
            {
                new AdminController().UpdateQuantity(id, quantity);
                // (It's also an example of how to throw a code that doesn't have a method built-in)
                // Semantically, we should be including a copy of the object (or at least a DTO rendering of it) in the Ok response.
                // For our purposes, a message with the fields will suffice.
                response = Ok(new { message = $"Successfully update quantity at ID {id} to {quantity}." });
            }
            catch (ProductValidationException e)
            {
                // If it couldn't find the entity to update, that's the primary concern, so discard the other subexceptions and just return NotFound().
                if (e.SubExceptions.Any(x => x.GetType() == typeof(NullReferenceException)))
                {
                    //response = NotFound(new { error = $"No entity exists at ID {id}." });
                    response = StatusCode(403, new { message = $"No entity exists at ID {id}." });
                }
                // If there's no NullReferenceException, but there's still an exception, return the list of problems.
                else
                {
                    // response = UnprocessableEntity(new { errors = e.SubExceptions.Select(x => x.Message) });
                    response = StatusCode(403, new { errors = e.SubExceptions.Select(x => x.Message) });
                }
            }
            return response;
        }

        /*
         * Create an HttpPut “SubtractQuantityProduct” endpoint that allows the user to subtract from a product’s quantity
            Accepts the product ID as a parameter
            If Product ID is invalid  - respond with Http Status 403 and include a descriptive message in the Content
            AmountSubtracted must be less than or equal to the current quantity and greater than zero
            If user attempts to subtract more than is in stock, reject the entire transaction, respond with Http Status 403 and include a descriptive message in the Content

         */

       
        [HttpPut("Product/SubtractQuantityProduct")]
        public ActionResult SubtractQuantityProduct_PUT(string id, string subtractAmount)
        {
            ActionResult response;
            try
            {
                new AdminController().SubtractQuantity(id, subtractAmount);
                // Semantically, we should be including a copy of the object (or at least a DTO rendering of it) in the Ok response.
                // For our purposes, a message with the fields will suffice.
                response = Ok(new { message = $"Successfully updated Quantity at ID {id} and after subtraction, the quantity is :{subtractAmount}." });
            }
            catch (ProductValidationException e)
            {
                
                if (e.SubExceptions.Any(x => x.GetType() == typeof(NullReferenceException)))
                {
                  response = StatusCode(403, new { errors = e.SubExceptions.Select(x => x.Message) });
                }
                // If there's no NullReferenceException, but there's still an exception, return the list of problems.
                else
                {
                    response = StatusCode(403, new { errors = e.SubExceptions.Select(x => x.Message) });
                }
            }
            return response;
        }

        /*  Create an HttpGet “ShowInventory” endpoint that displays the entire inventory
          Requires no parameters
          This endpoint will not return products that have been discontinued
          Order by “Quantity” from lowest to highest so that user will know what needs to be restocked*/

        [HttpGet("Product/ShowInventory")]
        public ActionResult<IEnumerable<Product>> GetInventory()
        {

            return new AdminController().GetProduct();
        }

    }
}

// Code borrowed @link: https://github.com/TECHCareers-by-Manpower/4.1-MVC/blob/e9c88722d326733c92e8e8c10c85edc53637fabc/MVC_4Point1/Controllers/PersonAPIController.cs