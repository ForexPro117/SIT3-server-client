using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class ConnectForm : Form
    {

        private string _ip;
        private int _port;


        public ConnectForm()
        {
            InitializeComponent();
            Random rd=new Random();
           nicname.Text += rd.Next(1, 999);
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
            this._ip = IPBox.Text;
            Int32.TryParse(PortBox.Text, out this._port);

            try
            {
                label2.Text = "Статус: Проверка";
                Socket socket = ConnectSocket(_ip, _port);

                if (socket == null)
                {
                    label2.Text = "Статус: Ошибка соединения";
                    socket.Close();
                    return;
                }
                label2.Text = "Статус: Успех";
                this.Hide();
                Form1 messageForm = new Form1(socket, nicname.Text);
                messageForm.Owner = this;
                messageForm.ShowDialog();
                this.Show();
                socket.Close();


            }
            catch (SocketException)
            {
                label2.Text = "Статус: Сервер недоступен";

            }
            this.Enabled = true;

        }

    }
}
