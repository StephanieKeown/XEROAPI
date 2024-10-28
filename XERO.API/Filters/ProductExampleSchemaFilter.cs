using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using XERO.API.Models;

namespace XERO.API.Filters
{
    public class ProductExampleSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(Product))
            {
                schema.Properties["price"].Example = new OpenApiString("99.99");
                schema.Properties["deliveryPrice"].Example = new OpenApiString("9.99");
                schema.Properties["name"].Example = new OpenApiString("Sample Product");
                schema.Properties["description"].Example = new OpenApiString("A sample description of the product.");
            }
        }
    }

}