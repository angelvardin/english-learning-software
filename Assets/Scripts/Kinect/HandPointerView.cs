using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Windows.Kinect;

public class HandPointerView : MonoBehaviour
{
    private Material BoneMaterial;

    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();
    private BodySourceManager _BodyManager;
    private float loadingNewLevelInterval = 0.0f;
    private float currentTime = 0.0f;

    private Dictionary<JointType, JointType> _BoneMap = new Dictionary<JointType, JointType>()
    {
        
    };
    void Start()
    {
        GameManegmentHelper.PlayerDetected = false;
        Scene scene = SceneManager.GetActiveScene();
        if (scene.Equals("MainMenu"))
        {
            GameManegmentHelper.loadingNewLevel = false;
        }
        else
        {
            GameManegmentHelper.loadingNewLevel = true;
        }
        _BodyManager = FindObjectOfType<BodySourceManager>();
    }
    void Update()
    {
        if (GameManegmentHelper.loadingNewLevel==true)
        {
            if (currentTime<loadingNewLevelInterval)
            {
                currentTime += Time.deltaTime;
                return;
            }
            else
            {
                GameManegmentHelper.loadingNewLevel = false;
            }
        }

        if (_BodyManager == null)
        {
            return;
        }

        Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                GameManegmentHelper.PlayerDetected = false;
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);
                }

                RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }

            // Tracking for UI Component
            if (body.IsTracked)
            {
                if (GameManegmentHelper.isUI)
                {
                    KinectInputModule.instance.TrackBody(body);
                    break;
                }

            }
            
        }
    }

    

    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Player:" + id);
        GameManegmentHelper.PlayerDetected = true;

        return body;
    }

    private void RefreshBodyObject(Body body, GameObject bodyObject)
    {
      
    }

    public static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}