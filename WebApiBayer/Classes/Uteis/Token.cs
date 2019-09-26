using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiBayer.Classes.Uteis
{
    public class Token
    {
        public static bool ValidarToken(string token)
        {
           
            token = Criptografia.Descriptografar(token);

            string[] param = token.Split('|');

            IMongoDatabase db = Classes.Mongo.GetDatabase("teste_bayer");

            if (db.GetCollection<BsonDocument>("usuario_sistema").CountDocuments(i => i["id_usuario"] == param[0]) <= 0)
                throw new BayerException("Nenhum usuario encontrado!");
            else if(DateTime.Parse(param[0]).Month != DateTime.Today.Month)
            {
                return true;
            }

            return true;
            //Escrever log;
        }

        public static string GerarToken(string id)
        {

            return string.Empty;
        }
    }
}