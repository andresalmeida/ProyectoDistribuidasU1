using ProyectoProxyService;
using System;

namespace TestProyectoProxyService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var proxy = new Proxy();

            Console.WriteLine("Probando obtener todas las categorías...");
            var categories = proxy.GetAllCategories();
            if (categories != null && categories.Count > 0)
            {
                Console.WriteLine($"Se encontraron {categories.Count} categorías:");
                foreach (var category in categories)
                {
                    Console.WriteLine($"ID: {category.CategoryID}, Nombre: {category.CategoryName}");
                }
            }
            else
            {
                Console.WriteLine("No se encontraron categorías.");
            }

            Console.WriteLine("Pruebas completadas. Presiona cualquier tecla para salir.");
            Console.ReadKey();
        }
    }
}