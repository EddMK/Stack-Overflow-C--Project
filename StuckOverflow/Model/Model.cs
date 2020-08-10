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

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public User CreateUser(string username, string password, string fullname, string email, Role role = Role.Member) {
            var member = Users.Create();
            member.UserName = username;
            member.Password = password;
            member.FullName = fullname;
            member.Email = email;
            Users.Add(member);
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
            Posts.Add(post);
            return post;
        }

        public void SeedData() {
            if (Users.Count() == 0) {
                Console.Write("Seeding members... ");
                var admin = CreateUser("admin", "admin","administrateur" ,"admin@admin.com",Role.Admin);
                var ben = CreateUser("ben", "ben","benjamin","ben@ben.com");
                var bruno = CreateUser("bruno", "bruno","bruno","bruno@bruno.com");
                SaveChanges();
                Console.WriteLine("ok");
            }
            if (Posts.Count() == 0) {
                Console.Write("Seeding members... ");
                var q1 = CreatePost(2,"Etre ou ne pas être ?","Question philosophique",
                    DateTime.Now,0,0);
                var q2 = CreatePost(3,"Q2","Q2",
                    DateTime.Now,0,0);
                SaveChanges();
                Console.WriteLine("ok");
            }
        }
    }
}