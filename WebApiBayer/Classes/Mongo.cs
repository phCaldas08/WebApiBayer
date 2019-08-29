using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;

namespace WebApiBayer.Classes
{
    public class Mongo
    {
        public static IMongoDatabase GetDatabase(string dataBaseName)
        {
            try
            {
                string conexaoMongoDB = ConfigurationManager.ConnectionStrings["conexaoMongoDB"].ConnectionString;

                var client = new MongoClient(conexaoMongoDB);
                IMongoDatabase db = client.GetDatabase(dataBaseName);

                return db;
            }
            catch(Exception ex)
            {                
                return null;
            }
        }

    }
}