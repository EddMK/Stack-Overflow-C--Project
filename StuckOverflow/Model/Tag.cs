using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PRBD_Framework;

namespace prbd_1920_xyy
{
    public class Tag : EntityBase<Model>
    {
        [Key]
        public int TagId { get; set; }
        public string TagName { get; set; }

        protected Tag() { }

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();

    }
}
