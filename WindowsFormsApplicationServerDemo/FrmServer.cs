using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCP104Library;

namespace WindowsFormsApplicationServerDemo
{
    public partial class FrmServer : Form
    {
        int port = 6666;
        Socket socketSend;
        Socket _SocketWatch;
        //发计数。程序中使用，实际应用中暂未起作用
        short sr = 0;
        //收计数。程序中使用，实际应用中暂未起作用
        short nr = 0;

        public FrmServer()
        {
            InitializeComponent();

            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false; 

        }
        void Listen(object o)
        {
            try
            {
                Socket socketWatch = o as Socket;
                while (true)
                {
                    socketSend = socketWatch.Accept();//等待接收客户端连接
                    ShowMsg(socketSend.RemoteEndPoint.ToString() + ":" + "连接成功!");
                    //开启一个新线程，执行接收消息方法
                    Thread r_thread = new Thread(Received);
                    r_thread.IsBackground = true;
                    r_thread.Start(socketSend);
                }
            }
            catch { }
        }



        /// <summary>
        /// 服务器端不停的接收客户端发来的消息
        /// </summary>
        /// <param name="o"></param>
        void Received(object o)
        {
            try
            {
                Socket socketSend = o as Socket;
                while (true)
                {
                    //客户端连接服务器成功后，服务器接收客户端发送的消息
                    byte[] buffer = new byte[1024 * 1024 * 3];
                    //实际接收到的有效字节数
                    int len = socketSend.Receive(buffer);
                    if (len == 0)
                    {
                        break;
                    }
                    //Convert.ToString(d,16)
                    //buffer = ConvertUser10to16(buffer);

                    string filename = DateTime.Now.ToString("yyyyMMddHH") + ".txt";
                    string path = AppDomain.CurrentDomain.BaseDirectory + "\\log\\";
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    //文件路径
                    string filePath = path + filename;
                    string strcontent = getbytestr(buffer, len, true);
                    StringBuilder strsb = new StringBuilder();
                    strsb.AppendFormat("16进制：{0}{1}", strcontent, Environment.NewLine);
                    strsb.AppendFormat("10进制：{0}{1}", getbytestr(buffer, len, false), Environment.NewLine);
                    strsb.AppendFormat("{0}时间：{2}{1}======================================{1}", "", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //写入文件
                    File.AppendAllText(filePath, strsb.ToString());//添加至文件
                    bool jxFlag = true;
                   string res= DataAnalysis(buffer, len,out jxFlag);

                    //string str = Encoding.UTF8.GetString(buffer, 0, len);
                    ShowMsg("接收到的客户端数据" + socketSend.RemoteEndPoint + ",时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    ShowMsg(res);
                    if (!jxFlag)
                    {
                        filename = DateTime.Now.ToString("yyyyMMddHH") + ".txt";
                        path = AppDomain.CurrentDomain.BaseDirectory + "\\log\\jiexiFaile\\";
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(path);
                        }
                        //文件路径
                        filePath = path + filename;
                        strcontent = getbytestr(buffer, len, true);
                        strsb = new StringBuilder();
                        strsb.AppendFormat("16进制：{0}{1}", strcontent, Environment.NewLine);
                        strsb.AppendFormat("10进制：{0}{1}", getbytestr(buffer, len, false), Environment.NewLine);
                        strsb.AppendFormat("{0}时间：{2}{1}======================================{1}", "", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        //写入文件
                        File.AppendAllText(filePath, strsb.ToString());//添加至文件
                    }
                }
            }
            catch (Exception ex){
                WriteExecption(ex);
            }
        }

        /// <summary>
        /// 可变结构动词
        /// </summary>
        string kbjgdc = string.Empty;
        /// <summary>
        /// 传输原因 
        /// 1 周期、循环
        /// 2 背景扫描
        /// 3 突发
        /// 4 初始化
        /// 5 请求或被请求
        /// 6 激活
        /// 7 激活确认
        /// 8 停止激活
        /// 9 停止激活确认
        /// 0a激活结束
        /// 14响应总召唤
        /// </summary>
        string csyy = string.Empty;
        /// <summary>
        /// 传输原因 
        /// 1 周期、循环
        /// 2 背景扫描
        /// 3 突发
        /// 4 初始化
        /// 5 请求或被请求
        /// 6 激活
        /// 7 激活确认
        /// 8 停止激活
        /// 9 停止激活确认
        /// 0a激活结束
        /// 14响应总召唤
        /// </summary>
        string csyyName = string.Empty;
        /// <summary>
        /// 报文类型
        /// 64H＝100 总召唤命令
        /// 01H＝1   单点信息,遥信
        /// </summary>
        string typestr = string.Empty;
        /// <summary>
        /// 序列号是否连续
        /// </summary>
        bool lianxuFlag = false;
        /// <summary>
        /// 数据数量 如：15个遥信数据
        /// </summary>
        int dataCount = 0;
        /// <summary>
        /// 站址
        /// </summary>
        string zhanzhi = string.Empty;
        /// <summary>
        /// 信息体起始地址
        /// </summary>
        string infoBeginAddress = string.Empty;

        private string DataAnalysis(byte[] buffer, int len,out bool jxFlag)
        {
            jxFlag = true;
            //string test = "104 26 74 2 0 0 13 2 3 0 1 0 8 64 0 121 233 206 63 0 11 64 0 34 219 121 63 0";
            //string[] testlist = test.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //for (int i = 0; i < testlist.Length; i++)
            //{
            //    buffer[i] = byte.Parse(testlist[i]);
            //}

            //len = testlist.Length;

            string res = string.Empty;
            if (buffer.Length > 6)
            {
                byte typeid = buffer[6];
                int bufferlen = int.Parse(string.Format("{0}", buffer[1])) + 2;
                typestr =CommonClass.ConvertUser10to16(typeid);

                //可变结构动词
                kbjgdc = CommonClass.ConvertUser10to16(buffer[7]);//可变结构定词
                lianxuFlag = CommonClass.getlianxuFlagandDataCount(buffer[7], out dataCount);


                csyy = CommonClass.ConvertUser10to16(buffer, 8, 2);//传输原因
                csyyName = getCsyyName(csyy);

                zhanzhi = CommonClass.ConvertUser10to16(buffer, 10, 2);//站址

                List<DataModel> list = new List<DataModel>();

                #region 总召唤命令
                if (typestr == "64")//总召唤命令
                {
                    if (csyy == "06")//激活
                    {
                        res = "总召唤激活";
                    }
                    else if (csyy == "07")//激活确认
                    {
                        res = "总召唤激活确认";
                    }
                    else if (csyy == "0a")//激活结束
                    {
                        res = "总召唤激活结束";
                    }
                }
                #endregion
                #region 遥信
                #region 单点遥信
                else if (typestr == "01")//不带时标的单点遥信，每个遥信占一个字节
                {
                    if (lianxuFlag)//序列号连续
                    {
                        infoBeginAddress = CommonClass.ConvertUser10to16_Append(buffer, 12, 3);//站址
                        //15
                        if (bufferlen >= dataCount + 14)
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = GetIndex(infoBeginAddress, i);
                                model.Value = CommonClass.ConvertUser10to16(buffer[15 + i]);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 0, "单点遥信(序列号连续)", csyyName);
                        }
                        else
                        {
                            res = "单点遥信(序列号连续)解析报文有误(遥信数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                    else//序号不连续
                    {
                        int c = (bufferlen - 12) / 4;
                        if (c >= dataCount)
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = CommonClass.ConvertUser10to16_Append(buffer, 12+i*4, 3);//站址
                                model.Value = CommonClass.ConvertUser10to16(buffer[15 + i * 4]);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 0, "单点遥信(序列号不连续)", csyyName);
                        }
                        else
                        {
                            res = "单点遥信(序列号不连续)解析报文有误(遥信数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                }
                    #endregion
                #region 双点遥信
                else if (typestr == "03")//双点遥信
                {
                    if (lianxuFlag)//序列号连续
                    {
                        infoBeginAddress = CommonClass.ConvertUser10to16_Append(buffer, 12, 3);//站址
                        //15
                        if (bufferlen >= dataCount + 15)
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = GetIndex(infoBeginAddress, i);
                                model.Value = CommonClass.ConvertUser10to16(buffer[15 + i]);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 0, "双点遥信(序列号连续)", csyyName);
                        }
                        else
                        {
                            res = "双点遥信(序列号连续)解析报文有误(遥信数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                    else//序号不连续
                    {
                        int c = (bufferlen - 12) / 4;
                        if (c >= dataCount)
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = CommonClass.ConvertUser10to16_Append(buffer, 12 + i * 4, 3);//站址
                                model.Value = CommonClass.ConvertUser10to16(buffer[15 + i * 4]);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 0, "双点遥信(序列号不连续)", csyyName);
                        }
                        else
                        {
                            res = "双点遥信(序列号不连续)解析报文有误(遥信数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                }
                #endregion
                #endregion
                #region 遥测
                else if (typestr == "0d")//遥测(序列号连续)
                {
                    if (lianxuFlag)//序列号连续
                    {
                        infoBeginAddress = CommonClass.ConvertUser10to16_Append(buffer, 12, 3);//站址
                        //15
                        if ((bufferlen - 15) / 3 >= dataCount)// && (bufferlen - 15) % 3 == 0
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = GetIndex(infoBeginAddress, i);
                                model.Value = CommonClass.ConvertUser10to16_Append(buffer, 15 + i * 3, 2);
                                model.Quality = CommonClass.ConvertUser10to16(buffer[17 + i * 3]);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 1, "遥测(序列号连续)", csyyName);
                        }
                        else
                        {
                            res = "遥测(序列号连续)解析报文有误(遥信数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                    else//序号不连续
                    {
                        //res = "遥测(序列号连续)解析报文有误(可变结构动词分析错误)";          
                        int c = (bufferlen - 12) / 6;
                        if (c >= dataCount)
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = CommonClass.ConvertUser10to16_Append(buffer, 12 + i * 6, 3);//站址
                                model.Value = CommonClass.ConvertUser10to16_Append(buffer, 15 + i * 6, 2);
                                model.Quality = CommonClass.ConvertUser10to16(buffer[17 + i * 6]);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 1, "遥测2(序列号不连续)", csyyName);
                        }
                        else
                        {
                            res = "遥测2(序列号不连续)解析报文有误(遥信数据个数与获取的数量不一致)" + "1:" + c.ToString() + ";2:" + dataCount.ToString();
                            jxFlag = false;
                        }
                    }
                }
                else if (typestr == "09")//遥测(序列号不连续)
                {
                    if (lianxuFlag)//序列号连续
                    {
                        //res = "遥测(序列号不连续)解析报文有误(可变结构动词分析错误)";     
                        infoBeginAddress = CommonClass.ConvertUser10to16_Append(buffer, 12, 3);//站址
                        //15
                        if ((bufferlen - 15) / 3 >= dataCount)// && (bufferlen - 15) % 3 == 0
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = GetIndex(infoBeginAddress, i);
                                model.Value = CommonClass.ConvertUser10to16_Append(buffer, 15 + i * 3, 2);
                                model.Quality = CommonClass.ConvertUser10to16(buffer[17 + i * 3]);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 1, "遥测1(序列号连续)", csyyName);
                        }
                        else
                        {
                            res = "遥测1(序列号连续)解析报文有误(遥信数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                    else//序号不连续
                    {
                        int c = (bufferlen - 12) / 6;
                        if (c >= dataCount)
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = CommonClass.ConvertUser10to16_Append(buffer, 12 + i * 6, 3);//站址
                                model.Value = CommonClass.ConvertUser10to16_Append(buffer, 15 + i * 6, 2);
                                model.Quality = CommonClass.ConvertUser10to16(buffer[17 + i * 6]);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 1, "遥测(序列号不连续)", csyyName);
                        }
                        else
                        {
                            res = "遥测(序列号不连续)解析报文有误(遥信数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                }
                #endregion

                #region SOE信息
                else if (typestr == "1e")//单点信息
                {
                    if (lianxuFlag)//序列号连续
                    {
                        infoBeginAddress = CommonClass.ConvertUser10to16_Append(buffer, 12, 3);//站址
                        //15
                        if ((bufferlen - 15) / 11 >= dataCount)// && (bufferlen - 15) % 11 == 0
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = GetIndex(infoBeginAddress, i);
                                model.Value = CommonClass.ConvertUser10to16(buffer[15 + i * 11]);

                                string s = CommonClass.ConvertUser10to16_Append(buffer, 16 + i * 11, 2);// 秒 毫秒
                                string minue = CommonClass.ConvertUser10to16(buffer[18 + i * 11]);
                                string houre = CommonClass.ConvertUser10to16(buffer[19 + i * 11]);
                                string day = CommonClass.ConvertUser10to16(buffer[20 + i * 11]);
                                string month = CommonClass.ConvertUser10to16(buffer[21 + i * 11]);
                                string year = CommonClass.ConvertUser10to16(buffer[22 + i * 11]);

                                model.DateStr = string.Format("20{0}-{1}-{2} {3}:{4}:{5}", year, month, day, houre, minue, s);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 2, "单点信息(序列号连续)", csyyName);
                        }
                        else
                        {
                            res = "单点信息(序列号连续)解析报文有误(单点数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                    else//序号不连续
                    {
                        int c = (bufferlen - 12) / 11;
                        if (c >= dataCount)
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = CommonClass.ConvertUser10to16_Append(buffer, 12 + i * 11, 3);//站址
                                model.Value = CommonClass.ConvertUser10to16(buffer[15 + i * 11]);

                                string s = CommonClass.ConvertUser10to16_Append(buffer, 16 + i * 11, 2);// 秒 毫秒
                                string minue = CommonClass.ConvertUser10to16(buffer[18 + i * 11]);
                                string houre = CommonClass.ConvertUser10to16(buffer[19 + i * 11]);
                                string day = CommonClass.ConvertUser10to16(buffer[20 + i * 11]);
                                string month = CommonClass.ConvertUser10to16(buffer[21 + i * 11]);
                                string year = CommonClass.ConvertUser10to16(buffer[22 + i * 11]);

                                model.DateStr = string.Format("20{0}-{1}-{2} {3}:{4}:{5}",year,month,day,houre,minue,s);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 2, "SOE单点信息(序列号不连续)", csyyName);
                        }
                        else
                        {
                            res = "SOE单点信息(序列号不连续)解析报文有误(单点数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                }
                else if (typestr == "1f")//双点信息
                {
                    if (lianxuFlag)//序列号连续
                    {
                        infoBeginAddress = CommonClass.ConvertUser10to16_Append(buffer, 12, 3);//站址
                        //15
                        if ((bufferlen - 15) / 11 >= dataCount)// && (bufferlen - 15) % 11 == 0
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = GetIndex(infoBeginAddress, i);
                                model.Value = CommonClass.ConvertUser10to16(buffer[15 + i * 11]);

                                string s = CommonClass.ConvertUser10to16_Append(buffer, 16 + i * 11, 2);// 秒 毫秒
                                string minue = CommonClass.ConvertUser10to16(buffer[18 + i * 11]);
                                string houre = CommonClass.ConvertUser10to16(buffer[19 + i * 11]);
                                string day = CommonClass.ConvertUser10to16(buffer[20 + i * 11]);
                                string month = CommonClass.ConvertUser10to16(buffer[21 + i * 11]);
                                string year = CommonClass.ConvertUser10to16(buffer[22 + i * 11]);

                                model.DateStr = string.Format("20{0}-{1}-{2} {3}:{4}:{5}", year, month, day, houre, minue, s);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 2, "双点信息(序列号连续)", csyyName);
                        }
                        else
                        {
                            res = "双点信息(序列号连续)解析报文有误(双点数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                    else//序号不连续
                    {
                        int c = (bufferlen - 12) / 11;
                        if (c >= dataCount)
                        {
                            for (int i = 0; i < dataCount; i++)
                            {
                                DataModel model = new DataModel();
                                model.Index = CommonClass.ConvertUser10to16_Append(buffer, 12 + i * 11, 3);//站址
                                model.Value = CommonClass.ConvertUser10to16(buffer[15 + i * 11]);

                                string s = CommonClass.ConvertUser10to16_Append(buffer, 16 + i * 11, 2);// 秒 毫秒
                                string minue = CommonClass.ConvertUser10to16(buffer[18 + i * 11]);
                                string houre = CommonClass.ConvertUser10to16(buffer[19 + i * 11]);
                                string day = CommonClass.ConvertUser10to16(buffer[20 + i * 11]);
                                string month = CommonClass.ConvertUser10to16(buffer[21 + i * 11]);
                                string year = CommonClass.ConvertUser10to16(buffer[22 + i * 11]);

                                model.DateStr = string.Format("20{0}-{1}-{2} {3}:{4}:{5}", year, month, day, houre, minue, s);

                                list.Add(model);
                            }
                            res = ListModelToString(list, 2, "SOE双点信息(序列号不连续)", csyyName);
                        }
                        else
                        {
                            res = "SOE双点信息(序列号不连续)解析报文有误(双点数据个数与获取的数量不一致)";
                            jxFlag = false;
                        }
                    }
                }
                #endregion
            }
            if (buffer.Length == 6)
            {
                res = "首次握手";
            }

            return res;
        }

        private string getCsyyName(string csyytemp)
        {
            string res = csyy;
            switch (csyytemp)
            {
                case "01":
                    res = "周期、循环";
                    break;
                case "02":
                    res = "背景扫描";
                    break;
                case "03":
                    res = "突发";
                    break;
                case "04":
                    res = "初始化";
                    break;
                case "05":
                    res = "请求或被请求";
                    break;
                case "06":
                    res = "激活";
                    break;
                case "07":
                    res = "激活确认";
                    break;
                case "08":
                    res = "停止激活";
                    break;
                case "09":
                    res = "停止激活确认";
                    break;
                case "0a":
                    res = "激活结束";
                    break;
                case "14":
                    res = "响应总召唤";
                    break;
                default:
                    res = "未设置该传送原因:" + csyytemp;
                    break;
            }
            return res;
        }

        private string ListModelToString(List<DataModel> list, int flag, string name,string cs)
        {
            StringBuilder sb = new StringBuilder("");
            if (list.Count > 0)
            {
                sb.AppendFormat("{0} {1}{2}", name,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Environment.NewLine);
                sb.AppendFormat("传送原因：{0}{1}", cs, Environment.NewLine);
            }
            foreach (DataModel item in list)
            {
                sb.AppendFormat("Index:{0},Value:{1}",item.Index,item.Value);
                if (flag == 0)
                {
                    sb.AppendFormat(";{0}", Environment.NewLine);
                }
                else if (flag == 1)
                {
                    sb.AppendFormat(",Quality:{1};{0}", Environment.NewLine, item.Quality);
                }
                else if (flag == 2)
                {
                    sb.AppendFormat(",DateTime:{1};{0}", Environment.NewLine, item.DateStr);
                }
            }
            return sb.ToString();
        }

        private string GetIndex(string address, int i)
        {
            string temp = CommonClass.ConvertUser16to10(byte.Parse("0x" + address));
            int c = int.Parse(temp) + i;
            string res = Convert.ToString(c, 16);
            if (res.Length % 2 == 1)
            {
                res = "0" + res;
            }
            return res;
        }

        private void WriteExecption(Exception ex)
        {
            string filename = "Error" + DateTime.Now.ToString("yyyyMMddHH") + ".txt";
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\log\\";
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            //文件路径
            string filePath = path + filename;
            string strcontent = string.Empty;
            StringBuilder strsb = new StringBuilder();
            strsb.AppendFormat("ExceptionMessage：{0}{1}", ex.Message, Environment.NewLine);
            strsb.AppendFormat("ExceptionSource：{0}{1}", ex.Source, Environment.NewLine);
            strsb.AppendFormat("{0}时间：{2}{1}======================================{1}", "", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //写入文件
            File.AppendAllText(filePath, strsb.ToString());//添加至文件
        }

        private string getbytestr(byte[] temp, int len, bool convert10to16flag)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                if (i < len)
                {
                    byte item = temp[i];
                    try
                    {
                        if (convert10to16flag)
                        {
                            sb.AppendFormat("{0} ", CommonClass.ConvertUser10to16(item));
                        }
                        else
                        {
                            sb.AppendFormat("{0} ", int.Parse(string.Format("{0}", item)));
                        }
                    }
                    catch
                    {
                        sb.AppendFormat("{0} ", item);
                    }
                }
            }
            return sb.ToString();
        }

        private byte[] ConvertUser10to16(byte[] buffer)
        {
            if (buffer != null)
            {
                for(int i=0;i<buffer.Length-1;i++)
                {
                    if (buffer[i] > 0)
                    {
                        int item = int.Parse(string.Format("{0}", buffer[i]));


                        buffer[i] = byte.Parse(Convert.ToString(item, 16));
                    }
                }
                return buffer;
            }
            return null;
        }
        /// <summary>
        /// 服务器向客户端发送消息
        /// </summary>
        /// <param name="str"></param>
        void Send(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            Send(buffer);
        }
        void Send(byte[] buffer)
        {
            socketSend.Send(buffer);
        }

        void ShowMsg(string msg)
        {
            textBox1.AppendText(msg + Environment.NewLine);

            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                port = Int32.Parse(textBox2.Text);
                //点击开始监听时 在服务端创建一个负责监听IP和端口号的Socket
                _SocketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(textBox4.Text);                //创建对象端口
                IPEndPoint point = new IPEndPoint(ip, port);
                _SocketWatch.Bind(point);//绑定端口号
                ShowMsg("启动成功!");
                _SocketWatch.Listen(10000);//设置监听
                //创建监听线程
                Thread thread = new Thread(Listen);
                thread.IsBackground = true;
                thread.Start(_SocketWatch);
            }
            catch(Exception ex) {
                WriteExecption(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ASDUClass clockBuffer = new ASDUClass();
            //clockBuffer.Pack(ASDUClass.TransRes.File, ASDUClass.FunType.FileReady);
            clockBuffer.Pack(textBox3.Text);
            APDUClass apdu = new APDUClass(new APCIClassIFormat(++sr, nr), clockBuffer);


            byte[] byf = new byte[] { 0x68, 0x04, 0x0B, 0x00, 0x00, 0x00 };
            byte[] bytetemp = new byte[] { 0x68, 0x18, 0x12, 0x00, 0x00, 0x00, 0x40, 0x01, 0x0d, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 ,0x00,0x00,0x00,0x56,0x33,0x03,0x0e,0x10,0x09,0x13};
            byte[] bytet = new byte[] { 0x68, 0x16, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x97, 0x00, 0x00, 0x00, 0x98, 0x00, 0x00, 0x00, 0x99 };
            Send(apdu.ToArray());
        }

        private void FrmServer_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_SocketWatch != null)
            {
                if (!_SocketWatch.Connected)
                {
                    return;
                }
                try
                {
                    _SocketWatch.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    WriteExecption(ex);
                }
                try
                {
                    _SocketWatch.Close();
                }
                catch (Exception ex)
                {
                    WriteExecption(ex);
                }
                
            }
        }


    }
}
