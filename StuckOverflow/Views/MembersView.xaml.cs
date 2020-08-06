using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_xyy {
    public partial class MembersView : UserControlBase {

        private ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts { get => posts; set => SetProperty(ref posts, value); }

        //public ICommand DisplayMemberDetails { get; set; }

        public MembersView() {
            InitializeComponent();

            DataContext = this;
            /*
            DisplayMemberDetails = new RelayCommand<Member>(m => {
                App.NotifyColleagues(AppMessages.MSG_DISPLAY_MEMBER, m);
            });
            */
            Refresh();
        }

        private void Refresh() {
            // Members = new ObservableCollection<Member>(App.Model.Members.OrderBy(m => m.Pseudo));
            Posts = new ObservableCollection<Post>(App.Model.Posts);
        }
    }
}
