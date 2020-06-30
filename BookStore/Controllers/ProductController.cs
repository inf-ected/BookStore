using BookStore.Domain.Entities;
using BookStore.Domain.Implementation;
using BookStore.Domain.Implementation.Repo;
using BookStore.Domain.Interfaces;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class ProductController : Controller
    {
        //private IProductRepository productRepository;
        private UnitOfWork<Product> unitOfWork;
        private UnitOfWork<Category> unitOfCategory;
        public int PageSize = 5;
        public Category CurrentCategory { get; set; }

        public ProductController() //IProductRepository repo)
        {
            //this.productRepository = repo;

            unitOfWork = new UnitOfWork<Product>(new GenericRepository<Product>(new ApplicationDbContext()));
            unitOfCategory = new UnitOfWork<Category>(new GenericRepository<Category>(new ApplicationDbContext()));
            CurrentCategory = unitOfCategory.GenericRepository.FirstOrDefault();//repo.GetPrincipalCategory(null);
        }

        // GET: Product
        //[Route ("test/{categoryId:int}/{page:int}")]
        public ViewResult List(int categoryId, int page = 1)
        {
            //var source = productRepository.Products
            //   .Where(p => p.Categories.Any(x => x.Id == categoryId));

            //var source = productRepository.GetProductsByCategoryId(categoryId);

            var source = unitOfWork.GenericRepository.Get();// null, null, "Category");

            ProductListViewModel model = new ProductListViewModel
            {
                Products = source
                .OrderBy(p => p.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize),
                PageInfo = new PageInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = unitOfWork.GenericRepository.Get(x => x.Categories.Any(y => y.Id == categoryId))
                    //source 
                    //productRepository.Products
                    //            .Where(p => p.Categories.Any(x => x.Id == categoryId))
                    .Count()
                },
                Category = CurrentCategory
            };
            return View(model);
        }


    }
}