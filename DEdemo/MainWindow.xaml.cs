using DEdemo.Database;
using DEdemo.Helpers;
using DEdemo.Statics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DEdemo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ShoesDEEntitites _db = new ShoesDEEntitites();
        private MessageHelper _mh = new MessageHelper();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginEnter.Text;
            string password = PasswordEnter.Password;

            var user = _db.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault();

            if (login == "" || password == "")
            {
                _mh.ShowWarning("Поля ввода не должны быть пусты");
            }
            else if (user == null)
            {
                _mh.ShowError("Введён неверный логин или пароль!");
            }
            else
            {
                CurrentSession.CurrentUser = user;
                new ProductWindow().Show();
                Close();
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new ProductWindow().Show();
            Close();
        }
    }
}
