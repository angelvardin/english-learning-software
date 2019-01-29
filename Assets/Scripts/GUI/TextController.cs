using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {

    private RectTransform textPosition;
    public float offcetX;
    public float offcetY;
    public bool isAnswer = false;
    public bool CanBeDestroied = false;
    public bool isChoosen = false;
    
    private GameObject parent;
    // Use this for initialization
    void Start ()
    {
        if (isAnswer==false)
        {
            return;
        }
        ChangePos();
    }

    void Update()
    {
        ChangePos();
    }

    private void ChangePos()
    {
        var canvas = gameObject.transform.parent.gameObject;
        parent = canvas.transform.parent.gameObject;
        textPosition = GetComponent<RectTransform>();
        var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, parent.transform.position);
        this.transform.position = GetHandScreenPosition(parent.transform.position);

    }

    public Vector3 GetHandScreenPosition(Vector3 coordinates)
    {

        return Camera.main.WorldToScreenPoint(new Vector3(coordinates.x+offcetX, coordinates.y+offcetY, coordinates.z));
    }

    public Vector2 GetHandScreenPositionRect(Vector3 coordinates)
    {
        var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, coordinates);

        return pos;
    }

    // Update is called once per frame
  
}
