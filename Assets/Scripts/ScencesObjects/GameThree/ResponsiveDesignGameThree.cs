using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveDesignGameThree : MonoBehaviour {

    public List<GameObject> gameObjects;
    public float yCoef = 0.1f;
    public float scalePauseBtn = 1.3f;
    public float scale = 0.1f;

    // Use this for initialization
    void Start () {
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
                    string name = obj.name;
                    if (name.Equals("CheckResults"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.2f;
                        pos.y = yCoef;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                    }
                    else if (name.Equals("PauseBtn"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.5f;
                        pos.y = yCoef;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                        obj.transform.localScale = new Vector3(scalePauseBtn, scalePauseBtn, 1);
                        continue;
                    }
                    else if (name.Equals("Cheese"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.8f;
                        pos.y = yCoef;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                    }
                    obj.transform.localScale = new Vector3(scale, scale, 1);
                }
                else
                {

                    Debug.Log("4:3");
                    string name = obj.name;
                    if (name.Equals("CheckResults"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.2f;
                        pos.y = yCoef;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                        
                    }
                    else if (name.Equals("PauseBtn"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.5f;
                        pos.y = yCoef;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                        obj.transform.localScale = new Vector3(scalePauseBtn, scalePauseBtn, 1);
                        continue;
                    }
                    else if (name.Equals("Cheese"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.8f;
                        pos.y = yCoef;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                    }
                    obj.transform.localScale = new Vector3(scale, scale, 1);
                }
                

            }

        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
