using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    private bool isColliding;
    private Rigidbody rigidbody;

    void Awake() {
        isColliding = false;
        rigidbody = this.gameObject.AddComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;

        this.gameObject.layer = 8;
        //Debug.Log("awake");
    }

    void OnTriggerStay(Collider collision) {
        if (collision.gameObject.tag == "hotel_item" && !(isColliding)) {
            isColliding = true;
        } else if (isColliding && collision.gameObject.tag != "hotel_item") {
            isColliding = false;
        }
    }

    public bool GetCollisionState() {
        return isColliding;
    }

    public void ChangeLayer() {
        this.gameObject.layer = 0;
    }
    public void DeleteRB() {
        Destroy(rigidbody);
    }
}
