using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AdminProject.Infrastructure
{
    public class AdminDbContextInitializer : DropCreateDatabaseAlways<AdminDbContext>
    {
        protected override void Seed(AdminDbContext context)
        {

        }
    }
}