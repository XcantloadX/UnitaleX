using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wing.Tools.ArgsParser;
using Wing.Tools.Terminal;
using Wing.Tools.WebServer;


public class VibrateWorker : MonoBehaviour
{
    private static bool inited = false;

    class Worker : IWorker
    {
        ArgumentParser parser = new ArgumentParser();

        public Worker()
        {
            parser.Arguments.Add(new Argument(typeof(long), "t", "time", "time(in ms)", false));
        }

        public string Do(Connect conn, MessageModel request, string[] args)
        {
            string reasons = "";
            parser.Arguments[0].Reset();
            if(parser.Parse(args, ref reasons))
            {
                Vibration.Vibrate((long)parser.Arguments[0].Value);
                Debug.Log(parser.Arguments[0].Value);
                return "done";
            }
            else
                return "failed";

            
        }

        public string GetName()
        {
            return "vibrate";
        }

        public string Description()
        {
            return "vibrate: vibrate your phone";
        }

        public void OnClose(Connect conn)
        {

        }


        public string Usage()
        {
            return parser.Usage();
        }
    }

    // Use this for initialization
    void Start()
    {
        if (!inited)
            UTerminal.Instance.Regist(new Worker());
        inited = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
