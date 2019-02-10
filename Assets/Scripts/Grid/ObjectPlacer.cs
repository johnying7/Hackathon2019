using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacer : MonoBehaviour
{
#region Private Variables
    private Gridy grid;
    public GameObject cur_gameObject = null;
    public GameObject placing_object = null;
    public GameManager gM;
    private FindNearNode path_man;
    public float roomCost;
    public ScrollList scrollList;
    int currentRotPos = 0; //forward = 0, right = 1, back = 2, left = 3
    public Transform selectIndicator;

    public AudioSource sfxSource;
    public AudioClip[] sfxList = new AudioClip[2]; //cash, blop
    public AudioSource bgmSource;
    private int curBgm = 1;
    public AudioClip[] bgmList = new AudioClip[2]; //babysteps, groovy
#endregion

    // Reference: https://www.youtube.com/watch?v=VBZFYGWvm4A
    void Awake()
    {
        // Find Grid In Scene
        grid = FindObjectOfType<Gridy>();
    }

    void Start() {
        path_man = GameObject.Find("PathManager").GetComponent<FindNearNode>();
        //GameObject.Find("GameManager").GetComponent<GameManager>();
        //
        // TODO: implement clear selection
        //
        print("still need to implement delete room AND wall pathing");
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseInput();
        if (Input.GetKeyUp(KeyCode.C)) //deselect current room
        {
            cur_gameObject = null;
            placing_object = null;
            if (selectIndicator.gameObject.activeSelf)
            {
                selectIndicator.gameObject.SetActive(false);
            }
            selectIndicator.GetComponent<RoomEmployeeSelect>().sQ = null;
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            PlayBgm();
        }
    }

    /// <summary>
    /// 0=cash, 1=blop
    /// </summary>
    /// <param name="index"></param>
    public void PlaySfx(int index)
    {
        sfxSource.clip = sfxList[index];
        sfxSource.Play();
    }

    public void PlayBgm()
    {
        if(curBgm == bgmList.Length-1)
        {
            curBgm = 0;
        }
        else
        {
            curBgm++;
        }
        bgmSource.clip = bgmList[curBgm];
        bgmSource.Play();
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
            else
            {
                //if (Input.GetMouseButtonDown(0))
                //{
                //    print("mouse button 1 down");
                //    print("with tag: " + hit_pos.transform.tag);
                //}

                if(Input.GetMouseButtonDown(0)
                    && (hit_pos.transform.tag == "hotel_item" || hit_pos.transform.parent.tag == "hotel_item")
                    && !EventSystem.current.IsPointerOverGameObject())
                {
                    float new_x = hit_pos.point.x;
                    float new_z = hit_pos.point.z;
                    Vector3 new_transform = new Vector3(new_x, -1.0f, new_z);
                    //select room
                    Transform myNode = path_man.getNearestSnap(new_transform);
                    Vector3 indicatorPosition = myNode.position;
                    indicatorPosition.y = 10.0f;
                    if (!selectIndicator.gameObject.activeSelf)
                    {
                        selectIndicator.gameObject.SetActive(true);
                    }
                    selectIndicator.position = indicatorPosition;
                    selectIndicator.GetComponent<RoomEmployeeSelect>().sQ = myNode.GetComponent<ServiceQueue>();
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
            
            ///
            /// Set place room code here....
            ///

            //set room type rotation
            Room roomScript = cur_gameObject.GetComponent<Room>();
            drop_position.GetComponent<Node>().setRoom(roomScript.roomType, (facingDirection)currentRotPos);
            path_man.GetComponent<CreateGrid>().roomList[(int)roomScript.roomType].Add(drop_position);
            gM.charge(-roomCost);
            PlaySfx(1);
            scrollList.RefreshDisplay();

            ///
            /// End room placement code here...
            /// 

            //clear selections
            cur_gameObject = null;
            placing_object = null;
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
