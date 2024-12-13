using ProyectoProxyService;
using Entities;
using System.Web.Mvc;
using SL.Logger;
using Proyecto.MVCPLS.Filters;

namespace Proyecto.MVCPLS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Proxy _proxy;

        public CategoryController()
        {
            _proxy = new Proxy();
        }

        [HttpGet]
        //[RoleAuthorize("Viewer", "Editor", "Admin")]
        public ActionResult List()
        {
            try
            {
                LogHelper.LogInformation("Consultando lista de categorías.");
                var categories = _proxy.GetAllCategories();

                if (categories == null || categories.Count == 0)
                {
                    LogHelper.LogWarning("No hay categorías disponibles.");
                    ViewBag.Message = "No hay categorías disponibles.";
                }
                else
                {
                    LogHelper.LogInformation("Categorías obtenidas exitosamente.");
                }

                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View(categories);
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("Error al obtener la lista de categorías.", ex);
                ViewBag.ErrorMessage = "No se pudieron cargar las categorías.";
                return View("Error");
            }
        }

        [HttpGet]
        //[RoleAuthorize("Editor", "Admin")]
        public ActionResult Create()
        {
            LogHelper.LogInformation("Mostrando formulario de creación de categoría.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[RoleAuthorize("Editor", "Admin")]
        public ActionResult Create(Category newCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LogHelper.LogInformation("Creando nueva categoría.");
                    var createdCategory = _proxy.CreateCategory(newCategory);

                    TempData["SuccessMessage"] = "Categoría creada exitosamente.";
                    return RedirectToAction("List");
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError("Error al crear la categoría.", ex);
                    ModelState.AddModelError("", "No se pudo crear la categoría.");
                }
            }

            return View(newCategory);
        }

        [HttpGet]
        //[RoleAuthorize("Editor", "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                LogHelper.LogInformation($"Consultando categoría con ID: {id} para edición.");
                var category = _proxy.GetCategoryByID(id);

                if (category == null)
                {
                    LogHelper.LogWarning($"No se encontró la categoría con ID: {id}.");
                    return HttpNotFound();
                }

                LogHelper.LogInformation($"Categoría obtenida: ID={category.CategoryID}, Nombre={category.CategoryName}, Descripción={category.Description}");
                return View(category);
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("Error al obtener la categoría para edición.", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[RoleAuthorize("Editor", "Admin")]
        public ActionResult Edit(Category updatedCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LogHelper.LogInformation($"Actualizando categoría con ID: {updatedCategory.CategoryID}.");
                    var result = _proxy.UpdateCategory(updatedCategory);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Categoría actualizada exitosamente.";
                        return RedirectToAction("List");
                    }
                    else
                    {
                        LogHelper.LogWarning("Error al actualizar la categoría.");
                        ModelState.AddModelError("", "No se pudo actualizar la categoría.");
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.LogError("Error al actualizar la categoría.", ex);
                    ModelState.AddModelError("", "Error al actualizar la categoría.");
                }
            }

            return View(updatedCategory);
        }

        [HttpGet]
        //[RoleAuthorize("Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                LogHelper.LogInformation($"Consultando categoría con ID: {id} para eliminación.");
                var category = _proxy.GetCategoryByID(id);

                if (category == null)
                {
                    LogHelper.LogWarning($"No se encontró la categoría con ID: {id}.");
                    return HttpNotFound();
                }

                return View(category);
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("Error al obtener la categoría para eliminación.", ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[RoleAuthorize("Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                LogHelper.LogInformation($"Eliminando categoría con ID: {id}.");
                var result = _proxy.DeleteCategory(id);

                if (result)
                {
                    TempData["SuccessMessage"] = "Categoría eliminada exitosamente.";
                }
                else
                {
                    LogHelper.LogWarning("Error al eliminar la categoría.");
                    TempData["ErrorMessage"] = "No se pudo eliminar la categoría.";
                }

                return RedirectToAction("List");
            }
            catch (System.Exception ex)
            {
                LogHelper.LogError("Error al eliminar la categoría.", ex);
                TempData["ErrorMessage"] = "Error al eliminar la categoría.";
                return RedirectToAction("List");
            }
        }
    }
}