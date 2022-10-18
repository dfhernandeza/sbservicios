using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using AppWebSantaBeatriz.Models;
using System.Net.Mail;
using System.Net.Configuration;
using System.Configuration;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace AppWebSantaBeatriz
{
    public class EmailService : IIdentityMessageService
    {
        public  Task SendAsync(IdentityMessage message)
        {
            
            var mail = new MailMessage();
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("mailSettings/smtp_1");
            var smtp = new SmtpClient(smtpSection.Network.Host, smtpSection.Network.Port);
            smtp.DeliveryMethod = smtpSection.DeliveryMethod;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
            string username = smtpSection.Network.UserName;
            mail.IsBodyHtml = true;
            mail.From = new MailAddress(username);
            mail.To.Add(message.Destination);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            //mail.Attachments(new Attachment())
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
            smtp.Send(mail);

            // Conecte su servicio de correo electrónico aquí para enviar correo electrónico.
            return Task.FromResult(0);
        }

        public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
   
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Conecte el servicio SMS aquí para enviar un mensaje de texto.
            return Task.FromResult(0);
        }
    }

    // Configure el administrador de usuarios de aplicación que se usa en esta aplicación. UserManager se define en ASP.NET Identity y se usa en la aplicación.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure la lógica de validación de nombres de usuario
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure la lógica de validación de contraseñas
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
                
            };

            // Configurar valores predeterminados para bloqueo de usuario
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Registre proveedores de autenticación en dos fases. Esta aplicación usa los pasos Teléfono y Correo electrónico para recibir un código para comprobar el usuario
            // Puede escribir su propio proveedor y conectarlo aquí.
            manager.RegisterTwoFactorProvider("Código telefónico", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Su código de seguridad es {0}"
            });
            manager.RegisterTwoFactorProvider("Código de correo electrónico", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Código de seguridad",
                BodyFormat = "Su código de seguridad es {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure el administrador de inicios de sesión que se usa en esta aplicación.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
