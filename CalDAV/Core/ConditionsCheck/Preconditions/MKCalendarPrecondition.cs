﻿using System.Collections.Generic;
using System.Net;
using CalDAV.Core.Method_Extensions;
using DataLayer;
using Microsoft.AspNet.Http;
using TreeForXml;

namespace CalDAV.Core.ConditionsCheck
{
    public class MKCalendarPrecondition : IPrecondition
    {
        public MKCalendarPrecondition(IFileSystemManagement fileSystemManagement, CalDavContext context)
        {
            fs = fileSystemManagement;
            db = context;
        }

        public IFileSystemManagement fs { get; }
        public CalDavContext db { get; }

        public bool PreconditionsOK(Dictionary<string, string> propertiesAndHeaders, HttpResponse response)
        {
            #region Extracting Properties
            var body = propertiesAndHeaders["body"];
            var url = propertiesAndHeaders["url"];

            #endregion

            if (fs.ExistCalendarCollection(url) || db.CollectionExist(url))
            {
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                response.Body.Write(@"<?xml version='1.0' encoding='UTF-8'?>
<error xmlns='DAV:'>
  <resource-must-be-null/>  
</error>");
                
                return false;
            }

            if (!string.IsNullOrEmpty(body))
            {
                var bodyTree = XmlTreeStructure.Parse(body);
                if (bodyTree == null)
                {
                    response.StatusCode = (int) HttpStatusCode.Forbidden;
                    response.Body.Write("Wrong Body");
                    return false;
                }
                if (bodyTree.NodeName != "mkcalendar")
                {
                    response.StatusCode = (int) HttpStatusCode.Forbidden;
                    response.Body.Write("Wrong Body");

                    return false;
                }
            }

            return true;
        }
    }
}