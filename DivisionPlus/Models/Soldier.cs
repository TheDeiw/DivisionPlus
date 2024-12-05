using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DivisionPlus.Models
{
    public enum SoldierWeapons
    {
        ПМ = 1,
        Glock_17,
        Форт_14,
        ТТ,
        МП5,
        М16,
        М4,
        Вулкан,
        Форт_224,
        АК_47,
        АК_74,
        М320,
        Форт_600А,
        RPG_40,
        УАГ_40,
        РПГ_7
    }
    public enum SoldierRank
    {
        Рекрут = 0,
        Солдат,
        Сержант,
        Штаб_сержант,
        Лейтенант,
        Капітан,
        Майор,
        Підполковник,
        Полковник,
        Генерал
    }
    public class Soldier 
    {
        public static int SoldiersCount { get; set; } = 0;

        [JsonIgnore]
        public int Number { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int BloodType { get; set; }
        public bool Rh {  get; set; }
        public SoldierRank Rank { get; set; }
        public List<SoldierWeapons> Weapons { get; set; }

        [JsonIgnore]
        public string BloodTypeString => $"{BloodType} {(Rh ? "+" : "-")}"; // Формат: тип + резус
        [JsonIgnore]
        public string RankString => Rank.ToString().Replace("_", "-"); // Перетворюємо Rank в текст
        [JsonIgnore]
        public string WeaponsString => string.Join(", ", Weapons.Select(w => w.ToString().Replace("_", "-"))); // Перетворюємо список зброї в текст

        public Soldier() 
        {
            Number = ++SoldiersCount;
            Surname = string.Empty;
            Name = string.Empty;
            Age = 0;
            BloodType = 0;
            Rh = false;
            Rank = 0;
            Weapons = new List<SoldierWeapons>();
        }
        public Soldier(string surname, string name, int age, int bloodType, bool rh, int rank, List<int> weapons)
        {
            Number = ++SoldiersCount;
            Surname = surname;
            Name = name;
            Age = age;
            BloodType = bloodType;
            Rh = rh;
            Rank = (SoldierRank)rank;
            Weapons = weapons.Select(w => (SoldierWeapons)w).ToList();
        }
        public Soldier(Soldier data)
        {
            Number = data.Number;
            Surname = data.Surname;
            Name = data.Name;
            Age = data.Age;
            BloodType = data.BloodType;
            Rh = data.Rh;
            Rank = data.Rank;
            Weapons = data.Weapons;
        }

        public bool HasWeapon(SoldierWeapons weapon)
        {
            return Weapons.Contains(weapon);
        }

        public static bool operator ==(Soldier? first, Soldier? second)
        {
            if (first is null || second is null)
            {
                return false;
            }

            return first.Equals(second);
        }
        public static bool operator !=(Soldier? first, Soldier? second)
        {
            return !(first == second);
        }
        public static bool operator >(Soldier? first, Soldier? second)
        {
            if (first is null || second is null)
            {
                return false;
            }

            return string.Compare(first.Surname, second.Surname) > 0;
        }
        public static bool operator <(Soldier? first, Soldier? second)
        {
            if (first is null || second is null)
            {
                return false;
            }

            return string.Compare(first.Surname, second.Surname) < 0;
        }
        public static bool operator >=(Soldier? first, Soldier? second)
        {
            if (first is null || second is null)
            {
                return false;
            }

            return string.Compare(first.Surname, second.Surname) >= 0;
        }
        public static bool operator <=(Soldier? first, Soldier? second)
        {
            if (first is null || second is null)
            {
                return false;
            }

            return string.Compare(first.Surname, second.Surname) <= 0;
        }
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj is not Soldier other)
            {
                return false;
            }

            return Name == other.Name && Surname == other.Surname && Age == other.Age
                && BloodType == other.BloodType && Rh == other.Rh;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number, Name, Surname, Age, BloodType, Rh);
        }
    }
}
