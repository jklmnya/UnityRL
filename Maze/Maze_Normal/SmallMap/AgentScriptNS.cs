using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace Maze
{
    
    public class AgentScriptNS : Agent
    {

        private Vector3 Up = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 Down = new Vector3(0.0f, 0.0f, 1.0f);
        private Vector3 Left = new Vector3(-1.0f, 0.0f, 0.0f);
        private Vector3 Right = new Vector3(1.0f, 0.0f, 0.0f);
        private Vector3 Dir;
        private float Speed = 0.2f;

        public GameObject StartLocationGameObject;
        public GameObject TargetGameObject;

        private Vector3 TargetLocation;
        private Vector3 AgentInitLocation;

        public int Len;

        private int Total = 0;
        private int Succeed = 0;
        private bool Find = false;

        public override void Initialize() {
            // this.tag = "agent";
            this.MaxStep = 2000;
        }

        public override void OnEpisodeBegin() {
            CancelInvoke("Move");
            // Debug.Log("Episode Start");
            InitAgentAndTarget();
            if (Find)
                ++Succeed;
            ++Total;
            Find = false;
            Invoke("StartLearn", 0.3f);
        }

        public override void CollectObservations(VectorSensor sensor) {
            // sensor.AddObservation(TargetLocation);
            // sensor.AddObservation(TargetGameObject.transform.position);
        }

        public override void OnActionReceived(ActionBuffers actions) {
            int Direction = actions.DiscreteActions[0];
            if (Direction == 0)
                Dir = Up;
            else if (Direction == 1)
                Dir = Down;
            else if (Direction == 2)
                Dir = Left;
            else
                Dir = Right;
            TargetLocation = TargetLocation + Dir * 10.0f;
            SetReward(-1.0f / MaxStep);
            InvokeRepeating("Move", 0.0f, 0.01f);
        }

        public override void Heuristic(in ActionBuffers actionsOut) {
            var ActionsOut = actionsOut.DiscreteActions;
            ActionsOut[0] = Random.Range(0, 4);
        }
        
        private void Move() {
            this.transform.position += Dir * Speed;
            float F = Vector3.Distance(this.transform.position, TargetLocation);
            if (F <= 0.01f) {
                // Debug.Log(this.transform.position + " " + TargetLocation);
                CancelInvoke("Move");
                this.RequestDecision();
            }
        }

        private void StartLearn() {
            this.RequestDecision();
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.CompareTag("wall")) {
                CancelInvoke("Move");
                SetReward(-2.0f);
                EndEpisode();
            } else if (collision.gameObject.CompareTag("goal")) {
                SetReward(5.0f);
                CancelInvoke("Move");
                Find = true;
                // Debug.Log("Goal");
                EndEpisode();
            }
        }

        private void InitAgentAndTarget() {
            int dx = Random.Range(0, Len);
            int dz = Random.Range(0, Len);
            Vector3 AgentLocation = StartLocationGameObject.transform.position + new Vector3(dx * 10.0f, 0.0f, -dz * 10.0f);
            TargetLocation = AgentLocation;
            this.transform.position = AgentLocation;
            AgentInitLocation = AgentLocation;
            while (true) {
                int _dx = Random.Range(0, Len);
                int _dz = Random.Range(0, Len);
                if (_dx != dx && _dz != dz) {
                    Vector3 TargetLocation = StartLocationGameObject.transform.position + new Vector3(_dx * 10.0f, 0.0f, -_dz * 10.0f);
                    TargetGameObject.transform.position = TargetLocation;
                    break;
                }
            }
        }

        public int[] getData() {
            return new int[2] { Succeed, Total };
        }
    }

}

