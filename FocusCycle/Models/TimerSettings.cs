using MessagePack;

namespace FocusCycle.Models
{
    [MessagePackObject]
    public class TimerSettings
    {
        [Key(0)]
        public TimeSpan WorkTime { get; set; } = TimeSpan.FromMinutes(80);

        [Key(1)]
        public TimeSpan BreakTime { get; set; } = TimeSpan.FromSeconds(15);

        [Key(2)]
        public double Volume { get; set; } = 1.0;
    }
}
