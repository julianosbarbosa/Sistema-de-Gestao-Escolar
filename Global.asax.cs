using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SistemaControleAlunos.Classes;

namespace SistemaControleAlunos
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //4º step
        // inicializa o migration com Database.SetInitializer(new MigrateDatabaseToLatestVersion<Models.ControleContext, Migrations.Configuration>());
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Models.ControleContext, Migrations.Configuration>());
            this.CheckRoles();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //16º step
        // criar o metodo CheckRoles, chama dentro de utilidades chame o metodo CheckRole e passe os parametros "Admin",  "Professor", "Estudante" e chame o metodo checkroles dentro de  Application_Start()
        private void CheckRoles()
        {
            Utilidades.CheckRole("Admin");
            Utilidades.CheckRole("Professor");
            Utilidades.CheckRole("Estudante");
        }
    }
}
