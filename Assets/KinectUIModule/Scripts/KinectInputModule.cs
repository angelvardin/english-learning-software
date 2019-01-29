using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Windows.Kinect;
[AddComponentMenu("Kinect/Kinect Input Module")]
[RequireComponent(typeof(EventSystem))]
public class KinectInputModule : BaseInputModule
{
    public KinectInputData[] _inputData = new KinectInputData[0];
    [SerializeField]
    private float _scrollTreshold = .5f;
    [SerializeField]
    private float _scrollSpeed = 3.5f;
    [SerializeField]
    private float _waitOverTime = 0.5f;



    PointerEventData _handPointerData;

    static KinectInputModule _instance = null;
    public static KinectInputModule instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(KinectInputModule)) as KinectInputModule;

                if (!_instance)
                {
                    if (EventSystem.current){
                        EventSystem.current.gameObject.AddComponent<KinectInputModule>();
                        Debug.LogWarning("Add Kinect Input Module to your EventSystem!");
                    }
                    else
                    {
                        Debug.LogWarning("Create your UI first");

                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    ///  Call this from your Kinect body view from Update method
    /// </summary>
    /// <param name="body"></param>
    public void TrackBody(Body body)
    {
        for (int i = 0; i < _inputData.Length; i++)
        {
            _inputData[i].UpdateComponent(body);
        }
    }
    // get a pointer event data for a screen position
    private PointerEventData GetLookPointerEventData(Vector3 componentPosition)
    {
        if (_handPointerData == null)
        {
            _handPointerData = new PointerEventData(eventSystem);
        }
        _handPointerData.Reset();
        _handPointerData.delta = Vector2.zero;
        _handPointerData.position = componentPosition;
        _handPointerData.scrollDelta = Vector2.zero;
        eventSystem.RaycastAll(_handPointerData, m_RaycastResultCache);
        _handPointerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_RaycastResultCache.Clear();
        return _handPointerData;
    }
   
    public override void Process()
    {
        ProcessHover();
        ProcessPress();
        ProcessDrag();
        ProcessWaitOver();
    }
    /// <summary>
    /// Processes waitint over componens, if hovererd buttons click type is waitover, process it!
    /// </summary>
    private void ProcessWaitOver()
    {
        for (int j = 0; j < _inputData.Length; j++)
        {
            if (!_inputData[j].IsHovering || _inputData[j].ClickGesture != KinectUIClickGesture.WaitOver) continue;
            _inputData[j].WaitOverAmount = (Time.time - _inputData[j].HoverTime) / _waitOverTime;
            if (Time.time >= _inputData[j].HoverTime + _waitOverTime)
            {
                PointerEventData lookData = GetLookPointerEventData(_inputData[j].GetHandScreenPosition());
                GameObject go = lookData.pointerCurrentRaycast.gameObject;
                ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.submitHandler);
                // reset time
                _inputData[j].HoverTime = Time.time;
            }
        }
    }

    private void ProcessDrag()
    {
        for (int i = 0; i < _inputData.Length; i++)
        {
            // if not pressing we can't drag
            if (!_inputData[i].IsPressing)continue;
            //Debug.Log("drag " + Mathf.Abs(_inputData[i].TempHandPosition.x - _inputData[i].HandPosition.x));
            // Check if we reach drag treshold for any axis, temporary position set when we press an object
            if (Mathf.Abs(_inputData[i].TempHandPosition.x - _inputData[i].HandPosition.x) > _scrollTreshold || Mathf.Abs(_inputData[i].TempHandPosition.y - _inputData[i].HandPosition.y) > _scrollTreshold)
            {
                _inputData[i].IsDraging = true;
            }
            else
            {
                _inputData[i].IsDraging = false;
            }
            //Debug.Log("drag " + _inputData[i].IsDraging + " press " + _inputData[i].IsPressing);
            // If dragging use unit's eventhandler to send an event to a scrollview like component
            if (_inputData[i].IsDraging)
            {
                PointerEventData lookData = GetLookPointerEventData(_inputData[i].GetHandScreenPosition());
                eventSystem.SetSelectedGameObject(null);
                //Debug.Log("drag");
                GameObject go = lookData.pointerCurrentRaycast.gameObject;
                PointerEventData pEvent = new PointerEventData(eventSystem);
                pEvent.dragging = true;
                pEvent.scrollDelta = (_inputData[i].TempHandPosition - _inputData[i].HandPosition) * _scrollSpeed;
                pEvent.useDragThreshold = true;
                ExecuteEvents.ExecuteHierarchy(go, pEvent, ExecuteEvents.scrollHandler);
            }
        }
    }
    /// <summary>
    ///  Process pressing, event click trigered on button by closing and opening hand,sends submit event to gameobject
    /// </summary>
    private void ProcessPress()
    {
        
        for (int i = 0; i < _inputData.Length; i++)
        {
            //Check if we are tracking hand state not wait over
            if (!_inputData[i].IsHovering || _inputData[i].ClickGesture != KinectUIClickGesture.HandState) continue;
            // If hand state is not tracked reset properties
            if (_inputData[i].CurrentHandState == HandState.NotTracked)
            {
                _inputData[i].IsPressing = false;
                _inputData[i].IsDraging = false;
            }
            //Debug.Log(_inputData[i].IsPressing);

            // When we close hand and we are not pressing set property as pressed
            if (!_inputData[i].IsPressing && _inputData[i].CurrentHandState == HandState.Closed)
            {
                //Debug.Log("Pressing");
                _inputData[i].IsPressing = true;
                //PointerEventData lookData = GetLookPointerEventData(_inputData[i].GetHandScreenPosition());
                //eventSystem.SetSelectedGameObject(null);
                //if (lookData.pointerCurrentRaycast.gameObject != null && !_inputData[i].IsDraging)
                //{
                //    GameObject go = lookData.pointerCurrentRaycast.gameObject;
                //    ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.pointerDownHandler);
                //}
            }
            // If hand state is opened and is pressed, make click action
            else if (_inputData[i].IsPressing && (_inputData[i].CurrentHandState == HandState.Open))//|| _inputData[i].CurrentHandState == HandState.Unknown))
            {
                //Debug.Log("Pressing really!!!!!");
                //_inputData[i].IsDraging = false;
                PointerEventData lookData = GetLookPointerEventData(_inputData[i].GetHandScreenPosition());
                eventSystem.SetSelectedGameObject(null);
                if (lookData.pointerCurrentRaycast.gameObject != null && !_inputData[i].IsDraging)
                {
                    GameObject go = lookData.pointerCurrentRaycast.gameObject;
                    ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.submitHandler);
                    //ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.pointerUpHandler);
                }
                _inputData[i].IsPressing = false;
            }
        }
    }
    /// <summary>
    /// Process hovering over component, sends pointer enter exit event to gameobject
    /// </summary>
    private void ProcessHover()
    {
        for (int i = 0; i < _inputData.Length; i++)
        {
            PointerEventData pointer = GetLookPointerEventData(_inputData[i].GetHandScreenPosition());
            var obj = _handPointerData.pointerCurrentRaycast.gameObject;
            
            
            HandlePointerExitAndEnter(pointer, obj);
            if (obj != null)
            {
                string tag = obj.tag;
                //Debug.Log(tag);
                //Debug.Log(obj.name + " " + obj.tag);
                if (tag.Equals("UIButton") == false)
                {
                    // Hover update
                    _inputData[i].IsHovering = false;
                    //if (obj != null)
                    _inputData[i].HoveringObject = null;
                    continue;
                }
                else if (tag.Equals("UIButton"))
                {
                    //Debug.Log("Is UI Button");

                }


            }
            // Hover update
            _inputData[i].IsHovering = obj != null ? true : false;
            //if (obj != null)
            _inputData[i].HoveringObject = obj;
        }
    }
    /// <summary>
    /// Used from UI hand cursor components
    /// </summary>
    /// <param name="handType"></param>
    /// <returns></returns>
    public KinectInputData GetHandData(KinectUIHandType handType)
    {
        for (int i = 0; i < _inputData.Length; i++)
        {
            if (_inputData[i].trackingHandType == handType)
                return _inputData[i];
        }
        return null;
    }
}
[System.Serializable]
public class KinectInputData
{
    // Which hand we are tracking
    public KinectUIHandType trackingHandType = KinectUIHandType.Right;
    // We can normalize camera z position with this
    public float handScreenPositionMultiplier = 5f;
    // Is hand in pressing condition
    private bool _isPressing;//, _isHovering;
    // Hovering Gameobject, needed for WaitOver like clicking detection
    private GameObject _hoveringObject;
    public float multuplayer = 0.9f;

