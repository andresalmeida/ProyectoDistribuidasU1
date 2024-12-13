//using ProyectoProxyService;
//using Entities;
//using SL.Logger;
//using SL.Authentication;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using Proyecto.MVCPLS.Filters;

//namespace Proyecto.MVCPLS.Controllers
//{
//    public class UserAccountController : Controller
//    {
//        private readonly Proxy _proxy;
//        private readonly IEmailService _emailService;

//        public UserAccountController()
//        {
//            _proxy = new Proxy();
//            _emailService = new EmailService();
//        }

//        // Listar usuarios
//        [HttpGet]
//        //[RoleAuthorize("Viewer", "Editor", "Admin")]
//        public ActionResult List()
//        {
//            try
//            {
//                LogHelper.LogInformation("Consultando lista de usuarios.");
//                var users = _proxy.GetAllUserAccounts();
//                if (users == null || !users.Any())
//                {
//                    LogHelper.LogWarning("No hay usuarios disponibles.");
//                    ViewBag.Message = "No hay usuarios registrados.";
//                }
//                ViewBag.SuccessMessage = TempData["SuccessMessage"];
//                ViewBag.ErrorMessage = TempData["ErrorMessage"];
//                return View(users);
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al consultar usuarios.", ex);
//                ViewBag.ErrorMessage = "No se pudieron cargar los usuarios.";
//                return View("Error");
//            }
//        }

//        // Crear usuario
//        //[RoleAuthorize("Editor", "Admin")]
//        //public ActionResult Create()
//        //{
//        //    LogHelper.LogInformation("Mostrando formulario de creación de usuario.");
//        //    return View();
//        //}

//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //[RoleAuthorize("Editor", "Admin")]
//        //public ActionResult Create(UserAccount newUserAccount)
//        //{
//        //    try
//        //    {
//        //        if (ModelState.IsValid)
//        //        {
//        //            LogHelper.LogInformation("Creando nuevo usuario.");
//        //            var createdUser = _proxy.CreateUserAccount(newUserAccount);
//        //            if (createdUser != null)
//        //            {
//        //                TempData["SuccessMessage"] = "Usuario creado exitosamente.";
//        //                return RedirectToAction("List");
//        //            }
//        //            else
//        //            {
//        //                ModelState.AddModelError("", "Error al crear el usuario.");
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        LogHelper.LogError("Error al crear usuario.", ex);
//        //        ModelState.AddModelError("", ex.Message);
//        //    }
//        //    return View(newUserAccount);
//        //}

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(UserAccount newUserAccount)
//        {
//            try
//            {
//                // Registro del valor recibido
//                LogHelper.LogInformation($"Estado recibido: {newUserAccount.Status}");

//                // Forzar un valor válido temporalmente
//                newUserAccount.Status = "Active";

//                if (string.IsNullOrEmpty(newUserAccount.Status) ||
//                    (newUserAccount.Status != "Active" && newUserAccount.Status != "Inactive"))
//                {
//                    ModelState.AddModelError("Status", "El estado debe ser 'Active' o 'Inactive'.");
//                    return View(newUserAccount);
//                }

//                // Registro antes de enviar al proxy
//                LogHelper.LogInformation($"Estado enviado al proxy: {newUserAccount.Status}");

//                // Lógica para guardar el usuario
//                var createdUser = _proxy.CreateUserAccount(newUserAccount);

//                if (createdUser != null)
//                {
//                    TempData["SuccessMessage"] = "Usuario creado exitosamente.";
//                    return RedirectToAction("List");
//                }
//                else
//                {
//                    ModelState.AddModelError("", "Error al crear el usuario.");
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al crear usuario.", ex);
//                ModelState.AddModelError("", ex.Message);
//            }

//            return View(newUserAccount);
//        }

//        // Editar usuario
//        [HttpGet]
//        //[RoleAuthorize("Editor", "Admin")]
//        public ActionResult Edit(int id)
//        {
//            try
//            {
//                LogHelper.LogInformation($"Consultando usuario con ID: {id}.");
//                var user = _proxy.GetUserAccountByID(id);
//                if (user == null)
//                {
//                    LogHelper.LogWarning($"No se encontró el usuario con ID: {id}.");
//                    return HttpNotFound();
//                }
//                return View(user);
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al consultar usuario para edición.", ex);
//                ViewBag.ErrorMessage = "No se pudo cargar el usuario.";
//                return View("Error");
//            }
//        }

