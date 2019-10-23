using System.Collections.Generic;

using UnityEngine;

namespace Tools.GamePatterns
{
    public class ObjectPool : Singleton<ObjectPool>
    {
        [System.Serializable]
        public struct Pool
        {
            public string name;
            public GameObject prefab;
            [Min(1)]
            public int size;
        }

        #region Public Variables
        public List<Pool> pools;
        public Dictionary<string, Queue<GameObject>> poolDictionary;

        #endregion

        #region Private Variables

        #endregion

        #region Unity Methods

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            InitializePool();
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Spawn object from the pool
        /// </summary>
        /// <param name="name">name of the pool</param>
        /// <param name="useInterface">use IPooledObject interface on the prefab</param>
        /// <param name="worldPosition">set object world position</param>
        /// <param name="worldRotation">set object world rotation</param>
        /// <returns></returns>
        public GameObject SpawnFromPool(string name, bool useInterface = false, Vector3 worldPosition = default(Vector3), Quaternion worldRotation = default(Quaternion))
        {
            if (!poolDictionary.ContainsKey(name))
            {
                Debug.LogWarning("Pool with name " + name + " doesn't exist");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[name].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = worldPosition;
            objectToSpawn.transform.rotation = worldRotation;

            if (useInterface)
            {
                IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

                if (pooledObject != null)
                {
                    pooledObject.OnObjectSpawn();
                }
            }

            poolDictionary[name].Enqueue(objectToSpawn);

            return objectToSpawn;

        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Assing list to the dictionary
        /// </summary>
        void InitializePool()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                if (pool.prefab)
                {
                    for (int i = 0; i < pool.size; i++)
                    {
                        GameObject obj = Instantiate(pool.prefab);
                        obj.SetActive(false);
                        objectPool.Enqueue(obj);
                    }
                    poolDictionary.Add(pool.name, objectPool);
                }
                else
                {
                    Debug.LogWarning("Pool with name " + name + " doesn't have a prefab");
                }
            }
        }

        #endregion     
    }
}