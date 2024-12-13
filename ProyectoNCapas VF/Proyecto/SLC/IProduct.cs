using Entities;
using System.Collections.Generic;

namespace SLC
{
    public interface IProduct
    {
        // Crea un nuevo producto
        Product CreateProduct(Product newProduct);

        // Recupera un producto por su ID
        Product GetProductByID(int productId);

        // Actualiza un producto existente
        bool UpdateProduct(Product productToUpdate);

        // Elimina un producto por su ID
        bool DeleteProduct(int productId);

        // Recupera productos filtrados por categoría
        List<Product> GetProductsByCategoryID(int categoryId);

        // Recupera todos los productos
        List<Product> GetAllProducts();
    }
}
