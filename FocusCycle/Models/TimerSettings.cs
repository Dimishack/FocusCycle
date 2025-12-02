using MessagePack;

namespace FocusCycle.Models
{
    [MessagePackObject]
    public class TimerSettings
    {
        [Key(0)]
        public bool IsAutorun { get; set; } = false;

        [Key(1)]
        public TimeSpan WorkTimer { get; set; } = TimeSpan.FromMinutes(80);

        [Key(2)]
        public TimeSpan BreakTimer { get; set; } = TimeSpan.FromSeconds(15);
    }
}
