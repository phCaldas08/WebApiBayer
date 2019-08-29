using MongoDB.Driver;
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



        /// <summary>
        /// Cadastra novo processo seletivo.
        /// </summary>
        /// <param name="jbody"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Inscrever curriculo do candidato
        /// </summary>
        /// <param name="jbody"></param>
        /// <returns></returns>
        [Route("InscreverCurriculo")]
        [HttpPost]
        public IHttpActionResult postInscrever([FromBody] JObject jbody)
        {
            try
            {


                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        /// <summary>
        /// Consulta processos seletivos abertos
        /// </summary>
        /// <param name="jbody"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Consultar Status Resumido do processo seletivo
        /// </summary>
        /// <param name="jbody"></param>
        /// <returns></returns>
        [Route("StatusResumido")]
        [HttpGet]
        public IHttpActionResult GetStatus([FromBody] JObject jbody)
        {
            try
            {
                if (!jbody.ContainsKey("token_cliente")) return BadRequest("Token não identificado!");

                //Validar o token

                string id_processo_seletivo = jbody["id_processo_seletivo"].ToString();

                IMongoDatabase db = Classes.Mongo.GetDatabase("teste_bayer");

                if (db.GetCollection<Models.StatusResumido>("processo_seletivo").CountDocuments(i => i.id_processo_seletivo == id_processo_seletivo) > 0)
                {
                    Models.StatusResumido status = db.GetCollection<Models.StatusResumido>("processo_seletivo").Find(i => i.id_processo_seletivo == id_processo_seletivo).First();
                    return Json(status);
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }  

}
