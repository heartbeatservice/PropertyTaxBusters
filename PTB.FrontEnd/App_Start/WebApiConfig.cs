using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PTB
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                       name: "PropertySearch",
                        routeTemplate: "api/Search/PropertySearch",
                            defaults: new { controller = "PropertySearch", action = "getSearch" }
                );

            config.Routes.MapHttpRoute(
                      name: "PropertySearchByClientInfo",
                       routeTemplate: "api/Search/PropertySearchByClientInfo",
                           defaults: new { controller = "PropertySearch", action = "getSearchByClientInfo" }
               );

            config.Routes.MapHttpRoute(
                      name: "PropertyAdvanceSearch",
                       routeTemplate: "api/Search/PropertyAdvanceSearch",
                           defaults: new { controller = "PropertySearch", action = "getAdvanceSearch" }
               );
            config.Routes.MapHttpRoute(
                       name: "getPropertyById",
                        routeTemplate: "api/Search/getPropertyById",
                            defaults: new { controller = "PropertySearch", action = "getPropertyById" }
                );

            config.Routes.MapHttpRoute(
                        name: "updateProperty",
                            routeTemplate: "api/Search/updateProperty",
                                defaults: new { controller = "PropertySearch", action = "updateProperty" }
                );

            config.Routes.MapHttpRoute(
                    name: "deleteProperty",
                        routeTemplate: "api/Search/deleteProperty",
                            defaults: new { controller = "PropertySearch", action = "deleteProperty" }
                    );
        }
    }
}