using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    private PassengerManager manager;
    private Taxi taxi;
    private SpriteRenderer sr;
    private float arrowDistFromTaxi = 1.2f;
    public bool taxiDead;

    void Start() {
        manager = FindObjectOfType<PassengerManager>();
        taxi = FindObjectOfType<Taxi>();
        transform.position = new Vector3(0, 0, 0);
        sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (!taxiDead) {
            Vector3 dst = Vector3.zero;
            // Passenger dropped off and no new passenger for a few secs
            if (!manager.inTaxi && manager.GetPassenger() == null) {
                sr.enabled = false;
            } else {
                sr.enabled = true;
            }
            if (manager.inTaxi) {
                dst = manager.GetDestination().transform.position;
            } else if (manager.GetPassenger() != null) {
                dst = manager.GetOrigin().transform.position;
            }
            Vector3 disp = arrowDistFromTaxi * Vector3.Normalize(
                dst - taxi.transform.position);
            transform.position = taxi.transform.position + disp;
            Vector3 facing = transform.eulerAngles;
            facing.z = Mathf.Atan2(disp.y, disp.x) * Mathf.Rad2Deg - 90.0f;
            transform.eulerAngles = facing;
        } else {
            sr.enabled = false;
        }
    }
}
