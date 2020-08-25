using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_xyy
{
    public partial class EditAnswerView : UserControlBase
    {

        private string body;
        public string Body
        {
            get => body;
            set => SetProperty<string>(ref body, value, () => ValidateBody());
        }
        
        private Post answer;
        public Post Answer
        {
            get => answer;
            set => SetProperty<Post>(ref answer, value);
        }

        public ICommand EditAnswer { get; set; }


        public EditAnswerView(Post answer)
        {
            InitializeComponent();

            DataContext = this;

            this.Answer = answer;

            this.Body = answer.Body;

            EditAnswer = new RelayCommand(EditAnswerAction,
               () => { return body != null && !HasErrors; });
        }

        private void EditAnswerAction()
        {
            if (ValidateBody())
            {
                Console.WriteLine(Body);
                this.Answer.Body = this.Body;
                App.Model.SaveChanges();
                var post = App.Model.Posts.Find(Answer.ParentId.PostId);
                App.NotifyColleagues(AppMessages.MSG_DELETE_VIEUW, "Edit Answer");
                App.NotifyColleagues(AppMessages.MSG_DISPLAY_QUESTION, post);
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


    }
}
