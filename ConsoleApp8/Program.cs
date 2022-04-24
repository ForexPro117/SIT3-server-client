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
                socket.Receive(bytesReceived, sizeof(int), 0);
                messageLength = BitConverter.ToInt32(bytesReceived);
                bytesReceived = new Byte[messageLength];
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