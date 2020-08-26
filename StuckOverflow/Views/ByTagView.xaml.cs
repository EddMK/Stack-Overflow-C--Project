using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_xyy
{
    public partial class ByTagView : UserControlBase
    {

        private Tag tag;
        public Tag TheTag
        {
            get => tag;
            set => SetProperty<Tag>(ref tag, value);
        }

        private ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts { get => posts; set => SetProperty(ref posts, value); }

        public ICommand DisplayQuestionDetails { get; set; }
        public ICommand DisplayByTag { get; set; }

        public ByTagView(Tag t)
        {
            InitializeComponent();

            DataContext = this;

            TheTag = t;

            DisplayQuestionDetails = new RelayCommand<Post>(m => {
                App.NotifyColleagues(AppMessages.MSG_DISPLAY_QUESTION, m);
            });

            DisplayByTag = new RelayCommand<Tag>(m => {
                App.NotifyColleagues(AppMessages.MSG_DISPLAY_BYTAG, m);
            });

            Refresh();
        }

        private void Refresh()
        {
            Posts = new ObservableCollection<Post>(TheTag.Posts);
        }
    }
}
