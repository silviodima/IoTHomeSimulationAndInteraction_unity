using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensori : MonoBehaviour
{
    public RenderTexture soggiorno, stanza, cucina, bagno, ingresso;
    //public RenderTexture soggiornoLightTexture;
    private GameObject mqtt;
    private string[] msg;
    private toJson toSend;
    private float[] lightLevel;
    private RenderTexture[] textures, tmpTextures, previous;
    private Texture2D[] tmp2Dtextures;
    private Collider cameraCollider, ingressoCollider;
    //private float lightLevel;
    // private const float DARK = 2876222f, PENUMBRA = 3082577f, LIGHT_ON = 6331760f, LIGHTON_BLINDOFF = 6370659f;

    // public int light;

    [Serializable]
    public class Command
    {
        public string locale, descrizione;
        public float valore;
    }

    [Serializable]
    public class toJson
    {
        public int type;
        public string[] sensore;
    }
    // Start is called before the first frame update
    void Start()
    {
        textures = new RenderTexture[5];
        textures[0] = soggiorno;
        textures[1] = stanza;
        textures[2] = cucina;
        textures[3] = bagno;
        textures[4] = ingresso;

        tmpTextures = new RenderTexture[5];
        previous = new RenderTexture[5];
        tmp2Dtextures = new Texture2D[5];
        lightLevel = new float[5];
        msg = new string[5];

        mqtt = GameObject.FindGameObjectWithTag("mqtt");
        cameraCollider = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Collider>();
        ingressoCollider = GameObject.FindGameObjectWithTag("ingressoCollider").GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {/*
        for (int i = 0; i < textures.Length; i++)
        {
            tmpTextures[i] = RenderTexture.GetTemporary(textures[i].width, textures[i].height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(textures[i], tmpTextures[i]);

            previous[i] = RenderTexture.active;
            RenderTexture.active = tmpTextures[i];

            tmp2Dtextures[i] = new Texture2D(textures[i].width, textures[i].height);
            tmp2Dtextures[i].ReadPixels(new Rect(0, 0, tmpTextures[i].width, tmpTextures[i].height), 0, 0);
            tmp2Dtextures[i].Apply();

            RenderTexture.active = previous[i];
            RenderTexture.ReleaseTemporary(tmpTextures[i]);

            Color32[] colors = tmp2Dtextures[i].GetPixels32();
            for (int k = 0; k < 5; k++)
            {
                lightLevel[k] = 0f;
            }
            for (int j = 0; j < colors.Length; j++)
            {
                //formula per il calcolo della luminosità
                lightLevel[i] += (0.2126f * colors[j].r) + (0.7152f * colors[j].g) + (0.0722f * colors[j].b);
                Command cmd = new Command();
                cmd.type = 2;
                cmd.value = lightLevel[i];
                switch (i)
                {
                    case 0:
                        cmd.room = "soggiorno";
                        break;
                    case 1:
                        cmd.room = "stanza";
                        break;
                    case 2:
                        cmd.room = "cucina";
                        break;
                    case 3:
                        cmd.room = "bagno";
                        break;
                }
                cmd.description = "luce";
                DAELIMINARE = cmd.room;

                // print("{\"type\":" + cmd.type + ",\"id\":" + cmd.id + ",\"lightlevel\": " + cmd.info + "}");
                msg = JsonUtility.ToJson(cmd);
            }
        // print("ROOM["+0+"]: "+DAELIMINARE+lightLevel[0]);

        }

        //print(lightLevel);

        /*   if (lightLevel == DARK || lightLevel == PENUMBRA || lightLevel == LIGHT_ON || lightLevel == LIGHTON_BLINDOFF)
           {
               Command cmd = new Command();
               cmd.type = 2;
               cmd.value = lightLevel;
               cmd.room = "soggiorno";
               cmd.description = "light";
               msg = JsonUtility.ToJson(cmd);
               publishSensor();

           }*/
    }

    public void getLightSensor()
    {
        toSend = new toJson();
        toSend.type = 2;
        toSend.sensore = new string[10];
        for (int i = 0; i < textures.Length; i++)
        {
            tmpTextures[i] = RenderTexture.GetTemporary(textures[i].width, textures[i].height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
            Graphics.Blit(textures[i], tmpTextures[i]);

            previous[i] = RenderTexture.active;
            RenderTexture.active = tmpTextures[i];

            tmp2Dtextures[i] = new Texture2D(textures[i].width, textures[i].height);
            tmp2Dtextures[i].ReadPixels(new Rect(0, 0, tmpTextures[i].width, tmpTextures[i].height), 0, 0);
            tmp2Dtextures[i].Apply();

            RenderTexture.active = previous[i];
            RenderTexture.ReleaseTemporary(tmpTextures[i]);

            Color32[] colors = tmp2Dtextures[i].GetPixels32();
            for (int k = 0; k < 5; k++)
            {
                lightLevel[k] = 0f;
            }
            for (int j = 0; j < colors.Length; j++)
            {
                //formula per il calcolo della luminosità
                lightLevel[i] += (0.2126f * colors[j].r) + (0.7152f * colors[j].g) + (0.0722f * colors[j].b);
                Command cmd = new Command();
                cmd.valore = lightLevel[i];

                switch (i)
                {
                    case 0:
                        if (lightLevel[i] < 2900000) cmd.valore = 0;
                        if (lightLevel[i] > 2900000 && lightLevel[i] < 3300000) cmd.valore = 25;
                        if (lightLevel[i] > 3300000 && lightLevel[i] < 4000000) cmd.valore = 50;
                        if (lightLevel[i] > 4000000 && lightLevel[i] < 4300000) cmd.valore = 75;
                        if (lightLevel[i] > 4300000) cmd.valore = 100;
                        cmd.locale = "soggiorno";
                        break;
                    case 1:
                        if (lightLevel[i] < 2900000) cmd.valore = 0;
                        if (lightLevel[i] > 2900000 && lightLevel[i] < 3300000) cmd.valore = 25;
                        if (lightLevel[i] > 3300000 && lightLevel[i] < 4100000) cmd.valore = 50;
                        if (lightLevel[i] > 4100000 && lightLevel[i] < 4700000) cmd.valore = 75;
                        if (lightLevel[i] > 4700000) cmd.valore = 100;
                        cmd.locale = "stanza";
                        break;
                    case 2:
                        if (lightLevel[i] < 2900000) cmd.valore = 0;
                        if (lightLevel[i] > 2900000 && lightLevel[i] < 3300000) cmd.valore = 25;
                        if (lightLevel[i] > 3300000 && lightLevel[i] < 4400000) cmd.valore = 50;
                        if (lightLevel[i] > 4400000 && lightLevel[i] < 5000000) cmd.valore = 75;
                        if (lightLevel[i] > 5000000) cmd.valore = 100;
                        cmd.locale = "cucina";
                        break;
                    case 3:
                        if (lightLevel[i] < 2900000) cmd.valore = 0;
                        if (lightLevel[i] > 2900000 && lightLevel[i] < 3529000) cmd.valore = 25;
                        if (lightLevel[i] > 3529000 && lightLevel[i] < 4414000) cmd.valore = 50;
                        if (lightLevel[i] > 4414000 && lightLevel[i] < 4365000) cmd.valore = 75;
                        if (lightLevel[i] > 4365000) cmd.valore = 100;
                        cmd.locale = "bagno";
                        break;
                    case 4:
                        if (lightLevel[i] < 2900000) cmd.valore = 0;
                        if (lightLevel[i] > 2900000 && lightLevel[i] < 3700000) cmd.valore = 25;
                        if (lightLevel[i] > 3700000 && lightLevel[i] < 5000000) cmd.valore = 50;
                        if (lightLevel[i] > 5000000 && lightLevel[i] < 5800000) cmd.valore = 75;
                        if (lightLevel[i] > 5800000) cmd.valore = 100;
                        cmd.locale = "ingresso";
                        break;

                }
                cmd.descrizione = "luce";

                // print("{\"type\":" + cmd.type + ",\"id\":" + cmd.id + ",\"lightlevel\": " + cmd.info + "}");
                toSend.sensore[i] = JsonUtility.ToJson(cmd);

            }

        }

        /* foreach (string mg in msg)
         {
             print("" + mg);
         }
         string toJson = JsonHelper.ToJson(msg, true);
         print("" + toJson);
         mqtt.GetComponent<MqttController>().Publish("test", toJson);*/
         getMovementSensor();

    }

    public void getMovementSensor()
    {
        for(int i =5; i<10;i++)
        {
            Command cmd = new Command();
            // cmd.valore = value[i];
            if (CameraController.movementValues[i-5] == 1)
                cmd.valore = 1;

            else cmd.valore = 0;
            cmd.descrizione = "movimento";
            switch(i)
                {
                case (5): cmd.locale = "soggiorno";
                    break;
                case (6):
                    cmd.locale = "stanza";
                    break;
                case (7):
                    cmd.locale = "cucina";
                    break;
                case (8):
                    cmd.locale = "bagno";
                    break;
                case (9):
                    cmd.locale = "ingresso";
                    break;
            }

            toSend.sensore[i] = JsonUtility.ToJson(cmd);

        }

        string toJson = JsonUtility.ToJson(toSend);
        mqtt.GetComponent<MqttController>().Publish("test", toJson);
    }

  /*  public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.sensori;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.sensori = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.sensori = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] sensori;
    }
}*/
}
