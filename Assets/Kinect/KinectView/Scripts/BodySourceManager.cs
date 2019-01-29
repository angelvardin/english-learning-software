using UnityEngine;
using System.Collections;
using Windows.Kinect;

public class BodySourceManager : MonoBehaviour 
{
    private KinectSensor _Sensor;
    private BodyFrameReader _Reader;
    private Body[] _Data = null;
    
    public Body[] GetData()
    {
        return _Data;
    }
    

    void Start () 
    {
        if (GameManegmentHelper.sensor==null)
        {
            _Sensor = KinectSensor.GetDefault();
            GameManegmentHelper.sensor = _Sensor;
        }
        else
        {
            _Sensor = GameManegmentHelper.sensor;
        }
       

        if (_Sensor != null)
        {
            if (GameManegmentHelper._Reader == null)
            {
                _Reader = _Sensor.BodyFrameSource.OpenReader();
                GameManegmentHelper._Reader = _Reader;
            }
            else
            {

                _Reader = GameManegmentHelper._Reader;
            }

            if (!_Sensor.IsOpen)
            {
                _Sensor.Open();
            }
        }   
    }
    
    void Update () 
    {
        if (_Reader != null)
        {
            var frame = _Reader.AcquireLatestFrame();
            if (frame != null)
            {
                if (_Data == null)
                {
                    _Data = new Body[_Sensor.BodyFrameSource.BodyCount];
                }
                
                frame.GetAndRefreshBodyData(_Data);
                
                frame.Dispose();
                frame = null;
            }
        }    
    }

    //void OnDestroy()
    //{
    //    Debug.Log("OnDestroy");
    //    if (GameManegmentHelper._gestureReader != null)
    //    {
    //        GameManegmentHelper._gestureReader.Dispose();
    //        //GameManegmentHelper._gestureReader = null;
    //    }

    //    if (GameManegmentHelper._gestureSource != null)
    //    {
    //        GameManegmentHelper._gestureSource.Dispose();
    //        //GameManegmentHelper._gestureSource = null;
    //    }
    //}

    void OnApplicationQuit()
    {
        if (_Reader != null)
        {
            _Reader.Dispose();
            _Reader = null;
        }
        if (GameManegmentHelper._Reader!=null)
        {
            GameManegmentHelper._Reader.Dispose();
            GameManegmentHelper._Reader = null;
        }
        if (GameManegmentHelper._gestureReader!=null)
        {
            GameManegmentHelper._gestureReader.Dispose();
            GameManegmentHelper._gestureReader = null;
        }

        if (GameManegmentHelper._gestureSource!=null)
        {
            GameManegmentHelper._gestureSource.Dispose();
            GameManegmentHelper._gestureSource = null;
        }
        
        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }
            if (GameManegmentHelper.sensor.IsOpen)
            {
                GameManegmentHelper.sensor.Close();
            }
            _Sensor = null;
            GameManegmentHelper.sensor = null;
        }
    }
}
