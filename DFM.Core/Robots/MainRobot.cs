using DFM.Core.Entities;

namespace DFM.Core.Robots
{
    public class MainRobot
    {
        public static void Run(User user)
        {
            ScheduleRunner.Run(user);
            FixRateRunner.Run(user);
        }
    }
}
