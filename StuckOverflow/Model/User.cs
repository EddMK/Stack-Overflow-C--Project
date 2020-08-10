using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PRBD_Framework;

namespace prbd_1920_xyy {
    public class User : EntityBase<Model> {
        
        
        
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }

        [InverseProperty(nameof(Post.Author))]
        public virtual ICollection<Post> PostWritten { get; set; } =
            new HashSet<Post>();

        protected User() { }

        public override string ToString() {
            return $"<Member: Pseudo={UserName} >";
        }
    }
}