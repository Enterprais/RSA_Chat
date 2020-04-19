using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace RSA_Chat
{
    class ChatControl
    {
        MainForm mainForm;

        string PublicKey;
        string PrivateKey;
        UdpClient MyClient;
        public IPAddress MyIp;
        Task listeningTask;


        public List<User> UsersList { get; private set; }

        public string MyName {get; set;}

        public ChatControl(MainForm mainForm)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024);
            PublicKey = RSA.ToXmlString(false);
            PrivateKey = RSA.ToXmlString(true);

            this.mainForm = mainForm;
            UsersList = new List<User>();

            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    MyIp = addr;
                    break;
                }
            }
            MyClient = new UdpClient(new IPEndPoint(MyIp, 12345));
            MyClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            listeningTask = new Task(Listen);
            listeningTask.Start();
        }

        public void SendEnterAlert()
        {
            try
            {
                byte[] Mes = Encoding.ASCII.GetBytes("EnterAlert;" + PublicKey + ";" + MyName);
                MyClient.Send(Mes, Mes.Length, new IPEndPoint(IPAddress.Broadcast, 12345));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendExitAlert()
        {
            try
            {
                byte[] Mes = Encoding.ASCII.GetBytes("ExitAlert;" + PublicKey + ";" + MyName);
                MyClient.Send(Mes, Mes.Length, new IPEndPoint(IPAddress.Broadcast, 12345));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendResponse(IPAddress ip)
        {
            try
            {
                byte[] Mes = Encoding.ASCII.GetBytes("ResponceAlert;" + PublicKey + ";" + MyName);
                MyClient.Send(Mes, Mes.Length, new IPEndPoint(IPAddress.Broadcast, 12345));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendMessage(string message, int index)
        {
            User usr = UsersList[index];
            byte[] Mes = EncodeMessage(Encoding.ASCII.GetBytes(message), usr.Key);

            try
            {
                MyClient.Send(Mes, Mes.Length, new IPEndPoint(usr.Address, 12345));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            usr.Session.Messages.Add(new Session.MesField(MyName, message));
        }

        byte[] DecodeMessage(byte[] mes)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024);
            RSA.FromXmlString(PrivateKey);
            return RSA.Decrypt(mes, false);
        }

        byte[] EncodeMessage(byte[] mes, string key)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024);
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
                    //адрес, с которого пришли данные
                    IPEndPoint RemoteIp = new IPEndPoint(IPAddress.Any, 0);
                   
                    byte[] data = MyClient.Receive(ref RemoteIp);
                    Builder.Append(Encoding.ASCII.GetString(data));                     

                    // получаем данные о подключении
                    IPEndPoint remoteFullIp = RemoteIp as IPEndPoint;

                    if(!remoteFullIp.Address.Equals(MyIp))
                    {
                        //mainForm.richTextBox_mess.Text += Builder.ToString() + "\n";
                        List<string> message = Builder.ToString().Split(';').ToList();

                        if (message.Count == 3)
                        {
                            if(message[0] == "EnterAlert")
                            {
#if DEBUG

                                Console.WriteLine(DateTime.Now.ToLongTimeString() +
                                    " Get Enter by " +
                                    remoteFullIp.Address.ToString() + "(" +
                                    message[2] + ")");
#endif
                                EnterHandler(message, remoteFullIp.Address);
                            }
                            else if(message[0] == "ExitAlert")
                            {
#if DEBUG
                                Console.WriteLine(DateTime.Now.ToLongTimeString() +
                                    " Get Exit by " +
                                    remoteFullIp.Address.ToString() + "(" +
                                    message[2] + ")");
#endif
                                ExitHandler(remoteFullIp.Address);
                            }
                            else if (message[0] == "ResponceAlert")
                            {
#if DEBUG
                                Console.WriteLine(DateTime.Now.ToLongTimeString() +
                                    " Get Responce by " +
                                    remoteFullIp.Address.ToString() + "(" +
                                    message[2] + ")");
#endif
                                ResponceHandler(message, remoteFullIp.Address);
                            }
                            else
                            {
                                MessageHandeler(data, remoteFullIp.Address);
                                continue;
                            }                   
                        }
                        else
                            MessageHandeler(data, remoteFullIp.Address);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        void EnterHandler(List<string> mes, IPAddress ip) //обработка сообщения о входе
        {         
            bool UserExist = false;
            foreach (User usr in UsersList)
            {
                if (usr.Address.Equals(ip))
                {
                    usr.UpdateKey(mes[1]);
                    usr.UpdateName(mes[2]);
                    UserExist = true;
                    return;
                }
            }

            if(!UserExist)
            {
                User temp = new User(mes[2], ip, mes[1]);
                UsersList.Add(temp);
            }

            SendResponse(ip);
            mainForm.Invoke(mainForm.UpdateUsers);
        }

        void ResponceHandler(List<string> mes, IPAddress ip) //обработка сообщения о входе
        {
            bool UserExist = false;
            foreach (User usr in UsersList)
            {
                if (usr.Address.Equals(ip))
                {
                    usr.UpdateKey(mes[1]);
                    UserExist = true;
                    return;
                }
            }

            if (!UserExist)
            {
                User temp = new User(mes[2], ip, mes[1]);
                UsersList.Add(temp);
            }

            mainForm.Invoke(mainForm.UpdateUsers);
        }



        void ExitHandler(IPAddress ip) //обработка сообщения о выходе
        {
            User del = null;
            foreach (User usr in UsersList)
            {
                if (usr.Address.Equals(ip))
                {
                    del = usr;
                    break;
                }
            }
            if (del != null)
            {
                if (UsersList.IndexOf(del) == mainForm.CurrentSession)
                    mainForm.ClearSession();

                UsersList.Remove(del);
            }
            mainForm.Invoke(mainForm.UpdateUsers);
        } 

        void MessageHandeler(byte[] mes, IPAddress ip)
        {
            User user = null;
            foreach (User usr in UsersList)
            {
                if (usr.Address.Equals(ip))
                {
                    user = usr;
                    break;
                }
            }
            if (user != null)
            {
                user.Session.Messages.Add(new Session.MesField(user.Name, Encoding.ASCII.GetString(DecodeMessage(mes))));
                if (UsersList.IndexOf(user) != mainForm.CurrentSession)
                    user.Session.NewMessage();
                mainForm.UpdateSession(UsersList.IndexOf(user).ToString());
            }
            mainForm.Invoke(mainForm.UpdateUsers);
        }
    }

    class Session
    {
        public List<MesField> Messages { get; private set; }
        public uint UnreadMes { get; private set; }

        public Session()
        {
            Messages = new List<MesField>();
            UnreadMes = 0;
        }

        public class MesField
        {
            public string user { get; private set; }
            public string mes { get; private set; }

            public MesField(string user, string mes)
            {
                this.user = user;
                this.mes = mes;
            }
        }

        public void NewMessage() { UnreadMes++; }
        public void ReadMessage() { UnreadMes = 0; }
    }

    class User
    {
        public string Name {get; private set; }
        public IPAddress Address { get; private set; }
        public string Key { get; private set; }
        public Session Session { get; private set; }

        public User(string name, IPAddress ip, string key)
        {
            this.Name = name;
            this.Address = ip;
            this.Key = key;
            this.Session = new Session();
        }

        public void UpdateKey(string key)
        {
            this.Key = key;
        }
        public void UpdateName(string name)
        {
            this.Name = name;
        }
    }

}
