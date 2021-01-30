using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wing.Tools.WebServer;
using Wing.Tools.Terminal;
using Wing.Tools;

public class RemoteLogging : MonoBehaviour {

    private WebServer server = new WebServer();
    private static bool inited = false;

    void Start()
    {
        if (inited)
            return;

        //when you run in editor,plz add this
#if UNITY_EDITOR
        Application.runInBackground = true;
#endif

        //for debug
        server.EnableCrossDomain = true;
        //HttpServer.Logging.LogFactory.Assign(new LogFactory());

        //add log worker to termial
        UTerminal.Instance.Regist(new LogWorker());
        server.Start();

        //termial default is close
        server.EnableTerminal(8087, "log -o", 1000);
        inited = true;
    }

	
	// Update is called once per frame
	void Update () {
		
	}

    void OnApplicationQuit()
    {
        if (server != null)
        {
            server.Stop();
        }
    }
}
