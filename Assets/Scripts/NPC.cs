using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Vehicle {
    public float oncomingCheckTimer = 0.4f;

    void Start() {
        orbitingPlanet = startingPlanet.GetComponent<Planet>();
        orbitingPlanet.AddCapacity(this);
        currentAngle = Random.Range(0.0f, TWOPI);
        if (Random.value < 0.5f) {
            direction = 1;
        } else {
            direction = -1;
        }
    }

    void Update() {
        switchingOrbit = false;
        oncomingCheckTimer -= Time.deltaTime;
        if (oncomingCheckTimer <= 0.0f) {
            oncomingCheckTimer = 0.4f;
            if (CanSwitchOrbit()) {
                if (direction == 1) {
                    input = InputEnum.Left;
                } else {
                    input = InputEnum.Right;
                }
            } else {
                input = InputEnum.None;
            }
        }
        UpdateAngle();
        UpdatePos();
        orbitingPlanet.DetectCollision(this);
    }

    bool CanSwitchOrbit() {
        return orbitingPlanet.CanSwitchOut(
            currentAngle, direction) && Random.value < 0.5f;
    }
}
