using ShoesDEMO.Database;
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

namespace ShoesDEMO
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductWindow.xaml
    /// </summary>
    public partial class AddEditProductWindow : Window
    {
        private ShoesDEEntitites _db = new ShoesDEEntitites();
        private bool _isEditing;
        private Product _product;
        public AddEditProductWindow(int? id)
        {
            InitializeComponent();

            if(id == null)
            {
                _isEditing = false;
                this.Title = "Создание карточки товара";
            }
            else
            {
                _isEditing= true;
                _product = _db.Product.Find(id);
                this.Title = "Редактирование карточки товара";
            }
            LoadData();
        }

        private void LoadData()
        {
            var units = _db.Unit.ToList();
            var categories = _db.Category.ToList();
            var producers = _db.ProducerId.ToList();
            var providers = _db.ProviderId.ToList();

            ProductUnit.ItemsSource = units;
            ProductUnit.DisplayMemberPath = "Name";
            ProductUnit.SelectedValuePath = "Id";
            ProductUnit.SelectedIndex = 0;

            ProductProducer.ItemsSource = producers;
            ProductProducer.DisplayMemberPath = "Name";
            ProductProducer.SelectedValuePath = "Id";
            ProductProducer.SelectedIndex = 0;

            ProductProvider.ItemsSource = providers;
            ProductProvider.DisplayMemberPath = "Name";
            ProductProvider.SelectedValuePath = "Id";
            ProductProvider.SelectedIndex = 0;

            ProductCategory.ItemsSource = categories;
            ProductCategory.DisplayMemberPath= "Name";
            ProductCategory.SelectedValuePath = "Id";
            ProductCategory.SelectedIndex = 0;

            if(_isEditing == true)
            {
                FillData();
            }
        }

        private void FillData()
        {
            ProductArticle.Text = _product.Article;
            ProductName.Text = _product.Name;
            ProductPrice.Text = _product.Price.ToString();
            ProductDiscount.Text = _product.Discount.ToString();
            ProductAmountInStock.Text = _product.AmountInStock.ToString();
            ProductDescription.Text = _product.Discription;
            ProductPhoto.Text = _product.Photo;

            ProductUnit.SelectedValue = _product.UnitId;
            ProductProducer.SelectedValue = _product.ProducerId;
            ProductProvider.SelectedValue = _product.ProducerId;
            ProductCategory.SelectedValue = _product.CategoryId;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            new ProductWindow().Show();
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
