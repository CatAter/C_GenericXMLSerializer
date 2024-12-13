using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

#if UNITY_EDITOR
namespace GenericSaveEditor
{
    /// <summary>
    /// The operation type to use during serialization. 
    /// </summary>
    public enum OperationType
    {
        DEFAULT,
        EXTERNAL
    }

    /// <summary>
    /// The serialization format to use. 
    /// </summary>
    public enum SerializationFormat
    {
        XML,
        JSON
    }

    /// <summary>
    /// Class used to define serialization schema. 
    /// </summary>
    public class SerializationSchema
    {
        public const string XML_EXTENSION = ".xml";
        public const string JSON_EXTENSION = ".json";
        public SerializationFormat serializationType = SerializationFormat.XML;

        /// <summary>
        /// The filename used for naming default data files. 
        /// </summary>
        public string defaultFilename = "DLS";

        /// <summary>
        /// The filename used for naming player data files. 
        /// </summary>
        public string externalFilename = "PLS";

        /// <summary>
        /// String used to name the directory for IO. 
        /// </summary>
        public string folderName = "Data";

        /// <summary>
        /// The folder path for persistent player data. 
        /// </summary>
        public string PersistentDataPath => Application.persistentDataPath + "/" + folderName + "/";

        /// <summary>
        /// The folder path for serialized editor data.  
        /// </summary>
        public string StreamingAssetsPath => Application.streamingAssetsPath + "/" + folderName + "/";

        /// <summary>
        /// Returns the full path used for default data file serialization/deserialization. 
        /// </summary>
        /// <param name="additionalNameData">The additional name data provided. </param>
        public string GetFullPath_Default(string additionalNameData, bool addExtentsion = false)
        {
            return StreamingAssetsPath + GetFullFileName(additionalNameData, OperationType.DEFAULT, addExtentsion);
        }

        /// <summary>
        /// Returns the full path used for Player data file serialization/deserialization. 
        /// </summary>
        /// <param name="additionalNameData">The additional name data provided. </param>
        public string GetFullPath_External(string additionalNameData, bool addExtension = false)
        {
            return PersistentDataPath + GetFullFileName(additionalNameData, OperationType.EXTERNAL, addExtension);
        }

        /// <summary>
        /// Returns the full file name with provided additional data. 
        /// </summary>
        /// <param name="additionalNameData"> The additional file name data. </param>
        /// <param name="opType"> The operation type to perform. </param>
        /// <param name="addExtension"> </param>
        /// <returns> Returns the formatted string with added details. </returns>
        public string GetFullFileName(string additionalNameData, OperationType opType, bool addExtension = false)
        {
            string baseString = "";
            string extent = "";

            switch (opType)
            {
                case OperationType.DEFAULT:
                    baseString = defaultFilename + "_{0}";
                    break;
                case OperationType.EXTERNAL:
                    baseString = externalFilename + "_{0}";
                    break;
                default:
                    baseString = defaultFilename + "_{0}";
                    break;
            }

            if (addExtension)
            {
                switch (serializationType)
                {
                    case SerializationFormat.XML:
                        extent = XML_EXTENSION;
                        break;
                    case SerializationFormat.JSON:
                        extent = JSON_EXTENSION;
                        break;
                    default:
                        extent = XML_EXTENSION;
                        break;
                }
            }

            return string.Format(baseString + extent, additionalNameData);
        }
    }

    /// <summary>
    /// Class used to control save file parsing flow. 
    /// </summary>
    /// <typeparam name="T"> The type used by the class. </typeparam>
    public class GenericSaveHandler<T> where T : class
    {
        /// <summary>
        /// flag denoting whether internal data serialization should be used.  
        /// </summary>
        public bool useCustomSerialization = false;

        /// <summary>
        /// The serialization schema used for the data type. 
        /// </summary>
        private SerializationSchema schema;

        #region Serialization Actions. 
        /// <summary>
        /// Action (anonymous function) used for receiving calls for custom Default serialization.
        /// Only called if GenericSaveHandler.useCustomSerialization is enabled.
        /// </summary>
        public Action<T, SerializationSchema, string> SaveDefaultAction;

        /// <summary>
        /// Action (anonymous function) used for receiving calls for custom Player serialization.  
        /// Only called if GenericSaveHandler.useCustomSerialization is enabled.
        /// </summary>
        public Action<T, SerializationSchema, string> SaveExternalAction;

        /// <summary>
        /// Function (anonymous function) used to receive calls for custom Default deserialization.
        /// Only called if GenericSaveHandler.useCustomSerialization is enabled.
        /// </summary>
        public Func<SerializationSchema, string, T> LoadDefaultFunc;

