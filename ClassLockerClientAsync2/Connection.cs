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
    class Connection
    {
        private Socket clientSock;  /* client Socket */
        private Socket cbSock;   /* client Async Callback Socket */
        private byte[] recvBuffer;
        MainWindow mainW;

        
        public Connection(MainWindow mainW, Socket clientSock, Socket cbSock, byte[] recvBuffer)
        {
            this.mainW = mainW;
            this.clientSock = clientSock;
            this.cbSock = cbSock;
            this.recvBuffer = recvBuffer;
        }
        public Connection()
        { 
        }
        public void BeginConnect()
        {
            mainW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
            {
                mainW.Message.Text = "서버 접속 대기 중";
            }));

            try
            {
                clientSock.BeginConnect(mainW.getHost(), mainW.getPort(), new AsyncCallback(ConnectCallBack), clientSock);
            }
            catch (SocketException se)
            {
                /*서버 접속 실패 */

                mainW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                {
                    mainW.Message.Text += "\r\n서버접속 실패하였습니다. " + se.NativeErrorCode;
                }));

                mainW.DoInit();
            }

        }
        private void ConnectCallBack(IAsyncResult IAR)
        {
            try
            {
                // 보류중인 연결을 완성
                Socket tempSock = (Socket)IAR.AsyncState;
                IPEndPoint svrEP = (IPEndPoint)tempSock.RemoteEndPoint;

                mainW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                {
                    mainW.Message.Text += "\r\n 서버로 접속 성공 : " + svrEP.Address;
                    BeginSend("C");
                }));


                tempSock.EndConnect(IAR);
                cbSock = tempSock;
                cbSock.BeginReceive(this.recvBuffer, 0, recvBuffer.Length, SocketFlags.None,
                                    new AsyncCallback(OnReceiveCallBack), cbSock);
            }
            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.NotConnected)
                {
                    mainW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                    {
                        mainW.Message.Text += "\r\n서버 접속 실패 CallBack " + se.Message;
                    }));
                    this.BeginConnect();
                }
            }
        }

        public void BeginSend(string message)
        {
            try
            {
                /* 연결 성공시 */
                if (clientSock.Connected)
                {
                    byte[] buffer = new UTF8Encoding().GetBytes(message);
                    clientSock.BeginSend(buffer, 0, buffer.Length, SocketFlags.None,
                                          new AsyncCallback(SendCallBack), message);
                }
            }
            catch (SocketException e)
            {
                mainW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                {
                    mainW.Message.Text = "\r\n전송 에러 : " + e.Message;
                }));
            }
        }
        private void SendCallBack(IAsyncResult IAR)
        {
            string message = (string)IAR.AsyncState;
            mainW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
            {
                mainW.Message.Text += "\r\n전송 완료 CallBack : " + message;
            }));

        }

        public void Receive()
        {
            cbSock.BeginReceive(this.recvBuffer, 0, recvBuffer.Length, SocketFlags.None,
                                 new AsyncCallback(OnReceiveCallBack), cbSock);
        }
        public void ReceiveStudentData(string message)
        {
            mainW.Message.Text += "receiving student data\r\n";

        }
        private void OnReceiveCallBack(IAsyncResult IAR)
        {
            try
            {
                Socket tempSock = (Socket)IAR.AsyncState;
                int nReadSize = tempSock.EndReceive(IAR);
                if (nReadSize != 0)
                {
                    string message = new UTF8Encoding().GetString(recvBuffer, 0, nReadSize);
                    mainW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                    {
                        mainW.Message.Text += "\r\n서버로 데이터 수신 : " + message;
                    }));
                    if (message.StartsWith("O"))
                    {
                        ReceiveStudentData(message);
                    }
                }
                this.Receive();
            }
            catch (SocketException se)
            {
                if (se.SocketErrorCode == SocketError.ConnectionReset)
                {
                    BeginConnect();
                }
            }
        }
    }
}

