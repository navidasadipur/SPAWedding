using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Seshin.Web.Models
{
    public class SendResponse
    {
        public string Message { get; set; }
        public long[] Indices { get; set; }
    }
}