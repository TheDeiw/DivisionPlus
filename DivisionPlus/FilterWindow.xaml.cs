using DivisionPlus.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace DivisionPlus
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>

    public partial class FilterWindow : Window
    {
        private const int RifleCount = 5;
        private const int GranatometCount = 9;
        private ObservableCollection<Soldier> FilteredSoldiers { get; set; }
        private bool[] BloodTypes { get; set; } = { false, false, false, false, false, false, false, false };
        private int SelectedRank { get; set; }
        private int MaxAge { get; set; }
        private int MinAge { get; set; }
        private int WeaponNumber { get; set; }
        private int[] WeaponTypes { get; set; } = new int[3];
        private double Result { get; set; }
        public FilterWindow(ObservableCollection<Soldier> soldiers, CheckBox[] BloodChecks, int SelectedRank,
            string MaxAgeString, string MinAgeString, string NumberWeaponString, CheckBox[] AllowWeaponChecks, int[] SelectedWeapons)
        {
            InitializeComponent();
            FilteredSoldiers = new ObservableCollection<Soldier>();
            for (int i = 0; i < BloodChecks.Length; i++)
            {
                if (BloodChecks[i].IsChecked == true)
                {
                    BloodTypes[i] = true;
                }
            }
            
            this.SelectedRank = SelectedRank;
            if (string.IsNullOrEmpty(MaxAgeString))
            {
                MaxAge = int.MaxValue;
            }
            else
            {
                MaxAge = int.Parse(MaxAgeString);
            }
            if (string.IsNullOrEmpty(MinAgeString))
            {
                MinAge = int.MinValue;
            }
            else
            {
                MinAge = int.Parse(MinAgeString);
            }
            if (string.IsNullOrEmpty(NumberWeaponString))
            {
                WeaponNumber = -1;
            }
            else
            {
                WeaponNumber = int.Parse(NumberWeaponString);
            }
            for (int i = 0; i < AllowWeaponChecks.Length; i++)
            {
                if (AllowWeaponChecks[i].IsChecked == true)
                {
                    WeaponTypes[i] = SelectedWeapons[i];
                    if (i == 1 && WeaponTypes[i] != 0)
                    {
                        WeaponTypes[i] += RifleCount;
                    }
                    if (i == 2 && WeaponTypes[i] != 0)
                    {
                        WeaponTypes[i] += GranatometCount;
                    }
                }
                else
                {
                    WeaponTypes[i] = -1;
                }
            }

            foreach (var Soldier in soldiers)
            {
                if (CheckBloodType(Soldier) && CheckRank(Soldier) && CheckAge(Soldier) && CheckNumberWeapon(Soldier) && CheckWeaponType(Soldier))
                {
                    FilteredSoldiers.Add(Soldier);
                }
            }

            if (FilteredSoldiers.Count == 0)
            {
                MessageBox.Show("На жаль, за таким фільтром не знайдено людей", "Помилка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Dispatcher.BeginInvoke(new Action(Close));
            }

            FilterSoldierDataGrid.ItemsSource = FilteredSoldiers;

        }

        private bool CheckBloodType(Soldier currentSoldier)
        {
            if (BloodTypes[0] == true && currentSoldier.BloodType == 1 && currentSoldier.Rh == true)
            {
                return true;
            }
            if (BloodTypes[1] == true && currentSoldier.BloodType == 2 && currentSoldier.Rh == true)
            {
                return true;
            }
            if (BloodTypes[2] == true && currentSoldier.BloodType == 3 && currentSoldier.Rh == true)
            {
                return true;
            }
            if (BloodTypes[3] == true && currentSoldier.BloodType == 4 && currentSoldier.Rh == true)
            {
                return true;
            }
            if (BloodTypes[4] == true && currentSoldier.BloodType == 1 && currentSoldier.Rh == false)
            {
                return true;
            }
            if (BloodTypes[5] == true && currentSoldier.BloodType == 2 && currentSoldier.Rh == false)
            {
                return true;
            }
            if (BloodTypes[6] == true && currentSoldier.BloodType == 3 && currentSoldier.Rh == false)
            {
                return true;
            }
            if (BloodTypes[7] == true && currentSoldier.BloodType == 4 && currentSoldier.Rh == false)
            {
                return true;
            }
            return false;

            //for (int i = 0; i < BloodTypes.Length; i++)
            //{
            //    if (BloodTypes[i] == true && currentSoldier.BloodType == (i + 1))
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        private bool CheckRank(Soldier currentSoldier)
        {
            if (SelectedRank == 0 || (SelectedRank - 1) == (int)currentSoldier.Rank)
            {
                return true;
            }
            return false;
        }

        private bool CheckAge(Soldier currentSoldier)
        {
            if (currentSoldier.Age >= MinAge && currentSoldier.Age <= MaxAge) { return true; }
            return false;
        }

        private bool CheckNumberWeapon(Soldier currentSoldier)
        {
            if (WeaponNumber == -1 || WeaponNumber == currentSoldier.Weapons.Count) { return true; }
            return false;
        }

        private bool CheckWeaponType(Soldier currentSoldier)
        {
            // Перевірка для кожного типу зброї
            for (int i = 0; i < WeaponTypes.Length; i++)
            {
                int selectedWeaponType = WeaponTypes[i];

                if (selectedWeaponType == -1) 
                {
                    if (i == 0 && currentSoldier.Weapons.Any(w => (int)w >= 1 && (int)w <= 5))
                    {
                        return false; 
                    }
                    else if (i == 1 && currentSoldier.Weapons.Any(w => (int)w >= 6 && (int)w <= 11))
                    {
                        return false; 
                    }
                    else if (i == 2 && currentSoldier.Weapons.Any(w => (int)w >= 12 && (int)w <= 16))
                    {
                        return false; 
                    }
                }
                else if (selectedWeaponType == 0) 
                {
                    //if (i == 0 && currentSoldier.Weapons.Any(w => (int)w >= 1 && (int)w <= 5))
                    //{
                    //    return true;
                    //}
                    //else if (i == 1 && currentSoldier.Weapons.Any(w => (int)w >= 6 && (int)w <= 11))
                    //{
                    //    return true;
                    //}
                    //else if (i == 2 && currentSoldier.Weapons.Any(w => (int)w >= 12 && (int)w <= 16))
                    //{
                    //    return true;
                    //} else { return false; }
                    continue;
                }
                else 
                {
                    if (!currentSoldier.Weapons.Contains((SoldierWeapons)selectedWeaponType))
                    {
                        return false; 
                    }
                }
            }
            return true;
        }

        private double CountMiddleAge()
        {
            double middleAge = 0;
            int count = 0;

            foreach (var Soldier in FilteredSoldiers)
            {
                middleAge += Soldier.Age;
                count++;
            }

            middleAge = middleAge / count;
            return middleAge;
        }

        private void MidAge_Button_Click(object sender, RoutedEventArgs e)
        {
            Result = CountMiddleAge();
            ResultLabel.Content = "Середній вік бійців: " + Result;
        }

        private int CountWeapons()
        {
            int count = 0;
            foreach (var Soldier in FilteredSoldiers)
            {
                count += Soldier.Weapons.Count;
            }
            return count;
        }
        private void CountWeapon_Button_Click(object sender, RoutedEventArgs e)
        {
            Result = CountWeapons();
            ResultLabel.Content = "Кількість зброї у солдатів: " + Result;
        }


        private void WriteToJsonFile(string filePath)
        {
            try
            {
                var data = new
                {
                    Soldiers = FilteredSoldiers,
                    Results = new
                    {
                        MiddleAge = CountMiddleAge(),
                        TotalWeapons = CountWeapons()
                    }
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

        private void WriteToFile_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                WriteToJsonFile(saveFileDialog.FileName);
            }
        }

        private void Sort_Button_Click(object sender, RoutedEventArgs e)
        {
            int minChar = int.MaxValue;
            int maxChar = int.MinValue;

            foreach (var soldier in FilteredSoldiers)
            {
                if (!string.IsNullOrEmpty(soldier.Surname))
                {
                    int firstCharCode = (int)soldier.Surname[0];
                    if (firstCharCode < minChar) minChar = firstCharCode;
                    if (firstCharCode > maxChar) maxChar = firstCharCode;
                }
            }

            int range = maxChar - minChar + 1;
            List<Soldier>[] countArray = new List<Soldier>[range];

            for (int i = 0; i < range; i++)
            {
                countArray[i] = new List<Soldier>();
            }

            foreach (var soldier in FilteredSoldiers)
            {
                if (!string.IsNullOrEmpty(soldier.Surname))
                {
                    int index = (int)soldier.Surname[0] - minChar;
                    countArray[index].Add(soldier);
                }
            }

            FilteredSoldiers.Clear();
            for (int i = 0; i < range; i++)
            {
                foreach (var soldier in countArray[i].OrderBy(s => s.Surname))
                {
                    FilteredSoldiers.Add(soldier);
                }
            }
        }

        private void Cancel_Filter_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
