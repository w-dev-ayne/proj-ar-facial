using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ARFaceReceiver : MonoBehaviour
{
    UdpClient udpClient;
    Thread receiveThread;
    private string receivingMessage;
    public int port = 49983;

    public FaceDataReceiveTest test;

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
            receivingMessage = Encoding.UTF8.GetString(data);
            Debug.Log($"Received : {receivingMessage}");
        }
    }

    void Update()
    {
        if (receivingMessage == String.Empty)
            return;

        test.SetRightEyeTransform(ExtractVector3(receivingMessage, "rightEye#"));
        test.SetLeftEyeTransform(ExtractVector3(receivingMessage, "leftEye#"));   
    }

    void OnApplicationQuit()
    {
        udpClient.Close();
        receiveThread.Abort();
    }

    Vector3 ExtractVector3(string data, string key)
    {
        int start = data.IndexOf(key);
        if (start == -1)
        {
            Debug.LogError($"Key not found: {key}");
            return Vector3.zero;
        }

        start += key.Length;
        int end = data.IndexOf('|', start);
        if (end == -1) end = data.Length;

        string vectorData = data.Substring(start, end - start);
        string[] parts = vectorData.Split(',');

        if (parts.Length != 3)
        {
            Debug.LogError($"Invalid vector data for {key}: {vectorData}");
            return Vector3.zero;
        }

        if (float.TryParse(parts[0], out float x) &&
            float.TryParse(parts[1], out float y) &&
            float.TryParse(parts[2], out float z))
        {
            return new Vector3(-x, -y, z);
        }

        Debug.LogError($"Failed to parse float for {key}: {vectorData}");
        return Vector3.zero;
    }
}
