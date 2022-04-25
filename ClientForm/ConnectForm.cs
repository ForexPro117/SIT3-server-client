using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class ConnectForm : Form
    {

        private string _username;
        private string _ip;
        private int _port;

        public ConnectForm()
        {
            InitializeComponent();
            Form1 messageForm = new Form1();
        }
        private static Socket ConnectSocket(string ip, int port)
        {

            IPAddress address = IPAddress.Parse(ip);

            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket =
                new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            tempSocket.Connect(ipe);
            return tempSocket;
        }
        private void checkButton_Click(object sender, EventArgs e)
        {
            this.Text = "Статус: проверка";
            /*this._username=nicname.Text;
            this._ip = IPBox.Text;
            this._ip=PortBox.Text;*/
        }

    }
}
