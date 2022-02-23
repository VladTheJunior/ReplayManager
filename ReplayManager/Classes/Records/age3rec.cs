using Newtonsoft.Json;
using ReplayManager.Classes.XMLFormalization;
using Resource_Manager.Classes.L33TZip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReplayManager.Classes.Records
{
    public class PlayerDeck
    {
        public int CurrentDeckId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int GameId { get; set; }
        public bool IsDefault { get; set; }

        public List<int> TechIds { get; set; } = new List<int>();

        public ObservableCollection<CardData> Age1Cards { get; set; } = new ObservableCollection<CardData>();
        public ObservableCollection<CardData> Age2Cards { get; set; } = new ObservableCollection<CardData>();
        public ObservableCollection<CardData> Age3Cards { get; set; } = new ObservableCollection<CardData>();
        public ObservableCollection<CardData> Age4Cards { get; set; } = new ObservableCollection<CardData>();
    }

    public class GameVersion
    {
        public uint ExeVersion { get; set; }
        public uint PatchVersion { get; set; }
        public string PatchNotes { get; set; }
    }

    public class GameCiv
    {
        public int id { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public string ai_icon { get; set; }
    }

    public class GamePlayer
    {
        public string name { get; set; }
        public int teamid { get; set; }
        public string clan { get; set; }
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
                return handicap == 0 ? "not set" : $"+{handicap}%";
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
    public class age3rec : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private BinaryReader Reader { get; set; }
        byte MagicNumber { get; set; }
        byte IsMultiplayer { get; set; }

        private byte viewPoint;
        public byte ViewPoint
        {
            get
            {
                return viewPoint;
            }
            set
            {
                viewPoint = value;
                NotifyPropertyChanged();
            }
        }

        public GamePlayer RecordOwner
        {
            get
            {
                if (Players.Count > 0)
                {
                    return Players[ViewPoint - 1];
                }
                else
                {
                    return null;
                }

            }
        }

        private string exeInfo = "";
        public string ExeInfo
        {
            get
            {
                return exeInfo;
            }
            set
            {
                exeInfo = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("ExeVersion");
                NotifyPropertyChanged("GameVersion");
            }
        }

        private string gameTitle;
        public string GameTitle
        {
            get
            {

                return string.IsNullOrEmpty(gameTitle) ? null : gameTitle;
            }
            set
            {
                gameTitle = value;
                NotifyPropertyChanged();
            }
        }

        private int gameNumPlayers;
        public int GameNumPlayers
        {
            get
            {
                return gameNumPlayers;
            }
            set
            {
                gameNumPlayers = value;
                NotifyPropertyChanged();
            }
        }
        private bool gameFFA;
        public bool GameFFA
        {
            get
            {
                return gameFFA;
            }
            set
            {
                gameFFA = value;
                NotifyPropertyChanged();
            }
        }
        bool GameNomadStart { get; set; }

        private int gameMapSize;
        public int GameMapSize
        {
            get
            {
                return gameMapSize;
            }
            set
            {
                gameMapSize = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("MapSize");
            }
        }

        public bool MapSize
        {
            get
            {
                return GameMapSize == 0 ? false : true;
            }
        }

        private string gameFileName = "";
        public string GameFileName
        {
            get
            {
                return gameFileName;
            }
            set
            {
                gameFileName = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("MapIcon");
            }
        }

        public string MapIcon
        {
            get
            {
                var map_name = GameFileName.ToLower();
                map_name = map_name.Replace("large", "");
                map_name = map_name.Replace(" ", "_");
                map_name = map_name.Replace("af_", "af");
                map_name = map_name.Replace("af", "af_");

                return "pack://application:,,,/data/maps/" + map_name + "_mini.png";
            }
        }


        private int gameStartingResources;
        public int GameStartingResources
        {
            get
            {
                return gameStartingResources;
            }
            set
            {
                gameStartingResources = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("StartingResources");
            }
        }

        public string StartingResources
        {
            get
            {
                switch (GameStartingResources)
                {
                    default: return "Standard";
                    case 1: return "Medium";
                    case 2: return "High";
                    case 3: return "Ultra";
                    case 4: return "Infinite";
                    case 5: return "Random";
                }
            }
        }


        private bool gameStartWithTreaty;
        public bool GameStartWithTreaty
        {
            get
            {
                return gameStartWithTreaty;
            }
            set
            {
                gameStartWithTreaty = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Rules");
            }
        }

        private int gameNoRush;
        public int GameNoRush
        {
            get
            {
                return gameNoRush;
            }
            set
            {
                gameNoRush = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Rules");
            }
        }

        private bool gameTradeMonopoly;
        public bool GameTradeMonopoly
        {
            get
            {
                return gameTradeMonopoly;
            }
            set
            {
                gameTradeMonopoly = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Rules");
            }
        }

        private bool gameBlockade;
        public bool GameBlockade
        {
            get
            {
                return gameBlockade;
            }
            set
            {
                gameBlockade = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Rules");
            }
        }

        private bool gameKOTH;
        public bool GameKOTH
        {
            get
            {
                return gameKOTH;
            }
            set
            {
                gameKOTH = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Rules");
            }
        }

        private int gameDifficulty;
        public int GameDifficulty
        {
            get
            {
                return gameDifficulty;
            }
            set
            {
                gameDifficulty = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Difficulty");
            }
        }

        public string Difficulty
        {
            get
            {
                switch (GameDifficulty)
                {
                    default: return "Easy";
                    case 1: return "Standard";
                    case 2: return "Moderate";
                    case 3: return "Hard";
                    case 4: return "Hardest";
                    case 5: return "Extreme";
                }
            }
        }

        private bool gameRecordGame;
        public bool GameRecordGame
        {
            get
            {
                return gameRecordGame;
            }
            set
            {
                gameRecordGame = value;
                NotifyPropertyChanged();
            }
        }

        private int gameStartingAge;
        public int GameStartingAge
        {
            get
            {
                return gameStartingAge;
            }
            set
            {
                gameStartingAge = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("StartingAge");
            }
        }

        public string StartingAge
        {
            get
            {
                if (GameNomadStart)
                {
                    return "Nomad";
                }

                switch (GameStartingAge)
                {
                    default: return "Exploration Age";
                    case 1: return "Commerce Age";
                    case 2: return "Fortress Age";
                    case 3: return "Industrial Age";
                    case 4: return "Post-Industrial Age";
                    case 5: return "Imperial Age";
                    case 6: return "Post-Imperial Age";
                }
            }
        }

        private int gameEndingAge = -1;
        public int GameEndingAge
        {
            get
            {
                return gameEndingAge;
            }
            set
            {
                gameEndingAge = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("EndingAge");
            }
        }

        public string EndingAge
        {
            get
            {
                switch (GameEndingAge)
                {
                    case 0: return "Exploration Age";
                    case 1: return "Commerce Age";
                    case 2: return "Fortress Age";
                    case 3: return "Industrial Age";
                    default: return "Imperial Age";
                }
            }
        }

        private int gameSpeed;
        public int GameSpeed
        {
            get
            {
                return gameSpeed;
            }
            set
            {
                gameSpeed = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Speed");
            }
        }

        public string Speed
        {
            get
            {
                switch (GameSpeed)
                {
                    case 0: return "Slow";
                    case 1: return "Medium";
                    default: return "Fast";
                }
            }
        }

        private int gameModeType;
        public int GameModeType
        {
            get
            {
                return gameModeType;
            }
            set
            {
                gameModeType = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Rules");
            }
        }

        public string Rules
        {
            get
            {
                var mode = "";
                switch (gameModeType)
                {
                    case 0: mode = "Supremacy"; break;
                    case 1: mode = "Deathmatch"; break;
                    case 2: mode = "Empire Wars"; break;
                    case 3: mode = "Ranked Treaty"; break;
                    case 4: mode = "Ranked Deathmatch"; break;
                    case 5: mode = "Co-op Battle"; break;
                    case 6: mode = "Scenario"; break;
                    case 7: mode = "Saved Game"; break;
                }

                if (GameNoRush > 0)
                {
                    if (!GameBlockade)
                    {
                        mode = mode + $" (No Rush {GameNoRush} min., No Blockade)";
                    }
                }

                if (GameKOTH)
                {
                    mode = mode + $" (King Of The Hill)";
                }

                if (GameTradeMonopoly)
                {
                    mode = mode + $" (Trade Monopoly Enabled)";
                }


                return mode;
            }
        }

        private int gameHandicapMode;
        public int GameHandicapMode
        {
            get
            {
                return gameHandicapMode;
            }
            set
            {
                gameHandicapMode = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Handicap");
            }
        }

        public bool Handicap
        {
            get
            {
                return GameHandicapMode != 0 ? true : false;
            }
        }

        private bool gameAllowCheats;
        public bool GameAllowCheats
        {
            get
            {
                return gameAllowCheats;
            }
            set
            {
                gameAllowCheats = value;
                NotifyPropertyChanged();
            }
        }


        private string gamePassword;
        public string GamePassword
        {
            get
            {
                return gamePassword;
            }
            set
            {
                gamePassword = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<GamePlayer> Players { get; set; } = new ObservableCollection<GamePlayer>();
        private ObservableCollection<PlayerDeck> Decks { get; set; } = new ObservableCollection<PlayerDeck>();
        private List<GameVersion> Versions { get; set; } = new List<GameVersion>();

        private List<DeckData> DeckDatas { get; set; }


        public uint ExeVersion
        {
            get
            {
                var buf = ExeInfo.Split(' ');
                return buf.Length > 1 ? Convert.ToUInt32(buf[1]) : 0;
            }
        }

        public GameVersion GameVersion
        {
            get
            {
                return Versions.FirstOrDefault(x => x.ExeVersion == ExeVersion);
            }
        }

        private void ReadGamePlayers()
        {
            for (int i = 0; i < 13; i++)
            {
                Players.Add(new GamePlayer()
                {
                    name = ReadGameInfoString($"gameplayer{i}name"),
                    teamid = ReadGameInfoInt32($"gameplayer{i}teamid"),
                    clan = ReadGameInfoString($"gameplayer{i}clan"),
                    color = ReadGameInfoInt32($"gameplayer{i}color"),
                    civ = ReadGameInfoInt32($"gameplayer{i}civ"),
                    type = ReadGameInfoInt32($"gameplayer{i}type"),
                    status = ReadGameInfoInt32($"gameplayer{i}status"),
                    rating = ReadGameInfoFloat($"gameplayer{i}rating"),
                    handicap = ReadGameInfoFloat($"gameplayer{i}handicap"),
                    aipersonality = ReadGameInfoString($"gameplayer{i}aipersonality"),
                    avatarid = ReadGameInfoString($"gameplayer{i}avatarid"),
                    rank = ReadGameInfoString($"gameplayer{i}rank"),
                    powerrating = ReadGameInfoString($"gameplayer{i}powerrating"),
                    totalxp = ReadGameInfoString($"gameplayer{i}totalxp"),
                    winratio = ReadGameInfoString($"gameplayer{i}winratio"),
                    ready = ReadGameInfoBool($"gameplayer{i}ready"),
                    hclocation = ReadGameInfoInt32($"gameplayer{i}hclocation"),
                    hclevel = ReadGameInfoInt32($"gameplayer{i}hclevel"),
                    hcfilename = ReadGameInfoString($"gameplayer{i}hcfilename"),
                    homecityname = ReadGameInfoString($"gameplayer{i}homecityname"),
                    explorername = ReadGameInfoString($"gameplayer{i}explorername"),
                    questid = ReadGameInfoInt32($"gameplayer{i}questid"),
                    queststatus = ReadGameInfoInt32($"gameplayer{i}queststatus"),
                    id = ReadGameInfoInt32($"gameplayer{i}id"),
                    civishidden = ExeVersion >= 134624 ? ReadGameInfoBool($"gameplayer{i}civishidden") : false,
                    civwasrandom = ExeVersion >= 169326 ? ReadGameInfoBool($"gameplayer{i}civwasrandom") : false,
                    explorerskinid = ExeVersion >= 174943 ? ReadGameInfoInt32($"gameplayer{i}explorerskinid") : 0


                });
            }
        }

        private string ReadGameInfoString(string property)
        {
            uint bufLength;
            string bufString;
            uint bufType;
            bufLength = Reader.ReadUInt32();
            bufString = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));
            //Debug.WriteLine(bufString);
            Trace.Assert(bufString == property, $"Property is not equal. {bufString} != {property}");
            bufType = Reader.ReadUInt32();
            Trace.Assert(bufType == 9, $"Value type is not String, but it is {bufType}"); // 9 means value is string
            bufLength = Reader.ReadUInt32();
            bufString = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));
            //Debug.WriteLine(bufString);
            return bufString;
        }

        private int ReadGameInfoInt32(string property)
        {
            uint bufLength;
            string bufString;
            uint bufType;
            int bufValue;
            bufLength = Reader.ReadUInt32();
            bufString = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));
            //Debug.WriteLine(bufString);
            Trace.Assert(bufString == property, $"Property is not equal. {bufString} != {property}");
            bufType = Reader.ReadUInt32();
            Trace.Assert(bufType == 2, $"Value type is not UInt32, but it is {bufType}"); // 2 means value is UInt32
            bufValue = Reader.ReadInt32();
            //Debug.WriteLine(bufValue);
            return bufValue;
        }

        private bool ReadGameInfoBool(string property)
        {
            uint bufLength;
            string bufString;
            uint bufType;
            bool bufValue;
            bufLength = Reader.ReadUInt32();
            bufString = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));
            //Debug.WriteLine(bufString);
            Trace.Assert(bufString == property, $"Property is not equal. {bufString} != {property}");
            bufType = Reader.ReadUInt32();
            Trace.Assert(bufType == 5, $"Value type is not Bool, but it is {bufType}"); // 5 means value is bool
            bufValue = Reader.ReadBoolean();
            //Debug.WriteLine(bufValue);
            return bufValue;
        }

        private float ReadGameInfoFloat(string property)
        {
            uint bufLength;
            string bufString;
            uint bufType;
            float bufValue;
            bufLength = Reader.ReadUInt32();
            bufString = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));
            //Debug.WriteLine(bufString);
            Trace.Assert(bufString == property, $"Property is not equal. {bufString} != {property}");
            bufType = Reader.ReadUInt32();
            Trace.Assert(bufType == 1, $"Value type is not Double, but it is {bufType}"); // 1 means value is Double
            bufValue = Reader.ReadSingle();
            //Debug.WriteLine(bufValue);
            return bufValue;
        }

        private long Search(byte[] src, long start, byte[] pattern)
        {
            int maxFirstCharSlot = src.Length - pattern.Length + 1;
            if (start > maxFirstCharSlot) return -1;
            for (long i = start; i < maxFirstCharSlot; i++)
            {
                if (src[i] != pattern[0]) // compare only first byte
                    continue;

                // found a match on first byte, now try to match rest of the pattern
                for (int j = pattern.Length - 1; j >= 1; j--)
                {
                    if (src[i + j] != pattern[j]) break;
                    if (j == 1) return i;
                }
            }
            return -1;
        }

        public async Task<bool> Read(string path)
        {
             try
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "logs"));
                var json = await File.ReadAllTextAsync("data\\Decks.txt");
                DeckDatas = JsonConvert.DeserializeObject<List<DeckData>>(json);


                Versions.Add(new GameVersion() { ExeVersion = 132912, PatchVersion = 0, PatchNotes = "" });
                Versions.Add(new GameVersion() { ExeVersion = 132601, PatchVersion = 1529, PatchNotes = "https://www.ageofempires.com/news/aoe3de-baseline-update/" });
                Versions.Add(new GameVersion() { ExeVersion = 134624, PatchVersion = 3552, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-3552/" });
                Versions.Add(new GameVersion() { ExeVersion = 135159, PatchVersion = 4087, PatchNotes = "https://www.ageofempires.com/news/aoe3de-hotfix-4087/" });
                Versions.Add(new GameVersion() { ExeVersion = 136097, PatchVersion = 5025, PatchNotes = "https://steamcommunity.com/games/933110/announcements/detail/2956010782079627673" });
                Versions.Add(new GameVersion() { ExeVersion = 136280, PatchVersion = 5208, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-5208/" });
                Versions.Add(new GameVersion() { ExeVersion = 137231, PatchVersion = 6159, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-6159/" });
                Versions.Add(new GameVersion() { ExeVersion = 137919, PatchVersion = 6847, PatchNotes = "https://www.ageofempires.com/news/aoe3de-hotfix-6847/" });
                Versions.Add(new GameVersion() { ExeVersion = 140548, PatchVersion = 9476, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-9476/" });
                Versions.Add(new GameVersion() { ExeVersion = 142220, PatchVersion = 10087, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-10807/" });
                Versions.Add(new GameVersion() { ExeVersion = 144562, PatchVersion = 13088, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-13088/" });
                Versions.Add(new GameVersion() { ExeVersion = 145897, PatchVersion = 14825, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-14825/" });
                Versions.Add(new GameVersion() { ExeVersion = 149565, PatchVersion = 18493, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-18493/" });
                Versions.Add(new GameVersion() { ExeVersion = 151394, PatchVersion = 20322, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-20322/" });
                Versions.Add(new GameVersion() { ExeVersion = 152776, PatchVersion = 21704, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-20322#hotfix-21704" });
                Versions.Add(new GameVersion() { ExeVersion = 154583, PatchVersion = 23511, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-23511/" });
                Versions.Add(new GameVersion() { ExeVersion = 155704, PatchVersion = 24632, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-23511/#hotfix-24632" });
                Versions.Add(new GameVersion() { ExeVersion = 1, PatchVersion = 26865, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-26865/" });
                Versions.Add(new GameVersion() { ExeVersion = 158402, PatchVersion = 27330, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-26865/#hotfix-27330" });
                Versions.Add(new GameVersion() { ExeVersion = 158884, PatchVersion = 27812, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-26865/#hotfix-27812" });
                Versions.Add(new GameVersion() { ExeVersion = 160787, PatchVersion = 29715, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-29715/" });
                Versions.Add(new GameVersion() { ExeVersion = 161253, PatchVersion = 30181, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-29715#hotfix-30181" });
                Versions.Add(new GameVersion() { ExeVersion = 169326, PatchVersion = 38254, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-38254/" });
                Versions.Add(new GameVersion() { ExeVersion = 170418, PatchVersion = 39346, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-38254#hotfix-39346" });
                Versions.Add(new GameVersion() { ExeVersion = 174943, PatchVersion = 43871, PatchNotes = "https://www.ageofempires.com/news/aoeiiide-update-43871/" });
                Versions.Add(new GameVersion() { ExeVersion = 178653, PatchVersion = 47581, PatchNotes = "https://www.ageofempires.com/news/aoeiii-de-update-47581/" });
                Versions.Add(new GameVersion() { ExeVersion = 181902, PatchVersion = 50830, PatchNotes = "https://www.ageofempires.com/news/aoeiii-de-update-50830/" });
                Versions.Add(new GameVersion() { ExeVersion = 185617, PatchVersion = 54545, PatchNotes = "https://www.ageofempires.com/news/aoeiii-definitive-edition-update-54545/" });
                Versions.Add(new GameVersion() { ExeVersion = 187932, PatchVersion = 56860, PatchNotes = "https://www.ageofempires.com/news/aoe-iii-de-update-56860/" });
                Versions.Add(new GameVersion() { ExeVersion = 192285, PatchVersion = 61213, PatchNotes = "https://www.ageofempires.com/news/age-of-empires-iii-definitive-edition-update-61213/" });
                Versions.Add(new GameVersion() { ExeVersion = 194071, PatchVersion = 62999, PatchNotes = "https://www.ageofempires.com/news/age-of-empires-iii-definitive-edition-update-61213/#Hotfix" });
                Versions.Add(new GameVersion() { ExeVersion = 194799, PatchVersion = 63727, PatchNotes = "https://www.ageofempires.com/news/age-of-empires-iii-definitive-edition-update-61213/#Hotfix-63727" });

                var data = await File.ReadAllBytesAsync(path);
                // decoding l33t
                if (L33TZipUtils.IsL33TZipFile(data))
                    data = await L33TZipUtils.ExtractL33TZippedBytesAsync(data);
                await File.WriteAllBytesAsync("logs\\uncompressed_record", data);
                using (var stream = new MemoryStream(data))
                {
                    Reader = new BinaryReader(stream);
                    MagicNumber = Reader.ReadByte();

                    if (MagicNumber != 4)
                    {
                        throw new Exception("File is not Age of Empires 3 record file");
                    }

                    Reader.ReadBytes(16);

                    IsMultiplayer = Reader.ReadByte();

                    if (IsMultiplayer != 1)
                    {
                        throw new Exception("Record is not multiplayer");
                    }

                    Reader.ReadBytes(12);

                    ViewPoint = Reader.ReadByte();

                    Reader.ReadBytes(242);

                    uint bufLength;
                    string bufString;

                    bufLength = Reader.ReadUInt32();
                    ExeInfo = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));
                    Reader.ReadBytes(8);
                    Reader.ReadBytes((int)(Reader.ReadUInt32() * 2));


                    bufLength = Reader.ReadUInt32();
                    bufString = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));
                    if (ExeVersion < 136280)
                    {
                        Reader.ReadBytes(36);
                    }
                    else if (ExeVersion < 154583)
                    {
                        Reader.ReadBytes(40);
                    }
                    else
                    {
                        Reader.ReadBytes(45);
                    }



                    GameTitle = ReadGameInfoString("gamename");
                    ReadGameInfoInt32("gametype");
                    ReadGameInfoBool("gamerestored");
                    ReadGameInfoInt32("gamecurplayers");
                    GameNumPlayers = ReadGameInfoInt32("gamenumplayers");
                    GameFFA = ReadGameInfoBool("gamefreeforall");
                    GameNomadStart = ReadGameInfoBool("gamenomadstart");
                    GameMapSize = ReadGameInfoInt32("gamemapsize");
                    ReadGameInfoString("gamemapname");


                    ReadGameInfoInt32("gamemaptype");

                    GameFileName = ReadGameInfoString("gamefilename");
                    ReadGameInfoString("gamefilenameext");
                    ReadGameInfoInt32("gamefilecrc");
                    ReadGameInfoBool("gamerestrictpause");
                    ReadGameInfoInt32("gamemapresources");
                    ReadGameInfoBool("gameteamshareres");
                    ReadGameInfoBool("gameteamsharepop");
                    ReadGameInfoBool("gameteamlock");
                    ReadGameInfoBool("gameteambalanced");
                    ReadGameInfoInt32("gamerandomseed");
                    ReadGameInfoBool("gamermdebug");
                    GameStartWithTreaty = ReadGameInfoBool("gamestartwithtreaty");
                    GameDifficulty = ReadGameInfoInt32("gamedifficulty");
                    GameRecordGame = ReadGameInfoBool("gamerecordgame");
                    GameStartingAge = ReadGameInfoInt32("gamestartingage");
                    if (ExeVersion >= 144562)
                    {
                        GameEndingAge = ReadGameInfoInt32("gameendingage");
                    }

                    GameSpeed = ReadGameInfoInt32("gamespeed");
                    ReadGameInfoInt32("gamemapvisibility");
                    GameModeType = ReadGameInfoInt32("gamemodetype");
                    GameHandicapMode = ReadGameInfoInt32("gamehandicapmode");
                    GameAllowCheats = ReadGameInfoBool("gameallowcheats");
                    ReadGameInfoInt32("gamehosttime");
                    GamePassword = ReadGameInfoString("gamepassword");
                    ReadGameInfoInt32("gamelatency");


                    ReadGameInfoInt32("homecitylevelmin");
                    ReadGameInfoInt32("homecitylevelmax");
                    ReadGameInfoString("custommapname");
                    GameNoRush = ReadGameInfoInt32("gamenorush");
                    GameTradeMonopoly = ReadGameInfoBool("gametrademonopoly");
                    GameBlockade = ReadGameInfoBool("gameblockade");
                    GameKOTH = ReadGameInfoBool("gamekoth");
                    ReadGameInfoInt32("gamescenariogameid");
                    ReadGameInfoInt32("gamecontentid");

                    ReadGameInfoBool("gameismpscenario");
                    if (ExeVersion >= 185617)
                    {
                        ReadGameInfoBool("gameismpcoop");
                    }
                    ReadGameInfoString("gamegcgameid");
                    ReadGameInfoInt32("gamegcgametype");
                    ReadGameInfoInt32("gamegcnumturns");
                    ReadGameInfoBool("gamehiddencards");
                    ReadGameInfoBool("gamepickcardsfirst");
                    ReadGameInfoInt32("gamecampaignselected");
                    ReadGameInfoInt32("gamecampaignprogress");
                    ReadGameInfoInt32("gamecampaignfarthest");
                    ReadGameInfoInt32("gamecampaignprogress1");
                    ReadGameInfoInt32("gamecampaignfarthest1");
                    ReadGameInfoInt32("gamecampaignprogress2");

                    ReadGameInfoInt32("gamecampaignfarthest2act1");
                    ReadGameInfoInt32("gamecampaignfarthest2act2");
                    ReadGameInfoInt32("gamecampaignfarthest2act3");

                    ReadGameInfoBool("gamecampaignshownhcnote");
                    ReadGameInfoBool("gamecampaignshownhcnote2");
                    ReadGameInfoBool("gamecampaignshownhcnote3");

                    ReadGamePlayers();



                    NotifyPropertyChanged("Players");
                    NotifyPropertyChanged("RecordOwner");

                    File.WriteAllText("logs\\Players.txt", JsonConvert.SerializeObject(Players, Formatting.Indented));

                    ReadGameInfoString("gameguid");
                    ReadGameInfoString("gamecontinuemainfilename");
                    ReadGameInfoString("gamecontinuecampaignfilename");
                    ReadGameInfoString("gamecontinuecampaignscenarioname");
                    ReadGameInfoInt32("gamecontinuecampaignscenarionameid");
                    ReadGameInfoInt32("gamecontinuecampaignid");
                    ReadGameInfoInt32("gamecontinuecampaignscenarioid");
                    ReadGameInfoString("gameregion");
                    ReadGameInfoString("gamelanguage");

                    if (ExeVersion >= 134624)
                    {
                        ReadGameInfoInt32("gamecustommapfilecount");
                    }
                    if (ExeVersion >= 144562)
                    {
                        GameStartingResources = ReadGameInfoInt32("gamestartingresources");
                    }
                    if (ExeVersion >= 169326)
                    {
                        ReadGameInfoInt32("mapsetfilter");
                    }
                    if (ExeVersion >= 192285)
                    {
                        ReadGameInfoInt32("mapmodid");
                        ReadGameInfoInt32("mapmodcrc");
                        ReadGameInfoInt32("gamempcoopcampaignid");
                        ReadGameInfoInt32("gamempcoopscenarioid");
                    }


                    var bufOffset = Search(data, 0, new byte[] { 0x44, 0x6b });

               
                    var currentPlayerId = 0;
                    while (bufOffset != -1)
                    {
                        bufOffset -= 4;
                        int currentDeckId = 1;
                        Reader.BaseStream.Seek(bufOffset, SeekOrigin.Begin);
                        while (true)
                        {
                            var deckCount = Reader.ReadUInt32();
                            if (deckCount == 0)
                            {
                                if (Reader.ReadUInt32() == 0)
                                {
                                    break;
                                }
                                else
                                {
                                    deckCount = 1;
                                    Reader.BaseStream.Seek(Reader.BaseStream.Position - 4, SeekOrigin.Begin);
                                }
                            }
                            var leaveLoop = false;

                            for (int i = 0; i < deckCount; i++)
                            {
                                if (Reader.ReadByte() != 0x44 || Reader.ReadByte() != 0x6b)
                                {
                                    leaveLoop = true;
                                    break;
                                }
                                long nextDeckOffset = Reader.ReadUInt32();
                                nextDeckOffset += Reader.BaseStream.Position;

                                if (Reader.ReadUInt32() != (ExeVersion > 151394 ? 5 : 4))
                                {
                                    leaveLoop = true;
                                    break;
                                }
                                var deckId = Reader.ReadInt32();
                                bufLength = Reader.ReadUInt32();
                                var deckName = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));

                                var gameId = Reader.ReadInt32();

                                var isDefault = Reader.ReadBoolean();

                                if (ExeVersion > 151394)

                                    Reader.ReadBoolean();

                                var techIds = new List<int>();
                                var cardCount = Reader.ReadUInt32();
                                for (uint j = 0; j < cardCount; j++)
                                {
                                    techIds.Add(Reader.ReadInt32());
                                }

                                Decks.Add(new PlayerDeck() { CurrentDeckId = currentDeckId, Name = deckName, Id = deckId, GameId = gameId, IsDefault = isDefault, TechIds = techIds });

                                Reader.BaseStream.Seek(nextDeckOffset, SeekOrigin.Begin);


                                
                            }
                            if (leaveLoop)
                            {
                                break;
                            }
                            currentDeckId += 1;

                        }

                        bufOffset = Search(data, Reader.BaseStream.Position, new byte[] { 0x44, 0x6b });
                    }

                    await File.WriteAllTextAsync("logs\\Decks.txt", JsonConvert.SerializeObject(Decks, Formatting.Indented));


                    foreach (var deck in Decks)
                    {
                        if (deck.CurrentDeckId == 1 && deck.Id == 0)
                        {
                            currentPlayerId += 1;
                        }
                        if (deck.CurrentDeckId == 1 && deck.Id >= 0)
                        {
                            if (Players.ElementAt(currentPlayerId).GameCiv != null)
                            {
                                var deckData = DeckDatas.FirstOrDefault(x => x.CivID == Players.ElementAt(currentPlayerId).GameCiv.id);
                                if (deckData != null)
                                {
                                    ObservableCollection<CardData> FilledDeck = new ObservableCollection<CardData>();
                                    foreach (var card in deck.TechIds)
                                    {
                                        var FilledCard = deckData.Cards.FirstOrDefault(x => x.CardID == card);
                                        if (FilledCard != null)
                                        {
                                            FilledDeck.Add(FilledCard);
                                        }

                                    }

                                    ObservableCollection<CardData> SortedDeck = new ObservableCollection<CardData>();
                                    foreach (var tech in deckData.Cards)
                                    {
                                        foreach (var card in FilledDeck)
                                        {
                                            if (tech.CardName == card.CardName)
                                            {
                                                SortedDeck.Add(card);
                                                break;
                                            }
                                        }
                                    }


                                    deck.Age1Cards = new ObservableCollection<CardData>(SortedDeck.Where(x => x.Age == 0));
                                    deck.Age2Cards = new ObservableCollection<CardData>(SortedDeck.Where(x => x.Age == 1));
                                    deck.Age3Cards = new ObservableCollection<CardData>(SortedDeck.Where(x => x.Age == 2));
                                    deck.Age4Cards = new ObservableCollection<CardData>(SortedDeck.Where(x => x.Age == 3));

                                    Players.ElementAt(currentPlayerId).Decks.Add(deck);

                                }
                            }
                            else
                            {
                                Players.ElementAt(currentPlayerId).Decks.Add(deck);
                            }
                        }
                    }

                
                    Players.RemoveAt(0);
                    for (int i = GameNumPlayers; i < 12; i++)
                    {
                        Players.RemoveAt(GameNumPlayers);
                    }
                
                    bufOffset = Search(data, 0, new byte[] { 0x9a, 0x99, 0x99, 0x3d });

                }


                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
