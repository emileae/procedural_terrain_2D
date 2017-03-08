using UnityEngine;
using System.Collections.Generic;

namespace Steer2D
{
    public class SteeringAgent : MonoBehaviour
    {
        public float MaxVelocity = 1;
        public float Mass = 10;
        public float Friction = .05f;
        public bool RotateSprite = true;

        // emile
        public GameObject container;
		public Bounds containerBounds;

        [HideInInspector]
        public Vector2 CurrentVelocity;

        public static List<SteeringAgent> AgentList = new List<SteeringAgent>();

        List<SteeringBehaviour> behaviours = new List<SteeringBehaviour>();

        public void RegisterSteeringBehaviour(SteeringBehaviour behaviour)
        {
            behaviours.Add(behaviour);
        }

        public void DeregisterSteeringBehaviour(SteeringBehaviour behaviour)
        {
            behaviours.Remove(behaviour);
        }

        void Start()
        {
            AgentList.Add(this);
			containerBounds = container.GetComponent<MeshRenderer>().bounds;
        }

        void Update ()
		{
			Vector2 acceleration = Vector2.zero;

			foreach (SteeringBehaviour behaviour in behaviours) {
				if (behaviour.enabled)
					acceleration += behaviour.GetVelocity () * behaviour.Weight;
			}

			Vector2 distFromBounds = new Vector2 (transform.position.x - containerBounds.max.x, transform.position.y - containerBounds.max.y);
//			Debug.Log("magnitude... " + distFromBounds.magnitude);
			if (distFromBounds.magnitude < 90) {
				Debug.Log("Too close to bounds................ move away");
			}

            CurrentVelocity += acceleration / Mass;

            CurrentVelocity -= CurrentVelocity * Friction;

            if (CurrentVelocity.magnitude > MaxVelocity)
                CurrentVelocity = CurrentVelocity.normalized * MaxVelocity;

            transform.position = transform.position + (Vector3)CurrentVelocity * Time.deltaTime;
        
            if (RotateSprite && CurrentVelocity.magnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(CurrentVelocity.y, CurrentVelocity.x) * Mathf.Rad2Deg;

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
            }
        }

        void OnDestroy()
        {
            AgentList.Remove(this);
        }
    }
}