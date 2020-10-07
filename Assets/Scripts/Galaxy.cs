using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour {
	public List<Planet> planets = new List<Planet>();
	public Material lineMat;

	public GameObject planetContainer;

    void Awake() {
		GetPlanets();
    }

	void GetPlanets() {
		int idx = 0;
		foreach (Transform child in planetContainer.transform) {
			Planet planet = child.GetComponent<Planet>();
			planet.idx = idx++;
			planets.Add(planet);
		}
	}
}
