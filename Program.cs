using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductsList productsList = new ProductsList();
            PromotionCalculate promotionCalculate = new PromotionCalculate();

            promotionCalculate.calculatePromotion(productsList.SKUID);
            Console.WriteLine("Promotions Engine !");
            Console.ReadLine();
        }
    }

    public class PromotionCalculate
    {
        // Calculates the  total price of the sales invoice after applying the promotions.
        public float calculatePromotion(Dictionary<string, int> SalesInvoice)
        {
            ProductsList products = new ProductsList();
            Promotions promotions = new Promotions();
            float totalPrice = 0;
            float promotionsOffers = 0;
            // Apply each promotion on the sales invoice and get the aggreageted offervalue.     
            foreach (var promotion in promotions.list_of_promotions)
            {
                var offer = applyPromotion(promotion, SalesInvoice);
                promotionsOffers = promotionsOffers + offer;
            }

            // Calculate the total price for non promotion items, ie left over items after aplying promotion
            var saleList = new List<string>(SalesInvoice.Keys);
            foreach (var sale in saleList)
            {
                var price = products.SKUID[sale] * SalesInvoice[sale];
                totalPrice = totalPrice + price;
                Console.WriteLine("Offer:{0} => {1}", price, totalPrice);
            }
            Console.WriteLine("Offer:{0}", totalPrice);
            return promotionsOffers + totalPrice;
        }

        // Apply the promotions on the the sales invoice.
        float applyPromotion(PromotionOffer promotions, Dictionary<string, int> SalesInvoice)
        {

            if (promotions.productOffer.Count == 1)     // Single promotion
            {
                var key = promotions.productOffer.ElementAt(0).Key;
                if (SalesInvoice.ContainsKey(key))
                {
                    int quantityInvoice = SalesInvoice[key];
                    int offerQuantity = promotions.productOffer[key];
                    int offerscount = quantityInvoice / offerQuantity;
                    int remainingQuantity = quantityInvoice % offerQuantity;
                    SalesInvoice[key] = remainingQuantity;
                    return offerscount * promotions.OfferPrice;
                }
            }
            else
            {  // Combi product promotion, considered for 2 products combination with AND operation
                var key1 = promotions.productOffer.ElementAt(0).Key;
                var key2 = promotions.productOffer.ElementAt(1).Key; ;
                if (SalesInvoice.ContainsKey(key1) && SalesInvoice.ContainsKey(key2))
                {
                    int quantityInvoice1 = SalesInvoice[key1];
                    int offerQuantity1 = promotions.productOffer[key1];
                    int offerscount1 = quantityInvoice1 / offerQuantity1;
                    int remainingQuantity1 = quantityInvoice1 % offerQuantity1;

                    int quantityInvoice2 = SalesInvoice[key2];
                    int offerQuantity2 = promotions.productOffer[key2];
                    int offerscount2 = quantityInvoice2 / offerQuantity2;
                    int remainingQuantity2 = quantityInvoice2 % offerQuantity2;

                    if (offerscount1 <= offerscount2) // consider combi offer with least offer counts.
                    {
                        SalesInvoice[key1] = remainingQuantity1;
                        SalesInvoice[key2] = remainingQuantity2 + (offerQuantity2 * (offerscount2 - offerscount1));
                        return offerscount1 * promotions.OfferPrice;
                    }
                    else
                    {
                        SalesInvoice[key1] = remainingQuantity1 + (offerQuantity1 * (offerscount1 - offerscount2));
                        SalesInvoice[key2] = remainingQuantity2;
                        return offerscount2 * promotions.OfferPrice;
                    }
                }
            }
            return 0;
        }


    }



    // The SKUId's list with it's unit price.
    public class ProductsList
    {
        public Dictionary<string, int> SKUID = new Dictionary<string, int>();
        public ProductsList()
        {
            SKUID.Add("A", 50);
            SKUID.Add("B", 30);
            SKUID.Add("C", 20);
            SKUID.Add("D", 15);
        }
    }

    // The Configured promotions rules.
    public class Promotions
    {
        public List<PromotionOffer> list_of_promotions = new List<PromotionOffer>();
        public Promotions()
        {
            PromotionOffer A = new PromotionOffer();
            A.productOffer.Add("A", 3);
            A.OfferPrice = 130;
            list_of_promotions.Add(A);

            PromotionOffer B = new PromotionOffer();
            B.productOffer.Add("B", 2);
            B.OfferPrice = 45;
            list_of_promotions.Add(B);

            PromotionOffer CD = new PromotionOffer();
            CD.productOffer.Add("C", 1);
            CD.productOffer.Add("D", 1);
            CD.OfferPrice = 30;
            list_of_promotions.Add(CD);
        }
    }

    // Promotion rule object with the offer price.
    public class PromotionOffer
    {
        public Dictionary<string, int> productOffer = new Dictionary<string, int>();
        public string inclusionType = "AND";  // Not user for now.
        public float OfferPrice;
    }
}
