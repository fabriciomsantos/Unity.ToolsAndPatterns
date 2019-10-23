using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

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
            public bool useEncryption;
            public bool formatJson;
            public ScriptableObject objectToSave;
        }

        #region Public Variables
        public bool loadOnEnable;
        public string encryptionKey = "b14ca5898a4e4133bbce2ea2315a1916";
        public SaveType[] saveTypes;

        #endregion

        #region Private Variables

        #endregion

        #region Unity Methods
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
            foreach (var save in saveTypes)
            {
                var fileName = save.fileName + ".json";
                var filePath = Path.Combine(Application.persistentDataPath, fileName);

                if (save.objectToSave)
                {
                    string dataAsJson = JsonUtility.ToJson(save.objectToSave, save.formatJson);
                    if (save.useEncryption)
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
        }
        public void LoadGame()
        {
            foreach (var save in saveTypes)
            {
                var fileName = save.fileName + ".json";
                var filePath = Path.Combine(Application.persistentDataPath, fileName);

                if (File.Exists(filePath))
                {
                    string dataAsJson = File.ReadAllText(filePath);
                    if (save.useEncryption)
                    {
                        dataAsJson = AesOperationEncryption.DecryptString(encryptionKey, dataAsJson);
                    }
                    JsonUtility.FromJsonOverwrite(dataAsJson, save.objectToSave);
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

    public class AesOperationEncryption
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