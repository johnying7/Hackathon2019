using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
#region Private Variables & Properties
    [SerializeField]
    // private float speed_of_cam = 5.0f;
    private float side_bounds = 50.0f;
    private float lower_bound = 8.0f;
    private float top_bound   = 50.0f;
    public enum PointingDirection {forward=1,left=2,right=3,backward=4};
    public PointingDirection current_dir;
#endregion

    void Start() {
        current_dir = PointingDirection.forward;
        side_bounds = GameObject.Find("PathManager").GetComponent<CreateGrid>().squareSize * 10.0f / 2.0f;
    }
    // Update is called once per frame
    void Update()
    {
        PlayerTransformPosition();
    }

    private void PlayerTransformPosition () {
        float x_change = Input.GetAxis("Horizontal");
        float y_change = Input.GetAxis("Vertical");
        float z_change = Input.GetAxis("Flight");

        float rot_change = Input.GetAxis("RotateCamera");

        if (Input.GetKeyUp("e")){ 
            RotatePlayer(true); 
        } else if (Input.GetKeyUp("q")){ 
            RotatePlayer(false); 
        }

        Vector3 new_transform;
        if (current_dir == PointingDirection.forward) {
            new_transform = CheckBounds(x_change, y_change, z_change);
        } else if (current_dir == PointingDirection.left) {
            new_transform = CheckBounds(-y_change, x_change, z_change);
        } else if (current_dir == PointingDirection.right) {
            new_transform = CheckBounds(y_change, -x_change, z_change);
        } else {
            new_transform = CheckBounds(-x_change, -y_change, z_change);
        }
        this.transform.position += new_transform;
    }

    private Vector3 CheckBounds (float x, float y, float z){
        Vector3 cur_pos = this.transform.position;

        float new_x = x + cur_pos.x;
        float new_y = y + cur_pos.z;
        float new_z = z + cur_pos.y;

        float offset = side_bounds;
        float edgeView = 10.0f;

        if(new_x > side_bounds + offset + edgeView || new_x < -side_bounds + offset - edgeView) {
            x = 0f;
        }

        if(new_y > side_bounds + offset + edgeView || new_y < -side_bounds + offset - edgeView) {
            y = 0f;
        }

        if(new_z > top_bound || new_z < lower_bound) {
            z = 0f;
        }
        return(new Vector3(x, z, y));
    }

    public void RotatePlayer(bool direction) {
        // direction is true for right, false for left
        float y_change = 90.0f;
        if (!direction) {
            y_change = -y_change;

            if (current_dir == PointingDirection.forward) {
                current_dir = PointingDirection.left;
            } else if (current_dir == PointingDirection.left) {
                current_dir = PointingDirection.backward;
            } else if (current_dir == PointingDirection.right) {
                current_dir = PointingDirection.forward;
            } else {
                current_dir = PointingDirection.right;
            }
        } else {
            if (current_dir == PointingDirection.forward) {
                current_dir = PointingDirection.right;
            } else if (current_dir == PointingDirection.left) {
                current_dir = PointingDirection.forward;
            } else if (current_dir == PointingDirection.right) {
                current_dir = PointingDirection.backward;
            } else {
                current_dir = PointingDirection.left;
            }
        }

        Vector3 rot = transform.rotation.eulerAngles;

        // make the change and don't if limits are hit
        float rot_ch = CalculatedRotation(y_change,rot);
        transform.eulerAngles = new Vector3(rot.x, rot_ch, rot.z);
    }

    float CalculatedRotation(float y_change,Vector3 rot) {
        float new_y = rot.y + y_change;
        return (new_y);
    }
}
