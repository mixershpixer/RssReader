using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReader.Classes
{
    public class Source
    {
        public int SourceId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public ICollection<News> News { get; set; }
    }
}
