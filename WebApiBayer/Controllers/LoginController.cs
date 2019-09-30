using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApiBayer.Controllers
{
    [RoutePrefix("api/bayer/login")]
    public class LoginController : ApiController
    {
        

        [Route("login")]
        [HttpPost]
        public IHttpActionResult PostLogar([FromBody] JObject jbody)
        {
            try
            {

                
                if (jbody.ContainsKey("usuario")) {

                    using (bayerEntities db = new bayerEntities())
                    {
                        Models.LoginModel usuario = JsonConvert.DeserializeObject<Models.LoginModel>(jbody["usuario"].ToString());

                        usuario.Senha = Classes.Uteis.Criptografia.Criptografar(usuario.Senha);

                        if (db.usuario.Any(i => i.login == usuario.Login && i.senha == usuario.Senha))
                            return Ok("candidato");
                        else if (db.recrutador.Any(i => i.login == usuario.Login && i.senha == usuario.Senha))
                            return Ok("recrutador");
                        else
                            return Unauthorized();
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

        [Route("esqueci_minha_senha")]
        [HttpPost]
        public IHttpActionResult PostEsqueciASenha([FromBody] JObject jbody)
        {
            try
            {
                jbody = JObject.Parse(jbody["usuario"].ToString());

                if (jbody.ContainsKey("data_nascimento") && jbody.ContainsKey("login"))
                {
                    using(bayerEntities db = new bayerEntities())
                    {
                        DateTime dataNascimento = DateTime.Parse(jbody["data_nascimento"].ToString()).Date;
                        string login = jbody["login"].ToString();

                        if (db.usuario.Any(i => i.login == login && i.data_nascimento == dataNascimento))
                        {
                            usuario usuario = db.usuario.FirstOrDefault(i => i.login == login && i.data_nascimento == dataNascimento);

                            return Ok(Classes.Uteis.Criptografia.Descriptografar(usuario.senha));
                        }
                        else if (db.recrutador.Any(i => i.login == login && i.data_nascimento == dataNascimento))
                        {
                            recrutador recrutador = db.recrutador.FirstOrDefault(i => i.login == login && i.data_nascimento == dataNascimento);

                            return Ok(Classes.Uteis.Criptografia.Descriptografar(recrutador.senha));

                        }
                        else
                           return NotFound();

                    }

                }
                else
                    return BadRequest();

            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [Route("cadastrar")]
        [HttpPost]
        public IHttpActionResult PostCadastrar([FromBody] JObject jbody) {
            try
            {
                if (jbody.ContainsKey("candidato"))
                {
                    using(bayerEntities db = new bayerEntities())
                    {
                        Models.UsuarioModel usuario = JsonConvert.DeserializeObject<Models.UsuarioModel>(jbody["candidato"].ToString());

                        if (db.usuario.Any(i => i.login == usuario.Login) ||db.recrutador.Any(i => i.login == usuario.Login))
                            return BadRequest("Usuário já cadastrado");
                        else if (string.IsNullOrEmpty(usuario.Senha))
                            return BadRequest("Senha vazia ou em branco");
                        else
                        {
                            db.usuario.Add(new usuario() { login = usuario.Login,
                                senha = Classes.Uteis.Criptografia.Criptografar(usuario.Senha),
                                email = usuario.Email,
                                sobrenome = usuario.SobreNome,
                                nome = usuario.Nome,
                                cpf = usuario.CPF,
                                tel = usuario.Tel,
                                data_nascimento = usuario.DataNascimento
                                
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
                                senha = Classes.Uteis.Criptografia.Criptografar(recrutador.Senha),
                                nome = recrutador.Nome,
                                sobrenome = recrutador.SobreNome,
                                email = recrutador.Email,
                                tel = recrutador.Tel,
                                registro = recrutador.Registro,
                                data_nascimento = recrutador.DataNascimento
                                
                                
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
