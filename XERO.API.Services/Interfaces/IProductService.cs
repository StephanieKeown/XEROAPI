using System.Runtime.InteropServices.ComTypes;
using XERO.API.DTO;

namespace XERO.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts(CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductDto>> GetProductsByName(string name, CancellationToken cancellationToken = default);

        Task<ProductDto> GetProductById(Guid id, CancellationToken cancellationToken = default);
        Task<ProductDto> CreateProduct(ProductDto product, CancellationToken cancellationToken = default);

        Task<bool> UpdateProduct(Guid id, ProductDto product, CancellationToken cancellationToken = default);
        Task<bool> DeleteProduct(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductOptionDto>> GetOptionsByProductId(Guid productId, CancellationToken cancellationToken = default);
        Task<bool> DoesProductExist(Guid productId, CancellationToken cancellationToken = default);

        Task<ProductOptionDto> GetOptionByProductIdAndOptionId(Guid productId, Guid optionId, CancellationToken cancellationToken = default);

        Task<ProductOptionDto> AddOptionToProduct(Guid productId, ProductOptionDto optionDto, CancellationToken cancellationToken = default);

        Task<bool> UpdateProductOption(Guid productId, Guid optionId, ProductOptionDto optionDto, CancellationToken cancellationToken = default);

        Task<bool> DeleteProductOption(Guid productId, Guid optionId, CancellationToken cancellationToken = default);
    }
}
