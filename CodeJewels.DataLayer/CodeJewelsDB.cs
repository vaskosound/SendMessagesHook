using CodeJewels.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeJewels.DataLayer
{
    public class CodeJewelsDB : DbContext
    {

        public CodeJewelsDB()
            : base("CodeJewelsDB")
        { 
        }

        public DbSet<Email> Emails { get; set; }
    }
}
