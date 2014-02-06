using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public string north = "grass";
    public string south = "grass";
    public string west = "grass";
    public string east = "grass";


    public void TestPlacement() {

    }

    public void Rotate(int dir) {
        transform.Rotate(Vector3.up, Mathf.Sign(dir) * 90); 
    }

}
