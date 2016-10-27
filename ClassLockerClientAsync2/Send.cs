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
    class Sender
    {
        private Socket clientSock;  /* client Socket */
        private Socket cbSock;   /* client Async Callback Socket */
        private byte[] recvBuffer;
        MainWindow mainW;
        
        public Sender(MainWindow mainW, Socket clientSock, Socket cbSock, byte[] recvBuffer)
        {
            this.mainW = mainW;
            this.clientSock = clientSock;
            this.cbSock = cbSock;
            this.recvBuffer = recvBuffer;
        }
       
}
