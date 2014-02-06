using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
    public float speed = 5.0f;
    public float scrollSpeed = 15.0f;

    // Update is called once per frame
    void Update() {
        Vector3 dst = new Vector3();

        if (Input.GetAxis("Vertical") > 0) {
            dst.y += speed;
        }

        if (Input.GetAxis("Vertical") < 0) {
            dst.y -= speed;
        }

        if (Input.GetAxis("Horizontal") > 0) {
            dst.x += speed;
        }

        if (Input.GetAxis("Horizontal") < 0) {
            dst.x -= speed;
        }

        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel != 0) {
            dst.z += Mathf.Sign(wheel) * scrollSpeed;
        }

        transform.Translate(dst * Time.deltaTime, Space.Self);
    }
}
