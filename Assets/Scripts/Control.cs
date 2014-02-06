using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {
    private GameObject currentLand;
    private GameObject[] land;
    private Dictionary<string, GameObject> placedLand;

    // Use this for initialization
    void Start() {
        placedLand = new Dictionary<string, GameObject>();
        land = GameObject.FindGameObjectsWithTag("Tiles");
        NewSelection();
        PlaceLand(Vector3.zero);
    }


    bool CheckPlacement() {
        bool results = true;
        bool tried = false;
        Vector3 location = currentLand.transform.position;
        for (int x = 0; x < 4; x++) {
            int ax = x % 2;
            int ay = (x + 1) % 2;

            if(x > 1) {
                ax *= -1;
                ay *= -1;
            }

            Vector3 key = new Vector3(location.x + ax, 0, location.z + ay );
            GameObject target;

            if (placedLand.TryGetValue(key.ToString(), out target)) {
                tried = true;
                results = results && currentLand.GetComponent<Tile>().TestPlacement(target, x);
            }
           
        }

        return results && tried;
    }

    void PlaceLand(Vector3 location) {
        currentLand.transform.position = location;
        placedLand.Add(location.ToString(), currentLand);
        NewSelection();
    }

    // Provide a newly selected piece
    void NewSelection() {
        currentLand = GameObject.Instantiate(land[Random.Range(0, land.Length - 1)], new Vector3(0,50,0), Quaternion.identity) as GameObject;
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

            if (!placedLand.ContainsKey(point.ToString())) {
                currentLand.transform.position = point;
            }
        }

        if (Input.GetButtonDown("Fire1") && CheckPlacement()) {
            PlaceLand(currentLand.transform.position);
        }

        if (Input.GetButtonDown("Fire2")) {
            currentLand.GetComponent<Tile>().Rotate(1);
        }
        
        if(Input.GetButtonDown("Fire3")) {
            Destroy(currentLand);
            NewSelection();
        }
    }
}
