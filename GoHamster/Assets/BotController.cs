using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DrawArea.bots.Add(this);
        transform.localRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * Time.deltaTime * 0.5f;
	}

    void OnCollisionEnter(Collision other)
    {
        transform.RotateAroundLocal(Vector3.up, 180f);
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

    public void CheckFigure(TrailRenderer tren)
    {
        if (InFigure(tren))
        {
            PlayerController.killCount++;
            Destroy(this.gameObject);
            //TODO: draw closing area
        }
    }

    public static bool IsInPolygon(List<Vector2> poly, Vector2 point)
    {
        var coef = poly.Skip(1).Select((p, i) =>
                                        (point.y - poly[i].y) * (p.x - poly[i].x)
                                      - (point.x - poly[i].x) * (p.y - poly[i].y))
                                .ToList();

        if (coef.Any(p => p == 0))
            return true;

        for (int i = 1; i < coef.Count(); i++)
        {
            if (coef[i] * coef[i - 1] < 0)
                return false;
        }
        return true;
    }



    public static bool IsPointInPolygon(List<Vector2> polygon, Vector2 point)
    {

        bool isInside = false;

        for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
        {

            if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&

            (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
            {

                isInside = !isInside;

            }

        }

        return isInside;

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

    public bool InFigure(TrailRenderer tren)
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
        if (IsInPolygon(points, new Vector2(this.transform.position.x, this.transform.position.z)))
        {
            Triangulator tr = new Triangulator(points.ToArray());
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[points.Count];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(points[i].x, points[i].y, 0);
            }
            Mesh msh = new Mesh();
            msh.vertices = vertices;
            msh.triangles = indices;
            msh.RecalculateNormals();
            msh.RecalculateBounds();

            GameObject go = new GameObject("Filler");
            go.transform.position = PlayerController.startPosition;
            go.AddComponent<MeshRenderer>();
            go.AddComponent<MeshFilter>().mesh = msh;
            return true;
        }
        return IsInPolygon(points, new Vector2(this.transform.position.x, this.transform.position.z));
    }

    public bool InFigureold(TrailRenderer tren)
    {
        int pcn = tren.positionCount;
        List<float> maxs = new List<float>();
        for (int i = 0; i < pcn; i++)
        {
            Vector3 pos = tren.GetPosition(i);
            List<float> dists = new List<float>();
            for (int j = 0; j < pcn; j++)
            {
                dists.Add(Vector3.Distance(tren.GetPosition(j), pos));
            }
            maxs.Add(FindV(dists));
        }
        for (int i = 0; i < pcn; i++)
        {
            foreach (float dst in maxs)
            {
                if (Vector3.Distance(tren.GetPosition(i), this.transform.position) > dst)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public float FindV(List<float> list)
    {
        float maxF = int.MinValue;
        foreach (float type in list)
        {
            if (type > maxF)
            {
                maxF = type;
            }
        }
        return maxF;
    }
}
