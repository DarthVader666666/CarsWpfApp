using System;
using System.Windows;
using System.IO;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Text;

namespace CarsWpfApp
{
    public class Cars
    {
        string FileName { get; set; }
        public CarColor Color { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime DateOfManufacture { get; set; }
        public string DateShort { get; set; }
        public Owner Person {get; set;}
        public ObservableCollection<Cars> carsCollection { get; set; }

        public Cars(string colorText, string Brand, string Model, int Year, int Month, int Day, Owner Person)
        {
            this.Color = new CarColor(colorText);
            this.Brand = Brand;
            this.Model = Model;
            this.DateOfManufacture = new DateTime(Year,Month,Day);
            this.DateShort = DateOfManufacture.ToShortDateString();
            this.Person = Person;
        }

        public Cars(Cars car)
        {
            this.Color = car.Color;
            this.Brand = car.Brand;
            this.Model = car.Model;
            this.DateOfManufacture = new DateTime(car.DateOfManufacture.Year, car.DateOfManufacture.Month, car.DateOfManufacture.Day);
            this.DateShort = DateOfManufacture.ToShortDateString();
            this.Person = new Owner { Name=car.Person.Name, Surname=car.Person.Surname, DayOfBirth=new DateTime(car.Person.DayOfBirth.Year, car.Person.DayOfBirth.Month, car.Person.DayOfBirth.Day) };
        }

        public Cars()
        {
            FileName="";
            carsCollection = new ObservableCollection<Cars>();
        }

        public string LoadCollection(string filename)
        {
            Regex reg = new Regex(@".*.txt");

            try
            { if (!reg.IsMatch(filename)) throw new Exception(); }
            catch
            { MessageBox.Show("Wrong file format!"); return null; }

            try
            { carsCollection.Clear(); }
            catch (NullReferenceException)
            { MessageBox.Show("Nullable collection"); return null;  }

            FileName = filename;

            string[] line;
            string[] date;
            string[] dob;

            StreamReader sr = new StreamReader(filename, Encoding.UTF8);

            while (((filename = sr.ReadLine())!="") & (filename != null))
            {
                try
                {
                    line = filename.Split('|');
                    date = line[3].Split('.');
                    dob = line[6].Split('.');

                    carsCollection.Add(new Cars(line[0], line[1], line[2],
                        int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]),
                        new Owner 
                        {
                            Name = line[4],
                            Surname = line[5],
                            DayOfBirth =
                        new DateTime(int.Parse(dob[0]), int.Parse(dob[1]), int.Parse(dob[2]))
                        }));
                }
                catch (Exception)
                {
                    MessageBox.Show("Data damaged!"); sr.Close();  return null;
                }
            }

            sr.Close();

            return FileName;
        }

        //запись файла
        public void SaveFile(string fileName, ObservableCollection<Cars> collection)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding(1251));
                foreach (Cars car in collection)
                {
                    sw.WriteLine(car.Color.colorText + '|' + car.Brand + '|' + car.Model + '|' + car.DateOfManufacture.Year + '.' +
                    car.DateOfManufacture.Month + '.' + car.DateOfManufacture.Day + '|' +
                    car.Person.Name + '|' + car.Person.Surname + '|' + car.Person.DayOfBirth.Year + '.' + car.Person.DayOfBirth.Month + '.' +
                    car.Person.DayOfBirth.Day);
                }
                sw.Close();
                MessageBox.Show("Сохранено в " + fileName);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("File not found!");
                return;
            }
        }
    }
}
