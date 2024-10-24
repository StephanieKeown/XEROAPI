using System.Collections.Generic;
using XERO.API.DataAccess.Entities;
using XERO.API.DTO;

namespace XERO.API.UnitTests
{
    public static class ProductServiceTestsData
    {
        public static IEnumerable<object[]> ListProductsTestData()
        {
            yield return new object[]
            {
                new List<Product>
                {
                    new Product { Id = Guid.NewGuid(), Name = "Product 1", Description = "Description 1", Price = 10.50M, DeliveryPrice = 33.49M },
                    new Product { Id = Guid.NewGuid(), Name = "Product 2", Description = "Description 2", Price = 20.99M, DeliveryPrice = 40.99M }
                }
            };
        }

        public static IEnumerable<object[]> ListProductDtoTestData()
        {
            yield return new object[]
            {
                new ProductDto
                {
                    Name = "Product 1",
                    Description = "Description 1",
                    Price = 10.50M,
                    DeliveryPrice = 33.49M
                }
            };

            yield return new object[]
            {
                new ProductDto
                {
                    Name = "Product 2",
                    Description = "Description 2",
                    Price = 20.99M,
                    DeliveryPrice = 40.99M
                }
            };
        }

        public static IEnumerable<object[]> NamedProductsTestData()
        {
            yield return new object[]
            {
                "Product 1",
                new List<Product>
                {
                    new Product { Id = Guid.NewGuid(), Name = "Product 1", Description = "Description 1", Price = 10.50M, DeliveryPrice = 33.49M },
                    new Product { Id = Guid.NewGuid(), Name = "Product 2", Description = "Description 2", Price = 20.99M, DeliveryPrice = 40.99M }
                }
            };

            yield return new object[]
            {
                "Product 2",
                new List<Product>
                {
                    new Product { Id = Guid.NewGuid(), Name = "Product 1", Description = "Description 1", Price = 10.50M, DeliveryPrice = 33.49M },
                    new Product { Id = Guid.NewGuid(), Name = "Product 2", Description = "Description 2", Price = 20.99M, DeliveryPrice = 40.99M }
                }
            };
        }

        public static IEnumerable<object[]> CombinedProductAndDtoTestData()
        {
            yield return new object[]
            {
                new List<Product>
                {
                    new Product { Id = Guid.NewGuid(), Name = "Product 1", Description = "Description 1", Price = 10.50M, DeliveryPrice = 33.49M }
                },
                new ProductDto
                {
                    Name = "Updated Product 1",
                    Description = "Updated Description 1",
                    Price = 15.99M,
                    DeliveryPrice = 5.99M
                }
            };

            yield return new object[]
            {
                new List<Product>
                {
                    new Product { Id = Guid.NewGuid(), Name = "Product 2", Description = "Description 2", Price = 20.99M, DeliveryPrice = 40.99M }
                },
                new ProductDto
                {
                    Name = "Updated Product 2",
                    Description = "Updated Description 2",
                    Price = 22.99M,
                    DeliveryPrice = 6.99M
                }
            };
        }
        public static IEnumerable<object[]> ProductOptionsTestData()
        {
            yield return new object[]
            {
                Guid.NewGuid(), 
                new List<ProductOption>
                {
                    new ProductOption { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Name = "Option 1", Description = "Option 1 Description" },
                    new ProductOption { Id = Guid.NewGuid(), ProductId = Guid.NewGuid(), Name = "Option 2", Description = "Option 2 Description" }
                }
            };
        }

        public static IEnumerable<object[]> ProductOptionByProductAndOptionIdTestData()
        {
            // Test case 1: Product and Option exist
            var productId = Guid.NewGuid();
            var optionId = Guid.NewGuid();
            yield return new object[]
            {
                productId,
                optionId,
                new ProductOption
                {
                    Id = optionId,
                    ProductId = productId,
                    Name = "Option 1",
                    Description = "Option 1 Description"
                }
            };

            // Test case 2: Product exists but no valid option is provided
            productId = Guid.NewGuid();
            optionId = Guid.NewGuid();
            yield return new object[]
            {
                productId,
                optionId,
                new ProductOption
                {
                    Id = optionId,
                    ProductId = productId,
                    Name = "Option 2",
                    Description = "Option 2 Description"
                }
            };
        }
    }
}