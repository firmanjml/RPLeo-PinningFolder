using Newtonsoft.Json;
using PinFolder.Model;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PinFolder
{
    class Program
    {
        private static int verbose_delay = 0;
        private static bool verbose = false;

        static void Main(string[] args)
        {
            Configuration config = JsonConvert.DeserializeObject<Configuration>(Utils.loadFile($"{Utils.CURRENT_DIRECTORY}\\config.json"));

            verbose = (config.verbose) ? true : false;
            verbose_delay = (config.verbose) ? config.verbose_sleep_time * 1000 : 0;

            if (!Directory.Exists(""))
            {
                string[] folders = Directory.GetDirectories(config.folder_dir);
                Directory.CreateDirectory("Modules");
                foreach (string folder in folders)
                {
                    string name = folder.Replace($"{config.folder_dir}\\", "");
                    if (name.Length > 3)
                    {
                        name = name.Substring(0, 4);
                    }
                    Utils.createShortcut(folder, name);
                }
            }

            if (!String.IsNullOrEmpty(config.prev_dir))
            {
                if (verbose)
                {
                    Console.WriteLine("Removing current lesson from taskbar...");
                }
                Utils.startProgram(config.prev_dir, false);
                Utils.saveFile($"{Utils.CURRENT_DIRECTORY}\\config.json", "");
                Thread.Sleep(3000);
            }

            Dictionary<string, Calendar> dict = new Dictionary<string, Calendar>();
            foreach (Calendar calendar in config.calendar)
            {
                dict.Add(Utils.timeStampToDate(calendar.StartTime).ToString(), calendar);
            }
            Hashtable ht = new Hashtable(dict);

            if (ht.ContainsKey(Utils.DATE_NOW.ToString()))
            {

                Calendar c = (Calendar)ht[Utils.DATE_NOW.ToString()];
                c.Title = c.Title.Substring(0, 4);

                if (verbose == true)
                {
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("Today's Date " + Utils.DATE_NOW.ToString("dddd, dd MMMM yyyy"));
                    Console.WriteLine("-------------------------------------");
                    Console.WriteLine("");
                    Console.WriteLine($"Module ID {c.Id}");
                    Console.WriteLine($"Module Code {c.Title}");
                    Console.WriteLine($"Module Venue {c.Venue}");
                }

                Utils.startProgram($"{Utils.CURRENT_DIRECTORY}\\Modules\\{c.Title}.lnk", true);
                Utils.saveFile($"{Utils.CURRENT_DIRECTORY}\\config.json", $"{Utils.CURRENT_DIRECTORY}\\Modules\\{c.Title}.lnk");
            }

            if (verbose)
            {
                Thread.Sleep(verbose_delay);
            }
        }
    }
}
