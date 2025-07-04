using System.Net.Sockets;
using System.Text;
using UnityEngine;
using extOSC;

public class ARFaceSender : MonoBehaviour
{
    public static ARFaceSender Instance;
    private OSCTransmitter transmitter;

    private string ip;
    private int port;
    public bool isStart { get; private set; } = false;

    private UdpClient client;

    void Awake()
    {
        Instance = this;

        transmitter = gameObject.AddComponent<OSCTransmitter>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void SendData(byte[] data)
    {
        if (!isStart)
            return;

        client.Send(data, data.Length, ip, port);
    }

    public void SendData(OSCMessage message)
    {
        if (!isStart)
            return;

        transmitter.Send(message);
    }

    public void StartSendData(string ipAddress, int port)
    {
        // 중복 실행 예외 처리
        if (isStart)
        {
            Debug.Log("이미 실행 중입니다.");
            return;
        }


        this.ip = ipAddress;
        this.port = port;

        transmitter.RemoteHost = ip;
        transmitter.RemotePort = port;

        client = new UdpClient();
        isStart = true;

        Debug.Log($"UDP 데이터 전송 시작 : {ip}:{port}");
    }

    public void StopSendData()
    {
        // 중복 실행 예외 처리
        if (!isStart)
            return;

        isStart = false;
        client.Close();
    }

    void OnApplicationQuit()
    {
        client?.Close();
        isStart = false;
    }
}
