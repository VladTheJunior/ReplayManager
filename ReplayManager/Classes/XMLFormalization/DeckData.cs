using System.Collections.Generic;

namespace ReplayManager.Classes.XMLFormalization
{
    public class DeckData
    {
        public int CivID { get; set; }
        public List<CardData> Cards { get; set; } = new List<CardData>();
    }
}