    // Joint type, we need it for getting body's hand world position
    public JointType handType
    {
        get
        {
            if (trackingHandType == KinectUIHandType.Right)
                return JointType.HandRight;
            else
                return JointType.HandLeft;
        }
    }
    // Hovering Gameobject getter setter, needed for WaitOver like clicking detection
    public GameObject HoveringObject
    {
        get { return _hoveringObject; }
        set
        {
            if (value != _hoveringObject)
            {
                HoverTime = Time.time;
                _hoveringObject = value;
                if (_hoveringObject == null) return;
                if (_hoveringObject.GetComponent<KinectUIWaitOverButton>())
                    ClickGesture = KinectUIClickGesture.WaitOver;
                else
                    ClickGesture = KinectUIClickGesture.HandState;
                WaitOverAmount = 0f;
            }
        }
    }
    public HandState CurrentHandState { get; private set; }
    // Click gesture of button
    public KinectUIClickGesture ClickGesture { get; private set; }
    // Is this hand tracking started
    public bool IsTracking { get; private set; }
    // Is this hand over a UI component
    public bool IsHovering { get; set; }
    // Is hand dragging a component
    public bool IsDraging { get; set; }

    float xOffcet = 0f;
    // Is hand pressing a button
    public bool IsPressing
    {
        get { return _isPressing; }
        set
        {
            _isPressing = value;
            if (_isPressing)
                TempHandPosition = HandPosition;
        }
    }
    // Global position of tracked hand
    public Vector3 HandPosition { get; private set; }
    // Temporary hand position of hand, used for draging check
    public Vector3 TempHandPosition { get; private set; }
    // Hover start time, used for waitover type buttons
    public float HoverTime { get; set; }
    // Amout of wait over , between 1 - 0 , when reaches 1 button is clicked
    public float WaitOverAmount { get; set; }

