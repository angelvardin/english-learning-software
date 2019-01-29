using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExersiceTextController : MonoBehaviour {


    private float maxheigth;
    private float maxweidgt;

    public string text = "some text";

    public float coordinateOffcetY = 2f;
    public float coordinateOffcetX = 10f;

    public void Start()
    {
        this.maxheigth = getCameraHeight() - coordinateOffcetY;
        this.maxweidgt = getCameraWidth() - coordinateOffcetX;
    }

    public float getCameraHeight()
    {
        return Camera.main.orthographicSize * 2;
    }

    public float getCameraWidth()
    {
        var height = Camera.main.orthographicSize * 2;
        var width = Camera.main.aspect * height;

        return width;
    }

    public void OnGUI()
    {
        var posx = 0;
        var posy = -maxheigth;
        var coinsCountRect = new Rect(posx, posy, 32, 32);
        var coinsStyle = new GUIStyle();
        coinsStyle.fontSize = 30;
        coinsStyle.fontStyle = FontStyle.Normal;
        coinsStyle.normal.textColor = Color.yellow;
        GUI.Label(coinsCountRect, text, coinsStyle);


    }
}
