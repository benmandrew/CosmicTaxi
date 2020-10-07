using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum InputEnum {
    None, Left, Right
}


public class Vehicle : MonoBehaviour {
    public GameObject startingPlanet;
    public Planet orbitingPlanet;
    private float linearSpeed = 2.5f;
    public float currentAngle = 0.0f;
    // Clockwise = 1, anticlockwise = -1
    // so can just multiply angle change
    public int direction = 1;
    public InputEnum input;
    public int takenLaneIdx;
    public bool switchingOrbit = false;


    protected static float TWOPI = 2 * Mathf.PI;

    protected void UpdateAngle() {
        float angularSpeed = linearSpeed / orbitingPlanet.orbitRadius;
        float newAngle = currentAngle + direction * angularSpeed * Time.deltaTime;
        if (input != InputEnum.None) {
            newAngle = SwitchOrbits(newAngle);
        }
        if (newAngle < 0f) {
            newAngle += TWOPI;
        } else if (newAngle > TWOPI) {
            newAngle -= TWOPI;
        }
        currentAngle = newAngle;
        Vector3 facing = transform.eulerAngles;
        facing.z = -Mathf.Rad2Deg * (currentAngle + 0.5f * direction * Mathf.PI);
        transform.eulerAngles = facing;
    }

    float SwitchOrbits(float newAngle) {
        // If the player is turning in the wrong direction
        if (direction == 1 && input == InputEnum.Right ||
            direction == -1 && input == InputEnum.Left) {
            return newAngle;
        }
        int idx = 0;
        foreach (float connectionAngle in orbitingPlanet.connectionAngles) {
            if (currentAngle < connectionAngle && newAngle >= connectionAngle ||
                currentAngle > connectionAngle && newAngle <= connectionAngle) {
                orbitingPlanet.RemoveCapacity(this);
                orbitingPlanet = orbitingPlanet.connections[idx];
                direction = -direction;
                orbitingPlanet.AddCapacity(this);
                switchingOrbit = true;
                return newAngle + Mathf.PI;
            }
            idx++;
        }
        return newAngle;
    }

    protected void UpdatePos() {
        Vector3 disp = new Vector3(
            orbitingPlanet.orbitRadius * Mathf.Sin(currentAngle),
            orbitingPlanet.orbitRadius * Mathf.Cos(currentAngle),
            0);
        transform.position = orbitingPlanet.transform.position + disp;
    }

    public int GetDirection() {
        return direction;
    }

    public virtual void BlowUpFromCollision() {
        Destroy(gameObject);
    }
}