//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //[RoleAuthorize("Editor", "Admin")]
//        //public ActionResult Edit(int id, UserAccount updatedUserAccount)
//        //{
//        //    if (ModelState.IsValid)
//        //    {
//        //        try
//        //        {
//        //            updatedUserAccount.UserID = id;
//        //            LogHelper.LogInformation($"Actualizando usuario con ID: {id}.");
//        //            var result = _proxy.UpdateUserAccount(updatedUserAccount);
//        //            if (result)
//        //            {
//        //                TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
//        //                return RedirectToAction("List");
//        //            }
//        //            else
//        //            {
//        //                ModelState.AddModelError("", "Error al actualizar el usuario.");
//        //            }
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            LogHelper.LogError("Error al actualizar usuario.", ex);
//        //            ModelState.AddModelError("", ex.Message);
//        //        }
//        //    }
//        //    return View(updatedUserAccount);
//        //}

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        //[RoleAuthorize("Editor", "Admin")]
//        public ActionResult Edit(int id, UserAccount updatedUserAccount)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    updatedUserAccount.UserID = id;

//                    // Validar el estado
//                    if (updatedUserAccount.Status != "Active" && updatedUserAccount.Status != "Inactive")
//                    {
//                        ModelState.AddModelError("Status", "El estado debe ser 'Active' o 'Inactive'.");
//                        return View(updatedUserAccount);
//                    }

//                    LogHelper.LogInformation($"Actualizando usuario con ID: {id}.");
//                    var result = _proxy.UpdateUserAccount(updatedUserAccount);

//                    if (result)
//                    {
//                        TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
//                        return RedirectToAction("List");
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "Error al actualizar el usuario.");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    LogHelper.LogError("Error al actualizar usuario.", ex);
//                    ModelState.AddModelError("", ex.Message);
//                }
//            }

//            return View(updatedUserAccount);
//        }


//        // Eliminar usuario
//        [HttpGet]
//        //[RoleAuthorize("Admin")]
//        public ActionResult Delete(int id)
//        {
//            try
//            {
//                LogHelper.LogInformation($"Consultando usuario con ID: {id} para eliminación.");
//                var user = _proxy.GetUserAccountByID(id);
//                if (user == null)
//                {
//                    LogHelper.LogWarning($"No se encontró el usuario con ID: {id}.");
//                    return HttpNotFound();
//                }
//                return View(user);
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al consultar usuario para eliminación.", ex);
//                ViewBag.ErrorMessage = "No se pudo cargar el usuario.";
//                return View("Error");
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        //[RoleAuthorize("Admin")]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            try
//            {
//                LogHelper.LogInformation($"Eliminando usuario con ID: {id}.");
//                var result = _proxy.DeleteUserAccount(id);
//                if (result)
//                {
//                    TempData["SuccessMessage"] = "Usuario eliminado exitosamente.";
//                }
//                else
//                {
//                    TempData["ErrorMessage"] = "No se pudo eliminar el usuario.";
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al eliminar usuario.", ex);
//                TempData["ErrorMessage"] = $"Error al eliminar el usuario: {ex.Message}";
//            }
//            return RedirectToAction("List");
//        }

//        // Recuperar contraseña
//        [HttpGet]
//        public ActionResult ForgotPassword()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<ActionResult> ForgotPassword(string email)
//        {
//            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
//            {
//                return Json(new { success = false, message = "Correo electrónico inválido." });
//            }

//            try
//            {
//                LogHelper.LogInformation($"Iniciando recuperación de contraseña para el correo: {email}.");
//                var user = _proxy.GetUserByEmail(email);
//                if (user == null)
//                {
//                    return Json(new { success = false, message = "No se encontró ningún usuario con ese correo." });
//                }

//                var random = new Random();
//                string securityCode = random.Next(1000, 9999).ToString();
//                string subject = "Recuperación de contraseña";
//                string body = $"Tu código de verificación es: {securityCode}";

//                await _emailService.SendEmailAsync(email, subject, body);

//                Session["SecurityCode"] = securityCode;
//                Session["UserIdToReset"] = user.UserID;