        /// <summary>
        /// Function (anonymous function) used to receive calls for custom Player deserialization.
        /// Only called if GenericSaveHandler.useCustomSerialization is enabled.
        /// </summary>
        public Func<SerializationSchema, string, T> LoadExternalFunc;
        #endregion

        /// <summary>
        /// Returns the currently set schema. 
        /// </summary>
        public SerializationSchema Schema => schema;

        /// <summary>
        /// CTR:
        /// </summary>
        /// <param name="schema"> The serialization schema to use to define serialization. </param>
        public GenericSaveHandler(SerializationSchema schema)
        {
            this.schema = schema;
        }

        /// <summary>
        /// Save provided data to file. 
        /// -- Will use custom serialization if useCustomSerialization is used. 
        /// </summary>
        /// <param name="data"> The data to serialize. </param>
        /// <param name="additionalNameData"> The additional name data for the file. </param>
        /// <param name="opType"> Specifies the operation type (Default, Player) used for the operation. </param>
        public void Save(T data, string additionalNameData, OperationType opType)
        {
            //If custom serialization is flagged, use it. 
            if (useCustomSerialization)
            {
                switch (opType)
                {
                    case OperationType.DEFAULT:
                        //Check if the action is assigned, if not debug, ELSE use provided custom serialization. 
                        if (SaveDefaultAction == null) Debug.Log("GenericSaveHandler: No custom SaveDefaultAction provided, Save Failed.");
                        else SaveDefaultAction?.Invoke(data, this.schema, additionalNameData);
                        break;
                    case OperationType.EXTERNAL:
                        //Check if the action is assigned, if not debug, ELSE use provided custom serialization. 
                        if (SaveDefaultAction == null) Debug.Log("GenericSaveHandler: No custom SaveDefaultAction provided, Save Failed.");
                        else SaveExternalAction?.Invoke(data, this.schema, additionalNameData);
                        break;
                }
                return;
            }
            else
            {
                //Use built-in serialization method. 
                string path = "";
                string filename = "";
                string fullPath = "";

                //Build serialization strings. 
                switch (opType)
                {
                    case OperationType.DEFAULT:
                        path += schema.StreamingAssetsPath;
                        break;
                    case OperationType.EXTERNAL:
                        path += schema.PersistentDataPath;
                        break;
                }

                filename = schema.GetFullFileName(additionalNameData, opType, true);
                fullPath = path + filename;

                //Ensure serialization directory exists. 
                EnsurePathExists(path);

#if DEBUG
                //Debug the built strings if required. 
                Debug.Log("Path: " + fullPath);
#endif

                //Serialize data. 
                if (schema.serializationType == SerializationFormat.JSON) GenericJSONSerializer.Serialize(data, fullPath);
                else GenericXMLSerializer.Serialize(data, fullPath);
            }
        }

        /// <summary>
        /// Load data from file. 
        /// </summary>
        /// <param name="additionalNameData"> The additional name data for the file. </param>
        /// <param name="opType"> Specifies the operation type (Default, Player) used for the operation. </param>
        /// <returns> Deserialized data file if located, or the default(T) if not located. </returns>
        public T Load(string additionalNameData, OperationType opType)
        {
            //If custom serialization is flagged, use it. 
            if (useCustomSerialization)
            {
                switch (opType)
                {
                    case OperationType.DEFAULT:
                        //Check if the action is assigned, if not debug, ELSE use provided custom serialization. 
                        if (LoadDefaultFunc == null)
                        {
                            Debug.Log("GenericSaveHandler: No custom LoadDefaultFunc provided, Load Failed.");
                            return default;
                        }
                        else return LoadDefaultFunc.Invoke(this.schema, additionalNameData);
                    case OperationType.EXTERNAL:
                        //Check if the action is assigned, if not debug, ELSE use provided custom serialization. 
                        if (LoadExternalFunc == null)
                        {
                            Debug.Log("GenericSaveHandler: No custom LoadPlayerFunc provided, Load Failed.");
                            return default;
                        }
                        else return LoadExternalFunc?.Invoke(this.schema, additionalNameData);
                }
            }

            //Use built-in serialization method. 

            string path = "";
            string filename = "";
            string fullPath = "";

            //Build serialization strings. 
            switch (opType)
            {
                case OperationType.DEFAULT:
                    path += schema.StreamingAssetsPath;
                    break;
                case OperationType.EXTERNAL:
                    path += schema.PersistentDataPath;
                    break;
            }

            filename = schema.GetFullFileName(additionalNameData, opType, true);
            fullPath = path + filename;

            //Ensure serialization directory exists. 
            EnsurePathExists(path);

#if DEBUG
            //Debug the built strings if required. 
            Debug.Log("Path: " + fullPath);
#endif

            //Serialize data. 
            if (schema.serializationType == SerializationFormat.JSON) return GenericJSONSerializer.Deserialize<T>(fullPath);
            else return GenericXMLSerializer.Deserialize<T>(fullPath);
        }

