using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public static MainCameraController instance;

    public float ScreenEdgeBorderThickness = 5.0f; // distance from screen edge. Used for mouse movement

    [Header("Camera Mode")]
    [Space]
    public bool RTSMode = true;
    public bool FlyCameraMode = false;

    [Header("Movement Speeds")]
    [Space]
    public float minPanSpeed;
    public float maxPanSpeed;
    public float secToMaxSpeed; //seconds taken to reach max speed;
    //public float zoomSpeed;

    [Header("Movement Limits")]
    [Space]
    public bool enableMovementLimits;
    public Vector2 heightLimit;
    public Vector2 lenghtLimit;
    public Vector2 widthLimit;
    //private Vector2 zoomLimit;

    private float panSpeed;
    private Vector3 initialPos;
    private Vector3 panMovement;
    private Vector3 pos;
    private Quaternion rot;
    private bool rotationActive = false;
    private Vector3 lastMousePosition;
    private Quaternion initialRot;
    private float panIncrease = 0.0f;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

    }

    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
        //zoomLimit.x = 15;
        //zoomLimit.y = 65;


    }

    void Update()
    {

        # region Camera Mode

        //check that ony one mode is choosen
        if (RTSMode == true) FlyCameraMode = false;
        if (FlyCameraMode == true) RTSMode = false;

        #endregion

        #region Movement

        panMovement = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - ScreenEdgeBorderThickness)
        {
            panMovement += Vector3.up * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= ScreenEdgeBorderThickness)
        {
            panMovement += Vector3.down * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= ScreenEdgeBorderThickness)
        {
            panMovement += Vector3.left * panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - ScreenEdgeBorderThickness)
        {
            panMovement += Vector3.right * panSpeed * Time.deltaTime;
        }
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    panMovement += Vector3.up * panSpeed * Time.deltaTime;
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    panMovement += Vector3.down * panSpeed * Time.deltaTime;
        //}

        if (RTSMode) transform.Translate(panMovement, Space.World);
        else if (FlyCameraMode) transform.Translate(panMovement, Space.Self);


        //increase pan speed
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)
            || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)
            //|| Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q)
            || Input.mousePosition.y >= Screen.height - ScreenEdgeBorderThickness
            || Input.mousePosition.y <= ScreenEdgeBorderThickness
            || Input.mousePosition.x <= ScreenEdgeBorderThickness
            || Input.mousePosition.x >= Screen.width - ScreenEdgeBorderThickness)
        {
            panIncrease += Time.deltaTime / secToMaxSpeed;
            panSpeed = Mathf.Lerp(minPanSpeed, maxPanSpeed, panIncrease);
        }
        else
        {
            panIncrease = 0;
            panSpeed = minPanSpeed;
        }

        #endregion

        #region boundaries

        if (enableMovementLimits == true)
        {
            //movement limits
            pos = transform.position;
            pos.y = Mathf.Clamp(pos.y, heightLimit.x, heightLimit.y);
            pos.z = Mathf.Clamp(pos.z, lenghtLimit.x, lenghtLimit.y);
            pos.x = Mathf.Clamp(pos.x, widthLimit.x, widthLimit.y);
            transform.position = pos;
        }



        #endregion
    }


    //Used to move the camera to the clicked position
    //private void FixedUpdate()
    //{
    //    if (moveTo != null && transform.position.x != moveTo.Value.x && transform.position.y != moveTo.Value.y)
    //    {
    //        Vector3 position = new Vector3(moveTo.Value.x, moveTo.Value.y, transform.position.z);
    //        var t = Time.deltaTime * timeToMove;
    //        transform.position = Vector3.MoveTowards(transform.position, position, t);
    //    }
    //}

    //public void MoveToPosition(Vector2 position)
    //{
    //    moveTo = position;
    //}
}
