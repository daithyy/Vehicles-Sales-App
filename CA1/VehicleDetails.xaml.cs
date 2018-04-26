using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

using CA1.Objects;
using System.IO;

namespace CA1
{
    /// <summary>
    /// Interaction logic for VehicleDetails.xaml
    /// </summary>
    public partial class VehicleDetails : Window
    {
        MainWindow ownerWindow;
        public enum OptionType
        {
            Add,
            Edit
        }
        public OptionType Option;
        public MainWindow.RadioCheckedType CheckedRadioButton;
        string imgFilePath;
        object SelectedVehicle;

        public VehicleDetails()
        {
            InitializeComponent();
        }

        private void VehicleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ownerWindow = Owner as MainWindow;

            InitWindow();
        }

        private void InitWindow()
        {
            UpdateDetails(Option);
            UpdateFields(Option);
        }

        private object GetVehicle()
        {
            return ownerWindow.lbxVehicles.SelectedItem;
        }

        public void UpdateDetails(OptionType option)
        {
            switch (option)
            {
                case OptionType.Add:
                    Title = "Add Vehicle Details";
                    btnAdd.Content = "ADD";
                    //SelectedVehicle = null;
                    lblTitle.Content = UpdateAddSubtitle();
                    break;
                case OptionType.Edit:
                    Title = "Edit Vehicle Details";
                    btnAdd.Content = "EDIT";
                    SelectedVehicle = GetVehicle();
                    lblTitle.Content = UpdateEditSubtitle();
                    break;
                default:
                    break;
            }
        }

        private string UpdateEditSubtitle()
        {        
            if (SelectedVehicle.GetType().Equals(typeof(Car)))
                return "Edit Car";
            else if (SelectedVehicle.GetType().Equals(typeof(Van)))
                return "Edit Van";
            else if (SelectedVehicle.GetType().Equals(typeof(Bike)))
                return "Edit Bike";
            else // Is default type Vehicle
            {
                return "Edit Vehicle";
            }
        }

        private string UpdateAddSubtitle()
        {
            switch (CheckedRadioButton)
            {
                case MainWindow.RadioCheckedType.All:
                    return "Add Vehicle";
                case MainWindow.RadioCheckedType.Car:
                    return "Add Car";
                case MainWindow.RadioCheckedType.Bike:
                    return "Add Bike";
                case MainWindow.RadioCheckedType.Van:
                    return "Add Van";
                default:
                    return "Add Vehicle";
            }
        }

        private void UpdateFields(OptionType option)
        {
            switch (option)
            {
                case OptionType.Add:
                    HideFields(true);
                    combobxBodyType.ItemsSource = GetBodyTypes(); // This updates visibility
                    combobxBodyType.SelectedIndex = 0;
                    combobxWheelbase.ItemsSource = Enum.GetNames(typeof(Van.Wheelbase)).ToList();
                    combobxWheelbase.SelectedIndex = 0;
                    break;
                case OptionType.Edit:
                    HideFields(true);
                    AssignFields(SelectedVehicle as Vehicle);
                    break;
                default:
                    break;
            }
        }

        private void AssignFields(Vehicle obj)
        {
            txtblkModel.Text = obj.Model;
            txtblkMake.Text = obj.Make;
            txtblkPrice.Text = obj.Price.ToString();
            txtblkYear.Text = obj.Year.ToString();
            clrpickerDialog.SelectedColor = obj.Colour;
            txtblkMileage.Text = obj.Mileage.ToString();
            txtblkDescription.Text = obj.Description;
            imgFilePath = obj.Image;
            CheckExtraFields(obj);
        }

