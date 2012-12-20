using System;
using System.IO;
using log4net.Config;

namespace Console
{
    class Program
    {
        /// <summary>
        /// 程序入口
        /// </summary>
        static void Main(string[] args)
        {
            // 初始化日志模块
            XmlConfigurator.Configure();

            // 读取检查项目文件内容
            string[] items;
            using (StreamReader sr = new StreamReader(Config.FilePath))
            {
                items = sr.ReadToEnd().Split('\t');
            }

            // 按协议组装消息内容
            string[] msg = new string[30];
            for (int j = 0; j < 30; j++)
            {
                switch (j % 3)
                {
                    case 0:
                        msg[j] = String.Format("Z94{0:0.000}", Convert.ToSingle(items[j]));
                        break;
                    case 1:
                        msg[j] = String.Format("Z60{0:0.00}", Convert.ToSingle(items[j]));
                        break;
                    case 2:
                        msg[j] = String.Format("Z62{0:0.0}", Convert.ToSingle(items[j]));
                        break;
                }
            }

            // F1
            string Fmaohao = ":";
            // F3 发送命令
            string SendCmd = "@072";
            // F4 当前发送数据的时间
            DateTime dt = DateTime.Now;
            string CurrentTime = string.Format("{0:yyyyMMddHHmmss}", dt);
            // F11 F12
            string Fend = "FF&"; //协议规则的结束终止符
            // 
                        
            //int i = 1;
            for (int i = 1; i <= Config.Users.Count; i++)
            {                
                // 6000 = 60s
                System.Threading.Thread.Sleep(6000);

                Command cmd = new Command();
                // 登录帐号
                UserInfo user = Config.Users[i - 1];                
                cmd.Bind(user.LoginName, user.Password);

                // 提交数据
                string mess = String.Format("{0},{1},{2}", msg[3 * i - 3], msg[3 * i - 2], msg[3 * i - 1]);
                //message += "FF&";                 

                mess = Fmaohao + user.LoginName + SendCmd + CurrentTime + mess + Fend;
                
                cmd.Submit(user.LoginName, mess);
                
                cmd.UnBind();
            }

            System.Console.ReadKey();

            System.Environment.Exit(0);
        }    

    }
}
