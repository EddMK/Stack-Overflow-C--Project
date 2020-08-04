using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using PRBD_Framework;

namespace prbd_1920_xyy {
    public enum Role {
        Member = 1,
        Admin = 2
    }

    public class Model : DbContext {
        public Model() : base("prbd_1920_xyy") {
            // la base de données est supprimée et recréée quand le modèle change
            Database.SetInitializer<Model>(new DropCreateDatabaseIfModelChanges<Model>());
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Post> Posts { get; set; }

        public Member CreateMember(string pseudo, string password, Role role = Role.Member) {
            var member = Members.Create();
            member.Pseudo = pseudo;
            member.Password = password;
            member.Role = role;
            Members.Add(member);
            return member;
        }

        public Post CreatePost(int AuthorId,string Title, string Body, DateTime Date,
            int AcceptedAnswerId, int ParentId)
        {
            var post = Posts.Create();
            post.AuthorId = AuthorId;
            post.Title = Title;
            post.Body = Body;
            post.DateTime = Date;
            post.AcceptedAnswerId = AcceptedAnswerId;
            post.ParentId = ParentId;
            return post;
        }

        public void SeedData() {
            if (Members.Count() == 0) {
                Console.Write("Seeding members... ");
                var admin = CreateMember("admin", "admin", Role.Admin);
                var ben = CreateMember("ben", "ben");
                var bruno = CreateMember("bruno", "bruno");
                SaveChanges();
                Console.WriteLine("ok");
            }
            if (Posts.Count() == 0) {
                Console.Write("Seeding members... ");
                var q1 = CreatePost(2,"Etre ou ne pas être ?","Question philosophique",
                    DateTime.Now,0,0);
                SaveChanges();
                Console.WriteLine("ok");
            }
        }
    }
}