using System.Threading.Tasks;
using Xunit;
using Moq;
using XERO.API.Services;
using XERO.API.DataAccess.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Update;
using Moq.EntityFrameworkCore;
using XERO.API.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace XERO.API.UnitTests
{
    public class ProductServiceTests : IClassFixture<ProductServiceFixture>
    {
        private readonly ProductServiceFixture _fixture;

        public ProductServiceTests(ProductServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData(nameof(ProductServiceTestsData.ListProductsTestData), MemberType = typeof(ProductServiceTestsData))]
        public async Task GetAllProducts_ReturnsProductDtos(List<Product> products)
        {
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);
            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetAllProducts();

            Assert.Equal(products.Count, result.Count());
            Assert.Equal(products[0].Name, result.First().Name); 
            Assert.Equal(products[products.Count - 1].Name, result.Last().Name);
        }


        [Theory]
        [MemberData(nameof(ProductServiceTestsData.NamedProductsTestData), MemberType = typeof(ProductServiceTestsData))]
        public async Task GetProductsByName_ReturnsFilteredProductDtos(string productName, List<Product> products)
        {
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);
            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetProductsByName(productName, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal(productName, result.FirstOrDefault().Name);  
        }


        [Theory]
        [MemberData(nameof(ProductServiceTestsData.ListProductsTestData), MemberType = typeof(ProductServiceTestsData))]
        public async Task GetProductsByName_ReturnsEmptyList_WhenNoProductsMatch(List<Product> products)
        {
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);
            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetProductsByName("NonExistentProduct", CancellationToken.None);

            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(ProductServiceTestsData.ListProductsTestData), MemberType = typeof(ProductServiceTestsData))]
        public async Task GetProductById_ReturnsProductDto_IfExists(List<Product> products)
        {
            var productId = products.First().Id;
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);
            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetProductById(productId, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(products.First().Name, result.Name);
        }

        [Theory]
        [MemberData(nameof(ProductServiceTestsData.ListProductsTestData), MemberType = typeof(ProductServiceTestsData))]
        public async Task GetProductById_ReturnsNull_IfProductDoesNotExist(List<Product> products)
        {
            var nonExistentProductId = Guid.NewGuid();
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);
            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetProductById(nonExistentProductId, CancellationToken.None);

            Assert.Null(result);
        }

        [Theory]
        [InlineData("Product 1", "Description 1", 33.49, 10.50)]
        [InlineData("Product 2", "Description 2", 40.99, 20.99)]
        public async Task CreateProduct_AddsNewProductToDatabase_ReturnsCreatedProductDto(string name, string description, decimal deliveryPrice, decimal price)
        {
            var productDto = new ProductDto
            {
                Name = name,
                Description = description,
                DeliveryPrice = deliveryPrice,
                Price = price
            };

            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = productDto.Name,
                Description = productDto.Description,
                DeliveryPrice = productDto.DeliveryPrice,
                Price = productDto.Price
            };

            _fixture.MockDbContext.Setup(db => db.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Callback<Product, CancellationToken>((product, token) => product.Id = newProduct.Id)
                .ReturnsAsync((Product product, CancellationToken token) => null);

            _fixture.MockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.CreateProduct(productDto, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(newProduct.Id, result.Id);  
            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Description, result.Description);
            Assert.Equal(productDto.DeliveryPrice, result.DeliveryPrice);
            Assert.Equal(productDto.Price, result.Price);
        }


        [Theory]
        [MemberData(nameof(ProductServiceTestsData.CombinedProductAndDtoTestData), MemberType = typeof(ProductServiceTestsData))]
        public async Task UpdateProduct_ReturnsTrue_WhenProductExists(List<Product> products, ProductDto updateProductDto)
        {
            var productId = products.First().Id;
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.UpdateProduct(productId, updateProductDto, CancellationToken.None);

            Assert.True(result);
        }


        [Theory]
        [MemberData(nameof(ProductServiceTestsData.ListProductDtoTestData),
            MemberType = typeof(ProductServiceTestsData))]
        public async Task UpdateProduct_ReturnsFalse_WhenProductDoesNotExist(ProductDto updateProductDto)
        {
            var nonExistentProductId = Guid.NewGuid();
            _fixture.MockDbContext.Setup(db => db.Products)
                .ReturnsDbSet(new List<Product>());
            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.UpdateProduct(nonExistentProductId, updateProductDto, CancellationToken.None);

            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(ProductServiceTestsData.ListProductsTestData), MemberType = typeof(ProductServiceTestsData))]
        public async Task DeleteProduct_ReturnsTrue_WhenProductExists(List<Product> products)
        {

            var productId = products.First().Id;
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);
            _fixture.MockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.DeleteProduct(productId, CancellationToken.None);

            Assert.True(result); 
        }

        [Fact]
        public async Task DeleteProduct_ReturnsFalse_WhenProductDoesNotExist()
        {
            var nonExistentProductId = Guid.NewGuid();
            var products = new List<Product>();

            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);
            _fixture.MockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.DeleteProduct(nonExistentProductId, CancellationToken.None);

            Assert.False(result); 
        }

        [Theory]
        [MemberData(nameof(ProductServiceTestsData.ProductOptionsTestData), MemberType = typeof(ProductServiceTestsData))]
        public async Task GetOptionsByProductId_ReturnsOptions_WhenOptionsExist(Guid productId, List<ProductOption> productOptions)
        {
            foreach (var option in productOptions)
            {
                option.ProductId = productId;
            }

            _fixture.MockDbContext.Setup(db => db.ProductOptions)
                .ReturnsDbSet(productOptions);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetOptionsByProductId(productId, CancellationToken.None);

            Assert.Equal(productOptions.Count, result.Count());
            Assert.Equal(productOptions.First().Name, result.First().Name);
        }

        [Fact]
        public async Task GetOptionsByProductId_ReturnsEmptyList_WhenNoOptionsExist()
        {
            var productId = Guid.NewGuid();
            var emptyProductOptions = new List<ProductOption>();

            _fixture.MockDbContext.Setup(db => db.ProductOptions)
                .ReturnsDbSet(emptyProductOptions);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetOptionsByProductId(productId, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(ProductServiceTestsData.ProductOptionByProductAndOptionIdTestData), MemberType = typeof(ProductServiceTestsData))]
        public async Task GetOptionByProductIdAndOptionId_ReturnsOption_WhenOptionExists(Guid productId, Guid optionId, ProductOption productOption)
        {
            var products = new List<Product> { new Product { Id = productId } }.AsQueryable();
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);

            _fixture.MockDbContext.Setup(db => db.ProductOptions.FindAsync(optionId))
                                  .ReturnsAsync(productOption);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetOptionByProductIdAndOptionId(productId, optionId, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(productOption.Name, result.Name);
            Assert.Equal(productOption.Description, result.Description);
        }

        [Fact]
        public async Task GetOptionByProductIdAndOptionId_ReturnsNull_WhenProductDoesNotExist()
        {

            var products = new List<Product>().AsQueryable();
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetOptionByProductIdAndOptionId(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetOptionByProductIdAndOptionId_ReturnsNull_WhenOptionDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var products = new List<Product> { new Product { Id = productId } }.AsQueryable();
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);


            _fixture.MockDbContext.Setup(db => db.ProductOptions.FindAsync(It.IsAny<Guid>()))
                                  .ReturnsAsync((ProductOption)null);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetOptionByProductIdAndOptionId(productId, Guid.NewGuid(), CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetOptionByProductIdAndOptionId_ReturnsNull_WhenOptionDoesNotBelongToProduct()
        {
            var productId = Guid.NewGuid();
            var optionId = Guid.NewGuid();
            var productOption = new ProductOption
            {
                Id = optionId,
                ProductId = Guid.NewGuid(),
                Name = "Test Option",
                Description = "Test Description"
            };

            var products = new List<Product> { new Product { Id = productId } }.AsQueryable();
            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);

            _fixture.MockDbContext.Setup(db => db.ProductOptions.FindAsync(optionId))
                                  .ReturnsAsync(productOption);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.GetOptionByProductIdAndOptionId(productId, optionId, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task DoesProductExist_ReturnsTrue_WhenProductExists()
        {
            var productId = Guid.NewGuid();
            var products = new List<Product>
            {
                new Product { Id = productId, Name = "Product 1", Price = 10.50M, DeliveryPrice = 33.49M }
            }.AsQueryable();

            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.DoesProductExist(productId, CancellationToken.None);

            Assert.True(result); 
        }

        [Fact]
        public async Task DoesProductExist_ReturnsFalse_WhenProductDoesNotExist()
        {
            var nonExistentProductId = Guid.NewGuid();
            var products = new List<Product>().AsQueryable();

            _fixture.MockDbContext.Setup(db => db.Products).ReturnsDbSet(products);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.DoesProductExist(nonExistentProductId, CancellationToken.None);

            Assert.False(result); 
        }


        [Fact]
        public async Task AddOptionToProduct_AddsNewOption_WhenProductExists()
        {
            var productId = Guid.NewGuid();
            var optionDto = new ProductOptionDto
            {
                Name = "Test Option",
                Description = "Test Description"
            };

            var products = new List<Product>
            {
                new Product { Id = productId, Name = "Product 1" }
            };

            _fixture.MockDbContext.Setup(db => db.Products)
                .ReturnsDbSet(products);

            _fixture.MockDbContext.Setup(db => db.ProductOptions.AddAsync(It.IsAny<ProductOption>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductOption productOption, CancellationToken token) => null);

            _fixture.MockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var service = new ProductService(_fixture.MockDbContext.Object);


            var result = await service.AddOptionToProduct(productId, optionDto, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(optionDto.Name, result.Name);
            Assert.Equal(optionDto.Description, result.Description);
        }


        [Fact]
        public async Task AddOptionToProduct_ThrowsException_WhenProductDoesNotExist()
        {
            var nonExistentProductId = Guid.NewGuid();
            var optionDto = new ProductOptionDto
            {
                Name = "Test Option",
                Description = "Test Description"
            };

            var products = new List<Product>();

            _fixture.MockDbContext.Setup(db => db.Products)
                .ReturnsDbSet(products); 

            var service = new ProductService(_fixture.MockDbContext.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => service.AddOptionToProduct(nonExistentProductId, optionDto, CancellationToken.None));
            Assert.Equal("Product not found.", exception.Message);
        }

        [Fact]
        public async Task UpdateProductOption_ReturnsTrue_WhenOptionExists()
        {
            var productId = Guid.NewGuid();
            var optionId = Guid.NewGuid();

            var optionDto = new ProductOptionDto
            {
                Name = "Updated Option",
                Description = "Updated Description"
            };

            var productOptions = new List<ProductOption>
            {
                new ProductOption
                {
                    Id = optionId,
                    ProductId = productId,
                    Name = "Old Option",
                    Description = "Old Description"
                }
            };

            _fixture.MockDbContext.Setup(db => db.ProductOptions)
                .ReturnsDbSet(productOptions); 

            _fixture.MockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.UpdateProductOption(productId, optionId, optionDto, CancellationToken.None);

            Assert.True(result);
            Assert.Equal(optionDto.Name, productOptions.First().Name);
            Assert.Equal(optionDto.Description, productOptions.First().Description);
        }

        [Fact]
        public async Task UpdateProductOption_ReturnsFalse_WhenOptionDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var optionId = Guid.NewGuid();

            var optionDto = new ProductOptionDto
            {
                Name = "Updated Option",
                Description = "Updated Description"
            };

            _fixture.MockDbContext.Setup(db => db.ProductOptions)
                .ReturnsDbSet(new List<ProductOption>());

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.UpdateProductOption(productId, optionId, optionDto, CancellationToken.None);

            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(ProductServiceTestsData.ProductOptionByProductAndOptionIdTestData), MemberType = typeof(ProductServiceTestsData))]
        public async Task DeleteProductOption_ReturnsTrue_WhenOptionExists(Guid productId, Guid optionId, ProductOption productOption)
        {
            productOption.ProductId = productId;
            productOption.Id = optionId;

            var productOptions = new List<ProductOption> { productOption }.AsQueryable();

            _fixture.MockDbContext.Setup(db => db.ProductOptions)
                                  .ReturnsDbSet(productOptions);

            _fixture.MockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(1);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.DeleteProductOption(productId, optionId, CancellationToken.None);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProductOption_ReturnsFalse_WhenOptionDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var optionId = Guid.NewGuid();

            var productOptions = new List<ProductOption>().AsQueryable();

            _fixture.MockDbContext.Setup(db => db.ProductOptions)
                                  .ReturnsDbSet(productOptions);

            var service = new ProductService(_fixture.MockDbContext.Object);

            var result = await service.DeleteProductOption(productId, optionId, CancellationToken.None);

            Assert.False(result);

        }

    }
}


