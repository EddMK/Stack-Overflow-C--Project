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
using System.Windows.Input;
using System.Windows.Media;

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
        public ICommand Edit { get; set; }
        public ICommand Delete { get; set; }
        public ICommand Back { get; set; }

        public TagsView()
        {
            InitializeComponent();

            DataContext = this;

            

            Add = new RelayCommand(AddTag,
                () => { return newTag != null  && !HasErrors; });

            Edit = new RelayCommand(() => EditClick());
            
            Delete = new RelayCommand(() => DeleteClick());

            Back = new RelayCommand(() => {
                App.NotifyColleagues(AppMessages.MSG_DELETE_TAGVIEUW);
            });

            Refresh();
            if (App.CurrentUser.Role == Role.Member)
            {
                            //YourDataGrid.Columns[IndexOftheColumn].Visibility = Visibility.Collapsed;
                            datagridtag.Columns[0].Visibility = Visibility.Collapsed;
            }

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

        private void EditClick()
        {
            //Console.WriteLine("Here we are !");
            var currentRowIndex = datagridtag.Items.IndexOf(datagridtag.CurrentItem);
            TextBlock id = datagridtag.Columns[0].GetCellContent(datagridtag.Items[currentRowIndex]) as TextBlock;
            TextBlock name = datagridtag.Columns[1].GetCellContent(datagridtag.Items[currentRowIndex]) as TextBlock;
            
            string editTag = name.Text;
            int tagid = Int32.Parse(id.Text);

            string error = "";
            if (string.IsNullOrEmpty(editTag))
            {
                error += "Not empty";
            }
            else
            {
                if (string.IsNullOrWhiteSpace(editTag))
                {
                    error += "Not white space";
                }
                else
                {
                    Tag tagCurrent = App.Model.Tags.SingleOrDefault(tag => tag.TagName == editTag);
                    if (tagCurrent != null)
                    {
                        error += "Already exists";

                    }
                }
            }

            if (error.Length == 0)
            {
                var tag = App.Model.Tags.Find(tagid);
                tag.TagName = editTag;
                App.Model.SaveChanges();
                Refresh();
            }
            else
            {
                MessageBox.Show(error);
                Refresh();
            }
            
        }

        private void DeleteClick()
        {
            var currentRowIndex = datagridtag.Items.IndexOf(datagridtag.CurrentItem);
            TextBlock id = datagridtag.Columns[0].GetCellContent(datagridtag.Items[currentRowIndex]) as TextBlock;
            int tagid = Int32.Parse(id.Text);

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete", "Delete", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    var tag = App.Model.Tags.Find(tagid);
                    App.Model.Tags.Remove(tag);
                    App.Model.SaveChanges();
                    Refresh();
                    break;
            }
        }
    }
}
