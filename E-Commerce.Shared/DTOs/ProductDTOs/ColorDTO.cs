using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.DTOs.ProductDTOs
{
    public class ColorDTO
    {
        public string Color { get; set; } = default!;
        public string HexValue { get; set; } = "#FFFFFF";
    } 
}
