using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public string north = "grass";
    public string south = "grass";
    public string west = "grass";
    public string east = "grass";
    private int rotations = 0;
    public bool TestPlacement(GameObject testee, int dir) {
        Tile test = testee.GetComponent<Tile>();

        string dst = "!";
        string src = "!!";

        int targetdir = dir - 2;
        if (dir < 2) {
            targetdir = dir + 2;
        }

        dst = test.GetFace(targetdir);
        src = GetFace(dir);

        return dst == src;
    }

    public string GetFace(int dir) {
        rotations = rotations % 4;
        switch (dir - rotations) {
            case 0:
                return north;
            case -3:
            case 1:
                return east;
            case -2:
            case 2:
                return south;
            case -1:
            case 3:
                return west;
        }
        return "";
    }

    public void Rotate(int dir) {
        rotations++;
        transform.Rotate(Vector3.up, Mathf.Sign(dir) * 90); 
    }

}
