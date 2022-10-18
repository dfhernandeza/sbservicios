using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AppWebSantaBeatriz.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Security;
using static BibliotecaDeClases.ProcesadorEmpleados;
using static BibliotecaDeClases.Logica.AccesoBlobs.AccesoBlobs;
using BibliotecaDeClases;
using Microsoft.AspNet.Identity.EntityFramework;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;

namespace AppWebSantaBeatriz.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if((System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // No cuenta los errores de inicio de sesión para el bloqueo de la cuenta
            // Para permitir que los errores de contraseña desencadenen el bloqueo de la cuenta, cambie a shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
               
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Requerir que el usuario haya iniciado sesión con nombre de usuario y contraseña o inicio de sesión externo
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // El código siguiente protege de los ataques por fuerza bruta a los códigos de dos factores. 
            // Si un usuario introduce códigos incorrectos durante un intervalo especificado de tiempo, la cuenta del usuario 
            // se bloqueará durante un período de tiempo especificado. 
            // Puede configurar el bloqueo de la cuenta en IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Código no válido.");
                    return View(model);
            }
        }

        ApplicationDbContext context = new ApplicationDbContext();
      
        // GET: /Account/Register
        //[AllowAnonymous]
        public ActionResult Register(string username, int idempleado)
        {
           if (User.IsInRole("Administrador"))
            {
                RegisterViewModel modelo = new RegisterViewModel
                {
                    UserName = username,
                    Email = username,
                    IDEmpleado = idempleado
                    
                };
                ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
                return View(modelo);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           

        }


        //public async Task<ActionResult> CorreoNuevoUsuario()
        //{

        //    throw new NotImplementedException();
        //}

        //
        // POST: /Account/Register
        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                Random random = new Random();         
                var pass = Membership.GeneratePassword(8, 1) + random.Next().ToString() + "Pp";
                var result = await UserManager.CreateAsync(user, pass);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    await ActualizaIDEmpleado(model.IDEmpleado,user.Id);
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
                    // Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771
                    // Enviar correo electrónico con este vínculo

                   

                    return RedirectToAction("VerUsuarios", "Account");
                }
                //ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin"))
                //                  .ToList(), "Name", "Name");
                AddErrors(result);
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
            return View(model);
        }

        public async Task correoConfirmarPassAsync(string userid)
        {
           var emp = await getNombreUsuarioAsync(userid);
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userid);
            string passwordSetCode = await UserManager.GeneratePasswordResetTokenAsync(userid);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = User, code = code, passwordSetCode = passwordSetCode }, protocol: Request.Url.Scheme);
            string html = await CorreoHtml(callbackUrl.ToString(), emp.Nombre);
            await UserManager.SendEmailAsync(userid, "Confirmar cuenta", html);

        }

        public async Task correoRestablecerPassAsync(string userid)
        {
            var emp = await getNombreUsuarioAsync(userid);            
            string passwordSetCode = await UserManager.GeneratePasswordResetTokenAsync(userid);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = userid, code = passwordSetCode, firstPassword = true }, protocol: Request.Url.Scheme);
            string html = await CorreoHtmlReset(callbackUrl.ToString(), emp.Nombre);
            await UserManager.SendEmailAsync(userid, "Restablecer contraseña", html);
        }

       
        public async Task<JsonResult> correoLiquidacionAsync(string iddoc, string idusuario)
        {
            
            //var code = await UserManager.GenerateUserTokenAsync("FirmarLiquidacion", idusuario);
            //var downloadcode = await UserManager.GenerateUserTokenAsync("DescargarLiquidacion", userId: idusuario);
            var callbackUrl = Url.Action("FirmarLiquidacion", "Account", new { userId = idusuario,/* code = code,*/ iddoc = iddoc}, protocol: Request.Url.Scheme);
            var linkdescarga = Url.Action("DownLoadFile", "Account", new { iddoc = iddoc/*, usuario = idusuario, codigo = downloadcode*/ }, protocol: Request.Url.Scheme);
            string html = await htmlcorreoLiquidacionAsync(callbackUrl, iddoc, linkdescarga);
            await UserManager.SendEmailAsync(idusuario, "Liquidación", html);
            await updateFchaEnvioAsync(iddoc);
            return Json("Enviado");
        }

        public async Task<JsonResult> correoActualizacionDatosAsync(int idempleado)
        {
            var iduser = await getUserAsync(idempleado);
            var codigo = await getCodEmpleadoAsync(idempleado);
            var callbackUrl = Url.Action("ActualizacionDatos", "Empleados", new { idempleado = idempleado, cod = codigo.Cod }, protocol: Request.Url.Scheme);
           
            string html = await htmlcorreoActualizarDatosAsync(callbackUrl,idempleado );
            await UserManager.SendEmailAsync(iduser, "Actualizar Datos Personales", html);
            await onoffActualizadoAsync(new BibliotecaDeClases.EmpleadoModel {ID = idempleado, Actualizado = false });
            await actualizacionFechaCorreoAsync(idempleado);
            return Json("Enviado");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> DownLoadFile(string iddoc/*, string usuario, string codigo*/)
        {
            //var tokencorrect = await UserManager.VerifyUserTokenAsync(usuario, "DescargarLiquidacion", codigo);
            //if (tokencorrect)
            //{
                var data = await CargarLiquidacionAsync(iddoc, "liquidaciones");


                 return File(data, "application/pdf");
            //}
            //else
            //{
            //    return RedirectToAction("Error");
            //}

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> FirmarLiquidacion(string userId,/* string code,*/ string iddoc)
        {
            //if (userId == null || code == null)
            //{
            //    return RedirectToAction("Error", new { mensaje = "Acceso no autorizado." });
            //}
            
            //var iscorrect = await UserManager.VerifyUserTokenAsync(userId, "FirmarLiquidacion", code);
            //if (iscorrect)
            //{
                var firmado = await firmadoAsync(iddoc);
                if(firmado == "true") { return RedirectToAction("Error", new { mensaje = "Este documento ya ha sido firmado." }); }
                //var codigo = await UserManager.GenerateUserTokenAsync("ConfirmarFirma", userId);
                var nombre = await getUsuarioAsync(userId);
                FirmaLiquidacionModel model = new FirmaLiquidacionModel { ID = iddoc,IDUsuario = userId, /*Code = codigo,*/ Usuario = nombre };
                return View(model);
            //}
            //else
            //{
            //    return RedirectToAction("Error",new {mensaje = "El enlace ya no es válido." });
            //}
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> FirmaLiquidacion(FirmaLiquidacionModel model)
        {
           
             //var iscorrect = await UserManager.VerifyUserTokenAsync(model.IDUsuario, "ConfirmarFirma", model.Code);
            //if (iscorrect)
            //{
                await firmarLiquidacion(model.ID);

                return Json("Exito", JsonRequestBehavior.AllowGet);

            //}

            //else
            //{
            //    return Json("Error", JsonRequestBehavior.AllowGet);
            //}
          
        }

        public  ActionResult NuevoRol()
        {

            return View();
        }
        [HttpPost]
        public ActionResult NuevoRol(RolesModel modelo)
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var role = new IdentityRole();
            role.Name = modelo.Rol;
            roleManager.Create(role);
            
            return RedirectToAction("VerRoles");
        }

        public ActionResult EditarRol(string id)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
           var rol = roleManager.FindById(id);
            RolesModel modelo = new RolesModel { Id = rol.Id, Rol = rol.Name };
            return View(modelo);
        }
        [HttpPost]
        public async Task<ActionResult> EditarRol(RolesModel modelo)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var rol = new IdentityRole { Id = modelo.Id, Name = modelo.Rol };
          await  roleManager.UpdateAsync(rol);

            return RedirectToAction("VerRoles");
        }



        public ActionResult VerRoles()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var data = roleManager.Roles.ToList();
            List<RolesModel> modelo = new List<RolesModel>();
            data.ForEach(x =>
            {
                modelo.Add(new RolesModel { Id = x.Id, Rol = x.Name });

            });
            return View(modelo);
        }




        public JsonResult GeneraPassword()
        {
            return Json(Membership.GeneratePassword(8, 1));
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Error(string mensaje)
        {
            Models.Error.ErrorModel modelo = new Models.Error.ErrorModel { Mensaje = mensaje };
            return View(modelo);
           
        }
        [HttpGet]
        public ActionResult VerUsuarios()
        {
            List<UsuariosViewModel> model = new List<UsuariosViewModel>();

            string sql = @"Select distinct AspNetUsers.Id, AspNetUsers.Email, AspNetUsers.UserName, AspNetRoles.Name, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado from                                      AspNetUsers inner join AspNetUserRoles on AspNetUserRoles.UserId = AspNetUsers.Id 
						inner join AspNetRoles on AspNetRoles.Id = AspNetUserRoles.RoleId
                        inner join PersonalServicios on PersonalServicios.IdUsuario = AspNetUsers.id  order by Empleado asc";

            var listo = context.Database.SqlQuery<UsuariosViewModel>(sql);
            
            return View(listo.ToList());
        }

        [HttpGet]
        public ActionResult EliminarUsuario(string userId)
        {
            string sql = "Select distinct AspNetUsers.Id, AspNetUsers.Email, AspNetUsers.UserName, AspNetRoles.Name from AspNetUsers " +
              "inner join AspNetUserRoles on AspNetUserRoles.UserId = AspNetUserRoles.UserId " +
              "inner join AspNetRoles on AspNetRoles.Id = AspNetUserRoles.RoleId where AspNetUsers.Id = @ID";

            return View(context.Database.SqlQuery<UsuariosViewModel>(sql, new SqlParameter("@ID", userId)).FirstOrDefault());

        }

        [HttpPost]
        public ActionResult EliminarUsuario(UsuariosViewModel model)
        {
            //var user = new ApplicationUser {Id = model.Id };
            UserManager.RemoveFromRole(model.Id, model.Name);
            var y = UserManager.Users.Where(x => x.Id == model.Id).FirstOrDefault();
            UserManager.Delete(y);

            return RedirectToAction("VerUsuarios", "Account");
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code, string passwordSetCode = null)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded && !string.IsNullOrEmpty(passwordSetCode))
            {
                return RedirectToAction("ResetPassword", "Account", new { userId = userId, code = passwordSetCode, firstPassword = true });
            }
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // No revelar que el usuario no existe o que no está confirmado
                    return View("ForgotPasswordConfirmation");
                }

                // Para obtener más información sobre cómo habilitar la confirmación de cuentas y el restablecimiento de contraseña, visite https://go.microsoft.com/fwlink/?LinkID=320771
                // Enviar correo electrónico con este vínculo
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Restablecer contraseña", "Para restablecer la contraseña, haga clic <a href=\"" + callbackUrl + "\">aquí</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Si llegamos a este punto, es que se ha producido un error y volvemos a mostrar el formulario
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // No revelar que el usuario no existe
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Solicitar redireccionamiento al proveedor de inicio de sesión externo
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generar el token y enviarlo
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Si el usuario ya tiene un inicio de sesión, iniciar sesión del usuario con este proveedor de inicio de sesión externo
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Si el usuario no tiene ninguna cuenta, solicitar que cree una
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Obtener datos del usuario del proveedor de inicio de sesión externo
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Aplicaciones auxiliares
        // Se usa para la protección XSRF al agregar inicios de sesión externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}