using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiBayer.Controllers
{

    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [Route("login")]
        [HttpGet]
        public IHttpActionResult GetLogar([FromBody] JObject jbody)
        {
            try
            {
                if (jbody.ContainsKey("usuario")) {

                    using (bayerEntities db = new bayerEntities())
                    {
                        Models.LoginModel usuario = JsonConvert.DeserializeObject<Models.LoginModel>(jbody["usuario"].ToString());

                        if (db.usuario.Any(i => i.login == usuario.Login && i.senha == usuario.Senha))
                            return Ok("candidato");
                        else if (db.recrutador.Any(i => i.login == usuario.Login && i.senha == usuario.Senha))
                            return Ok("recrutador");
                        else
                            return NotFound();
                    }
                }
                else
                    return BadRequest();
            }
            catch(Exception ex)
            {
                return InternalServerError(new Exception("Não foi possivel realizar o login"));
            }

        }

        [Route("cadastrar")]
        [HttpPost]
        public IHttpActionResult PostCadastrar([FromBody] JObject jbody) {
            try
            {
                if (jbody.ContainsKey("usuario"))
                {
                    using(bayerEntities db = new bayerEntities())
                    {
                        Models.UsuarioModel usuario = JsonConvert.DeserializeObject<Models.UsuarioModel>(jbody["usuario"].ToString());

                        if (db.usuario.Any(i => i.login == usuario.Login) ||db.recrutador.Any(i => i.login == usuario.Login))
                            return BadRequest("Usuário já cadastrado");
                        else if (string.IsNullOrEmpty(usuario.Senha))
                            return BadRequest("Senha vazia ou em branco");
                        else
                        {
                            db.usuario.Add(new usuario() { login = usuario.Login,
                                senha = usuario.Senha,
                                email = usuario.Email,
                                sobrenome = usuario.SobreNome,
                                nome = usuario.Nome,
                                cpf = usuario.CPF,
                                tel = usuario.Tel
                            });

                            db.SaveChanges();

                            return Ok();
                        }
                    }
                }
                else if (jbody.ContainsKey("recrutador"))
                {
                    using (bayerEntities db = new bayerEntities())
                    {
                        Models.RecrutadorModel recrutador = JsonConvert.DeserializeObject<Models.RecrutadorModel>(jbody["recrutador"].ToString());

                        if (db.usuario.Any(i => i.login == recrutador.Login) || db.recrutador.Any(i => i.login == recrutador.Login))
                            return BadRequest("Recrutador já cadastrado");
                        else if (string.IsNullOrEmpty(recrutador.Senha))
                            return BadRequest("Senha vazia ou em branco");
                        else
                        {
                            db.recrutador.Add(new recrutador() { login = recrutador.Login,
                                senha = recrutador.Senha,
                                nome = recrutador.Nome,
                                sobrenome = recrutador.SobreNome,
                                email = recrutador.Email,
                                tel = recrutador.Tel
                                
                            });

                            db.SaveChanges();

                            return Ok();
                        }
                    }
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
