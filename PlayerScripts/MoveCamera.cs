using UnityEngine;

public class MoveCamera : MonoBehaviour {

    public Transform player;
    public float cameraOffset = 0f;

    void Update() {
        transform.position = new Vector3(player.transform.position.x,player.transform.position.y+cameraOffset,player.transform.position.z);;
    }
}
