using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightBoardController : MonoBehaviour {
    public float speed;
    public float rotationSpeed;
    public float maxRotLeftRight;
    public float maxRotUpDown;
    public float resetRotationFactor;
    public float rotationPositionFactor;
    public float lvlGenerationTunnelWidth;
    public float lvlGenerationTunnelHeight;
    public float lvlGenerationTunnelInlineBorder;
    private int lvlGenerationCurrent;
    public int lvlGenerationBackwards;
    public int lvlGenerationInAdvance;
    public float lvlGenerationMeshDistance;
    public Material lvlGenerationBaseMaterial;
    public Text scoreDisplay;
    public GameObject originalCollectable;
    private int score;
    private Mesh lvlGenerationTestMesh;
    private List<GameObject> lvlGenerationGameObjects;

    private void UpdateScoreDisplay() {
        scoreDisplay.text = score.ToString();
    }

	void Start () {
        lvlGenerationGameObjects = new List<GameObject>();
        lvlGenerationCurrent = -lvlGenerationBackwards;
        transform.position = new Vector3(0.0f, 2.0f, 0.0f);
        transform.eulerAngles = Vector3.up;
        score = 0;
        UpdateScoreDisplay();
        Mesh mesh = new Mesh();
        {
            mesh.vertices = new Vector3[]{
                new Vector3(-lvlGenerationTunnelWidth, 0, 0),
                new Vector3(-lvlGenerationTunnelWidth, 0, lvlGenerationMeshDistance),
                new Vector3(lvlGenerationTunnelWidth, 0, lvlGenerationMeshDistance),
                new Vector3(lvlGenerationTunnelWidth, 0, 0)
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

	void Update ()
    {
        Vector3 r = transform.eulerAngles;
        float deltaRotation = rotationSpeed * Time.deltaTime;
        float deltaResetRotation = deltaRotation * resetRotationFactor;
        if (Input.GetKey(KeyCode.S)) {
            r.x -= deltaRotation;
        } else if (Input.GetKey(KeyCode.W)) {
            r.x += deltaRotation;
        } else {
            if (r.x > deltaResetRotation && r.x < 180) {
                r.x -= deltaResetRotation;
            } else if (r.x - 360 < -deltaResetRotation && r.x > 180) {
                r.x += deltaResetRotation;
            } else {
                r.x += 0;
            }
        }
        if (Input.GetKey(KeyCode.A)) {
            r.z += deltaRotation;
        } else if (Input.GetKey(KeyCode.D)) {
            r.z -= deltaRotation;
        } else {
            if (r.z > deltaResetRotation && r.z < 180) {
                r.z -= deltaResetRotation;
            } else if (r.z - 360 < -deltaResetRotation && r.z > 180) {
                r.z += deltaResetRotation;
            } else {
                r.z += 0;
            }
        }
        if (r.z > maxRotLeftRight && r.z < 180) {
            r.z = maxRotLeftRight;
        }
        if (r.z < 360 - maxRotLeftRight && r.z > 180) {
            r.z = 360 - maxRotLeftRight;
        }
        if (r.x > maxRotUpDown && r.x < 180) {
            r.x = maxRotUpDown;
        }
        if (r.x < 360 - maxRotUpDown && r.x > 180) {
            r.x = 360 - maxRotUpDown;
        }
        transform.eulerAngles = r;
        Vector3 p = transform.position;
        p.z += speed * Time.deltaTime;
        if (r.x > 180) {
            r.x -= 360;
        }
        if (r.z > 180) {
            r.z -= 360;
        }
        p.x -= r.z * rotationPositionFactor * Time.deltaTime;
        p.y -= r.x * rotationPositionFactor * Time.deltaTime;
        if (p.x > lvlGenerationTunnelWidth - lvlGenerationTunnelInlineBorder) {
            p.x = lvlGenerationTunnelWidth - lvlGenerationTunnelInlineBorder;
        }
        if (p.x < -lvlGenerationTunnelWidth + lvlGenerationTunnelInlineBorder) {
            p.x = -lvlGenerationTunnelWidth + lvlGenerationTunnelInlineBorder;
        }
        if (p.y > lvlGenerationTunnelHeight - lvlGenerationTunnelInlineBorder) {
            p.y = lvlGenerationTunnelHeight - lvlGenerationTunnelInlineBorder;
        }
        if (p.y < lvlGenerationTunnelInlineBorder) {
            p.y = lvlGenerationTunnelInlineBorder;
        }
        transform.position = p;
        int playersSection = Convert.ToInt32(Math.Ceiling(transform.position.z / lvlGenerationMeshDistance));
        int lvlGenerationUpTo = playersSection + lvlGenerationInAdvance;
        while (lvlGenerationCurrent < lvlGenerationUpTo) {
            GameObject lvlGenerationGameObject = new GameObject("LvlGenerationGameObject");
            lvlGenerationGameObject.AddComponent<MeshFilter>().sharedMesh = lvlGenerationTestMesh;
            lvlGenerationGameObject.AddComponent<MeshRenderer>();
            lvlGenerationGameObject.transform.position = Vector3.forward * lvlGenerationCurrent * lvlGenerationMeshDistance;
            lvlGenerationGameObject.GetComponent<Renderer>().material = lvlGenerationBaseMaterial;
            lvlGenerationGameObjects.Add(lvlGenerationGameObject);
            if (lvlGenerationCurrent > -lvlGenerationInAdvance) {
                GameObject collectable = Instantiate(originalCollectable);
                collectable.transform.position = new Vector3(
                    UnityEngine.Random.Range(-lvlGenerationTunnelWidth + lvlGenerationTunnelInlineBorder, lvlGenerationTunnelWidth - lvlGenerationTunnelInlineBorder),
                    UnityEngine.Random.Range(lvlGenerationTunnelInlineBorder, lvlGenerationTunnelHeight - lvlGenerationTunnelInlineBorder),
                    lvlGenerationCurrent * lvlGenerationMeshDistance);
                lvlGenerationGameObjects.Add(collectable);
            }
            ++lvlGenerationCurrent;
        }
        for (int i = lvlGenerationGameObjects.Count - 1; i >= 0; i--)
        {
            GameObject lvlGenerationGameObject = lvlGenerationGameObjects[i];
            if (transform.position.z - lvlGenerationGameObject.transform.position.z > lvlGenerationMeshDistance * lvlGenerationBackwards) {
                lvlGenerationGameObjects.RemoveAt(i);
                Destroy(lvlGenerationGameObject);
            }
        }
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Collectable") {
            lvlGenerationGameObjects.Remove(col.gameObject);
            Destroy(col.gameObject);
            score++;
            UpdateScoreDisplay();
        }
    }
}