//                // Redirigir a la vista de verificación del código
//                return RedirectToAction("VerifyCodeAndChangePassword", "UserAccount");
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error durante la recuperación de contraseña.", ex);
//                return Json(new { success = false, message = $"Error al enviar el correo: {ex.Message}" });
//            }
//        }

//        [HttpGet]
//        public ActionResult VerifyCodeAndChangePassword()
//        {
//            // Aquí se puede agregar lógica adicional si es necesario, pero generalmente solo se devuelve la vista.
//            return View();
//        }


//        [HttpPost]
//        public ActionResult VerifyResetCode(string enteredCode)
//        {
//            var storedCode = Session["SecurityCode"] as string;
//            var userIdToReset = Session["UserIdToReset"] as int?;

//            if (storedCode == enteredCode && userIdToReset.HasValue)
//            {
//                return Json(new { success = true, userId = userIdToReset.Value });
//            }

//            return Json(new { success = false, message = "El código ingresado es incorrecto." });
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult EditLog(UserAccount userAccount)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    // Lógica para actualizar el usuario
//                    var result = _proxy.UpdateUserAccount(userAccount);
//                    if (result)
//                    {
//                        TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
//                        return RedirectToAction("List");
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "No se pudo actualizar el usuario.");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError("", "Ocurrió un error: " + ex.Message);
//                }
//            }
//            return View(userAccount);
//        }

//        public ActionResult AddUser()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult AddUser(UserAccount newUserAccount)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    // Lógica para crear el usuario
//                    var createdUser = _proxy.CreateUserAccount(newUserAccount);
//                    if (createdUser != null)
//                    {
//                        TempData["SuccessMessage"] = "Usuario creado exitosamente.";
//                        return RedirectToAction("List");
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "No se pudo crear el usuario.");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError("", "Error interno: " + ex.Message);
//                }
//            }
//            return View(newUserAccount);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        //[RoleAuthorize("Editor", "Admin")]
//        public ActionResult CreateUser(UserAccount newUserAccount)
//        {
//            try
//            {
//                // Verificar si el correo ha sido verificado
//                if (Session["IsEmailVerified"] == null || !(bool)Session["IsEmailVerified"] || newUserAccount.Email != Session["EmailToVerify"]?.ToString())
//                {
//                    return Json(new { success = false, message = "El correo no ha sido verificado." });
//                }

//                if (ModelState.IsValid)
//                {
//                    LogHelper.LogInformation($"Creando nuevo usuario: {newUserAccount.UserName}");

//                    // Llamar al proxy para crear el usuario
//                    var createdUser = _proxy.CreateUserAccount(newUserAccount);

//                    if (createdUser != null)
//                    {
//                        // Limpiar sesión tras creación exitosa
//                        Session.Remove("IsEmailVerified");
//                        Session.Remove("EmailToVerify");

//                        TempData["SuccessMessage"] = "Usuario creado exitosamente.";
//                        return Json(new { success = true, redirectUrl = Url.Action("List", "UserAccount") });
//                    }
//                }

//                // Si la validación falla
//                ModelState.AddModelError("", "Error al crear el usuario.");
//                return Json(new { success = false, message = "Error al crear el usuario. Verifique los datos ingresados." });
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al crear el usuario.", ex);
//                return Json(new { success = false, message = "Ocurrió un error interno. Intente nuevamente más tarde." });
//            }
//        }

//        private bool IsValidEmail(string email)
//        {
//            try
//            {
//                var addr = new System.Net.Mail.MailAddress(email);
//                return addr.Address == email;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}

//SEGUNDA VERSION

//using ProyectoProxyService;
//using Entities;
//using SL.Logger;
//using SL.Authentication;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using Proyecto.MVCPLS.Filters;

//namespace Proyecto.MVCPLS.Controllers
//{
//    public class UserAccountController : Controller
//    {
//        private readonly Proxy _proxy;
//        private readonly IEmailService _emailService;

//        public UserAccountController()
//        {
//            _proxy = new Proxy();
//            _emailService = new EmailService();
//        }

