using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media;

namespace CarsWpfApp
{
    public class CarColor
    {
        public string[] colorArray = new string[]{"белый","черный","серый","красный","ораньжевый",
        "желтый","зеленый","голубой","синий","феолетовый","серебристый","коричневый","бежевый"};
        public string colorText { get; set; }
        public SolidColorBrush Brush { get; set; }
        public ObservableCollection<CarColor> colorList;
        //public CarColor(string color, SolidColorBrush Brush)
        //{
        //    this.colorText = colorText; this.Brush = Brush;
        //}

        public CarColor(string text)
        {
            switch (text)
            {
                case ("белый"): this.colorText = text; Brush = Brushes.White; break;
                case ("черный"): this.colorText = text; Brush = Brushes.Black; break;
                case ("серый"): this.colorText = text; Brush = Brushes.Gray; break;
                case ("красный"): this.colorText = text; Brush = Brushes.Red; break;
                case ("ораньжевый"): this.colorText = text; Brush = Brushes.Orange; break;
                case ("желтый"): this.colorText = text; Brush = Brushes.Yellow; break;
                case ("зеленый"): this.colorText = text; Brush = Brushes.Green; break;
                case ("голубой"): this.colorText = text; Brush = Brushes.Blue; break;
                case ("синий"): this.colorText = text; Brush = Brushes.DarkBlue; break;
                case ("феолетовый"): this.colorText = text; Brush = Brushes.Violet; break;
                case ("серебристый"): this.colorText = text; Brush = Brushes.Silver; break;
                case ("коричневый"): this.colorText = text; Brush = Brushes.Brown; break;
                case ("бежевый"): this.colorText = text; Brush = Brushes.Beige; break;
                default: this.colorText = "?"; Brush = Brushes.White; break;
            }
        }

        public CarColor()
        {
            colorList = new ObservableCollection<CarColor>();

            foreach (string c in colorArray)
            {
                colorList.Add(new CarColor(c));
            }
        }
    }
}
