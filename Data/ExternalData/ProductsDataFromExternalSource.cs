using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Data.ExternalData
{
    public class ProductsDataFromExternalSource : IProductsDataFromExternalSource
    {
        private IHttpClientFactory Factory { get; }

        public ProductsDataFromExternalSource(IHttpClientFactory factory)
        {
            Factory = factory;
        }

        public async Task<ProductAdditionalData> GetAsync(int id)
        {
            try
            {
                var client = Factory.CreateClient("MockAPI");
                var uri = $"products/{id}";
                var response = await client.GetFromJsonAsync<ProductAdditionalData>(uri);

                return response;
            }
            catch (Exception ex)
            {
                return ProductAdditionalData.GetErrorHandler(ex);
            }
        }

    }
}
