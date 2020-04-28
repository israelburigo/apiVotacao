using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotacaoApi.Models
{
    public class HttpBaseMessage
    {
        public object Content { get; set; }
        public string Message { get; set; }
    }
}