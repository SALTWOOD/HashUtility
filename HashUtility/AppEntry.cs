using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashUtility
{
    public class AppEntry
    {
        [STAThread]
        public static void Main(string[] args)
        {
            App app = new App();
            app.Run();
        }
    }
}
