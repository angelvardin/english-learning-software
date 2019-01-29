using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class MoveObject : MonoBehaviour {

    //public GameObject BodyScrManager;
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;
    
    private Rigidbody2D rb;
    public float multuplayer = 0.9f;

    public float coordinateOffcetY = 2f;
    public float coordinateOffcetX = 10f;

    private float maxheigth;
    private float maxweidgt;

    // Use this for initialization
    void Start()
    {

        this.rb = this.GetComponent<Rigidbody2D>();
        this.maxheigth = getCameraHeight()-coordinateOffcetY;
        this.maxweidgt = getCameraWidth()-coordinateOffcetX;

        bodyManager = FindObjectOfType<BodySourceManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManegmentHelper.isUI)
        {
            return;
        }

        if (bodyManager == null)
        {
            Debug.Log("null");
            return;
        }
        bodies = bodyManager.GetData();

        if (bodies == null) { return; }
        foreach (var body in bodies)
        {
            if (body == null)
            {
                continue;
            }
            if (body.IsTracked)
            {
                //Debug.Log("Init");
                
                var pos = body.Joints[TrackedJoint].Position;
                
                Vector3 direction = (new Vector3(pos.X*multuplayer,pos.Y*multuplayer,0) - transform.position);
                //Debug.Log(direction);
                Vector3 direction1 = (new Vector3(pos.X , pos.Y, 0) - transform.position);

                //Vector3 result = transform.position + direction * Time.fixedDeltaTime*MovementSpeed;
                //Vector3 result = new Vector3(pos.X*multuplayer,pos.Y*multuplayer,0);

                var horizontal =
                        (float)(body.Joints[TrackedJoint].Position.X
                        * multuplayer);

                var vertical = (float)(body.Joints[TrackedJoint].Position.Y * multuplayer);

                Vector3 result = new Vector3(
                        this.gameObject.transform.position.x + horizontal,
                        this.gameObject.transform.position.y + vertical,
                        0);


                if (result.x>maxweidgt/2 )
                {
                    result.x = maxweidgt/2;
                }else if (result.x < -maxweidgt/2)
                {
                    result.x = -maxweidgt/2;
                }

                if (result.y > maxheigth/2)
                {
                    result.y = maxheigth/2;
                }
                else if (result.y < -maxheigth/2)
                {
                    result.y = -maxheigth/2;
                }

                this.rb.MovePosition(result);
               
                
                //gameObject.transform.position = new Vector3(pos.X * MovementSpeed, pos.Y * MovementSpeed, 0);
            }
        }
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

}
