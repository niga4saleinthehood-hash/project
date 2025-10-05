using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Brew_Coffe_Shop.domain
{
    public class Enum
    {
        
        public enum OrderStatus
        {
            Pending, Confirm, Paid 
        }
        public enum ProductCategory
        {
            Coffee, Tea, Smoothie, Pastry , Combo
        }
        public enum RoastLevel
        {
            Light, Medium, Dark
        }



        public enum Size
        {
            S, M, L
        }

        public enum TeaType
        {
            Green, Black, Herbal
        }
        public enum SurgarLevel
        {
            No = 0,
            Less = 30,
            Mid = 50,
            Abit = 70,
            Noraml = 100

        }

        public enum Topping
        {
            None = 0,
            Bubble = 1,
            Jelly = 2,
            Cream = 3
        }

        public enum IceLevel
        {
            Low, Midium, High
        }

        public enum PastryType
        {
            Cake, Cookie, Sandwich
        }
        public enum MemberShip
        {
            Basic, Premium
        }

        public enum VIPLevel
        {
            Gold , Plantium
        }
    }
}
