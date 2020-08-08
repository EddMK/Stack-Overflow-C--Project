using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PRBD_Framework;

namespace prbd_1920_xyy
{
    public class Post : EntityBase<Model>
    {
        [Key]
        public int PostId { get; set; }
        public  int AuthorId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
        public int AcceptedAnswerId { get; set; }
        public int ParentId { get; set; }

        public virtual Member Author { get; set; }

        protected Post() { }

        public override string ToString()
        {
            return $"{Title}";
        }
    }
}
