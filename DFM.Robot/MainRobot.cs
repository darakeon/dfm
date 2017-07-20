using DFM.BusinessLogic.SuperServices;
using DFM.Email;
using DFM.Entities;

namespace DFM.Robot
{
    public class MainRobot
    {
        public static void Run(User user, RobotService access)
        {
            var scheduleRunner = new ScheduleRunner(user, access);
            
            scheduleRunner.Run();
        }

    }
}
