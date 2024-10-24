using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XERO.API.DTO
{
    public class ProductOptionDto
    {
        public Guid? Id { get; set; } /// <summary>
        /// ????
        /// </summary>


        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
