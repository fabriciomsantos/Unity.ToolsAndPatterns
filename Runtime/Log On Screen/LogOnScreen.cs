using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Tools.DebugTools
{
    public class LogOnScreen : MonoBehaviour
    {
        public float time = 1;
        Queue<string> myLogQueue = new Queue<string>();

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            string newString = "\n <b>[" + type + "] : </b>" + logString;

            if (type == LogType.Exception)
            {
                newString += "\n" + stackTrace;
                newString = "<color=red>" + newString + "</color>";
            }
            if (type == LogType.Error)
            {
                newString = "<color=red>" + newString + "</color>";
            }
            if (type == LogType.Warning)
            {
                newString = "<color=yellow>" + newString + "</color>";
            }

            myLogQueue.Enqueue(newString);
            StartCoroutine(RemoveLog());
        }

        void OnGUI()
        {
            foreach (string mylog in myLogQueue)
            {
                GUILayout.Label(mylog);
            }
        }

        IEnumerator RemoveLog()
        {
            yield return new WaitForSeconds(time);
            myLogQueue.Dequeue();
        }
    }
}