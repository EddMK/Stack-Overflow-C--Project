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
        public ICommand Accept { get; set; }
        public ICommand CancelAccept { get; set; }
        public ICommand AddComment { get; set; }
        public ICommand DeleteComment { get; set; }
        public ICommand EditComment { get; set; }
        public ICommand EditCommentAnswer { get; set; }

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
            Accept = new RelayCommand<Post>(param =>AcceptAction(param));
            CancelAccept = new RelayCommand<Post>(param =>CancelAcceptAction(param));
            AddComment = new RelayCommand<Post>((p) => {
                App.NotifyColleagues(AppMessages.MSG_ADD_COMMENT, p);
            });
            EditComment = new RelayCommand<Comment>((c) => {
                App.NotifyColleagues(AppMessages.MSG_EDIT_COMMENT, c);
            });
            DeleteComment = new RelayCommand<Comment>(param => DeleteCommentAction(param));

        }

        private void DeleteCommentAction(Comment c)
        {

        }

        private void AcceptAction(Post param)
        {
            Question.AcceptedAnswerId = param;
            App.Model.SaveChanges();
            //Console.WriteLine(param.Body);
        }
        private void CancelAcceptAction(Post param)
        {
            
            
            Question.AcceptedAnswerId = null;
            App.Model.SaveChanges();
            //Console.WriteLine(param.Body);
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
            if (Question.AcceptedAnswerId == null)
            {
                var q1 = from m in App.Model.Posts
                         let scores = m.Votes.Sum( v => v.UpDown)
                         where m.Title == null && m.ParentId.PostId == Question.PostId
                         orderby scores descending, m.DateTime descending
                         select m;
                Answers = new ObservableCollection<Post>(q1);
            }
            else
            {
                Post v = App.Model.Posts.SingleOrDefault(post =>  post.PostId == Question.AcceptedAnswerId.PostId);
                Answers = new ObservableCollection<Post>();
                Answers.Add(v);
                var q1 = from m in App.Model.Posts
                         let scores = m.Votes.Sum( s  => s.UpDown)
                         where m.Title == null && m.PostId != Question.AcceptedAnswerId.PostId && m.ParentId.PostId == Question.PostId
                         orderby scores descending, m.DateTime descending
                         select m;
                var xlist = new List<Post>();
                xlist.Add(v);
                xlist.AddRange(q1.ToList());
                Answers = new ObservableCollection<Post>(xlist);
            }
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
