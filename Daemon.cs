using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using System.Text;

namespace Daemon
{
    public class Daemon
    {
        public Daemon(Logger logger)
        {
            Task.Run(() => {
                int port = 8888;

                IPEndPoint ipAdd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                TcpListener listener = new TcpListener(ipAdd);
                listener.Start();
                logger.Info("Listening port " + port);

                TcpClient client = listener.AcceptTcpClient();
                logger.Info("Connected");

                if (client.Connected)
                {
                    listener.Stop();
                    NetworkStream netStream = client.GetStream();
                    StreamReader sReader = new StreamReader(netStream, Encoding.UTF8);

                    string str = String.Empty;

                    do
                    {
                        str = sReader.ReadLine();
                        if (null == str)
                        {
                            break;
                        }
                        logger.Info(str);
                    } while (!str.Equals("bye"));

                    sReader.Close();
                    client.Close();
                }
            });
        }
    }
}