    public Vector3 PreviousHandPosition { get;private set; }

    // Must be called for each hand 
    public void UpdateComponent(Body body)
    {
        PreviousHandPosition = HandPosition;
        Vector3 result = new Vector3();
        int method = 0;
        if (method == 1)
        {
            var horizontal =
                        (float)(GetVector3FromJoint(body.Joints[handType]).x
                        * multuplayer);
            var vertical = (float)(GetVector3FromJoint(body.Joints[handType]).y
                            * multuplayer);
            var z = GetVector3FromJoint(body.Joints[handType]).z;
            result = new Vector3(
                           (PreviousHandPosition.x + horizontal) * 0.7f,
                           (PreviousHandPosition.y + vertical) * 0.7f,
                           z);
        }
        else if (method == 0)
        {
            float scale = 22.5f;
            float multiplayer = 7f;

            var pos = GetVector3FromJoint(body.Joints[handType]);

            result = new Vector3(pos.x, pos.y, 0);
            result = result * scale;

            
            if (GameManegmentHelper.CurrentGame == 5)
            {
                if (xOffcet==0f)
                {
                    xOffcet = Camera.main.transform.position.x;

                }

            }
            result.x += xOffcet;


            float step = multiplayer * Time.deltaTime;

            Vector3 scaleResult = Vector3.MoveTowards(PreviousHandPosition, result, step);

            scaleResult = CheckCoordinatesIfIsInBounds(scaleResult);
            result = scaleResult;
        }
        
        HandPosition = result;
        CurrentHandState = GetStateFromJointType(body, handType);
        IsTracking = true;
    }

    private Vector3 CheckCoordinatesIfIsInBounds(Vector3 result)
    {
        
        float maxheigth = getCameraHeight() - 2f;
        float maxweidgt = getCameraWidth() - 2f;

       

        if (GameManegmentHelper.CurrentGame == 5)
        {

            //var pos = GameObject.Find("PlayButton").gameObject.transform.position;
            //pos = Camera.main.ScreenToWorldPoint(pos);
            ////pos.x += 3000;
            ////result.x += 3000;

            //var go = GameObject.Find("PlayButton");
            //Debug.Log("result "+result.x);
            if (xOffcet == 0f)
            {
                xOffcet = Camera.main.transform.position.x;

            }
            if (result.x >xOffcet+ (maxweidgt/2)+1)
            {
                
                //Debug.Log(">"+ result.x+" "+);
                result.x = xOffcet+ (maxweidgt/2)+1;
            }
            else if (result.x <xOffcet - maxweidgt / 2)
            {
                //Debug.Log("<");
                result.x = xOffcet - maxweidgt / 2;
            }
            if (result.y > maxheigth / 2)
            {
                result.y = maxheigth / 2;
            }
            else if (result.y < -maxheigth / 2)
            {
                result.y = -maxheigth / 2;
            }

            return result;
        }
        if (result.x > maxweidgt / 2)
        {
            result.x = maxweidgt / 2;
        }
        else if (result.x < -maxweidgt / 2)
        {
            result.x = -maxweidgt / 2;
        }
        if (result.y > maxheigth / 2)
        {
            result.y = maxheigth / 2;
        }
        else if (result.y < -maxheigth / 2)
        {
            result.y = -maxheigth / 2;
        }


        return result;

        
    }

    private float getCameraHeight()
    {

        return Camera.main.orthographicSize * 2;
    }

    private float getCameraWidth()
    {
        var height = Camera.main.orthographicSize * 2;
        var width = Camera.main.aspect * height;

        return width;
    }



    // Converts hand position to screen coordinates
    public Vector3 GetHandScreenPosition()
    {

        return Camera.main.WorldToScreenPoint(new Vector3(HandPosition.x, HandPosition.y, HandPosition.z - handScreenPositionMultiplier));
    }
    // Get hand state data from kinect body
    private HandState GetStateFromJointType(Body body, JointType type)
    {
        switch (type)
        {
            case JointType.HandLeft:
                return body.HandLeftState;
            case JointType.HandRight:
                return body.HandRightState;
            default:
                Debug.LogWarning("Please select a hand joint, by default right hand will be used!");
                return body.HandRightState;
        }
    }
    // Get Vector3 position from Joint position
    private Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        float xOffcet = 0f;
        if (GameManegmentHelper.CurrentGame==5)
        {
            //xOffcet = Camera.main.transform.position.x;
           
        }
        return new Vector3(joint.Position.X+xOffcet, joint.Position.Y, joint.Position.Z * 10);
    }
}

public enum KinectUIClickGesture
{
    HandState, Push, WaitOver
}
public enum KinectUIHandType
{
    Right,Left
}