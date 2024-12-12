# Documentation: C_GenericGameSave

Hi all, this MD serves to provide documentation for C_GenericGameSave. 

### Feature List
- A generic based serializer so its not reliant on types. 
- A generic based save handler. 
- Allows for cloning default save data to the player data folder. 

### Future possible additions
- Adding version control directly to file naming to allow fallbacks and extra control. 
- Built in JSON serialization. 
- Built in encryption. 

## Setup
- Install package from repo. 
- Import into project. 

## General usage guide
- Generating an instance. 
- Import GenericSaveRuntime for runtime uses or GenericSaveEditor for editor applications.
- Define serialization schema: 

        public SerializationSchema schema = new SerializationSchema()
        {
            defaultFilename = "DefaultTest_{0}.xml",
            externalFileName = "PTest_{0}.xml",
            folderName = "/TestData/"
        };
- Create an instance of GenericSaveHandler: 
  
        GenericSaveHandler<FooData> gsh = new GenericSaveEditor.GenericSaveHandler<FooData>(schema);

- Call Save/Load Functions:

        gsh.Save(data, "_", GenericSaveHandler<TestData>.OperationType.DEFAULT);
        FooData data = gsh.Load("_", GenericSaveHandler<TestData>.OperationType.DEFAULT);


### Note versions are identical between both editor and runtime version currently. 
However, the separation is kept because of possible future divergence between the two. 



## API Documentation

### Important notes - System structure. 
- Default: refers to a default file.
    - The Default file should contain the initial starting data provided to the player. 
    - This file should then be cloned from streamingAssetsPath to the persistentDataPath on new game (within the context of your project).
    - This file should not be actively written to with current player data. 
- Player: refers to the current data file used at runtime. 
    - This file exists for player data modification and updating. 
    - Should be cloned from the Default file. 

## SerializationSchema
Responsible for handling string constructs for serialization (filepath, filename). 

    public string defaultFilename = "DLS_{0}.xml"; Filename used for serialization to streamingAssetsPath. **1**
    public string externalFileName = "PLS_{0}.xml"; Filename used for serialization to the persistant data. path **1**
    public string folderName = "/Data/"; The provided folder name used for serialization for both default and player files. **2**
    public string PersistentDataPath: Returns the player save directory + the provided foldername. 
    public string StreamingAssetsPath: Returns the streamingAssetsDataPath + provided foldername; 
    public string GetFullPath_Default(string additionalNameData) returns the full path for default serialization. 
    public string GetFullPath_Player(string additionalNameData) returns the full path for player serialization. 

    Notes -> 
    **1** The _{0} part of the string allows for the formatting of additional data to the file name (for example level name).
          Removing this will likely cause errors during compilation. 
    **2** The "/"'s are required for file path structure, but will likely be moved into the method later so y'all don't forget to add them. 

## GameSaveHandler
Responsible for handling save logic, as well as serving as an anchor for assigning custom serialization functions. 

### Custom Serialization Setup: 

- Actions

        public void SerializeDefault(T data, SerializationSchema schema, string additionalNameData){
            //Handle serialization logic here. 
        }

Then when assigning the action:
        
        SerializationSchema.SaveDefaultAction = SerializeDefault;

- Func

        public T DeserializeDefault(SerializationSchema schema, string additionalNameData){
            //Handle deserialization logic here. 
        }

Then when assigning the action:
        
        SerializationSchema.LoadDefaultAction = DeserializeDefault;

- Note that for serialization to function correctly the following actions must be set: 
    - SerializationSchema.SaveDefaultAction
    - SerializationSchema.SavePlayerAction
    - SerializationSchema.LoadDefaultFunc
    - SerializationSchema.LoadPlayerFunc

### API Reference: 
            public enum OperationType - Used to define the current operation type being handled (Default, Player).
            public bool useCustomSerialization = false; - flag denoting whether internal data serialization should be used.
            private SerializationSchema schema; - The serialization schema used for the data type. 

            #region Serialization Actions. 
            /// Action (anonymous function) used for receiving calls for custom Default serialization.
            /// Only called if GenericSaveHandler.useCustomSerialization is enabled.
            public Action<T, SerializationSchema, string> SaveDefaultAction;

            /// Action (anonymous function) used for receiving calls for custom Player serialization.  
            /// Only called if GenericSaveHandler.useCustomSerialization is enabled.
            public Action<T, SerializationSchema, string> SavePlayerAction;

            /// Function (anonymous function) used to receive calls for custom Default deserialization.
            /// Only called if GenericSaveHandler.useCustomSerialization is enabled.
            public Func<SerializationSchema, string, T> LoadDefaultFunc;

            /// Function (anonymous function) used to receive calls for custom Player deserialization.
            /// Only called if GenericSaveHandler.useCustomSerialization is enabled.
            public Func<SerializationSchema, string, T> LoadPlayerFunc;

            public SerializationSchema Schema => schema; - Returns the currently set schema.  

            /// <summary>
            /// CTR:
            /// </summary>
            /// <param name="schema"> The serialization schema to use to define serialization. </param>
            public GenericSaveHandler(SerializationSchema schema)

            /// <summary>
            /// Save provided data to file. 
            /// -- Will use custom serialization if useCustomSerialization is used. 
            /// </summary>
            /// <param name="data"> The data to serialize. </param>
            /// <param name="additionalNameData"> The additional name data for the file. </param>
            /// <param name="opType"> Specifies the operation type (Default, Player) used for the operation. </param>
            public void Save(T data, string additionalNameData, OperationType opType)

            /// <summary>
            /// Load data from file. 
            /// </summary>
            /// <param name="additionalNameData"> The additional name data for the file. </param>
            /// <param name="opType"> Specifies the operation type (Default, Player) used for the operation. </param>
            /// <returns> Deserialized data file if located, or the default(T) if not located. </returns>
            public T Load(string additionalNameData, OperationType opType)


            /// <summary>
            /// Create a new file in the player data location using pre-saved default save file. 
            /// </summary>
            /// <param name="additionalNameData"> The additional name data added to the file. </param>
            public void GenerateNewPlayerDefault(string additionalNameData)

            /// <summary>
            /// Ensures that a given data path exists for serialization. 
            /// </summary>
            /// <param name="path"> The directory file path to serialize to. </param>
            public static void EnsurePathExists(string path)

            /// <summary>
            /// Check if a given file exists at the provided path. 
            /// </summary>
            /// <param name="path"> The path to the given file. </param>
            /// <param name="filename"> The name of the file verify exists. </param>
            /// <returns> True if the file is located. </returns>
            public static bool CheckFileExists(string path, string filename)

## GenericSerializationHandler
Inbuilt method of serializing using XML parsing.

            /// <summary>
            /// Generic XML Serializer.  
            /// </summary>
            /// <typeparam name="T"> The type of file to be serialized. </typeparam>
            /// <param name="type"> The file reference. </param>
            /// <param name="fileName"> The file name to serialize the data to. </param>
            public static void Serialize<T>(T type, string path)

            /// <summary>
            /// Generic XML deserializer. 
            /// </summary>
            /// <typeparam name="T"> The file type to deserialize. </typeparam>
            /// <param name="fileName"> The name of the file to deserialize. </param>
            /// <returns> A deserialized instance of type T. </returns>
            public static T Deserialize<T>(string path)

            /// <summary>
            /// Check if the data file exists.
            /// </summary>
            /// <param name="path"> The path to serialize to. </param>
            /// <returns> True if the file exist. </returns>
            public static bool DataExists(string path)