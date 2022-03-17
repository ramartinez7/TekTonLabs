using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace API
{
    public class ResourceExistsAttribute : TypeFilterAttribute
    {
        /// <summary>
        ///  Validates the resource existence using the method ExistsAsync in the EntityStore
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="keyType">Type of the entity key</param>
        /// <param name="routeParam">RouteParam name where the entity key will be available</param>
        public ResourceExistsAttribute(Type entityType, Type keyType, string routeParam) : base(typeof(ResourceExistsAttributeImplementation))
        {
            Arguments = new object[] { entityType, keyType, routeParam };
        }

        public class ResourceExistsAttributeImplementation : IAsyncActionFilter
        {
            public IHttpContextAccessor ContextAccessor { get; }
            public Type EntityType { get; }
            public Type KeyType { get; }
            public string RouteParam { get; }

            public ResourceExistsAttributeImplementation(IHttpContextAccessor contextAccessor, Type entityType, Type keyType, string routeParam)
            {
                ContextAccessor = contextAccessor;
                EntityType = entityType;
                KeyType = keyType;
                RouteParam = routeParam;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                Type storeType = typeof(IEntityStore<,>).MakeGenericType(EntityType, KeyType);
                var store = ContextAccessor.HttpContext.RequestServices.GetService(storeType);

                var methodInfo = storeType.GetMethod("ExistsAsync");
                
                if (methodInfo is null)
                {
                    throw new HttpRequestException("Couldn't validate resource existance");
                }

                var id = context.ActionArguments[RouteParam];

                if (id is null)
                {
                    throw new HttpRequestException("Couldn't find resource id in route params");
                }

                bool exists = await (Task<bool>)methodInfo.Invoke(store, new object[] { id });

                if (!exists)
                {
                    HttpResponse response = context.HttpContext.Response;
                    var message = new { message = $"Resource does not exist", id };
                    response.ContentType = "application/json";
                    response.StatusCode = (int)HttpStatusCode.NotFound;

                    await response.WriteAsJsonAsync(message);
                    return;
                }

                await next();
            }
        }

        
    }
}
