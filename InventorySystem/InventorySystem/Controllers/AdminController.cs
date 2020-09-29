using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InventorySystem.Models;
using MVC_4Point1.Models.Exceptions;
using System.Collections.Generic;

namespace InventorySystem.Controllers
{
    public class AdminController : Controller
    {

        public Product CreateProduct(string name, string quantity, string isDiscontinued)
        {
            int quantityParsed;
            bool isDiscontinuedParsed;

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name), "Product name was not provided.");
            }
            else
            {
                name = name.Trim();
            }

            if (string.IsNullOrWhiteSpace(quantity))
            {
                throw new ArgumentNullException(nameof(quantity), "Product quantity was not provided.");
            }
            else
            {
                quantity = quantity.Trim();
                if (!int.TryParse(quantity, out quantityParsed))
                {
                    throw new ArgumentException("Product quantity was not a valid integer.", nameof(quantity));
                }
                else
                {
                    if (quantityParsed < 0)
                    {
                        throw new ArgumentException("Product quantity cannot be less than zero.", nameof(quantity));
                    }
                }
            }

            if (string.IsNullOrEmpty(isDiscontinued))
            {
                // Default for IsDiscontinued;
                isDiscontinuedParsed = false;
            }
            else
            {
                isDiscontinued = isDiscontinued.Trim();
                if (!bool.TryParse(isDiscontinued, out isDiscontinuedParsed))
                {
                    throw new ArgumentException("Please provide either 'true' or 'false' for whether the product is discontinued. If you do not provide it, it will be false.", nameof(isDiscontinued));
                }
            }

            Product created = new Product()
            {
                Name = name,
                Quantity = quantityParsed,
                IsDiscontinued = isDiscontinuedParsed
            };

            using (InventoryContext context = new InventoryContext())
            {
                context.Product.Add(created);
                context.SaveChanges();
            }

            return created;
        }
        public Product DiscontinueProductByID(string id)
        {
            int idParsed;

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Product ID was not provided.");
            }
            else
            {
                id = id.Trim();
                if (!int.TryParse(id, out idParsed))
                {
                    throw new ArgumentException("Product ID was not a valid integer.", nameof(id));
                }
            }

            Product modified;

            using (InventoryContext context = new InventoryContext())
            {
                modified = context.Product.Where(x => x.ID == idParsed).Single();
                modified.IsDiscontinued = true;
                context.SaveChanges();
            }

            return modified;
        }
        public Product AddQuantityToProductByID(string id, string amount)
        {
            int idParsed;
            int amountParsed;

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Product ID was not provided.");
            }
            else
            {
                id = id.Trim();
                if (!int.TryParse(id, out idParsed))
                {
                    throw new ArgumentException("Product ID was not a valid integer.", nameof(id));
                }
            }

            if (string.IsNullOrWhiteSpace(amount))
            {
                throw new ArgumentNullException(nameof(amount), "Product amount was not provided.");
            }
            else
            {
                id = id.Trim();
                if (!int.TryParse(amount, out amountParsed))
                {
                    throw new ArgumentException("Product amount was not a valid integer.", nameof(amount));
                }
                else
                {
                    if (amountParsed < 0)
                    {
                        throw new ArgumentException("Product amount cannot be less than zero.", nameof(amount));
                    }
                }
            }

            Product modified;

            using (InventoryContext context = new InventoryContext())
            {
                modified = context.Product.Where(x => x.ID == idParsed).Single();
                if (modified.IsDiscontinued)
                {
                    throw new ArgumentException($"The item with the ID provided ({idParsed}) is discontinued.", nameof(id));
                }
                else
                {
                    modified.Quantity += amountParsed;
                    context.SaveChanges();
                }

            }
            return modified;
        }
        public Product SubtractQuantityFromProductByID(string id, string amount)
        {
            int idParsed;
            int amountParsed;

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Product ID was not provided.");
            }
            else
            {
                id = id.Trim();
                if (!int.TryParse(id, out idParsed))
                {
                    throw new ArgumentException("Product ID was not a valid integer.", nameof(id));
                }
            }

            if (string.IsNullOrWhiteSpace(amount))
            {
                throw new ArgumentNullException(nameof(amount), "Product amount was not provided.");
            }
            else
            {
                id = id.Trim();
                if (!int.TryParse(amount, out amountParsed))
                {
                    throw new ArgumentException("Product amount was not a valid integer.", nameof(amount));
                }
                else
                {
                    if (amountParsed < 0)
                    {
                        throw new ArgumentException("Product amount cannot be less than zero.", nameof(amount));
                    }
                }
            }

            Product modified;

            using (InventoryContext context = new InventoryContext())
            {
                modified = context.Product.Where(x => x.ID == idParsed).Single();
                if (modified.Quantity - amountParsed < 0)
                {
                    throw new ArgumentException($"The item with the ID provided ({idParsed}) does not have the requisite quantity to subtract. It has {modified.Quantity} units remaining.", nameof(id));
                }
                else
                {
                    modified.Quantity -= amountParsed;
                    context.SaveChanges();
                }

            }
            return modified;
        }
        public List<Product> GetProducts()
        {
            List<Product> products;
            using (InventoryContext context = new InventoryContext())
            {
                products = context.Product.Where(x => x.IsDiscontinued == false).OrderBy(x => x.Quantity).ToList();
            }
            return products;
        }
    }
}
// Code borrowed @link:https://github.com/TECHCareers-by-Manpower/4.1-MVC/blob/e9c88722d326733c92e8e8c10c85edc53637fabc/MVC_4Point1/Controllers/PersonController.cs
