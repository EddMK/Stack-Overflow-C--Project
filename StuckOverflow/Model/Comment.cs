using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRBD_Framework;

namespace prbd_1920_xyy
{
    public class Comment : EntityBase<Model>
    {
        [Key]
        public int CommentId { get; set; }

        public virtual User UserId { get; set; }
        public virtual Post PostId { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }

        protected Comment() { }
    }
}