using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class Form1 : Form
    {
        private static Socket _socket;
        private string Nickname;
        public Form1(Socket socket, string name)
        {
            Nickname = name;
            _socket = socket;
            InitializeComponent();
        }

        internal string MessageReceive()
        {
            Byte[] bytesReceived;
            int messageLength;


            try
            {
                bytesReceived = new Byte[4];
                _socket.Receive(bytesReceived, sizeof(int), 0);
                messageLength = BitConverter.ToInt32(bytesReceived, 0);
                bytesReceived = new Byte[messageLength];
                _socket.Receive(bytesReceived, messageLength, 0);
                return Encoding.UTF8.GetString(bytesReceived, 0, messageLength);
            }
            catch (SocketException)
            {

                return "";
            }
            catch (ObjectDisposedException)
            {

                return "";
            }


        }

        private void sendMessage_Click(object sender, EventArgs e)
        {

            if (messageBox.Text != "")
            {
                Byte[] bytesSend;
                string message = $"{DateTime.Now:t} {Nickname}: " + messageBox.Text + "\n";
                bytesSend = Encoding.UTF8.GetBytes(message + '\0');
                _socket.Send(BitConverter.GetBytes(bytesSend.Length), sizeof(int), 0);
                _socket.Send(bytesSend, bytesSend.Length, 0);
                TextBox.Text += $"{DateTime.Now:t} Вы: " + messageBox.Text + "\n";
                messageBox.Text = null;
            }

        }
        private async void Form1_Load(object sender, EventArgs e)
        {

            TextBox.Text = $"{null,-35}Соединение установленно!\n\n";


            while (true)
            {
                string message = await Task.Run(() => MessageReceive());
                if (message == "")
                {
                    TextBox.Text += $"\n{null,-40}Ошибка соединения!";
                    sendButton.Enabled = false;
                    messageBox.Enabled=false;
                    break;
                }
                TextBox.Text += message;
                
            }

        }


    }
}
