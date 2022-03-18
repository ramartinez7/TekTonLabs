using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Newtonsoft.Json;
using System.Net;

namespace API
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ResourceExistsAttribute : TypeFilterAttribute
    {
        /// <summary>
        ///  Validates the resource existence using the method ExistsAsync in the EntityStore
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="keyType">Type of the entity key</param>
        /// <param name="routeParam">RouteParam name where the entity key will be available</param>
        public ResourceExistsAttribute(Type entityType, string routeParam) : base(typeof(ResourceExistsAttributeImplementation))
        {
            Arguments = new object[] { entityType, routeParam };
        }

        public class ResourceExistsAttributeImplementation : IAsyncActionFilter
        {
            public IHttpContextAccessor ContextAccessor { get; }
            public Type EntityType { get; }
            public Type KeyType { get; set; }
            public string RouteParam { get; }

            public ResourceExistsAttributeImplementation(IHttpContextAccessor contextAccessor, Type entityType, string routeParam)
            {
                ContextAccessor = contextAccessor;
                EntityType = entityType;
                RouteParam = routeParam;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!IsAssignableToGenericType(EntityType, typeof(IEntity<>)))
                {
                    HandleFailedExpectation($"Given Entitytype is not valid. It should be: {typeof(IEntity<>)}");
                }

                KeyType = EntityType.GetProperty("Id")?.PropertyType;

                if (KeyType is null)
                {
                    HandleFailedExpectation("Property Id not found");
                }

                Type storeType = typeof(IEntityStore<,>).MakeGenericType(EntityType, KeyType);
                var store = ContextAccessor.HttpContext.RequestServices.GetService(storeType);

                var methodInfo = storeType.GetMethod("ExistsAsync");
                
                if (methodInfo is null)
                {
                    HandleFailedExpectation("Couldn't validate resource existance");
                }

                var id = context.ActionArguments[RouteParam];

                if (id is null)
                {
                    HandleFailedExpectation("Couldn't find resource id in route params");
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

            public static bool IsAssignableToGenericType(Type givenType, Type genericType)
            {
                var interfaceTypes = givenType.GetInterfaces();

                foreach (var it in interfaceTypes)
                {
                    if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                        return true;
                }

                if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                    return true;

                Type baseType = givenType.BaseType;
                if (baseType == null) return false;

                return IsAssignableToGenericType(baseType, genericType);
            }

            public void HandleFailedExpectation(string message)
            {
                throw new HttpRequestException(message);
            }
        }

        
    }
}
