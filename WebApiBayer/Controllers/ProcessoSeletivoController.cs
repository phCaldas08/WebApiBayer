using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiBayer.Classes.Uteis;

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
                using (bayerEntities db = new bayerEntities())
                {
                    if (!jbody.ContainsKey("id_vaga") || !db.vagas.ToList().Any(i => i.ToString() == jbody["id_vaga"].ToString())) return NotFound();

                    BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(jbody["vaga"].ToString());

                    IMongoDatabase dbMongo = Classes.Mongo.GetDatabase("teste_bayer");

                    if (dbMongo.GetCollection<BsonDocument>("processo_seletivo").CountDocuments(i => i["id_vaga"] == document["id_vaga"]) == 0)
                    {
                        dbMongo.GetCollection<BsonDocument>("processo_seletivo").InsertOne(document);
                        return Ok("Processo seletivo cadastrado com sucesso!");
                    }
                    else
                        return BadRequest("Vaga já cadastrada!");
                }
            }
            catch (Exception ex)
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
            catch (BayerException bex)
            {
                return InternalServerError(bex);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Erro ao inscrever curriculo para o processo."));
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
                using (bayerEntities db = new bayerEntities())
                {
                    IMongoDatabase dbMongo = Classes.Mongo.GetDatabase("teste_bayer");

                    List<string> ids_vagas = db.vagas.ToList().Where(i => i.data_criacao.AddDays(i.duracao) <= DateTime.Today).Select(i => i.id_vaga.ToString()).ToList();

                    if (dbMongo.GetCollection<BsonDocument>("processo_seletivo").CountDocuments(i => ids_vagas.Contains(i["id_vaga"].ToString())) > 0)
                    {
                        List<BsonDocument> listbson = dbMongo.GetCollection<BsonDocument>("processo_seletivo")
                            .Find(i => ids_vagas.Contains(i["id_vaga"].ToString()))
                            .Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();

                        string jsonstr = "{ [";

                        //Formatar json com todos os processos seletivos do banco
                        foreach(BsonDocument bson in listbson)
                            jsonstr += bson.ToJson<MongoDB.Bson.BsonDocument>() + (listbson.Last() == bson ? "] }" : ",");


                        return Json(JObject.Parse(jsonstr));
                    }
                }

                return Unauthorized();
            }
            catch (Classes.Uteis.BayerException bex)
            {
                return InternalServerError(bex);
            }
            catch (Exception ex)
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
            catch(Classes.Uteis.BayerException bex)
            {
                return InternalServerError(bex);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("Erro ao consultar status."));
            }
        }
    }  

}
