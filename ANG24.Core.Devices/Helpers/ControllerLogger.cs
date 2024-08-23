using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANG24.Core.Devices.Helpers
{
    public static class ControllerLogger
    {
        private static readonly string dirName = "C:\\Log\\Device";
        private static readonly DirectoryInfo dirInfo;
        private static readonly object locker = new object();
        public static double GP500Voltage;
        public static double GP500Current;
        public static bool GP500On;
        public static bool read = false;

        static ControllerLogger()
        {
            dirInfo = new DirectoryInfo(dirName);
            if (!dirInfo.Exists)
                dirInfo.Create();
        }
        public static async void WriteString(string[] values) //Принимает строку разделенную по значениям
        {

            bool LogDataCollected = false;

            if (!read)
            {
                await Task.Run(() =>
                {
                    lock (locker)
                    {
                        try
                        {
                            StreamWriter fm = new StreamWriter(@"" + dirInfo.FullName + @"LogInfo_" +
                                    DateTime.Now.ToString("yyyy-MM-dd") +
                                     ".log", true);

                            fm.Write("{0:T} -> ", DateTime.Now);
                            if (GP500On) //если включен режим GP500, то заменять значения текущего напряжения и тока на его
                            {
                                values[4] = $"{GP500Voltage}";
                                values[5] = $"{GP500Current}";
                            }
                            foreach (var item in values)
                            {
                                //if(Logging)
                                //{
                                fm.Write(item + ",");
                                LogDataCollected = true;
                                //}
                                //Console.Write(@"{0};", item);
                            }
                            if (LogDataCollected)
                            {
                                fm.WriteLine();
                                LogDataCollected = false;
                            }
                            fm.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                });
            }
            //Console.WriteLine();
        }
        public static async void WriteString(string message, bool command = false)
        {
            if (!read)
            {
                await Task.Run(() =>
                {
                    lock (locker)
                    {
                        try
                        {
                            StreamWriter fm = new StreamWriter(@"" + dirInfo.FullName + @"\LogInfo_" +
                                    DateTime.Now.ToString("yyyy-MM-dd") +
                                     ".log", true);
                            if (!command)
                            {
                                fm.Write($"{DateTime.Now.ToString("HH:mm")}.{DateTime.Now.Millisecond} -> ");
                            }
                            else
                            {
                                fm.Write($"{DateTime.Now.ToString("HH:mm")}.{DateTime.Now.Millisecond} <- ");
                            }
                            Console.WriteLine($"{message}");
                            fm.Write(@"{0}", message);
                            fm.WriteLine();
                            fm.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                });
            }
        }
    }
}
