using System;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Infrastructure.AdminDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Infrastructure.AdminDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //context.Admins.Add(new Admin
            //{
            //    Authorization = "Setting",
            //    CreatedDate = DateTime.Now,
            //    LastLoginDate = new DateTime(1970, 1, 1),
            //    Name = "Default User",
            //    Username = "admin",
            //    Password = "rubyadmin",
            //    Status = Models.StatusTypes.Active
            //});

            //context.Languages.AddOrUpdate(
            //    new Language { Name = "Türkçe", UrlTag = "tr", Status = Models.StatusTypes.Active },
            //    new Language { Name = "English", UrlTag = "en", Status = Models.StatusTypes.Active },
            //    new Language { Name = "Russian", UrlTag = "ru", Status = Models.StatusTypes.Deactive }
            //    );

            //context.SaveChanges();
        }
    }
}
