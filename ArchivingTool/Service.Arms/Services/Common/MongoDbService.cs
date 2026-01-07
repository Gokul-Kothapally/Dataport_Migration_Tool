using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArchivingTool.Service.Arms.Services.Common
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<BsonDocument> _citationCollection;

        public MongoDbService(IConfiguration configuration)
        {
            var connectionString = configuration["ApplicationSettings:MongoConnectionString"];
            var databaseName = configuration["ApplicationSettings:MongoDatabase"];
            var collectionName = configuration["ApplicationSettings:MongoCollection"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);

            _citationCollection = _database.GetCollection<BsonDocument>(collectionName);
        }

        public async Task InsertCitationsAsync(List<Dictionary<string, object>> citations)
        {
            var bsonDocs = new List<BsonDocument>();

            foreach (var dict in citations)
            {
                var doc = new BsonDocument();

                foreach (var kvp in dict)
                {
                    if (kvp.Value == null || kvp.Value == DBNull.Value)
                    {
                        doc[kvp.Key] = BsonNull.Value;
                    }
                    else if (kvp.Value is Guid guid)
                    {
                        doc[kvp.Key] = guid.ToString();
                    }
                    else if (kvp.Value is string str)
                    {
                        doc[kvp.Key] = str.Trim();
                    }
                    else
                    {
                        doc[kvp.Key] = BsonValue.Create(kvp.Value);
                    }
                }


                bsonDocs.Add(doc);
            }

            await _citationCollection.InsertManyAsync(bsonDocs);
        }

        public async Task InsertCasesAsync(List<Dictionary<string, object>> cases, string collectionName)
        {
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            var bsonDocs = new List<BsonDocument>();

            foreach (var dict in cases)
            {
                var json = JsonConvert.SerializeObject(dict);
                var doc = BsonDocument.Parse(json);
                bsonDocs.Add(doc);
            }

            if (bsonDocs.Count > 0)
            {
                await collection.InsertManyAsync(bsonDocs);
            }
        }

        public async Task InsertDocumentsAsync(List<object> records, string collectionName)
        {
            if (string.IsNullOrWhiteSpace(collectionName))
                throw new ArgumentException("Collection name must be provided.");

            var collection = _database.GetCollection<BsonDocument>(collectionName);
            var bsonDocs = new List<BsonDocument>();

            foreach (var record in records)
            {
                if (record == null)
                    continue;

                BsonDocument doc;

                // Case 1: Flat dictionary
                if (record is Dictionary<string, object> flatDict)
                {
                    doc = new BsonDocument();
                    foreach (var kvp in flatDict)
                    {
                        if (kvp.Value == null || kvp.Value == DBNull.Value)
                            doc[kvp.Key] = BsonNull.Value;
                        else if (kvp.Value is Guid guid)
                            doc[kvp.Key] = guid.ToString();
                        else if (kvp.Value is string str)
                            doc[kvp.Key] = str.Trim();
                        else
                            doc[kvp.Key] = BsonValue.Create(kvp.Value);
                    }
                }
                // Case 2: Already a BsonDocument (safety)
                else if (record is BsonDocument bson)
                {
                    doc = bson;
                }
                // Case 3: Nested structure (from JSON string or anonymous object)
                else
                {
                    try
                    {
                        string json = JsonConvert.SerializeObject(record);
                        doc = BsonDocument.Parse(json);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error serializing record: {ex.Message}");
                        continue;
                    }
                }

                bsonDocs.Add(doc);
            }

            if (bsonDocs.Count > 0)
            {
                await collection.InsertManyAsync(bsonDocs);
            }
        }

    }
}
