﻿using System;
using System.Collections.Generic;
using DataLayer;
using Microsoft.AspNet.Http;
using Microsoft.Data.Entity;

namespace CalDAV.Core.ConditionsCheck
{
    public class GetPoscondition : IPoscondition
    {
        public GetPoscondition(DbContext db, IFileSystemManagement fs)
        {
            Db = db;
            Fs = fs;
        }

        public IFileSystemManagement Fs { get; }
        public DbContext Db { get; }

        public bool PosconditionOk(Dictionary<string, string> propertiesAndHeaders, HttpResponse response)
        {
            throw new NotImplementedException();
        }
    }
}