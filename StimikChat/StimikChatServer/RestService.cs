﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StimikChatServer
{
    public class RestService : HttpClient
    {

        public RestService()
        {

            // this.MaxResponseContentBufferSize = 256000;
            //var a = ConfigurationManager.AppSettings["IP"];
            string _server = "http://restsimak.stimiksepnop.ac.id/";
            this.BaseAddress = new Uri(_server);
            this.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //key api = 57557c4f25f436213fe34a2090a266e2
        }

        public RestService(string apiUrl)
        {
            this.BaseAddress = new Uri(apiUrl);

        }

        public void SetToken(string token)
        {
            if (token != null)
            {
                this.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                    token);
                //this.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Basic", token);
            }
        }
        public StringContent GenerateHttpContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }
    }

    internal class NameValueCollection
    {
        internal void Add(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }

    public class UserAccount
    {
        public string IdUser { get; set; }
        public string Token { get; set; }
        public string NamaUser { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public RoleUser RoleUser { get; set; }
    }


    public class RoleUser
    {
        public List<Role> Role { get; set; }
    }

    public class Role
    {
        public string Nama { get; set; }
    }

    public class ResponseResult
    {
        public bool status { get; set; }
        public object data { get; set; }
        public string message { get; set; }

    }
}
