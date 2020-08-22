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


        public QuestionDetailsView(Post question)
        {
            InitializeComponent();

            DataContext = this;

        }

        
    }
}
