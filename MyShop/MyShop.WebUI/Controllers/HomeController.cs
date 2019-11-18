﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Product> _context;
        private readonly IRepository<ProductCategory> _productCategories;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext)
        {
            _context = productContext;
            _productCategories = productCategoriesContext;
        }
        public ActionResult Index(string category = null)
        {
            List<Product> products;
            List<ProductCategory> categories = _productCategories.Collection().ToList();

            if (category == null)
            {
                products = _context.Collection().ToList();
            }
            else
            {
                products = _context.Collection().Where(p => p.Category == category).ToList();
            }
            ProductListViewModel viewModel = new ProductListViewModel();
            viewModel.Products = products;
            viewModel.ProductCategories = categories;

            return View(viewModel);
        }

        public ActionResult Details(string id)
        {
            Product product = _context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}