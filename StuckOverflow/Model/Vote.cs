using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PRBD_Framework;

namespace prbd_1920_xyy
{
    public class Vote : EntityBase<Model>
    {
        [Key, Column(Order = 0)]
        public Post PostId { get; set; }
        [Key, Column(Order = 1)]
        public User UserId { get; set; }
        public int UpDown { get; set; }


        protected Vote() { }
    }
}