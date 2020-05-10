using System;
using System.IO;
using System.Net;
using System.Text;
using NUnit.Framework.Interfaces;

namespace SimpleApp.Appium.Core
{
    public class SessionStatus
    {
        string sessionId;
        public SessionStatus(String sessionId)
        {
            this.sessionId = sessionId;
        }

        internal void changeSessionStatus(string status, string reason, string username, string accesskey)
        {
            string reqString = "{\"status\":\""+ status + "\", \"reason\":\" " + reason+ "\"}";
            Console.WriteLine(reqString);

            byte[] requestData = Encoding.UTF8.GetBytes(reqString);
            Uri myUri = new Uri(string.Format("https://www.browserstack.com/app-automate/sessions/" + this.sessionId + ".json"));
            WebRequest myWebRequest = HttpWebRequest.Create(myUri);
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)myWebRequest;
            myWebRequest.ContentType = "application/json";
            myWebRequest.Method = "PUT";
            myWebRequest.ContentLength = requestData.Length;
            using (Stream st = myWebRequest.GetRequestStream()) st.Write(requestData, 0, requestData.Length);
           
            NetworkCredential myNetworkCredential = new NetworkCredential(username, accesskey);
            CredentialCache myCredentialCache = new CredentialCache();
            myCredentialCache.Add(myUri, "Basic", myNetworkCredential);
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Credentials = myCredentialCache;

            myWebRequest.GetResponse().Close();
        }
    }
}
