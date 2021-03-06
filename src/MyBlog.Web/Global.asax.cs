﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using FluentValidation.Mvc;
using MyBlog.Core.Contracts;
using MyBlog.Core.Entities;
using MyBlog.Core.Models;
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
                "PostsByTag",
                "blog/tag/{name}",
                new { controller = "Tags", action = "ByName" });

            routes.MapRoute(
                "BlogMain", // Route name
                "blog",
                new { controller = "Posts", action = "List" } // Parameter defaults
            );

            routes.MapRoute("Default",
                "{controller}/{action}/{id}",
                new { controller = "Posts", action = "List", id = UrlParameter.Optional });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var resolver = new StructureMapDependencyResolver(new MvcRegistry());
            DependencyResolver.SetResolver(resolver);

            var allStartables = resolver.GetServices<IStartable>().ToList();
            allStartables.ForEach(x => x.Init());

            FluentValidationModelValidatorProvider.Configure(cfg =>
            {
                cfg.ValidatorFactory = new StructureMapValidatorFactory(new FluentValidationRegistry());
            });

            ConfigureAutoMapper();
        }

        void ConfigureAutoMapper()
        {
            Mapper.CreateMap<Core.Models.TagModel, Models.TagModel>();
            Mapper.CreateMap<PostReadModel, PostModel>()
                .ForMember(x => x.DisqusShortName, mo => mo.MapFrom(prm => ConfigurationManager.AppSettings["DisqusShortName"]));
            Mapper.CreateMap<BlogPost, PostModel>();
            Mapper.CreateMap<BlogPost, PostReadModel>()
                .ForMember(x => x.Tags, mo => mo.Ignore())
                .ForMember(x => x.Created, mo => mo.MapFrom(bp => bp.Created.ToDateTimeUtc()));
            Mapper.CreateMap<IEnumerable<PostReadModel>, PostListModel>()
                  .ForMember(x => x.Posts, x => x.MapFrom(t => t));
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