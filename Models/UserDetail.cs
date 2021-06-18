using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserApi.Models
{
    public class UserInfoDatabaseSettings : IUserInfoDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IUserInfoDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class UserDetail
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement]
        [BsonRequired]
        [BsonDateTimeOptions]
        public DateTime DateOfBirth { get; set; }

        [BsonElement]
        [BsonRequired]
        public string Email { get; set; }

        [BsonElement]
        [BsonRequired]
        public string Phone { get; set; }
        [BsonElement]
        [BsonRequired]
        public double MathScore { get; set; }
        [BsonElement]
        [BsonRequired]
        public double PhysicsScore { get; set; }
        [BsonElement]
        public double ChemistryScore { get; set; }
        [BsonElement]
        [BsonRequired]
        public double MediumScore
        {
            get
            {
                return (MathScore + PhysicsScore + ChemistryScore) / 3.0;
            }
        }
        [BsonElement]
        public DateTime CreatedAt { get; set; }
        [BsonElement]
        public DateTime UpdatedAt { get; set; }
        [BsonElement]
        public int Age
        {
            get
            {
                return DateTime.Now.Year - DateOfBirth.Year + 1;
            }
        }
    }
}
