using Assets.Scripts;
using Assets.Scripts.UI;
using UnityEngine;

public class Control : MonoBehaviour
{
    public bool AttackOverride { get; private set; }

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
    private Ray? TargetRay;

    public Texture2D NormalCursor;
    public Texture2D AttackCursor;

    private void RaycastTarget()
    {
        ClickHitState.Target = null;
        ClickHitState.Ship = null;

        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out var hitInfo))
        {
            ClickHitState.Target = hitInfo.transform.GetComponent<Target>();
            ClickHitState.Ship = hitInfo.transform.GetComponent<Ship>();
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
    
    public void OnDrawGizmos()
    {
        if (TargetRay.HasValue)
        {
            Gizmos.DrawRay(TargetRay.Value);
        }
    }

    public void StartAttackSelection()
    {
        AttackOverride = true;
    }
}
