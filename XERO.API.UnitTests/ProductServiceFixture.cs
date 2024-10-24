using Moq;
using XERO.API.DataAccess.Entities;
using System;
using Moq.EntityFrameworkCore;

namespace XERO.API.UnitTests
{
    public class ProductServiceFixture : IDisposable
    {
        public Mock<XeroDbContext> MockDbContext { get; private set; }

        public ProductServiceFixture()
        {
            ResetMockContext();
        }

        public void ResetMockContext()
        {
            MockDbContext = new Mock<XeroDbContext>();

            MockDbContext.Setup(db => db.Products).ReturnsDbSet(new List<Product>());
            MockDbContext.Setup(db => db.ProductOptions).ReturnsDbSet(new List<ProductOption>());
        }

        public void Dispose()
        {
            ResetMockContext();
        }
    }
}