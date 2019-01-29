using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject objectToFollow;

	private float offsetX;

	// Use this for initialization
	public void Start () {

        var height = this.getCameraHeight();
        var wigth = this.getCameraWidth();

        var offset = wigth / 4;

        this.offsetX = offset;

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

    // Update is called once per frame
    public void Update () {
		var currentPosition = this.transform.position;
		currentPosition.x = objectToFollow.transform.position.x + offsetX;
		this.transform.position = currentPosition;
	}


}
