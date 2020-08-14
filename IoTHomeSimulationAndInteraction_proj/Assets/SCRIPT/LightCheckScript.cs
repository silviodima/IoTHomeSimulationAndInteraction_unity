using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCheckScript : MonoBehaviour
{
    public RenderTexture lightCheckTexture;
    private GameObject mqtt;
    private string msg;
    public float lightLevel;
    // public int light;

    [Serializable]
    public class Command
    {
        public int id, type;
        public float info;
    }
    // Start is called before the first frame update
    void Start()
    {
        mqtt = GameObject.FindGameObjectWithTag("mqtt");
    }

    // Update is called once per frame
    void Update()
    {
        RenderTexture tmpTexture = RenderTexture.GetTemporary(lightCheckTexture.width, lightCheckTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(lightCheckTexture, tmpTexture);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmpTexture;

        Texture2D temp2DTexture = new Texture2D(lightCheckTexture.width, lightCheckTexture.height);
        temp2DTexture.ReadPixels(new Rect(0, 0, tmpTexture.width, tmpTexture.height), 0, 0);
        temp2DTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmpTexture);

        Color32[] colors = temp2DTexture.GetPixels32();

        lightLevel = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            //brightness formula
            lightLevel += (0.2126f * colors[i].r) + (0.7152f * colors[i].g) + (0.0722f * colors[i].b);
        }

       // print(lightLevel);

        if (lightLevel < 1761270)
            print("notte:"+lightLevel);

        if (lightLevel > 6372500 && lightLevel<6372999)
            print("mezzogiorno");

        if (lightLevel > 1761270 && lightLevel<6372500)
        {
            print("mattina" + lightLevel);
            Command cmd = new Command();
            cmd.type = 0;
            cmd.id = 33;
            cmd.info = lightLevel;
            print("{\"type\":" + cmd.type + ",\"id\":" + cmd.id + ",\"lightlevel\": " + cmd.info + "}");
            msg = JsonUtility.ToJson(cmd);
            print("Opla " + msg);
          //mqtt.GetComponent<mqttController>().Publish("test", msg);

        }
    }
}
