using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Vehicle {
    float rotAngle;
    float rotSpeed;

    void Start() {
        rotAngle = Random.Range(0.0f, TWOPI);
        rotSpeed = Random.Range(2.0f, 4.0f);
        orbitingPlanet = startingPlanet.GetComponent<Planet>();
        orbitingPlanet.AddCapacity(this);
    }

    void Update() {
        UpdateAngle();
        UpdatePos();
        rotAngle += rotSpeed * Time.deltaTime;
        if (rotAngle < 0f) {
            rotAngle += TWOPI;
        } else if (rotAngle > TWOPI) {
            rotAngle -= TWOPI;
        }
        Vector3 facing = transform.eulerAngles;
        facing.z = rotAngle;
        transform.eulerAngles = Mathf.Rad2Deg * facing;
        orbitingPlanet.DetectCollision(this);
    }
}
