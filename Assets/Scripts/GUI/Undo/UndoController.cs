using UnityEngine;

public class UndoController : MonoBehaviour
{
    private Collider2D undoCollider;

    public float interval = 0.5f;
    private float timeSinceStart;
    public float overlap = 0.5f;
    public LayerMask PlayerLayer;

    // Use this for initialization
    private void Start()
    {
        var obj = GameObject.FindGameObjectWithTag("undo");

        undoCollider = obj.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        UndoButtonClicked();
    }

    private void UndoButtonClicked()
    {
        var Triggered = Physics2D.OverlapCircle(undoCollider.transform.position, overlap, this.PlayerLayer);
        if (Triggered != null)
        {
            this.timeSinceStart += Time.deltaTime;
            if (timeSinceStart >= interval)
            {
                OnUndoClicked();
                this.timeSinceStart = 0;
            }
        }
        else
        {
            this.timeSinceStart = 0;
        }
    }

    private void OnUndoClicked()
    {

        if (GameManegmentHelper.order.Count == 0)
        {
            return;
        }
        while (true)
        {
            if (GameManegmentHelper.order.Count == 0)
            {
                return;
            }

            var coord = (UndoInformation)GameManegmentHelper.order.Pop();

            var go = GameObject.Find(coord.ObjectName);
            if (go==null)
            {
                return;
            }
            var pos = go.transform.position;
            if (pos.x != coord.X&& pos.y != coord.Y)
            {
                pos.x = coord.X;
                pos.y = coord.Y;
                go.transform.position = pos;
                break;
            }
           
        }
    }
}