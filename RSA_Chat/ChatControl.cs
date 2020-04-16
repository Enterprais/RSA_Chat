using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;

namespace RSA_Chat
{
    class ChatControl
    {
        MainForm mainForm;

        string PublicKey;
        string PrivateKey;
        Socket MySocket;
        public IPAddress MyIp;
        Task listeningTask;

        public string MyName {get; set;}

        public ChatControl(MainForm mainForm)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            PublicKey = RSA.ToXmlString(true);
            PrivateKey = RSA.ToXmlString(false);
            this.mainForm = mainForm;

            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    MyIp = addr;
                    break;
                }
            }
            MySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            MySocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            MySocket.Bind(new IPEndPoint(MyIp, 12345));
            listeningTask = new Task(Listen);
            listeningTask.Start();
        }

        public void SendEnterAlert()
        {
            try
            {
                string Mes = PublicKey + ";" + MyName;
                MySocket.SendTo(Encoding.ASCII.GetBytes(Mes), new IPEndPoint(IPAddress.Broadcast, 12345));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        byte[] DecodeMessage(byte[] mes)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(PrivateKey);
            return RSA.Decrypt(mes, false);
        }

        byte[] EncodeMessage(byte[] mes, string key)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(key);
            return RSA.Encrypt(mes, false);
        }

        private void Listen()
        {
            try
            {
                while (true)
                {
                    // получаем сообщение
                    StringBuilder Builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[4096]; // буфер для получаемых данных

                    //адрес, с которого пришли данные
                    EndPoint RemoteIp = new IPEndPoint(IPAddress.Any, 0);
                    
                    do
                    {
                        bytes = MySocket.ReceiveFrom(data, ref RemoteIp);
                        Builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                       
                    }
                    while (MySocket.Available > 0);

                    // получаем данные о подключении
                    IPEndPoint remoteFullIp = RemoteIp as IPEndPoint;

                    // выводим сообщение
                    mainForm.richTextBox_mess.Text += Builder.ToString();
                                                    //remoteFullIp.Port, Builder.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    class Session
    {
        User User;
        Dictionary<string, string> Messages;
        uint UnreadMes;     

        public Session(User user)
        {
            this.User = user;
            Messages = new Dictionary<string, string>();
            UnreadMes = 0;
        }
    }

    class User
    {
        public string Name {get; private set; }
        public IPAddress Address { get; private set; }
        public string Key { get; private set; }

        public User(string name, IPAddress ip, string key)
        {
            this.Name = name;
            this.Address = ip;
            this.Key = key;
        }
    }

}
