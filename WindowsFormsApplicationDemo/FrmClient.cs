using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplicationClientDemo
{
    public partial class FrmClient : Form
    {
        Socket socketSend;
        string ipaddress = "192.168.1.146";
        int port = 2404;
        public FrmClient()
        {
            InitializeComponent();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {


            try
            {

                string ipstr = txtipstr.Text;
                if (string.IsNullOrEmpty(ipstr))
                {
                    ipstr = ipaddress;
                }

                //创建客户端Socket，获得远程ip和端口号
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(ipstr);
                IPEndPoint point = new IPEndPoint(ip, port);

                socketSend.Connect(point);
                ShowMsg("连接成功!");
                //开启新的线程，不停的接收服务器发来的消息
                Thread c_thread = new Thread(Received);
                c_thread.IsBackground = true;
                c_thread.Start();
            }
            catch (Exception)
            {
                ShowMsg("IP或者端口号错误...");
            }
        }

        void ShowMsg(string str)
        {



            //Dispatcher.Invoke(() => { ListConnet.AppendText(str + "\r\n"); });
        }
        /// <summary>
        /// 接收服务端返回的消息
        /// </summary>
        void Received()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024 * 3];
                    //实际接收到的有效字节数
                    int len = socketSend.Receive(buffer);
                    if (len == 0)
                    {
                        continue;
                    }
                    string str = Encoding.UTF8.GetString(buffer, 0, len);
                    ShowMsg("接收到的服务端数据：" + socketSend.RemoteEndPoint + ":" + str);
                }
                catch
                {



                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = "发送时间："+DateTime.Now.ToString();
                byte[] buffer = new byte[1024 * 1024 * 3];
                buffer = Encoding.UTF8.GetBytes(msg);
                socketSend.Send(buffer);
            }
            catch { }
        }

        private void btnFrist_Click(object sender, EventArgs e)
        {
            string msg = "680407000000";
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            socketSend.Send(buffer);
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            //0x68 0x0A 0x04 0x00 0x00 0x00 
            byte[] buffer = new byte[] { 0x68, 0x0A, 0x04, 0x00, 0x00, 0x00 };
            string str = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            string t = string.Format("{0},{1}", HexChar2Value(string.Format("{0}", 68)), HexChar2Value(string.Format("{0}", 'A')),
                HexChar2Value(string.Format("{0}", 4)), HexChar2Value(string.Format("{0}", 0)), 
                HexChar2Value(string.Format("{0}", 0)), HexChar2Value(string.Format("{0}", 0)));





        }

        /// <summary>
        /// 十六进制转换到十进制
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string Hex2Ten(string hex)
        {
            int ten = 0;
            for (int i = 0, j = hex.Length - 1; i < hex.Length; i++)
            {
                ten += HexChar2Value(hex.Substring(i, 1)) * ((int)Math.Pow(16, j));
                j--;
            }
            return ten.ToString();
        }

        public static int HexChar2Value(string hexChar)
        {
            switch (hexChar)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    return Convert.ToInt32(hexChar);
                case "a":
                case "A":
                    return 10;
                case "b":
                case "B":
                    return 11;
                case "c":
                case "C":
                    return 12;
                case "d":
                case "D":
                    return 13;
                case "e":
                case "E":
                    return 14;
                case "f":
                case "F":
                    return 15;
                default:
                    return 0;
            }
        }
//this.txtStartShi.Text = Hex2Ten(this.txtStartSN.Text.Trim().Substring(4, 8));
//this.txtEndShi.Text = Hex2Ten(this.txtEndSN.Text.Trim().Substring(4, 8));


    }
}
