using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Newtonsoft.Json.Serialization;

namespace Naylor.Gateway.Extensibility
{
    public class Response : Nancy.Response
    {
        public object Data { get; set; }

        public string Message { get; set; }

        public Response()
        {
            ContentType = "application/json";
            Contents = s =>
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(GetResponseBodyObject(), settings);
                var jsonBytes = Encoding.UTF8.GetBytes(json);
                s.Write(jsonBytes, 0, jsonBytes.Length);
            };
        }
        public Response(object data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Data = data;
            StatusCode = statusCode;
            ContentType = "application/json";
            Contents = s =>
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(GetResponseBodyObject(), settings);
                var jsonBytes = Encoding.UTF8.GetBytes(json);
                s.Write(jsonBytes, 0, jsonBytes.Length);
            };
        }

        private object GetResponseBodyObject()
        {
            var bodyObj = new
            {
                Data = this.Data,
                Message = this.Message
            };

            return bodyObj;
        }
    }
}
