using BLL;
using Entities;
using SLC;
using System.Collections.Generic;
using System.Web.Http;

namespace Service.Controllers
{
    [RoutePrefix("product")]
    public class ProductController : ApiController, IProduct
    {
        private readonly ProductLogic _logic;

        public ProductController()
        {
            _logic = new ProductLogic();
        }

        [HttpPost]
        [Route("create")]
        public Product CreateProduct(Product newProduct)
        {
            return _logic.CreateProduct(newProduct);
        }

        [HttpGet]
        [Route("get/{id}")]
        public Product GetProductByID(int id)
        {
            return _logic.GetProductByID(id);
        }

        [HttpPost]
        [Route("update")]
        public bool UpdateProduct(Product productToUpdate)
        {
            return _logic.UpdateProduct(productToUpdate);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public bool DeleteProduct(int id)
        {
            return _logic.DeleteProduct(id);
        }

        [HttpGet]
        [Route("category/{id}")]
        public List<Product> GetProductsByCategoryID(int id)
        {
            return _logic.GetProductsByCategory(id);
        }

        [HttpGet]
        [Route("all")]
        public List<Product> GetAllProducts()
        {
            return _logic.GetAllProducts();
        }
    }
}
