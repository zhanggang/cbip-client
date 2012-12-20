using System;
using System.Text;
using System.Net.Sockets;
using log4net;

namespace Console
{
    public class Command
    {
        private ILog _log = LogManager.GetLogger(typeof(Command));
        private TcpClient _client = new TcpClient(Config.Server, Config.Port);

        /// <summary>
        /// 连接服务器
        /// </summary>
        public void Bind(string loginName, string password)
        {
            //_log.Error("开始Bind操作");

            try
            {
                
                    NetworkStream stream = _client.GetStream();
                    byte[] data = new byte[22];

                    // 消息长度
                    data[0] = Convert.ToByte(data.Length);

                    // 命令
                    data[4] = Convert.ToByte(1);

                    // 登录名称
                    byte[] name = Encoding.ASCII.GetBytes(loginName);
                    for (int i = 0; i < name.Length; i++)
                        data[8 + i] = name[i];

                    // 登录密码
                    byte[] pwd = Encoding.ASCII.GetBytes(password);
                    for (int i = 0; i < pwd.Length; i++)
                        data[16 + i] = pwd[i];


                    _log.Info(String.Format("Bind操作数据发送: {0}  {1}", loginName, password));

                    stream.Write(data, 0, data.Length);

                    // _log.Info("Bind操作数据包发送完毕,准备接受应答数据包.");

                    data = new Byte[256];
                    String responseData = String.Empty;
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = Encoding.ASCII.GetString(data, 0, bytes);

                    _log.Info(String.Format("Bind操作应答数据包解包完毕,内容为:{0}", responseData));
                }
            
            catch (Exception e)
            {
                _log.Error(String.Format("Bind操作异常:{0}", e.Message));
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void UnBind()
        {
            // _log.Error("开始UnBind操作");

            try
            {
                
                    NetworkStream stream = _client.GetStream();
                    byte[] data = new byte[8];

                    // 消息长度
                    data[0] = Convert.ToByte(data.Length);

                    // 命令
                    data[4] = Convert.ToByte(3);

                    _log.Info("UnBind操作数据包发送-------------------------------------");

                    stream.Write(data, 0, data.Length);

                    // _log.Info("UnBind操作数据包发送完毕,准备接受应答数据包.");

                    //data = new Byte[256];
                    //String responseData = String.Empty;
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    //responseData = Encoding.ASCII.GetString(data, 0, bytes);

                    //_log.Info(String.Format("UnBind操作应答数据包解包完毕,内容为:{0}", responseData));
                }
            
            catch (Exception e)
            {
                _log.Error(String.Format("UnBind操作异常:{0}", e.Message));
            }
        }

        /// <summary>
        /// 提交数据 Submit 操作
        /// </summary>
        public void Submit(string loginName, string message)
        {
            // _log.Error("开始Submit操作");
            try
            {
                
                    NetworkStream stream = _client.GetStream();                                        
                    byte[] msg = Encoding.ASCII.GetBytes(message);
                    byte[] data = new byte[msg.Length + 37];                    

                    // 消息长度
                    //tmp = ConvertIntToBytes(data.Length);
                    //for (int i = 0; i < tmp.Length; i++)
                      //  data[3-i] = tmp[i];
                    data[0] = Convert.ToByte(data.Length);

                    // 命令
                    data[4] = Convert.ToByte(2);

                    // 消息来源
                    //tmp = Encoding.ASCII.GetBytes(Config.From);
                    byte[] source = Encoding.ASCII.GetBytes(loginName);
                    for (int i = 0; i < source.Length; i++)
                        data[8 + i] = source[i];

                    // 消息去向
                    byte[] dest = Encoding.ASCII.GetBytes(Config.To);
                    for (int i = 0; i < dest.Length; i++)
                        data[16 + i] = dest[i];

                    // 内容长度
                    //tmp = ConvertIntToBytes(msg.Length);
                    //for (int i = 0; i < tmp.Length; i++)
                      //  data[27 - i] = tmp[i];
                    data[24] = Convert.ToByte(msg.Length);

                    // 内容
                    byte[] content = Encoding.ASCII.GetBytes(message);
                    for (int i = 0; i < content.Length; i++)
                        data[28 + i] = content[i];

                    // 填充的一个整数
                    data[data.Length - 9] = Convert.ToByte(0);

                    // 应答标志
                    //data[data.Length - 5] = Convert.ToByte('1');
                    //byte[] isrespon = Encoding.ASCII.GetBytes(Config.IsRespon);
                    //data[data.Length - 5] = isrespon[0];    
                    data[data.Length - 5] = Convert.ToByte(1);

                    // 填充的一个整数
                    data[data.Length - 4] = Convert.ToByte(0);

                    _log.Info(String.Format("Submit操作数据包:{0}", message));

                    stream.Write(data, 0, data.Length);

                    //_log.Info("Submit操作数据包发送完毕,准备接受应答数据包.");

                    data = new Byte[256];
                    String responseData = String.Empty;
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = Encoding.ASCII.GetString(data, 0, bytes);

                    _log.Info(String.Format("Submit应答数据包:{0}", responseData));

                
            }
            catch (Exception e)
            {
                _log.Error(String.Format("Submit异常类型:{0}", e.GetType()));
              //  _log.Error(String.Format("Submit异常信息:{0}", e.Message));
             //   _log.Error(String.Format("Submit异常来源:{0}", e.Source));
                _log.Error(String.Format("Submit异常堆栈:{0}", e.StackTrace));
                _log.Error(String.Format("Submit内部异常:{0}", e.InnerException));
            }
        }


        /// <summary>
        /// 整形转换成BYTE
        /// </summary>
        private byte[] ConvertIntToBytes(int value)
        {
            byte[] bArray = new byte[4];
            bArray[3] = (byte)(value & 0xFF);
            bArray[2] = (byte)((value >> 8) & 0xFF);
            bArray[1] = (byte)((value >> 16) & 0xFF);
            bArray[0] = (byte)((value >> 24) & 0xFF);

            return bArray;
        }
    }
}
