using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;

namespace Maze {

    public class CountScriptS : MonoBehaviour {
        public Maze.AgentScriptNS[] NormalAgents;
        public Maze.AgentScriptBS[] BackAgents;

        // Start is called before the first frame update
        void Start() {
            InvokeRepeating("Show", 30.0f, 30.0f);
        }

        private void Show() {
            int[] NormalData = new int[2];
            int[] BackData = new int[2];
            foreach (Maze.AgentScriptNS Agent in NormalAgents) {
                int[] Data = Agent.getData();
                NormalData[0] += Data[0];
                NormalData[1] += Data[1];
            }
            foreach (Maze.AgentScriptBS Agent in BackAgents) {
                int[] Data = Agent.getData();
                BackData[0] += Data[0];
                BackData[1] += Data[1];
            }
            Debug.Log("With Curiosity: " + BackData[0] + " of " + BackData[1] + " Found -> " + (double)BackData[0] / BackData[1]);
            Debug.Log("Without Curiosity: " + NormalData[0] + " of " + NormalData[1] + " Found -> " + (double)NormalData[0] / NormalData[1]);
        }
    }


}
