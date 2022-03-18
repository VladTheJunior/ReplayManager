using System;

namespace ReplayManager.Classes.Records
{
    public class GameVersion
    {
        public uint ExeVersion { get; set; }
        public string PatchVersion { get; set; }
        public string PatchNotes { get; set; }
        public DateOnly ReleaseDate { get; set; }
    }
}
