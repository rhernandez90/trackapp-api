using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Helpers
{
    public class RequestResponseDto
    {
        public Object  Data { get; set; }
        public string Message { get; set; }
        public int Key { get; set; }
    }
}
