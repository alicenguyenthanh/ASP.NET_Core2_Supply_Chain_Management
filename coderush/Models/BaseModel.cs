using coderush.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class BaseModel
    {
        //audit trail
        public string CreateBy { get; set; }
        public DateTime CreateAtUtc { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateAtUtc { get; set; }
    }
}
