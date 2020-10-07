using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private float pickUpTime;
    private List<float> speeds = new List<float>();

    public void PickUpPassenger() {
        pickUpTime = Time.time;
    }

    public void DropOffPassenger(Planet origin, Planet destination) {
        var journeyTime = Time.time - pickUpTime;
        var euclideanDistance = (origin.transform.position - destination.transform.position).magnitude;
        var speedRating = euclideanDistance / journeyTime;
        speeds.Add(speedRating);
    }

    // Compute score when the game ends
    public float GameOver() {
        if (speeds.Count > 0) {
            return speeds.Average();
        }
        return 0.0f;
    }
}
