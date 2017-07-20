using DFM.Core.Email;
using DFM.Core.Entities;

namespace DFM.Core.Robots
{
    public class MainRobot
    {
        public static void Run(User user, Format.GetterForMove formatGetter)
        {
            new ScheduleRunner(user, formatGetter).Run();
        }

    }
}
