using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody rigb;
    private TrailRenderer tren;
	// Use this for initialization
	void Start () {
        rigb = this.GetComponent<Rigidbody>();
        tren = this.GetComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.forward*Time.deltaTime;
            //Debug.Log("W");
            GetResults();
            return;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position -= Vector3.right * Time.deltaTime;
            //Debug.Log("A");
            GetResults();
            return;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position -= Vector3.forward * Time.deltaTime;
            //Debug.Log("S");
            GetResults();
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * Time.deltaTime;
            //Debug.Log("D");
            GetResults();
            return;
        }
	}

    public void EnableDraw()
    {
        tren.enabled = true;
    }

    public void DisableDraw()
    {
        tren.enabled = false;
        GetResults();
        tren.Clear();
    }

    public void GetResults()
    {
        int posc = tren.positionCount;
        for (int i = 0; i < posc; i+=1)
        {
            Vector3 pos1 = tren.GetPosition(i);
            Vector3 pos2;
            if (posc - 1 == i)
            {
                pos2 = this.transform.position;
            }
            else
            {
                pos2 = tren.GetPosition(i + 1);
            }
            foreach (BotController bc in DrawArea.bots)
            {
                bc.CheckCross(pos1, pos2);
            }
        }
    }
}
