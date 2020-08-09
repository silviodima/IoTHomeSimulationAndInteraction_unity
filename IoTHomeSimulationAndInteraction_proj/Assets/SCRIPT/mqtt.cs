using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//librerie di mqtt
using uPLibrary.Networking.M2Mqtt.Exceptions;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
//librerie di supporto a mqtt
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Numerics;
using System.Linq;

public class mqtt : MonoBehaviour
{
    public MqttClient client;
    public string brokerHostname = "127.0.0.1";
    public int brokerPort = 1883;
    public string userName = "test";
    public string password = "test";
    public TextAsset certificate;
    // listen on all the Topic
    static string subTopic = "test";
    private GameObject lampController, blindController;
    private bool actionLamp, actionBlind;
    private bool receivedMsg;
    private Command cmd;

   
    
    // Start is called before the first frame update
    void Start()
    {
        receivedMsg = false;
        actionLamp = false;
        actionBlind = false;
        lampController = GameObject.FindGameObjectWithTag("lampController");
        blindController = GameObject.FindGameObjectWithTag("blindController");
        if (brokerHostname != null && userName != null && password != null)
        {
            Debug.Log("connecting to " + brokerHostname + ":" + brokerPort);
            Connect();
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
            client.Subscribe(new string[] { subTopic }, qosLevels);
        }

    }

    // Update is called once per frame
    void Update()
    {
       if(receivedMsg)
        {
            print("ID:" + cmd.id + cmd.action);

            /*string msg = File.ReadAllText(Application.dataPath + "/command.json");
            receivedMsg = false;
            Command command = JsonUtility.FromJson<Command>(msg);
            print("FILE:" + msg);
            print("Id: " + command.id);
            print("Cmd: " + command.cmd);*/
            receivedMsg = false;
            lampController.GetComponent<LampController>().switchLamp(cmd.id, cmd.action);


        }
    }

    [Serializable]
    public class Command
    {
        public int id;
        public string cmd;
        public string action;
    }

    [Serializable]
    public class RootCommand
    {
        public List<Command> cmdList;

        public List<Command> getCmdList()
        {
            return this.cmdList;
        }
    }


    private void Connect()
    {
        Debug.Log("about to connect on '" + brokerHostname + "'");
        // Forming a certificate based on a TextAsset
        //X509Certificate cert = new X509Certificate();
        //Debug.Log("allah"+ certificate.bytes);
        //cert.Import(certificate.bytes);
        //Debug.Log("Using the certificate '" + cert + "'");
        client = new MqttClient(brokerHostname);
        string clientId = Guid.NewGuid().ToString();
        Debug.Log("About to connect using '" + userName + "' / '" + password + "'");
        try
        {
            client.Connect(clientId, userName, password);
        }
        catch (Exception e)
        {
            Debug.LogError("Connection error: " + e);
        }
    }
    public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string msg = System.Text.Encoding.UTF8.GetString(e.Message);
        //msg = "{\"Command[]\":" + msg + "}";
        Debug.Log("Received message from " + e.Topic + " : " + msg);
        cmd = JsonUtility.FromJson<Command>(msg);

        print("ID:" + cmd.id+cmd.action);
        receivedMsg = true;
        

        /*//List<Command> cmds = JsonUtility.FromJson<List<Command>>(msg);
        //cmds.Add(new Command());
        RootCommand cmds = new RootCommand();
        cmds.cmds = JsonUtility.FromJson<Command[]>(msg);


        print("Num comandi:" + cmds.cmds.Length);*/

/*  foreach (Command cmd in cmds)
  {
      print("id: " + cmd.id);
      print("comando: " + cmd.cmd);
      print("azione: " + cmd.action);
  }

 Command[] cmds = JsonHelper.FromJson<Command>(msg);

 if (cmds == null) print("mammt");
foreach (Command cmd in cmds)
{
   print("id: " + cmd.id);
   print("comando: " + cmd.cmd);
   print("azione: " + cmd.action);
}*/

/* RootCommand cmds = JsonUtility.FromJson<RootCommand>(msg);
 print("COMANDI: " + cmds.getCmdList().Count);

 /*foreach (Command cmd in cmds)
 {
     print("id: " + cmd.id);
     print("comando: " + cmd.cmd);
     print("azione: " + cmd.action);
 }*/

}

private void Publish(string _topic, string msg)
{
client.Publish(
   _topic, System.Text.Encoding.UTF8.GetBytes(msg),
   MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
}


public static class JsonHelper
{
public static T[] FromJson<T>(string json)
{
   Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
   return wrapper.Items;
}

public static string ToJson<T>(T[] array)
{
   Wrapper<T> wrapper = new Wrapper<T>();
   wrapper.Items = array;
   return JsonUtility.ToJson(wrapper);
}

public static string ToJson<T>(T[] array, bool prettyPrint)
{
   Wrapper<T> wrapper = new Wrapper<T>();
   wrapper.Items = array;
   return JsonUtility.ToJson(wrapper, prettyPrint);
}

[Serializable]
private class Wrapper<T>
{
   public T[] Items;
}
}
}
