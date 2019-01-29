using UnityEngine;
using Windows.Kinect;

public class MoveObjectsTest : MonoBehaviour
{
    public JointType TrackedJoint;
    private BodySourceManager bodyManager;
    private Body[] bodies;

    private Rigidbody2D rb;
    public float multuplayer = 7f;
    public float scale = 22.5f;

    public float coordinateOffcetY = 2f;
    public float coordinateOffcetX = 2f;

    private float maxheigth;
    private float maxweidgt;

    private Renderer rndr;
    private Renderer rndrPS;

    // Use this for initialization
    private void Start()
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
    private void Update()
    {
        if (GameManegmentHelper.isUI==true)
        {
            //Debug.Log("isUI");
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

                var pos = body.Joints[TrackedJoint].Position;

                Vector3 result = new Vector3(pos.X, pos.Y, 0);
                result = result * scale;


                float step = multuplayer * Time.deltaTime;

                Vector3 scaleResult = Vector3.MoveTowards(gameObject.transform.position, result, step);

                scaleResult = CheckCoordinatesIfIsInBounds(scaleResult);

                gameObject.transform.position = scaleResult;
            }
        }
        if (count == length)
        {
            rndr.enabled = false;
            rndrPS.enabled = false;
        }
    }

    private Vector3 CheckCoordinatesIfIsInBounds(Vector3 result)
    {
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

        return result;
    }

    public void SetHandState(Body body)
    {
        if (TrackedJoint == JointType.HandLeft)
        {
            GameManegmentHelper.handState = body.HandLeftState;
        }
        else if (TrackedJoint == JointType.HandRight)
        {
            GameManegmentHelper.handState = body.HandRightState;

        }

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

        if (TrackedJoint == JointType.HandLeft && body.HandLeftState == HandState.Closed)
        {
            GameManegmentHelper.isHandClosed = true;
        }
        else if (TrackedJoint == JointType.HandRight && body.HandRightState == HandState.Closed)
        {
            GameManegmentHelper.isHandClosed = true;
        }
        else
        {
            GameManegmentHelper.isHandClosed = false;
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