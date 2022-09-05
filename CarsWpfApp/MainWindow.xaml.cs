using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace CarsWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    // сортировка по модели
    public class SortByModel : IComparer<Cars>
    {
        public bool Flag { get; set; }
        
        int IComparer<Cars>.Compare(Cars x, Cars y)
        {
            if (Flag)
            {
                if (string.Compare(x.Model,y.Model) <= 0) return 1;
                if (string.Compare(x.Model, y.Model) > 0) return -1;
                else return 0;
            }
            else
            {
                if (string.Compare(x.Model, y.Model) <= 0) return -1;
                if (string.Compare(x.Model, y.Model) > 0) return 1;
                else return 0;
            }            
        }
    }

    // сортировка по марке
    public class SortByBrand : IComparer<Cars>
    {
        public bool Flag { get; set; }

        int IComparer<Cars>.Compare(Cars x, Cars y)
        {
            if (Flag)
            {
                if (string.Compare(x.Brand, y.Brand) <= 0) return 1;
                if (string.Compare(x.Brand, y.Brand) > 0) return -1;
                else return 0;
            }
            else
            {
                if (string.Compare(x.Brand, y.Brand) <= 0) return -1;
                if (string.Compare(x.Brand, y.Brand) > 0) return 1;
                else return 0;
            }
        }
    }

    // сортировка по дате выпуска
    public class SortByDate : IComparer<Cars>
    {
        public bool Flag { get; set; }
        int IComparer<Cars>.Compare(Cars x, Cars y)
        {

            if (Flag)
            {
                if (DateTime.Compare(x.DateOfManufacture, y.DateOfManufacture) >= 1) return 1;
                if (DateTime.Compare(x.DateOfManufacture, y.DateOfManufacture) < 1) return -1;
                else return 0;
            }
            else
            {
                if (DateTime.Compare(x.DateOfManufacture, y.DateOfManufacture) >= 1) return -1;
                if (DateTime.Compare(x.DateOfManufacture, y.DateOfManufacture) < 1) return 1;
                else return 0;
            }
        }
    }

    public partial class MainWindow : Window
    {
        Cars cars;
        bool brand, model, date;

        public MainWindow()
        {
            InitializeComponent();
            brand=model=date=true;
            cars = new Cars();
            listViewOne.ItemsSource = cars.carsCollection;            
        }

        // События вызова сортировщика
        // по модели
        private void sortByModel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (listViewOne.Items.Count > 0)
            {
                model = !model;
                cars.carsCollection = new ObservableCollection<Cars>(new SortedSet<Cars>(listViewOne.ItemsSource as ObservableCollection<Cars>, new SortByModel { Flag = model }));
                listViewOne.ItemsSource = cars.carsCollection;
            }
        }
        // по марке
        private void sortByBrand_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (listViewOne.Items.Count > 0)
            {
                brand = !brand;
                cars.carsCollection = new ObservableCollection<Cars>(new SortedSet<Cars>(listViewOne.ItemsSource as ObservableCollection<Cars>, new SortByBrand { Flag = brand }));
                listViewOne.ItemsSource = cars.carsCollection;
            }
        }
        // по дате выпуска
        private void sortByYear_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (listViewOne.Items.Count > 0)
            {
                date = !date;
                cars.carsCollection = new ObservableCollection<Cars>(new SortedSet<Cars>(listViewOne.ItemsSource as ObservableCollection<Cars>, new SortByDate { Flag = date }));
                listViewOne.ItemsSource = cars.carsCollection;
            }
        }

        // Ввод в поиск
        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex reg = new Regex((sender as TextBox).Text,RegexOptions.IgnoreCase);
            ObservableCollection<Cars> cars1 = new ObservableCollection<Cars>();

            foreach (Cars car in cars.carsCollection)
            {
                if (reg.IsMatch(car.Brand)|reg.IsMatch(car.Model))
                {
                    cars1.Add(car);
                }
            }
            listViewOne.ItemsSource = cars1;
        }

        // Добавить
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddItemWindow addWindow = new AddItemWindow();
            addWindow.DataContext = new Cars { Person = new Owner()};

            addWindow.Title = "Добавление объекта";
            addWindow.button.Content = "Добавить";

            if (addWindow.ShowDialog() == true)
            {
                cars.carsCollection.Add(addWindow.DataContext as Cars);
            }
        }

        // Удалить
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            //var collection = listViewOne.ItemsSource as ObservableCollection<Cars>;
            //cars.carsCollection.Remove(listViewOne.SelectedItems as Cars);
            m1:
            try
            {
                if (listViewOne.SelectedItems.Count > 0)
                {
                    foreach (object car in listViewOne.SelectedItems)
                    {
                        cars.carsCollection.Remove(car as Cars);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                goto m1;
            }
            
            listViewOne_GotFocus(listViewOne, null);
        }

        // Изменить
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            AddItemWindow addWindow = new AddItemWindow();
            addWindow.DataContext = new Cars(listViewOne.SelectedItem as Cars);

            addWindow.dobDayBox.Text = (addWindow.DataContext as Cars).Person.DayOfBirth.Day.ToString();
            addWindow.dobMonthBox.Text = (addWindow.DataContext as Cars).Person.DayOfBirth.Month.ToString();
            addWindow.dobYearBox.Text = (addWindow.DataContext as Cars).Person.DayOfBirth.Year.ToString();
            addWindow.dateDayBox.Text = (addWindow.DataContext as Cars).DateOfManufacture.Day.ToString();
            addWindow.dateMonthBox.Text = (addWindow.DataContext as Cars).DateOfManufacture.Month.ToString();
            addWindow.dateYearBox.Text = (addWindow.DataContext as Cars).DateOfManufacture.Year.ToString();

            addWindow.Title = "Изменение объекта";
            addWindow.button.Content = "Изменить";

            if (addWindow.ShowDialog()==true)
            {
                DelButton_Click(addWindow.DataContext, null);

                cars.carsCollection.Add(addWindow.DataContext as Cars);
                listViewOne.ItemsSource = cars.carsCollection;
                listViewOne.SelectedItem = addWindow.DataContext;
            }

            listViewOne_SelectionChanged(listViewOne.SelectedItem, null);
        }

        // загрузить из файла
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Текст(*.txt)|*.txt" + "|Все файлы (*.*)|*.*";
            fileDialog.CheckFileExists = true;

            if (fileDialog.ShowDialog() == true)
            {
                DataContext = cars.LoadCollection(fileDialog.FileName);
            }
        }

        // Save as
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (listViewOne.ItemsSource == null) throw new Exception();
            }
            catch (Exception)
            {
                MessageBox.Show("Empty list!"); return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Текст(*.txt)|*.txt" + "|Все файлы (*.*)|*.*";
            saveDialog.CheckFileExists = false;

            if (saveDialog.ShowDialog() == true)
                cars.SaveFile(saveDialog.FileName, listViewOne.ItemsSource as ObservableCollection<Cars>);
        }

        // Save
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = 
            MessageBox.Show("Overwrite file " + DataContext.ToString(), "Overwrite?",MessageBoxButton.YesNo);

            if(mbr.ToString()=="Yes")
                cars.SaveFile(DataContext.ToString(), listViewOne.ItemsSource as ObservableCollection<Cars>);
        }

        // статус кнопок Изменить и Удалить
        private void listViewOne_GotFocus(object sender, RoutedEventArgs e)
        {
            if (listViewOne.SelectedItem == null)
            {
                DelButton.IsEnabled = false;
                ChangeButton.IsEnabled = false;
            }
        }

        private void listViewOne_LostFocus(object sender, RoutedEventArgs e)
        {
            if (listViewOne.SelectedItem != null)
            {
                DelButton.IsEnabled = true;
                ChangeButton.IsEnabled = true;
            }   
        }

        // событие выбора строки в списке
        private void listViewOne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DelButton.IsEnabled = true;
            ChangeButton.IsEnabled = true;
            labelBackground.Content = "";
            labelBackground.Background = null;

            try
            {
                personName.Content = ((Cars)listViewOne.SelectedItem).Person.Name;
                personSurname.Content = ((Cars)listViewOne.SelectedItem).Person.Surname;
                dayOfBirth.Content = ((Cars)listViewOne.SelectedItem).Person.DayOfBirth.ToShortDateString();
                carBrand.Content = ((Cars)listViewOne.SelectedItem).Brand;
                carModel.Content = ((Cars)listViewOne.SelectedItem).Model;
                carColor.Content = ((Cars)listViewOne.SelectedItem).Color.colorText;

                CarColor colorOfCar = new CarColor(carColor.Content as string);
                labelBackground.Background = colorOfCar.Brush;

            }
            catch (NullReferenceException)
            {
                personName.Content="";
                personSurname.Content = "";
                dayOfBirth.Content = "";
                carBrand.Content = "";
                carModel.Content ="";
                carColor.Content = "";
            }
        }
    }
}
