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

        public class Taggue
        {
            public int TagId { get; set; }
            public string TagName { get; set; }
            public bool Coche  { get; set; }

            public Taggue(int id, string name, bool peut)
            {
                this.TagId = id;
                this.TagName = name;
                this.Coche = peut;
            }
        }

        private ObservableCollection<Taggue> tags;
        public ObservableCollection<Taggue> Tags { get => tags; set => SetProperty(ref tags, value); }

        private Post question;
        public Post Question
        {
            get => question;
            set => SetProperty<Post>(ref question, value);
        }

        private string body;
        public string Body
        {
            get => body;
            set => SetProperty<string>(ref body, value, () => ValidateBody());
        }

        private string titre;
        public string Titre
        {
            get => titre;
            set => SetProperty<string>(ref titre, value, () => ValidateBody());
        }

        private List<Taggue> tagChecked = new List<Taggue>();

        public List<Taggue> TagChecked { get => tagChecked; set => SetProperty(ref tagChecked, value); }

        public ICommand EditQuestion { get; set; }
        public ICommand BoxChecked { get; set; }
        public ICommand BoxUnchecked { get; set; }

        public EditQuestionView(Post question)
        {
            InitializeComponent();

            DataContext = this;

            this.Titre = question.Title;

            this.Body = question.Body;

            this.Question = question;

            Refresh();
            Console.WriteLine("les checkeds : " + TagChecked.Count);
            BoxChecked = new RelayCommand<Object>(p => CheckedClick(p));
            BoxUnchecked = new RelayCommand<Object>(p => UncheckedClick(p));
            EditQuestion = new RelayCommand(EditQuestionAction,
               () => { return titre!=null && body != null && !HasErrors; });
        }

        private void CheckedClick(object parameter)
        {
            Taggue x = parameter as Taggue;
            CheckBoxList(x, 1);
        }

        private void UncheckedClick(object parameter)
        {
            Taggue x = parameter as Taggue;
            CheckBoxList(x, 0);
        }

        private void CheckBoxList(Taggue t, int x)
        {
            //Console.WriteLine(Tags.Select(tir => tir.Coche == true).Count());
            //Console.WriteLine(t.Coche);
            if (x == 1)
            {
                //t.Coche = true;
                TagChecked.Add(t);
            }
            else
            {
                Taggue solu = TagChecked.FirstOrDefault(sonia => sonia.TagName == t.TagName);
                TagChecked.Remove(solu);
                //Console.WriteLine("Coche : "+ t.Coche);
                //int indice = jours.IndexOf("Mercredi");
                //t.Coche = false;

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

            var data = new ObservableCollection<Taggue>();
            foreach (var item in q1)
            {
                Taggue t = null;
                if (Question.Tags.Contains(item))
                {
                     t = new Taggue(item.TagId,item.TagName,true);
                }
                else
                {
                     t = new Taggue(item.TagId, item.TagName, false);
                }
                data.Add(t);
            }



            foreach (var r in Question.Tags.ToList()) 
            {
                TagChecked.Add(new Taggue(r.TagId, r.TagName, true));
            }
            Tags = data;

        }

        private void EditQuestionAction()
        {
            if (ValidateBody() && ValidateTitle() && ValidateTags())
            {
                foreach (var tag in Question.Tags)
                {
                    tag.Posts.Remove(Question);
                }
                Question.Tags.Clear();
                Question.Title = Titre;
                Question.Body = Body;
                foreach(Taggue x in TagChecked)
                {
                    var y = App.Model.CreateTag(x.TagName);
                    Question.Tags.Add(y);
                    App.Model.SaveChanges();
                }
                App.NotifyColleagues(AppMessages.MSG_DELETE_VIEUW, "Edit Question");
                App.NotifyColleagues(AppMessages.MSG_DELETE_VIEUW, "Question");
                App.NotifyColleagues(AppMessages.MSG_REFRESH_QUESTION, Question);
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

        private bool ValidateTitle()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(Titre))
            {
                AddError("Titre", "Not empty");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Titre))
                {
                    AddError("Titre", "Not white space");
                }
            }
            return !HasErrors;
        }

    }
}
