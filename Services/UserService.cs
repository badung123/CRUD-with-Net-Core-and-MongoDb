using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UserApi.Models;
using xNet;

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
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Phone)
                || string.IsNullOrEmpty(user.Email))
            {
                return new UserDetail();
            }
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            _users.InsertOne(user);
            return user;
        }
        public long GetCurBalance(string url)
        {
            var uri = "http://api.thuongtin.xyz/auth/login";
            HttpRequest httpRequest = new HttpRequest();
            var userArgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";
            httpRequest.UserAgent = userArgent;
            string userName = "lenam123";
            string password = "lenam123";
            string data = "&authUsername=" + WebUtility.UrlEncode(userName) + "&authPassword=" + WebUtility.UrlEncode(password);
            //login with username,password
            var html = PostData(httpRequest, uri, data, "application/x-www-form-urlencoded; charset=UTF-8").ToString();

            //get responce after login success
            var result_login = JsonConvert.DeserializeObject<Responce>(html);
            //add Authorization
            httpRequest.Authorization = "Bearer " + result_login.payload.accessToken;

            //get data by url api
            var responce = JsonConvert.DeserializeObject<Responce>(httpRequest.Get(url).ToString());

            //get current balance
            var curbalance = long.Parse(responce.payload.curBalance.ToString());
            return curbalance;
        }
        public class Responce
        {
            public bool isSuccess { get; set; }
            public int errorCode { get; set; }
            public string errorMessage { get; set; }
            public dynamic errorDetail { get; set; }
            public dynamic payload { get; set; }
        }
        private string PostData(HttpRequest http, string url, string data = null, string contentType = null, string userArgent = "", string cookie = null)
        {
            if (http == null)
            {
                http = new HttpRequest();
                http.Cookies = new CookieDictionary();
            }

            if (!string.IsNullOrEmpty(cookie))
            {
                AddCookie(http, cookie);
            }

            if (!string.IsNullOrEmpty(userArgent))
            {
                http.UserAgent = userArgent;
            }

            string html = http.Post(url, data, contentType).ToString();
            return html;
        }
        private void AddCookie(HttpRequest http, string cookie)
        {
            var temp = cookie.Split(';');
            foreach (var item in temp)
            {
                var temp2 = item.Split('=');
                if (temp2.Count() > 1)
                {
                    http.Cookies.Add(temp2[0], temp2[1]);
                }
            }
        }

    }
}
