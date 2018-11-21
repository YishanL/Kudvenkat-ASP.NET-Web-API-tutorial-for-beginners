using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiContrib.Formatting.Jsonp;

namespace EmployeeService
{
    // Approach 2
    public class CustomJsonFormatter : JsonMediaTypeFormatter
    {
        public CustomJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            //config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();


            // Since we removed the Xml formatter, will always format the data using Json formatter irrespective of the 'Accept' header value in the request.
            //config.Formatters.Remove(config.Formatters.XmlFormatter);


            // When a request made from browser(Accept header value is "text/html"), want to return Json instead of XML
            // Approach 1
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));


            // Approach 2
            config.Formatters.Add(new CustomJsonFormatter());


            // Issue cross domain ajax calls by using Jsonp
            //var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            //config.Formatters.Insert(0, jsonpFormatter);


            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            // The first parameter "origins", is a comma separated list of websites that we want to be able to issue cross domain ajax calls
            // e.g. new EnableCorsAttribute("http://localhost:34049, http://pragimtech.com", "*", "");
            // The second parameter "headers", is a comma separated list of headers that are supported by the resource
            // The third parameter "methods", is a comma separated list of methods that is supported by the resource
            // e.g. if you want to issue cross domain ajax calls to GET method only, can just specify GET there
            config.EnableCors(cors);
            // These two lines of code that we have in the Register() method enables cors globally for the entire application, that is for all the controllers and all action methods
            // Sometimes you may not want to enable cors globally for the entire application. We can decorate a particular controller with "EnableCorsAttribute".


            config.Filters.Add(new RequireHttpsAttribute());
            // this enables HTTPS for the entire Web API application that is for all the controllers and action methods.
            // If you want to enable HTTPS only for specific controllers within your Web API application, not register here, instead decorate only those controllers which you want to enable HTTPS
        }
    }
}