//        // Listar usuarios
//        [HttpGet]
//        //[RoleAuthorize("Viewer", "Editor", "Admin")]
//        public ActionResult List()
//        {
//            try
//            {
//                LogHelper.LogInformation("Consultando lista de usuarios.");
//                var users = _proxy.GetAllUserAccounts();
//                if (users == null || !users.Any())
//                {
//                    LogHelper.LogWarning("No hay usuarios disponibles.");
//                    ViewBag.Message = "No hay usuarios registrados.";
//                }
//                ViewBag.SuccessMessage = TempData["SuccessMessage"];
//                ViewBag.ErrorMessage = TempData["ErrorMessage"];
//                return View(users);
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al consultar usuarios.", ex);
//                ViewBag.ErrorMessage = "No se pudieron cargar los usuarios.";
//                return View("Error");
//            }
//        }

//        // Crear usuario
//        //[RoleAuthorize("Editor", "Admin")]
//        //public ActionResult Create()
//        //{
//        //    LogHelper.LogInformation("Mostrando formulario de creación de usuario.");
//        //    return View();
//        //}

//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //[RoleAuthorize("Editor", "Admin")]
//        //public ActionResult Create(UserAccount newUserAccount)
//        //{
//        //    try
//        //    {
//        //        if (ModelState.IsValid)
//        //        {
//        //            LogHelper.LogInformation("Creando nuevo usuario.");
//        //            var createdUser = _proxy.CreateUserAccount(newUserAccount);
//        //            if (createdUser != null)
//        //            {
//        //                TempData["SuccessMessage"] = "Usuario creado exitosamente.";
//        //                return RedirectToAction("List");
//        //            }
//        //            else
//        //            {
//        //                ModelState.AddModelError("", "Error al crear el usuario.");
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        LogHelper.LogError("Error al crear usuario.", ex);
//        //        ModelState.AddModelError("", ex.Message);
//        //    }
//        //    return View(newUserAccount);
//        //}

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(UserAccount newUserAccount)
//        {
//            try
//            {
//                // Registro del valor recibido
//                LogHelper.LogInformation($"Estado recibido: {newUserAccount.Status}");

//                // Forzar un valor válido temporalmente
//                newUserAccount.Status = "Active";

//                if (string.IsNullOrEmpty(newUserAccount.Status) ||
//                    (newUserAccount.Status != "Active" && newUserAccount.Status != "Inactive"))
//                {
//                    ModelState.AddModelError("Status", "El estado debe ser 'Active' o 'Inactive'.");
//                    return View(newUserAccount);
//                }

//                // Registro antes de enviar al proxy
//                LogHelper.LogInformation($"Estado enviado al proxy: {newUserAccount.Status}");

//                // Lógica para guardar el usuario
//                var createdUser = _proxy.CreateUserAccount(newUserAccount);

//                if (createdUser != null)
//                {
//                    TempData["SuccessMessage"] = "Usuario creado exitosamente.";
//                    return RedirectToAction("List");
//                }
//                else
//                {
//                    ModelState.AddModelError("", "Error al crear el usuario.");
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al crear usuario.", ex);
//                ModelState.AddModelError("", ex.Message);
//            }

//            return View(newUserAccount);
//        }

//        // Editar usuario
//        [HttpGet]
//        //[RoleAuthorize("Editor", "Admin")]
//        public ActionResult Edit(int id)
//        {
//            try
//            {
//                LogHelper.LogInformation($"Consultando usuario con ID: {id}.");
//                var user = _proxy.GetUserAccountByID(id);
//                if (user == null)
//                {
//                    LogHelper.LogWarning($"No se encontró el usuario con ID: {id}.");
//                    return HttpNotFound();
//                }
//                return View(user);
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al consultar usuario para edición.", ex);
//                ViewBag.ErrorMessage = "No se pudo cargar el usuario.";
//                return View("Error");
//            }
//        }

//        //[HttpPost]
//        //[ValidateAntiForgeryToken]
//        //[RoleAuthorize("Editor", "Admin")]
//        //public ActionResult Edit(int id, UserAccount updatedUserAccount)
//        //{
//        //    if (ModelState.IsValid)
//        //    {
//        //        try
//        //        {
//        //            updatedUserAccount.UserID = id;
//        //            LogHelper.LogInformation($"Actualizando usuario con ID: {id}.");
//        //            var result = _proxy.UpdateUserAccount(updatedUserAccount);
//        //            if (result)
//        //            {
//        //                TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
//        //                return RedirectToAction("List");
//        //            }
//        //            else
//        //            {
//        //                ModelState.AddModelError("", "Error al actualizar el usuario.");
//        //            }
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            LogHelper.LogError("Error al actualizar usuario.", ex);
//        //            ModelState.AddModelError("", ex.Message);
//        //        }
//        //    }
//        //    return View(updatedUserAccount);
//        //}

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        //[RoleAuthorize("Editor", "Admin")]
//        public ActionResult Edit(int id, UserAccount updatedUserAccount)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    updatedUserAccount.UserID = id;

