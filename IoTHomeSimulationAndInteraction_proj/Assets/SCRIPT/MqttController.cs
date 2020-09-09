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
using Unity.Collections;
using Unity.Jobs;

public class MqttController : MonoBehaviour
{
    public MqttClient client;
    //public string brokerHostname = "192.168.1.112";
    public string brokerHostname = "127.0.0.1";
    public int brokerPort = 1883;
    public string userName = "test";
    public string password = "test";
    public TextAsset certificate;
    // listen on all the Topic
    static string subTopic = "web";
    private GameObject lampController, blindController, sensori, TVscreen, conditionerController, blindSoggiorno, blindStanza;
    private bool actionLamp, actionBlind, actionSensor, actionTV, actionConditioner;
    private bool receivedMsg;
    private Command cmd; 
    private InitCommand initCmd;
        
    private string msg;



    // Start is called before the first frame update
    void Start()
    {
        receivedMsg = false;
        actionSensor = false;
        actionLamp = false;
        actionBlind = false;
        actionTV = false;
        actionConditioner = false;
        lampController = GameObject.FindGameObjectWithTag("lampController");
        blindController = GameObject.FindGameObjectWithTag("blindController");
        sensori = GameObject.FindGameObjectWithTag("sensor");
        TVscreen = GameObject.FindGameObjectWithTag("TVController");
        conditionerController = GameObject.FindGameObjectWithTag("conditionerController");
        blindSoggiorno = GameObject.FindGameObjectWithTag("10");
        blindStanza = GameObject.FindGameObjectWithTag("22");
        

        if (brokerHostname != null && userName != null && password != null)
        {
            Debug.Log("connecting to " + brokerHostname + ":" + brokerPort);
            Connect();
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
            client.Subscribe(new string[] { subTopic }, qosLevels);
        }

    }

    public struct job: IJobParallelFor
    {
        public int id; 
        public int action;
        [ReadOnly]
        public GameObject blind;

        public void Execute()
        {
            //GameObject blindController = GameObject.FindGameObjectWithTag("blindController");
            //blindController.GetComponent<BlindController>().move(id, action);
        }

        public void Execute(int index)
        {
            throw new NotImplementedException();
        }
    }

    private void faiCose(GameObject blind, int id, int action) //doveva essere jobHandle
    {
        job j = new job();
        j.id = id;
        j.action = action;
        j.blind = blind;
        //return j.Schedule();
    }


    // Update is called once per frame
    private void Update()
    {
       if(actionLamp)
        {
            //print("ID:" + cmd.id + cmd.action);
            if (cmd.cmd.Equals("switch"))
            {
                print("sto qua");
                lampController.GetComponent<LampController>().switchLamp(cmd.id, cmd.action);
            }

            if(cmd.cmd.Equals("power") && (cmd.id == 1 || cmd.id == 6 || cmd.id == 9))
            {
                //action sarà 0,25,50,75,100
                lampController.GetComponent<LampController>().powerLamp(cmd.id, Int32.Parse(cmd.action));
            }
            actionLamp = false;
        }

        if (actionBlind)
        {
            actionBlind = false;
            print("ID:" + cmd.action);
            //Task.Factory.StartNew(() => blindController.GetComponent<BlindController>().move(10, Int32.Parse("100")));
            blindController.GetComponent<BlindController>().move(cmd.id, Int32.Parse(cmd.action));
            //new Thread(delegate ()
            //{
            //    blindController.GetComponent<BlindController>().move(cmd.id, Int32.Parse(cmd.action));
            //}).Start();
            //NativeList<JobHandle> list = new NativeList<JobHandle>(Allocator.Temp);
            //JobHandle jobHandle = faiCose(cmd.id, cmd.action);
            //list.Add(jobHandle);
            //jobHandle.CompleteAll(list);
            //list.Dispose();
            //NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
            ////JobHandle jobHandle = faiCose(blindController, cmd.id, Int32.Parse(cmd.action));

            ////jobHandleList.Add(jobHandle);
            //JobHandle.CompleteAll(jobHandleList);
            //jobHandleList.Dispose();



        }

        if (actionSensor)
        {
            actionSensor = false;
            sensori.GetComponent<Sensori>().getLightSensor();
        }

       if(actionTV)
        {
            print("CMD:" + cmd.cmd + "\nACTION:" + cmd.action);
            actionTV = false;
            if (cmd.cmd.Equals("switch"))
            {
                TVscreen.GetComponent<TVController>().switchTV(cmd.id, cmd.action);
            }

            if(cmd.cmd.Equals("channel"))
            {
                TVscreen.GetComponent<TVController>().switchChannel(cmd.id, Int32.Parse(cmd.action));
            }

            if(cmd.cmd.Equals("volume"))
            {
                //print("cambio volume");
                TVscreen.GetComponent<TVController>().changeVolume(cmd.id, Int32.Parse(cmd.action));
            }
        }

       if(actionConditioner)
        {
            actionConditioner = false;
            if (cmd.cmd.Equals("switch"))
            {
                conditionerController.GetComponent<CondizionatoreController>().switchConditioner(cmd.id, cmd.action);
            }

            if (cmd.cmd.Equals("temp"))
            {
                //print("cambio volume");
                conditionerController.GetComponent<CondizionatoreController>().changeTemp(cmd.id, Int32.Parse(cmd.action));
            }
        }
    }


    [Serializable]
    public class Command
    {
        public int type;
        public int id;
        public string cmd, component, action;
    }

    public class InitCommand
    {
        public int type;
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

        //Richiesta di inizializzazione
        initCmd = new InitCommand
        {
            type = 1
        };

        msg = JsonUtility.ToJson(initCmd);
        Publish("unity", msg);

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

        //se cmd.type == 0, è richiesto l'invio di comandi
        if (cmd.type == 0)
        {
            //lampadine o fornelli
            if (cmd.id <= 9 || cmd.id>=14 && cmd.id<=17 )
            {
                print("lampadine");
                actionLamp = true;
            }

            //persiane
            if (cmd.id >= 10 && cmd.id<=13 || cmd.id==22)
            {
                print("persiane"+cmd.action);
                actionBlind = true;
            }

            //condizionatori
            if (cmd.id == 18 || cmd.id == 19)
            {
                print("condizionatori");
                actionConditioner = true;
            }


            //TV
            if (cmd.id==20 || cmd.id==21)
            {
                print("TV");
                actionTV = true;
            }
        }

        //se cmd.type ==  1, è richiesto l'invio dei dati dei sensori
        if(cmd.type==1)
        {
            print("sensori");
            actionSensor = true;
        }

}

public void Publish(string _topic, string msg)
{
client.Publish(
   _topic, System.Text.Encoding.UTF8.GetBytes(msg),
   MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
}



}
