using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookfyApi.DTOs
{
    public class AutorReadDto
    {
        
        public int Id {get ; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Nacionalidad { get; set; } = string.Empty;
    }
}