using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
namespace Tools.Components.Events
{
    [RequireComponent(typeof(Collider))]
    public class OnCollisionEvent : MonoBehaviour
    {
        [System.Serializable]
        public class GameObjectEvent : UnityEvent<GameObject>
        { }

        public GameObjectEvent OnCollisionEnterEvent = new GameObjectEvent();

        public GameObjectEvent OnCollisionStayEvent = new GameObjectEvent();

        public GameObjectEvent OnCollisionExitEvent = new GameObjectEvent();

        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        void OnCollisionEnter(Collision other)
        {
            OnCollisionEnterEvent.Invoke(other.gameObject);
        }

        /// <summary>
        /// OnCollisionStay is called once per frame for every collider/rigidbody
        /// that is touching rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        void OnCollisionStay(Collision other)
        {
            OnCollisionStayEvent.Invoke(other.gameObject);
        }

        /// <summary>
        /// OnCollisionExit is called when this collider/rigidbody has
        /// stopped touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        void OnCollisionExit(Collision other)
        {
            OnCollisionExitEvent.Invoke(other.gameObject);
        }
    }
}