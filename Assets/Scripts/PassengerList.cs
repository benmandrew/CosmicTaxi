using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerList : MonoBehaviour
{
    public List<Passenger> passengers;
    void Start()
    {
        // The gameobject this script is attached too should have all of the possible passengers as children. Add these passengers to the list.
        GameObject parent = this.gameObject;
        passengers.AddRange(parent.GetComponentsInChildren<Passenger>());
        FindObjectOfType<PassengerManager>().SetPassengers(passengers);
    }
}
