using System.Collections.Generic;
using System.Linq;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Models;

namespace AdminProject.Helpers
{
    public static class Function
    {
        //dip toplam
        public static TotalSum TotalAmountCalculate(List<ProductDto> list, Cargo cargo)
        {
            var totalSum = new TotalSum();

            list.ForEach(item =>
            {
                totalSum.KdvOdd = item.KdvOdd;
                totalSum.KdvTotalAmount += item.KdvAmount;
                totalSum.SubTotalAmount += item.SubTotalAmount;
            });
            if (totalSum.SubTotalAmount <= 0)
                return totalSum;

            totalSum.CargoAmount = cargo.Price;
            totalSum.TotalAmount = totalSum.KdvTotalAmount + totalSum.SubTotalAmount;
            totalSum.OrginalTotalAmount = totalSum.TotalAmount;
            return totalSum;
        }

        //urun fiyat ve indirim hesaplama
        public static List<ProductDto> ProductDiscountCalculate(List<ProductDto> list)
        {
            list.ForEach(item =>
            {
                if (item.DiscountOdd <= 0)
                    return;

                var discountOdd = item.DiscountOdd;
                var price = item.ProductPrice;
                var kdvOdd = item.KdvOdd;

                if (item.IsKdv)
                    price = KdvAmountProductDown(price, kdvOdd);

                var itemDiscountAmount = ProductDiscount(price, discountOdd);
                var totalDiscountAmount = SubTotalAmount(itemDiscountAmount, item.Unit);

                item.DiscountAmount = totalDiscountAmount;

                item.TotalAmount = item.TotalAmount - totalDiscountAmount;
            });
            return list;
        }

        public static decimal ProductDiscount(decimal price, decimal discountOdd)
        {
            return price/100*discountOdd;
        }

        //kdv haric olarak eklendiyse kullanilir
        public static decimal KdvAmount(decimal price, decimal kdvOdd, int unit)
        {
            return price*kdvOdd/100*unit;
        }
        public static decimal KdvAmount(decimal price, decimal kdvOdd)
        {
            return price * kdvOdd / 100;
        }

        //fiyata kdv dahilse kullanilir.
        public static decimal KdvAmountProductDown(decimal price, decimal kdvOdd)
        {
            return price / (1 + kdvOdd / 100);
        }

        public static decimal KdvAmountProductDown(decimal price, decimal kdvOdd, int unit)
        {
            return price / (1 + kdvOdd / 100) * unit;
        }

        public static decimal TotalAmount(decimal price, decimal kdvOdd, int unit)
        {
            return (price*kdvOdd/100 + price)*unit;
        }

        public static decimal SubTotalAmount(decimal price, int unit)
        {
            return price * unit;
        }

        public static decimal ProductDiscountAmount(decimal price, decimal discountOdd, int unit)
        {
            return price / 100 * discountOdd * unit;
        }
    }
}