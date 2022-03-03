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
using System.Windows.Data;

namespace ReplayManager.Classes.Records
{
    public class age3rec : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string recordPath;
        public string RecordPath
        {
            get
            {
                return recordPath;
            }
            set
            {
                recordPath = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("RecordPathFileName");
            }
        }

        public string RecordPathFileName
        {
            get
            {
                return Path.GetFileName(RecordPath);
            }
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

        private TimeSpan gameDuration;
        public TimeSpan GameDuration
        {
            get
            {
                return gameDuration;
            }
            set
            {
                gameDuration = value;
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
                NotifyPropertyChanged("ShortGameModeType");
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
                NotifyPropertyChanged("ShortGameModeType");
            }
        }

        public string RenamedRecord
        {
            get
            {
                if (GameNumPlayers == 2)
                {
                    string name =  $"[{ShortGameModeType}] {Players[0].name}[{Players[0].GameCiv.short_name}] vs {Players[1].name}[{Players[1].GameCiv.short_name}] - {GameFileName}.age3yrec";
                    return string.Join("_", name.Split(Path.GetInvalidFileNameChars()));
                }
                else
                {
                    string name = $"[{ShortGameModeType}] ";
                    foreach (GamePlayer player in Players)
                    {
                        name+= $"{player.name}[{player.GameCiv.short_name}], ";

                    }
                    name = name.Remove(name.Length - 2);
                    name += $" - {GameFileName}.age3yrec";
                    return string.Join("_", name.Split(Path.GetInvalidFileNameChars()));
                }
            }
        }

        public string ShortGameModeType
        {
            get
            {
                var mode = "UNK";
                switch (gameModeType)
                {
                    case 0: mode = "SP"; break;
                    case 1: mode = "DM"; break;
                    case 2: mode = "EW"; break;
                    case 3: mode = "TR"; break;
                    case 4: mode = "DM"; break;
                    case 5: mode = "CB"; break;
                    case 6: mode = "SC"; break;
                    case 7: mode = "SG"; break;
                }
                if (GameNoRush > 0)
                    mode += GameNoRush.ToString();
                return mode;
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
                    else
                    {
                        mode = mode + $" (No Rush {GameNoRush} min.)";
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

        private IReadOnlyCollection<GameAction> Actions { get; set; }
        private List<GameVersion> Versions { get; set; } = new List<GameVersion>();


        private CollectionViewSource actionsCollection;
        public ICollectionView ActionsCollection
        {
            get
            {
                return actionsCollection.View;
            }
        }


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
            int bufLength;
            string bufString;
            int bufType;
            bufLength = Reader.ReadInt32();
            bufString = Encoding.Unicode.GetString(Reader.ReadBytes(bufLength * 2));
            Trace.Assert(bufString == property, $"Property is not equal. {bufString} != {property}");
            bufType = Reader.ReadInt32();
            Trace.Assert(bufType == 9, $"Value type is not String, but it is {bufType}"); // 9 means value is string
            bufLength = Reader.ReadInt32();
            bufString = Encoding.Unicode.GetString(Reader.ReadBytes(bufLength * 2));
            return bufString;
        }

        private int ReadGameInfoInt32(string property)
        {
            int bufLength;
            string bufString;
            int bufType;
            int bufValue;
            bufLength = Reader.ReadInt32();
            bufString = Encoding.Unicode.GetString(Reader.ReadBytes(bufLength * 2));
            Trace.Assert(bufString == property, $"Property is not equal. {bufString} != {property}");
            bufType = Reader.ReadInt32();
            Trace.Assert(bufType == 2, $"Value type is not Int32, but it is {bufType}"); // 2 means value is UInt32
            bufValue = Reader.ReadInt32();
            return bufValue;
        }

        private bool ReadGameInfoBool(string property)
        {
            int bufLength;
            string bufString;
            int bufType;
            bool bufValue;
            bufLength = Reader.ReadInt32();
            bufString = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));
            Trace.Assert(bufString == property, $"Property is not equal. {bufString} != {property}");
            bufType = Reader.ReadInt32();
            Trace.Assert(bufType == 5, $"Value type is not Bool, but it is {bufType}"); // 5 means value is bool
            bufValue = Reader.ReadBoolean();
            return bufValue;
        }

        private float ReadGameInfoFloat(string property)
        {
            int bufLength;
            string bufString;
            int bufType;
            float bufValue;
            bufLength = Reader.ReadInt32();
            bufString = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));
            Trace.Assert(bufString == property, $"Property is not equal. {bufString} != {property}");
            bufType = Reader.ReadInt32();
            Trace.Assert(bufType == 1, $"Value type is not Double, but it is {bufType}"); // 1 means value is Double
            bufValue = Reader.ReadSingle();
            return bufValue;
        }

        private int Search(byte[] src, int start, byte[] pattern)
        {
            int maxFirstCharSlot = src.Length - pattern.Length + 1;
            if (start > maxFirstCharSlot) return -1;
            for (int i = start; i < maxFirstCharSlot; i++)
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


        private void ExpectedValue(int value1, int value2)
        {
            if (value1 != value2)
            {
                throw new Exception($"Offset: {Reader.BaseStream.Position}\nInvalid value: {value1}, expected: {value2}");
            }
        }

        private void ExpectedInt32(int value)
        {
            ExpectedValue(Reader.ReadInt32(), value);
        }

        private void ExpectedByte(int value)
        {
            ExpectedValue(Reader.ReadByte(), value);
        }

        private bool filterBuild = false;
        public bool FilterBuild
        {
            get
            {
                return filterBuild;
            }
            set
            {
                filterBuild = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterTrain = false;
        public bool FilterTrain
        {
            get
            {
                return filterTrain;
            }
            set
            {
                filterTrain = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterResearch = true;
        public bool FilterResearch
        {
            get
            {
                return filterResearch;
            }
            set
            {
                filterResearch = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }


        private bool filterResign = true;
        public bool FilterResign
        {
            get
            {
                return filterResign;
            }
            set
            {
                filterResign = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterDeck = true;
        public bool FilterDeck
        {
            get
            {
                return filterDeck;
            }
            set
            {
                filterDeck = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterShipment = true;
        public bool FilterShipment
        {
            get
            {
                return filterShipment;
            }
            set
            {
                filterShipment = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterCheat = true;
        public bool FilterCheat
        {
            get
            {
                return filterCheat;
            }
            set
            {
                filterCheat = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterSpawn = true;
        public bool FilterSpawn
        {
            get
            {
                return filterSpawn;
            }
            set
            {

                filterSpawn = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterChat = true;
        public bool FilterChat
        {
            get
            {
                return filterChat;
            }
            set
            {
                filterChat = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor0 = true;
        public bool FilterColor0
        {
            get
            {
                return filterColor0;
            }
            set
            {
                filterColor0 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor1 = true;
        public bool FilterColor1
        {
            get
            {
                return filterColor1;
            }
            set
            {
                filterColor1 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor2 = true;
        public bool FilterColor2
        {
            get
            {
                return filterColor2;
            }
            set
            {
                filterColor2 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor3 = true;
        public bool FilterColor3
        {
            get
            {
                return filterColor3;
            }
            set
            {
                filterColor3 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor4 = true;
        public bool FilterColor4
        {
            get
            {
                return filterColor4;
            }
            set
            {
                filterColor4 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor5 = true;
        public bool FilterColor5
        {
            get
            {
                return filterColor5;
            }
            set
            {
                filterColor5 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor6 = true;
        public bool FilterColor6
        {
            get
            {
                return filterColor6;
            }
            set
            {
                filterColor6 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor7 = true;
        public bool FilterColor7
        {
            get
            {
                return filterColor7;
            }
            set
            {
                filterColor7 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor8 = true;
        public bool FilterColor8
        {
            get
            {
                return filterColor8;
            }
            set
            {
                filterColor8 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor9 = true;
        public bool FilterColor9
        {
            get
            {
                return filterColor9;
            }
            set
            {
                filterColor9 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor10 = true;
        public bool FilterColor10
        {
            get
            {
                return filterColor10;
            }
            set
            {
                filterColor10 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor11 = true;
        public bool FilterColor11
        {
            get
            {
                return filterColor11;
            }
            set
            {
                filterColor11 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        private bool filterColor12 = true;
        public bool FilterColor12
        {
            get
            {
                return filterColor12;
            }
            set
            {
                filterColor12 = value;
                NotifyPropertyChanged();
                actionsCollection.View.Refresh();
            }
        }

        void Filter(object sender, FilterEventArgs e)
        {
            var action = e.Item as GameAction;

            if (action.Type != "chat" && action.Type != "research_tech1" && action.Type != "research_tech2"
                && action.Type != "research_tech3" && action.Type != "train" && action.Type != "shipment"
                && action.Type != "build" && action.Type != "resigned" && action.Type != "spawn_unit"
                && action.Type != "pick_deck")
            {
                e.Accepted = false;
                return;
            }

            bool color = action.Player.color == 0 && FilterColor0 ||
                        action.Player.color == 1 && FilterColor1 ||
                        action.Player.color == 2 && FilterColor2 ||
                        action.Player.color == 3 && FilterColor3 ||
                        action.Player.color == 4 && FilterColor4 ||
                        action.Player.color == 5 && FilterColor5 ||
                        action.Player.color == 6 && FilterColor6 ||
                        action.Player.color == 7 && FilterColor7 ||
                        action.Player.color == 8 && FilterColor8 ||
                        action.Player.color == 9 && FilterColor9 ||
                        action.Player.color == 10 && FilterColor10 ||
                        action.Player.color == 11 && FilterColor11 ||
                        action.Player.color == 12 && FilterColor12;



            e.Accepted = action.Type == "build" && FilterBuild && color ||
                         action.Type == "research_tech1" && FilterResearch && color ||
                         action.Type == "pick_deck" && FilterDeck && color ||
                         action.Type == "shipment" && FilterShipment && color ||
                         (action.Type == "research_tech2" || action.Type == "research_tech3") && FilterCheat && color ||
                         action.Type == "spawn_unit" && FilterSpawn && color ||
                         action.Type == "resigned" && FilterResign && color ||
                         action.Type == "train" && FilterTrain && color ||
                         action.Type == "chat" && FilterChat && color;
        }


        public async Task<bool> Read(string path)
        {
            try
            {
                List<DeckData> DeckDatas;

                List<TechData> TechDatas;
                List<ProtoData> UnitDatas;
                RecordPath = path;
                var json = await File.ReadAllTextAsync("data\\Decks.txt");
                DeckDatas = JsonConvert.DeserializeObject<List<DeckData>>(json);
                json = await File.ReadAllTextAsync("data\\Techs.txt");
                TechDatas = JsonConvert.DeserializeObject<List<TechData>>(json);
                json = await File.ReadAllTextAsync("data\\Units.txt");
                UnitDatas = JsonConvert.DeserializeObject<List<ProtoData>>(json);

                Versions.Add(new GameVersion() { ExeVersion = 132912, PatchVersion = 0, PatchNotes = "", ReleaseDate = new DateOnly(2020, 10, 13) });
                Versions.Add(new GameVersion() { ExeVersion = 132601, PatchVersion = 1529, PatchNotes = "https://steamcommunity.com/games/933110/announcements/detail/5409345509730843359", ReleaseDate = new DateOnly(2020, 10, 16) });
                Versions.Add(new GameVersion() { ExeVersion = 134624, PatchVersion = 3552, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-3552/", ReleaseDate = new DateOnly(2020, 10, 22) });
                Versions.Add(new GameVersion() { ExeVersion = 135159, PatchVersion = 4087, PatchNotes = "https://www.ageofempires.com/news/aoe3de-hotfix-4087/", ReleaseDate = new DateOnly(2020, 10, 27) });
                Versions.Add(new GameVersion() { ExeVersion = 136097, PatchVersion = 5025, PatchNotes = "https://steamcommunity.com/games/933110/announcements/detail/2956010782079627673", ReleaseDate = new DateOnly(2020, 10, 30) });
                Versions.Add(new GameVersion() { ExeVersion = 136280, PatchVersion = 5208, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-5208/", ReleaseDate = new DateOnly(2020, 11, 5) });
                Versions.Add(new GameVersion() { ExeVersion = 137231, PatchVersion = 6159, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-6159/", ReleaseDate = new DateOnly(2020, 11, 12) });
                Versions.Add(new GameVersion() { ExeVersion = 137919, PatchVersion = 6847, PatchNotes = "https://www.ageofempires.com/news/aoe3de-hotfix-6847/", ReleaseDate = new DateOnly(2020, 11, 18) });
                Versions.Add(new GameVersion() { ExeVersion = 140548, PatchVersion = 9476, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-9476/", ReleaseDate = new DateOnly(2020, 12, 8) });
                Versions.Add(new GameVersion() { ExeVersion = 142220, PatchVersion = 10087, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-10807/", ReleaseDate = new DateOnly(2020, 12, 17) });
                Versions.Add(new GameVersion() { ExeVersion = 144562, PatchVersion = 13088, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-13088/", ReleaseDate = new DateOnly(2021, 1, 19) });
                Versions.Add(new GameVersion() { ExeVersion = 145897, PatchVersion = 14825, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-14825/", ReleaseDate = new DateOnly(2021, 2, 2) });
                Versions.Add(new GameVersion() { ExeVersion = 149565, PatchVersion = 18493, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-18493/", ReleaseDate = new DateOnly(2021, 2, 25) });
                Versions.Add(new GameVersion() { ExeVersion = 151394, PatchVersion = 20322, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-20322/", ReleaseDate = new DateOnly(2021, 3, 16) });
                Versions.Add(new GameVersion() { ExeVersion = 152776, PatchVersion = 21704, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-20322#hotfix-21704", ReleaseDate = new DateOnly(2021, 3, 25) });
                Versions.Add(new GameVersion() { ExeVersion = 154583, PatchVersion = 23511, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-23511/", ReleaseDate = new DateOnly(2021, 4, 13) });
                Versions.Add(new GameVersion() { ExeVersion = 155704, PatchVersion = 24632, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-23511/#hotfix-24632", ReleaseDate = new DateOnly(2021, 4, 22) });
                Versions.Add(new GameVersion() { ExeVersion = 1, PatchVersion = 26865, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-26865/", ReleaseDate = new DateOnly(2021, 5, 11) });
                Versions.Add(new GameVersion() { ExeVersion = 158402, PatchVersion = 27330, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-26865/#hotfix-27330", ReleaseDate = new DateOnly(2021, 5, 13) });
                Versions.Add(new GameVersion() { ExeVersion = 158884, PatchVersion = 27812, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-26865/#hotfix-27812", ReleaseDate = new DateOnly(2021, 5, 19) });
                Versions.Add(new GameVersion() { ExeVersion = 160787, PatchVersion = 29715, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-29715/", ReleaseDate = new DateOnly(2021, 6, 8) });
                Versions.Add(new GameVersion() { ExeVersion = 161253, PatchVersion = 30181, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-29715#hotfix-30181", ReleaseDate = new DateOnly(2021, 6, 8) });
                Versions.Add(new GameVersion() { ExeVersion = 169326, PatchVersion = 38254, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-38254/", ReleaseDate = new DateOnly(2021, 7, 29) });
                Versions.Add(new GameVersion() { ExeVersion = 170418, PatchVersion = 39346, PatchNotes = "https://www.ageofempires.com/news/aoe3de-update-38254#hotfix-39346", ReleaseDate = new DateOnly(2021, 8, 9) });
                Versions.Add(new GameVersion() { ExeVersion = 174943, PatchVersion = 43871, PatchNotes = "https://www.ageofempires.com/news/aoeiiide-update-43871/", ReleaseDate = new DateOnly(2021, 9, 14) });
                Versions.Add(new GameVersion() { ExeVersion = 178653, PatchVersion = 47581, PatchNotes = "https://www.ageofempires.com/news/aoeiii-de-update-47581/", ReleaseDate = new DateOnly(2021, 10, 12) });
                Versions.Add(new GameVersion() { ExeVersion = 181902, PatchVersion = 50830, PatchNotes = "https://www.ageofempires.com/news/aoeiii-de-update-50830/", ReleaseDate = new DateOnly(2021, 11, 9) });
                Versions.Add(new GameVersion() { ExeVersion = 185617, PatchVersion = 54545, PatchNotes = "https://www.ageofempires.com/news/aoeiii-definitive-edition-update-54545/", ReleaseDate = new DateOnly(2021, 11, 30) });
                Versions.Add(new GameVersion() { ExeVersion = 187932, PatchVersion = 56860, PatchNotes = "https://www.ageofempires.com/news/aoe-iii-de-update-56860/", ReleaseDate = new DateOnly(2021, 12, 16) });
                Versions.Add(new GameVersion() { ExeVersion = 192285, PatchVersion = 61213, PatchNotes = "https://www.ageofempires.com/news/age-of-empires-iii-definitive-edition-update-61213/", ReleaseDate = new DateOnly(2022, 2, 1) });
                Versions.Add(new GameVersion() { ExeVersion = 194071, PatchVersion = 62999, PatchNotes = "https://www.ageofempires.com/news/age-of-empires-iii-definitive-edition-update-61213/#Hotfix", ReleaseDate = new DateOnly(2022, 2, 14) });
                Versions.Add(new GameVersion() { ExeVersion = 194799, PatchVersion = 63727, PatchNotes = "https://www.ageofempires.com/news/age-of-empires-iii-definitive-edition-update-61213/#Hotfix-63727", ReleaseDate = new DateOnly(2022, 2, 18) });



                var data = await File.ReadAllBytesAsync(path);
                // decoding l33t
                if (L33TZipUtils.IsL33TZipFile(data))
                    data = await L33TZipUtils.ExtractL33TZippedBytesAsync(data);
                //await File.WriteAllBytesAsync("uncompressed_record", data);
                using var stream = new MemoryStream(data);

                Reader = new BinaryReader(stream);
                MagicNumber = Reader.ReadByte();

                if (MagicNumber != 4)
                {
                    throw new Exception("File is not Age of Empires 3: DE record file");
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

                int bufLength;
                string bufString;

                bufLength = Reader.ReadInt32();
                ExeInfo = Encoding.Unicode.GetString(Reader.ReadBytes(bufLength * 2));
                Reader.ReadBytes(8);
                Reader.ReadBytes(Reader.ReadInt32() * 2);

                //Debug.WriteLine($"Game {ExeVersion}");
                bufLength = Reader.ReadInt32();
                bufString = Encoding.Unicode.GetString(Reader.ReadBytes(bufLength * 2));
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
                await Task.Run(() =>
                {
                    while (bufOffset != -1)
                    {
                        bufOffset -= 4;
                        int currentDeckId = 1;
                        Reader.BaseStream.Seek(bufOffset, SeekOrigin.Begin);
                        while (true)
                        {
                            var deckCount = Reader.ReadInt32();
                            if (deckCount == 0)
                            {
                                if (Reader.ReadInt32() == 0)
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
                                long nextDeckOffset = Reader.ReadInt32();
                                nextDeckOffset += Reader.BaseStream.Position;

                                if (Reader.ReadInt32() != (ExeVersion > 152776 ? 5 : 4))
                                {
                                    leaveLoop = true;
                                    break;
                                }
                                var deckId = Reader.ReadInt32();
                                bufLength = Reader.ReadInt32();
                                if (bufLength < 0)
                                {
                                    leaveLoop = true;
                                    break;
                                }
                                var deckName = Encoding.Unicode.GetString(Reader.ReadBytes(bufLength * 2));
                                var gameId = Reader.ReadInt32();
                                var isDefault = Reader.ReadBoolean();

                                if (ExeVersion > 152776)

                                    Reader.ReadBoolean();

                                var techIds = new List<int>();
                                var cardCount = Reader.ReadInt32();
                                for (int j = 0; j < cardCount; j++)
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

                        bufOffset = Search(data, (int)Reader.BaseStream.Position, new byte[] { 0x44, 0x6b });
                    }
                });
                foreach (var deck in Decks)
                {
                    if (deck.CurrentDeckId == 1 && deck.Id == 0)
                    {
                        currentPlayerId += 1;
                    }
                    if (deck.CurrentDeckId == 1 && deck.Id >= 0)
                    {
                        if (Players[currentPlayerId].GameCiv != null)
                        {
                            var deckData = DeckDatas.FirstOrDefault(x => x.CivID == Players[currentPlayerId].GameCiv.id);
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

                                deck.SortedCards = new ObservableCollection<CardData>(SortedDeck.OrderBy(x => x.Age));
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



                List<GameAction> actions = new List<GameAction>();
                actionsCollection = new CollectionViewSource();


                var buf = 0;

                Players[0].name = "Mother Nature";
                bufOffset = Search(data, 0, new byte[] { 0x9a, 0x99, 0x99, 0x3d });
                if (bufOffset == -1)
                {
                    throw new Exception("Couldnt find players actions");
                }
                Reader.BaseStream.Seek(bufOffset + 4, SeekOrigin.Begin);
                Reader.ReadBytes(138);
                buf = Reader.ReadInt32();
                for (int i = 0; i < buf; i++)
                {
                    var sender = Reader.ReadInt32(); // from
                    var reciever = Reader.ReadInt32(); // to
                    bufLength = Reader.ReadInt32();
                    var msg = Encoding.Unicode.GetString(Reader.ReadBytes(bufLength * 2));
                    actions.Add(new GameAction() { Player = Players[sender], Duration = 0, Type = "chat", Message = $"sending message to {Players[reciever].name}: {msg}" });


                }
                long duration = 0;
                duration += Reader.ReadByte();
                int selectedCount = Reader.ReadByte();

                List<int> selectedIds = new List<int>();
                for (int i = 0; i <= selectedCount; i++)
                {
                    selectedIds.Add(Reader.ReadInt32());
                }

                ExpectedByte(0);


                bufOffset = Search(data, 0, new byte[] { 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x19 });
                if (bufOffset == -1)
                {
                    throw new Exception("Couldnt find players actions");
                }
                await Task.Run(() =>
                {
                    while (bufOffset != -1)
                    {
                        Reader.BaseStream.Seek(bufOffset, SeekOrigin.Begin);
                        Reader.ReadBytes(113);
                        int mainCommand;
                        try
                        {
                            mainCommand = Reader.ReadByte();
                        }
                        catch
                        {
                            break;
                        }
                        //Debug.WriteLine($"command: {mainCommand}, offset: {Reader.BaseStream.Position}");
                        if (mainCommand == 0)
                        {
                            throw new Exception("mainCommand cannot be 0");
                        }

                        var hasCommands = true;
                        var selectObjects = false;
                        switch (mainCommand)
                        {
                            case 33:
                            case 65:
                                break;
                            case 1:
                                hasCommands = false;
                                break;
                            case 161:
                            case 193:
                                selectObjects = true;
                                break;
                            case 129:
                                hasCommands = false;
                                selectObjects = true;
                                break;
                            case 35:
                            case 37:
                            case 41:
                            case 67:
                            case 73:
                                Reader.ReadBytes(4);
                                break;
                            case 3:
                            case 5:
                            case 9:
                                Reader.ReadBytes(4);
                                hasCommands = false;
                                break;

                            case 163:
                            case 165:
                            case 169:
                            case 195:
                            case 201:

                                Reader.ReadBytes(4);
                                selectObjects = true;
                                break;
                            case 131:
                            case 133:
                            case 137:
                                Reader.ReadBytes(4);
                                hasCommands = false;
                                selectObjects = true;
                                break;
                            case 39:
                            case 43:
                            case 45:
                            case 75:
                                Reader.ReadBytes(8);
                                break;

                            case 7:
                            case 11:
                            case 13:
                                Reader.ReadBytes(8);
                                hasCommands = false;
                                break;
                            case 167:
                            case 171:
                            case 173:
                            case 203:
                                Reader.ReadBytes(8);
                                selectObjects = true;
                                break;
                            case 135:
                            case 139:
                            case 141:
                                Reader.ReadBytes(8);
                                hasCommands = false;
                                selectObjects = true;
                                break;
                            case 47:
                                Reader.ReadBytes(12);
                                break;
                            case 15:
                                Reader.ReadBytes(12);
                                hasCommands = false;
                                break;
                            case 175:
                            case 207:
                                Reader.ReadBytes(12);
                                selectObjects = true;
                                break;
                            case 143:
                                Reader.ReadBytes(12);
                                hasCommands = false;
                                selectObjects = true;
                                break;
                            case 49:
                                Reader.ReadBytes(36);
                                break;
                            case 17:
                                Reader.ReadBytes(36);
                                hasCommands = false;
                                break;
                            case 177:
                                Reader.ReadBytes(36);
                                selectObjects = true;
                                break;
                            case 145:
                                Reader.ReadBytes(36);
                                hasCommands = false;
                                selectObjects = true;
                                break;
                            case 19:
                            case 21:
                            case 25:
                                Reader.ReadBytes(40);
                                hasCommands = false;
                                break;
                            case 51:
                            case 53:
                            case 57:
                                Reader.ReadBytes(40);
                                break;
                            case 179:
                            case 181:
                            case 185:
                                Reader.ReadBytes(40);
                                selectObjects = true;
                                break;
                            case 147:
                            case 149:
                            case 153:
                                Reader.ReadBytes(40);
                                hasCommands = false;
                                selectObjects = true;
                                break;
                            case 55:
                            case 59:
                            case 61:

                                Reader.ReadBytes(44);
                                break;

                            case 23:
                            case 27:
                            case 29:
                                Reader.ReadBytes(44);
                                hasCommands = false;
                                break;
                            case 183:
                            case 187:
                            case 189:
                                Reader.ReadBytes(44);
                                selectObjects = true;
                                break;
                            case 151:
                            case 155:
                            case 157:
                                Reader.ReadBytes(44);
                                hasCommands = false;
                                selectObjects = true;
                                break;
                            case 63:
                                Reader.ReadBytes(48);
                                break;
                            case 31:
                                Reader.ReadBytes(48);
                                hasCommands = false;
                                break;
                            case 191:
                            case 223:
                                Reader.ReadBytes(48);
                                selectObjects = true;
                                break;
                            case 159:
                                Reader.ReadBytes(48);
                                hasCommands = false;
                                selectObjects = true;
                                break;
                            default:
                                throw new Exception($"Unknown main command: {mainCommand}");
                        }
                        int message_count = Reader.ReadInt32();
                        // ingame chat
                        for (int i = 0; i < message_count; i++)

                        {
                            int sender = Reader.ReadInt32();
                            int reciever = Reader.ReadInt32();
                            bufLength = Reader.ReadInt32();
                            var msg = Encoding.Unicode.GetString(Reader.ReadBytes((int)bufLength * 2));
                            actions.Add(new GameAction() { Player = Players[sender], Duration = duration, Type = "chat", Message = $"sending message to {Players[reciever].name}: {msg}" });
                        }


                        duration += Reader.ReadByte();

                        int commandsCount = 0;
                        if (hasCommands)
                        {
                            if (mainCommand == 65 || mainCommand == 67 || mainCommand == 73
                                || mainCommand == 75 || mainCommand == 193 || mainCommand == 195
                                || mainCommand == 201 || mainCommand == 203 || mainCommand == 207 || mainCommand == 223)
                            {
                                commandsCount = Reader.ReadInt32();
                            }
                            else
                            {
                                commandsCount = Reader.ReadByte();
                            }



                            for (int i = 0; i < commandsCount; i++)
                            {
                                ExpectedByte(1);

                                int commandId = Reader.ReadInt32();

                                if (commandId == 14)
                                {
                                    Reader.ReadBytes(12);
                                }
                                ExpectedByte(commandId);


                                int playerId = Reader.ReadInt32();

                                ExpectedInt32(-1);
                                ExpectedInt32(-1);
                                ExpectedInt32(3);


                                int unknown0 = Reader.ReadInt32();

                                if (unknown0 == 1)
                                {
                                    ExpectedInt32(playerId);
                                }
                                else if (unknown0 != 0)
                                {
                                    throw new Exception($"Unknown0: {unknown0}");
                                }

                                int unknown1 = Reader.ReadInt32();
                                selectedCount = Reader.ReadInt32();

                                selectedIds = new List<int>();
                                for (int j = 0; j < selectedCount; j++)
                                {
                                    selectedIds.Add(Reader.ReadInt32());
                                }

                                int unknown2 = Reader.ReadInt32();

                                Reader.ReadBytes(unknown2 * 12);
                                int unknownCount = Reader.ReadInt32();
                                List<byte> unknownList = new List<byte>();
                                for (int j = 0; j < unknownCount; j++)
                                {
                                    unknownList.Add(Reader.ReadByte());
                                }


                                ExpectedByte(0);
                                ExpectedInt32(0);
                                ExpectedInt32(0);
                                ExpectedInt32(0);
                                ExpectedInt32(-1);

                                Reader.ReadBytes(4);

                                if (commandId == 0)
                                {
                                    ExpectedValue(unknown1, 0);
                                    ExpectedValue(unknownCount, 4);

                                    if (unknown2 == 0)
                                    {
                                        int objectId = Reader.ReadInt32();
                                        Reader.ReadBytes(16);

                                        actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "action" });
                                    }
                                    else if (unknown2 == 1)
                                    {
                                        Reader.ReadBytes(8);
                                        ExpectedByte(0);
                                        ExpectedByte(0);
                                        ExpectedByte(128);
                                        ExpectedByte(191);
                                        ExpectedByte(0);
                                        ExpectedByte(0);
                                        ExpectedByte(128);
                                        ExpectedByte(191);
                                        ExpectedByte(0);
                                        ExpectedByte(0);
                                        ExpectedByte(128);
                                        ExpectedByte(191);

                                        actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "way_point" });

                                    }
                                    else
                                    {
                                        Reader.ReadBytes(4);
                                        ExpectedInt32(0);
                                        ExpectedByte(0);
                                        ExpectedByte(0);
                                        ExpectedByte(128);
                                        ExpectedByte(191);
                                        ExpectedByte(0);
                                        ExpectedByte(0);
                                        ExpectedByte(128);
                                        ExpectedByte(191);
                                        ExpectedByte(0);
                                        ExpectedByte(0);
                                        ExpectedByte(128);
                                        ExpectedByte(191);

                                        actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown0" });
                                    }

                                    ExpectedByte(0);
                                    ExpectedByte(0);
                                    ExpectedByte(128);
                                    ExpectedByte(63);

                                    if (ExeVersion >= 154583)
                                    {
                                        if (Reader.ReadByte() == 255)
                                        {
                                            Reader.ReadBytes(7);
                                        }
                                        else
                                        {
                                            Reader.BaseStream.Seek(Reader.BaseStream.Position - 1, SeekOrigin.Begin);
                                        }
                                    }

                                }
                                else if (commandId == 1)
                                {

                                    ExpectedValue(unknown2, 0);
                                    ExpectedValue(unknownCount, 4);

                                    int techID = Reader.ReadInt32();
                                    var tech = TechDatas.FirstOrDefault(x => x.ID == techID);
                                    string techName = tech != null ? tech.DisplayName : techID.ToString();
                                    actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "research_tech1", Message = $"researching tech: {techName}" });
                                }
                                else if (commandId == 2)
                                {
                                    ExpectedValue(unknown2, 0);
                                    ExpectedValue(unknownCount, 4);

                                    int unitId = Reader.ReadInt32();
                                    int shipmentId = Reader.ReadInt32();

                                    ExpectedByte(255);
                                    int amount = Reader.ReadByte();
                                    Reader.ReadBytes(4);
                                    if (unknown1 == 0)
                                    {
                                        ExpectedValue(shipmentId, -1);

                                        var unit = UnitDatas.FirstOrDefault(x => x.ID == unitId);

                                        string unitName = unit != null ? unit.DisplayName : unitId.ToString();
                                        actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "train", Message = $"training: {unitName}(x{amount})" });

                                    }
                                    else if (unknown1 == 2)
                                    {
                                        ExpectedValue(unitId, -1);
                                        ExpectedValue(amount, 1);

                                        if (shipmentId != -1)
                                        {
                                            var selectedDeckId = Players.FirstOrDefault(x => x.id == playerId).SelectedDeckId;
                                            PlayerDeck selectedDeck;
                                            if (selectedDeckId == -1)
                                            {
                                                selectedDeck = Players.FirstOrDefault(x => x.id == playerId).Decks[0];
                                            }
                                            else
                                            {
                                                selectedDeck = Players.FirstOrDefault(x => x.id == playerId).Decks.FirstOrDefault(x => x.Id == selectedDeckId);

                                            }
                                            if (selectedDeck != null && shipmentId < selectedDeck.SortedCards.Count)
                                            {
                                                var card = selectedDeck.SortedCards[shipmentId];
                                                actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "shipment", Message = $"sending shipment: {card.DisplayName}" });

                                            }
                                            else
                                            {
                                                actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "shipment", Message = $"sending shipment: {shipmentId}" });
                                            }

                                        }
                                        else
                                        {
                                            actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "shipment", Message = $"sending shipment: {shipmentId}" });
                                        }

                                    }
                                    else
                                    {
                                        throw new Exception($"Unknown1: {unknown1}");
                                    }
                                    if (ExeVersion >= 154583)
                                        Reader.ReadByte(); // ADDED ZERO or 1
                                }
                                else if (commandId == 3)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(4, unknownCount);

                                    int buildId = Reader.ReadInt32();

                                    Reader.ReadBytes(24);
                                    ExpectedInt32(-1);
                                    ExpectedInt32(-1);
                                    Reader.ReadBytes(8);

                                    var unit = UnitDatas.FirstOrDefault(x => x.ID == buildId);

                                    string unitName = unit != null ? unit.DisplayName : buildId.ToString();
                                    actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "build", Message = $"building: {unitName}" });

                                }
                                else if (commandId == 4)
                                {
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);

                                    Reader.ReadBytes(25);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown1" });
                                }
                                else if (commandId == 6)
                                {
                                    ExpectedValue(4, unknown1);
                                    ExpectedValue(0, selectedCount);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown2" });
                                    Reader.ReadBytes(32);
                                    Reader.ReadBytes(4);
                                }
                                else if (commandId == 7)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(4, unknownCount);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown3" });
                                    if (ExeVersion > 132919)
                                        Reader.ReadByte(); // 0
                                }
                                else if (commandId == 9)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown4" });
                                }
                                else if (commandId == 12)
                                {
                                    //ExpectedValue(1, unknown2); it was 1 before, now can be both 0 or 1
                                    ExpectedValue(4, unknownCount);

                                    int abilityId = Reader.ReadInt32();
                                    Reader.ReadBytes(24);
                                    int objectId = Reader.ReadInt32();
                                    int powerId = Reader.ReadInt32();
                                    string Type = "";
                                    if (unknown1 == 0)
                                    {
                                        Type = "ability";
                                        // if (ExeVersion > 132919) hmmm I was mistaken here probably
                                        if (ExeVersion > 135159)
                                            Reader.ReadBytes(1); // EAT 1 byte
                                    }
                                    else if (unknown1 == 3)
                                    {
                                        ExpectedValue(abilityId, -1);

                                        Type = "special_power";
                                    }
                                    else
                                    {
                                        throw new Exception($"Unknown1: {unknown1}");
                                    }



                                    actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = Type });
                                }
                                else if (commandId == 13)
                                {

                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(1, selectedCount);
                                    ExpectedValue(playerId, selectedIds[0]);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);

                                    Reader.ReadBytes(4);
                                    ExpectedInt32(0);
                                    Reader.ReadBytes(4);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown5" });
                                }
                                else if (commandId == 14)
                                {
                                    ExpectedValue(4, unknownCount);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown6" });
                                }
                                else if (commandId == 16)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(0, selectedCount);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);

                                    Reader.ReadBytes(4);
                                    ExpectedInt32(playerId);
                                    Reader.ReadBytes(4);

                                    actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "resigned", Message = "Player resigned" });
                                    //if (ExeVersion >= 154583)
                                    Reader.ReadByte(); // some byte
                                }
                                else if (commandId == 18)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown7" });
                                    Reader.ReadBytes(4);
                                }
                                else if (commandId == 19)
                                {
                                    ExpectedValue(4, unknown1);
                                    ExpectedValue(0, selectedCount);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);

                                    Reader.ReadBytes(26);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown8" });
                                }
                                else if (commandId == 23)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);

                                    Reader.ReadBytes(5);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown9" });
                                    if (ExeVersion >= 154583)
                                        Reader.ReadByte(); // added some byte
                                }
                                else if (commandId == 24)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);

                                    Reader.ReadBytes(12);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown10" });
                                }
                                else if (commandId == 25)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);

                                    Reader.ReadBytes(1);
                                    ExpectedByte(255);
                                    Reader.ReadBytes(4);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown11" });
                                }
                                else if (commandId == 26)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(0, selectedCount);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);

                                    int withPlayerId = Reader.ReadByte();
                                    int diplomacy = Reader.ReadByte();
                                    Reader.ReadBytes(2);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "diplomacy" });
                                }
                                else if (commandId == 34)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(4, unknownCount);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown12" });
                                }
                                else if (commandId == 35)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(2, unknownCount);
                                    Reader.ReadBytes(4);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown13" });
                                }
                                else if (commandId == 37)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    Reader.ReadBytes(5);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown14" });
                                }
                                else if (commandId == 41)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    int control1 = Reader.ReadInt32();
                                    int control2 = Reader.ReadInt32();
                                    int control3 = Reader.ReadInt32();
                                    ExpectedInt32(-1);
                                    ExpectedInt32(0);
                                    unknown1 = Reader.ReadInt32();
                                    if (control1 == 1)
                                    {
                                        if (control2 == 1)
                                        {
                                            ExpectedValue(5, control3);
                                        }
                                        else
                                        {
                                            ExpectedValue(0, control3);
                                        }
                                        ExpectedValue(-1, unknown1);
                                        ExpectedInt32(0);
                                        unknown2 = Reader.ReadInt32();
                                        int unknown3 = -1;
                                        if (unknown2 == 1)
                                        {
                                            unknown3 = Reader.ReadInt32();
                                        }
                                        ExpectedByte(1);

                                        if (unknown2 == 1)
                                        {
                                            ExpectedByte(0);
                                            ExpectedByte(0);
                                            ExpectedByte(128);
                                            ExpectedByte(191);
                                            ExpectedByte(0);
                                            ExpectedByte(0);
                                            ExpectedByte(128);
                                            ExpectedByte(191);
                                            ExpectedByte(0);
                                            ExpectedByte(0);
                                            ExpectedByte(128);
                                            ExpectedByte(191);
                                        }
                                        else
                                        {
                                            Reader.ReadBytes(12);
                                        }

                                        if (control2 == 1)
                                        {
                                            actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "allies_request" });
                                        }
                                        else if (control2 == 3)
                                        {
                                            string resource;
                                            if (unknown3 == 0)
                                            {
                                                resource = "gold";
                                            }
                                            else if (unknown3 == 1)
                                            {
                                                resource = "wood";
                                            }
                                            else if (unknown3 == 2)
                                            {
                                                resource = "food";
                                            }
                                            else
                                            {
                                                throw new Exception($"unknown3: {unknown3}");
                                            }

                                            actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "allies_request" });
                                        }
                                        else if (control2 == 4)
                                        {
                                            string resource;
                                            if (unknown3 == 0)
                                            {
                                                resource = "gold";
                                            }
                                            else if (unknown3 == 1)
                                            {
                                                resource = "wood";
                                            }
                                            else if (unknown3 == 2)
                                            {
                                                resource = "food";
                                            }
                                            else
                                            {
                                                throw new Exception($"unknown3: {unknown3}");
                                            }

                                            actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "allies_request" });

                                        }
                                        else if (control2 == 5)
                                        {

                                            actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "allies_request" });

                                        }
                                        else if (control2 == 7)
                                        {

                                            actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "allies_request" });

                                        }
                                        else if (control2 == 8)
                                        {

                                            actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "allies_request" });

                                        }
                                        else
                                        {
                                            throw new Exception($"control2: {control2}");
                                        }
                                    }
                                    else if (control1 == 3)
                                    {
                                        ExpectedValue(0, control2);
                                        ExpectedValue(0, control3);
                                        ExpectedInt32(0);
                                        ExpectedInt32(0);
                                        Reader.ReadBytes(13);
                                        actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown15" });
                                    }
                                    else
                                    {
                                        throw new Exception($"control1: {control1}");
                                    }
                                }
                                else if (commandId == 44)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(1, selectedCount);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    Reader.ReadBytes(8);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown16" });
                                }
                                else if (commandId == 46)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    Reader.ReadBytes(8);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown17" });
                                }
                                else if (commandId == 48)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(0, selectedCount);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    Reader.ReadBytes(9);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown18" });
                                }
                                else if (commandId == 53)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(4, unknownCount);
                                    Reader.ReadBytes(8);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown19" });
                                }
                                else if (commandId == 57)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(4, unknownCount);
                                    int objectId = Reader.ReadInt32();
                                    Reader.ReadBytes(4);
                                    int resource = Reader.ReadInt32();

                                    actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "change_trade_route_resource" });

                                }
                                else if (commandId == 58)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(0, selectedCount);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    Reader.ReadBytes(4);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown20" });
                                }
                                else if (commandId == 61)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    int objectId = Reader.ReadInt32();
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "shipments_way_point" });
                                    if (ExeVersion > 160787)
                                        Reader.ReadBytes(4); // some 4 bytes
                                }
                                else if (commandId == 62)
                                {
                                    ExpectedValue(0, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(4, unknownCount);
                                    Reader.ReadBytes(4);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown21" });
                                }
                                else if (commandId == 63)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    int unitId = Reader.ReadInt32();
                                    int objectId = Reader.ReadInt32();
                                    int amount = Reader.ReadInt32();

                                    var unit = UnitDatas.FirstOrDefault(x => x.ID == unitId);

                                    string unitName = unit != null ? unit.DisplayName : unitId.ToString();
                                    actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "spawn_unit", Message = $"spawning unit: {unitName}(x{amount})" });
                                    if (ExeVersion >= 154583)
                                        Reader.ReadBytes(4); // some 4 bytes =70
                                }
                                else if (commandId == 64)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(0, selectedCount);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(2, unknownCount);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown22" });
                                }
                                else if (commandId == 65)
                                {
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(4, unknownCount);

                                    int techID = Reader.ReadInt32();
                                    string Type;
                                    if (unknown1 == 0)
                                    {
                                        Type = "research_tech2";
                                    }

                                    else if (unknown1 == 2)
                                    {
                                        Type = "research_tech3";
                                    }

                                    else
                                    {
                                        throw new Exception($"Unknown1: {unknown1}");
                                    }
                                    var tech = TechDatas.FirstOrDefault(x => x.ID == techID);
                                    string techName = tech != null ? tech.DisplayName : techID.ToString();
                                    actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = Type, Message = $"researching tech: {techName}" });

                                }
                                else if (commandId == 66)
                                {
                                    ExpectedValue(2, unknown1);
                                    ExpectedValue(1, selectedCount);
                                    ExpectedValue(playerId, selectedIds[0]);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(4, unknownCount);
                                    int deckId = Reader.ReadInt32();
                                    Reader.ReadBytes(4);
                                    Players.FirstOrDefault(x => x.id == playerId).SelectedDeckId = deckId;
                                    if (Players.FirstOrDefault(x => x.id == playerId).Decks.Count > 0)
                                    {
                                        actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "pick_deck", Message = $"picking deck: {Players.FirstOrDefault(x => x.id == playerId).Decks[deckId].Name}" });
                                    }
                                    else
                                    {
                                        actions.Add(new GameAction() { Player = Players.FirstOrDefault(x => x.id == playerId), Duration = duration, Type = "pick_deck", Message = $"picking deck: {deckId}" });
                                    }
                                }
                                else if (commandId == 67)
                                {
                                    ExpectedValue(3, unknown1);
                                    ExpectedValue(0, selectedCount);
                                    ExpectedValue(0, unknown2);
                                    ExpectedValue(4, unknownCount);
                                    Reader.ReadBytes(12);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown23" });
                                }
                                else if (commandId == 71)
                                {
                                    Reader.ReadBytes(4);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown24" });
                                }
                                else if (commandId == 72)
                                {
                                    Reader.ReadBytes(16);
                                    actions.Add(new GameAction() { Duration = duration, Player = Players.FirstOrDefault(x => x.id == playerId), Type = "unknown", Message = "unknown25" });
                                }
                                else
                                {
                                    throw new Exception($"Unknown command: {commandId}");
                                }
                            }



                        }
                        /*
                        if (selectObjects)
                        {
                            selectedCount = Reader.ReadByte();
                            selectedIds = new List<int>();
                            for (int i = 0; i < selectedCount; i++)
                            {
                                selectedIds.Add(Reader.ReadInt32());
                            }
                        }
                       // Debug.WriteLine(Reader.BaseStream.Position);
                        try
                        {
                            int resignCount = Reader.ReadByte();
                            List<int> resigned = new List<int>();
                            for (int i = 0; i < resignCount; i++)
                            {
                                resigned.Add(Reader.ReadByte());
                            }
                        }
                        catch
                        {

                        }*/
                        bufOffset = Search(data, (int)Reader.BaseStream.Position, new byte[] { 0x1, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x19 });

                    }
                });
                GameDuration = TimeSpan.FromMilliseconds(duration);
                Actions = new ReadOnlyCollection<GameAction>(actions);
                actionsCollection.Source = Actions;
                actionsCollection.Filter += Filter;
                foreach (var player in Players)
                {
                    player.CPM = actions.Count(x => x.Player.id == player.id) / (actions.Last().Duration / 1000.0 / 60.0);
                }

                Players.RemoveAt(0);
                for (int i = GameNumPlayers; i < 12; i++)
                {
                    Players.RemoveAt(GameNumPlayers);
                }

                NotifyPropertyChanged("Players");
                NotifyPropertyChanged("RenamedRecord");
                NotifyPropertyChanged("RecordOwner");
                /*Debug.WriteLine("player 1");
                foreach (var line in actions.Where(x=>x.Player.id==1).GroupBy(info => info.Type)
                    .Select(group => new {
                        Type = group.Key,
                        Count = group.Count()
                    })
                    .OrderBy(x => x.Type))
                {
                    Debug.WriteLine("{0} {1}", line.Type, line.Count);
                }

                Debug.WriteLine("player 2");
                foreach (var line in actions.Where(x => x.Player.id == 2).GroupBy(info => info.Type)
                    .Select(group => new {
                        Type = group.Key,
                        Count = group.Count()
                    })
                    .OrderBy(x => x.Type))
                {
                    Debug.WriteLine("{0} {1}", line.Type, line.Count);
                }
                */

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File name: {RecordPathFileName}\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
