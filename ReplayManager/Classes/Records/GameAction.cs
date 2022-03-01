using System;

namespace ReplayManager.Classes.Records
{
    public class GameAction
    {
        public long Duration { get; set; }
        public GamePlayer Player { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }

        public TimeSpan Timestamp
        {
            get
            {
                return TimeSpan.FromMilliseconds(Duration);
            }
        }
    }
}
