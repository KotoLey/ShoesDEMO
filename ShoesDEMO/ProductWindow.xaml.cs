using ShoesDEMO.Database;
using ShoesDEMO.Helpers;
using ShoesDEMO.Statics;
using ShoesDEMO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ShoesDEMO
{
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private ShoesDEEntitites _db = new ShoesDEEntitites();
        private MessageHelper _mbh = new MessageHelper();
        private List<ProductViewModel> _productViewModels = new List<ProductViewModel>();

        private string[] sotringTypes = new string[]
        {
            "По умолчанию",
            "По убыванию",
            "По возрастанию"
        };
        private List<string> _filteringTypes = new List<string>()
        {
            "Все поставщики"
        };

        public ProductWindow()
        {
            InitializeComponent();
            LoadProducts();
            LoadData();
            LoadUI();
        }

        private void LoadUI()
        {
            Users user = CurrentSession.CurrentUser;
            if (user == null || user.RoleId == 3)
            {
                AdminPanel.Visibility = Visibility.Collapsed;
            }
            else if (user.RoleId == 1)
            {
                CreateButton.Visibility = Visibility.Visible;
            }
        }

        private void LoadData()
        {
            SortingComboBox.ItemsSource = sotringTypes;
            SortingComboBox.SelectedIndex = 0;

            var provider = _db.ProviderId.ToList();
            foreach (var p in provider)
                _filteringTypes.Add(p.Name);
            FilteringComboBox.ItemsSource = _filteringTypes;
            FilteringComboBox.SelectedIndex = 0;

            Users user = CurrentSession.CurrentUser;
            if (user != null)
                FullUserName.Text = $"{user.Surname} {user.Name} {user.Patronymic}";
            else FullUserName.Text = "Гость";
        }

        private void LoadProducts()
        {
            var products = _db.Product.ToList();
            foreach (var p in products)
            {
                _productViewModels.Add(new ProductViewModel(p));
            }
            ProductList.ItemsSource = _productViewModels;
        }

        private void UpdateProducts()
        {
            ProductList.ItemsSource = _productViewModels;
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentSession.CurrentUser = null;
            new MainWindow().Show();
            Close();
        }

        private void SearchingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchingText = SearchingTextBox.Text;
            _productViewModels = _db.Product
                .Where(p =>
                    p.Category.Name.ToLower().Contains(searchingText) ||
                    p.Name.ToLower().Contains(searchingText) ||
                    p.Discription.ToLower().Contains(searchingText) ||
                    p.ProviderId1.Name.ToLower().Contains(searchingText) ||
                    p.ProducerId1.Name.ToLower().Contains(searchingText) ||
                    p.Unit.Name.ToLower().Contains(searchingText)
                )
                .ToList()
                .Select(p => new ProductViewModel(p))
                .ToList();
            UpdateProducts();
        }

        private void SotringComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sotringType = SortingComboBox.SelectedIndex;
            if (sotringType == 0)
            {
                LoadProducts();
            }
            else if (sotringType == 1)
            {
                _productViewModels = _productViewModels
                    .OrderByDescending(p => p.AmountInStock)
                    .ToList();
                UpdateProducts();
            }
            else if (sotringType == 2)
            {
                _productViewModels = _productViewModels
                      .OrderBy(p => p.AmountInStock)
                      .ToList();
                UpdateProducts();
            }
        }

        private void FilteringComboBox_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            string filterText = FilteringComboBox.SelectedValue.ToString();
            if (filterText == "Все поставщики")
            {
                LoadProducts();
                return;
            }
            _productViewModels = _productViewModels
                .Where(p => p.ProviderId1.Name ==  filterText)
                .ToList();
            UpdateProducts();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            new AddEditProductWindow(null).Show();
            Close();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Users user = CurrentSession.CurrentUser;
            if (user.RoleId != 1)
                return;

            int id = (int)(sender as Border).Tag;

            new AddEditProductWindow(id).Show();
            Close();
        }
    }
}