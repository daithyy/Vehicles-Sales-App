using CA1.Objects;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CA1
{
    public partial class MainWindow : Window
    {
        /*
         * Extra Features:
         * Google's Material Design Theme
         * WpfToolkit Extended Color Picker
         */

        public ObservableCollection<Vehicle> Vehicles = new ObservableCollection<Vehicle>();
        public Vehicle selectedObj;
        JsonSerializerSettings settings;

        public enum RadioCheckedType
        {
            All,
            Car,
            Bike,
            Van
        }
        public enum SortByType
        {
            Price,
            Mileage,
            Make
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            // Get Sort By list from enum.
            cbxSortBy.ItemsSource = Enum.GetNames(typeof(SortByType)).ToList();
            cbxSortBy.SelectedIndex = 0;

            // Update observable collection.
            rBtnAll.IsChecked = true;
            CheckVehicleType(RadioCheckedType.All);
        }

        public void CheckVehicleType(RadioCheckedType enumIn)
        {
            switch (enumIn)
            {
                case RadioCheckedType.All:
                    lbxVehicles.ItemsSource = Vehicles;
                    break;
                case RadioCheckedType.Car:
                    // Set the listbox item source to vehicles where their name
                    // is equal to the radio buttons name.
                    lbxVehicles.ItemsSource = Vehicles.Where(
                        v => v.GetType().Name.ToUpper().Equals(
                            RadioCheckedType.Car.ToString().ToUpper()));
                    break;
                case RadioCheckedType.Bike:
                    lbxVehicles.ItemsSource = Vehicles.Where(
                        v => v.GetType().Name.ToUpper().Equals(
                            RadioCheckedType.Bike.ToString().ToUpper()));
                    break;
                case RadioCheckedType.Van:
                    lbxVehicles.ItemsSource = Vehicles.Where(
                        v => v.GetType().Name.ToUpper().Equals(
                            RadioCheckedType.Van.ToString().ToUpper()));
                    break;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            OpenVehicleDetails(VehicleDetails.OptionType.Add);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lbxVehicles.SelectedItem != null)
                OpenVehicleDetails(VehicleDetails.OptionType.Edit);
            else
            {
                // Do nothing.
            }
        }

        private void OpenVehicleDetails(VehicleDetails.OptionType choosenOption)
        {
            //Create new window object
            VehicleDetails window = new VehicleDetails();

            //Set the owner of this new object
            window.Owner = this;

            //Set vehicle type to add (to display window title correctly)
            window.SetRadioButtonType(this);

            //Update Window Option
            window.Option = choosenOption;

            //Display the new window
            window.ShowDialog();
        }

        private void cbxSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        private void Sort()
        {
            string selection = cbxSortBy.SelectedItem as string; // Price, Mileage, Make

            if (selection != null)
            {
                switch (selection.ToUpper())
                {
                    case "PRICE":
                        lbxVehicles.ItemsSource = Vehicles.OrderBy(v => v.Price);
                        break;
                    case "MILEAGE":
                        lbxVehicles.ItemsSource = Vehicles.OrderBy(v => v.Mileage);
                        break;
                    case "MAKE":
                        lbxVehicles.ItemsSource = Vehicles.OrderBy(v => v.Make);
                        break;
                }
            }
        }

        public string GetImageDirectory()
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo parent = Directory.GetParent(currentDir);
            DirectoryInfo grandParent = Directory.GetParent(parent.FullName);
            string imageDirectory = grandParent + "\\images\\";

            return imageDirectory;
        }

        private void ShowAllFields()
        {
            lblBodyType.Visibility = Visibility.Visible;
            inputBodyType.Visibility = Visibility.Visible;
            lblWheelbase.Visibility = Visibility.Visible;
            inputWheelbase.Visibility = Visibility.Visible;
        }

        private void HideFields(bool hideAll)
        {
            if (hideAll)
            {
                lblBodyType.Visibility = Visibility.Collapsed;
                inputBodyType.Visibility = Visibility.Collapsed;
                lblWheelbase.Visibility = Visibility.Collapsed;
                inputWheelbase.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Show Body Type only
                lblBodyType.Visibility = Visibility.Visible;
                inputBodyType.Visibility = Visibility.Visible;
                lblWheelbase.Visibility = Visibility.Collapsed;
                inputWheelbase.Visibility = Visibility.Collapsed;
            }
        }

        public void UpdateFields(Vehicle obj)
        {
            inputModel.Content = obj.Model;
            inputMake.Content = obj.Make;
            inputPrice.Content = String.Format("{0:C2}", obj.Price);
            inputYear.Content = obj.Year;
            inputMileage.Content = obj.Mileage.ToString();
            inputDescription.Content = obj.Description;
            if (obj.Image != null)
            {
                imgVehicle.Source = new BitmapImage(
                new Uri(System.IO.Path.Combine(GetImageDirectory(), obj.Image),
                UriKind.Absolute));
            }
        }

        public void UpdateExtraFields(object obj)
        {
            if (obj.GetType().Equals(typeof(Car)))
            {
                Car tempCar = obj as Car;
                inputBodyType.Content = tempCar.bodyType;
                HideFields(false);
            }
            else if (obj.GetType().Equals(typeof(Van)))
            {
                Van tempVan = obj as Van;
                inputBodyType.Content = tempVan.type;
                inputWheelbase.Content = tempVan.wheelBase;
                ShowAllFields();
            }
            else if (obj.GetType().Equals(typeof(Bike)))
            {
                Bike tempBike = obj as Bike;
                inputBodyType.Content = tempBike.type;
                HideFields(false);
            }
            else // Is default type Vehicle
            {
                HideFields(true);
            }
        }

        private void EraseFields()
        {
            inputModel.Content = " ";
            inputMake.Content = " ";
            inputPrice.Content = " ";
            inputYear.Content = " ";
            inputMileage.Content = " ";
            inputBodyType.Content = null;
            inputWheelbase.Content = null;
            inputDescription.Content = " ";
            imgVehicle.Source = new BitmapImage(
                new Uri(System.IO.Path.Combine(GetImageDirectory(), "all.png"),
                UriKind.Absolute));
            HideFields(true);
        }

        private void lbxVehicles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object obj = lbxVehicles.SelectedItem as object;

            if (obj != null)
            {
                UpdateFields(obj as Vehicle);
                UpdateExtraFields(obj);
            }           
            else
                EraseFields();
        }

        private void rBtnAll_Checked(object sender, RoutedEventArgs e)
        {
            CheckVehicleType(RadioCheckedType.All);
        }

        private void rBtnCars_Checked(object sender, RoutedEventArgs e)
        {
            CheckVehicleType(RadioCheckedType.Car);
        }

        private void rBtnBikes_Checked(object sender, RoutedEventArgs e)
        {
            CheckVehicleType(RadioCheckedType.Bike);
        }

        private void rBtnVans_Checked(object sender, RoutedEventArgs e)
        {
            CheckVehicleType(RadioCheckedType.Van);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            object obj = lbxVehicles.SelectedItem as object;

            if (obj != null)
            {
                Vehicles.Remove(obj as Vehicle);
                EraseFields();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json file (*.json)|*.json|Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    string json = JsonConvert.SerializeObject(Vehicles, Formatting.Indented, settings);
                    sw.Write(json);
                }
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json file (*.json)|*.json|Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            if (openFileDialog.ShowDialog() == true)
            {
                using (StreamReader r = new StreamReader(openFileDialog.FileName))
                {
                    string json = r.ReadToEnd();
                    Vehicles = JsonConvert.DeserializeObject<ObservableCollection<Vehicle>>(json, settings);
                }
            }

            // Update observable collection.
            rBtnAll.IsChecked = true;
            CheckVehicleType(RadioCheckedType.All);
        }
    }
}
