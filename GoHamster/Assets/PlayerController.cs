using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    private Rigidbody rigb;
    public TrailRenderer tren;
    public static Vector3 startPosition;
    public static PlayerController cpc;
    public Material trenMat;
    public GameObject selfPrefab;
    public static int killCount = 0;
    public Text won;
	// Use this for initialization
	void Start () {
        rigb = this.GetComponent<Rigidbody>();
        cpc = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (killCount >= DrawArea.botts)
        {
            won.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.forward*Time.deltaTime*3;
            //Debug.Log("W");
            GetResults();
            return;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position -= Vector3.right * Time.deltaTime*3;
            //Debug.Log("A");
            GetResults();
            return;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position -= Vector3.forward * Time.deltaTime*3;
            //Debug.Log("S");
            GetResults();
            return;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * Time.deltaTime*3;
            //Debug.Log("D");
            GetResults();
            return;
        }
	}

    public void EnableDraw()
    {
        tren.enabled = true;
        startPosition = this.transform.position;
    }

    public void DisableDraw()
    {
        GetResults();
        CheckBot();
        Destroy(tren.gameObject);
        tren = Instantiate(selfPrefab, transform).GetComponent<TrailRenderer>();
        tren.enabled = false;
    }

    public void GetResults()
    {
        int posc = tren.positionCount;
        Debug.Log("POSC"+posc);
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
                if(bc!=null)
                    bc.CheckCross(pos1, pos2);
            }
        }
    }

    public void CheckBot()
    {
        foreach (BotController bc in DrawArea.bots)
        {
            if (bc != null)
            {
                List<Vector2> points = new List<Vector2>();
                points.Add(new Vector2(PlayerController.startPosition.x, PlayerController.startPosition.z));
                int pco = tren.positionCount;
                for (int i = 0; i < pco; i++)
                {
                    Vector3 pt = tren.GetPosition(i);
                    points.Add(new Vector2(pt.x, pt.z));
                }
                points.Add(new Vector2(PlayerController.cpc.transform.position.x, PlayerController.cpc.transform.position.z));
                Debug.Log(points.Count);
                Debug.Log("PCO" + pco);
                if (BotController.IsPointInPolygon(points, new Vector2(bc.transform.position.x, bc.transform.position.z)))
                {
                    Destroy(bc.gameObject);
                }
            }
        }
    }
}
