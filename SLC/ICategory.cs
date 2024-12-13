using Entities;
using System.Collections.Generic;

namespace SLC
{
    public interface ICategory
    {
        // Crea una nueva categoría
        Category CreateCategory(Category newCategory);

        // Recupera una categoría por su ID
        Category GetCategoryByID(int categoryId);

        // Actualiza una categoría existente
        bool UpdateCategory(Category categoryToUpdate);

        // Elimina una categoría por su ID
        bool DeleteCategory(int categoryId);

        // Recupera todas las categorías
        List<Category> GetAllCategories();
    }
}
