using DAL;
using Entities;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class CategoryLogic
    {
        public Category CreateCategory(Category newCategory)
        {
            Category result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Verificar si la categoría ya existe
                var existingCategory = repository.Retrieve<Category>(c => c.CategoryName == newCategory.CategoryName);
                if (existingCategory != null)
                {
                    throw new InvalidOperationException("La categoría ya existe.");
                }

                // Crear la categoría
                result = repository.Create(newCategory);
            }
            return result;
        }

        public Category GetCategoryById(int categoryId)
        {
            Category result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                result = repository.Retrieve<Category>(c => c.CategoryID == categoryId);
                if (result == null)
                {
                    throw new KeyNotFoundException("Categoría no encontrada.");
                }
            }
            return result;
        }

        public List<Category> GetAllCategories()
        {
            List<Category> result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                result = repository.GetAll<Category>();
            }
            return result;
        }

        public bool UpdateCategory(Category updatedCategory)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                var existingCategory = repository.Retrieve<Category>(c =>
                    c.CategoryName == updatedCategory.CategoryName && c.CategoryID != updatedCategory.CategoryID);
                if (existingCategory != null)
                {
                    throw new InvalidOperationException("El nombre de la categoría ya está en uso.");
                }

                result = repository.Update(updatedCategory);
            }
            return result;
        }

        public bool DeleteCategory(int categoryId)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                var category = GetCategoryById(categoryId);
                if (category == null)
                {
                    throw new KeyNotFoundException("Categoría no encontrada.");
                }

                result = repository.Delete(category);
            }
            return result;
        }
    }
}