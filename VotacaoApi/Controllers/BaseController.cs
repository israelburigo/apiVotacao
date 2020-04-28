using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using VotacaoApi.Models;

namespace VotacaoApi.Controllers
{
    public class BaseController : ApiController
    {
        public HttpResponseMessage RetSucesso(object ret)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new HttpBaseMessage { Content = ret }, "application/json");
        }

        public HttpResponseMessage RetErro(Exception e)
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest, new HttpBaseMessage { Message = e.Message }, "application/json");
        }
    }
}