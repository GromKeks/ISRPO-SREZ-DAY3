using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lib
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

       
        //Скрытие/раскрытие пароля
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Passbox.Visibility == Visibility.Hidden)
            {
                Passbox.Password = Pas_Tb.Text;
                Passbox.Visibility = Visibility.Visible;
                Pas_Tb.Visibility = Visibility.Hidden;
            }
            else
            {
                Pas_Tb.Text = Passbox.Password ;
                Pas_Tb.Visibility = Visibility.Visible;
                Passbox.Visibility = Visibility.Hidden;

            }
        }

        
        //Авторизация
        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create("https://localhost:7256/Login");

                    var postData = "login=" + Uri.EscapeDataString(Tb_Login.Text);
                    postData += "&password=" + Uri.EscapeDataString(Sha(Pas_Tb.Text));

                    var data = Encoding.ASCII.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    var responsee = (HttpWebResponse)request.GetResponse();
                    var respp = new StreamReader(responsee.GetResponseStream()).ReadToEnd();
                }
                catch
                {

                    Pas_Tb.Text = Passbox.Password;
                    var request = (HttpWebRequest)WebRequest.Create("https://localhost:7256/Login");

                    var postData = "login=" + Uri.EscapeDataString(Tb_Login.Text);
                    postData += "&password=" + Uri.EscapeDataString(Sha(Pas_Tb.Text));

                    var data = Encoding.ASCII.GetBytes(postData);

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();
                    var resp = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
                string Sha(string inputString)
                {
                    var crypt = new SHA256Managed();
                    string hash = String.Empty;
                    byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(inputString));
                    foreach (byte theByte in crypto)
                    {
                        hash += theByte.ToString("x2");
                    }
                    return hash;
                }

                MessageBox.Show("Успешный вход");
                Menu work = new Menu();
                work.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
        
        
    }
}
