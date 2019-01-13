using Assets.Scripts.UI;
using UnityEngine;

public class Control : MonoBehaviour
{
    public bool AttackOverride;

    [HideInInspector]
    public MovementInformation MovementInformation { get; } = new MovementInformation();

    void Start()
    {
        var moveDiskGameObject = new GameObject("MoveDisk");
        _moveDisk = moveDiskGameObject.AddComponent<MoveDisk>();
        _moveDisk.enabled = true;
    }

    public ClickHitState ClickHitState;

    public MoveDisk GetMoveDisk()
    {
        return _moveDisk;
    }

    private MoveDisk _moveDisk;

    public Texture2D NormalCursor;
    public Texture2D AttackCursor;

    private void RaycastTarget()
    {
        ClickHitState.OldShip = null;

        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out var hitInfo))
        {
            ClickHitState.OldShip = hitInfo.transform.GetComponent<OldShip>();
        }
    }

    public void ShowAttackCursor()
    {
        Cursor.SetCursor(AttackCursor, Vector2.zero, CursorMode.Auto);
    }

    public void ShowDefaultCursor()
    {
        Cursor.SetCursor(NormalCursor, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        RaycastTarget();
    }
    
    public void StartAttackSelection()
    {
        AttackOverride = true;
    }
}
