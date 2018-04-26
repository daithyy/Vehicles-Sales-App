using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CA1.Objects
{
    class Van : Vehicle
    {
        public enum Type
        {
            CombiVan,
            Dropside,
            PanelVan,
            Pickup,
            Tipper,
            Unlisted
        }
        public Type type;
        public enum Wheelbase
        {
            Short,
            Medium,
            Long,
            Unlisted
        }
        public Wheelbase wheelBase;

        public Van(string make, string model, double price, int year, 
            Color color, double mileage, string description, string image, Type type, Wheelbase wheelbase) 
            : base(make, model, price, year, color, mileage, description, image)
        {
            this.type = type;
            this.wheelBase = wheelbase;
        }
    }
}
