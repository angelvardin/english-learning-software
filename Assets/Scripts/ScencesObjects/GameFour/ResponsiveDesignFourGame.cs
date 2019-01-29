using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsiveDesignFourGame : MonoBehaviour {

	public List<GameObject> gameObjects;
	// Use this for initialization
	void Start () {
		if (gameObjects != null || gameObjects.Count > 0)
		{
			foreach (var obj in gameObjects)
			{
				if (Camera.main.aspect >= 1.7)
				{
					Debug.Log("16:9");
                    break;
				}
				else if (Camera.main.aspect >= 1.5)
				{
					Debug.Log("3:2");
                    string name = obj.name;
                    if (name.Equals("CheckResults"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.2f;
                        pos.y = 0.9f;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                    }
                    else if (name.Equals("PauseBtn"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.5f;
                        pos.y = 0.9f;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                    }
                    else if (name.Equals("Cheese"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.8f;
                        pos.y = 0.9f;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                    }
                }
				else
				{

					Debug.Log("4:3");
                    string name = obj.name;
                    if (name.Equals("CheckResults"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.2f;
                        pos.y = 0.9f;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                    }
                    else if (name.Equals("PauseBtn"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.5f;
                        pos.y = 0.9f;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                    }
                    else if (name.Equals("Cheese"))
                    {
                        Vector3 pos = Camera.main.WorldToViewportPoint(obj.transform.position);
                        pos.x = 0.8f;
                        pos.y = 0.9f;
                        pos.z = 0;
                        Vector3 newPos = Camera.main.ViewportToWorldPoint(pos);
                        newPos.z = 0;
                        obj.transform.position = newPos;
                    }
					
				}
				
			}

		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
