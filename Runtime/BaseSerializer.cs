using Newtonsoft.Json;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FertileSky.Serialization
{
    /// <summary>
    /// Basic Json Serializer
    /// </summary>
    public static class BaseSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly JsonSerializerSettings SERIALIZER_SETTINGS =
            new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

        /// <summary>
        /// Gets the absolute path to a file.
        /// Does not check if the file exists or not.
        /// Uses forward slash '/'
        /// <para> Path should be: [ApplicationPath]/[relativePrecedingPath]/[Fully-Qualified-namespace-of-class-instance][_][fileUniqueName].json</para> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="relativePrecedingPath"></param>
        /// <param name="fileUniqueName"></param>
        /// <returns></returns>
        public static string GetPath<T>(
            string relativePrecedingPath,
            string fileUniqueName)
        {
            return GetAbsoluteDirectoryPath(relativePrecedingPath) +
            typeof(T) + "_" + fileUniqueName + ".json";
        }

        /// <summary>
        /// Get the absolute path to a folder. 
        /// Takes in account the relative Preceding path. 
        /// Uses forward slash '/'.
        /// </summary>
        /// <param name="relativePrecedingPath">This will be added after the application path and before the file name.</param>
        /// <returns>the path to the directory</returns>
        public static string GetAbsoluteDirectoryPath(string relativePrecedingPath)
        {
            return Application.dataPath + "/" + relativePrecedingPath + "/";
        }

        //TODO : Fix this
        private static bool TryToMakePathDirectory(
            string path,
            out string createdPath)
        {

            createdPath = Directory.CreateDirectory(path).ToString();
            return true;
        }

        /// <summary>
        /// Saves object to a file with JSON string representation of that object.
        /// It supports derived types, properties and collections.
        /// This tool does some basic path validation.null If the path is not valid, it will return null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize">Instance of the object that will be serialized</param>
        /// <param name="fileUniqueNamePart">String that will be used to name the saved file.</param>
        /// <param name="aditionalPathPart">Relative path for the file to be saved. 
        /// The default directory is the project's root directory\Assets. </param>
        /// <param name="overwrite">Should the file be overwritten if found.</param>
        /// <returns></returns>
        /// <exception cref="System.NullReferenceException">Method will return null if the path is not valid.</exception>
        public static string SaveToJson<T>(
            T objectToSerialize,
            string fileUniqueName,
            string relativePrecedingPath = "",
            bool overwrite = false)
        {
            string directoryPath = GetAbsoluteDirectoryPath(relativePrecedingPath);
            string filePath = GetPath<T>(relativePrecedingPath, fileUniqueName);

            if (File.Exists(filePath) && !overwrite)
                throw new System.FormatException(
                     @"Cannot save file to this location 
                     because File already Exists! Use Overwrite parameter!");
            bool de = Directory.Exists(directoryPath);
            if (!de &
            !TryToMakePathDirectory(directoryPath, out directoryPath))
                throw new System.FormatException(@"Cannot create directory at path " + directoryPath);

            string json = JsonConvert.SerializeObject(objectToSerialize, SERIALIZER_SETTINGS);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(json);
                stream.Write(info, 0, info.Length);
                stream.Close();
            }
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            return filePath;
        }

        /// <summary>
        /// Loads objects from a file path. The path is absolute, eg 
        ///  C:\MyProject\Assets\Monsters\{monster-File-Name}.json
        /// The filepath needs to be of JSON format.
        /// It uses TypeNameHandling.All so any changes to namespaces will make the
        /// previous namespace json obsolete and unusable.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadFromJson<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning(string.Format("Path {0} is empty", path));
                return default(T);
            }
            if (!File.Exists(path))
            {
                Debug.LogWarning(string.Format("Path {0} is not a file.", path));
                return default(T);
            }

            using (var stream = new FileStream(path, FileMode.Open))
            {
                TextReader tr = new StreamReader(stream);
                var deserializedProduct = JsonConvert.DeserializeObject<T>(tr.ReadToEnd(), SERIALIZER_SETTINGS);
                stream.Close();
                tr.Close();
                return deserializedProduct;
            }
        }
    }
}