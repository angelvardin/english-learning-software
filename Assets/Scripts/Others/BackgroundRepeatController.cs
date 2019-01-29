using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeatController : MonoBehaviour {

	public List<GameObject> availableRooms;
	public List<GameObject> currentRoom;

	private float screenwidht;

	// Use this for initialization
	public void Start () {
		var height = Camera.main.orthographicSize * 2;
		var width = Camera.main.aspect * height;
		screenwidht = width;
	}
	
	// Update is called once per frame
	public void Update () {

		var playerPos = this.transform.position;

		var maxWitht = playerPos.x + this.screenwidht;
		var minWitht = playerPos.x - this.screenwidht;

		float fathestDistanseX = 0.0f;

		var roomToRemove = new List<GameObject>();

		foreach (var room in this.currentRoom)
		{
			var currentRoomX = room.transform.position.x +
				(this.GetRoomWidth(room)/2);
			fathestDistanseX = Mathf.Max(currentRoomX, fathestDistanseX);

			if (currentRoomX < minWitht)
			{
				roomToRemove.Add(room);
			}

		}
		foreach (var r in roomToRemove)
		{
			this.currentRoom.Remove(r);
			Destroy(r.gameObject);
            //r.gameObject.SetActive(false);
		}
		if (fathestDistanseX<maxWitht)
		{
			this.AddRoom(fathestDistanseX);
		}
	}

	

	public void AddRoom(float farestDistance)
	{
		var randomIndex = Random.Range(0, this.availableRooms.Count);
		var room = (GameObject)Instantiate(this.availableRooms[randomIndex]);
		var roomWidht = GetRoomWidth(room);
		room.transform.position = new Vector3(roomWidht / 2 + farestDistance, 0, 0);
        //room.SetActive(false);
		this.currentRoom.Add(room);


	}

	private float GetRoomWidth(GameObject room)
	{
        return room.transform.Find("floor").GetComponent<BoxCollider2D>().bounds.size.x;
        //Debug.Log(room.GetComponentInChildren<BoxCollider2D>().transform.localScale.x);
        //return room.GetComponentInChildren<BoxCollider2D>().transform.localScale.x;
	}


}
