using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
#region Private Variables
    private Gridy grid;
    public GameObject cur_gameObject = null;
    public GameObject placing_object = null;
    private FindNearNode path_man;
    int currentRotPos = 0; //forward = 0, right = 1, back = 2, left = 3
#endregion

    // Reference: https://www.youtube.com/watch?v=VBZFYGWvm4A
    void Awake()
    {
        // Find Grid In Scene
        grid = FindObjectOfType<Gridy>();
    }

    void Start() {
        path_man = GameObject.Find("PathManager").GetComponent<FindNearNode>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseInput();
    }

    private void GetMouseInput () {
        RaycastHit hit_pos;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit_pos, Mathf.Infinity)) {
            //Choose from list of objects in GUI
            
            if((placing_object != null)) {
                float new_x = hit_pos.point.x;
                float new_z = hit_pos.point.z;
                Vector3 new_transform = new Vector3(new_x,-1.0f,new_z);
                // placing_object.transform.position = new_transform;

                SnapIt(new_transform);

                if (Input.GetKeyUp("f")) {
                    RotateObj();
                }
            
                if (Input.GetMouseButtonDown(0) && CheckPlaceCollision() == false) {
                    placing_object.GetComponent<CollisionCheck>().DeleteRB();
                    placing_object.GetComponent<CollisionCheck>().ChangeLayer();
                    PlaceObjectHere(new_transform);
                }
            }
        }
    }

    public void RotateObj() {
        // direction is true for right, false for left
        float y_change = 90.0f;
        Vector3 rot = placing_object.transform.rotation.eulerAngles;

        // make the change and don't if limits are hit
        float rot_ch = CalculatedRotation(y_change,rot);
        placing_object.transform.eulerAngles = new Vector3(rot.x, rot_ch, rot.z);

        //set room values
        if (currentRotPos == 3)
            currentRotPos = 0;
        else
            currentRotPos++;
    }

    float CalculatedRotation(float y_change,Vector3 rot) {
        float new_y = rot.y + y_change;
        return (new_y);
    }

    private void SnapIt(Vector3 pos) {
        var snap_pos = path_man.getNearestSnap(pos);// grid.GetGridPoint(pos);

        placing_object.transform.position = snap_pos.position;
        //placing_object.transform.rotation = Quaternion.identity;
    }

    private void PlaceObjectHere(Vector3 click_pos) {
        var drop_position = path_man.getNearestSnap(click_pos);// grid.GetGridPoint(click_pos);
        

        if(cur_gameObject != null) {
            //Instantiate(cur_gameObject, drop_position, Quaternion.identity);
            placing_object.transform.position = drop_position.position;
            cur_gameObject = null;
            placing_object = null;

            //set room type rotation
            Room roomScript = cur_gameObject.GetComponent<Room>();
            drop_position.GetComponent<Node>().roomType = roomScript.roomType;
            drop_position.GetComponent<Node>().currentFaceDirection = (facingDirection)currentRotPos;
        } else {
            Debug.Log("ERROR: No gameobject to place..");
        }
    }

    public void SetPlacementObject(GameObject gameObj) {
        cur_gameObject = gameObj;
        placing_object = Instantiate(cur_gameObject, Input.mousePosition, Quaternion.identity);
    }

    public bool CheckPlaceCollision() { 
        bool coll = placing_object.GetComponent<CollisionCheck>().GetCollisionState();
        return coll;
    }
}