        /// <summary>
        /// Create a new file in the player data location using pre-saved default save file. 
        /// </summary>
        /// <param name="additionalNameData"> The additional name data added to the file. </param>
        public void GenerateNewPlayerDefault(string additionalNameData)
        {
            string defaultFileName = schema.GetFullFileName(additionalNameData, OperationType.DEFAULT, true);

            //Make sure the file exists prior to transfer. 
            if (!CheckFileExists(schema.StreamingAssetsPath, defaultFileName))
            {
                Debug.Log("GenericSaveHandler: Requested default file"
                    + schema.StreamingAssetsPath + string.Format(schema.defaultFilename, additionalNameData)
                    + " does not exist");
                return;
            }
            else
            {
                //Generate required path strings. 
                string pathDefault = schema.StreamingAssetsPath + defaultFileName;

#if DEBUG
                //Debug the built strings if debug (stripped before build through preprocessor commands). 
                Debug.Log("Default Path: " + pathDefault);
#endif

                //Load the data. 
                T data;
                if (schema.serializationType == SerializationFormat.JSON) data = GenericJSONSerializer.Deserialize<T>(pathDefault);
                else data = GenericXMLSerializer.Deserialize<T>(pathDefault);


                this.Save(data, additionalNameData, OperationType.EXTERNAL);
            }
        }

        /// <summary>
        /// Ensures that a given data path exists for serialization. 
        /// </summary>
        /// <param name="path"> The directory file path to serialize to. </param>
        public static void EnsurePathExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Check if a given file exists at the provided path. 
        /// </summary>
        /// <param name="path"> The path to the given file. </param>
        /// <param name="filename"> The name of the file verify exists. </param>
        /// <returns> True if the file is located. </returns>
        public static bool CheckFileExists(string path, string filename)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return false;
            }

            if (File.Exists(path + filename)) return true;

            return false;
        }
    }

    /// <summary>
    /// XML Serialization handler for generic data.  
    /// </summary>
    internal static class GenericXMLSerializer
    {
        /// <summary>
        /// Generic XML Serializer.  
        /// </summary>
        /// <typeparam name="T"> The type of file to be serialized. </typeparam>
        /// <param name="type"> The file reference. </param>
        /// <param name="path"> The file path to serialize the data to </param>
        public static void Serialize<T>(T type, string path)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(path, FileMode.Create);
            s.Serialize(stream, type);
            stream.Close();

#if DEBUG
            Debug.Log("File Path: " + path);
#endif
        }

        /// <summary>
        /// Generic XML deserializer. 
        /// </summary>
        /// <typeparam name="T"> The file type to deserialize. </typeparam>
        /// <param name="path"> The file path to deserialize from. </param>
        /// <returns> A deserialized instance of type T. </returns>
        public static T Deserialize<T>(string path)
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(path, FileMode.OpenOrCreate);
            var output = s.Deserialize(stream);
            stream.Close();
            return (T)output;
        }
    }

    /// <summary>
    /// XML Serialization handler for generic data.  
    /// </summary>
    internal static class GenericJSONSerializer
    {
        /// <summary>
        /// Generic JSON Serializer.  
        /// </summary>
        /// <typeparam name="T"> The type of file to be serialized. </typeparam>
        /// <param name="data"> The file reference. </param>
        /// <param name="path"> The file name to serialize the data to. </param>
        public static void Serialize<T>(T data, string path)
        {
            string fileJson = JsonUtility.ToJson(data, true);
            //Debug.Log(fileJson);
            Debug.Log("Serialized Path: " + path);
            File.WriteAllText(path, fileJson);
        }

        /// <summary>
        /// Generic JSON deserializer. 
        /// </summary>
        /// <typeparam name="T"> The file type to deserialize. </typeparam>
        /// <param name="path"> The name of the file to deserialize. </param>
        /// <returns> A deserialized instance of type T. </returns>
        public static T Deserialize<T>(string path)
        {
            string fContents = File.ReadAllText(path);
            Debug.Log(fContents);
            return JsonUtility.FromJson<T>(fContents);
        }
    }
}
#endif