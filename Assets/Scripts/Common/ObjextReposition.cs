using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjextReposition : MonoBehaviour {

    // Use this for initialization
    public List<GameObject> gameObjects;
    public bool type;
	void Start () {
        if (type == true)
        {
            if (gameObjects != null || gameObjects.Count > 0)
            {
                foreach (var obj in gameObjects)
                {
                    if (Camera.main.aspect >= 1.7)
                    {
                        Debug.Log("16:9");
                    }
                    else if (Camera.main.aspect >= 1.5)
                    {
                        Debug.Log("3:2");
                    }
                    else
                    {
                        Debug.Log("4:3");
                    }
                    Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                    pos.x = Mathf.Clamp(pos.x,0.1f,0.9f);
                    pos.y = Mathf.Clamp01(pos.y);
                    pos.z = 0;
                    Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                    newPos.z = 0;
                    obj.transform.position = newPos;
                }

            }
        }
        else
        {
            if (gameObjects != null || gameObjects.Count > 0)
            {
                foreach (var obj in gameObjects)
                {
                    Vector3 pos = obj.transform.position;
                    Debug.Log("old coord" + pos);
                    Vector3 newPos = Camera.main.ScreenToViewportPoint(pos);
                    //newPos.z = 0;
                    obj.transform.position = newPos;
                    Debug.Log("coord" + obj.transform.position);
                }

            }
        }
      

        
        
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
