/*
 
    -----------------------
    UDP-Receive (send to)
    -----------------------
    // [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
   
   
    // > receive
    // 127.0.0.1 : 8051
   
    // send
    // nc -u 127.0.0.1 8051
 
*/

using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour {

    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;

	// when game stops, stop receiving data
	Boolean keepReceiving = true;

    // public string IP = "127.0.0.1"; default local
    public int port = 8051;

    // FaceAPI
    public float xPos;
    public float yPos;
    public float zPos;

    public float pitch;
    public float yaw;
    public float roll;

    // start from unity3d
    public void Start()
    {
       init();
    }

    // init
    private void init()
    {
        // set initial values
        xPos = 0;
        yPos = 0;
        zPos = 0;
        pitch = 0;
        yaw = 0;
        roll = 0;

        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        //print("UDPSend.init()");

        // status
        //print("Sending to 127.0.0.1 : " + port);
        //print("Test-Sending to this Port: nc -u 127.0.0.1  " + port + "");


        // ----------------------------
        // Abhören
        // ----------------------------
        // Lokalen Endpunkt definieren (wo Nachrichten empfangen werden).
        // Einen neuen Thread für den Empfang eingehender Nachrichten erstellen.
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);

		while (keepReceiving)
        {
            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                // Bytes mit der UTF8-Kodierung in das Textformat kodieren.
                //string text = Encoding.UTF8.GetString(data);

                // Den abgerufenen Text anzeigen.
                //print(">> " + text);

                // get positions
                for (int i = 0; i < 6; i++)
                {
                    double datum = System.BitConverter.ToDouble(data, i * 8);
                    var j = i + 1;
                    switch (j)
                    {
                        case 1:
                            {
                                xPos = (float)datum;
                                break;
                            }
                        case 2:
                            {
                                yPos = (float)datum;
                                break;
                            }
                        case 3:
                            {
                                zPos = (float)datum;
                                break;
                            }
                        case 4:
                            {
                                yaw = (float)datum;
                                break;
                            }
                        case 5:
                            {
                                pitch = (float)datum;
                                break;
                            }
                        case 6:
                            {
                                roll = (float)datum;
                                break;
                            }
                    }
                }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

	/**
	 * Kill Thread
	 */
	public void OnDestroy() 
	{
		keepReceiving = false;
		client.Close ();
		receiveThread.Abort();
	}
}