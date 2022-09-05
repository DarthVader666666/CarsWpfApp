using System;
using System.Windows;
using System.Windows.Controls;

namespace CarsWpfApp
{
    /// <summary>
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    /// 
    public partial class AddItemWindow : Window
    {
        public AddItemWindow()
        {
            InitializeComponent();
            TextBlock tBlock;
            CarColor carColor = new CarColor();

            // заполнение цветов
            foreach (CarColor cc in carColor.colorList)
            {
                tBlock = new TextBlock
                { Text = cc.colorText, Background = cc.Brush };

                colorBox.Items.Add(tBlock);
            }

            // заполнение дат
            for (int i = 1900; i<=2021; i++)
            {
                if(i>=1970 && i<=2021) // год выпуска
                    dateYearBox.Items.Add(new TextBlock().Text = i.ToString());
                if (i >= 1900 && i <= 2003) // год рождения
                    dobYearBox.Items.Add(new TextBlock().Text = i.ToString());
            }

            dateYearBox.SelectedIndex = 0;
            dobYearBox.SelectedIndex = 0;

            for (int i = 1; i <= 12; i++)
            {
                dateMonthBox.Items.Add(new TextBlock().Text = i.ToString());
                dobMonthBox.Items.Add(new TextBlock().Text = i.ToString());
            }
            
            dateMonthBox.SelectedIndex = 0;
            dobMonthBox.SelectedIndex = 0;

            dateBox_SelectionChanged(dateDayBox, null); // заполнение днями месяца
            dateBox_SelectionChanged(dobDayBox, null);
        }

        // выбор дат выпуска и рождения
        private void dateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox dateBox = sender as ComboBox;
            ComboBox dayBox, monthBox, yearBox;
            DateTime date;

            if (dateBox.Name=="dateDayBox" || dateBox.Name == "dateMonthBox" || dateBox.Name == "dateYearBox")
            { dayBox = dateDayBox; monthBox = dateMonthBox; yearBox = dateYearBox; }
            else
            { dayBox = dobDayBox; monthBox = dobMonthBox; yearBox = dobYearBox; }

            try
            {
                int selectedIndex;
                // дней в месяце
                int daysInMonths;

                if (dayBox.SelectedIndex < 0) selectedIndex = 0;
                else selectedIndex = dayBox.SelectedIndex;

                // день
                if (dayBox.SelectedItem == null)
                {
                    date = new DateTime(int.Parse(yearBox.Text), int.Parse(monthBox.Text), 1);
                }
                else
                {
                    try
                    {
                        date = new DateTime(int.Parse(yearBox.SelectedItem.ToString()),
                             int.Parse(monthBox.SelectedItem.ToString()), int.Parse(dayBox.SelectedItem.ToString()));
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        date = new DateTime(int.Parse(yearBox.SelectedItem.ToString()),
                             int.Parse(monthBox.SelectedItem.ToString()), 1);
                    };
                }

                daysInMonths = DateTime.DaysInMonth(date.Year, date.Month);
                
                // перезаполнение combobox днями в месяце
                if (dayBox.Items.Count != daysInMonths)
                {  
                    dayBox.Items.Clear();

                    for (int i = 1 ; i <= daysInMonths; i++)
                    {
                        dayBox.Items.Add(new TextBlock().Text = i.ToString());
                    }
                }
                if (dayBox.SelectedIndex + 1 > daysInMonths)
                    dayBox.SelectedIndex = daysInMonths-1;
                else
                    dayBox.SelectedIndex = selectedIndex;
            }
            catch (FormatException) { return; }
        }

        // Обработчик кнопки в дополнительном окне
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Cars Data = DataContext as Cars;

            try
            {
                Data.Color = new CarColor(colorBox.Text); Data.Brand = brandBox.Text; Data.Model = modelBox.Text;
                Data.DateOfManufacture = new DateTime(int.Parse(dateYearBox.Text), int.Parse(dateMonthBox.Text), int.Parse(dateDayBox.Text));
                Data.DateShort = Data.DateOfManufacture.ToShortDateString();
                Data.Person.Name = nameBox.Text; Data.Person.Surname = surnameBox.Text;
                Data.Person.DayOfBirth = new DateTime(int.Parse(dobYearBox.Text), int.Parse(dobMonthBox.Text), int.Parse(dobDayBox.Text));

                DialogResult = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong input data!");
            }
        }
    }
}
