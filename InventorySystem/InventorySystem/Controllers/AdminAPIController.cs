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
        /*
          For Response "Ok":
          PUT or POST: The resource describing the result of the action is transmitted in the message body.
          - PUT or POST - This descriptor applies to when we respond to a PUT or POST request with Ok.
          - The resourse - The new entity that we've created, or the entity that we've modified.
          - The result - The entity that has been created, or post-modification.
          - Transmitted in the message body - Encode it as JSON and send it with the Ok.
          */

        [HttpPost("AddProduct")]
        public ActionResult<Product> AddProduct(string name, string quantity, string isDiscontinued)
        {
            ActionResult<Product> response;
            Product created;
            try
            {
                // We aren't concerned with validation here. Only in BLL.
                created = new AdminController().CreateProduct(name, quantity, isDiscontinued);
                // Encode our created object as JSON and bounce it back with the request.
                response = Ok(created);
            }
            catch (Exception e)
            {
                response = UnprocessableEntity(new { error = e.Message });
            }

            // Return the response.
            return response;
        }


        [HttpPut("DiscontinueProduct")]
        public ActionResult<Product> DiscontinueProduct(string id)
        {
            ActionResult<Product> response;
            Product modified;
            try
            {
                // We aren't concerned with validation here. Only in BLL.
                modified = new AdminController().DiscontinueProductByID(id);
                // Encode our created object as JSON and bounce it back with the request.
                response = Ok(modified);
            }
            catch (InvalidOperationException)
            {
                response = StatusCode(403, new { error = $"No product was found with the ID of {id}." });
            }
            catch (Exception e)
            {
                response = StatusCode(403, new { error = e.Message }); ;
            }

            // Return the response.
            return response;
        }

        [HttpPut("AddQuantityProduct")]
        public ActionResult<Product> AddQuantityProduct(string id, string amount)
        {
            ActionResult<Product> response;
            Product modified;
            try
            {
                // We aren't concerned with validation here. Only in BLL.
                modified = new AdminController().AddQuantityToProductByID(id, amount);
                // Encode our created object as JSON and bounce it back with the request.
                response = Ok(modified);
            }
            catch (InvalidOperationException)
            {
                response = StatusCode(403, new { error = $"No product was found with the ID of {id}." });
            }
            catch (Exception e)
            {
                response = StatusCode(403, new { error = e.Message }); ;
            }

            // Return the response.
            return response;
        }
        [HttpPut("SubtractQuantityProduct")]
        public ActionResult<Product> SubtractQuantityProduct(string id, string amount)
        {
            ActionResult<Product> response;
            Product modified;
            try
            {
                // We aren't concerned with validation here. Only in BLL.
                modified = new AdminController().SubtractQuantityFromProductByID(id, amount);
                // Encode our created object as JSON and bounce it back with the request.
                response = Ok(modified);
            }
            catch (InvalidOperationException)
            {
                response = StatusCode(403, new { error = $"No product was found with the ID of {id}." });
            }
            catch (Exception e)
            {
                response = StatusCode(403, new { error = e.Message }); ;
            }

            // Return the response.
            return response;
        }
        [HttpGet("ShowInventory")]
        public ActionResult<List<Product>> ShowInventory()
        {
            // TODO: Catch for unable to connect to database.
            // Return the response.
            return new AdminController().GetProducts();
        }


        [HttpPatch("ModifyQuantity")]
        public ActionResult<Product> ModifyQuantity(string id, string op, string amount)
        {
            ActionResult<Product> response;

            op = op ?? "".Trim().ToLower();
            string[] validOps = { "add", "subtract" };
            if (!validOps.Contains(op))
            {
                response = UnprocessableEntity(new { error = "Invalid PATCH operation specified, choices are 'add' and 'subtract'." });
            }
            else
            {
                Product modified;
                try
                {
                    switch (op)
                    {
                        case "add":
                            modified = new AdminController().AddQuantityToProductByID(id, amount);
                            response = Ok(modified);
                            break;
                        case "subtract":
                            modified = new AdminController().SubtractQuantityFromProductByID(id, amount);
                            response = Ok(modified);
                            break;
                        default:
                            response = StatusCode(500);
                            break;
                    }
                }
                catch (InvalidOperationException)
                {
                    response = StatusCode(403, new { error = $"No product was found with the ID of {id}." });
                }
                catch (Exception e)
                {
                    response = StatusCode(403, new { error = e.Message }); ;
                }
            }


            // Return the response.
            return response;
        }
    }

}

// Code borrowed @link: https://github.com/TECHCareers-by-Manpower/4.1-MVC/blob/e9c88722d326733c92e8e8c10c85edc53637fabc/MVC_4Point1/Controllers/PersonAPIController.cs