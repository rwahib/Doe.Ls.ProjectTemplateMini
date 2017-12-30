using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Doe.Ls.EntityBase.Http
    {
    public static class HttpHelper
        {
        public static WebResponse GetResponse(string uri, HttpVerbs verb = HttpVerbs.Get,
            Dictionary<string, string> headers = null)
            {
            var httpRequest = WebRequest.Create(uri) as HttpWebRequest;
            if(headers != null && headers.Count > 0)
                {
                foreach(var headerKey in headers.Keys)
                    {
                    httpRequest.Headers.Add(headerKey, headers[headerKey]);
                    }

                }

            httpRequest.Method = verb.ToString().ToUpper();
            return httpRequest.GetResponse();

            }

        public static WebResponse GetResponse(string uri, string requestBody, HttpVerbs verb = HttpVerbs.Get,
            Dictionary<string, string> headers = null)
            {

            var httpRequest = WebRequest.Create(uri) as HttpWebRequest;
            WebResponse response;
            if(headers != null && headers.Count > 0)
                {
                foreach(var headerKey in headers.Keys)
                    {
                    httpRequest.Headers.Add(headerKey, headers[headerKey]);
                    }

                }

            httpRequest.Method = verb.ToString().ToUpper();
            var bytes = Encoding.Unicode.GetBytes(requestBody);
            httpRequest.ContentLength = bytes.Length;
            httpRequest.ContentType = "application/json";

            var reqStream = httpRequest.GetRequestStream();
            reqStream.Write(bytes, 0, bytes.Length);
            response = httpRequest.GetResponse();

            return response;
            }

        public static string GetString(Stream stream)
            {
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();

            }

        public static string GetAppUrl(bool fromConfig = false)
            {
            if(fromConfig)
                {
                return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["AppUrl"].Trim();
                }
            else
                {
                if(HttpContext.Current != null)
                    {
                    var requestUrl = HttpContext.Current.Request.Url;
                    var virtualPath = string.Empty;
                    if(HttpRuntime.AppDomainAppVirtualPath != "/")
                        {
                        virtualPath = HttpRuntime.AppDomainAppVirtualPath;
                        }
                    return $"{requestUrl.Scheme}://{requestUrl.Authority}{virtualPath}/";
                    }
                else
                    {
                    return (ConfigurationManager.GetSection(@"site") as NameValueCollection)["AppUrl"].Trim();
                    }
                }
            }

        public static string GetActionUrl(string actionName, string controlerName, object parameters = null, bool fromConfig = true)
            {
            var parameterString = GetParameterString(parameters);
            return string.IsNullOrWhiteSpace(parameterString) ? $"{GetAppUrl(fromConfig)}{controlerName}/{actionName}" : $"{GetAppUrl(fromConfig)}{controlerName}/{actionName}?{parameterString}";
            }
        public static string GetAbsoluteUrl(string relativePath, object parameters = null, bool fromConfig = true)
            {
            var parameterString = GetParameterString(parameters);
            return string.IsNullOrWhiteSpace(parameterString) ? $"{GetAppUrl(fromConfig)}{relativePath}" : $"{GetAppUrl(fromConfig)}{relativePath}?{parameterString}";
            }
        /// <summary>
        /// Add missing web separators as Path does in folders and files 
        /// </summary>
        /// <param name="urls">base url, others</param>
        /// <returns></returns>
        public static string GetAbsoluteUri(params string[] urls)
            {
            if(urls == null)
                {
                throw new ArgumentNullException($"{nameof(urls)} should have value");

                }
            else
                {
                if(urls.Length <= 1)
                    {
                    throw new ArgumentNullException($"{nameof(urls)} should have at least 2 urls");

                    }
                }
            var url = urls[0].TrimEnd('/');
            url = url + "/";
          var builder = new UriBuilder(url);
            for(var i = 1; i < urls.Length; i++)
                {
                url = urls[i].TrimEnd('/').TrimStart('/')+"/";
                builder.Path += url;
                }
            builder.Path=builder.Path.TrimEnd('/');
            return builder.Uri.AbsoluteUri;
            }

        public static UriBuilder GetAbsoluteUriBuilder(params string[] urls)
            {
            var result = GetAbsoluteUri(urls);

            return new UriBuilder(result);
            }

        public static string GetParameterString(object parameters)
            {
            if(parameters == null) return string.Empty;
            var sb = new StringBuilder();
            var pType = parameters.GetType();
            var propList = pType.GetProperties().ToList();

            foreach(var propertyInfo in propList)
                {
                var val = propertyInfo.GetValue(parameters).ToString();
                sb.Append($"{propertyInfo.Name}={val}&");
                }

            return sb.ToString().TrimEnd('&');
            }
        }
    }