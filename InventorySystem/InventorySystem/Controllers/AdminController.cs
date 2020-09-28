﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InventorySystem.Models;
using MVC_4Point1.Models.Exceptions;
using System.Collections.Generic;

namespace InventorySystem.Controllers
{
    public class AdminController : Controller
    {
        // In-class practice:
        // Add validation to ensure that the first and last name do not contain numbers.
        // Add trimming to the CreatePerson method.
        // Add validation that neither first nor last name are greater than 50 characters long.

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // id is not necessary as it is now AUTO_INCREMENT in the database, and is generated thereby.
        public IActionResult Add(string name, string quantity)
        {
            // When this Action gets called, there are 3 likely states:
            // 1) First page load, no data has been provided (initial state).
            // 2) Partial data has been provided (error state).
            // 3) Complete data has been provided (submit state).

            // A request has come in that has some data stored in the query (GET or POST).

            if (Request.Query.Count > 0)
            {
                AddProduct(name, quantity);
               try
                {
                   

                    ViewBag.Success = "Successfully added the person to the list.";
                }
                catch (Exception e)
                {
                    // All expected data not provided, so this will be our error state.
                    ViewBag.Exception = e;

                }
            }

            return View();
        }

        public int AddProduct(string name, string quantity)
        {
            name = name != null ? name.Trim() : null;
            quantity = quantity != null ? quantity.Trim() : null;
            int createdID;
            ProductValidationException exception = new ProductValidationException();
            // Be a little more specific than "== null" because that doesn't account for whitespace.
            if (string.IsNullOrWhiteSpace(name))
            {
                exception.SubExceptions.Add(new ArgumentNullException(nameof(name), "Name was not provided."));
            }
            else
            {
                if (name.Any(x => char.IsDigit(x)))
                {
                    exception.SubExceptions.Add(new ArgumentException(nameof(name), "Name cannot contain numbers."));
                }
                if (name.Length > 50)
                {
                    exception.SubExceptions.Add(new ArgumentOutOfRangeException(nameof(name), "Product name cannot be more than 50 characters long."));
                }
            }
            if (string.IsNullOrWhiteSpace(quantity))
            {
                exception.SubExceptions.Add(new ArgumentNullException(nameof(quantity), "Quantity was not provided."));
            }
            else
            {
                if (int.Parse(quantity) < 0)
                {
                    exception.SubExceptions.Add(new ArgumentNullException(nameof(quantity), "Quantity can not be leass than 0."));
                }
            }
            // If any exceptions have been generated by any validation, throw them as one bundled exception.
            if (exception.SubExceptions.Count > 0)
            {
                throw exception;
            }
            // If we're at this point, we have no exceptions, as nothing got thrown.
            // At this point, ID is 0.
            Product newProduct = new Product()
            {
                Name = name,
                Quantity = int.Parse(quantity)
            };

            // Add the new model instances to the database.
            using (InventoryContext context = new InventoryContext())
            {
                // By adding our object to the context, we're queueing it to receive an AUTO_INCREMENT ID once saved.
                context.Product.Add(newProduct);

                // When we save it, the object and all references thereto are updated with the new ID.
                context.SaveChanges();
                // Which makes it very simple to then get the new ID to return.
                createdID = newProduct.ID;
            }
            return createdID;
        }


