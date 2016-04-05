﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CalDAV.XML_Processors;
using CalDAV.Models;
using CalDAV.Utils.XML_Processors;
using TreeForXml;
using System.Reflection;

namespace CalDAV.CALDAV_Properties
{
    public static class CollectionProperties
    {
        public static string CaldavNs => "urn:ietf:params:xml:ns:caldav";
        public static string DavNs => "DAV:";

        /// <summary>
        /// Contains all the properties that are common for all Calendar Collection.
        /// </summary>
        private static Dictionary<string, KeyValuePair<string, string>> XmlGeneralProperties = new Dictionary<string, KeyValuePair<string, string>>()
        {
             {"calendar-timezone", new KeyValuePair<string,string>("LaHabana/Cuba", CaldavNs)},
             {"max-resource-size", new KeyValuePair<string, string>("102400", CaldavNs)},
             {"min-date-time", new KeyValuePair<string, string>(MinDateTime(), CaldavNs)},
             {"max-date-time",new KeyValuePair<string, string>( MaxDateTime(), CaldavNs)},
             {"max-instances", new KeyValuePair<string,string>("10", CaldavNs)},
             {"getcontentlength", new KeyValuePair<string,string>("0", DavNs)},
             {"supported-calendar-component-set", new KeyValuePair<string, string>(@"<C:comp name=""VEVENT""/><C:comp name=""VTODO""/>",CaldavNs)},
             {"supported-calendar-data", new KeyValuePair<string, string>(@"<C:calendar-data content-type=""text/calendar""version=""2.0""/>", CaldavNs) },
             {"getetag", new KeyValuePair<string, string>("0", DavNs) }
        };


        private static List<string> VisibleGeneralProperties = new List<string>()
        {
            "displayname", "resourcetype", "creationdate", "calendar-description"
        };

        private static string MinDateTime()
        {
            var thisMonth = DateTime.Now.Month;
            var thisDay = DateTime.Now.Day;
            return
                new DateTime(DateTime.Now.Year, (thisMonth - 1)==0?12: thisMonth - 1, thisDay>28?28:thisDay).ToUniversalTime()
                    .ToString("yyyyMMddTHHmmssZ");
        }

        private static string MaxDateTime()
        {
            var thisMonth = DateTime.Now.Month;
            var thisDay = DateTime.Now.Day;
            return
                   new DateTime(DateTime.Now.Year, (thisMonth + 1) == 13?1:thisMonth+1, thisDay > 28 ? 28 : thisDay).ToUniversalTime()
                       .ToString("yyyyMMddTHHmmssZ");
        }

        /// <summary>
        /// Returns the value of a collection property given its name.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="propertyName"></param>
        /// <param name="mainNS"></param>
        /// <returns></returns>
        public static XmlTreeStructure ResolveProperty(this CalendarCollection collection, string propertyName, string mainNS = "DAV:")
        {
            
            //First I look to see if is one of the static ones.
            if (XmlGeneralProperties.ContainsKey(propertyName))
            {
                var svalue = XmlGeneralProperties[propertyName];
                var sprop = new XmlTreeStructure(propertyName, svalue.Value);
                sprop.AddValue(svalue.Key);
                return sprop;
            }
            //TODO: Check all property names
            var realPropName = propertyName.ToLower();
            string value;
            realPropName = realPropName[0].ToString().ToUpper() + realPropName.Substring(1);
            realPropName = realPropName.Replace("-", "");
            try
            {
                value = (string)collection.GetType().GetProperty(realPropName).GetValue(collection);
            }
            catch (Exception)
            {
                value = null;
                //throw new Exception("The value could not be retrieved");
            }
            XmlTreeStructure prop;
            try
            {
                if (value != null)
                    prop = (XmlTreeStructure)XmlTreeStructure.Parse(value);
                else
                    prop = new XmlTreeStructure(propertyName, mainNS);
            }
            catch (Exception e)
            {
                prop = null;
                //throw new Exception("The Property Value Could Not Be Parsed");
                throw e;
            }

            return prop;
        }

        /// <summary>
        /// Returns all the properties of a collection that must be returned for
        /// an "allprop" property method of Propfind.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static List<XmlTreeStructure> GetAllVisibleProperties(this CalendarCollection collection)
        {
            var list = new List<XmlTreeStructure>();
            foreach (var property in VisibleGeneralProperties)
            {
                list.Add(ResolveProperty(collection, property));
            }
            return list;

        }

        /// <summary>
        /// Returns the Name of all collection properties.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static List<XmlTreeStructure> GetAllPropertyNames(this CalendarCollection collection)
        {
            var list = new List<XmlTreeStructure>();

            foreach (var property in XmlGeneralProperties)
            {
                //I took the namespace from the last letter in general properties
                list.Add(new XmlTreeStructure(property.Key, property.Value.Value));
            }

            //calendar desription
            var description = new XmlTreeStructure("calendar-description", CaldavNs);
            list.Add(description);
            //a todos los que estan abajo le tienes q pasar el MainNs
            //Display Name
            var displayName = new XmlTreeStructure("displayname", DavNs);
            list.Add(displayName);

            //CreationDate
            var creationDate = new XmlTreeStructure("creationdate", DavNs);
            list.Add(creationDate);

            //resource type
            var resourceType = new XmlTreeStructure("resourcetype", DavNs);
            list.Add(resourceType);

            return list;
        }

    }
}
