using IWshRuntimeLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinFolder
{
    class Utils
    {
        public static string WIN_DIRECTORY = Environment.GetEnvironmentVariable("windir");
        public static string CURRENT_DIRECTORY = Directory.GetCurrentDirectory();
        public static DateTime DATE_NOW = DateTime.Now.Date;
        private static short PIN_TO_TASKBAR = 5386;
        private static short UNPIN_TO_TASKBAR = 5387;

        public static String loadFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string file = sr.ReadToEnd();
                    sr.Close();
                    return file;
                }
                
            } catch (Exception e)
            {
                Console.WriteLine($"Exception occured when loading file '{path}'");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                Environment.Exit(1);
            }
            return "";
        }

        public static bool saveFile(string path, string data)
        {
            try
            {
                JObject obj = JObject.Parse(loadFile(path));
                using(StreamWriter sw = new StreamWriter(path))
                {
                    obj["prev_dir"] = data;
                    sw.WriteLine(obj);
                    sw.Close();
                    return true;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public static DateTime timeStampToDate(long ts)
        {
            System.DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dt.AddMilliseconds(ts).ToLocalTime().Date;
        }

        public static void startProgram(string path, bool pin)
        {
            Process process = new Process();
            process.StartInfo.FileName = $"{CURRENT_DIRECTORY}\\syspin.exe";
            process.StartInfo.Arguments = $"{path} " + (pin ? PIN_TO_TASKBAR : UNPIN_TO_TASKBAR);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
        }

        public static void createShortcut(string path, string name)
        {
            WshShell wsh = new WshShell();
            IWshShortcut shortcut = wsh.CreateShortcut($"{CURRENT_DIRECTORY}\\Modules\\{name}.lnk") as IWshShortcut;
            shortcut.TargetPath = $"{WIN_DIRECTORY}\\explorer.exe";
            shortcut.Arguments = path;
            shortcut.WindowStyle = 1;
            shortcut.Save();
        }

    }
}
