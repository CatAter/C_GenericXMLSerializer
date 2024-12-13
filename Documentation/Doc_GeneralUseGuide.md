# General usage guide

This guide is intended to explain the general programming workflow for the application. 
The guide will cover: 
- Generating an instance. 
- Import GenericSaveRuntime for runtime uses or GenericSaveEditor for editor applications.
- Defining serialization schema:
- And finally provide some examples of what using everything in conjunction might look like. 

### Defining a SerializationSchema:
Defining a SerializationSchema isn't too complex, the main things to consider are: 
- The default file name - The name used to save the file internally (to StreamingAssetsPath).
- The external file name - The name used to save the file externally (to PersistentAssetsPath).
- The name to use for the folder in which the data is stored externally and internally. 
- The type of serialization to apply to your data. 

XML serialization schema.
        
          public SerializationSchema schemaXML = new SerializationSchema()
          {
            defaultFilename = "DefaultTest",
            externalFilename = "PTest",
            folderName = "TestData",
            serializationType = SerializationFormat.XML
           };
    
JSON serialization schema.
        
          public SerializationSchema schemaJson = new SerializationSchema()
          {
            defaultFilename = "DefaultTest",
            externalFilename = "PTest",
            folderName = "TestData",
            serializationType = SerializationFormat.JSON
          };


### Creating an instance of GenericSave: 
Once you have defined a SerializationSchema creating an instance of GenericSaveEditor/GenericSaveRuntime is pretty easy, in fact its one line. 
  
        GenericSaveHandler<FooData> gsh = new GenericSaveEditor.GenericSaveHandler<FooData>(schema);
Or: 

        GenericSaveHandler<FooData> gsh = new GenericSaveEditor.GenericSaveHandler<FooData>(schema);

Note that the type passed in is constant, in this case FooData. This means that once the GenericSaveHandler is created it will be bound to that type. 
So to serialize a new type a new instance of GenericSaveHandler will be required. 

### Call Save/Load Functions:
After accomplishing the prior steps actually saving/loading is achieved with a single function call. 

        gsh.Save(data, "GivenFileName", OperationType.DEFAULT);
        FooData data = gsh.Load("GivenFileName", OperationType.DEFAULT);

### Everything Put Together. 

Therefore a general application of GameSaveHandler for XML might look like: 

    public class TestDataEditor
    {
        public string name;
        public int index;
        public float value;
        public bool state;
    }

    public SerializationSchema schemaXML = new SerializationSchema()
    {
        defaultFilename = "DefaultTest",
        externalFilename = "PTest",
        folderName = "TestData", 
        serializationType = SerializationFormat.XML
    };

    TestDataEditor data = new TestDataEditor()
    {
        name = "Test_Swap",
        index = 1,
        value = 1.5F,
        state = false
    };

    public void SaveUtil_SerializeDefaultDataXML()
    {
        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaXML);
        gsh.Save(data, "_", OperationType.DEFAULT);
    }

    public void SaveUtil_SerializeExternalDataXML()
    {
        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaXML);
        gsh.Save(data, "_", OperationType.EXTERNAL);
    }

    public void SaveUtil_DeserializeDefaultDataXML()
    {
        TestDataEditor dataDeserialized = gsh.Load("_", OperationType.DEFAULT);
    }

    public void SaveUtil_DeserializeExternalDataXML()
    {
        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaXML);
        TestDataEditor dataDeserialized = gsh.Load("_", OperationType.EXTERNAL);
    }

    public void SaveUtil_WriteDefaultToExternalXML()
    {
        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaXML);
        gsh.GenerateNewPlayerDefault("_S");
    }

A JSON implementation might look like: 

    public class TestDataEditor
    {
        public string name;
        public int index;
        public float value;
        public bool state;
    }

    public SerializationSchema schemaJson = new SerializationSchema()
    {
        defaultFilename = "DefaultTest",
        externalFilename = "PTest",
        folderName = "TestData",
        serializationType = SerializationFormat.JSON
    };

    TestDataEditor data = new TestDataEditor()
    {
        name = "Test_Swap",
        index = 1,
        value = 1.5F,
        state = false
    };

        [Test]
    public void SaveUtil_SerializeDefaultDataJson()
    {
        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaJson);
        gsh.Save(data, "_", OperationType.DEFAULT);
    }

    public void SaveUtil_SerializeExternalDataJson()
    {
        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaJson);
        gsh.Save(data, "_", OperationType.EXTERNAL);
    }

    public void SaveUtil_DeserializeDefaultDataJson()
    {

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaJson);
        gsh.Save(data, "_", OperationType.DEFAULT);
        TestDataEditor dataDeserialized = gsh.Load("_", OperationType.DEFAULT);
        Assert.AreEqual(data, dataDeserialized);
    }

    public void SaveUtil_WriteDefaultToExternalJson()
    {
        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaJson);
        gsh.GenerateNewPlayerDefault("_S");
    }

    public void SaveUtil_DeserializeExternalDataJson()
    {
        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaJson);
        TestDataEditor dataDeserialized = gsh.Load("_", OperationType.EXTERNAL);
    }