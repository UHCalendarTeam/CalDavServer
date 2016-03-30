using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using CalDAV.Models;

namespace CalDAV.Migrations
{
    [DbContext(typeof(CalDavContext))]
    [Migration("20160326045720_ChangingPropNamesForEasyResolve")]
    partial class ChangingPropNamesForEasyResolve
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CalDAV.Models.CalendarCollection", b =>
                {
                    b.Property<int>("CalendarCollectionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Calendardescription");

                    b.Property<string>("Creationdate");

                    b.Property<string>("Displayname");

                    b.Property<string>("GetContenttype");

                    b.Property<string>("Getcontentlanguage");

                    b.Property<string>("Getetag");

                    b.Property<string>("Getlastmodified");

                    b.Property<string>("Lockdiscovery");

                    b.Property<string>("Name");

                    b.Property<string>("Resourcetype");

                    b.Property<string>("Supportedlock");

                    b.Property<string>("Url")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("CalendarCollectionId");
                });

            modelBuilder.Entity("CalDAV.Models.CalendarResource", b =>
                {
                    b.Property<int>("CalendarResourceId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CollectionCalendarCollectionId");

                    b.Property<string>("Creationdate");

                    b.Property<string>("Displayname");

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.Property<string>("Getcontentlanguage");

                    b.Property<string>("Getcontentlength");

                    b.Property<string>("Getcontenttype");

                    b.Property<string>("Getetag");

                    b.Property<string>("Getlastmodified");

                    b.Property<string>("Lockdiscovery");

                    b.Property<string>("Supportedlock");

                    b.Property<string>("Uid");

                    b.Property<int>("UserId");

                    b.HasKey("CalendarResourceId");
                });

            modelBuilder.Entity("CalDAV.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName");

                    b.HasKey("UserId");
                });

            modelBuilder.Entity("CalDAV.Models.CalendarCollection", b =>
                {
                    b.HasOne("CalDAV.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CalDAV.Models.CalendarResource", b =>
                {
                    b.HasOne("CalDAV.Models.CalendarCollection")
                        .WithMany()
                        .HasForeignKey("CollectionCalendarCollectionId");

                    b.HasOne("CalDAV.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}