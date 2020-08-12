using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRBD_Framework;

namespace prbd_1920_xyy
{
    public class Vote : EntityBase<Model>
    {
        [Key]
        public int VoteId { get; set; }

        public virtual User UserId { get; set; }
        public virtual Post PostId { get; set; }
        public int UpDown { get; set; }

        protected Vote() { }
    }
}