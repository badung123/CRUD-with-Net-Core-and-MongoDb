using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApi.Models;

namespace UserApi.Services
{
    

    public class UserSevice
    {
        private readonly IMongoCollection<UserDetail> _users;
        public UserSevice(IUserInfoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<UserDetail>(settings.UsersCollectionName);
        }
        public List<UserDetail> Get() =>
            _users.Find(user => true).ToList();
        public UserDetail Get(ObjectId id) =>
            _users.Find<UserDetail>(user => user.Id == id).FirstOrDefault();
        public UserDetail Create(UserDetail user)
        {
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            _users.InsertOne(user);
            return user;
        }

    }
}
