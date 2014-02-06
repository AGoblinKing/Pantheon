using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public string north = "grass";
    public string south = "grass";
    public string west = "grass";
    public string east = "grass";

    public bool TestPlacement(GameObject testee, int dir) {
        Tile test = testee.GetComponent<Tile>();

        string dst = "!";
        string src = "!!";
        Debug.Log(dir);
        switch (dir) {
            case 0: // north
                dst = test.GetFace(0);
                src = GetFace(2);
                break;
            case 1: // east
                dst = test.GetFace(1);
                src = GetFace(3);
                break;
            case 2: // south
                dst = test.GetFace(2);
                src = GetFace(0);
                break;
            case 3: // west
                dst = test.GetFace(3);
                src = GetFace(1);
                break;
        }

        if (dst != src) {
            Debug.Log(dst);
            Debug.Log(src);
        }

        return dst == src;
    }

    public string GetFace(int dir) {
        Vector3 axis;
        float angle;

        transform.rotation.ToAngleAxis(out angle, out axis);

        int rotations = Mathf.RoundToInt(angle / 90);
        Debug.Log("Rotations for " + rotations);
        switch (dir + rotations) {
            case 4:
            case 0:
                return north;
            case 5:
            case 1:
                return east;
            case 6:
            case 2:
                return south;
            case 7:
            case 3:
                return west;
        }
        return "";
    }

    public void Rotate(int dir) {
        transform.Rotate(Vector3.up, Mathf.Sign(dir) * 90); 
    }

}