//                    // Validar el estado
//                    if (updatedUserAccount.Status != "Active" && updatedUserAccount.Status != "Inactive")
//                    {
//                        ModelState.AddModelError("Status", "El estado debe ser 'Active' o 'Inactive'.");
//                        return View(updatedUserAccount);
//                    }

//                    LogHelper.LogInformation($"Actualizando usuario con ID: {id}.");
//                    var result = _proxy.UpdateUserAccount(updatedUserAccount);

//                    if (result)
//                    {
//                        TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
//                        return RedirectToAction("List");
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "Error al actualizar el usuario.");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    LogHelper.LogError("Error al actualizar usuario.", ex);
//                    ModelState.AddModelError("", ex.Message);
//                }
//            }

//            return View(updatedUserAccount);
//        }


//        // Eliminar usuario
//        [HttpGet]
//        //[RoleAuthorize("Admin")]
//        public ActionResult Delete(int id)
//        {
//            try
//            {
//                LogHelper.LogInformation($"Consultando usuario con ID: {id} para eliminación.");
//                var user = _proxy.GetUserAccountByID(id);
//                if (user == null)
//                {
//                    LogHelper.LogWarning($"No se encontró el usuario con ID: {id}.");
//                    return HttpNotFound();
//                }
//                return View(user);
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al consultar usuario para eliminación.", ex);
//                ViewBag.ErrorMessage = "No se pudo cargar el usuario.";
//                return View("Error");
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        //[RoleAuthorize("Admin")]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            try
//            {
//                LogHelper.LogInformation($"Eliminando usuario con ID: {id}.");
//                var result = _proxy.DeleteUserAccount(id);
//                if (result)
//                {
//                    TempData["SuccessMessage"] = "Usuario eliminado exitosamente.";
//                }
//                else
//                {
//                    TempData["ErrorMessage"] = "No se pudo eliminar el usuario.";
//                }
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al eliminar usuario.", ex);
//                TempData["ErrorMessage"] = $"Error al eliminar el usuario: {ex.Message}";
//            }
//            return RedirectToAction("List");
//        }

//        // Recuperar contraseña
//        [HttpGet]
//        public ActionResult ForgotPassword()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<ActionResult> ForgotPassword(string email)
//        {
//            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
//            {
//                return Json(new { success = false, message = "Correo electrónico inválido." });
//            }

//            try
//            {
//                LogHelper.LogInformation($"Iniciando recuperación de contraseña para el correo: {email}.");
//                var user = _proxy.GetUserByEmail(email);
//                if (user == null)
//                {
//                    return Json(new { success = false, message = "No se encontró ningún usuario con ese correo." });
//                }

//                var random = new Random();
//                string securityCode = random.Next(1000, 9999).ToString();
//                string subject = "Recuperación de contraseña";
//                string body = $"Tu código de verificación es: {securityCode}";

//                await _emailService.SendEmailAsync(email, subject, body);

//                Session["SecurityCode"] = securityCode;
//                Session["UserIdToReset"] = user.UserID;

//                // Redirigir a la vista de verificación del código
//                return RedirectToAction("VerifyCodeAndChangePassword", "UserAccount");
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error durante la recuperación de contraseña.", ex);
//                return Json(new { success = false, message = $"Error al enviar el correo: {ex.Message}" });
//            }
//        }

//        [HttpGet]
//        public ActionResult VerifyCodeAndChangePassword()
//        {
//            // Aquí se puede agregar lógica adicional si es necesario, pero generalmente solo se devuelve la vista.
//            return View();
//        }


//        [HttpPost]
//        public ActionResult VerifyResetCode(string enteredCode)
//        {
//            var storedCode = Session["SecurityCode"] as string;
//            var userIdToReset = Session["UserIdToReset"] as int?;

