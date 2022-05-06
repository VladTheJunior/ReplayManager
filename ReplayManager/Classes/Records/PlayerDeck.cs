using ReplayManager.Classes.XMLFormalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReplayManager.Classes.Records
{
    public class PlayerDeck
    {
        public int CurrentDeckId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int GameId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsSelected { get; set; }
        public List<int> TechIds { get; set; } = new List<int>();
        public ObservableCollection<CardData> SortedCards { get; set; } = new ObservableCollection<CardData>();
        public ObservableCollection<CardData> Age1Cards { get; set; } = new ObservableCollection<CardData>();
        public ObservableCollection<CardData> Age2Cards { get; set; } = new ObservableCollection<CardData>();
        public ObservableCollection<CardData> Age3Cards { get; set; } = new ObservableCollection<CardData>();
        public ObservableCollection<CardData> Age4Cards { get; set; } = new ObservableCollection<CardData>();


        public string SelectedIcon
        {
            get
            {
                return IsSelected ? "pack://application:,,,/resources/fow_toggle.png" : "";
            }
        }
    }
}
