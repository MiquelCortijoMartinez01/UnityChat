using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSocketIO : MonoBehaviour
{
    public TMP_InputField input;
    public Transform otherTransform;

    private SocketIOUnity _socket;
    private const string ServerUrlLink = "http://10.40.2.65:3000";
    // Start is called before the first frame update
    void Start()
    {
        Uri uri = new Uri(ServerUrlLink);
        _socket = new SocketIOUnity(uri);

        _socket.OnConnected += (sender, e) =>{
            Debug.Log("Socket Connected");
            _socket.Emit("createRoom");
        };

        _socket.On("enterToRoom", response =>
        {
            Debug.Log("Enter to room: " + response.ToString());
        });
        _socket.On("chatMessage", response =>
        {
            Debug.Log("Message: " + response.ToString());
            UnityThread.executeInUpdate(() =>
            {
                otherTransform.position += otherTransform.forward;
            });
        });
        _socket.Connect();
    }

    public void SendMessage()
    {
        string text = input.text;

        if(text != "")
        {
            _socket.Emit("chatMessage", text);
        }

        input.text = "";
    }
}
