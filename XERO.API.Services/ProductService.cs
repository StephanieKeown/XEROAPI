
using Microsoft.EntityFrameworkCore;
using XERO.API.DataAccess.Entities;
using XERO.API.DTO;
using XERO.API.Services.Interfaces;

namespace XERO.API.Services
{//Add cancellation tokens
    public class ProductService : IProductService
    {
        private readonly XeroDbContext _context;

        // Constructor injection of DbContextscribd
        public ProductService(XeroDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts(CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Select(product => new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    DeliveryPrice = product.DeliveryPrice,
                    Price = product.Price
                }).ToListAsync(cancellationToken); //async?

        } //im injecting from the same prject

        public async Task<IEnumerable<ProductDto>> GetProductsByName(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Where(product => product.Name == name)  // Filter before fetching from the database
                .Select(product => new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    DeliveryPrice = product.DeliveryPrice,
                    Price = product.Price
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductDto> GetProductById(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Where(x => x.Id == id)
                .Select(product => new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    DeliveryPrice = product.DeliveryPrice,
                    Price = product.Price
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProductDto> CreateProduct(ProductDto product, CancellationToken cancellationToken = default)
        {
            var newProduct = new Product()
            {
                Name = product.Name,
                Description = product.Description,
                DeliveryPrice = product.DeliveryPrice,
                Price = product.Price
            };

            await _context.Products.AddAsync(newProduct, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new ProductDto
            {
                Id = newProduct.Id,  // The generated Id
                Name = newProduct.Name,
                Description = newProduct.Description,
                DeliveryPrice = newProduct.DeliveryPrice,
                Price = newProduct.Price
            };
        }

        public async Task<bool> UpdateProduct(Guid id, ProductDto updateProductDto, CancellationToken cancellationToken = default)
        {
            // Find the existing product
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (existingProduct == null)
            {
                return false; // Product not found
            }

            // Update the product fields
            existingProduct.Name = updateProductDto.Name;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.DeliveryPrice = updateProductDto.DeliveryPrice;

            // Save changes to the database
            await _context.SaveChangesAsync(cancellationToken);

            return true; // Update was successful
        }
        public async Task<bool> DeleteProduct(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IEnumerable<ProductOptionDto>> GetOptionsByProductId(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.ProductOptions
                .Where(option => option.ProductId == productId)
                .Select(option => new ProductOptionDto
                {
                    Id = option.Id,
                    Name = option.Name,
                    Description = option.Description
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<ProductOptionDto> GetOptionByProductIdAndOptionId(Guid productId, Guid optionId, CancellationToken cancellationToken = default)
        {
            var productExists = await _context.Products.AnyAsync(p => p.Id == productId, cancellationToken);
            if (!productExists)
            {
                return null;
            }

            var option = await _context.ProductOptions.FindAsync(optionId);

            if (option == null || option.ProductId != productId)
            {
                return null;
            }

            return new ProductOptionDto
            {
                Id = option.Id,
                Name = option.Name,
                Description = option.Description
            };
        }

        public async Task<bool> DoesProductExist(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId, cancellationToken);
        }


        public async Task<ProductOptionDto> AddOptionToProduct(Guid productId, ProductOptionDto optionDto, CancellationToken cancellationToken = default)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            var newOption = new ProductOption
            {
                ProductId = productId,
                Name = optionDto.Name,
                Description = optionDto.Description
            };

            await _context.ProductOptions.AddAsync(newOption, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new ProductOptionDto
            {
                Id = newOption.Id,
                Name = newOption.Name,
                Description = newOption.Description
            };
        }

        public async Task<bool> UpdateProductOption(Guid productId, Guid optionId, ProductOptionDto optionDto, CancellationToken cancellationToken = default)
        {
            // Find the product option directly by productId and optionId
            var option = await _context.ProductOptions
                .FirstOrDefaultAsync(po => po.Id == optionId && po.ProductId == productId, cancellationToken);

            if (option == null)
            {
                return false;  // Return false if product or option doesn't exist
            }

            // Update the option details
            option.Name = optionDto.Name;
            option.Description = optionDto.Description;

            // Save the changes
            await _context.SaveChangesAsync(cancellationToken);

            return true; // Return true if the update was successful
        }

        public async Task<bool> DeleteProductOption(Guid productId, Guid optionId, CancellationToken cancellationToken = default)
        {
            // Find the product option directly by both productId and optionId
            var option = await _context.ProductOptions
                .FirstOrDefaultAsync(po => po.Id == optionId && po.ProductId == productId, cancellationToken);

            if (option == null)
            {
                return false;  // Return false if product option doesn't exist
            }

            // Delete the option
            _context.ProductOptions.Remove(option);
            await _context.SaveChangesAsync(cancellationToken);

            return true;  // Return true if deletion was successful
        }



    }
}
