using PU_EmiReminder_Due.EmiReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PU_EmiReminder_Due
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                EmiReminderDue emiReminder = new EmiReminderDue();
                await emiReminder.Due_EmiReminder();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
