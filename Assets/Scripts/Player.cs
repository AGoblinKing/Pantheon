using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public ArrayList deck = new ArrayList();
	public ArrayList hand = new ArrayList();

	public int handSize = 5;
	public int deckSize = 60;

	private GameObject lastCloned;

	public void AddDeckTile(GameObject tile) {
		deck.Add (tile);
		tile.transform.parent = transform;
	}

	public bool drawTile() {
		if (deck.Count > 0) {
			GameObject nextTile = (GameObject)deck[0];

			nextTile.transform.parent = Camera.main.transform;
			nextTile.tag = "Selectable";

			hand.Add (deck[0]);
			deck.RemoveAt (0);

			MoveTiles ();

			return true;
		} else {
			return false;
		}
	}

	void MoveTiles() {
		foreach (GameObject nextTile in hand) {
			// move into screen
			Tile tile = nextTile.GetComponent<Tile>();
			tile.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 10*hand.IndexOf(nextTile) + 100, Screen.height / 10, Camera.main.nearClipPlane*30)); 
		}
	}

	public bool DrawTiles() {
		int count = 0;
		int deckCount = 0;
 
		while (count < handSize && drawTile()) {
			count++;
		}

		return deck.Count > 0;
	}

	public void placeTile() {
		GameObject placed = lastCloned;
		hand.RemoveAt (hand.IndexOf (lastCloned));
		GameObject.Destroy (placed);
		MoveTiles ();
	}

	public void startTurn() {
		if (hand.Count < handSize) {
			drawTile ();
		}
	}

	public void endTurn() {

	}

	void Update() {
		if (lastCloned) {
			lastCloned.transform.Rotate(new Vector3(0, 5, 0));
		}
	}

	public GameObject cloneAt(int tileId) {

		if (tileId < hand.Count) {
			lastCloned = (GameObject)hand[tileId];
			return GameObject.Instantiate (lastCloned, new Vector3 (0, 50, 0), Quaternion.identity) as GameObject;
		} else {
			return null;
		}
	}
}
