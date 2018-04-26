using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CA1.Objects
{
    class Car : Vehicle
    {
        public enum BodyType
        {
            Convertible,
            Hatchback,
            Coupe,
            Estate,
            MPV,
            SUV,
            Saloon,
            Unlisted
        }
        public BodyType bodyType;

        public Car(string make, string model, double price, int year, 
            Color color, double mileage, string description, string image, BodyType bodyType) 
            : base(make, model, price, year, color, mileage, description, image)
        {
            this.bodyType = bodyType;
        }
    }
}
