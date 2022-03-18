using Models;

namespace Data.ExternalData
{
    public interface IProductsDataFromExternalSource
    {
        Task<ProductAdditionalData> GetAsync(int id);
    }
}