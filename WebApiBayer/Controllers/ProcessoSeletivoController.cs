using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiBayer.Controllers
{
    [RoutePrefix("api/bayer/processoseletivo")]
    public class ProcessoSeletivoController : ApiController
    {
        [Route("NovoProcessoSeletivo")]
        [HttpPost]
        public IHttpActionResult postNovo([FromBody] JObject jbody)
        {
            try
            {


                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }            
        }

        [Route("ProcessosSeletivos")]
        [HttpGet]
        public IHttpActionResult getConsultaProcessos([FromBody] JObject jbody)
        {
            try
            {
                return Json("");
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }


        }
    }  

}
