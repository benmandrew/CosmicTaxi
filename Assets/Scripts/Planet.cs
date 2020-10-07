using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
	public List<Planet> connections;
	public List<float> connectionAngles;
	public float orbitRadius = 3f;
	private int nOrbitSegments = 100;
	public LineRenderer orbit;
	public Material lineMat;
	public GameObject explosionPrefab;
	public int idx;
	private HashSet<Vehicle> clockwiseVehicles = new HashSet<Vehicle>();
	private HashSet<Vehicle> antiClockwiseVehicles = new HashSet<Vehicle>();
	private static float TWOPI = 2 * Mathf.PI;

	public AudioSource explosionSE;

    void Start() {
        orbit = GetComponent<LineRenderer>();
		orbit.positionCount = nOrbitSegments;
		orbit.loop = true;
		orbit.material = new Material(lineMat);
		orbit.startWidth = 0.02f;
		orbit.endWidth = 0.02f;
		SetOrbit();
		GetConnectionAngles();
    }

	void GetConnectionAngles() {
		foreach (Planet planet in connections) {
			Vector3 to = planet.transform.position - transform.position;
			float angle = Mathf.Deg2Rad * Vector3.Angle(
				new Vector3(0, 1, 0),
				to);
			if (to.x < 0.0f) {
				angle = TWOPI - angle;
			}
			connectionAngles.Add(angle);
		}
	}

    void Update() {
        SetOrbit();
    }

	void SetOrbit() {
		for (int i = 0; i < nOrbitSegments; i++) {
			Vector3 disp = new Vector3(
				orbitRadius * Mathf.Sin(2 * Mathf.PI * (i / (float) nOrbitSegments)),
				orbitRadius * Mathf.Cos(2 * Mathf.PI * (i / (float) nOrbitSegments)),
				1);
			orbit.SetPosition(i, transform.position + disp);
		}
	}

	public void AddCapacity(Vehicle vehicle) {
		if (vehicle.GetDirection() == 1) {
			clockwiseVehicles.Add(vehicle);
		} else {
			antiClockwiseVehicles.Add(vehicle);
		}
	}

	public void RemoveCapacity(Vehicle vehicle) {
		if (vehicle.GetDirection() == 1) {
			clockwiseVehicles.Remove(vehicle);
		} else {
			antiClockwiseVehicles.Remove(vehicle);
		}
	}

	public bool CanSwitchOut(float angle, int direction) {
		int idx = GetOncomingConnectionIdx(angle, direction);
		return connections[idx].HasCapacity(direction);
	}

	public bool HasCapacity(int direction) {
		if (direction == 1) {
			return clockwiseVehicles.Count == 0;
		}
		return antiClockwiseVehicles.Count == 0;
	}

	public int GetOncomingConnectionIdx(float angle, int direction) {
		int ret = 0;
		float closest = 0f;
		if (direction == 1) {
			closest = TWOPI;
		}
		int idx = 0;
		foreach (float connectionAngle in connectionAngles) {
			if (direction == 1 &&
				connectionAngle > angle &&
				connectionAngle < closest) {
				closest = connectionAngle;
				ret = idx;
			} else if (direction == -1 &&
				connectionAngle < angle &&
				connectionAngle > closest) {
				closest = connectionAngle;
				ret = idx;
			}
			idx++;
		}
		return ret;
	}

	public void DetectCollision(Vehicle vehicle) {
		// So that the collision distance is
		// linear distance rather than angle
		float delta = (Mathf.PI / 12f) / orbitRadius;
		bool collision = false;
		Vehicle collidedVehicle = null;
		if (vehicle.GetDirection() == 1) {
			foreach (Vehicle against in antiClockwiseVehicles) {
				if (NormaliseAngle(vehicle.currentAngle + delta) > against.currentAngle &&
					NormaliseAngle(vehicle.currentAngle - delta) < against.currentAngle &&
					vehicle != against) {
					collision = true;
					collidedVehicle = against;
				}
			}
		} else {
			foreach (Vehicle against in clockwiseVehicles) {
				if (NormaliseAngle(vehicle.currentAngle + delta) > against.currentAngle &&
					NormaliseAngle(vehicle.currentAngle - delta) < against.currentAngle &&
					vehicle != against) {
					collision = true;
					collidedVehicle = against;
				}
			}
		}
		if (collision && collidedVehicle != null) {
			if (collidedVehicle.orbitingPlanet != this) {
				// Prevents wierd concurrency error
				// where collision would occur even
				// after the vehicle left the system
				return;
			}
			float explosionAngle = (vehicle.currentAngle + collidedVehicle.currentAngle) / 2.0f;
			vehicle.BlowUpFromCollision();
			collidedVehicle.BlowUpFromCollision();
			ExplodeAtAngle(explosionAngle);
		}
	}

	private void ExplodeAtAngle(float angle) {
		Vector3 disp = new Vector3(
            orbitRadius * Mathf.Sin(angle),
            orbitRadius * Mathf.Cos(angle),
            0);
		Instantiate(explosionPrefab, transform.position + disp, Quaternion.identity, transform);
		explosionSE.Play();
	}

	private float NormaliseAngle(float angle) {
		while (angle < 0.0f || angle > TWOPI) {
			if (angle < 0.0f) {
				angle += TWOPI;
			} else {
				angle -= TWOPI;
			}
		}
		return angle;
	}
}
