using System;
using System.Collections.Generic;
using UnityEngine;

public class FlightBoardController : MonoBehaviour {
    private Vector3 origin;
    public float speed = 2;
    private Vector3 direction = Vector3.forward;
    private float lvlGenerationBorderLeft = 10.0f;
    private float lvlGenerationBorderSky = 30.0f;
    private float lvlGenerationBorderRight = 10.0f;
    private int lvlGenerationCurrent;
    private int lvlGenerationBackwards = 3;
    private int lvlGenerationInAdvance = 10;
    private int lvlGenerationMeshDistance = 4;
    private Mesh lvlGenerationTestMesh;
    
	void Start ()
    {
        origin = transform.position;
        lvlGenerationCurrent = -lvlGenerationBackwards;

        Mesh mesh = new Mesh();
        {
            Vector3 p0 = new Vector3(0, 0, 0);
            Vector3 p1 = new Vector3(1, 0, 0);
            Vector3 p2 = new Vector3(0.5f, 0, Mathf.Sqrt(0.75f));
            Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3);
            mesh.vertices = new Vector3[]{
                p0,p1,p2,
                p0,p2,p3,
                p2,p1,p3,
                p0,p3,p1
            };
            mesh.triangles = new int[]{
                0,1,2,
                3,4,5,
                6,7,8,
                9,10,11
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
        }
        lvlGenerationTestMesh = mesh;
    }
	
	void Update () {
        transform.Translate(direction * Time.deltaTime * speed);
        float d = Vector3.Distance(origin, transform.position);
        int next = Convert.ToInt32(Math.Ceiling(d / lvlGenerationMeshDistance)) + lvlGenerationInAdvance;
        for (; lvlGenerationCurrent < next; ++lvlGenerationCurrent)
        {
            Vector3 pos = origin + direction * lvlGenerationCurrent * lvlGenerationMeshDistance;
            pos.y = 0.0f;
            GameObject lvlObject = new GameObject();
            lvlObject.AddComponent<MeshFilter>().sharedMesh = lvlGenerationTestMesh;
            lvlObject.AddComponent<MeshRenderer>();
            lvlObject.transform.position = pos;
        }
    }
}
