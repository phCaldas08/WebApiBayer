using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApiBayer.Controllers
{
    [RoutePrefix("api/bayer/curriculo")]
    public class CurriculoController : ApiController
    {
        [Route("InserirCurriculo")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] JObject jbody)
        {
            try
            {
                if (!jbody.ContainsKey("token_candidato")) return BadRequest("Token não identificado!");
                               
                BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(jbody["curriculo"].ToString());

                IMongoDatabase db = Classes.Mongo.GetDatabase("teste_bayer");

                if (db.GetCollection<BsonDocument>("curriculo").CountDocuments(i => i["candidato"]["id_candidato"] == document["candidato"]["id_candidato"]) == 0)
                {
                    db.GetCollection<BsonDocument>("curriculo").InsertOne(document);
                    return Ok("Candidato cadastrado com sucesso!");
                }
                else
                    return BadRequest("Candidato já cadastrado para o processo seletivo");
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("ConsultarCurriculo")]
        [HttpGet]
        public IHttpActionResult GetConsultar([FromBody] JObject jbody)
        {
            try
            {
                //Validar token

                var a = jbody.ToString();

                string candidato_id = jbody["id_candidato"].ToString();
                string processo_id = jbody["id_processo_seletivo"].ToString();

                IMongoDatabase db = Classes.Mongo.GetDatabase("teste_bayer");

                if (db.GetCollection<BsonDocument>("curriculo").CountDocuments(i => i["candidato"]["id_candidato"] == candidato_id && i["id_processo_seletivo"] == processo_id) > 0)
                {
                    BsonDocument bson = db.GetCollection<BsonDocument>("curriculo")
                        .Find(i => i["candidato"]["id_candidato"] == candidato_id && i["id_processo_seletivo"] == processo_id)
                        .Project(Builders<BsonDocument>.Projection.Exclude("_id"))
                        .First();

                    string jsonstr = bson.ToJson<MongoDB.Bson.BsonDocument>();

                    return Json(JObject.Parse(jsonstr));
                }
                else
                    return BadRequest("Nenhum candidato encontrado com este id encontrado para o processo!");
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [Route("ValidarCurriculo")]
        [HttpGet]
        public IHttpActionResult GetValidar([FromBody] JObject jbody)
        {
            try
            {
                //Buscar processo seletivo

                //validar candidato

                //validar 
                               
                //Verificar compatibilidade minima
                     

                return Ok("Candidato cadastrado com sucesso!");
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }



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
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
