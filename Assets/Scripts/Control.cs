using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {
    private GameObject currentLand;

    private GameObject[] land;
	private GameObject[] players;
    private Dictionary<string, GameObject> placedLand;
	private Player currentPlayer;
	private Vector3 mousePos;

    // Use this for initialization
    void Start() {
        placedLand = new Dictionary<string, GameObject>();
        land = GameObject.FindGameObjectsWithTag("Tiles");
		players = GameObject.FindGameObjectsWithTag ("Player");

		PopulateDecks ();
		currentLand = InstantiateRandom ();
        PlaceLand(Vector3.zero);
		currentLand = null;
    }

	void PopulateDecks() {
		foreach (GameObject player in players) {
			Player ply = player.GetComponent<Player>();
			for(int x = 0; x < ply.deckSize; x++) {
				ply.AddDeckTile(InstantiateRandom());
			}
			ply.DrawTiles ();
			currentPlayer = ply;
		}
	}

    bool CheckPlacement(bool checkMatch) {
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
				if(checkMatch) {
                	results = results && currentLand.GetComponent<Tile>().TestPlacement(target, x);
				} else {
					results = true;
				}
            }
           
        }

        return results && tried;
    }

    void PlaceLand(Vector3 location) {
        currentLand.transform.position = location;
        placedLand.Add(location.ToString(), currentLand);
		currentLand = null;
    }

	GameObject getRandom() {
		return land [Random.Range (0, land.Length - 1)];
	}

	GameObject InstantiateRandom() {
		return GameObject.Instantiate(getRandom (), new Vector3(0,50,0), Quaternion.identity) as GameObject;
	}

    // Update is called once per frame
    void Update() {
        Plane cross = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		bool overPlace = false;
        float distance;
		Vector2 dragDif = -(Input.mousePosition - mousePos);

		if (dragDif.x > 400 || dragDif.y > 400 || dragDif.x < -400 || dragDif.y < -400) {
			dragDif = new Vector2(0,0);
		}

		if (currentLand && cross.Raycast(ray, out distance)) {
            Vector3 point = ray.GetPoint(distance);
            point.x = Mathf.Round(point.x);
            point.z = Mathf.Round(point.z);

            if (!placedLand.ContainsKey(point.ToString())) {
				currentLand.transform.position = point;

				if(!CheckPlacement(false)) {
					currentLand.transform.position = new Vector3(0, 500, 0);
				} else {
					overPlace = true;
				}
            }
        }

		bool dragPossible = true;
        if (Input.GetButtonDown("Fire1")) {
			RaycastHit[] hits;
			Transform selection = null;

			hits = Physics.RaycastAll(ray);

			foreach(RaycastHit hit in hits) {
				if(hit.transform.tag == "Selectable") {
					selection = hit.transform;
					if(currentLand)
						Destroy(currentLand);

					currentLand = currentPlayer.cloneAt (currentPlayer.hand.IndexOf (hit.transform.gameObject));
					currentLand.transform.parent = null;
					currentLand.tag = "Tiles";
					dragPossible = false;
				}
			}

			if(selection == null) {
				if(currentLand && CheckPlacement(true) && overPlace) {
	            	PlaceLand(currentLand.transform.position);
					currentPlayer.placeTile ();
					dragPossible = false;
				} 
			}
        }

		if (Input.GetMouseButton (0) && dragPossible) {
			// Dragging
			Camera.main.transform.Translate (new Vector3(dragDif.x * 0.01f, dragDif.y * 0.01f, 0));
		}

		bool rotatePossible = true;
        if (Input.GetButtonDown("Fire2") && currentLand && overPlace) {
            currentLand.GetComponent<Tile>().Rotate(1);
			rotatePossible = false;
        }

		if (Input.GetButton("Fire2") && rotatePossible) {
			Vector3 camCollide;
			Ray camRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
			cross.Raycast(camRay, out distance);
			camCollide = ray.GetPoint (distance);
			camCollide.z = 0;

			Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, dragDif.x * -0.5f);
		}

		if (Input.GetKeyUp ("space")) {
			currentPlayer.endTurn ();
			currentPlayer.startTurn ();
		}

		mousePos = Input.mousePosition;
    }
}
