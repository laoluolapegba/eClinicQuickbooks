using System;
using System.Configuration;
using System.Data.Entity;

namespace FCQB.Data
{
    internal class AppDbInitializer : IDatabaseInitializer<eClatModel>
    {
        void IDatabaseInitializer<eClatModel>.InitializeDatabase(eClatModel context)
        {
            string dbname = ConfigurationManager.AppSettings["SchemaName"].ToString();
            if (!context.Database.Exists())
            {
                // if database did not exist before - create it
                //context.Database.Create();
            }
        }
    }
}