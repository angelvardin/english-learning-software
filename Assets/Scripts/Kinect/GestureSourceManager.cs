using Microsoft.Kinect.VisualGestureBuilder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using System;
using UnityEngine.SceneManagement;

public abstract class GestureSourceManager : MonoBehaviour {
    public struct EventArgs
    {
        public string name;
        public float confidence;

        public EventArgs(string _name, float _confidence)
        {
            name = _name;
            confidence = _confidence;
        }
    }

    protected BodySourceManager _BodySource;
    public string databasePath;
    public bool fixedUpdate = false;
    public float MinConfidence = 0.9f;
    protected KinectSensor _Sensor;
    protected VisualGestureBuilderFrameSource _Source;
    protected VisualGestureBuilderFrameReader _Reader;
    protected VisualGestureBuilderDatabase _Database;
    protected bool isPlayerDetected = false;

    private string CurrentGestureName;

    private List<string> AllGestures;
    private string currentTime = String.Empty;

    protected List<string> AllGesturesString
    {
        get
        {
            return AllGestures;
        }

        set
        {
            
        }
    }

    protected string CurrentDetectedGestureName
    {
        get
        {
            return CurrentGestureName;
        }

        set
        {
           
        }
    }

    protected float DetectedConfidence;

    

    // Gesture Detection Events
    public delegate void GestureAction(EventArgs e);
    public event GestureAction OnGesture;

    // Use this for initialization
    public virtual void Start()
    {
        GameManegmentHelper.isUI = false;
        this.AllGestures = new List<string>();
        this.CurrentGestureName = string.Empty;
        this.DetectedConfidence = 0.0f;
        currentTime = System.DateTime.Now.ToString();
        
        
        _BodySource = FindObjectOfType<BodySourceManager>();
        if (GameManegmentHelper.sensor==null)
        {
            Debug.Log("Initbhj");
            _Sensor = KinectSensor.GetDefault();
            GameManegmentHelper.sensor = _Sensor;
        }
        else
        {
            _Sensor = GameManegmentHelper.sensor;
        }
        //Debug.Log("Initbhj");
        if (_Sensor != null)
        {
           // Debug.Log("Initbhj");
            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
            _Source = VisualGestureBuilderFrameSource.Create(_Sensor, 0);
            _Reader = _Source.OpenReader();
            // Set up Gesture Source
            //if (GameManegmentHelper._gestureSource==null)
            //{
            //    _Source = VisualGestureBuilderFrameSource.Create(_Sensor, 0);
            //    GameManegmentHelper._gestureSource = _Source;
            //   // Debug.Log(_Source);
            //}
            //else
            //{
            //    _Source = GameManegmentHelper._gestureSource;
            //    //Debug.Log(_Source);

            //}

            //// open the reader for the vgb frames
            //if (GameManegmentHelper._gestureReader==null)
            //{
            //    _Reader = _Source.OpenReader();
            //    GameManegmentHelper._gestureReader = _Reader;
            //}
            //else
            //{
            //    _Reader = GameManegmentHelper._gestureReader;
            //}
           
            if (_Reader != null)
            {
                _Reader.IsPaused = true;
                _Reader.FrameArrived += GestureFrameArrived;
            }

            // load the  gesture from the gesture database
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, databasePath);
            _Database = VisualGestureBuilderDatabase.Create(path);

            // Load all gestures
            IList<Gesture> gesturesList = _Database.AvailableGestures;
            for (int g = 0; g < gesturesList.Count; g++)
            {
                Gesture gesture = gesturesList[g];
                Debug.Log(gesture.Name);
                _Source.AddGesture(gesture);
                AllGestures.Add(gesture.Name);
                
            }

        }

        InitComponents();

    }

    public virtual void InitComponents()
    {
        
    }

    // Public setter for Body ID to track
    public virtual void SetBody(ulong id)
    {
        if (id > 0)
        {
            _Source.TrackingId = id;
            _Reader.IsPaused = false;
        }
        else
        {
            _Source.TrackingId = 0;
            _Reader.IsPaused = true;
        }
    }

    // Update Loop, set body if we need one
    public virtual void Update()
    {
        if (_Source == null)
        {
            //Debug.Log(currentTime);
            return;
        }
        //if (!gameObject.scene.name.Equals("GameTwoTopicOne"))
        //{
        //    //Debug.Log(1 + " "+ currentTime);
        //}
        if (fixedUpdate)
        {
            return;
        }
        if (!_Source.IsTrackingIdValid)
        {
            FindValidBody();
        }

        

    }

    public void FixedUpdateGestureDetect()
    {
        if (!_Source.IsTrackingIdValid)
        {
            FindValidBody();
        }
    }

    public void DestroySource()
    {
        if (_Source!=null)
        {
            _Source.Dispose();
            _Source = null;
        }
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }
    }

    // Check Body Manager, grab first valid body
    protected virtual void FindValidBody()
    {

        if (_BodySource != null)
        {
            Body[] bodies = _BodySource.GetData();
            if (bodies != null)
            {
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                    {
                        isPlayerDetected = true;
                        SetBody(body.TrackingId);
                        break;
                    }
                    else
                    {
                        isPlayerDetected = false;
                    }
                }
            }
        }

    }

    /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
    protected virtual void GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null)
            {
                // get the discrete gesture results which arrived with the latest frame
                IDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                if (discreteResults != null)
                {
                    foreach (Gesture gesture in _Source.Gestures)
                    {
                        if (gesture.GestureType == GestureType.Discrete)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);


                            if (result != null)
                            {
                                // Fire Event
                                if (result.Detected)
                                {
                                    
                                    //Debug.Log("Detected Gesture " + gesture.Name + " with Confidence " + result.Confidence);
                                    if (result.Confidence>=MinConfidence)
                                    {
                                        DetectedConfidence = result.Confidence;
                                        this.CurrentGestureName = gesture.Name;
                                        DetectedWork();

                                        OnGesture(new EventArgs(gesture.Name, result.Confidence));

                                    }
                                    else
                                    {
                                        this.CurrentGestureName = string.Empty;
                                        this.DetectedConfidence = 0.0f;
                                    }   

                                }
                                else
                                {
                                    this.CurrentGestureName = string.Empty;
                                    this.DetectedConfidence = 0.0f;
                                    NotDetectedWork();
                                }
                                       // OnGesture(new EventArgs(gesture.Name, result.Confidence));
                                //

                            }
                        }
                    }
                }
            }
        }
    }

    

    public abstract void DetectedWork();
    public abstract void NotDetectedWork();

}