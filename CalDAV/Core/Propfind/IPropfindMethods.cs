﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalDAV.Utils.XML_Processors;

namespace CalDAV.Core.Propfind
{
    interface IPropfindMethods
    {
        /// <summary>
        /// Returns all dead properties and some live properties.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="collectionName"></param>
        /// <param name="calendarResourceId"></param>
        /// <param name="depth"></param>
        /// <param name="multistatusTree"></param>
        /// <returns></returns>
        void AllPropMethod(string userEmail, string collectionName, string calendarResourceId, int? depth, XMLTreeStructure multistatusTree);

        /// <summary>
        /// Returns the value of the specified properties.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="collectionName"></param>
        /// <param name="depth"></param>
        /// <param name="propFindBody"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void PropMethod(string userEmail, string collectionName, int? depth, XMLTreeStructure propFindBody, XMLTreeStructure result);

        /// <summary>
        ///  Returns the name of all the properties of a collection.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="collectionName"></param>
        /// <param name="calendarResourceId"></param>
        /// <param name="depth"></param>
        /// <param name="multistatusTree"></param>
        /// <returns></returns>
        void PropNameMethod(string userEmail, string collectionName, string calendarResourceId, int? depth, XMLTreeStructure multistatusTree);


        /// <summary>
        /// Returns the values of the specified properties in a Calendar Object Resource.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="collectionName"></param>
        /// <param name="calendarResourceId"></param>
        /// <param name="propFindBody"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        void PropObjectResource(string userEmail, string collectionName, string calendarResourceId, XMLTreeStructure propFindBody, XMLTreeStructure result);

        
    }
}
