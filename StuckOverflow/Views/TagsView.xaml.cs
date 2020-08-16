using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_xyy
{
    public partial class TagsView : UserControlBase
    {
        private ObservableCollection<Tag> tags;
        public ObservableCollection<Tag> Tags { get => tags; set => SetProperty(ref tags, value); }

        private string newTag;
        public string NewTag
        {
            get => newTag;
            set => SetProperty<string>(ref newTag, value, () => ValidateTag());
        }

        private bool ValidateTag()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(NewTag))
            {
                AddError("NewTag", "Not empty");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(NewTag))
                {
                    AddError("NewTag", "Not white space");
                }
                else
                {
                    Tag tagCurrent = App.Model.Tags.SingleOrDefault(tag => tag.TagName == NewTag);
                    if (tagCurrent != null)
                    {
                        AddError("NewTag", "Already exists");
                    }
                }
            }
            return !HasErrors;
        }

        public ICommand Add { get; set; }

        public TagsView()
        {
            InitializeComponent();

            DataContext = this;

            Refresh();

            Add = new RelayCommand(AddTag,
                () => { return newTag != null  && !HasErrors; });
        }

        public void AddTag()
        {
            if (ValidateTag())
            {
                var newtag = App.Model.CreateTag(NewTag);
                App.Model.Tags.Add(newtag);
                App.Model.SaveChanges();
                Console.Write(App.Model.Tags);
                this.txtNewTag.Clear();
                Refresh();
                ClearErrors();
                
                /*
                var newmember = App.Model.CreateUser(Pseudo, Password, Pseudo, Pseudo);
                App.Model.Users.Add(newmember);
                App.Model.SaveChanges(); 
                Console.Write(App.Model.Users);
                App.CurrentUser = newmember; // le membre connecté devient le membre courant
                ShowMainView(); // ouverture de la fenêtre principale
                Close(); // fermeture de la fenêtre de login
                */
            }
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
