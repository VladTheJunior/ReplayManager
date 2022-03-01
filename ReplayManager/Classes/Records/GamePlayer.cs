using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ReplayManager.Classes.Records
{
    public class GamePlayer
    {
        public string name { get; set; }
        public int teamid { get; set; }
        public string clan { get; set; } = null;
        public int color { get; set; }
        public int civ { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public float rating { get; set; }
        public float handicap { get; set; }
        public string aipersonality { get; set; }
        public string avatarid { get; set; }
        public string rank { get; set; }
        public string powerrating { get; set; }
        public string totalxp { get; set; }
        public string winratio { get; set; }
        public bool ready { get; set; }
        public int hclocation { get; set; }
        public int hclevel { get; set; }
        public string hcfilename { get; set; }
        public string homecityname { get; set; }
        public string explorername { get; set; }
        public int questid { get; set; }
        public int queststatus { get; set; }
        public int id { get; set; }
        public bool civishidden { get; set; }
        public bool civwasrandom { get; set; }
        public long explorerskinid { get; set; }

        public int SelectedDeckId { get; set; } = -1;

        public double CPM { get; set; }


        public ObservableCollection<PlayerDeck> Decks { get; set; } = new ObservableCollection<PlayerDeck>();

        public string Avatar
        {
            get
            {
                if (type != 0)
                {
                    return GameCiv != null ? GameCiv.ai_icon : "pack://application:,,,/data/avatars/cpai_avatar_empty.png";
                }


                var buf = avatarid.Split(' ');
                if (buf.Length < 2) return "pack://application:,,,/data/avatars/avatar_tier0_Starter_01.png";

                switch (Convert.ToInt32(buf[1]))
                {
                    case 0: return "pack://application:,,,/data/avatars/avatar_tier0_Starter_" + buf[0].PadLeft(2, '0') + ".png";
                    case 1:
                        return "pack://application:,,,/data/avatars/avatar_event0_" + buf[0].PadLeft(2, '0') + ".png";
                    case 2:
                        return "pack://application:,,,/data/avatars/avatar_persistent1_" + buf[0].PadLeft(2, '0') + ".png";
                    case 3:
                        return "pack://application:,,,/data/avatars/avatar_weekly_" + buf[0].PadLeft(2, '0') + ".png";
                    case 4:
                        return "pack://application:,,,/data/avatars/avatar_usa_" + buf[0].PadLeft(2, '0') + ".png";
                    case 5:
                        return "pack://application:,,,/data/avatars/avatar_mexico_" + buf[0].PadLeft(2, '0') + ".png";
                    default: return "pack://application:,,,/data/avatars/avatar_tier0_Starter_01.png";

                }
            }
        }

        public string Handicap
        {
            get
            {
                return handicap == 0 ? "" : $"Handicap: +{handicap}%";
            }
        }

        public string Team
        {
            get
            {
                switch (teamid)
                {
                    case 1: return "Team 1";
                    case 2: return "Team 2";
                    case 3: return "Team 3";
                    case 4: return "Team 4";
                    default: return "Team ?";
                }
            }
        }

        public string Clan
        {
            get
            {
                return string.IsNullOrEmpty(clan) ? null : clan;
            }
        }

        public string Type
        {
            get
            {
                return type == 0 ? "Human" : "AI";
            }
        }

        private List<GameCiv> Civs { get; set; } = new List<GameCiv>()
        {
            new GameCiv() { id = 1, name = "Spanish", icon = "pack://application:,,,/data/flags/Flag_Spanish.png",ai_icon= "pack://application:,,,/data/avatars/cpai_avatar_spanish.png"},
            new GameCiv() { id = 2, name = "British", icon = "pack://application:,,,/data/flags/Flag_British.png", ai_icon="pack://application:,,,/data/avatars/cpai_avatar_british.png"},
            new GameCiv() { id = 3, name = "French", icon = "pack://application:,,,/data/flags/Flag_French.png" , ai_icon="pack://application:,,,/data/avatars/cpai_avatar_french.png"},
            new GameCiv() { id = 4, name = "Portuguese", icon = "pack://application:,,,/data/flags/Flag_Portuguese.png", ai_icon="pack://application:,,,/data/avatars/cpai_avatar_portuguese.png" },
            new GameCiv() { id = 5, name = "Dutch", icon = "pack://application:,,,/data/flags/Flag_Dutch.png", ai_icon="pack://application:,,,/data/avatars/cpai_avatar_dutch.png" },
            new GameCiv() { id = 6, name = "Russians", icon = "pack://application:,,,/data/flags/Flag_Russian.png" ,ai_icon= "pack://application:,,,/data/avatars/cpai_avatar_russians.png"},
            new GameCiv() { id = 7, name = "Germans", icon = "pack://application:,,,/data/flags/Flag_German.png",ai_icon= "pack://application:,,,/data/avatars/cpai_avatar_germans.png" },
            new GameCiv() { id = 8, name = "Ottomans", icon = "pack://application:,,,/data/flags/Flag_Ottoman.png",ai_icon= "pack://application:,,,/data/avatars/cpai_avatar_ottomans.png" },
            new GameCiv() { id = 15, name = "Haudenosaunee", icon = "pack://application:,,,/data/flags/Flag_Iroquois.png", ai_icon="pack://application:,,,/data/avatars/cpai_avatar_iroquois.png" },
            new GameCiv() { id = 16, name = "Lakota", icon = "pack://application:,,,/data/flags/Flag_Sioux.png" ,ai_icon= "pack://application:,,,/data/avatars/cpai_avatar_sioux.png"},
            new GameCiv() { id = 17, name = "Aztec", icon = "pack://application:,,,/data/flags/Flag_Aztec.png", ai_icon="pack://application:,,,/data/avatars/cpai_avatar_aztec.png" },
            new GameCiv() { id = 19, name = "Japanese", icon = "pack://application:,,,/data/flags/Flag_Japanese.png",ai_icon= "pack://application:,,,/data/avatars/cpai_avatar_japanese.png" },
            new GameCiv() { id = 20, name = "Chinese", icon = "pack://application:,,,/data/flags/Flag_Chinese.png",ai_icon= "pack://application:,,,/data/avatars/cpai_avatar_chinese.png" },
            new GameCiv() { id = 21, name = "Indians", icon = "pack://application:,,,/data/flags/Flag_Indian.png", ai_icon="pack://application:,,,/data/avatars/cpai_avatar_indians.png" },
            new GameCiv() { id = 27, name = "Inca", icon = "pack://application:,,,/data/flags/Flag_Incan.png",ai_icon= "pack://application:,,,/data/avatars/cpai_avatar_inca.png" },
            new GameCiv() { id = 28, name = "Swedish", icon = "pack://application:,,,/data/flags/Flag_Swedish.png",ai_icon= "pack://application:,,,/data/avatars/cpai_avatar_swedish.png" },
            new GameCiv() { id = 38, name = "USA", icon = "pack://application:,,,/data/flags/Flag_American.png", ai_icon="pack://application:,,,/data/avatars/cpai_avatar_usa.png" },
            new GameCiv() { id = 39, name = "Ethiopians", icon = "pack://application:,,,/data/flags/Flag_Ethiopian.png",ai_icon= "pack://application:,,,/data/avatars/cpai_avatar_ethiopian.png" },
            new GameCiv() { id = 40, name = "Hausa", icon = "pack://application:,,,/data/flags/Flag_Hausa.png" , ai_icon="pack://application:,,,/data/avatars/cpai_avatar_hausa.png"},
            new GameCiv() { id = 42, name = "Mexicans", icon = "pack://application:,,,/data/flags/Flag_Mexican.png", ai_icon="pack://application:,,,/data/avatars/cpai_avatar_mexicans.png" },

        };

        public string hexcolor
        {
            get
            {
                switch (color)
                {

                    case 1: return "#2d2df5";
                    case 2: return "#d22828";
                    case 3: return "#e0e01e";
                    case 4: return "#910ff3";
                    case 5: return "#2ad43a";
                    case 6: return "#ea6900";
                    case 7: return "#1cc2db";
                    case 8: return "#eb61eb";
                    case 9: return "#1e1e1e";
                    case 10: return "#787878";
                    case 11: return "#d3d2bf";
                    case 12: return "#d3b46b";
                    default: return "#936c2e";

                }
            }
        }

        public GameCiv GameCiv
        {
            get
            {
                return Civs.FirstOrDefault(x => x.id == civ);
            }
        }
    }
}
