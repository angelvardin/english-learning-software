using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneratorController : MonoBehaviour {


	public List<GameObject> availableObjects;
	public List<GameObject> currentObjects;

	private float minY;
	private float maxY;
    private float maxObjOffset;
    public float maxObjOffsetStep= 0.25f;
    private float objOffset;
    private float screenWidth; 


	// Use this for initialization
	public void Start ()
	{
        //maxObjOffset = 0.25f;
		var height = Camera.main.orthographicSize;
		minY = -height / 2;
		maxY = height / 2;

        this.screenWidth = height * 2 * Camera.main.aspect;
        this.maxObjOffset = screenWidth / 4;
        this.objOffset = 0;

    }

	public void AddObject(float maxDistanceX)
	{
		var randomObjectIndex = Random.Range(0, this.availableObjects.Count);
		var obj = Instantiate(this.availableObjects[randomObjectIndex]);

        this.currentObjects.Add(obj);

		var pos = obj.transform.position;
		pos.x = maxDistanceX;
        pos.y = Random.Range(this.minY, this.maxY);
        obj.transform.position = pos;

	}
	
	// Update is called once per frame
	public void Update () {

        var pos = this.transform.position;

        var maxOffcetX = pos.x + this.screenWidth;

        var minOffcerX = pos.x - screenWidth;

        float farthestDistanceX = 0;

        var objectsToRemove = new List<GameObject>();

        foreach (var obj in this.currentObjects)
        {
            var objCenter = obj.transform.position.x;
            farthestDistanceX = Mathf.Max(farthestDistanceX, objCenter);
            if (farthestDistanceX < minOffcerX)
            {
                objectsToRemove.Add(obj);
            }
        }

        foreach (var r in objectsToRemove)
        {
            this.currentObjects.Remove(r);
            Destroy(r.gameObject);
            //r.gameObject.SetActive(false);
        }

        if (farthestDistanceX<maxOffcetX)
        {

            this.AddObject(farthestDistanceX+this.screenWidth-objOffset);
            if (objOffset<this.maxObjOffset)
            {
                objOffset += this.maxObjOffset;

            }
           
        }

	}
}
