using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ARFaceSender : MonoBehaviour
{
    public static ARFaceSender Instance;

    private string ip;
    private int port;
    private bool isStart = false;

    private UdpClient client;

    void Awake()
    {
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart)
            return;

        var blendShapeKey = "jawOpen";
        float blendShapeValue = Mathf.Abs(Mathf.Sin(Time.time)) + 0.9f;

        string message = $"/blendshapes/{blendShapeKey} {blendShapeValue}";
        byte[] data = Encoding.UTF8.GetBytes(message);

        client.Send(data, data.Length, ip, port);
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
