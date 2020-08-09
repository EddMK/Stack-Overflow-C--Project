using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PRBD_Framework;

namespace prbd_1920_xyy
{
    public class Post : EntityBase<Model>
    {
        [Key]
        public int PostId { get; set; }
        public  int AuthorId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
        public int AcceptedAnswerId { get; set; }
        public int ParentId { get; set; }

        public virtual Member Author { get; set; }

        protected Post() { }

        public override string ToString()
        {
            return $"{Title}";
        }

        
        public string GetAgo{
            get
            {
                DateTime now = new DateTime();
                now = DateTime.Now;

                TimeSpan interval = now - DateTime;
                string ret = "";
                if (interval.TotalDays == 0)
                {
                    if (interval.TotalHours == 0)
                    {
                        if (interval.TotalMinutes == 0)
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
