using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Windows.Kinect;
/// <summary>
/// Abstract UI component class for hand cursor objects
/// </summary>
[RequireComponent(typeof(CanvasGroup), typeof(Image))]
public abstract class AbstractKinectUICursor : MonoBehaviour {

    [SerializeField]
    protected KinectUIHandType _handType;
    protected KinectInputData _data;
    protected Image _image;
    protected Renderer imgRndr;
    protected bool isRendered;
    protected Color initColor = new Color(1f, 1f, 1f, 0f);
    protected float startInterval;
    protected float time;


    public void Awake()
    {
        //Debug.Log("Init");
        startInterval = 0.1f;
        time = 0.0f;
    }

    public virtual void Start()
    {
        Setup();
    }

    protected void Setup()
    {
        
        _data = KinectInputModule.instance.GetHandData(_handType);
       
        
        // Make sure we dont block raycasts
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<CanvasGroup>().interactable = false;
        // image component
        _image = GetComponent<Image>();
        //imgRndr = _image.
        isRendered = true;
        if (_data == null)
        {
            Debug.Log("Error");
        }
        //Debug.Log(_data.HandPosition);
    }

    public virtual void Update()
    {
        if (GameManegmentHelper.PlayerDetected==false)
        {
            if (isRendered == true)
            {
                // Debug.Log("Null col");
                isRendered = false;
                _image.color = initColor;
            }
            return;
        }
        _data = KinectInputModule.instance.GetHandData(_handType);

        if (_data == null || !_data.IsTracking)
        {
            if (isRendered == true)
            {
               // Debug.Log("Null col");
                isRendered = false;
                _image.color = initColor;
            }
            return;
        }
        isRendered = true;
        ProcessData();
    }

    public abstract void ProcessData();
}
