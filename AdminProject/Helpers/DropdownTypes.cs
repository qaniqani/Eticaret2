using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;

namespace AdminProject.Helpers
{
    public class DropdownTypes
    {
        public static SelectList GetStatus(StatusTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Active", Value = StatusTypes.Active.ToInt32().ToString()},
                new ListItem {Text = "Deactive", Value = StatusTypes.Deactive.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetCategoryType(CategoryTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Product", Value = CategoryTypes.Product.ToInt32().ToString()},
                new ListItem {Text = "Master", Value = CategoryTypes.Master.ToInt32().ToString()},
                new ListItem {Text = "Static", Value = CategoryTypes.Static.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetContentType(ContentTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Content", Value = ContentTypes.Content.ToInt32().ToString()},
                //new ListItem {Text = "Parent", Value = CategoryTypes.Parent.ToInt32().ToString()},
                new ListItem {Text = "Gallery Detail", Value = ContentTypes.GalleryDetail.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetVideoType(VideoTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Not Video", Value = VideoTypes.IsNotVideo.ToInt32().ToString()},
                new ListItem {Text = "Video", Value = VideoTypes.IsVideo.ToInt32().ToString()},
                new ListItem {Text = "Video Embed Code", Value = VideoTypes.IsEmbedCode.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetPictureType(PictureTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Slider", Value = PictureTypes.Slider.ToInt32().ToString()},
                //new ListItem {Text = "Showcase", Value = PictureTypes.Showcase.ToInt32().ToString()},
                //new ListItem {Text = "Content Slider", Value = PictureTypes.ContentSlider.ToInt32().ToString()},
                new ListItem {Text = "LightBox", Value = PictureTypes.LightBox.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetStockType(StockTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Stokta", Value = StockTypes.InStock.ToInt32().ToString()},
                new ListItem {Text = "Stokta Yok", Value = StockTypes.OutStock.ToInt32().ToString()},
                new ListItem {Text = "2-3 Gün İçerisinde", Value = StockTypes.Days23.ToInt32().ToString()},
                new ListItem {Text = "Tedarik Sürecinde", Value = StockTypes.ProcurementProcess.ToInt32().ToString()},
                new ListItem {Text = "Ön Sipariş", Value = StockTypes.PreOrder.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetStockSearchType(StockTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Tümü", Value = StockTypes.All.ToInt32().ToString()},
                new ListItem {Text = "Stokta", Value = StockTypes.InStock.ToInt32().ToString()},
                new ListItem {Text = "Stokta Yok", Value = StockTypes.OutStock.ToInt32().ToString()},
                new ListItem {Text = "2-3 Gün İçerisinde", Value = StockTypes.Days23.ToInt32().ToString()},
                new ListItem {Text = "Tedarik Sürecinde", Value = StockTypes.ProcurementProcess.ToInt32().ToString()},
                new ListItem {Text = "Ön Sipariş", Value = StockTypes.PreOrder.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetProductType(ProductTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Normal", Value = ProductTypes.Normal.ToInt32().ToString()},
                new ListItem {Text = "Yeni", Value = ProductTypes.New.ToInt32().ToString()},
                new ListItem {Text = "Özel Ürün", Value = ProductTypes.Special.ToInt32().ToString()},
                new ListItem {Text = "Çok Satılan", Value = ProductTypes.VerySold.ToInt32().ToString()},
                new ListItem {Text = "Tükeniyor", Value = ProductTypes.Ending.ToInt32().ToString()},
                new ListItem {Text = "İndirimde", Value = ProductTypes.Discount.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetProductSearchType(ProductTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = Utility.AllProductConvert[ProductTypes.All], Value = ProductTypes.All.ToString()},
                new ListItem {Text = Utility.AllProductConvert[ProductTypes.Normal], Value = ProductTypes.Normal.ToInt32().ToString()},
                new ListItem {Text = Utility.AllProductConvert[ProductTypes.New], Value = ProductTypes.New.ToInt32().ToString()},
                new ListItem {Text = Utility.AllProductConvert[ProductTypes.Special], Value = ProductTypes.Special.ToInt32().ToString()},
                new ListItem {Text = Utility.AllProductConvert[ProductTypes.VerySold], Value = ProductTypes.VerySold.ToInt32().ToString()},
                new ListItem {Text = Utility.AllProductConvert[ProductTypes.Ending], Value = ProductTypes.Ending.ToInt32().ToString()},
                new ListItem {Text = Utility.AllProductConvert[ProductTypes.Discount], Value = ProductTypes.Discount.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetCommentType(CommentTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Yeni", Value = CommentTypes.New.ToInt32().ToString()},
                new ListItem {Text = "Aktif", Value = CommentTypes.Active.ToInt32().ToString()},
                new ListItem {Text = "Pasif", Value = CommentTypes.Deactive.ToInt32().ToString()},
                new ListItem {Text = "Silinen", Value = CommentTypes.Deleted.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetUserType(UserTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Aktif", Value = UserTypes.Active.ToInt32().ToString()},
                new ListItem {Text = "Pasif", Value = UserTypes.Deactive.ToInt32().ToString()},
                new ListItem {Text = "Silinen", Value = UserTypes.Deleted.ToInt32().ToString()},
                new ListItem {Text = "Yasaklanan", Value = UserTypes.Banned.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetCampaignType(CampaignType selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Cargo", Value = CampaignType.Cargo.ToInt32().ToString()},
                new ListItem {Text = "GeneralDiscount", Value = CampaignType.GeneralDiscount.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetProductGroupType(ProductGroupTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Normal Ürün", Value = ProductGroupTypes.Normal.ToInt32().ToString()},
                new ListItem {Text = "Favoriler", Value = ProductGroupTypes.Favorites.ToInt32().ToString()},
                new ListItem {Text = "Beğenilenler", Value = ProductGroupTypes.Liked.ToInt32().ToString()},
                new ListItem {Text = "Yeniler", Value = ProductGroupTypes.News.ToInt32().ToString()},
                new ListItem {Text = "Seçtiklerimiz", Value = ProductGroupTypes.WeChose.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetGenderType(string selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Bay", Value = "Bay"},
                new ListItem {Text = "Bayan", Value = "Bayan"}
            };

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList GetUserStatus(StatusTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Aktif", Value = StatusTypes.Active.ToInt32().ToString()},
                new ListItem {Text = "Pasif", Value = StatusTypes.Deactive.ToInt32().ToString()},
                new ListItem {Text = "Dondur", Value = StatusTypes.Freeze.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList GetUserInvoiceTypes(InoviceTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem {Text = "Kurumsal", Value = InoviceTypes.Corporate.ToInt32().ToString()},
                new ListItem {Text = "Bireysel", Value = InoviceTypes.Personal.ToInt32().ToString()}
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToInt32().ToString());
        }

        public static SelectList TakeCount(int selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem { Text = "10", Value= "10" },
                new ListItem { Text = "20", Value= "20" },
                new ListItem { Text = "30", Value= "30" },
                new ListItem { Text = "50", Value= "50" },
                new ListItem { Text = "100", Value= "100" }
            };

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList ProductTakeCount(int selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem { Text = "4", Value= "4" },
                new ListItem { Text = "12", Value= "12" },
                new ListItem { Text = "16", Value= "16" },
                new ListItem { Text = "18", Value= "18" },
                new ListItem { Text = "24", Value= "24" },
                new ListItem { Text = "28", Value= "28" },
                new ListItem { Text = "32", Value= "32" }
            };

            return new SelectList(list, "Value", "Text", selectedValue);
        }

        public static SelectList ProductOrder(ProductOrderTypes selectedValue)
        {
            var list = new List<ListItem>
            {
                new ListItem { Text = "Normal", Value= ProductOrderTypes.Normal.ToString() },
                new ListItem { Text = "Fiyat: Azalan", Value= ProductOrderTypes.PriceAsc.ToString() },
                new ListItem { Text = "Fiyat: Artan", Value= ProductOrderTypes.PriceDesc.ToString() },
                new ListItem { Text = "Popülarite", Value= ProductOrderTypes.Popularity.ToString() }
            };

            return new SelectList(list, "Value", "Text", selectedValue.ToString());
        }
    }
}