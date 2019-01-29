using UnityEngine;
using Windows.Kinect;

public class HandPointMovement : MonoBehaviour
{
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;

    private Rigidbody2D rb;
    public float multuplayer = 1.8f;

    public float coordinateOffcetY = 2f;
    public float coordinateOffcetX = 2f;

    private float maxheigth;
    private float maxweidgt;

    private Renderer rndr;
    private Renderer rndrPS;


    // Use this for initialization
    void Start()
    {
        GameManegmentHelper.isUI = false;
        //this.rb = this.GetComponent<Rigidbody2D>();
        this.maxheigth = getCameraHeight() - coordinateOffcetY;
        this.maxweidgt = getCameraWidth() - coordinateOffcetX;

        bodyManager = FindObjectOfType<BodySourceManager>();
        rndr = this.gameObject.GetComponent<Renderer>();
        rndr.enabled = false;
        var ps = this.transform.Find("JetPack");
        rndrPS = ps.gameObject.GetComponent<Renderer>();
        rndrPS.enabled = false;

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
        if (bodies == null)
        {
            return;
        }


        int length = bodies.Length;

        int count = 1;
        foreach (var body in bodies)
        {
            if (body == null)
            {
                count++;
                continue;
            }


            if (body.IsTracked)
            {
                if (rndr.enabled == false || rndrPS.enabled == false)
                {
                    rndr.enabled = true;
                    rndrPS.enabled = true;
                }
                //Debug.Log("Init");
                SetHandState(body);

                var horizontal =
                        (float)(body.Joints[TrackedJoint].Position.X
                        * multuplayer);

                var vertical = (float)(body.Joints[TrackedJoint].Position.Y * multuplayer);

                Vector3 result = new Vector3(
                        this.gameObject.transform.position.x + horizontal,
                        this.gameObject.transform.position.y + vertical,
                        0);


                if (result.x > maxweidgt / 2)
                {
                    result.x = maxweidgt / 2;
                }
                else if (result.x < -maxweidgt / 2)
                {
                    result.x = -maxweidgt / 2;
                }

                if (result.y > maxheigth / 2)
                {
                    result.y = maxheigth / 2;
                }
                else if (result.y < -maxheigth / 2)
                {
                    result.y = -maxheigth / 2;
                }
                gameObject.transform.position = result;
                //this.rb.MovePosition(result);
                //gameObject.transform.position = new Vector3(pos.X * MovementSpeed, pos.Y * MovementSpeed, 0);
            }
        }
        if (count == length)
        {
            rndr.enabled = false;
            rndrPS.enabled = false;
        }
    }

    public void SetHandState(Body body)
    {
        if (TrackedJoint == JointType.HandLeft && body.HandLeftState == HandState.Open)
        {
            GameManegmentHelper.isHandOpen = true;

        }
        else if (TrackedJoint == JointType.HandRight && body.HandRightState == HandState.Open)
        {
            GameManegmentHelper.isHandOpen = true;
        }
        else
        {
            GameManegmentHelper.isHandOpen = false;
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