using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prbd_1920_xyy
{
    /// <summary>
    /// Logique d'interaction pour SignUpView.xaml
    /// </summary>
    public partial class SignUpView : WindowBase
    {
        private string pseudo;
        public string Pseudo
        {
            get => pseudo;
            set => SetProperty<string>(ref pseudo, value, () => ValidatePseudo());
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty<string>(ref password, value, () => ValidatePassword());
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty<string>(ref confirmPassword, value, () => ValidateConfirmPassword());
        }

        private bool ValidatePseudo()
        {
            ClearErrors();
            var member = App.Model.Users.Find(Pseudo);
            if (string.IsNullOrEmpty(Pseudo))
            {
                AddError("Pseudo", Properties.Resources.Error_Required);
            }
            else
            {
                if (Pseudo.Length < 3)
                {
                    AddError("Pseudo", Properties.Resources.Error_LengthGreaterEqual3);
                }
                else
                {
                    if (member != null)
                    {
                        AddError("Pseudo", "Member already exists with the same pseudo");
                    }
                }
            }
            return !HasErrors;
        }

        private bool ValidatePassword()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(Password))
            {
                AddError("Password", Properties.Resources.Error_Required);
            }
            else
            {
                if (Password.Length < 8)
                {
                    AddError("Password", Properties.Resources.Error_LengthGreaterEqual3);
                }
                else
                {
                    if (!Password.Any(char.IsUpper))
                    {
                        AddError("Password", "UpperCase");
                    }
                }
            }

            return !HasErrors;
        }

        private bool ValidateConfirmPassword()
        {
            ClearErrors();
            if (string.IsNullOrEmpty(Password))
            {
                AddError("Password", "Password required");
            }
            else
            {
                if (string.IsNullOrEmpty(ConfirmPassword))
                {
                    AddError("ConfirmPassword", "ConfirmPassword required");
                }
                else
                {
                    if (!Password.Equals(ConfirmPassword))
                    {
                        AddError("ConfirmPassword", "Not same");
                    }
                }
            }

            return !HasErrors;
        }

        public ICommand Cancel { get; set; }
        public ICommand SignUp { get; set; }

        private void SignupAction()
        {
            if (!HasErrors)
            {
                var newmember = App.Model.CreateUser(Pseudo, Password,Pseudo,Pseudo);
                App.Model.Users.Add(newmember);
                App.Model.SaveChanges();
                Console.Write(App.Model.Users);
                App.CurrentUser = newmember; // le membre connecté devient le membre courant
                ShowMainView(); // ouverture de la fenêtre principale
                Close(); // fermeture de la fenêtre de login
            }
            

        }

        private static void ShowMainView()
        {
            var mainView = new MainView();
            mainView.Show();
            Application.Current.MainWindow = mainView;
        }

        private void CloseAction()
        {
            var loginView = new LoginView();
            loginView.Show();
            window.Close();
            Application.Current.MainWindow = loginView;
        }

        public SignUpView()
        {
            InitializeComponent();

            DataContext = this;

            SignUp = new RelayCommand(SignupAction,
                () => { return pseudo != null && password != null && confirmPassword != null && !HasErrors; });



            Cancel = new RelayCommand(() => CloseAction());


        }
    }
}
