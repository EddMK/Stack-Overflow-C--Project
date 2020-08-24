using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_xyy
{
    public partial class EditCommentView : UserControlBase
    {

        private Post post;
        public Post Post
        {
            get => post;
            set => SetProperty<Post>(ref post, value);
        }

        private string body;
        public string Body
        {
            get => body;
            set => SetProperty<string>(ref body, value, () => ValidateBody());
        }

        public ICommand AddComment { get; set; }


        public EditCommentView(Comment c)
        {
            InitializeComponent();

            DataContext = this;

            //this.Post = p;

            Console.WriteLine(c.Body);

            AddComment = new RelayCommand(AddCommentAction,
               () => { return body != null && !HasErrors; });
        }

        private void AddCommentAction()
        {
            if (ValidateBody())
            {
                User connected = App.CurrentUser;
                DateTime now = new DateTime();
                now = DateTime.Now;
                var newcomment = App.Model.CreateComment(connected, Post, Body, now);
                App.Model.Comments.Add(newcomment);
                App.Model.SaveChanges();
                Post post;
                if (Post.Title == null)
                {
                    post = App.Model.Posts.SingleOrDefault(v => v.PostId == Post.ParentId.PostId);
                }
                else
                {
                    post = Post;
                }
                App.NotifyColleagues(AppMessages.MSG_DELETE_VIEUW, "Add Comment");
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
