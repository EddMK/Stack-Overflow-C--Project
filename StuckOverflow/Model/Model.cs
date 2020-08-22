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
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public User CreateUser(string username, string password, string fullname, string email, Role role = Role.Member) {
            var member = Users.Create();
            member.UserName = username;
            member.Password = password;
            member.FullName = fullname;
            member.Email = email;
            Users.Add(member);
            return member;
        }

        public Post CreatePost(User AuthorId,string Title, string Body, DateTime Date,
            Post AcceptedAnswerId, Post ParentId)
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

        public Post CreateQuestion(User AuthorId, string Title, string Body, DateTime Date,
            Post AcceptedAnswerId)
        {
            var post = Posts.Create();
            post.AuthorId = AuthorId;
            post.Title = Title;
            post.Body = Body;
            post.DateTime = Date;
            post.AcceptedAnswerId = AcceptedAnswerId;
            Posts.Add(post);
            return post;
        }

        public Vote CreateVote(User User, Post Post, int UpDown)
        {
            var vote = Votes.Create();
            vote.UserId = User;
            vote.PostId = Post;
            vote.UpDown = UpDown;
            User.Votes.Add(vote);
            Post.Votes.Add(vote);
            Votes.Add(vote);
            return vote;
        }

        public Post CreateAnswer(User AuthorId, string Body, DateTime Date, Post ParentId)
        {
            var post = Posts.Create();
            post.AuthorId = AuthorId;
            post.Body = Body;
            post.DateTime = Date;
            post.ParentId = ParentId;
            ParentId.AnsweredQuestions.Add(post);
            Posts.Add(post);
            return post;
        }

        public Tag CreateTag(string tagName)
        {
            var tag = Tags.Create();
            tag.TagName = tagName;
            Tags.Add(tag);
            return tag;
        }

        public void SeedData()
        {
            if (Users.Count() == 0)
            {
                Console.Write("Seeding members... ");
                var admin = CreateUser("admin", "admin", "administrateur", "admin@admin.com", Role.Admin);
                var ben = CreateUser("ben", "ben", "benjamin", "ben@ben.com");
                var bruno = CreateUser("bruno", "bruno", "bruno", "bruno@bruno.com");
                SaveChanges();
                Console.WriteLine("ok");
            }
            if (Posts.Count() == 0)
            {
                Console.Write("Seeding members... ");
                var member = App.Model.Users.Find(2);
                var member2 = App.Model.Users.Find(3);
                var q1 = CreateQuestion(member, "Etre ou ne pas être ?", "Question philosophique",
                    DateTime.Now, null);
                member.PostWritten.Add(q1);

                var q3 = CreateAnswer(member, "Etre",
                    DateTime.Now, q1);
                member.PostWritten.Add(q3);

                var q2 = CreateQuestion(member2, "Q2", "Q2",
                    DateTime.Now, null);
                member2.PostWritten.Add(q2);
                SaveChanges();
                Console.WriteLine("ok");
            }
            if (Votes.Count() == 0)
            {
                
                Console.Write("Seeding members... ");
                var member = App.Model.Users.Find(1);
                var member2 = App.Model.Users.Find(2);
                var member3 = App.Model.Users.Find(3);
                var p1 = App.Model.Posts.Find(1);
                var p2 = App.Model.Posts.Find(2);
                var p3 = App.Model.Posts.Find(3);
                var v1 = CreateVote(member, p1, 1);
                var v2 = CreateVote(member2, p1, 1);
                var v3 = CreateVote(member3, p1, -1);
                var v4 = CreateVote(member2, p3, 1);
                var v5 = CreateVote(member3, p2, 1);
                SaveChanges();
                Console.WriteLine("ok");
                
            }
            if (Tags.Count() == 0)
            {
                Console.Write("Seeding members... ");
                var t1 = CreateTag("Angular");
                var t2 = CreateTag("Java");
                var t3 = CreateTag("Php");
                var t4 = CreateTag("Sql");
                var t5 = CreateTag("C++");
                SaveChanges();
                Console.WriteLine("ok");

            }

        }
    }
}