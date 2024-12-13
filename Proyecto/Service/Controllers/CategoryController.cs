using BLL;
using Entities;
using SLC;
using System.Collections.Generic;
using System.Web.Http;

namespace Service.Controllers
{
    [RoutePrefix("categories")]
    public class CategoryController : ApiController, ICategory
    {
        private readonly CategoryLogic _logic;

        public CategoryController()
        {
            _logic = new CategoryLogic();
        }

        [HttpPost]
        [Route("create")]
        public Category CreateCategory(Category newCategory)
        {
            return _logic.CreateCategory(newCategory);
        }

        [HttpGet]
        [Route("get/{id}")]
        public Category GetCategoryByID(int id)
        {
            return _logic.GetCategoryById(id);
        }

        [HttpPost]
        [Route("update")]
        public bool UpdateCategory(Category categoryToUpdate)
        {
            return _logic.UpdateCategory(categoryToUpdate);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public bool DeleteCategory(int id)
        {
            return _logic.DeleteCategory(id);
        }

        [HttpGet]
        [Route("all")]
        public List<Category> GetAllCategories()
        {
            return _logic.GetAllCategories();
        }
    }
}