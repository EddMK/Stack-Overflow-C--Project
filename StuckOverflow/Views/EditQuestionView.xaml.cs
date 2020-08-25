using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_xyy
{
    public partial class EditQuestionView : UserControlBase
    {

        private Comment comment;
        public Comment Comment
        {
            get => comment;
            set => SetProperty<Comment>(ref comment, value);
        }

        private string body;
        public string Body
        {
            get => body;
            set => SetProperty<string>(ref body, value, () => ValidateBody());
        }

        public ICommand EditComment { get; set; }


        public EditQuestionView(Post question)
        {
            InitializeComponent();

            DataContext = this;

            //this.Comment = comment;

            this.Body = comment.Body;

            EditComment = new RelayCommand(EditCommentAction,
               () => { return body != null && !HasErrors; });
        }

        private void EditCommentAction()
        {
            if (ValidateBody())
            {
                Console.WriteLine(Body);
                this.Comment.Body = this.Body;
                App.Model.SaveChanges();
                var post = App.Model.Posts.Find(Comment.UserId.UserId);
                if (post.Title == null)
                {
                    post = App.Model.Posts.SingleOrDefault(v => v.PostId == post.ParentId.PostId);
                }
                App.NotifyColleagues(AppMessages.MSG_DELETE_VIEUW, "Edit Comment");
                App.NotifyColleagues(AppMessages.MSG_DISPLAY_QUESTION, post);
                /*
                User connected = App.CurrentUser;
                DateTime now = new DateTime();
                now = DateTime.Now;
                var newcomment = App.Model.CreateComment(connected, Post, Body, now);
                App.Model.Comments.Add(newcomment);
                
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
                */
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
