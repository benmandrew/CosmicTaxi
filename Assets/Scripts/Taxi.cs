using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Taxi : Vehicle {
    private CameraManager cameraManager;
    private PassengerManager passengerManager;

    void Start() {
        orbitingPlanet = startingPlanet.GetComponent<Planet>();
        orbitingPlanet.AddCapacity(this);
        passengerManager = FindObjectOfType<PassengerManager>();
        passengerManager.SwitchOrbit(orbitingPlanet);
        cameraManager = FindObjectOfType<CameraManager>();
    }

    void Update() {
        UpdateAngle();
        UpdatePos();
        input = InputEnum.None;
        if (Input.GetKey(KeyCode.LeftArrow)) {
            input = InputEnum.Left;
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            input = InputEnum.Right;
        }
        if (switchingOrbit) {
            cameraManager.SwitchPlanet(orbitingPlanet);
            passengerManager.SwitchOrbit(orbitingPlanet);
            switchingOrbit = false;
        }
        orbitingPlanet.DetectCollision(this);
    }

    public override void BlowUpFromCollision() {
        GameMenu gm = FindObjectOfType<GameMenu>();
        gm.NotifyDeath();
        Arrow a = FindObjectOfType<Arrow>();
        a.taxiDead = true;
        Destroy(gameObject);
    }
}
