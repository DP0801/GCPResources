using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GetGCPResourceClient
{
    public class HttpHelper
    {
        public static ILog log = LogManager.GetLogger(typeof(HttpHelper));

        /// <summary>
        /// Sends the web request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="data">The data.</param>
        /// <param name="method">The method.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// Web request response
        /// </returns>
        /// <exception cref="Exception">Web request exception.</exception>
        public static string SendWebRequest(string url, List<KeyValuePair<string, string>> headers, string data = null, string method = "GET", string contentType = "application/json")
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            string response = string.Empty;
            string error = string.Empty;
            log.InfoFormat("Put Request Data: {0}", data);
            try
            {
                req.Method = method;
                req.Timeout = 120000;
                req.ContentType = contentType;
                foreach (var header in headers)
                {
                    req.Headers[header.Key] = header.Value;
                }

                if (!string.IsNullOrWhiteSpace(data))
                {
                    byte[] sentData = Encoding.UTF8.GetBytes(data);
                    req.ContentLength = sentData.Length;

                    using (System.IO.Stream sendStream = req.GetRequestStream())
                    {
                        sendStream.Write(sentData, 0, sentData.Length);
                        sendStream.Close();
                    }
                }
                else
                {
                    req.ContentLength = 0;
                }

                System.Net.WebResponse res = req.GetResponse();
                System.IO.Stream receiveStream = res.GetResponseStream();
                using (System.IO.StreamReader sr = new
                System.IO.StreamReader(receiveStream, Encoding.UTF8))
                {
                    char[] read = new char[256];
                    int count = sr.Read(read, 0, 256);

                    while (count > 0)
                    {
                        string str = new string(read, 0, count);
                        response += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                error = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
                log.Error(error, ex);
                throw ex;
            }
            catch (WebException ex)
            {
                error = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
                if (ex.Response != null)
                {
                    Stream responseStream = ex.Response.GetResponseStream();
                    var responseString = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();

                    log.ErrorFormat("Web exception while sending azure request : {0}", responseString);

                    throw new Exception(responseString, ex);
                }

                log.Error(error, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                error = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
                log.Error(error, ex);
                throw ex;
            }

            log.Info(response);
            Console.WriteLine(error);

            return response;
        }
    }
}