//            if (storedCode == enteredCode && userIdToReset.HasValue)
//            {
//                return Json(new { success = true, userId = userIdToReset.Value });
//            }

//            return Json(new { success = false, message = "El código ingresado es incorrecto." });
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult EditLog(UserAccount userAccount)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    // Lógica para actualizar el usuario
//                    var result = _proxy.UpdateUserAccount(userAccount);
//                    if (result)
//                    {
//                        TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
//                        return RedirectToAction("List");
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "No se pudo actualizar el usuario.");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError("", "Ocurrió un error: " + ex.Message);
//                }
//            }
//            return View(userAccount);
//        }

//        public ActionResult AddUser()
//        {
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult AddUser(UserAccount newUserAccount)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    // Lógica para crear el usuario
//                    var createdUser = _proxy.CreateUserAccount(newUserAccount);
//                    if (createdUser != null)
//                    {
//                        // Mensaje de éxito y redirección al login
//                        TempData["SuccessMessage"] = "Usuario creado exitosamente. Por favor inicie sesión.";
//                        return RedirectToAction("Login", "Account"); // Especifica el controlador "Account" donde está el login
//                    }
//                    else
//                    {
//                        ModelState.AddModelError("", "No se pudo crear el usuario.");
//                    }
//                }
//                catch (Exception ex)
//                {
//                    ModelState.AddModelError("", "Error interno: " + ex.Message);
//                }
//            }

//            // Si no es válido o hay errores, vuelve a mostrar el formulario
//            return View(newUserAccount);
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        //[RoleAuthorize("Editor", "Admin")]
//        public ActionResult CreateUser(UserAccount newUserAccount)
//        {
//            try
//            {
//                // Verificar si el correo ha sido verificado
//                if (Session["IsEmailVerified"] == null || !(bool)Session["IsEmailVerified"] || newUserAccount.Email != Session["EmailToVerify"]?.ToString())
//                {
//                    return Json(new { success = false, message = "El correo no ha sido verificado." });
//                }

//                if (ModelState.IsValid)
//                {
//                    LogHelper.LogInformation($"Creando nuevo usuario: {newUserAccount.UserName}");

//                    // Llamar al proxy para crear el usuario
//                    var createdUser = _proxy.CreateUserAccount(newUserAccount);

//                    if (createdUser != null)
//                    {
//                        // Limpiar sesión tras creación exitosa
//                        Session.Remove("IsEmailVerified");
//                        Session.Remove("EmailToVerify");

//                        TempData["SuccessMessage"] = "Usuario creado exitosamente.";
//                        return Json(new { success = true, redirectUrl = Url.Action("List", "UserAccount") });
//                    }
//                }

//                // Si la validación falla
//                ModelState.AddModelError("", "Error al crear el usuario.");
//                return Json(new { success = false, message = "Error al crear el usuario. Verifique los datos ingresados." });
//            }
//            catch (Exception ex)
//            {
//                LogHelper.LogError("Error al crear el usuario.", ex);
//                return Json(new { success = false, message = "Ocurrió un error interno. Intente nuevamente más tarde." });
//            }
//        }

//        private bool IsValidEmail(string email)
//        {
//            try
//            {
//                var addr = new System.Net.Mail.MailAddress(email);
//                return addr.Address == email;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}

//TERCERA VERSION

using ProyectoProxyService;
using Entities;
using SL.Logger;
using SL.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Proyecto.MVCPLS.Filters;

