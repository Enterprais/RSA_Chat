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
        MainForm mainForm; //форма чата

        string PublicKey; //публичный ключ
        string PrivateKey; //секретный ключ
        UdpClient MyClient; //клиент Udp
        public IPAddress MyIp; //ip адрес данного компьютера
        Task listeningTask; //поток для обработки входящих пакетов
        public string MyName {get; set;} //имя пользователя в чате
        public List<User> UsersList { get; private set; } //список актинвных пользователей

        public ChatControl(MainForm mainForm)
        {
            //создание ключей шифрования
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024);
            PublicKey = RSA.ToXmlString(false);
            PrivateKey = RSA.ToXmlString(true);

            this.mainForm = mainForm;
            UsersList = new List<User>();
            //считывание ip адреса компьютера
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    MyIp = addr;
                    break;
                }
            }
            //создание клиента Upd

            try
            {
                MyClient = new UdpClient(new IPEndPoint(MyIp, 12345));
            }
            catch (Exception)
            {
                MessageBox.Show("Порт приложения уже занят!");
                Application.Exit();
            }
            MyClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            //запуск потока обработки входящих пакетов
            listeningTask = new Task(Listen);
            listeningTask.Start();
        }

        public void SendEnterAlert() //отправка сообщения о входе
        {
            try
            {
                byte[] Mes = Encoding.Unicode.GetBytes("EnterAlert;" + PublicKey + ";" + MyName);
                MyClient.Send(Mes, Mes.Length, new IPEndPoint(IPAddress.Broadcast, 12345));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendExitAlert() //отправка сообщения о выходе
        {
            try
            {
                byte[] Mes = Encoding.Unicode.GetBytes("ExitAlert;" + PublicKey + ";" + MyName);
                MyClient.Send(Mes, Mes.Length, new IPEndPoint(IPAddress.Broadcast, 12345));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendResponse(IPAddress ip) //отправка ответа на сообщение о входе
        {
            try
            {
                byte[] Mes = Encoding.Unicode.GetBytes("ResponceAlert;" + PublicKey + ";" + MyName);
                MyClient.Send(Mes, Mes.Length, new IPEndPoint(IPAddress.Broadcast, 12345));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendMessage(string message, int index) //отправка сообщения из чата
        {
            User usr = UsersList[index];
            byte[] Mes = EncodeMessage(Encoding.Unicode.GetBytes(message), usr.Key);

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

        byte[] DecodeMessage(byte[] mes) //дешифрование сообщения
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024);
            RSA.FromXmlString(PrivateKey);
            return RSA.Decrypt(mes, false);
        }

        byte[] EncodeMessage(byte[] mes, string key) //шифрование сообщения
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024);
            RSA.FromXmlString(key);
            return RSA.Encrypt(mes, false); 
        }

        private void Listen() //чтение входящих сообщений
        {
            try
            {
                while (true)
                {                    
                    StringBuilder Builder = new StringBuilder();             
                    IPEndPoint remoteFullIp = new IPEndPoint(IPAddress.Any, 0);//адрес, с которого пришли данные
                   
                    byte[] data = MyClient.Receive(ref remoteFullIp); //считывание сообщения
                    Builder.Append(Encoding.Unicode.GetString(data));                     

                    if(!remoteFullIp.Address.Equals(MyIp)) //если сообщение пришло не со своего адреса
                    {
                        List<string> message = Builder.ToString().Split(';').ToList(); //делим сообщение по разделителю

                        if (message.Count == 3)
                        {
                            if(message[0] == "EnterAlert") //если оповещение о входе
                            {
#if DEBUG
                                Console.WriteLine(DateTime.Now.ToLongTimeString() +
                                    " Get Enter by " +
                                    remoteFullIp.Address.ToString() + "(" +
                                    message[2] + ")");
#endif
                                EnterHandler(message, remoteFullIp.Address);
                            }
                            else if(message[0] == "ExitAlert") //если оповещение о выходе
                            {
#if DEBUG
                                Console.WriteLine(DateTime.Now.ToLongTimeString() +
                                    " Get Exit by " +
                                    remoteFullIp.Address.ToString() + "(" +
                                    message[2] + ")");
#endif
                                ExitHandler(remoteFullIp.Address);
                            }
                            else if (message[0] == "ResponceAlert") //если ответы на оповещение о входе
                            {
#if DEBUG
                                Console.WriteLine(DateTime.Now.ToLongTimeString() +
                                    " Get Responce by " +
                                    remoteFullIp.Address.ToString() + "(" +
                                    message[2] + ")");
#endif
                                ResponceHandler(message, remoteFullIp.Address);
                            }
                            else //иначе просто сообщение
                            {
                                MessageHandeler(data, remoteFullIp.Address);
                                continue;
                            }                   
                        }
                        else MessageHandeler(data, remoteFullIp.Address);
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
            foreach (User usr in UsersList) //проход по всем пользователям
            {
                if (usr.Address.Equals(ip)) //если пользователь уже есть
                {
                    //обновить ключ и имя
                    usr.UpdateKey(mes[1]); 
                    usr.UpdateName(mes[2]);
                    UserExist = true;
                    return;
                }
            }

            if(!UserExist) //если пользвотеля нет, создать нового
            {
                User temp = new User(mes[2], ip, mes[1]);
                UsersList.Add(temp);
            }

            SendResponse(ip); //отправить ответ на сообщение о входе
            mainForm.Invoke(mainForm.UpdateUsers); //обновить список пользователей
        }

        void ResponceHandler(List<string> mes, IPAddress ip) //обработка ответа на сообщения о входе
        {
            bool UserExist = false;
            foreach (User usr in UsersList) //проход по всем пользователям
            {
                if (usr.Address.Equals(ip)) //если пользователь уже есть
                {
                    //обновить ключ и имя
                    usr.UpdateKey(mes[1]);
                    UserExist = true;
                    return;
                }
            }

            if (!UserExist) //если пользвотеля нет, создать нового
            {
                User temp = new User(mes[2], ip, mes[1]);
                UsersList.Add(temp);
            }
            mainForm.Invoke(mainForm.UpdateUsers); //обновить список пользователей
        }

        void ExitHandler(IPAddress ip) //обработка сообщения о выходе
        {
            User del = null;
            foreach (User usr in UsersList) //проход по всем пользователям
            {
                if (usr.Address.Equals(ip)) //если пользователь есть, запомнить его
                {
                    del = usr;
                    break;
                }
            }
            if (del != null) //если пользователь найден
            {
                if (UsersList.IndexOf(del) == mainForm.CurrentSession) //удалить пользователя
                    mainForm.ClearSession();

                UsersList.Remove(del);
            }
            mainForm.Invoke(mainForm.UpdateUsers); //обновить список пользователей
        } 

        void MessageHandeler(byte[] mes, IPAddress ip) //обработка сообщения из чата
        {
            User user = null;
            foreach (User usr in UsersList) //проход по всем пользователям
            {
                if (usr.Address.Equals(ip)) //если пользователь есть, то запомнить его
                {
                    user = usr;
                    break;
                }
            }
            if (user != null) //если пользователь есть
            {
                user.Session.Messages.Add(new Session.MesField(user.Name, Encoding.Unicode.GetString(DecodeMessage(mes)))); //добавить сообщение в чат
                if (UsersList.IndexOf(user) != mainForm.CurrentSession) //если открыт чат с другим пользователем
                    user.Session.NewMessage(); //прибавить непрочитанное сообщение
                mainForm.UpdateSession(UsersList.IndexOf(user).ToString()); //обновить окно чата
            }
            mainForm.Invoke(mainForm.UpdateUsers); //обновить окно пользователей
        }
    }

    class Session //класс состояния чата с пользователем 
    {
        public List<MesField> Messages { get; private set; } //сообщения
        public uint UnreadMes { get; private set; } //кол-во непрочитанных сообщений

        public Session()
        {
            Messages = new List<MesField>();
            UnreadMes = 0;
        }

        public class MesField //класс компонентов сообщения
        {
            public string user { get; private set; } //имя пользователя отправившего сообщение
            public string mes { get; private set; } //сообщение пользователя

            public MesField(string user, string mes)
            {
                this.user = user;
                this.mes = mes;
            }
        }

        public void NewMessage() { UnreadMes++; } //увеличить счетчик непрочитанных сообщений
        public void ReadMessage() { UnreadMes = 0; } //обнулить счетчик непрочитанных сообщений
    } 

    class User //класс пользователя
    {
        public string Name { get; private set; }  //имя пользователя
        public IPAddress Address { get; private set; } //ip адрес пользователя
        public string Key { get; private set; } //публичный ключ пользователя
        public Session Session { get; private set; } // состоние чата с пользвателем

        public User(string name, IPAddress ip, string key)
        {
            this.Name = name;
            this.Address = ip;
            this.Key = key;
            this.Session = new Session();
        }

        public void UpdateKey(string key) //обновить публичный ключ
        {
            this.Key = key;
        }
        public void UpdateName(string name) //обновить имя
        {
            this.Name = name;
        }
    }

}
