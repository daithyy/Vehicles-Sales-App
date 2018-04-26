using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CA1.Objects
{
    class Bike : Vehicle
    {
        public enum Type
        {
            Scooter,
            Trail,
            Bike,
            Sports,
            Commuter,
            Tourer
        }
        public Type type;

        public Bike(string make, string model, double price, int year, 
            Color color, double mileage, string description, string image, Type type) 
            : base(make, model, price, year, color, mileage, description, image)
        {
            this.type = type;
        }
    }
}
