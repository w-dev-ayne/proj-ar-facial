using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ARFaceReceiver : MonoBehaviour
{
    UdpClient udpClient;
    Thread receiveThread;
    public int port = 49983;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        udpClient = new UdpClient(port);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        Debug.Log($"UDP Receiver started on port {port}");
    }

    private void ReceiveData()
    {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, port);
        while (true)
        {
            byte[] data = udpClient.Receive(ref remoteEP);
            string message = Encoding.UTF8.GetString(data);
            Debug.Log($"Received : {message}");
        
        }
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
        receiveThread.Abort();
    }
}
