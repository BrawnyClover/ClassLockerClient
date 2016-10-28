using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClassLockerClientAsync2
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        int nowCondition = 0;
        private Socket clientSock;  /* client Socket */
        private Socket cbSock;   /* client Async Callback Socket */
        private byte[] recvBuffer;

        private const int MAXSIZE = 4096;   /* 4096  */
        private string HOST = "52.78.141.159";
        private int PORT = 9999;
        private string id = "";
        private string passwd = "";
        Connection connector;
        Login loginPage;
        WindowSize windowSizeControl;
        Image img = new Image();
        bool turn = false;

        public MainWindow()
        {
            InitializeComponent();
            changeIcon();
            Condition.TextAlignment = TextAlignment.Center;
            windowSizeControl = new WindowSize(this);
            loginPage = new Login(this);
            recvBuffer = new byte[MAXSIZE];
            clientSock = new Socket(AddressFamily.InterNetwork,
                                          SocketType.Stream, ProtocolType.Tcp);
            connector = new Connection(this, clientSock, cbSock, recvBuffer);
            this.DoInit();

            loginPage.Show();
        }
        public string getHost()
        {
            return this.HOST;
        }
        public int getPort()
        {
            return this.PORT;
        }
        public void setId(string id)
        {
            this.id = id;
        }
        public void setPswd(string passwd)
        {
            this.passwd = passwd;
        }

        public void login()
        {
            String sendText = "L";
            int len = 10000 + id.Length;
            sendText += len.ToString().Substring(1, 4);
            sendText += id;
            len = 10000 + passwd.Length;
            sendText += len.ToString().Substring(1, 4);
            sendText += passwd;
            connector.BeginSend(sendText);
            cbSock.Receive(recvBuffer);
            if (recvBuffer.ToString() == "S")
            {
                loginPage.Close();
            }
            else
            {
                loginPage.status.Text = "Login Failed...";
            }
        }

        public void DoInit()
        {
            clientSock = new Socket(AddressFamily.InterNetwork,
                                          SocketType.Stream, ProtocolType.Tcp);
            connector.BeginConnect();
        }

        private void LockerButton_Click(object sender, RoutedEventArgs e)
        {
            char[] condition = {'B'};
            connector.BeginSend(condition[nowCondition].ToString());
            //Receive();  
            changeIcon();
        }
        public void changeIcon()
        {
            Button btn = LockerButton;
             
            if (turn == true)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                Uri u = new Uri("C:\\Users\\sonbi\\Desktop\\ClassLockerClientAsync2\\padlock.png", UriKind.RelativeOrAbsolute);
                bmp.UriSource = u;
                ImageBrush i = new ImageBrush();
                i.ImageSource = bmp;
                btn.Background = i;
                bmp.EndInit();
            }
            else
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                Uri u = new Uri("C:\\Users\\sonbi\\Desktop\\ClassLockerClientAsync2\\unlock.png", UriKind.RelativeOrAbsolute);
                bmp.UriSource = u;
                ImageBrush i = new ImageBrush();
                i.ImageSource = bmp;
                btn.Background = i;
                bmp.EndInit();
            }
            turn = !turn;
        }
    }
}
