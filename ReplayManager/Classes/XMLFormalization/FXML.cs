using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReplayManager.Classes.XMLFormalization
{
    public class DeckData
    {
        public int CivID { get; set; }
        public List<CardData> Cards { get; set; } = new List<CardData>();
    }

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
    public class ProtoData
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
    }

    public class TechData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string RolloverText { get; set; }
        public string Icon { get; set; }
    }

    public class FXML
    {
        public FXML(string StringTablePath, string ProtoPath, string TechTreePath, List<string> HomecityPaths)
        {
            XmlDocument StringsXML = new XmlDocument();
            StringsXML.Load(StringTablePath);

            XmlDocument ProtoXML = new XmlDocument();
            ProtoXML.Load(ProtoPath);

            XmlDocument TechXML = new XmlDocument();
            TechXML.Load(TechTreePath);

            List<ProtoData> Units = new List<ProtoData>();
            List<TechData> Techs = new List<TechData>();

            foreach (XmlElement xnode in ProtoXML.DocumentElement)
            {
                var id = Convert.ToInt32(xnode.GetAttribute("id"));
                var name_id = xnode.SelectSingleNode("displaynameid")?.InnerText;
                if (name_id != null)
                {
                    var name = StringsXML.DocumentElement.SelectSingleNode($"language/string[@_locid='{name_id}']").InnerText;
                    Units.Add(new ProtoData() { ID = id, DisplayName = name });
                }
            }

            foreach (XmlElement xnode in TechXML.DocumentElement)
            {
                var id = Convert.ToInt32(xnode.SelectSingleNode("dbid")?.InnerText);
                var name = xnode.GetAttribute("name");
                var dname_id = xnode.SelectSingleNode("displaynameid")?.InnerText;
                var rname_id = xnode.SelectSingleNode("rollovertextid")?.InnerText;
                var icon = xnode.SelectSingleNode("icon")?.InnerText;
                if (dname_id != null)
                {
                    var dname = StringsXML.DocumentElement.SelectSingleNode($"language/string[@_locid='{dname_id}']").InnerText;
                    var rname = StringsXML.DocumentElement.SelectSingleNode($"language/string[@_locid='{rname_id}']");
                    var rollname = rname != null ? rname?.InnerText : null;
                    Techs.Add(new TechData() { ID = id, Name = name, Icon = icon, RolloverText = rollname, DisplayName = dname });
                }
            }
            File.WriteAllText("Techs.txt", JsonConvert.SerializeObject(Techs));

            List<DeckData> Decks = new List<DeckData>();


            foreach (var homecityPath in HomecityPaths)
            {
                XmlDocument homecityXML = new XmlDocument();
                homecityXML.Load(homecityPath);
                List<CardData> Cards = new List<CardData>();
                foreach (XmlElement xnode in homecityXML.DocumentElement.SelectSingleNode("cards"))
                {
                    var name = xnode.SelectSingleNode("name")?.InnerText;
                    var age = Convert.ToInt32(xnode.SelectSingleNode("age")?.InnerText);
                    var max_count = xnode.SelectSingleNode("maxcount")?.InnerText.Replace("-1", "∞").Replace("2", "2x");
                    var display_count = xnode.SelectSingleNode("displayunitcount")?.InnerText;

                    var index = -1;
                    for (int i = 0; i < TechXML.DocumentElement.ChildNodes.Count; i++)
                    {
                        if (TechXML.DocumentElement.ChildNodes[i].Attributes["name"].Value.ToLower() == name.ToLower())
                        {
                            var dname_id = TechXML.DocumentElement.ChildNodes[i].SelectSingleNode("displaynameid")?.InnerText;
                            var rname_id = TechXML.DocumentElement.ChildNodes[i].SelectSingleNode("rollovertextid")?.InnerText;
                            var icon = TechXML.DocumentElement.ChildNodes[i].SelectSingleNode("icon")?.InnerText;
                            if (dname_id != null)
                            {
                                var dname = StringsXML.DocumentElement.SelectSingleNode($"language/string[@_locid='{dname_id}']").InnerText;
                                var rname = StringsXML.DocumentElement.SelectSingleNode($"language/string[@_locid='{rname_id}']");
                                var rollname = rname != null ? rname?.InnerText : null;
                                CardData CardData = new CardData() { Age = age, CardID = i, MaxCount=max_count, DisplayCount = display_count, CardName = name, DisplayName = dname, Icon = icon, RolloverText = rollname };
                                Cards.Add(CardData);

                            }
                            break;
                        }
                    }

                }
                var civ_id = -1;


                if (homecityPath.Contains("spanish")) civ_id = 1;
                else if (homecityPath.Contains("british")) civ_id = 2;
                else if (homecityPath.Contains("french")) civ_id = 3;
                else if (homecityPath.Contains("portuguese")) civ_id = 4;
                else if (homecityPath.Contains("dutch")) civ_id = 5;
                else if (homecityPath.Contains("russians")) civ_id = 6;
                else if (homecityPath.Contains("german")) civ_id = 7;
                else if (homecityPath.Contains("ottomans")) civ_id = 8;
                else if (homecityPath.Contains("iroquois")) civ_id = 15;
                else if (homecityPath.Contains("sioux")) civ_id = 16;
                else if (homecityPath.Contains("aztec")) civ_id = 17;
                else if (homecityPath.Contains("japanese")) civ_id = 19;
                else if (homecityPath.Contains("chinese")) civ_id = 20;
                else if (homecityPath.Contains("indians")) civ_id = 21;
                else if (homecityPath.Contains("inca")) civ_id = 27;
                else if (homecityPath.Contains("swedish")) civ_id = 28;
                else if (homecityPath.Contains("americans")) civ_id = 38;
                else if (homecityPath.Contains("ethiopians")) civ_id = 39;
                else if (homecityPath.Contains("hausa")) civ_id = 40;
                else if (homecityPath.Contains("mexicans")) civ_id = 42;

                Decks.Add(new DeckData() { Cards = Cards, CivID = civ_id });
            }



            File.WriteAllText("Decks.txt", JsonConvert.SerializeObject(Decks));

        }
    }
}
