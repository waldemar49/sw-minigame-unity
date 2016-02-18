using UnityEngine;
using System.Collections;

public class BoardCameraController : MonoBehaviour {
    public GameObject board;

    private FlightBoardController flightBoardController;
    private Vector3 firstPersonTranslation = new Vector3(0.0f, 1.5f, 0.0f);
    private Vector3 thirdPersonTranslation = new Vector3(0.0f, 1.0f, -4.0f);

    void Start() {
        flightBoardController = board.GetComponent<FlightBoardController>();
        transform.eulerAngles = Vector3.up;
    }

    void Update () {
        transform.position = board.transform.position;
        if (flightBoardController.thirdPersonView) {
            transform.position = transform.position + thirdPersonTranslation;
        } else {
            transform.position = transform.position + firstPersonTranslation;
        }
        Vector3 r = transform.eulerAngles;
        r.y = flightBoardController.sideways ? (flightBoardController.sidewaysRightIsForward ? -90.0f : 90.0f) : 0.0f;
        transform.eulerAngles = r;
    }
}
