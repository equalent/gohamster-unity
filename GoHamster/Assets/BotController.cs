using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DrawArea.bots.Add(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CheckCross(Vector3 pos1, Vector3 pos2)
    {
        if (OnLine(pos1, pos2))
        {
            Debug.LogError("COLLISION");
            Application.LoadLevel(Application.loadedLevelName);
            DrawArea.bots = new List<BotController>();
        }
        else
        {
            Debug.Log("No collision with object.");
        }
    }

    public bool OnLine(Vector3 pos1, Vector3 pos2)
    {
        if (pos1.x == pos2.x)
        {
            if (Mathf.Abs(transform.position.x - pos2.x) <= 0.5)
            {
                if (transform.position.z >= pos1.z)
                {
                    if (transform.position.z <= pos2.z)
                    {
                        return true;
                    }
                }
                else if (transform.position.z <= pos1.z)
                {
                    if (transform.position.z >= pos2.z)
                    {
                        return true;
                    }
                }
            } else if (Mathf.Abs(transform.position.z - pos2.z) <= 0.5)
            {
                if (transform.position.x >= pos1.x)
                {
                    if (transform.position.x <= pos2.x)
                    {
                        return true;
                    }
                }
                else if (transform.position.x <= pos1.x)
                {
                    if (transform.position.x >= pos2.x)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
