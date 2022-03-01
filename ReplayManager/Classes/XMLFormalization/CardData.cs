using System;
using System.IO;

namespace ReplayManager.Classes.XMLFormalization
{
    public class CardData
    {
        public int CardID { get; set; }
        public string CardName { get; set; }
        public int Age { get; set; }
        public string MaxCount { get; set; }
        public string DisplayCount { get; set; }
        public string DisplayName { get; set; }
        public string RolloverText { get; set; }
        public string Icon { get; set; }

        public string IconPath
        {
            get
            {
                return "pack://application:,,,/data/cards/" + Path.GetFileName(Icon);
            }
        }

        public string Tooltip
        {
            get
            {
                return string.IsNullOrEmpty(RolloverText) ? DisplayName : DisplayName + Environment.NewLine + RolloverText;
            }
        }
    }
}