namespace Proyecto.MVCPLS.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly Proxy _proxy;
        private readonly IEmailService _emailService;

        public UserAccountController()
        {
            _proxy = new Proxy();
            _emailService = new EmailService();
        }

        // Listar usuarios
        [HttpGet]
        //[RoleAuthorize("Viewer", "Editor", "Admin")]
        public ActionResult List()
        {
            try
            {
                LogHelper.LogInformation("Consultando lista de usuarios.");
                var users = _proxy.GetAllUserAccounts();
                if (users == null || !users.Any())
                {
                    LogHelper.LogWarning("No hay usuarios disponibles.");
                    ViewBag.Message = "No hay usuarios registrados.";
                }
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
                return View(users);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al consultar usuarios.", ex);
                ViewBag.ErrorMessage = "No se pudieron cargar los usuarios.";
                return View("Error");
            }
        }

        // Crear usuario
        //[RoleAuthorize("Editor", "Admin")]
        //public ActionResult Create()
        //{
        //    LogHelper.LogInformation("Mostrando formulario de creación de usuario.");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[RoleAuthorize("Editor", "Admin")]
        //public ActionResult Create(UserAccount newUserAccount)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            LogHelper.LogInformation("Creando nuevo usuario.");
        //            var createdUser = _proxy.CreateUserAccount(newUserAccount);
        //            if (createdUser != null)
        //            {
        //                TempData["SuccessMessage"] = "Usuario creado exitosamente.";
        //                return RedirectToAction("List");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Error al crear el usuario.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogError("Error al crear usuario.", ex);
        //        ModelState.AddModelError("", ex.Message);
        //    }
        //    return View(newUserAccount);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserAccount newUserAccount)
        {
            try
            {
                // Registro del valor recibido
                LogHelper.LogInformation($"Estado recibido: {newUserAccount.Status}");

                // Forzar un valor válido temporalmente
                newUserAccount.Status = "Active";

                if (string.IsNullOrEmpty(newUserAccount.Status) ||
                    (newUserAccount.Status != "Active" && newUserAccount.Status != "Inactive"))
                {
                    ModelState.AddModelError("Status", "El estado debe ser 'Active' o 'Inactive'.");
                    return View(newUserAccount);
                }

                // Registro antes de enviar al proxy
                LogHelper.LogInformation($"Estado enviado al proxy: {newUserAccount.Status}");

                // Lógica para guardar el usuario
                var createdUser = _proxy.CreateUserAccount(newUserAccount);

                if (createdUser != null)
                {
                    TempData["SuccessMessage"] = "Usuario creado exitosamente.";
                    return RedirectToAction("List");
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el usuario.");
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al crear usuario.", ex);
                ModelState.AddModelError("", ex.Message);
            }

            return View(newUserAccount);
        }

        // Editar usuario
        [HttpGet]
        //[RoleAuthorize("Editor", "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                LogHelper.LogInformation($"Consultando usuario con ID: {id}.");
                var user = _proxy.GetUserAccountByID(id);
                if (user == null)
                {
                    LogHelper.LogWarning($"No se encontró el usuario con ID: {id}.");
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al consultar usuario para edición.", ex);
                ViewBag.ErrorMessage = "No se pudo cargar el usuario.";
                return View("Error");
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[RoleAuthorize("Editor", "Admin")]
        //public ActionResult Edit(int id, UserAccount updatedUserAccount)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            updatedUserAccount.UserID = id;
        //            LogHelper.LogInformation($"Actualizando usuario con ID: {id}.");
        //            var result = _proxy.UpdateUserAccount(updatedUserAccount);
        //            if (result)
        //            {
        //                TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
        //                return RedirectToAction("List");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Error al actualizar el usuario.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.LogError("Error al actualizar usuario.", ex);
        //            ModelState.AddModelError("", ex.Message);
        //        }
        //    }
        //    return View(updatedUserAccount);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[RoleAuthorize("Editor", "Admin")]
        public ActionResult Edit(int id, UserAccount updatedUserAccount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    updatedUserAccount.UserID = id;

                    // Validar el estado
                    if (updatedUserAccount.Status != "Active" && updatedUserAccount.Status != "Inactive")
                    {
                        ModelState.AddModelError("Status", "El estado debe ser 'Active' o 'Inactive'.");
                        return View(updatedUserAccount);
                    }

                    LogHelper.LogInformation($"Actualizando usuario con ID: {id}.");
                    var result = _proxy.UpdateUserAccount(updatedUserAccount);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                        return RedirectToAction("List");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error al actualizar el usuario.");
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogError("Error al actualizar usuario.", ex);
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(updatedUserAccount);
        }


        // Eliminar usuario
        [HttpGet]
        //[RoleAuthorize("Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                LogHelper.LogInformation($"Consultando usuario con ID: {id} para eliminación.");
                var user = _proxy.GetUserAccountByID(id);
                if (user == null)
                {
                    LogHelper.LogWarning($"No se encontró el usuario con ID: {id}.");
                    return HttpNotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al consultar usuario para eliminación.", ex);
                ViewBag.ErrorMessage = "No se pudo cargar el usuario.";
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
                LogHelper.LogInformation($"Eliminando usuario con ID: {id}.");
                var result = _proxy.DeleteUserAccount(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Usuario eliminado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar el usuario.";
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al eliminar usuario.", ex);
                TempData["ErrorMessage"] = $"Error al eliminar el usuario: {ex.Message}";
            }
            return RedirectToAction("List");
        }

        // Recuperar contraseña
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
            {
                return Json(new { success = false, message = "Correo electrónico inválido." });
            }

            try
            {
                LogHelper.LogInformation($"Iniciando recuperación de contraseña para el correo: {email}.");
                var user = _proxy.GetUserByEmail(email);
                if (user == null)
                {
                    return Json(new { success = false, message = "No se encontró ningún usuario con ese correo." });
                }

                var random = new Random();
                string securityCode = random.Next(1000, 9999).ToString();
                string subject = "Recuperación de contraseña";
                string body = $"Tu código de verificación es: {securityCode}";

                await _emailService.SendEmailAsync(email, subject, body);

                Session["SecurityCode"] = securityCode;
                Session["UserIdToReset"] = user.UserID;

                // Redirigir a la vista de verificación del código
                return RedirectToAction("VerifyCodeAndChangePassword", "UserAccount");
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error durante la recuperación de contraseña.", ex);
                return Json(new { success = false, message = $"Error al enviar el correo: {ex.Message}" });
            }
        }

        [HttpGet]
        public ActionResult VerifyCodeAndChangePassword()
        {
            // Aquí se puede agregar lógica adicional si es necesario, pero generalmente solo se devuelve la vista.
            return View();
        }


        [HttpPost]
        public ActionResult VerifyResetCode(string enteredCode)
        {
            var storedCode = Session["SecurityCode"] as string;
            var userIdToReset = Session["UserIdToReset"] as int?;

            if (storedCode == enteredCode && userIdToReset.HasValue)
            {
                return Json(new { success = true, userId = userIdToReset.Value });
            }

            return Json(new { success = false, message = "El código ingresado es incorrecto." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLog(UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lógica para actualizar el usuario
                    var result = _proxy.UpdateUserAccount(userAccount);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                        return RedirectToAction("List");
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo actualizar el usuario.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error: " + ex.Message);
                }
            }
            return View(userAccount);
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(UserAccount newUserAccount)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lógica para crear el usuario
                    var createdUser = _proxy.CreateUserAccount(newUserAccount);
                    if (createdUser != null)
                    {
                        TempData["SuccessMessage"] = "Usuario creado exitosamente.";
                        return RedirectToAction("List");
                    }
                    else
                    {
                        ModelState.AddModelError("", "No se pudo crear el usuario.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error interno: " + ex.Message);
                }
            }
            return View(newUserAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[RoleAuthorize("Editor", "Admin")]
        public ActionResult CreateUser(UserAccount newUserAccount)
        {
            try
            {
                // Verificar si el correo ha sido verificado
                if (Session["IsEmailVerified"] == null || !(bool)Session["IsEmailVerified"] || newUserAccount.Email != Session["EmailToVerify"]?.ToString())
                {
                    return Json(new { success = false, message = "El correo no ha sido verificado." });
                }

                if (ModelState.IsValid)
                {
                    LogHelper.LogInformation($"Creando nuevo usuario: {newUserAccount.UserName}");

                    // Llamar al proxy para crear el usuario
                    var createdUser = _proxy.CreateUserAccount(newUserAccount);

                    if (createdUser != null)
                    {
                        // Limpiar sesión tras creación exitosa
                        Session.Remove("IsEmailVerified");
                        Session.Remove("EmailToVerify");

                        TempData["SuccessMessage"] = "Usuario creado exitosamente.";
                        return Json(new { success = true, redirectUrl = Url.Action("List", "UserAccount") });
                    }
                }

                // Si la validación falla
                ModelState.AddModelError("", "Error al crear el usuario.");
                return Json(new { success = false, message = "Error al crear el usuario. Verifique los datos ingresados." });
            }
            catch (Exception ex)
            {
                LogHelper.LogError("Error al crear el usuario.", ex);
                return Json(new { success = false, message = "Ocurrió un error interno. Intente nuevamente más tarde." });
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}