using PRBD_Framework;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace prbd_1920_xyy {
    public partial class MainView : WindowBase {

        public ICommand Logout { get; set; }
        public ICommand Tags { get; set; }
        public ICommand Ask { get; set; }

        public MainView() {
            InitializeComponent();
            
            DataContext = this;

            App.Register<Post>(this, AppMessages.MSG_ADD_COMMENT, m => {
                foreach (TabItem t in tabControl.Items)
                {
                    if (t.Header.ToString().Equals("Add Comment"))
                    {
                        Dispatcher.InvokeAsync(() => t.Focus());
                        return;
                    }
                }
                var tab = new TabItem()
                {
                    Header = "Add Comment",
                    Content = new AddCommentView(m)
                };
                tabControl.Items.Add(tab);
                Dispatcher.InvokeAsync(() => tab.Focus());
            });


            App.Register<Comment>(this, AppMessages.MSG_EDIT_COMMENT, m => {
                //Console.WriteLine(m);
                foreach (TabItem t in tabControl.Items)
                {
                    if (t.Header.ToString().Equals("Edit Comment"))
                    {
                        Dispatcher.InvokeAsync(() => t.Focus());
                        return;
                    }
                }
                var tab = new TabItem()
                {
                    Header = "Edit Comment",
                    Content = new EditCommentView(m)
                };
                tabControl.Items.Add(tab);
                Dispatcher.InvokeAsync(() => tab.Focus());
            });


            App.Register<Post>(this, AppMessages.MSG_REFRESH_QUESTION, m => {
                var tab = new TabItem()
                {
                    Header = "Question",
                    Content = new QuestionDetailsView(m)
                };
                tabControl.Items.Add(tab);
                Dispatcher.InvokeAsync(() => tab.Focus());
            });

            App.Register<Post>(this, AppMessages.MSG_DISPLAY_QUESTION, m => {
                TabOfMember(m);
            });

            App.Register<string>(this, AppMessages.MSG_DELETE_VIEUW, m => {
                int index = 0;
                foreach (TabItem tabitem in tabControl.Items)
                {
                    if (tabitem.Header.Equals(m))
                    {
                        index = tabControl.Items.IndexOf(tabitem);
                    }
                }
                tabControl.Items.RemoveAt(index);

                foreach (TabItem tabitem in tabControl.Items)
                {
                    if (tabitem.Header.Equals("Newest"))
                    {
                        Dispatcher.InvokeAsync(() => tabitem.Focus());
                    }
                }
            });

            Logout = new RelayCommand(LogoutAction);

            Tags = new RelayCommand(TagsAction);
            Ask = new RelayCommand(AskAction);
        }

        private void TabOfMember(Post question) {
            foreach (TabItem t in tabControl.Items) {
                if (t.Header.ToString().Equals("Question")) {
                    Dispatcher.InvokeAsync(() => t.Focus());
                    return;
                }
            }
            var tab = new TabItem() {
                Header = "Question",
                Content = new QuestionDetailsView(question)
            };
            tabControl.Items.Add(tab);
            tab.MouseDown += (o, e) => {
                if (e.ChangedButton == MouseButton.Middle &&
                    e.ButtonState == MouseButtonState.Pressed) {
                    tabControl.Items.Remove(o);
                    (tab.Content as UserControlBase).Dispose();
                }
            };
            tab.PreviewKeyDown += (o, e) => {
                if (e.Key == Key.W && Keyboard.IsKeyDown(Key.LeftCtrl)) {
                    tabControl.Items.Remove(o);
                    (tab.Content as UserControlBase).Dispose();
                }
            };
            // exécute la méthode Focus() de l'onglet pour lui donner le focus (càd l'activer)
            Dispatcher.InvokeAsync(() => tab.Focus());
        }

        private void Login() {
            var loginView = new LoginView();
            Visibility = Visibility.Hidden;
            var res = loginView.ShowDialog();
            if (res == true) {
                Visibility = Visibility.Visible;
            }
            else {
                Close();
            }
        }

        private void LogoutAction() {
            App.CurrentUser = null;
            for (int i = tabControl.Items.Count - 1; i > 0; i--) 
                tabControl.Items.RemoveAt(i);
            Login();
        }
        
        private void TagsAction() {
            Boolean exist = false;
            foreach (TabItem tabitem in tabControl.Items)
            {
                if (tabitem.Header.Equals("Tags"))
                {
                    exist = true;
                }
            }
            var tab = new TabItem()
            {
                Header = "Tags",
                Content = new TagsView()
            };
            if (!exist)
            {
                // ajoute ce onglet à la liste des onglets existant du TabControl
                tabControl.Items.Add(tab);
                // exécute la méthode Focus() de l'onglet pour lui donner le focus (càd l'activer)
                Dispatcher.InvokeAsync(() => tab.Focus());
            }
        }

        private void AskAction()
        {
            Boolean exist = false;
            foreach (TabItem tabitem in tabControl.Items)
            {
                if (tabitem.Header.Equals("Ask"))
                {
                    exist = true;
                }
            }
            var tab = new TabItem()
            {
                Header = "Ask",
                Content = new AskView()
            };
            if (!exist)
            {
                // ajoute ce onglet à la liste des onglets existant du TabControl
                tabControl.Items.Add(tab);
                // exécute la méthode Focus() de l'onglet pour lui donner le focus (càd l'activer)
                Dispatcher.InvokeAsync(() => tab.Focus());
            }
        }
    }
}
