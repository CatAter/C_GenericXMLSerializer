# API Reference

## Important Notes - System Structure: 
- Default: refers to a default file.
    - The Default file should contain the initial starting data provided to the player. 
    - This file should then be cloned from streamingAssetsPath to the persistentDataPath on new game (within the context of your project).
    - This file should not be actively written to with current player data. 
- External: refers to the current data file used at runtime. 
    - This file exists and is persistent at the user level. 
    - Should be cloned from the Default file. 

## OperationType
|  Name | Functionality |
| ---|---|
| DEFAULT  |Denotes a default (internal) save operation. |
| EXTERNAL |Denotes an external save operation.         |

---

## SerializationType
Enum representing the type of serialization to use. 

| Name | Functionality|
| ---|---|
| XML  | Denotes that the used serialization should be XML |
| JSON | Denotes that the used serialization should be JSON|

---

## SerializationSchema
Responsible for handling string constructs for serialization (filepath, filename). 

| Access Modifier | Name | Type | Parameters | Functionality | Comments|
| ---|---|---|---|---|---|
| public |XML_EXTENSION| const string |---|The default extension for XML files.|---|
| public |JSON_EXTENSION| const string |---|The default extension for JSON files.|---|
| public |serializationFormat| SerializationFormat |---|Enum representation of the serialization format to use (Defaults to XML).|---|
| public |defaultFilename| string |---| The filename used for naming default data files.| Examples: "internalData", "TestFile".  Can be anything as long as its alphanumeric. |
| public |externalFilename| string |---| The filename used for naming external data files.| Examples: "externalData", "externalTestFile". Can be anything as long as its alphanumeric. |
| public |folderName| string |---| String used to name the directory for IO. | Examples: "LevelData", "PlayerData". Can be anything as long as its alphanumeric. |
| public |PersistentDataPath|Read only Property, string|---|The folder path for serialized internal data.|---|
| public |StreamingAssetsPath|Read only Property, string|---|The folder path for serialized external data.|---|
| public |GetFullPath_Default(string additionalNameData, bool addExtension)|Function, return type: string|string additionalNameData - additional name data provided for file naming, bool addExtension - whether or not to add the serialization file extension. |Returns the full path used for default data file serialization/deserialization.|---|
| public |GetFullPath_External(string additionalNameData, bool addExtension)|Function, return type: string|string additionalNameData - additional name data provided for file naming, bool addExtension - whether or not to add the serialization file extension. |Returns the full path used for external data file serialization/deserialization.|---|
| public |GetFullFileName(string additionalNameData, bool addExtension)|Function, return type: string|string additionalNameData - additional name data provided for file naming, OperationType opType - The operation type to perform, bool addExtension - flags whether to add the serialization file extension. |Returns the file name used for serialization/deserialization.|---|

---

## GameSaveEditor<T>
Class providing Editor save file utilities, utilized prior to parsing data internally. 
Provided types are constrained to being classes. 
NOTE: This is the editor version, and will not be compiled, meaning it is not useful for runtime data parsing. 

| Access Modifier | Name | Type | Parameters | Functionality | Comments|
|---|---|---|---|---|---|
| Public | useCustomSerialization | bool | -- | Defines whether the system should use custom serialization functions. |
| Private | schema | SerializationSchema | -- |The serialization schema used for the data type.||
| Private | SaveDefaultAction | Action<T, SerializationSchema, string> | -- |Action (anonymous function) used for receiving calls for custom Default serialization.|Only called if GenericSaveHandler.useCustomSerialization is enabled.|
| Private | SaveExternalAction | Action<T, SerializationSchema, string> | -- |Action (anonymous function) used for receiving calls for custom External serialization.|Only called if GenericSaveHandler.useCustomSerialization is enabled.|
| Private | LoadDefaultFunc | Func<SerializationSchema, string, T> | -- |Func (anonymous function) used to receive calls for custom Default deserialization.|Only called if GenericSaveHandler.useCustomSerialization is enabled.|
| Private | LoadExternalFunc | Func<SerializationSchema, string, T> | -- |Func (anonymous function) used to receive calls for custom External deserialization.|Only called if GenericSaveHandler.useCustomSerialization is enabled.|
| Public  | Schema | Read only SerializationSchema |---| Returns the currently set schema. |---|
| Public | GenericSaveHandler(SerializationSchema schema) | Constructor | schema - The schema to use for parsing the data file. |---|---|
| Public |void Save(T data, string additionalNameData, OperationType opType)|---|data - The data class to serialize, additionalNameData - Additional data to add to the files name, opType - the type of operation to execute| Saves file according to the given schema.| This process will be partially replaced when using custom serialization. |
| Public |T Load(string additionalNameData, OperationType opType)|---|additionalNameData - Additional data added to the files name, opType - the type of operation to execute| Loads data according to the given schema.| This process will be partially replaced when using custom serialization. |
| Public |void GenerateNewPlayerDefault(string additionalNameData)|---|additionalNameData - Additional data added to the files name. |---|This process will be partially replaced when using custom serialization.|
| Public Static |void EnsurePathExists(string path)|path - The directory file path to serialize to.|---| Ensures that a given data path exists for serialization. |---|
| Public Static |bool CheckFileExists(string path, string filename)| path - The directory file path to serialize to, filename - The name of the file verify. |---| Check if a given file exists at the provided path, returns true if file exists. |---|

