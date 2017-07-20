using DFM.Email;
using DFM.Entities;

namespace DFM.Robot
{
    public class MainRobot
    {
        public static void Run(User user, Format.GetterForMove formatGetter)
        {
            new ScheduleRunner(user, formatGetter).Run();
        }

    }
}
