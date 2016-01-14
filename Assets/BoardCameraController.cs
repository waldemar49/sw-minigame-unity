using UnityEngine;
using System.Collections;

public class BoardCameraController : MonoBehaviour {
    public GameObject board;

    void Start() {
        transform.eulerAngles = Vector3.up;
    }

	void Update () {
        transform.position = new Vector3(board.transform.position.x, board.transform.position.y + 1, board.transform.position.z - 4);
	}
}
