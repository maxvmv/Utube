using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Utube.Models
{
    public class Comment
    {
        public int Id { set; get; }
        public string Content { set; get; }
        public DateTime Datein { set; get; }
        public int Videid { set; get; }
        public Guid Userid { set; get; }
    }
}