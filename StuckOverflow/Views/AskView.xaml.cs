using PRBD_Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace prbd_1920_xyy
{
    public partial class AskView : UserControlBase
    {
        private ObservableCollection<Tag> tags;
        public ObservableCollection<Tag> Tags { get => tags; set => SetProperty(ref tags, value); }

        private string titre;
        public string Title
        {
            get => titre;
            set => SetProperty<string>(ref titre, value, () => ValidateTitle());
        }

        private string body;
        public string Body
        {
            get => body;
            set => SetProperty<string>(ref body, value, () => ValidateBody());
        }

        private List<Tag> tagChecked = new List<Tag>();

        public List<Tag> TagChecked { get => tagChecked; set => SetProperty(ref tagChecked, value); }


        public ICommand BoxChecked { get; set; }
        public ICommand BoxUnchecked { get; set; }
        public ICommand  Cancel { get; set; }
        public ICommand Save { get; set; }

        public AskView()
        {
            InitializeComponent();

            DataContext = this;

            Refresh();

            BoxChecked = new RelayCommand<Object>(p => CheckedClick(p));
            BoxUnchecked = new RelayCommand<Object>(p => UncheckedClick(p));

            Cancel = new RelayCommand(() => {
                App.NotifyColleagues(AppMessages.MSG_DELETE_VIEUW, "Ask");
            });

            Save = new RelayCommand(SaveAction,
                () => { return titre != null && body != null && !HasErrors; });
        }

        private bool ValidateTitle()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(Title))
            {
                AddError("Title", "Not empty");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Title))
                {
                    AddError("Title", "Not white space");
                }
            }
            return !HasErrors;
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

        private void SaveAction()
        {
            if( ValidateBody() && ValidateTitle() && ValidateTags())
            {
                User connected = App.CurrentUser;
                DateTime now = new DateTime();
                now = DateTime.Now;
                var newquestion = App.Model.CreateQuestion(connected,Title,Body,now,null);
                App.Model.Posts.Add(newquestion);
                App.Model.SaveChanges();
                foreach(Tag t in TagChecked)
                {
                    newquestion.Tags.Add(t);
                }
                App.Model.SaveChanges();
                App.NotifyColleagues(AppMessages.MSG_DELETE_VIEUW, "Ask");
                App.NotifyColleagues(AppMessages.MSG_DISPLAY_QUESTION, newquestion);
                


            }
        }

        private void CheckedClick(object parameter)
        {
            Tag x = parameter as Tag;
            CheckBoxList(x,1);
        }
        
        private void UncheckedClick(object parameter)
        {
            Tag x = parameter as Tag;
            CheckBoxList(x,0);
        }

        private void CheckBoxList(Tag t, int x) 
        {
            if (x == 1)
            {
                TagChecked.Add(t);
            }
            else
            {
                //int indice = jours.IndexOf("Mercredi");
                int indice = TagChecked.IndexOf(t);
                TagChecked.RemoveAt(indice);
            }


            ValidateTags();

        }

        private bool ValidateTags()
        {
            ClearErrors();
            if (TagChecked.Count > 3)
            {
                Console.WriteLine("No more than 3");
                AddError("Tags", "No more than 3");
            }
            return !HasErrors;
        }

        private void Refresh()
        {

            var q1 = from m in App.Model.Tags
                     select m;

            var data = new ObservableCollection<Tag>();
            foreach (var item in q1)
                data.Add(item);
            Tags = data;
        }

        
    }

}
