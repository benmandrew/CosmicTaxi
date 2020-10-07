using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PassengerManager : MonoBehaviour
{
    private ScoreManager scoreManager;
    private GameMenu gameMenu;
    private Planet currentlyOrbiting;

    private CanvasController canvas;

    public Queue<Passenger> passengers;

    Galaxy galaxy;

    Taxi taxi;

    bool passengersSet = false;

    private Planet origin;
    private Planet destination;
    private Passenger currPassenger;
    public bool inTaxi;
    public bool delivered;

    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<CanvasController>();
        galaxy = FindObjectOfType<Galaxy>();
        taxi = FindObjectOfType<Taxi>();
        scoreManager = FindObjectOfType<ScoreManager>();
        gameMenu = FindObjectOfType<GameMenu>();
        delivered = false;
    }

    // Called by the Passenger List script once it has created a list of all the passengers
    public void SetPassengers(List<Passenger> passengers) 
    {
        this.passengers = new Queue<Passenger>(passengers);
        //Debug.Log(this.passengers.Count);
        passengersSet = true;
        // Initilise passenger gameplay loop
        StartCoroutine(SpawnPassenger());
    }

    public Passenger GetPassenger() 
    {
        return currPassenger;
    }

    public Planet GetOrigin() 
    {
        return origin;
    }

    public Planet GetDestination() 
    {
        return destination;
    }

    IEnumerator SpawnPassenger() {
        if (passengersSet && passengers.Count >= 1)
        {
            //Wait some seconds
            delivered = false;
            yield return new WaitForSeconds(Random.Range(2, 5));
            Debug.Log("PASSENGER");
            // Randomly select two planets, the pick up and drop off points
            var endPoints = new List<Planet>(galaxy.planets);
            endPoints.Remove(taxi.orbitingPlanet);
            System.Random random = new System.Random();
            var selectedEndPoints = endPoints.OrderBy(p => random.Next()).Take(2).ToList();

            // Select the next passenger from the queue (assuming non-empty)
            var passenger = passengers.Dequeue();
            currPassenger = passenger;
            origin = selectedEndPoints[0];
            destination = selectedEndPoints[1];
            inTaxi = false;

            canvas.SetPassenger(currPassenger, origin, destination);
            Debug.Log("Passenger: " + currPassenger.passengerName + " is waiting at Planet: " + origin.name + " to go to Planet: " + destination.name);
        }
    }

    public void SwitchOrbit(Planet planet) 
    {
        currentlyOrbiting = planet;
        if (currentlyOrbiting == origin && !inTaxi) 
        {
            // Pick up the passenger
            PickUpAnimation();
            inTaxi = true;
            scoreManager.PickUpPassenger();
        }
        if (currentlyOrbiting == destination && inTaxi) 
        {
            // Drop off the passenger
            DropOffAnimation();
            inTaxi = false;
            delivered = true;
            currPassenger = null;
            scoreManager.DropOffPassenger(origin, destination);
            if (passengers.Count == 0) {
                gameMenu.NotifyWin(scoreManager.GameOver());
            }
            //SpawnPassenger();
        }
    }

    public void MonitorClosed() 
    {
        if (delivered) 
        {
            if (passengers.Count == 0) {
                // If player closes monitor manually, re-notify of win to override the timer
                gameMenu.NotifyWin(scoreManager.GameOver());
            }
            StartCoroutine(SpawnPassenger());
        }
    }

    public void PickUpAnimation() 
    {
        // Handle the passenger pick up stuff
        canvas.NotifyPickUp();
        Debug.Log(currPassenger.pickUpText);
    }

    public void DropOffAnimation()
    {
        // Handle the passenger drop off stuff
        canvas.NotifyDropOff();
        Debug.Log(currPassenger.dropOffText);
    }
}
