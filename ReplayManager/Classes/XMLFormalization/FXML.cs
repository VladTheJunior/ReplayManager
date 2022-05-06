using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace ReplayManager.Classes.XMLFormalization
{
    public static class FXML
    {
        public static async Task GenerateUnitsFile(string StringTablePath, string ProtoPath)
        {
            XmlDocument StringsXML = new XmlDocument();
            StringsXML.Load(StringTablePath);

            XmlDocument ProtoXML = new XmlDocument();
            ProtoXML.Load(ProtoPath);
            List<ProtoData> Units = new List<ProtoData>();
            await Task.Run(() =>
            {
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
            });
            await File.WriteAllTextAsync("Units.txt", JsonSerializer.Serialize(Units));
        }
        public static async Task GenerateTechsFile(string StringTablePath, string TechTreePath)
        {
            XmlDocument StringsXML = new XmlDocument();
            StringsXML.Load(StringTablePath);

            XmlDocument TechXML = new XmlDocument();
            TechXML.Load(TechTreePath);

            List<TechData> Techs = new List<TechData>();

            await Task.Run(() =>
            {
                var index = 0;
                foreach (XmlElement xnode in TechXML.DocumentElement)
                {

                    var name = xnode.GetAttribute("name");
                    var dname_id = xnode.SelectSingleNode("displaynameid")?.InnerText;
                    var rname_id = xnode.SelectSingleNode("rollovertextid")?.InnerText;

                    if (dname_id != null)
                    {
                        var dname = StringsXML.DocumentElement.SelectSingleNode($"language/string[@_locid='{dname_id}']").InnerText;

                        Techs.Add(new TechData() { ID = index, Name = name, DisplayName = dname });
                    }
                    else
                    {
                        Techs.Add(new TechData() { ID = index, Name = name, DisplayName = "" });

                    }
                    index++;
                }
            });
            await File.WriteAllTextAsync("Techs.txt", JsonSerializer.Serialize(Techs));
        }
        public static async Task GenerateDecksFile(string StringTablePath, string TechTreePath, List<string> HomecityPaths)
        {
            XmlDocument StringsXML = new XmlDocument();
            StringsXML.Load(StringTablePath);

            XmlDocument TechXML = new XmlDocument();
            TechXML.Load(TechTreePath);

            List<DeckData> Decks = new List<DeckData>();
            await Task.Run(() =>
            {
                foreach (var homecityPath in HomecityPaths)
                {
                    XmlDocument homecityXML = new XmlDocument();
                    homecityXML.Load(homecityPath);
                    List<CardData> Cards = new List<CardData>();
                    foreach (XmlElement xnode in homecityXML.DocumentElement.SelectSingleNode("cards"))
                    {
                        var name = xnode.SelectSingleNode("name")?.InnerText;
                        var age = Convert.ToInt32(xnode.SelectSingleNode("age")?.InnerText);
                        var max_count = Convert.ToInt32(xnode.SelectSingleNode("maxcount")?.InnerText);
                        var display_count = xnode.SelectSingleNode("displayunitcount")?.InnerText;

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
                                    CardData CardData = new CardData() { Age = age, CardID = i, maxCount = max_count, DisplayCount = display_count, CardName = name, DisplayName = dname, Icon = icon, RolloverText = rollname };
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
            });
            await File.WriteAllTextAsync("Decks.txt", JsonSerializer.Serialize(Decks));
        }
    }
}
