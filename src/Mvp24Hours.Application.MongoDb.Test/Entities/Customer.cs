//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Mvp24Hours.Core.Contract.Domain.Entity;
using System;

namespace Mvp24Hours.Application.MongoDb.Test.Entities
{
    public class Customer : IEntityBase
    {
        [BsonIgnore()]
        public object EntityKey => Oid;

        [BsonId()]
        public ObjectId Oid { get; set; } = ObjectId.GenerateNewId();

        [BsonElement("created")]
        [BsonRequired()]
        public DateTime Created { get; set; }

        [BsonElement("name")]
        [BsonRequired()]
        public string Name { get; set; }

        [BsonElement("active")]
        [BsonRequired()]
        public bool Active { get; set; }

    }
}
