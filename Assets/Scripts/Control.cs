using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {
    private GameObject currentLand;
    private GameObject[] land;
    private Dictionary<Vector3, GameObject> placedLand;

    // Use this for initialization
    void Start() {
        placedLand = new Dictionary<Vector3, GameObject>();
        land = GameObject.FindGameObjectsWithTag("Tiles");
        NewSelection();
        PlaceLand(Vector3.zero);
    }


    void CheckPlacement(Vector3 location) {
        for (var x = -1; x < 2; x++) {
            for (var y = -1; y < 2; y++) {
                if (placedLand.ContainsKey(new Vector3(location.x + x, 0, location.y + y))) {
                    
                }
            }
        }
    }

    void PlaceLand(Vector3 location) {
        currentLand.transform.position = location;
        placedLand[location] = currentLand;
        NewSelection();
    }

    // Provide a newly selected piece
    void NewSelection() {
        currentLand = GameObject.Instantiate(land[Random.Range(0, land.Length - 1)]) as GameObject;
    }

    // Update is called once per frame
    void Update() {
        Plane cross = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;

        if (currentLand && cross.Raycast(ray, out distance)) {
            Vector3 point = ray.GetPoint(distance);
            point.x = Mathf.Round(point.x);
            point.z = Mathf.Round(point.z);

            currentLand.transform.position = point;
        }

        if (Input.GetButtonDown("Fire2")) {
            currentLand.GetComponent<Tile>().Rotate(1);
        }
    }
}
