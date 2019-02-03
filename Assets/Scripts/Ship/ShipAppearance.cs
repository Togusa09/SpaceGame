using System;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

namespace Scripts.Ship
{
    [RequireComponent(typeof(BoxCollider))]
    public class ShipAppearance : MonoBehaviour
    {
        public GameObject ShipModel;
        public bool IsHostile;
        public bool IsSelected;
        public bool IsFixed;
        public float MoveSpeed;
        public float TurnSpeed;

        private Vector3 _destination;
        private GameObject _destinationCircle;
        private GameObject _destinationLine;
        private Bounds _bounds;

        private Vector3 DestinationVectorLocal => transform.position - _destination;
        private float DestinationDistance => Vector3.Distance(transform.position, _destination);
        


        // Start is called before the first frame update
        void Start()
        {
            var model = transform.Find("ShipModel");
            if (!model)
            {
                Instantiate(ShipModel, transform);
            }

            _destination = transform.position;

            //_engines = GetComponentsInChildren<Engine>();
            var meshFilters = GetComponentsInChildren<MeshFilter>();
            _bounds = new Bounds();

            foreach (var mesh in meshFilters)
            {
                _bounds.Encapsulate(mesh.sharedMesh.bounds);
                
            }

            gameObject.DrawCircle(_bounds.max.magnitude / 2, 1f, Color.green);
            var line = GetComponent<LineRenderer>();
            line.enabled = false;

            _destinationCircle = new GameObject("DestinationCircle");
            _destinationCircle.transform.SetParent(this.transform);
            _destinationCircle.DrawCircle(10, 1f, Color.green);
            _destinationCircle.SetActive(false);
            _destinationLine = new GameObject("DestinationLine");
            _destinationLine.transform.SetParent(this.transform);
            var lineRenderer = _destinationLine.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
            lineRenderer.enabled = true;
            lineRenderer.material.color = Color.green;
            _destinationLine.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            ShowLineIfSelected();
            ProcessMovement();

            if (Mathf.Abs(DestinationDistance) > 1f)
            {
                _destinationCircle.transform.position = _destination;

                var moveDir = DestinationVectorLocal;
                var circleEdge = Vector3.Normalize(new Vector3(moveDir.x, 0, moveDir.z)) * 10f;

                var lineRenderer = _destinationLine.GetComponent<LineRenderer>();
                var points = new[] { _destination + circleEdge, transform.position };
                lineRenderer.SetPositions(points);
                _destinationCircle.transform.rotation = Quaternion.identity; ;

                _destinationCircle.SetActive(true);
                _destinationLine.SetActive(true);
            }
            else
            {
                _destinationCircle.SetActive(false);
                _destinationLine.SetActive(false);
            }
        }

        private enum MoveState
        {
            Stopped,
            Turning,
            Moving
        }

        private MoveState _moveState;

        private void ProcessMovement()
        {
            switch (_moveState)
            {
                case MoveState.Stopped:
                    ProcessMoveStopped();
                    break;
                case MoveState.Turning:
                    ProcessTurning();
                    break;
                case MoveState.Moving:
                    ProcessMoving();
                    break;
            }
        }

        private void ProcessMoveStopped()
        {
            //    foreach (var engine in _engines)
            //    {
            //        engine.StopEngine();
            //    }
            if (DestinationDistance > 0.1f)
            {
                _moveState = MoveState.Turning;
            }
        }

        private void ProcessMoving()
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, MoveSpeed);

            //    foreach (var engine in _engines)
            //    {
            //        engine.StartEngine();
            //    }

            if (DestinationDistance < 0.1f)
            {
                _moveState = MoveState.Stopped;
                return;
            }

            var direction = Quaternion.LookRotation(DestinationVectorLocal, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, direction, Time.deltaTime * TurnSpeed);

            var angle = Vector3.Angle(transform.forward, DestinationVectorLocal);

            if (angle < 2)
            {
                _moveState = MoveState.Turning;
            }
        }

        private void ProcessTurning()
        {
            // https://answers.unity.com/questions/29751/gradually-moving-an-object-up-to-speed-rather-then.html

            var direction = Quaternion.LookRotation(DestinationVectorLocal, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, direction, Time.deltaTime * TurnSpeed);

            var remainingAngle = Quaternion.Angle(transform.rotation, direction);
            if (Mathf.Abs(remainingAngle) < 2)
            {
                _moveState = MoveState.Moving;
            }
        }

        private void ShowLineIfSelected()
        {
            var line = GetComponent<LineRenderer>();
            if (IsSelected)
            {
                if (line != null)
                {
                    line.enabled = true;
                }
            }
            else
            {
                if (line != null)
                {
                    line.enabled = false;
                }
            }
        }

        public void StopAll()
        {
            GetComponent<TurretMounting>().StopAll();
            _moveState = MoveState.Stopped;
            _destination = transform.position;
        }

        public void MoveTo(Vector3 destination)
        {
            Debug.Log("Moving to " + destination);
            _destination = destination;
        }

        public void ApproachTarget(ShipAppearance ship)
        {
            ApproachToDistance(ship.transform.position, _bounds.max.magnitude);
        }


        public void ApproachToDistance(Vector3 position, float distance)
        {
            var dir = transform.position - position;
            var target = new Ray(position, dir);
            Debug.DrawRay(position, dir, Color.blue, 6);
            var destination = target.GetPoint(distance);
            MoveTo(destination);
        }
    }
}
