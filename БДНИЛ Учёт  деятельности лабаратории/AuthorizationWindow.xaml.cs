using System.Windows;
using System.Windows.Input;
using System;

namespace БДНИЛ_Учёт__деятельности_лабаратории
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        AdminWindow adminWindow = new AdminWindow();
        AddPasswordWin addPasswordWin = new AddPasswordWin();

        private string TextLogin;
        

        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            using (Entities db = new Entities())
            {
                try
                {
                    TextLogin = loginTb.Text.Trim().ToLower();
                    string passwordText = passwordTb.Password;

                    var query = db.Database.SqlQuery<Users>("Select * From Users");
                    addPasswordWin.LoginText = TextLogin;
                    foreach (var item in query)
                    {
                        if (TextLogin == item.Login && item.Password == null && passwordText == "" && (item.UserRoleId == "1" || item.UserRoleId == "2"))
                        {

                            addPasswordWin.Show();
                            this.Close();
                        }
                        else
                        {
                            if (TextLogin == item.Login && passwordText == item.Password && item.UserRoleId == "1")
                            {
                                adminWindow.Show();
                                this.Close();
                            }

                            else if (TextLogin == item.Login && passwordText == item.Password && item.UserRoleId == "2")
                            {
                                MessageBox.Show("Пользователь");
                            }
                        }
                    }
                }
                catch { MessageBox.Show("Неудалось подключиться к базе данных"); }
            }
        }


        private void Window_Closed(object sender, EventArgs e) {

        }
        
        private void loginTb_GotFocus(object sender, RoutedEventArgs e)
        {
            //EnterLoginL.Visibility = Visibility.Collapsed;
        }

        private void loginTb_LostFocus(object sender, RoutedEventArgs e)
        {
            //if(loginTb.Text.Trim() == "")
            //    EnterLoginL.Visibility = Visibility.Visible;
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EnterButton_Click(sender, e);
            }
        }

        private void EnterPasswordL_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void EnterLoginL_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //loginTb_GotFocus(sender, e);
        }
    }
}