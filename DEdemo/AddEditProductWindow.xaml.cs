using DEdemo.Database;
using DEdemo.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

namespace DEdemo
{
    /// <summary>
    /// Логика взаимодействия для AddEditProductWindow.xaml
    /// </summary>
    public partial class AddEditProductWindow : Window
    {
        private ShoesDEEntitites _db = new ShoesDEEntitites();
        private MessageHelper _mh = new MessageHelper();
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
            if (!ValidateInput())
                return;

            if (_isEditing == true)
                UpdateProduct();
            else
                CreateProduct();
        }

        private void CreateProduct()
        {
            Product product = new Product();

            string article = ProductArticle.Text;
            string name = ProductName.Text;
            decimal price = Convert.ToDecimal(ProductPrice.Text);
            int discount = Convert.ToInt32(ProductDiscount.Text);
            int amount = Convert.ToInt32(ProductAmountInStock.Text);
            string description = ProductDescription.Text;
            string photo = ProductPhoto.Text;

            product.Article = article;
            product.Name = name;
            product.Price = price;
            product.Discount = discount;
            product.AmountInStock = amount;
            product.Discription = description;

            product.UnitId = (int)ProductUnit.SelectedValue;
            product.ProducerId = (int)ProductProducer.SelectedValue;
            product.ProviderId = (int)ProductProvider.SelectedValue;
            product.CategoryId = (int)ProductCategory.SelectedValue;

            try
            {
                _db.Product.AddOrUpdate(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _mh.ShowError(ex.Message);
            }
        }

        private void UpdateProduct()
        {
            string article = ProductArticle.Text;
            string name = ProductName.Text;
            decimal price = Convert.ToDecimal(ProductPrice.Text);
            int discount = Convert.ToInt32(ProductDiscount.Text);
            int amount = Convert.ToInt32(ProductAmountInStock.Text);
            string description = ProductDescription.Text;
            string photo = ProductPhoto.Text;

            _product.Article = article;
            _product.Name = name;
            _product.Price = price;
            _product.Discount = discount;
            _product.AmountInStock = amount;
            _product.Discription = description;

            _product.UnitId =(int)ProductUnit.SelectedValue;
            _product.ProducerId = (int)ProductProducer.SelectedValue;
            _product.ProviderId = (int)ProductProvider.SelectedValue;
            _product.CategoryId = (int)ProductCategory.SelectedValue;

            try
            {
                _db.Product.AddOrUpdate(_product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _mh.ShowError(ex.Message);
            }
        }

        private bool ValidateInput()
        {
            StringBuilder errors = new StringBuilder();

            string article = ProductArticle.Text;
            string name = ProductName.Text;
            string price = ProductPrice.Text;
            string discount = ProductDiscount.Text;
            string amount = ProductAmountInStock.Text;
            string description = ProductDescription.Text;
            string photo = ProductPhoto.Text;

            if (string.IsNullOrWhiteSpace(article))
                errors.AppendLine("Поле артикул не заполнено");

            if (string.IsNullOrWhiteSpace(name))
                errors.AppendLine("Поле наименование не заполнено");

            if (string.IsNullOrWhiteSpace(price) || !decimal.TryParse(price, out decimal priceDec) || priceDec < 0)
                errors.AppendLine("Поле цена не заполнено");

            if (string.IsNullOrWhiteSpace(discount) || !int.TryParse(discount, out int discountInt) || discountInt > 100 || discountInt < 0)
                errors.AppendLine("Поле скидка не заполнено");

            if (string.IsNullOrWhiteSpace(amount) || !int.TryParse(amount, out int amountInt) || amountInt < 0)
                errors.AppendLine("Поле количество не заполнено");

            if (string.IsNullOrWhiteSpace(description))
                errors.AppendLine("Поле описание не заполнено");

            if (errors.Length > 0)
            {
                _mh.ShowError(errors.ToString());
                return false;
            }
            return true;      
        }
    }
}
