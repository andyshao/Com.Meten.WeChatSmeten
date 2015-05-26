using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Com.Meten.WeChatSmeten.Helper
{
    public class HttpHelper
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtml(string url)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(new Uri(url));
                using (var streamReader = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    var resultString = streamReader.ReadToEnd();
                    return resultString;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static String GetHTTPPost(string url, string postData)
        {
            string result = "";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            byte[] b = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; MALCJS; rv:11.0) like Gecko";
            request.Referer = url;
            request.Method = "post";
            request.ContentLength = b.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(b, 0, b.Length);
            }

            HttpWebResponse response = null;
            try
            {
                //获取服务器返回的资源
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
            catch (WebException wex)
            {
                WebResponse wr = wex.Response;
                using (Stream st = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(st, System.Text.Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("POST请求发生异常:" + ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string UploadFile(string url, string method, string filepath)
        {
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            byte[] responseArray = myWebClient.UploadFile(url, method.ToUpper(), filepath);
            return System.Text.Encoding.Default.GetString(responseArray, 0, responseArray.Length);
        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static void DownloadFile(string url, string filepath)
        {
            WebClient myWebClient = new WebClient();
            myWebClient.DownloadFile(url, filepath);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string DownloadFilePost(string url, string filepath, string postData)
        {
            string result = "";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            byte[] b = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; MALCJS; rv:11.0) like Gecko";
            request.Referer = url;
            request.Method = "post";
            request.ContentLength = b.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(b, 0, b.Length);
            }

            HttpWebResponse response = null;
            try
            {
                //获取服务器返回的资源
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    using (Stream sr = response.GetResponseStream())
                    {
                        using (System.IO.StreamWriter objwrite = new System.IO.StreamWriter(filepath))
                        {
                            int k = 0;
                            while (k != -1)
                            {
                                k = sr.ReadByte();
                                if (k != -1)
                                {
                                    objwrite.BaseStream.WriteByte((byte)k);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError("POST请求发生异常:" + ex.Message);
            }
            return result;
        }
    }
}
