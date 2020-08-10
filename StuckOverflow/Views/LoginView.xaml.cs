using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prbd_1920_xyy {
    public partial class LoginView : WindowBase {
        private string pseudo;
        public string Pseudo {
            get => pseudo;
            set => SetProperty<string>(ref pseudo, value, () => Validate());
        }

        private string password;
        public string Password {
            get => password;
            set => SetProperty<string>(ref password, value, () => Validate());
        }

        public override bool Validate() {
            ClearErrors();
            User myUser = App.Model.Users.SingleOrDefault(user => user.UserName == Pseudo);

            if (!ValidateLogin(myUser) || !ValidatePwd(myUser))
                RaiseErrors();
            return !HasErrors;
        }

        private bool ValidateLogin(User member) {
            if (string.IsNullOrEmpty(Pseudo)) {
                AddError("Pseudo", Properties.Resources.Error_Required);
            }
            else {
                if (Pseudo.Length < 3) {
                    AddError("Pseudo", Properties.Resources.Error_LengthGreaterEqual3);
                }
                else {
                    if (member == null) {
                        AddError("Pseudo", Properties.Resources.Error_DoesNotExist);
                    }
                }
            }
            return !HasErrors;
        }

        private bool ValidatePwd(User member) {
            if (string.IsNullOrEmpty(Password)) {
                AddError("Password", Properties.Resources.Error_Required);
            }
            else if (!member.Password.Equals(Password)) {
                AddError("Password", Properties.Resources.Error_WrongPassword);
            }
            return !HasErrors;
        }
        private void LoginAction() {
            if (Validate()) { // si aucune erreurs
                User myUser = App.Model.Users.SingleOrDefault(user => user.UserName == Pseudo);
                App.CurrentUser = myUser; // le membre connecté devient le membre courant
                ShowMainView(); // ouverture de la fenêtre principale
                Close(); // fermeture de la fenêtre de login
            }
        }

        private void ShowSignUp()
        {
            var signupView = new SignUpView();
            signupView.Show();
            window.Close();
            Application.Current.MainWindow = signupView;
        }

        public ICommand Login { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand SignUp { get; set; }

        private static void ShowMainView() {
            var mainView = new MainView();
            mainView.Show();
            Application.Current.MainWindow = mainView;
        }

        public LoginView() {
            InitializeComponent();

            DataContext = this;
            
            Login = new RelayCommand(LoginAction,
                () => { return pseudo != null && password != null && !HasErrors; });
            
            Cancel = new RelayCommand(() => Close());

            SignUp = new RelayCommand(() => ShowSignUp());
        }
    }
}
