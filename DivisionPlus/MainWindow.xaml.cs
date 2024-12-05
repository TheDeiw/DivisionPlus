using DivisionPlus.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DivisionPlus.Exceptions;

namespace DivisionPlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Soldier> Soldiers { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            Soldiers = new ObservableCollection<Soldier>();

            SoldierDataGrid.ItemsSource = Soldiers;

            Application.Current.Exit += Application_Exit;
        }

        //Перевірка на ввід не цифр у поле
        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }

        //Додавання солдата
        private void Add_Soldier_Button_Click(object sender, RoutedEventArgs e)
        {
            AddSoldierWindow addSoldierWindow = new AddSoldierWindow(Soldiers);
            addSoldierWindow.ShowDialog();
        }

        //Редагування солдата
        private void Change_Soldier_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(SoldierDataGrid.SelectedItem is Soldier selectedSoldier))
                {
                    throw new Exception("Солдат не вибраний");
                }
                // Відкрити вікно для зміни даних солдата
                ChangeSoldierWindow changeSoldierWindow = new ChangeSoldierWindow(new Soldier(selectedSoldier));
                changeSoldierWindow.ShowDialog();

                // Оновлення значень вибраного солдата
                int index = Soldiers.IndexOf(selectedSoldier);
                if (index >= 0)
                {
                    Soldiers[index] = changeSoldierWindow.CurrentSoldier;
                }
                SoldierDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Видалення солдата
        private void Delete_Soldier_Button_Click(object sender, RoutedEventArgs e)
        {
            if (SoldierDataGrid.SelectedItem is Soldier selectedSoldier)
            {
                Soldiers.Remove(selectedSoldier); // Soldiers - ObservableCollection<Soldier>, яка є джерелом даних для таблиці
            }
            else
            {
                MessageBox.Show("Солдат не вибраний", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Пошук солдата
        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            string surname = SearchSurnameInput.Text;
            string name = SearchNameInput.Text;

            foreach (var soldier in Soldiers)
            {
                if (soldier.Surname == surname && soldier.Name == name)
                {
                    MessageBox.Show("Знайдено солдата" +
                        "\nНомер: " + soldier.Number +
                        "\nПрізвище: " + soldier.Surname +
                        "\nІм'я: " + soldier.Name +
                        "\nВік: " + soldier.Age +
                        "\nЗвання: " + soldier.RankString +
                        "\nВид крові: " + soldier.BloodTypeString +
                        "\nАрсенал: " + soldier.WeaponsString,
                        "Солдат знайдений", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            MessageBox.Show("Солдата не знайдено", "Солдат відсутній у списку", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;

        }

        //----------------------------------------------------------------------------------------------------------------
        //Фільтрування та його кнопки 
        private void Filter_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckBox[] BloodChecks = { Blood1P, Blood2P, Blood3P, Blood4P, Blood1M, Blood2M, Blood3M, Blood4M };
            //CheckBox[] ResusChecks = { Rh1, Rh2 };
            int SelectedRank = RankInput.SelectedIndex;
            string MaxAgeString = MaxAgeInput.Text;
            string MinAgeString = MinAgeInput.Text;
            string NumberWeaponString = WeaponNumberInput.Text;
            CheckBox[] AllowWeaponChecks = { AllowWeapon1, AllowWeapon2, AllowWeapon3 };
            int[] SelectedWeapons = { Weapon1Select.SelectedIndex, Weapon2Select.SelectedIndex, Weapon3Select.SelectedIndex };

            FilterWindow filterWindow = new FilterWindow(Soldiers, BloodChecks, 
                SelectedRank, MaxAgeString, MinAgeString, NumberWeaponString, AllowWeaponChecks, SelectedWeapons);
            filterWindow.ShowDialog();

        }

        private void Cancel_Filter_Button_Click(object sender, RoutedEventArgs e)
        {
            Blood1P.IsChecked = true;
            Blood2P.IsChecked = true;
            Blood3P.IsChecked = true;
            Blood4P.IsChecked = true;
            Blood1M.IsChecked = true;
            Blood2M.IsChecked = true;
            Blood3M.IsChecked = true;
            Blood4M.IsChecked = true;
            RankInput.SelectedIndex = 0;
            MaxAgeInput.Text = string.Empty;
            MinAgeInput.Text = string.Empty;
            WeaponNumberInput.Text = string.Empty;
            AllowWeapon1.IsChecked = true;
            AllowWeapon2.IsChecked = true;
            AllowWeapon3.IsChecked = true;
            Weapon1Select.SelectedIndex = 0;
            Weapon2Select.SelectedIndex = 0;
            Weapon3Select.SelectedIndex = 0;
        }

        private void MakeAllBloodClear()
        {
            Blood1M.IsChecked = false;
            Blood2M.IsChecked = false;
            Blood3M.IsChecked = false;
            Blood4M.IsChecked = false;
            Blood1P.IsChecked = false;
            Blood2P.IsChecked = false;
            Blood3P.IsChecked = false;
            Blood4P.IsChecked = false;
        }
        private void Donor1P_Button_Click(object sender, RoutedEventArgs e)
        {
            MakeAllBloodClear();
            Blood1P.IsChecked = true;
            Blood1M.IsChecked = true;
        }

        private void Donor2P_Button_Click(object sender, RoutedEventArgs e)
        {
            MakeAllBloodClear();
            Blood1P.IsChecked = true;
            Blood1M.IsChecked = true;
            Blood2P.IsChecked = true;
            Blood2M.IsChecked = true;
        }

        private void Donor3P_Button_Click(object sender, RoutedEventArgs e)
        {
            MakeAllBloodClear();
            Blood1P.IsChecked = true;
            Blood1M.IsChecked = true;
            Blood3P.IsChecked = true;
            Blood3M.IsChecked = true;
        }

        private void Donor4P_Button_Click(object sender, RoutedEventArgs e)
        {
            MakeAllBloodClear();
            Blood1P.IsChecked = true;
            Blood1M.IsChecked = true;
            Blood2P.IsChecked = true;
            Blood2M.IsChecked = true;
            Blood3P.IsChecked = true;
            Blood3M.IsChecked = true;
            Blood4P.IsChecked = true;
            Blood4M.IsChecked = true;
        }

        private void Donor1M_Button_Click(object sender, RoutedEventArgs e)
        {
            MakeAllBloodClear();
            Blood1M.IsChecked = true;
        }

        private void Donor2M_Button_Click(object sender, RoutedEventArgs e)
        {
            MakeAllBloodClear();
            Blood1M.IsChecked = true;
            Blood2M.IsChecked = true;
        }

        private void Donor3M_Button_Click(object sender, RoutedEventArgs e)
        {
            MakeAllBloodClear();
            Blood1M.IsChecked = true;
            Blood3M.IsChecked = true;
        }

        private void Donor4M_Button_Click(object sender, RoutedEventArgs e)
        {
            MakeAllBloodClear();
            Blood1M.IsChecked = true;
            Blood2M.IsChecked = true;
            Blood3M.IsChecked = true;
            Blood4M.IsChecked = true;
        }
        //----------------------------------------------------------------------------------------------------------------
        //Функції для роботи з файлами
        private void ReadFromFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                ReadFromJsonFile(openFileDialog.FileName);
            }
        }

        private void WriteToFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                WriteToJsonFile(saveFileDialog.FileName);
            }
        }

        private void ReadFromJsonFile(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath, Encoding.UTF8);

                var jsonDocument = JsonDocument.Parse(json);
                var root = jsonDocument.RootElement;

                Soldier.SoldiersCount = 0;
                if (root.TryGetProperty("Soldiers", out JsonElement soldiersElement))
                {
                    var soldiersFromFile = JsonSerializer.Deserialize<List<Soldier>>(soldiersElement.ToString());
                    if (soldiersFromFile != null)
                    {
                        int count = 0;
                        Soldiers.Clear();
                        foreach (var soldier in soldiersFromFile)
                        {
                            Soldiers.Add(soldier);
                            count++;
                        }
                        Soldier.SoldiersCount = count;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при зчитуванні з файлу: " + ex.Message);
            }
        }

        private void WriteToJsonFile(string filePath)
        {
            try
            {
                var data = new
                {
                    Soldiers = Soldiers,
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                string json = JsonSerializer.Serialize(data, options);
                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при записі у файл: " + ex.Message);
            }
        }

        
        
        //Первірка на зберігання файлу
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (Soldiers.Count > 0)
            {
                if (MessageBox.Show("У вас є незбережені дані\nХочете зберегти?", "Підтвердження", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        WriteToJsonFile(saveFileDialog.FileName);
                    }
                }
            }
        }
    }
}