        private void CheckExtraFields(Vehicle obj)
        {
            if (obj.GetType().Equals(typeof(Car)))
            {
                Car tempCar = obj as Car;
                combobxBodyType.ItemsSource = Enum.GetNames(typeof(Car.BodyType)).ToList();
                combobxBodyType.SelectedItem = tempCar.bodyType.ToString();
                HideFields(false);
            }
            else if (obj.GetType().Equals(typeof(Bike)))
            {
                Bike tempBike = obj as Bike;
                combobxBodyType.ItemsSource = Enum.GetNames(typeof(Bike.Type)).ToList();
                combobxBodyType.SelectedItem = tempBike.type.ToString();
                HideFields(false);
            }
            else if (obj.GetType().Equals(typeof(Van)))
            {
                Van tempVan = obj as Van;
                combobxBodyType.ItemsSource = Enum.GetNames(typeof(Van.Type)).ToList();
                combobxBodyType.SelectedItem = tempVan.type.ToString();
                ShowAllFields();
            }
            else // Is default type Vehicle
            {
                HideFields(true);
            }
        }

        private void ShowAllFields()
        {
            lblBodyType.Visibility = Visibility.Visible;
            combobxBodyType.Visibility = Visibility.Visible;
            lblWheelbase.Visibility = Visibility.Visible;
            combobxWheelbase.Visibility = Visibility.Visible;
        }

        private void HideFields(bool hideAll)
        {
            if (hideAll)
            {
                lblBodyType.Visibility = Visibility.Collapsed;
                combobxBodyType.Visibility = Visibility.Collapsed;
                lblWheelbase.Visibility = Visibility.Collapsed;
                combobxWheelbase.Visibility = Visibility.Collapsed;
            }
            else // Show Body Type only
            {
                lblBodyType.Visibility = Visibility.Visible;
                combobxBodyType.Visibility = Visibility.Visible;
                lblWheelbase.Visibility = Visibility.Collapsed;
                combobxWheelbase.Visibility = Visibility.Collapsed;
            }
        }

        private List<string> GetBodyTypes()
        {
            switch (CheckedRadioButton)
            {
                case MainWindow.RadioCheckedType.All:
                    HideFields(true); // Hide all extra fields
                    return null;
                case MainWindow.RadioCheckedType.Car:
                    HideFields(false); // Display Body Type field only
                    return Enum.GetNames(typeof(Car.BodyType)).ToList();
                case MainWindow.RadioCheckedType.Bike:
                    HideFields(false);
                    return Enum.GetNames(typeof(Bike.Type)).ToList();
                case MainWindow.RadioCheckedType.Van:
                    ShowAllFields(); // Display both extra fields
                    return Enum.GetNames(typeof(Van.Type)).ToList();
                default:
                    HideFields(true);
                    return null;
            }
        }

        public void SetRadioButtonType(MainWindow owner) // ADD
        {
            if (owner.rBtnCars.IsChecked.Value)
                CheckedRadioButton = MainWindow.RadioCheckedType.Car;
            else if (owner.rBtnBikes.IsChecked.Value)
                CheckedRadioButton = MainWindow.RadioCheckedType.Bike;
            else if (owner.rBtnVans.IsChecked.Value)
                CheckedRadioButton = MainWindow.RadioCheckedType.Van;
            else
                CheckedRadioButton = MainWindow.RadioCheckedType.All;
        }

