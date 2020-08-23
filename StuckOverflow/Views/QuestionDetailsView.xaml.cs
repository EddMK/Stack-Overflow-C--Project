using Microsoft.EntityFrameworkCore.Internal;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_xyy
{
    public partial class QuestionDetailsView : UserControlBase
    {
        private Post question;
        public Post Question
        {
            get => question;
            set => SetProperty<Post>(ref question, value);
        }

        private ObservableCollection<Post> answers;
        public ObservableCollection<Post> Answers { get => answers; set => SetProperty(ref answers, value); }

        private string body;
        public string Body
        {
            get => body;
            set => SetProperty<string>(ref body, value, () => ValidateBody());
        }

        public ICommand Answer { get; set; }
        public ICommand Up { get; set; }
        public ICommand Down { get; set; }
        public ICommand Zero { get; set; }

        public QuestionDetailsView(Post question)
        {
            InitializeComponent();

            DataContext = this;

            this.question = question;

            RefreshAnswers();

            Answer = new RelayCommand(AnswerAction,
                () => { return body != null && !HasErrors; });

            Up = new RelayCommand<Post>(param =>UpVote(param));
            Down = new RelayCommand<Post>(param =>DownVote(param));
            Zero = new RelayCommand<Post>(param =>ZeroVote(param));

        }

        private int VoteExist(Post post, int valeur)
        {
            User connected = App.CurrentUser;
            Vote v = App.Model.Votes.SingleOrDefault(vote => vote.PostId.PostId == post.PostId &&
                                                    vote.UserId.UserId == connected.UserId &&
                                                    vote.UpDown == valeur);
            if(v == null)
            {
                return 0;
            }
            else
            {
                return App.Model.Votes.IndexOf(v);
            }
                        
        }

        private void UpVote(Post param)
        {
            User connected = App.CurrentUser;
            int index = VoteExist(param, 1);
            if (index == 0)
            {
                App.Model.CreateVote(connected, param, 1);
                App.Model.SaveChanges();
            }
            else
            {
                Vote v = App.Model.Votes.Find(index);
                App.Model.Votes.Remove(v);
                App.Model.SaveChanges();
            }
        }
        private void DownVote(Post param)
        {
            User connected = App.CurrentUser;
            int index = VoteExist(param, -1);
            if (index == 0)
            {
                App.Model.CreateVote(connected, param, -1);
                App.Model.SaveChanges();
            }
            else
            {
                Vote v = App.Model.Votes.Find(index);
                App.Model.Votes.Remove(v);
                App.Model.SaveChanges();
            }
        }
        
        private void ZeroVote(Post param)
        {
            User connected = App.CurrentUser;
            Vote v = App.Model.Votes.SingleOrDefault(vote => vote.PostId.PostId == param.PostId &&
                                                    vote.UserId.UserId == connected.UserId);
            
            if (v != null)
            {
                App.Model.Votes.Remove(v);
                App.Model.SaveChanges();
            }
        }

        public void RefreshAnswers()
        {
            var q1 = from m in App.Model.Posts
                     where m.ParentId.PostId == Question.PostId 
                     orderby m.DateTime descending
                     select m;
            Answers = new ObservableCollection<Post>(q1);
        }

        private bool ValidateBody()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(Body))
            {
                AddError("Body", "Not empty");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Body))
                {
                    AddError("Body", "Not white space");
                }
            }
            return !HasErrors;
        }

        private void AnswerAction()
        {
            if (ValidateBody())
            {
                User connected = App.CurrentUser;
                DateTime now = new DateTime();
                now = DateTime.Now;
                var newanswer = App.Model.CreateAnswer(connected, Body, now, Question);
                App.Model.Posts.Add(newanswer);
                App.Model.SaveChanges();
                textAnswer.Clear();
                RefreshAnswers();
            }
        }


    }
}
