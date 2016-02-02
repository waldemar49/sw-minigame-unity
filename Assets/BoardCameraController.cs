using UnityEngine;
using System.Collections;

public class BoardCameraController : MonoBehaviour {
    public GameObject board;
    public bool thirdPersonView;

    private Vector3 firstPersonTranslation = new Vector3(0.0f, 1.5f, 0.0f);
    private Vector3 thirdPersonTranslation = new Vector3(0.0f, 1.0f, -4.0f);

    void Start() {
        transform.eulerAngles = Vector3.up;
    }

    void Update () {
        transform.position = board.transform.position;
        if (thirdPersonView) {
            transform.position = transform.position + thirdPersonTranslation;
        } else {
            transform.position = transform.position + firstPersonTranslation;
        }
    }
}
