using System.Net;
using System.Net.Sockets;
using System.Text;

public class GetSocket
{
    private static Socket ConnectSocket(string server, int port)
    {

        IPAddress address = IPAddress.Parse("127.0.0.1");

        IPEndPoint ipe = new IPEndPoint(address, 1111);
        Socket tempSocket =
            new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        tempSocket.Connect(ipe);
        return tempSocket;
    }
    private static void SendMessage(Socket socket)
    {
        Byte[] bytesSend;
        string message;
        while (true)
        {
            message = Console.ReadLine();
            bytesSend = Encoding.UTF8.GetBytes(message + '\0');
            socket.Send(BitConverter.GetBytes(bytesSend.Length), sizeof(int), 0);
            socket.Send(bytesSend, bytesSend.Length, 0);

        }
    }

    // This method requests the home page content for the specified server.
    public static string SocketSendReceive(string server, int port)
    {
        // Create a socket connection with the specified server and port.
        using (Socket socket = ConnectSocket(server, port))
        {

            if (socket == null)
                return ("Connection failed");

            Task.Run(() => SendMessage(socket));

            Byte[] bytesReceived;
            string message = "";
            int messageLength;

            while (true)
            {
                bytesReceived = new Byte[4];
                socket.Receive(bytesReceived,sizeof(int), 0);
               messageLength = BitConverter.ToInt32(bytesReceived);
              // messageLength = Int32.Parse(Encoding.ASCII.GetString(bytesReceived, 0, 4));
               bytesReceived =new Byte[messageLength];
                socket.Receive(bytesReceived, messageLength, 0);

                
                message = Encoding.UTF8.GetString(bytesReceived, 0, messageLength);

                if (message == "exit")
                    return message;
                Console.WriteLine(message);
            }

        }

    }
}
class Program
{
    public static void Main()
    {

        GetSocket.SocketSendReceive("127.0.0.1", 1111);
        Console.WriteLine("\n Press Enter to continue...");
        Console.Read();
    }




    static void Connect()
    {
        try
        {
            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer
            // connected to the same address as specified by the server, port
            // combination.
            string message = "Hello";
            Int32 port = 1111;
            TcpClient client = new TcpClient("127.0.0.1", port);

            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            NetworkStream stream = client.GetStream();

            //// Send the message to the connected TcpServer.
            //stream.Write(data, 0, data.Length);

            //Console.WriteLine("Sent: {0}", message);

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            data = new Byte[256];
            int index;
            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            while (true)
            {
                Int32 bytes = stream.Read(data, 0, data.Length);

                index = Array.IndexOf(data, (Byte)0);
                if (index == -1)
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                else
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, index);

                if (responseData == "exit")
                {
                    Console.WriteLine("End of work");
                    break;
                }
                Console.WriteLine("Received: {0}", responseData);
            }

            // Close everything.
            stream.Close();
            client.Close();
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("Установленно соединение с сервером");
        }
        catch (IOException e)
        {
            Console.WriteLine("Сервер прекратил свою работу!");
        }

        Console.WriteLine("\n Press Enter to continue...");
        Console.Read();
    }
}