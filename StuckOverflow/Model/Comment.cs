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

        public bool IsCurrentAuthor
        {
            get
            {
                //Console.WriteLine(App.CurrentUser.Role);
                return App.CurrentUser == this.UserId || App.CurrentUser.Role == Role.Admin;
            }
        }

        public string GetUserName
        {
            get
            {
                return this.UserId.UserName;
            }
        }

        public string GetAgo
        {
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

        protected Comment() { }
    }
}