using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CA1.Objects
{
    public class Vehicle
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public int Year { get; set; }
        public Color Colour { get; set; }

        private SolidColorBrush brushColor;

        public SolidColorBrush BrushColor
        {
            get { return brushColor; }
            set { brushColor = value; }
        }
        public double Mileage { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Vehicle(string make, string model, double price, int year, 
            Color color, double mileage, string description, string image)
        {
            Make = make;
            Model = model;
            Price = price;
            Year = year;
            Colour = color;
            BrushColor = new SolidColorBrush(Colour);
            Mileage = mileage;
            Description = description;
            Image = image;
        }

        // Alternate type checking method
        //public dynamic CheckType()
        //{
        //    if (this.GetType().Equals(typeof(Car)))
        //        return (Car)this;
        //    else if (this.GetType().Equals(typeof(Van)))
        //        return (Van)this;
        //    else if (this.GetType().Equals(typeof(Bike)))
        //        return (Bike)this;
        //    else // Is default type Vehicle
        //    {
        //        return this;
        //    }
        //}
    }
}
