using System;
using System.Collections.Generic;
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
    private DebouncedMouseInput _debouncedMouseInput;

    public Control()
    {
        _debouncedMouseInput = new DebouncedMouseInput();
    }

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

    public CameraControl CameraControl;

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
        _debouncedMouseInput.Update();

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

        if (_debouncedMouseInput.GetMouseButtonUp(0))
        {
            Debug.Log("Button click");
        }

        if (_debouncedMouseInput.GetMouseButtonHoldDown(0))
        {
            Debug.Log("Button hold");
        }

        if (Input.GetKey(KeyCode.W))
        {
            CameraControl.MoveNorth();
        }
        if (Input.GetKey(KeyCode.S))
        {
            CameraControl.MoveSouth();
        }
        if (Input.GetKey(KeyCode.A))
        {
            CameraControl.MoveWest();
        }
        if (Input.GetKey(KeyCode.D))
        {
            CameraControl.MoveEast();
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            CameraControl.MoveVertical(Input.mouseScrollDelta.y);
        }

        if (Input.GetMouseButton(2)) // Right mouse button down
        {
            CameraControl.Rotate(Input.GetAxis("Mouse X"));
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

        if (_debouncedMouseInput.GetMouseButtonUp(1))
        {
            var selectedShip = SelectionManager.Instance.GetSelectedShip();
            if (selectedShip != null && !selectedShip.IsFixed)
            {
                if (ClickHitState.Ship != null)
                {
                    if (ClickHitState.Ship.IsHostile)
                    {
                        _controlState = ControlState.Attacking;
                    }
                    else
                    {
                        _controlState = ControlState.ProcessMove;
                    }

                    MovementInformation.SetDestination(ClickHitState.Ship);
                }
                else
                {
                    _controlState = ControlState.ShowingDisk;

                    var ship = SelectionManager.Instance.GetSelectedShip();
                    
                    GetMoveDisk().Activate(ship.gameObject);
                }
            }
        }
    }

    public void ShowDiskState()
    {
        var moveDisk = GetMoveDisk();

        if (Input.GetMouseButtonUp(1))
        {
            _controlState = ControlState.IdleState;
            moveDisk.Deactivate();
            return;
        }

        if ((_debouncedMouseInput.GetMouseButtonHoldDown(0)) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveDisk.SetDiskMode(MoveDisk.DiskMode.Vertical);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveDisk.SetDiskMode(MoveDisk.DiskMode.Horizontal);
        }

        if (_debouncedMouseInput.GetMouseButtonUp(0))
        {
            _controlState = ControlState.ProcessMove;
            var position = moveDisk.HitPoint;
            MovementInformation.SetDestination(position);
            moveDisk.Deactivate();
            _controlState = ControlState.ProcessMove;
        }
    }

    public void AttackingState()
    {
        var clickState = ClickHitState;
        var selectedShip = SelectionManager.Instance.GetSelectedShip();

        selectedShip.Attack(clickState.Ship);
        AttackOverride = false;
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

        _controlState = ControlState.IdleState;
        ;
    }
}
