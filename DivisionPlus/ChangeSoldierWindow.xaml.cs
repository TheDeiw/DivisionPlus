using DivisionPlus.Exceptions;
using DivisionPlus.Models;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DivisionPlus
{
    /// <summary>
    /// Interaction logic for ChangeSoldier.xaml
    /// </summary>
    public partial class ChangeSoldierWindow : Window
    {
        public Soldier CurrentSoldier {  get; set; }
        public ChangeSoldierWindow(Soldier soldier)
        {
            InitializeComponent();
            CurrentSoldier = soldier;
            SoldierNumber.Content = CurrentSoldier.Number;
            SurnameInput.Text = CurrentSoldier.Surname;
            NameInput.Text = CurrentSoldier.Name;
            AgeInput.Text = soldier.Age.ToString();
            RankInput.SelectedIndex = (int)CurrentSoldier.Rank;
            BloodTypeInput.SelectedIndex = CurrentSoldier.BloodType - 1;
            ResusInput.SelectedIndex = CurrentSoldier.Rh ? 1 : 0;

            CheckBox[] weaponsCheckBox = { Weapon1, Weapon2, Weapon3, Weapon4, Weapon5, Weapon6,
                Weapon7, Weapon8, Weapon9, Weapon10, Weapon11, Weapon12, Weapon13,
                Weapon14, Weapon15, Weapon16 };
            for (int i = 1; i <= weaponsCheckBox.Length; i++)
            {
                if (CurrentSoldier.HasWeapon((SoldierWeapons)i))
                {
                    weaponsCheckBox[i-1].IsChecked = true;
                }
            }
        }


        private bool CheckInput(string surname, string name, string ageString, int selectedRank, int selectedBloodType, int selectedResus)
        {
            if (string.IsNullOrEmpty(surname))
            {
                throw new EmptyFieldException("Поле порожнє", "Прізвище");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new EmptyFieldException("Поле порожнє", "Ім'я");
            }
            if (string.IsNullOrEmpty(ageString))
            {
                throw new EmptyFieldException("Поле порожнє", "Вік");
            }
            if (selectedRank == -1)
            {
                throw new EmptyFieldException("Поле порожнє", "Звання");
            }
            if (selectedBloodType == -1)
            {
                throw new EmptyFieldException("Поле порожнє", "Група крові");
            }
            if (selectedResus == -1)
            {
                throw new EmptyFieldException("Поле порожнє", "Резус фактор");
            }
            int age = int.Parse(ageString);
            if (age < 18 || age > 60)
            {
                throw new AgeOutOfRange("Значення числа не входить у межі", 18, 60);
            }
            return true;
        }
        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
        private void Change_Soldier(string surname, string name, int age, int bloodType, bool rh, int rank, List<int> weapons)
        {
            CurrentSoldier.Surname = surname;
            CurrentSoldier.Name = name;
            CurrentSoldier.Age = age;
            CurrentSoldier.BloodType = bloodType;
            CurrentSoldier.Rh = rh;
            CurrentSoldier.Rank = (SoldierRank)rank;
            CurrentSoldier.Weapons = weapons.Select(w => (SoldierWeapons)w).ToList();
        }

        private void Change_Soldier_Button_Click(object sender, RoutedEventArgs e)
        {
            string surname = SurnameInput.Text;
            string name = NameInput.Text;
            string ageString = AgeInput.Text;
            int selectedRank = RankInput.SelectedIndex;
            int selectedBloodType = BloodTypeInput.SelectedIndex;
            int selectedResus = ResusInput.SelectedIndex;

            try
            {
                CheckInput(surname, name, ageString, selectedRank, selectedBloodType, selectedResus);
            }
            catch (EmptyFieldException ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.FieldName, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (AgeOutOfRange ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.StringBount, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int age = int.Parse(AgeInput.Text);
            selectedBloodType++;
            bool resus = selectedResus != 0;
            CheckBox[] weaponsCheckBox = { Weapon1, Weapon2, Weapon3, Weapon4, Weapon5, Weapon6,
                Weapon7, Weapon8, Weapon9, Weapon10, Weapon11, Weapon12, Weapon13,
                Weapon14, Weapon15, Weapon16 };
            List<int> weapons = new List<int>();

            for (int i = 0; i < weaponsCheckBox.Length; i++)
            {
                if (weaponsCheckBox[i].IsChecked == true)
                {
                    weapons.Add(i + 1);
                }
            }

            Change_Soldier(surname, name, age, selectedBloodType, resus, selectedRank, weapons);
            //DialogResult = soldier;
            Close();
            //return;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
