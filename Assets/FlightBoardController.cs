using System;
using System.Collections.Generic;
using UnityEngine;

public class FlightBoardController : MonoBehaviour {
    public float speed;
    public float lvlGenerationBorderLeft;
    public float lvlGenerationBorderSky;
    public float lvlGenerationBorderRight;
    private int lvlGenerationCurrent;
    public int lvlGenerationBackwards;
    public int lvlGenerationInAdvance;
    public float lvlGenerationMeshDistance;
    private Mesh lvlGenerationTestMesh;
    private List<GameObject> lvlGenerationGameObjects;

	void Start () {
        lvlGenerationGameObjects = new List<GameObject>();
        lvlGenerationCurrent = -lvlGenerationBackwards;
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        transform.up = Vector3.up;
        Mesh mesh = new Mesh();
        {
            mesh.vertices = new Vector3[]{
                new Vector3(-lvlGenerationBorderLeft, 0, 0),
                new Vector3(-lvlGenerationBorderLeft, 0, lvlGenerationMeshDistance),
                new Vector3(lvlGenerationBorderRight, 0, lvlGenerationMeshDistance),
                new Vector3(lvlGenerationBorderRight, 0, 0)
            };
            mesh.triangles = new int[]{
                0,1,2,
                0,2,3
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
        }
        lvlGenerationTestMesh = mesh;
    }

	void Update () {
        transform.Translate(Vector3.forward * speed);
        int lvlGenerationUpTo = Convert.ToInt32(Math.Ceiling(transform.position.z / lvlGenerationMeshDistance)) + lvlGenerationInAdvance;
        while (lvlGenerationCurrent < lvlGenerationUpTo) {
            GameObject lvlGenerationGameObject = new GameObject();
            lvlGenerationGameObject.AddComponent<MeshFilter>().sharedMesh = lvlGenerationTestMesh;
            lvlGenerationGameObject.AddComponent<MeshRenderer>();
            lvlGenerationGameObject.transform.position = Vector3.forward * lvlGenerationCurrent * lvlGenerationMeshDistance;
            lvlGenerationGameObjects.Add(lvlGenerationGameObject);
            ++lvlGenerationCurrent;
        }
        for (int i = lvlGenerationGameObjects.Count - 1; i >= 0; i--)
        {
            GameObject lvlGenerationGameObject = lvlGenerationGameObjects[i];
            if (transform.position.z - lvlGenerationGameObject.transform.position.z > lvlGenerationMeshDistance * lvlGenerationBackwards)
            {
                Destroy(lvlGenerationGameObject);
                lvlGenerationGameObjects.RemoveAt(i);
            }
        }
    }
}
