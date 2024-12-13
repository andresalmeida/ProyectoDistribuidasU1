using DAL;
using Entities;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ProductLogic
    {
        public Product CreateProduct(Product newProduct)
        {
            Product result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Verificar si el nombre del producto ya existe
                var existingProduct = repository.Retrieve<Product>(p => p.ProductName == newProduct.ProductName);
                if (existingProduct != null)
                {
                    throw new InvalidOperationException("El nombre del producto ya está registrado.");
                }

                // Crear el producto
                result = repository.Create(newProduct);
            }
            return result;
        }

        public Product GetProductByID(int productId)
        {
            Product result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                result = repository.Retrieve<Product>(p => p.ProductID == productId);
                if (result == null)
                {
                    throw new KeyNotFoundException("Producto no encontrado.");
                }
            }
            return result;
        }

        public bool UpdateProduct(Product productToUpdate)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                // Validar que el nombre del producto no esté en uso por otro producto
                var existingProduct = repository.Retrieve<Product>(p =>
                    p.ProductName == productToUpdate.ProductName && p.ProductID != productToUpdate.ProductID);
                if (existingProduct != null)
                {
                    throw new InvalidOperationException("El nombre del producto ya está en uso por otro producto.");
                }

                result = repository.Update(productToUpdate);
            }
            return result;
        }

        public bool DeleteProduct(int productId)
        {
            bool result = false;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                var product = GetProductByID(productId);
                if (product == null)
                {
                    throw new KeyNotFoundException("Producto no encontrado.");
                }

                if (product.UnitsInStock > 0)
                {
                    throw new InvalidOperationException("No se puede eliminar el producto porque aún tiene existencias.");
                }

                result = repository.Delete(product);
            }
            return result;
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            List<Product> result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                result = repository.Filter<Product>(p => p.CategoryID == categoryId);
            }
            return result;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> result = null;
            using (var repository = RepositoryFactory.CreateRepository())
            {
                result = repository.GetAll<Product>();
            }
            return result;
        }
    }
}