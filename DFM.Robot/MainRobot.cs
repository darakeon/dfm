using DFM.BusinessLogic.SuperServices;
using DFM.Email;
using DFM.Entities;

namespace DFM.Robot
{
    public class MainRobot
    {
        public static void Run(User user, Format.GetterForMove formatGetter, RobotService access)
        {
            var scheduleRunner = new ScheduleRunner(user, formatGetter, access);
            
            scheduleRunner.Run();
        }

    }
}
