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
        private Blackboard blackboard;
		private Vector2 randomTargetInContainer;
		public Arrive arriveScript;

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

        void Start ()
		{
			AgentList.Add (this);
			// emile
			if (blackboard == null) {
				blackboard = GameObject.Find("Blackboard").GetComponent<Blackboard>();
			}
			arriveScript = GetComponent<Arrive>();
        }

        void Update ()
		{
			Vector2 acceleration = Vector2.zero;

			foreach (SteeringBehaviour behaviour in behaviours) {
				if (behaviour.enabled)
					acceleration += behaviour.GetVelocity () * behaviour.Weight;
			}

			// emile
//			Debug.Log("magnitude... " + distFromBounds.magnitude);
			if (transform.position.y >= blackboard.seaBounds.max.y - blackboard.seaBuffer
			    || transform.position.y <= blackboard.seaBounds.min.y + blackboard.seaBuffer
			    || transform.position.x >= blackboard.seaBounds.max.x - blackboard.seaBuffer
			    || transform.position.x <= blackboard.seaBounds.min.x + blackboard.seaBuffer) {
				if (!arriveScript.enabled) {
					arriveScript.enabled = true;
				} 
				arriveScript.TargetPoint = GetRandomContainerPoint ();
			}

			CurrentVelocity += acceleration / Mass;

			CurrentVelocity -= CurrentVelocity * Friction;

			if (CurrentVelocity.magnitude > MaxVelocity)
				CurrentVelocity = CurrentVelocity.normalized * MaxVelocity;

			transform.position = transform.position + (Vector3)CurrentVelocity * Time.deltaTime;
        
			if (RotateSprite && CurrentVelocity.magnitude > 0.0001f) {
				float angle = Mathf.Atan2 (CurrentVelocity.y, CurrentVelocity.x) * Mathf.Rad2Deg;

				transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, angle);
			}

        }

        void OnDestroy()
        {
            AgentList.Remove(this);
        }

        // emile
        Vector2 GetRandomContainerPoint(){
			//randomTargetInContainer
			float randomX = Random.Range(blackboard.seaBounds.min.x + blackboard.seaBuffer, blackboard.seaBounds.max.x - blackboard.seaBuffer);
			float randomY = Random.Range(blackboard.seaBounds.min.y + blackboard.seaBuffer, blackboard.seaBounds.max.y - blackboard.seaBuffer);
			return new Vector2(randomX, randomY);
        }
    }
}