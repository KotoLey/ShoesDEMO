using ShoesDEMO.Database;
using System.Collections.Generic;
using System.Windows.Media;

namespace ShoesDEMO.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(Product product)
        {
            Id = product.Id;
            Article = product.Article;
            Name = product.Name;
            UnitId = product.UnitId;
            Price = product.Price;
            Discount = product.Discount;
            AmountInStock = product.AmountInStock;
            Discription = product.Discription;
            Photo = product.Photo;
            Category = product.Category;
            ProducerId1 = product.ProducerId1;
            ProviderId1 = product.ProviderId1;
            Unit = product.Unit;

            GetBackground();
            GetPhoto();
            GetPrice();
        }
        public int Id { get; set; }
        public string Article { get; set; }
        public string Name { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int Discount { get; set; }
        public int AmountInStock { get; set; }
        public string Discription { get; set; }
        public string Photo { get; set; }

        public Category Category { get; set; }
        public ProducerId ProducerId1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProducInOrder> ProducInOrder { get; set; }
        public ProviderId ProviderId1 { get; set; }
        public Unit Unit { get; set; }
        public Brush Background { get; set; }

        private void GetBackground()
        {
            if (Discount >= 15)
            {
                Background = (Brush)new BrushConverter().ConvertFromString("#2e8b57");
                return;
            }
            else if (AmountInStock == 0)
            {
                Background = Brushes.LightBlue;
                return;
            }
            else
            {
                Background = (Brush)new BrushConverter().ConvertFromString("#7fff00");
                return;
            }
        }

        private void GetPhoto()
        {
            if (!string.IsNullOrEmpty(Photo) || Photo != "")
                return;
            Photo = "/Res/picture.png";
        }

        private void GetPrice()
        {
            if (Discount == 0)
                return;
            OldPrice = Price;
            Price = OldPrice * (1 - (decimal)Discount / 100);
        }
    }
}
