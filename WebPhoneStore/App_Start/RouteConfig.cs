using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebPhoneStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Product Category",
                url: "san-pham/{SeoTitle}-{cateid}",
                defaults: new { controller = "Product", action = "Category" },
                namespaces: new[] { "WebPhoneStore.Controllers" }
            );

            routes.MapRoute(
                name: "Product Detail",
                url: "chi-tiet/{SeoTitle}-{id}",
                defaults: new { controller = "Product", action = "Detail" },
                namespaces: new[] { "WebPhoneStore.Controllers" }
            );

            routes.MapRoute(
                name: "Add Cart",
                url: "them-gio-hang/{id}",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "WebPhoneStore.Controllers" }
            );

            routes.MapRoute(
                name: "Payment Success",
                url: "hoan-thanh",
                defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
                namespaces: new[] { "WebPhoneStore.Controllers" }
            );

            routes.MapRoute(
                name: "Home",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "WebPhoneStore.Controllers" }
            );

            routes.MapRoute(
                name: "Register",
                url: "dang-ky",
                defaults: new { controller = "ClientUser", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "WebPhoneStore.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "ClientUser", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "WebPhoneStore.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
