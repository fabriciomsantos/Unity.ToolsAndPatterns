using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Tools.EditorTools.Attributes;
using Tools.GamePatterns;

using UnityEngine;

namespace Tools.Save.Local
{
    public class LocalSaveManager : Singleton<LocalSaveManager>
    {
        [System.Serializable]
        public class SaveType
        {
            public string fileName;
            public bool formatJson;

            [InspectInline(canEditRemoteTarget = true)]
            public ScriptableObject objectToSave;
        }

        #region Public Variables
        [Header("Settings")]
        public bool loadOnEnable;
        public bool uniqueFile;

        [Header("Encryption")]
        public bool useEncryption;
        public string encryptionKey = "b14ca5898a4e4133bbce2ea2315a1916";

        [Header("Objects")]
        public SaveType[] saveTypes;

        #endregion

        #region Private Variables
        private const char separator = ';';

#if UNITY_EDITOR
        [Header("Test")][SerializeField]
        private bool load = false;
        [SerializeField]
        private bool save = false;
#endif

        #endregion

        #region Unity Methods

        /// <summary>
        /// Called when the script is loaded or a value is changed in the
        /// inspector (Called in the editor only).
        /// </summary>
        void OnValidate()
        {
            if (save)
            {
                SaveGame();
                save = false;
            }

            if (load)
            {
                LoadGame();
                load = false;
            }
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            if (loadOnEnable)
            {
                LoadGame();
            }
        }

        #endregion

        #region Public Methods
        public void SaveGame()
        {
            if (uniqueFile)
            {
                var fileName = "save.json";
                var filePath = Path.Combine(Application.persistentDataPath, fileName);
                string dataAsJson = "";
                foreach (var save in saveTypes)
                {
                    if (save.objectToSave)
                    {
                        dataAsJson += JsonUtility.ToJson(save.objectToSave, save.formatJson) + separator + "\n";
                        if (useEncryption)
                        {
                            dataAsJson = AesOperationEncryption.EncryptString(encryptionKey, dataAsJson);
                        }
                    }
                    else
                    {
                        Debug.LogError("missing object data");
                    }
                }
                if (dataAsJson != null)
                {
                    File.WriteAllText(filePath, dataAsJson);
                    Debug.Log("File Saved");
                }
            }
            else
            {
                foreach (var save in saveTypes)
                {
                    var fileName = save.fileName + ".json";
                    if (save.fileName == "")
                    {
                        Debug.LogWarning("Enter file names");
                        return;
                    }
                    var filePath = Path.Combine(Application.persistentDataPath, fileName);

                    if (save.objectToSave)
                    {
                        string dataAsJson = JsonUtility.ToJson(save.objectToSave, save.formatJson);
                        if (useEncryption)
                        {
                            dataAsJson = AesOperationEncryption.EncryptString(encryptionKey, dataAsJson);
                        }
                        File.WriteAllText(filePath, dataAsJson);

                    }
                    else
                    {
                        Debug.LogError("missing object data");
                    }
                }
                Debug.Log("Files Saved");
            }
        }
        public void LoadGame()
        {
            for (int i = 0; i < saveTypes.Length; i++)
            {
                SaveType save = saveTypes[i];

                string fileName;

                if (uniqueFile)
                {
                    fileName = "save.json";
                }
                else
                {
                    if (save.fileName == "")
                    {
                        Debug.LogWarning("Enter file name");
                        return;
                    }
                    fileName = save.fileName + ".json";
                }

                var filePath = Path.Combine(Application.persistentDataPath, fileName);

                if (File.Exists(filePath))
                {
                    string dataAsJson = File.ReadAllText(filePath);

                    if (useEncryption)
                    {
                        dataAsJson = AesOperationEncryption.DecryptString(encryptionKey, dataAsJson);
                    }

                    if (uniqueFile)
                    {
                        string[] types = dataAsJson.Split(separator);
                        JsonUtility.FromJsonOverwrite(types[i], save.objectToSave);
                        Debug.Log("File Loaded");
                    }
                    else
                    {
                        JsonUtility.FromJsonOverwrite(dataAsJson, save.objectToSave);
                        Debug.Log("Files Loaded");
                    }
                }
                else
                {
                    Debug.LogError("Couldn't find save file");
                }

            }
        }

        #endregion

        #region Private Methods

        #endregion
    }

    public static class AesOperationEncryption
    {
        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using(Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using(MemoryStream memoryStream = new MemoryStream())
                {
                    using(CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using(StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using(Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using(MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using(CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using(StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}