        // A PUT request, semantically, overwrites an entire entity and does not update a specific field.
        public void DiscontinueProduct(string id, string isdiscontinued)
        {
            id = id != null ? id.Trim() : null;
            isdiscontinued = isdiscontinued != null ? isdiscontinued.Trim() : null;
            
            int idParsed = 0;

            using (InventoryContext context = new InventoryContext())
            {
                ProductValidationException exception = new ProductValidationException();

                if (string.IsNullOrWhiteSpace(id))
                {
                    exception.SubExceptions.Add(new ArgumentNullException(nameof(id), "ID was not provided."));
                }
                else
                {
                    if (!int.TryParse(id, out idParsed))
                    {
                        exception.SubExceptions.Add(new ArgumentException(nameof(id), "ID was not valid."));
                    }
                    else
                    {
                        if (context.Product.Where(x => x.ID == idParsed).Count() != 1)
                        {
                            exception.SubExceptions.Add(new NullReferenceException("Product with that ID does not exist."));
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(isdiscontinued))
                {
                    exception.SubExceptions.Add(new ArgumentNullException(nameof(isdiscontinued), "Value is not provided."));
                }
                else
                {
                    if (isdiscontinued.Any(x => char.IsDigit(x)))
                    {
                        exception.SubExceptions.Add(new ArgumentException(nameof(isdiscontinued), "Value cannot contain numbers."));
                    }
                   
                }              

                // If any exceptions have been generated by any validation, throw them as one bundled exception.
                if (exception.SubExceptions.Count > 0)
                {
                    throw exception;
                }

                // If we're at this point, we have no exceptions, as nothing got thrown.
                Product target = context.Product.Where(x => x.ID == idParsed).Single();
                target.IsDiscontinued = bool.Parse(isdiscontinued);                
                context.SaveChanges();
            }
        }



        // A PUT request, semantically, overwrites an entire entity and does not update a specific field.
        public void UpdateQuantity(string id, string quantity)
        {
            id = id != null ? id.Trim() : null;
            quantity = quantity != null ? quantity.Trim() : null;
            int idParsed = 0;
            bool Quantity = true;
            using (InventoryContext context = new InventoryContext())
            {
                ProductValidationException exception = new ProductValidationException();
                if (string.IsNullOrWhiteSpace(id))
                {
                    exception.SubExceptions.Add(new ArgumentNullException(nameof(id), "ID was not provided."));
                }
                else
                {
                    if (!int.TryParse(id, out idParsed))
                    {
                        exception.SubExceptions.Add(new ArgumentException(nameof(id), "ID was not valid."));
                    }
                    else
                    {
                        if (context.Product.Where(x => x.ID == idParsed).Count() != 1)
                        {
                            exception.SubExceptions.Add(new NullReferenceException("Product with that ID does not exist."));
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(quantity))
                {
                    exception.SubExceptions.Add(new ArgumentNullException(nameof(quantity), "Quantity was not provided."));
                }
                else
                {
                    if (int.Parse(quantity) < 0)
                    {
                        exception.SubExceptions.Add(new ArgumentNullException(nameof(quantity), "Quantity can not be less than 0."));
                    }
                    else
                    {
                        if (context.Product.Where(x => x.ID == idParsed && x.IsDiscontinued == Quantity).Count() != 1)
                        {
                            exception.SubExceptions.Add(new NullReferenceException("Product is discontinued, You can not add it ."));
                        }
                    }
                }
                // If any exceptions have been generated by any validation, throw them as one bundled exception.
                if (exception.SubExceptions.Count > 0)
                {
                    throw exception;
                }
                // If we're at this point, we have no exceptions, as nothing got thrown.
                Product target = context.Product.Where(x => x.ID == idParsed).Single();
                target.Quantity = int.Parse(quantity);
                context.SaveChanges();
            }
        }

        public void SubtractQuantity(string id, string subtractedquantity)
        {
            id = id != null ? id.Trim() : null;
            subtractedquantity = subtractedquantity != null ? subtractedquantity.Trim() : null;
            int idParsed = 0;
            using (InventoryContext context = new InventoryContext())
            {
                ProductValidationException exception = new ProductValidationException();
                if (string.IsNullOrWhiteSpace(id))
                {
                    exception.SubExceptions.Add(new ArgumentNullException(nameof(id), "ID was not provided."));
                }
                else
                {
                    if (!int.TryParse(id, out idParsed))
                    {
                        exception.SubExceptions.Add(new ArgumentException(nameof(id), "ID was not valid."));
                    }
                    else
                    {
                        if (context.Product.Where(x => x.ID == idParsed).Count() != 1)
                        {
                            exception.SubExceptions.Add(new NullReferenceException("Product with that ID does not exist."));
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(subtractedquantity))
                {
                    exception.SubExceptions.Add(new ArgumentNullException(nameof(subtractedquantity), "Amount of Subtracted Quantity was not provided."));
                }
                else
                {
                    if (int.Parse(subtractedquantity) < 0)
                    {
                        exception.SubExceptions.Add(new ArgumentNullException(nameof(subtractedquantity), "Quantity can not be less than 0."));
                    }
                    else
                    {
                        if (context.Product.Where(x => x.ID == idParsed && x.IsDiscontinued == false).Count() != 1)
                        {
                            exception.SubExceptions.Add(new NullReferenceException("You can not add the quantity to discontinued product."));
                        }
                        if (context.Product.Where(x => x.ID == idParsed && x.Quantity >= int.Parse(subtractedquantity)).Count() != 1)
                        {
                            exception.SubExceptions.Add(new NullReferenceException("Amount subtracted must be less than or equal to the current quantity and greater than zero."));
                        }
                    }
                }
                // If any exceptions have been generated by any validation, throw them as one bundled exception.
                if (exception.SubExceptions.Count > 0)
                {
                    throw exception;
                }
                // If we're at this point, we have no exceptions, as nothing got thrown.
                Product target = context.Product.Where(x => x.ID == idParsed).Single();
                target.Quantity = target.Quantity - int.Parse(subtractedquantity);
                context.SaveChanges();
            }
        }

        public List<Product> GetProduct()
        {
            using (InventoryContext context = new InventoryContext())
            {
                return context.Product.Where(x => x.IsDiscontinued==false).OrderBy(x=>x.Quantity).ToList();
            }
        }      
    }
}
// Code borrowed @link:https://github.com/TECHCareers-by-Manpower/4.1-MVC/blob/e9c88722d326733c92e8e8c10c85edc53637fabc/MVC_4Point1/Controllers/PersonController.cs