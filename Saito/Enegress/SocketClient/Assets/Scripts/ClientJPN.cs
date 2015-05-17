#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using UnityEngine;
using SocketIO;
using MiniJSON;

public class ClientJPN : MonoBehaviour
{
    private SocketIOComponent socket;
    private GameObject barsetter;
    private GameObject maincam;
    private GameObject JPNObject;

    public void Start()
    {
        barsetter = GameObject.Find("BarSetter");
        JPNObject = GameObject.Find("JAPAN");
        JPNObject.transform.position = new Vector3(34,-0.3f,-135);
        maincam = GameObject.Find("Main Camera");
        maincam.SendMessage("DetectPart", "JAPAN");
        maincam.SendMessage("SetDistance", 12.0f);


        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        socket.On("open", TestOpen);
        //socket.On("jpn", TestJPN);

        //socket.On("all", TestAll);
        //socket.On("error", TestError);
        //socket.On("close", TestClose);
        //StartCoroutine("AddEventEmitter");
        //socket.On("b", OnMcPush);
    }

    private IEnumerator AddEventEmitter()
    {
        // wait 1 second and continue
        yield return new WaitForSeconds(1);
        string path = "unitytest";
        AddDataStoreEvent("jpn", path);
        AddDataStoreEvent("all", path);
    }

    private void AddDataStoreEvent(string eventname, string path)
    {
        // add listener [send] event
        //JSONObject jsonobj = new JSONObject(JSONObject.Type.OBJECT);
        //jsonobj.AddField("event", eventname);
        //jsonobj.AddField("path", path);
        //socket.Emit("on", jsonobj);
    }

    public void TestAll(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] All received: " + e.name + " " + e.data);
        barsetter.SendMessage("SetBars",e.data);
        
    }

    public void TestJPN(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] JPN received: " + e.name + " " + e.data);
        barsetter.SendMessage("SetBars", e.data);

    }

    public void TestIZM(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] IZM received: " + e.name + " " + e.data);
        //Debug.Log(e.data);
        barsetter.SendMessage("SetIzumida", e.data);

    }

    public void TestOpen(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    }

    public void TestError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }

    public void SendOn(string query){
        Debug.Log("query:"+query);
        if (query == "all")
        {
            //socket.On("all", TestAll);
            socket.On("all", TestAll);
            socket.Emit("all");
        }
        else if (query == "jpn")
        {
            socket.On("jpn", TestJPN);
            socket.Emit("jpn");
        }
        else if (query == "posts") {
            socket.On("posts", TestIZM);
            socket.Emit("posts");
        }
    }
}
