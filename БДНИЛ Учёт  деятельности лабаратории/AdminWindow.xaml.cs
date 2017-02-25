using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
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
using БДНИЛ_Учёт__деятельности_лабаратории.Classes;

namespace БДНИЛ_Учёт__деятельности_лабаратории
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        Query classQuery = new Query();

        public AdminWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e) { Environment.Exit(0); }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            using (Entities db = new Entities())
            {
                switch (comboBox.SelectedIndex)
                {
                    case 0:
                        classQuery.QueryAddTableEmployee(tableDbDg);
                        break;
                    case 1:
                        classQuery.QueryAddTablePlants(tableDbDg);
                        break;
                    case 2:
                        classQuery.QueryAddTableProvider(tableDbDg);
                        break;
                    case 3:
                        classQuery.QueryAddTableUser(tableDbDg);
                        break;
                }
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        { }

        private void button1_Click(object sender, RoutedEventArgs e)
        { }

        private void AddUserBt_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LoginTb_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void NameTb_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void SNameTb_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void PatronymicTb_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void addUserDg_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void addUserDg_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
