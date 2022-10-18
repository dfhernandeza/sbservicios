using AppWebSantaBeatriz.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

using System;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using static BibliotecaDeClases.ProcesadorEmpleados;

[assembly: OwinStartupAttribute(typeof(AppWebSantaBeatriz.Startup))]
namespace AppWebSantaBeatriz
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
           
            //createRolesandUsers();
            //Algo();
        }
        //private void Algo()
        //{
        //    var mail = new MailMessage();
        //    var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        //    var smtp = new SmtpClient(smtpSection.Network.Host, smtpSection.Network.Port);
        //    smtp.DeliveryMethod = smtpSection.DeliveryMethod;
        //    smtp.UseDefaultCredentials = false;
        //    smtp.Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
        //    string username = smtpSection.Network.UserName;
        //    mail.IsBodyHtml = true;
        //    mail.From = new MailAddress(username);
        //    mail.To.Add("davhernand@gmail.com");
        //    mail.Subject = "Hola";
        //    mail.Body = CorreoHtml();

        //    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
        //    smtp.Send(mail);

        //}
        //public static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    return true;
        //}

        //private void createRolesandUsers()
        //{
        //    ApplicationDbContext context = new ApplicationDbContext();

        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        //    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


        //    // In Startup iam creating first Admin Role and creating a default Admin User     

        //        //// first we create Admin rool    
        //        //var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        //        //role.Name = "Admin";
        //        //roleManager.Create(role);

        //        //Here we create a Admin super user who will maintain the website                   

        //        //var user = new ApplicationUser();
        //        //user.UserName = "admin@sbservicios.cl";
        //        //user.Email = "admin@sbservicios.cl";

        //        //string userPWD = "Santabeatriz1+";

        //        //var chkUser = UserManager.Create(user, userPWD);

        //        ////Add default User to Role Admin    
        //        //if (chkUser.Succeeded)
        //        //{
        //        //    var result1 = UserManager.AddToRole(user.Id, "Admin");

        //        //}



        //    //// creating Creating Manager role     
        //    //if (!roleManager.RoleExists("Manager"))
        //    //{
        //    //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        //    //    role.Name = "Manager";
        //    //    roleManager.Create(role);

        //    //}

        //    //// creating Creating Employee role     
        //    //if (!roleManager.RoleExists("Employee"))
        //    //{
        //    //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
        //    //    role.Name = "Employee";
        //    //    roleManager.Create(role);

        //    //}
        //}
    }
}