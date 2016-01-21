using UnityEngine;
using System.Collections;

public class Recenter : MonoBehaviour {

    public KeyCode[] recenter;

    void Update() {
        if (allDown(recenter)) {
            UnityEngine.VR.InputTracking.Recenter();
        }
    }

    private bool allDown(KeyCode[] keys) {
        foreach (KeyCode key in keys) {
            if (!Input.GetKey(key)) {
                return false;
            }
        }
        return true;
    }
}
