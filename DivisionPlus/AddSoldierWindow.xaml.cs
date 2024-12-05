using DivisionPlus.Exceptions;
using DivisionPlus.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DivisionPlus
{
    /// <summary>
    /// Interaction logic for AddSoldierWindow.xaml
    /// </summary>
    public partial class AddSoldierWindow : Window
    {
        ObservableCollection<Soldier> Soldiers;
        public AddSoldierWindow(ObservableCollection<Soldier> soldiers)
        {
            InitializeComponent();
            Soldiers = soldiers;
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

        private void Add_SoldierData_Button_Click(object sender, RoutedEventArgs e)
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

            Soldiers.Add(new Soldier(surname, name, age, selectedBloodType, resus, selectedRank, weapons));
            Close();
        }

        private void Cancel_Adding_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
    }
}
