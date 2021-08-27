using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Demo : MonoBehaviour
{
    public float avgFrameRate;
    // Start is called before the first frame update
    void Awake()
    {
        JObject o = JObject.FromObject(new
        {
            name = "Start",
            data = Time.realtimeSinceStartup.ToString()
        });
        UnityMessageManager.Instance.SendMessageToRN(UnityMessageManager.MessagePrefix + o.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        avgFrameRate = Time.frameCount / Time.time;
        JObject o = JObject.FromObject(new
        {
            name = "FPS",
            data = avgFrameRate.ToString()
        });
        UnityMessageManager.Instance.SendMessageToRN(UnityMessageManager.MessagePrefix + o.ToString());
    }
}
