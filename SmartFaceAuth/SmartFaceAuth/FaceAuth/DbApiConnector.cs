using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using Newtonsoft.Json;
using SmartFaceAuth.Models;

namespace SmartFaceAuth.FaceAuth
{
    public class DbApiConnector
    {
        public Log AppendLog(Log inputLog)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebConfigurationManager.AppSettings["ApiServerAddress"] + "/log/addlog");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(inputLog);

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<Log>(result);
            }
        }

        public Group AddGroup(Group inputGroup)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebConfigurationManager.AppSettings["ApiServerAddress"] + "/group/addgroup");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(inputGroup);

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<Group>(result);
            }
        }

        public Group AddPerson(Guid groupGuid, Person inputPerson)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebConfigurationManager.AppSettings["ApiServerAddress"] + "/group/addperson/" + groupGuid);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(inputPerson);

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<Group>(result);
            }
        }

        public bool RemovePerson(Guid groupGuid, Guid personGuid)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(WebConfigurationManager.AppSettings["ApiServerAddress"] + "/group/rmperson/" + groupGuid);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(personGuid);

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<bool>(result);
            }
        }

        public Person GetPerson(Guid personGuid)
        {
            string requestUrl = WebConfigurationManager.AppSettings["ApiServerAddress"] + "/group/person/" + personGuid;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<Person>(result);
            }
        }

        public Group GetGroup(string email)
        {
            string requestUrl = WebConfigurationManager.AppSettings["ApiServerAddress"] + "/group/byemail";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(email);

                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<Group>(result);
            }
        }

        public Group SetTrained(bool inputTrained, Guid groupGuid)
        {
            if (inputTrained)
            {
                string requestUrl = WebConfigurationManager.AppSettings["ApiServerAddress"] + "/group/settrained/" + groupGuid.ToString();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Group>(result);
                }
            }
            else
            {
                string requestUrl = WebConfigurationManager.AppSettings["ApiServerAddress"] + "/group/setnontrained/" + groupGuid.ToString();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Group>(result);
                }
            }
        }

        public bool GetIsTrained(Guid groupGuid)
        {
            string requestUrl = WebConfigurationManager.AppSettings["ApiServerAddress"] + "/group/istrained/" + groupGuid.ToString();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<bool>(result);
            }
        }

        public Log[] GetLogsList(Guid groupGuid)
        {
            string requestUrl = WebConfigurationManager.AppSettings["ApiServerAddress"] + "/log/list/" + groupGuid.ToString();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<Log[]>(result);
            }
        }
    }
}