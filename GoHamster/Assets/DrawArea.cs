using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArea : MonoBehaviour {
    public PlayerController pcontrol;
    public GameObject bot2spawn;
    public Transform st1;
    public Transform st2;
    private Vector3 sp1;
    private Vector3 sp2;
    public int botCount;
    public static int botts;
    public static List<BotController> bots=new List<BotController>();
	// Use this for initialization
	void Start () {
        botts = botCount;
        sp1 = st1.position;
        sp2 = st2.position;
        for (int i = 0; i < botCount; i++)
        {
            float ppx = Random.Range(sp1.x, sp2.x);
            float ppy = 2.157f;
            float ppz = Random.Range(sp1.z, sp2.z);
            Instantiate(bot2spawn, new Vector3(ppx, ppy, ppz), Quaternion.Euler(Vector3.zero));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pcontrol.EnableDraw();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            pcontrol.DisableDraw();
        }
    }
}
