using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
namespace Tools.Components.Events
{
    [RequireComponent(typeof(Collider))]
    public class OnTriggerEvent : MonoBehaviour
    {
        [System.Serializable]
        public class GameObjectEvent : UnityEvent<GameObject>
        { }

        public GameObjectEvent OnTriggerEnterEvent = new GameObjectEvent();

        public GameObjectEvent OnTriggerStayEvent = new GameObjectEvent();

        public GameObjectEvent OnTriggerExitEvent = new GameObjectEvent();

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent.Invoke(other.gameObject);
        }

        /// <summary>
        /// OnTriggerStay is called once per frame for every Collider other
        /// that is touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerStay(Collider other)
        {
            OnTriggerStayEvent.Invoke(other.gameObject);
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent.Invoke(other.gameObject);
        }
    }
}