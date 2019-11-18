using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        private readonly IRepository<Product> _context;
        private readonly IRepository<ProductCategory> _productCategories;

        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext)
        {
            _context = productContext;
            _productCategories = productCategoriesContext;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = _context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel viewModel = new ProductManagerViewModel
            {
                Product = new Product(),
                ProductCategories = _productCategories.Collection()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                _context.Insert(product);
                _context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string id)
        {
            Product product = _context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel
                {
                    Product = product,
                    ProductCategories = _productCategories.Collection()
                };

                return View(viewModel);
            }
        }
        [HttpPost]
        public ActionResult Edit(Product product, string id)
        {
            Product productToEdit = _context.Find(id);

            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }

                productToEdit.Category = product.Category;
                productToEdit.Description = product.Description;
                productToEdit.Image = product.Image;
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;

              
                _context.Commit();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(string id)
        {
            Product productToDelete = _context.Find(id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            Product productToDelete = _context.Find(id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                _context.Delete(id);
                _context.Commit();
                return RedirectToAction("Index");

            }

        }
    }
}