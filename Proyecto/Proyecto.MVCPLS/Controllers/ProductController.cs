using ProyectoProxyService;
using Entities;
using System.Web.Mvc;
using SL.Logger;

namespace Proyecto.MVCPLS.Controllers
{
    public class ProductController : Controller
    {
        private readonly Proxy _proxy;

        public ProductController()
        {
            _proxy = new Proxy();
        }

        [HttpGet]
        public ActionResult List()
        {
            try
            {
                LogHelper.LogInformation("Consultando lista de productos.");
                var products = _proxy.GetAllProducts();
                if (products == null || products.Count == 0)
                {
                    LogHelper.LogWarning("No hay productos disponibles.");
                    ViewBag.Message = "No hay productos disponibles.";
                }
                else
                {
                    LogHelper.LogInformation("Productos obtenidos exitosamente.");
                }

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
                return View(products);
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("Error al obtener la lista de productos.", ex);
                ViewBag.ErrorMessage = "No se pudieron cargar los productos.";
                return View("Error");
            }
        }

        public ActionResult Create()
        {
            try
            {
                LogHelper.LogInformation("Mostrando formulario de creación de producto.");
                var categories = _proxy.GetAllCategories();
                ViewBag.Categories = categories;
                return View();
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("Error al cargar categorías para el formulario de creación.", ex);
                ViewBag.ErrorMessage = "No se pudieron cargar las categorías.";
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LogHelper.LogInformation("Creando nuevo producto.");
                    var createdProduct = _proxy.CreateProduct(newProduct);
                    TempData["SuccessMessage"] = "Producto creado exitosamente.";
                    return RedirectToAction("List");
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError("Error al crear el producto.", ex);
                    ModelState.AddModelError("", "No se pudo crear el producto.");
                }
            }
            return View(newProduct);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                LogHelper.LogInformation($"Consultando producto con ID: {id} para edición.");
                var product = _proxy.GetProductByID(id);
                if (product == null)
                {
                    LogHelper.LogWarning($"No se encontró el producto con ID: {id}.");
                    return HttpNotFound();
                }
                var categories = _proxy.GetAllCategories();
                ViewBag.Categories = categories;
                return View(product);
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("Error al obtener el producto para edición.", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product updatedProduct)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LogHelper.LogInformation($"Actualizando producto con ID: {updatedProduct.ProductID}.");
                    var result = _proxy.UpdateProduct(updatedProduct);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Producto actualizado exitosamente.";
                        return RedirectToAction("List");
                    }
                    else
                    {
                        LogHelper.LogWarning("Error al actualizar el producto.");
                        ModelState.AddModelError("", "No se pudo actualizar el producto.");
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError("Error al actualizar el producto.", ex);
                    ModelState.AddModelError("", "Error al actualizar el producto.");
                }
            }
            return View(updatedProduct);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                LogHelper.LogInformation($"Consultando producto con ID: {id} para eliminación.");
                var product = _proxy.GetProductByID(id);
                if (product == null)
                {
                    LogHelper.LogWarning($"No se encontró el producto con ID: {id}.");
                    return HttpNotFound();
                }
                return View(product);
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("Error al obtener el producto para eliminación.", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                LogHelper.LogInformation($"Eliminando producto con ID: {id}.");
                var result = _proxy.DeleteProduct(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Producto eliminado exitosamente.";
                }
                else
                {
                    LogHelper.LogWarning("Error al eliminar el producto.");
                    TempData["ErrorMessage"] = "No se pudo eliminar el producto.";
                }
                return RedirectToAction("List");
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("Error al eliminar el producto.", ex);
                TempData["ErrorMessage"] = "Error al eliminar el producto.";
                return RedirectToAction("List");
            }
        }
    }
}