using System;

namespace UpdateChecker.Lib
{
    [Serializable]
    public class Config
    {
        public bool StartWithWindows { get; set; }

        public bool MinimizeToTray { get; set; }

        public bool Timer { get; set; }

        public float UpdateInterval { get; set; }
    }
}
