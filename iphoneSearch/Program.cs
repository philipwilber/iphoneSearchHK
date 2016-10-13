using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace iphoneSearch
{
    class Program
    {
        private static System.Timers.Timer aTimer;
        static void Main(string[] args)
        {
            aTimer = new System.Timers.Timer();
            aTimer.Enabled = true;
            aTimer.Interval = 2000; //执行间隔时间,单位为毫秒; 这里实际间隔为10分钟  
            aTimer.Start();
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(getAlert);
            
            Console.ReadKey();
        }

        public static void getAlert(object source, ElapsedEventArgs e)
        {
            String str = "";
            string[] arry = { "R499", "R409", "R485", "R428", "R610", "R673" };
            //string url1 = "https://reserve.cdn-apple.com/HK/zh_HK/reserve/iPhone/availability?channel=1";
            string url = "https://reserve.cdn-apple.com/HK/zh_HK/reserve/iPhone/availability.json";
            string webContent = GetWebContent(url);

            JObject jObject = JObject.Parse(webContent);
            string updateCode = jObject["updated"].ToString();
            foreach (string store in arry)
            {
                JObject obj = JObject.Parse(jObject[store].ToString());
                JToken jUser = jObject[store];
                //7+ 黑色 128
                string name = (string)jUser["MN482ZP/A"];
                if (name != "NONE")
                {
                    switch (store)
                    {
                        case "R499":
                            str += "Canton Road ! \n";
                            break;
                        case "R409":
                            str += "Causeway Bay ! \n";
                            break;
                        case "R485":
                            str += "Festival Walk ! \n";
                            break;
                        case "R428":
                            str += "ifc mall ! \n";
                            break;
                        case "R610":
                            str += "New Town Plaza ! \n";
                            break;
                        case "R673":
                            str += "apm Hong Kong ! \n";
                            break;
                    }
                }
            }

            Console.WriteLine("time: " + DateTime.Now.ToString("hh:mm:ss") + "    updated:" + updateCode);
            if (str != "")
            {
                aTimer.Stop();
                aTimer.Dispose();
                System.Diagnostics.Process.Start("https://reserve.cdn-apple.com/HK/zh_HK/reserve/iPhone/availability?channel=1");
                MessageBox.Show(str);
                
            }
           
        }
        private static string GetWebContent(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse respone = (HttpWebResponse)request.GetResponse();
            Stream stream = respone.GetResponseStream();

            Encoding encoding = Encoding.Default;
            encoding = Encoding.GetEncoding("utf-8");

            StreamReader streamReader = new StreamReader(stream, encoding);
            return streamReader.ReadToEnd();
        }
        
    }
}
