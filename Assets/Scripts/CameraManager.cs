using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    private Vector3 prevFocusedPos;
    private Planet newFocusedPlanet;
    private bool switchingFocus = false;
    private Camera cam;

    float timeSinceSwitch;

    void Start() {
        cam = Camera.main;
        prevFocusedPos = cam.transform.position;
    }

    void Update() {
        if (switchingFocus) {
            timeSinceSwitch += Time.deltaTime;
            if (timeSinceSwitch < 1.0f) {
                cam.transform.position = EaseInOutQuad(
                    prevFocusedPos,
                    newFocusedPlanet.transform.position,
                    timeSinceSwitch
                );
            } else {
                switchingFocus = false;
                cam.transform.position = newFocusedPlanet.transform.position;
                prevFocusedPos = cam.transform.position;
            }
            ResetCamZCoord();
        }
    }

    public void SwitchPlanet(Planet newPlanet) {
        timeSinceSwitch = 0.0f;
        newFocusedPlanet = newPlanet;
        switchingFocus = true;
        prevFocusedPos = cam.transform.position;
    }

    Vector3 EaseOut(Vector3 start, Vector3 end, float v) {
        end -= start;
        return -end * v * (v - 2) + start;
    }

    Vector3 EaseInOutQuad(Vector3 start, Vector3 end, float v) {
        v /= .5f;
        end -= start;
        if (v < 1) return end * 0.5f * v * v + start;
        v--;
        return -end * 0.5f * (v * (v - 2) - 1) + start;
    }

    void ResetCamZCoord() {
        Vector3 v = cam.transform.position;
        v.z = -1f;
        cam.transform.position = v;
    }
}
