﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PRBD_Framework;

namespace prbd_1920_xyy
{
    public class Post : EntityBase<Model>
    {
        [Key]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
        public virtual Post AcceptedAnswerId { get; set; }

        public virtual User AuthorId { get; set; }

        public virtual Post ParentId { get; set; }



        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

        [InverseProperty(nameof(Post.ParentId))]
        public virtual ICollection<Post> AnsweredQuestions { get; set; } =
            new HashSet<Post>();

        [InverseProperty(nameof(Vote.PostId))]
        public virtual ICollection<Vote> Votes { get; set; } =
            new HashSet<Vote>();

        protected Post() { }

        public override string ToString()
        {
            return $"{Title}";
        }

        public string GetScore
        {
            get
            {
                if(Votes.Count == 0)
                {
                    return "0 score";
                }
                else
                {
                    int somme = 0;
                    foreach(Vote v in Votes)
                    {
                        somme += v.UpDown;
                    }
                    return somme + " score(s)";
                }
            }
        }

        public int GetScores
        {
            get
            {
                if (Votes.Count == 0)
                {
                    return 0;
                }
                else
                {
                    int somme = 0;
                    foreach (Vote v in Votes)
                    {
                        somme += v.UpDown;
                    }
                    return somme;
                }
            }
        }

        public bool IsCurrentAuthor
        {
            get
            {
                return App.CurrentUser == this.AuthorId;
            }
        }
        
        public bool IsAccepted
        {
            get
            {
                Post v = App.Model.Posts.SingleOrDefault(post => post.AcceptedAnswerId.PostId == this.PostId);
                
                if (v == null)
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Accepted " + this.Body);
                    return true;
                }
            }
        }

        public bool IsNotAccepted
        {
            get
            {
                return !IsAccepted;
            }
        }

        public string GetAgo{
            get
            {
                DateTime now = new DateTime();
                now = DateTime.Now;

                TimeSpan interval = now - DateTime;
                string ret = "";
                if (interval.TotalDays <= 1)
                {
                    
                    if (interval.TotalHours <= 1)
                    {
                        if (interval.TotalMinutes <= 1)
                        {
                            ret = interval.Seconds + " secondes ago ";
                        }
                        else
                        {
                            ret = interval.Minutes + " minutes ago ";
                        }
                    }
                    else
                    {
                        ret = interval.Hours + " hours ago ";
                    }
                }
                else
                {
                    if (interval.TotalDays >= 365)
                    {
                        int nbr = (int)interval.TotalDays / 365;
                        ret = nbr + " years ago ";
                    }
                    else
                    {
                        if (interval.TotalDays < 30)
                        {
                            int nbr = (int)interval.TotalDays / 7;
                            ret = nbr + " weeks ago ";

                            if (interval.TotalDays < 7)
                            {
                                ret = interval.Days + " days ago ";
                            }
                        }
                        else
                        {
                            int nbr = (int)interval.TotalDays / 30;
                            ret = nbr + " months ago ";
                        }
                    }
                }
                return ret;

            }
        }


    }
}
