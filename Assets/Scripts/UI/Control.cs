using System;
using Assets.Scripts;
using Assets.Scripts.UI;
using UnityEngine;

public enum ControlState
{
    Unknown,
    IdleState,
    Attacking,
    ProcessMove,
    ShowingDisk
}

public class Control : MonoBehaviour
{
    public bool AttackOverride;

    [HideInInspector] public MovementInformation MovementInformation { get; } = new MovementInformation();

    void Start()
    {
        var moveDiskGameObject = new GameObject("MoveDisk");
        _moveDisk = moveDiskGameObject.AddComponent<MoveDisk>();
        _moveDisk.enabled = true;
        _controlState = ControlState.IdleState;
    }

    public ClickHitState ClickHitState;
    private ControlState _controlState;

    public MoveDisk GetMoveDisk()
    {
        return _moveDisk;
    }

    private MoveDisk _moveDisk;

    public Texture2D NormalCursor;
    public Texture2D AttackCursor;

    private void RaycastTarget()
    {
        ClickHitState.Ship = null;

        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseRay, out var hitInfo))
        {
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
        switch (_controlState)
        {
           case ControlState.IdleState:
               IdleState();
               break;
            case ControlState.Attacking:
                AttackingState();
                break;
            case ControlState.ShowingDisk:
                ShowDiskState();
                break;
            case ControlState.ProcessMove:
                ProcessMove();
                break;
        }
    }

    public void StartAttackSelection()
    {
        AttackOverride = true;
    }

    public void IdleState()
    {
        if ((ClickHitState.Ship != null && ClickHitState.Ship.IsHostile) || AttackOverride)
        {
            ShowAttackCursor();
        }
        else
        {
            ShowDefaultCursor();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (AttackOverride && ClickHitState.Ship != null)
            {
                //animator.SetBool(UIAnimationControlParameters.Attacking, true);
                _controlState = ControlState.Attacking;
            }
            else
            {
                if (ClickHitState.Ship != null)
                {
                    SelectionManager.Instance.SelectShip(ClickHitState.Ship);
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            var selectedShip = SelectionManager.Instance.GetSelectedShip();
            if (selectedShip != null && !selectedShip.IsFixed)
            {
                if (ClickHitState.Ship != null)
                {
                    if (ClickHitState.Ship.IsHostile)
                    {
                        //animator.SetBool(UIAnimationControlParameters.Attacking, true);
                        _controlState = ControlState.Attacking;
                    }
                    else
                    {
                        //animator.SetBool(UIAnimationControlParameters.Moving, true);
                        _controlState = ControlState.ProcessMove;
                    }

                    MovementInformation.SetDestination(ClickHitState.Ship);
                }
                else
                {
                    //animator.SetBool(UIAnimationControlParameters.ShowDisk, true);
                    _controlState = ControlState.ShowingDisk;
                }
            }
        }
    }

    public void ShowDiskState()
    {
        if (Input.GetMouseButtonUp(1))
        {
            //_controlState = ControlState.ShowingDisk;
            _controlState = ControlState.ProcessMove;
            return;
        }

        var moveDisk = GetMoveDisk();

        //moveDisk.gameObject.SetActive(true);
        var ship = SelectionManager.Instance.GetSelectedShip();
        moveDisk.Activate(ship.gameObject);

        if (Input.GetMouseButtonDown(0))
        {
            StartDebounce();
        }

        if ((Input.GetMouseButton(0) && HasDebounceCompleted()) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveDisk.SetDiskMode(MoveDisk.DiskMode.Vertical);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveDisk.SetDiskMode(MoveDisk.DiskMode.Horizontal);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _controlState = ControlState.ProcessMove;
            var position = moveDisk.HitPoint;
            MovementInformation.SetDestination(position);
            moveDisk.Deactivate();
        }
    }

    public void AttackingState()
    {
        var clickState = ClickHitState;
        var selectedShip = SelectionManager.Instance.GetSelectedShip();

        selectedShip.Attack(clickState.Ship);
        AttackOverride = false;
        //animator.SetBool(UIAnimationControlParameters.Attacking, false);
        _controlState = ControlState.IdleState;
    }

    public void ProcessMove()
    {
        var selectionManager = SelectionManager.Instance;

        var movement = MovementInformation;
        switch (movement.MoveTargetType)
        {
            case MoveTargetType.Position:
                selectionManager.GetSelectedShip().MoveTo(movement.Location);
                break;
            case MoveTargetType.Ship:
                selectionManager.GetSelectedShip().ApproachTarget(movement.Ship);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //animator.SetBool(UIAnimationControlParameters.Moving, false);
        _controlState = ControlState.IdleState;
        ;
    }

    private float _mouseButtonDownTime;
    private float _mouseButtonDebounceInterval = 0.2f;

    private void StartDebounce()
    {
        _mouseButtonDownTime = Time.time;
    }
    private bool HasDebounceCompleted()
    {
        return Time.time - _mouseButtonDownTime > _mouseButtonDebounceInterval;
    }
}
