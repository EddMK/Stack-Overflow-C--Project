using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_xyy {
    public partial class MemberDetailView : UserControlBase {

        public User Member { get; set; }

        public MemberDetailView(User member) {
            InitializeComponent();
            DataContext = this;
            Member = member;
        }
    }
}
