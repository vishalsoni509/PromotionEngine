using NUnit.Framework;
using PromotionEngine;
using System.Collections.Generic;

namespace PromotionTest
{
    public class PromotionUnitTests
    {
        PromotionCalculate promotionCalculate =new PromotionCalculate();
      
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("A",2,100)]
        [TestCase("A",3,130)]
        [TestCase("B",2,45)]
        [TestCase("B",3,75)]
        [TestCase("C",3,60)]
        [TestCase("D",4,60)]
        public void Single_product_promotion(string product,int quantity,float expectedPrice)
        {
             Dictionary<string,int> salesInvoice=new Dictionary<string, int>();
            salesInvoice.Add(product,quantity);
            float totalPrice=promotionCalculate.calculatePromotion(salesInvoice);        
            Assert.AreEqual(totalPrice,expectedPrice);
        }

     [TestCase("A",3,150)]
     [TestCase("B",3,90)]
      public void Single_product_promotion_Negative(string product,int quantity,float expectedPrice)
        {
             Dictionary<string,int> salesInvoice=new Dictionary<string, int>();
            salesInvoice.Add(product,quantity);
            float totalPrice=promotionCalculate.calculatePromotion(salesInvoice);        
            Assert.AreNotEqual(totalPrice,expectedPrice);
        }
       
         [Test,TestCaseSource("MultiProductCases")]
        public void Multiple_product_promotion(Dictionary<string,int> salesInvoice,float expectedPrice)
        { 
            float totalPrice=promotionCalculate.calculatePromotion(salesInvoice);        
            Assert.AreEqual(totalPrice,expectedPrice);
        }
     [Test,TestCaseSource("MultiProductNegative")]
        public void Multiple_product_promotion_Negative(Dictionary<string,int> salesInvoice,float expectedPrice)
        { 
            float totalPrice=promotionCalculate.calculatePromotion(salesInvoice);        
            Assert.AreNotEqual(totalPrice,expectedPrice);
        }
     static object[] MultiProductCases=
     {
         new object[]{new Dictionary<string, int>(){{"A",1},{"B",1},{"C",1}},100 },
         new object[]{new Dictionary<string, int>(){{"A",1},{"B",1},{"D",1}},95 },
         new object[]{new Dictionary<string, int>(){{"A",1},{"B",1},{"C",1},{"D",1}},110 },
         new object[]{new Dictionary<string, int>(){{"A",1},{"B",1},{"C",4},{"D",5}},215 },
         new object[]{new Dictionary<string, int>(){{"A",1},{"B",1},{"C",3},{"D",2}},160 },
         new object[]{new Dictionary<string, int>(){{"A",5},{"B",5},{"C",1}},370 },
         new object[]{new Dictionary<string, int>(){{"A",3},{"B",5},{"C",1},{"D",1}},280 },
     };  
            
     static object[] MultiProductNegative=
     {
         new object[]{new Dictionary<string, int>(){{"A",1},{"B",1},{"C",1},{"D",1}},115 },       
         new object[]{new Dictionary<string, int>(){{"A",1},{"B",2},{"C",1},{"D",1}},135 },
         new object[]{new Dictionary<string, int>(){{"A",3},{"B",1},{"C",1},{"D",1}},215 },
         new object[]{new Dictionary<string, int>(){{"A",3},{"B",2}},210 },
     };  

  }
}
