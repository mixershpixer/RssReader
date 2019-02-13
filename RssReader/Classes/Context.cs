using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReader.Classes
{
    class NewsContext : DbContext
    {
        public NewsContext()
            : base("DBConnection")
        { }

        public DbSet<News> News { get; set; }
    }

    class SourceContext : DbContext
    {
        public SourceContext()
            : base("DBConnection")
        { }

        public DbSet<Source> Sources { get; set; }
    }
}
