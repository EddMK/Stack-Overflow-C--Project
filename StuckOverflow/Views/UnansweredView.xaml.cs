using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_xyy
{
    /// <summary>
    /// Logique d'interaction pour UnansweredView.xaml
    /// </summary>
    public partial class UnansweredView : UserControlBase
    {
        private ObservableCollection<Post> posts;
        public ObservableCollection<Post> Posts { get => posts; set => SetProperty(ref posts, value); }
        public UnansweredView()
        {
            InitializeComponent();

            DataContext = this;
            Refresh();
        }

        private void Refresh()
        {
            var q1 = from m in App.Model.Posts
                     where m.Title != null && m.AcceptedAnswerId == null
                     orderby m.DateTime descending
                     select m;
            Posts = new ObservableCollection<Post>(q1);
        }
    }
}
