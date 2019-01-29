using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITtetController : MonoBehaviour {

   
    public GameObject gameobj;
    public float offcetX;
    public float offcetY;

    private GameObject parent;
    // Use this for initialization
    void Start()
    {
        this.transform.position = GetHandScreenPosition(gameobj.transform.position);

    }

    public Vector3 GetHandScreenPosition(Vector3 coordinates)
    {

        return Camera.main.WorldToScreenPoint(new Vector3(coordinates.x + offcetX, coordinates.y + offcetY, coordinates.z));
    }

    public Vector2 GetHandScreenPositionRect(Vector3 coordinates)
    {
        var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, coordinates);

        return pos;
    }

    // Update is called once per frame
    void Update()
    {

    }
}