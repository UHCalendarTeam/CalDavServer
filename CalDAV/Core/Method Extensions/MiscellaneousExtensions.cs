﻿using System;
using System.IO;
using System.Text;
using DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace CalDAV.Core.Method_Extensions
{
    public static class MiscellaneousExtensions
    {
        public static void Write(this Stream stream, string content)
        {
            var data = Encoding.UTF8.GetBytes(content);
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        ///     Remove the base url from the requested url. (i.e:http://...com/api/v1/caldav/)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRealUrl(this HttpRequest request)
        {
            var url = request.GetEncodedUrl();
            var host = "http://" + request.Host.Value + SystemProperties._baseUrl;
            url = url.Replace(host, "");
            return url;
        }

        /// <summary>
        /// Reads the string representation of a Stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string StreamToString(this Stream stream)
        {
            var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        ///     FOr the ACL PROFINDs the principalId is included
        ///     in the url. Takes the principalId from it.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>
        ///     Returns the PrincipalStringIdentifier. If the principal represents
        ///     a user then returns the name of the groups, otherwise returns the email.
        /// </returns>
        public static string TakePrincipalIdFromUrl(this HttpContext context)
        {
            var url = context.Request.GetRealUrl();
            var index = url.LastIndexOf("/", StringComparison.Ordinal);

            var output = url.Substring(index + 1);
            return output;
        }
        /// <summary>
        /// Returns the name of the target collection based on the url.
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <returns></returns>
        public static string GetCollectionName(this HttpRequest httpRequest)
        {
            var url = httpRequest.GetRealUrl();
            string collectionName = null;
           
            var elements = url.Split('/');
            //the word collection in the url refers to a phisical URL (calendar homes, collections or resources)
            //and the number 4 means that is at least a collection with 3 it is a calendar home.
            if (url.StartsWith("collections") && elements.Length > 4)
            {
                collectionName = elements[3];
            }
            return collectionName;
        }

        public static string GetIfMatchValues(this HttpRequest request)
        {
            var ifmatch = request.Headers["If-Match"];
            if (ifmatch.Count > 0)
            {
                return ifmatch.ToString();
            }
            return null;
        }
    }
}