---

## GameSaveRuntime<T>
Class providing runtime save file utilities, utilized prior to parsing data internally. 
Provided types are constrained to being classes. 

| Access Modifier | Name | Type | Parameters | Functionality | Comments|
|---|---|---|---|---|---|
| Public | useCustomSerialization | bool | -- | Defines whether the system should use custom serialization functions. |
| Private | schema | SerializationSchema | -- |The serialization schema used for the data type.||
| Private | SaveDefaultAction | Action<T, SerializationSchema, string> | -- |Action (anonymous function) used for receiving calls for custom Default serialization.|Only called if GenericSaveHandler.useCustomSerialization is enabled.|
| Private | SaveExternalAction | Action<T, SerializationSchema, string> | -- |Action (anonymous function) used for receiving calls for custom External serialization.|Only called if GenericSaveHandler.useCustomSerialization is enabled.|
| Private | LoadDefaultFunc | Func<SerializationSchema, string, T> | -- |Func (anonymous function) used to receive calls for custom Default deserialization.|Only called if GenericSaveHandler.useCustomSerialization is enabled.|
| Private | LoadExternalFunc | Func<SerializationSchema, string, T> | -- |Func (anonymous function) used to receive calls for custom External deserialization.|Only called if GenericSaveHandler.useCustomSerialization is enabled.|
| Public  | Schema | Read only SerializationSchema |---| Returns the currently set schema. |---|
| Public | GenericSaveHandler(SerializationSchema schema) | Constructor | schema - The schema to use for parsing the data file. |---|---|
| Public |void Save(T data, string additionalNameData, OperationType opType)|---|data - The data class to serialize, additionalNameData - Additional data to add to the files name, opType - the type of operation to execute| Saves file according to the given schema.| This process will be partially replaced when using custom serialization. |
| Public |T Load(string additionalNameData, OperationType opType)|---|additionalNameData - Additional data added to the files name, opType - the type of operation to execute| Loads data according to the given schema.| This process will be partially replaced when using custom serialization. |
| Public |void GenerateNewPlayerDefault(string additionalNameData)|---|additionalNameData - Additional data added to the files name. |---|This process will be partially replaced when using custom serialization.|
| Public Static |void EnsurePathExists(string path)|path - The directory file path to serialize to.|---| Ensures that a given data path exists for serialization. |---|
| Public Static |bool CheckFileExists(string path, string filename)| path - The directory file path to serialize to, filename - The name of the file verify. |---| Check if a given file exists at the provided path, returns true if file exists. |---|

---

## GenericXMLSerializer
Internal static class providing XML save file utilities. 

| Access Modifier | Name | Type | Parameters | Functionality | Comments|
|---|---|---|---|---|---|
| public static|Serialize<T>(T type, string path)| void |T - The type of file to be serialized, type - The object reference, fileName - The file path to serialize the data to.|Generic XML serializer, parses data from type to XML prior to file parsing. |---|
| public static|Deserialize<T>(string path) | Returns T. | T - The type of file to deserialize, path - The file path to deserialize from. |Generic XML deserializer, parses data from type to XML prior to file parsing.|
| public static|Deserialize<T>(string path) | Returns T. | T - The type of file to deserialize, path - The file path to deserialize from. |Generic XML deserializer, parses data from type to XML prior to file parsing.|

---

## GenericXMLSerializer
Class providing JSON save file utilities.

| Access Modifier | Name | Type | Parameters | Functionality | Comments|
|---|---|---|---|---|---|
|public static |Serialize<T>(T data, string path)|---|T - The type of file to be serialized, data - The object reference, path - The path to serialize the data to. |Generic JSON Serializer.|---|
|public static| T Deserialize<T>(string path)|Function, Returns T|T - The file type to deserialize, fileName - The path of the file to deserialize|Generic JSON deserializer.|---|

---
