﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IDGenerator
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "GenerateID",
                routeTemplate: "api/{controller}/"
            );

            config.Routes.MapHttpRoute(
                name: "ValidateID",
                routeTemplate: "api/{controller}/{idNumberToCheck}"
            );
        }
    }
}
