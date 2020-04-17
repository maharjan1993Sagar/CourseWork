using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coursework02.Models
{
    public class DVD
    {
        public int Id { get; set; }
        public string CopyNo { get; set; }
        public int AlbumsId { get; set; }
        public virtual Albums Albums { get; set; }

    }
}