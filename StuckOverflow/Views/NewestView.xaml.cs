using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_xyy {
    public partial class NewestView : UserControlBase {

        private ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts { get => posts; set => SetProperty(ref posts, value); }

        public ICommand DisplayQuestionDetails { get; set; }

        public NewestView() {
            InitializeComponent();

            DataContext = this;

            DisplayQuestionDetails = new RelayCommand<Post>(m => {
                App.NotifyColleagues(AppMessages.MSG_DISPLAY_QUESTION, m);
            });
            
            Refresh();
        }

        private void Refresh() {
            var q1 = from m in App.Model.Posts
                     where m.Title != null
                     orderby m.DateTime descending
                     select m;
            Posts = new ObservableCollection<Post>(q1);
        }
    }
}
