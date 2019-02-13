using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReader.Classes
{
    public class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Discription { get; set; }
        public string NewsUrl { get; set; }
        public int SourceId { get; set; }
        public Source Source { get; set; }

    }
}