        private void AddVehicle()
        {
            switch (CheckedRadioButton)
            {
                case MainWindow.RadioCheckedType.All:
                    ownerWindow.Vehicles.Add(new Vehicle(
                        txtblkMake.Text, txtblkModel.Text, double.Parse(txtblkPrice.Text),
                        int.Parse(txtblkYear.Text), clrpickerDialog.SelectedColor.Value,
                        double.Parse(txtblkMileage.Text), txtblkDescription.Text, imgFilePath));
                    break;
                case MainWindow.RadioCheckedType.Car:
                    ownerWindow.Vehicles.Add(new Car(
                        txtblkMake.Text, txtblkModel.Text, double.Parse(txtblkPrice.Text),
                        int.Parse(txtblkYear.Text), clrpickerDialog.SelectedColor.Value,
                        double.Parse(txtblkMileage.Text), txtblkDescription.Text, imgFilePath,
                        (Car.BodyType)Enum.Parse(typeof(Car.BodyType), combobxBodyType.SelectedValue.ToString())));
                    break;
                case MainWindow.RadioCheckedType.Bike:
                    ownerWindow.Vehicles.Add(new Bike(
                        txtblkMake.Text, txtblkModel.Text, double.Parse(txtblkPrice.Text),
                        int.Parse(txtblkYear.Text), clrpickerDialog.SelectedColor.Value,
                        double.Parse(txtblkMileage.Text), txtblkDescription.Text, imgFilePath,
                        (Bike.Type)Enum.Parse(typeof(Bike.Type), combobxBodyType.SelectedValue.ToString())));
                    break;
                case MainWindow.RadioCheckedType.Van:
                    ownerWindow.Vehicles.Add(new Van(
                        txtblkMake.Text, txtblkModel.Text, double.Parse(txtblkPrice.Text),
                        int.Parse(txtblkYear.Text), clrpickerDialog.SelectedColor.Value,
                        double.Parse(txtblkMileage.Text), txtblkDescription.Text, imgFilePath,
                        (Van.Type)Enum.Parse(typeof(Van.Type), combobxBodyType.SelectedValue.ToString()),
                        (Van.Wheelbase)Enum.Parse(typeof(Van.Wheelbase), combobxWheelbase.SelectedValue.ToString())));
                    break;
            }
            ownerWindow.CheckVehicleType(CheckedRadioButton);
        }

        private void EditVehicle(Vehicle obj)
        {
            obj.Model = txtblkModel.Text;
            obj.Make = txtblkMake.Text;
            obj.Price = double.Parse(txtblkPrice.Text);
            obj.Year = int.Parse(txtblkYear.Text);
            obj.Colour = clrpickerDialog.SelectedColor.Value;
            obj.Mileage = double.Parse(txtblkMileage.Text);
            obj.Description = txtblkDescription.Text;
            obj.Image = imgFilePath;
            EditExtraFields(obj);
            ownerWindow.CheckVehicleType(CheckedRadioButton);
            ownerWindow.UpdateFields(obj);
            ownerWindow.UpdateExtraFields(obj);
        }

        private void EditExtraFields(Vehicle obj)
        {
            if (obj.GetType().Equals(typeof(Car)))
            {
                Car tempCar = obj as Car;
                tempCar.bodyType = (Car.BodyType)Enum.Parse(typeof(Car.BodyType),
                    combobxBodyType.SelectedItem.ToString());
                obj = tempCar;
            }
            else if (obj.GetType().Equals(typeof(Bike)))
            {
                Bike tempBike = obj as Bike;
                tempBike.type = (Bike.Type)Enum.Parse(typeof(Bike.Type),
                    combobxBodyType.SelectedItem.ToString());
                obj = tempBike;
            }
            else if (obj.GetType().Equals(typeof(Van)))
            {
                Van tempVan = obj as Van;
                tempVan.type = (Van.Type)Enum.Parse(typeof(Van.Type),
                    combobxBodyType.SelectedItem.ToString());
                tempVan.wheelBase = (Van.Wheelbase)Enum.Parse(typeof(Van.Wheelbase),
                    combobxBodyType.SelectedItem.ToString());
                obj = tempVan;
            }
            else // Is default type Vehicle
            {
                // Do nothing.
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            switch (Option)
            {
                case OptionType.Add:
                    AddVehicle();
                    break;
                case OptionType.Edit:
                    EditVehicle(SelectedVehicle as Vehicle);
                    break;
                default:
                    break;
            }
            this.Close();
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Images (*.JPG;*.JPEG;*.PNG) | *.JPG;*.JPEG;*.PNG";
            bool? result = dlg.ShowDialog();
            string filename = "";
            if (result == true)
            {
                filename = dlg.FileName;

                string imageDirectory = ownerWindow.GetImageDirectory();

                string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1);

                string newFile = imageDirectory + shortFileName;

                if (!File.Exists(newFile))
                    File.Copy(filename, newFile);

                imgFilePath = newFile;
            }
        }
    }
}
