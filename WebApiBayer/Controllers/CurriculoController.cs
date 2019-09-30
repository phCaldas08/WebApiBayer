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

        /// <summary>
        /// Inserir curriculo
        /// </summary>
        /// <param name="jbody"></param>
        /// <returns></returns>
        [Route("InserirCurriculo")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] JObject jbody)
        {
            try
            {

                if (!jbody.ContainsKey("id_usuario")) return BadRequest("Usuário não identificado!");
                               
                BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(jbody["curriculo"].ToString());

                IMongoDatabase db = Classes.Mongo.GetDatabase("teste_bayer");

                if (db.GetCollection<BsonDocument>("curriculo").CountDocuments(i => i["candidato"]["id_candidato"] == document["candidato"]["id_candidato"]) > 0)
                {
                    db.GetCollection<BsonDocument>("curriculo").DeleteOne(i => i["candidato"]["id_candidato"] == document["candidato"]["id_candidato"]);
                }
                
                db.GetCollection<BsonDocument>("curriculo").InsertOne(document);
                return Ok("Candidato cadastrado com sucesso!");
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Consulta curriculo do candidato.
        /// </summary>
        /// <param name="jbody"></param>
        /// <returns></returns>
        [Route("ConsultarCurriculo")]
        [HttpGet]
        public IHttpActionResult GetConsultar([FromBody] JObject jbody)
        {
            try
            {
                //Validar token

                var a = jbody.ToString();

                string candidato_id = jbody["id_candidato"].ToString();

                IMongoDatabase db = Classes.Mongo.GetDatabase("teste_bayer");

                if (db.GetCollection<BsonDocument>("curriculo").CountDocuments(i => i["candidato"]["id_candidato"] == candidato_id) > 0)
                {
                    BsonDocument bson = db.GetCollection<BsonDocument>("curriculo")
                        .Find(i => i["candidato"]["id_candidato"] == candidato_id)
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

        /// <summary>
        /// Valida campos do curriculo e verifica compatibilidade.
        /// </summary>
        /// <param name="jbody"></param>
        /// <returns></returns>
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




    }
}
