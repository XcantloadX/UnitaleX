using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wing.Tools.ArgsParser;
using Wing.Tools.Terminal;
using Wing.Tools.WebServer;

public class PlayerWorker : MonoBehaviour {
    private static bool lockedHealth = false;
    private static bool inited = false;
    private static HealthWorker healthIns = null;

    class HealthWorker : IWorker
    {
        ArgumentParser parser = new ArgumentParser();

        public HealthWorker()
        {
            parser.Arguments.Add(new Argument(typeof(int), "h", "health", "health to modify", false));
        }

        public string Do(Connect conn, MessageModel request, string[] args)
        {
            string reasons = "";
            parser.Arguments[0].Reset();
            if (parser.Parse(args, ref reasons))
            {
                int hp = (int)parser.Arguments[0].Value;
                PlayerController player = GameObject.FindObjectOfType<PlayerController>();
                if(player == null)
                {
                    reasons = "Failed. Are you in the battle scene?";
                    return reasons;
                }
                player.setHP(hp);
                reasons = "Done.";

                //PlayerWorker.lockedHealth = (bool)parser.Arguments[1].Value;
            }
            else
                reasons = "Failed.";

            return reasons;
        }

        public string GetName()
        {
            return "pHealth";
        }

        public string Description()
        {
            return "pHealth: modify player's health";
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
        if(!inited)
            UTerminal.Instance.Regist(new HealthWorker());
        inited = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        //Debug.Log("destroy");
        //UTerminal.Instance.UnRegist("pHealth"); //防止下次加载时重复注册
    }

    
}
