using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Control_IO.Misc
{
    class ComputeGesture
    {
        FormMain_03 form;
        static string file = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\Settings\\gesture_settings.txt";
        static List<ComputeGesture> settings { get; set; }        
        string name { get; set; }
        string category { get; set; }
        string value { get; set; }


        public ComputeGesture()
        {
        }

        public static void AssignFunction(string name, string category, string value)
        {
            ComputeGesture gesture = settings.Where(x => x.name == name).First();
            gesture.category = category;
            gesture.value = value;
            WriteToFile();
            LoadSettings();
        }

        public static void UpdateGestures(string[] gestures)
        {
            List<string> g = new List<string>();
            for (int i = 1; i < gestures.Count(); i++)
            {                
                g.Add(gestures[i]);

                if (!settings.Any(x => x.name == gestures[i]))
                {
                    settings.Add(new ComputeGesture() { name = gestures[i], category = "", value = "" });
                }
            }

            settings.RemoveAll(x => !g.Exists(y => y == x.name));

            WriteToFile();
            LoadSettings();
        }

        private static void WriteToFile()
        {
            File.WriteAllLines(file, settings.Select(x => x.name + "|" + x.category + "|" + x.value));
        }

        public static void Compute(string name)
        {
            LoadSettings();
            ComputeGesture gesture = settings.Where(x => x.name == name).First();

            if (gesture.category == "open")
                System.Diagnostics.Process.Start(gesture.value);
            else if (gesture.category == "key")
                SendKeys.SendWait(gesture.value);
        }

        public static List<string> GetGestureList()
        {
            LoadSettings();
            List<string> list = new List<string>();

            foreach (ComputeGesture ges in settings)            
                list.Add(ges.name);

            return list;
        }

        public static List<string> GetGesture(int idx)
        {
            ComputeGesture ges = settings[idx];
            List<string> g = new List<string>();
            g.Add(ges.name);
            g.Add(ges.category);
            g.Add(ges.value);
            return g;
        }

        private static void LoadSettings()
        {
            string[] gestures = File.ReadAllLines(file);
            settings = new List<ComputeGesture>();
            for (int i = 0; i < gestures.Count(); i++)
            {
                string[] setting = gestures[i].Split('|');

                settings.Add(new ComputeGesture() { name = setting[0], category = setting[1], value = setting[2] });

                //if (setting.Count() == 3)
                //settings.Add(new ComputeGesture() { name = setting[0], category = setting[1], value = setting[2] });
                //else
                //    settings.Add(new ComputeGesture() { name = setting[0], category = "", value = "" });
            }
        }
    }
}
