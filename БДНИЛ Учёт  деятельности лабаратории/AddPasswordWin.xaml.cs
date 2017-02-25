using System;
using System.Collections.Generic;
using System.Linq;
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

namespace БДНИЛ_Учёт__деятельности_лабаратории
{
    /// <summary>
    /// Логика взаимодействия для AddPasswordWin.xaml
    /// </summary>
    public partial class AddPasswordWin : Window
    {
        private string loginText;
        public string LoginText
        {
            get
            {
                return loginText;
            }

            set
            {
                loginText = value;
            }
        }

        public AddPasswordWin()
        {
            InitializeComponent();
        }

        private void NextFormBt_Click(object sender, RoutedEventArgs e)
        {
            using (Entities db = new Entities()) {
                AuthorizationWindow AW = new AuthorizationWindow();
                if (PasswordTb.Password == AcceptPasswordTb.Password && PasswordTb.Password != "" && AcceptPasswordTb.Password != "")
                {
                    var query = db.Database.ExecuteSqlCommand("UPDATE Users SET Password=" + "'" + PasswordTb.Password + "'"+  " WHERE Login='" + loginText + "'");
                    AW.Show();
                    this.Close();
                }
            }
        }
    }
}
