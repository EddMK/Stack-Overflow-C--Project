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

        public QuestionDetailsView(Post question)
        {
            InitializeComponent();

            DataContext = this;

            this.question = question;

            RefreshAnswers();

            Answer = new RelayCommand(AnswerAction,
                () => { return body != null && !HasErrors; });

        }

        public void RefreshAnswers()
        {
            var q1 = from m in App.Model.Posts
                     where m.ParentId.PostId == Question.PostId 
                     orderby m.DateTime descending
                     select m;
            Console.WriteLine(q1);
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
