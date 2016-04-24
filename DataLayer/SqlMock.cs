﻿using System.IO;
using System.Threading.Tasks;
using DataLayer.Models.ACL;
using DataLayer.Models.Entities;
using Microsoft.AspNet.Http;

namespace DataLayer
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public static class SqlMock
    {
        /// <summary>
        /// This is method is used to seed the database with some fictional values.
        /// </summary>
        public static void SeedDb_Fs()
        {
            using (var db = new CalDavContext())
            {
                var fs = new FileSystemManagement();
                var frank = new User("Frank", "f.underwood@wh.org");
                var frankPrincipal = new Principal("f.underwood@wh.org", SystemProperties.PrincipalType.User)
                {
                    User = frank
                };

                var frankCollection = new CalendarCollection($"collections/users/f.underwood@wh.org/durtyplans/", "durtyplans");
                var assesinationEvent = new CalendarResource("collections/users/f.underwood@wh.org/durtyplans/russodies.ics", "russodies.ics");
                
                frankCollection.CalendarResources.Add(assesinationEvent);
                frankPrincipal.CalendarCollections.Add(frankCollection);
                db.Principals.Add(frankPrincipal);
                
                fs.AddPrincipalFolder(SystemProperties._userCollectionUrl + frank.Email);
                fs.AddCalendarCollectionFolder(frankCollection.Url);

                #region Body
                var body = @"BEGIN:VCALENDAR
PRODID:-//Google Inc//Google Calendar 70.9054//EN
VERSION:2.0
CALSCALE:GREGORIAN
BEGIN:VTIMEZONE
TZID:Cuba/La Habana
END:VTIMEZONE
BEGIN:VEVENT
DTSTART;TZID=America/Los_Angeles:20120629T130000
DTEND;TZID=America/Los_Angeles:20120629T140000
DTSTAMP:20120629T112428Z
UID:0kusnhnnacaok1r02v16simh8c@google.com
CREATED:20120629T111935Z
DESCRIPTION:foo
LAST-MODIFIED:20120629T112428Z
LOCATION:Barcelona
SEQUENCE:0
STATUS:CONFIRMED
SUMMARY:Demo B2G Calendar
BEGIN:VALARM
ACTION:EMAIL
DESCRIPTION:This is an event reminder
SUMMARY:Alarm notification
ATTENDEE:mailto:calmozilla1@gmail.com
TRIGGER:-P0DT0H30M0S
END:VALARM
BEGIN:VALARM
ACTION:DISPLAY
DESCRIPTION:This is an event reminder
TRIGGER:-P0DT0H30M0S
END:VALARM
END:VEVENT
END:VCALENDAR
";
                #endregion

                
                fs.AddCalendarObjectResourceFile(assesinationEvent.Href, body);

                var claire = new User("Claire", "c.underwood@wh.org");
                var clairePrincipal = new Principal(claire.Email, SystemProperties.PrincipalType.User) {User = claire};

                db.Principals.Add(clairePrincipal);
                fs.AddPrincipalFolder(SystemProperties._userCollectionUrl+claire.Email);

                db.SaveChanges();
            }

        }

        public static void RecreateDb()
        {
            using (var db = new CalDavContext())
            {
                var fs = new FileSystemManagement();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.SaveChanges();
                fs.DestroyAll();
            }
        }

        public static void DestroyAll(this IFileSystemManagement fs)
        {
            if (Directory.Exists("collections"))
                Directory.Delete("collections", true);
        }
    }
}