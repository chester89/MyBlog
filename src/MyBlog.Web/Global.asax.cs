using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using FluentValidation.Mvc;
using MyBlog.Core.Entities;
using MyBlog.Infrastructure;
using MyBlog.Web.Models;

namespace MyBlog.Web
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              "PostSlug", // Route name
              "blog/{slug}", // URL with parameters
              new { controller = "Posts", action = "Default" } // Parameter defaults
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Default", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(new MvcRegistry()));

            FluentValidationModelValidatorProvider.Configure(cfg =>
            {
                cfg.ValidatorFactory = new StructureMapValidatorFactory(new FluentValidationRegistry());
            });

            ConfigureAutoMapper();
        }

        void ConfigureAutoMapper()
        {
            Mapper.CreateMap<BlogPost, PostModel>()
                .ForMember(x => x.Created, mo => mo.MapFrom(t => t.Created.ToDateTimeUtc()));
        }

        protected void Application_EndRequest()
        {
            dynamic resolver = DependencyResolver.Current;
            if (resolver is StructureMapDependencyResolver)
            {
                resolver.DisposeOfHttpCachedObjects();
            }
        }
    }
}