using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensor : MonoBehaviour
{
    public RenderTexture soggiornoLightTexture;
    private GameObject mqtt;
    private string msg;
    private float lightLevel;
    private const float DARK = 2876222f, PENUMBRA = 3082577f, LIGHT_ON = 6331760f, LIGHTON_BLINDOFF = 6370659f;

    // public int light;

    [Serializable]
    public class Command
    {
        public int type;
        public string room, description;
        public float value;
    }
    // Start is called before the first frame update
    void Start()
    {
        mqtt = GameObject.FindGameObjectWithTag("mqtt");
    }

    // Update is called once per frame
    void Update()
    {
        RenderTexture tmpTexture = RenderTexture.GetTemporary(soggiornoLightTexture.width, soggiornoLightTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(soggiornoLightTexture, tmpTexture);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmpTexture;

        Texture2D temp2DTexture = new Texture2D(soggiornoLightTexture.width, soggiornoLightTexture.height);
        temp2DTexture.ReadPixels(new Rect(0, 0, tmpTexture.width, tmpTexture.height), 0, 0);
        temp2DTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmpTexture);

        Color32[] colors = temp2DTexture.GetPixels32();

        lightLevel = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            //formula per il calcolo della luminosità
            lightLevel += (0.2126f * colors[i].r) + (0.7152f * colors[i].g) + (0.0722f * colors[i].b);
            Command cmd = new Command();
            cmd.type = 2;
            cmd.value = lightLevel;
            cmd.room = "soggiorno";
            cmd.description = "light";

           // print("{\"type\":" + cmd.type + ",\"id\":" + cmd.id + ",\"lightlevel\": " + cmd.info + "}");
            msg = JsonUtility.ToJson(cmd);
        }

        //print(lightLevel);

        if (lightLevel == DARK || lightLevel == PENUMBRA || lightLevel == LIGHT_ON || lightLevel == LIGHTON_BLINDOFF)
        {
            Command cmd = new Command();
            cmd.type = 2;
            cmd.value = lightLevel;
            cmd.room = "soggiorno";
            cmd.description = "light";
            msg = JsonUtility.ToJson(cmd);
            publishSensor();

        }
    }

    public void publishSensor()
    {
        mqtt.GetComponent<MqttController>().Publish("test", msg);
    }
}
