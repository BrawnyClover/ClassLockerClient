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
        Connection connector;
        public MainWindow()
        {
            InitializeComponent();
            recvBuffer = new byte[MAXSIZE];
            clientSock = new Socket(AddressFamily.InterNetwork,
                                          SocketType.Stream, ProtocolType.Tcp);
            connector = new Connection(this, clientSock, cbSock, recvBuffer);
            this.DoInit();
        }
        public string getHost()
        {
            return this.HOST;
        }
        public int getPort()
        {
            return this.PORT;
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
        }
    }
}
