using Autofac;
using Autofac.Integration.Mvc;
using IssueTracker.Data;
using System;
using System.Web.Mvc;

namespace IssueTracker
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            // Register our Data dependencies
            builder.RegisterModule(new DataModule("MVCWithAutofacDB"));

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}