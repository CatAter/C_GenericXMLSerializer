using System.Collections;
using System.Collections.Generic;
using System.IO;
using GenericSaveRuntime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SaveUtilTest_Runtime
{
    public class TestDataRuntime
    {
        public string name;
        public int index;
        public float value;
        public bool state;

        public override bool Equals(object obj)
        {
            if (obj is TestDataRuntime)
            {
                TestDataRuntime other = (TestDataRuntime)obj;

                return (
                    other.name == name &&
                    other.index == index &&
                    other.value == value &&
                    other.state == state
                    );
            }
            return false;
        }
    }

    public SerializationSchema schemaXML = new SerializationSchema()
    {
        defaultFilename = "DefaultTest",
        externalFilename = "PTest",
        folderName = "TestData",
        serializationType = SerializationFormat.XML
    };

    public SerializationSchema schemaJson = new SerializationSchema()
    {
        defaultFilename = "DefaultTest",
        externalFilename = "PTest",
        folderName = "TestData",
        serializationType = SerializationFormat.JSON
    };

    #region Base Cases.
    [Test]
    public void SaveUtilTest_AutoPass()
    {
        // Should always succeed. 
        Assert.IsTrue(true);
    }

    [Test]
    public void SaveUtil_GenerateInstance()
    {
        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaXML);
        Assert.IsNotNull(gsh);
        Assert.AreEqual(schemaXML, gsh.Schema);
    }
    #endregion

    #region Test Data Cases. 
    [Test]
    public void SaveUtil_TestData()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true,
        };

        TestDataRuntime data2 = new TestDataRuntime()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true,
        };

        Assert.AreEqual(data, data2);
    }
    #endregion

    #region XML.

    [Test]
    public void SaveUtil_SerializeDefaultDataXML()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaXML);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_", OperationType.DEFAULT);
        string path = schemaXML.GetFullPath_Default("_", true);
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void SaveUtil_SerializeExternalDataXML()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaXML);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_", OperationType.EXTERNAL);
        string path = schemaXML.GetFullPath_External("_", true);
        Debug.Log(path);
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void SaveUtil_DeserializeDefaultDataXML()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true,
        };

        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaXML);
        gsh.Save(data, "_", OperationType.DEFAULT);
        TestDataRuntime dataDeserialized = gsh.Load("_", OperationType.DEFAULT);
        Assert.AreEqual(data, dataDeserialized);
    }

    [Test]
    public void SaveUtil_WriteDefaultToExternalXML()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test_Swap",
            index = 1,
            value = 1.5F,
            state = false
        };

        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaXML);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_S", OperationType.DEFAULT);
        string pathDefault = schemaXML.GetFullPath_Default("_S", true);
        Assert.IsTrue(File.Exists(pathDefault));

        gsh.GenerateNewPlayerDefault("_S");
        string pathPlayer = schemaXML.GetFullPath_External("_S", true);
        Assert.IsTrue(File.Exists(pathPlayer));

        TestDataRuntime deserialized = gsh.Load("_S", OperationType.EXTERNAL);
        Assert.AreEqual(data, deserialized);
    }

    [Test]
    public void SaveUtil_DeserializeExternalDataXML()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaXML);
        gsh.Save(data, "_", OperationType.EXTERNAL);
        TestDataRuntime dataDeserialized = gsh.Load("_", OperationType.EXTERNAL);
        Assert.AreEqual(data, dataDeserialized);
    }
    #endregion

    #region JSON. 
    [Test]
    public void SaveUtil_SerializeDefaultDataJson()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true,
        };

        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaJson);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_", OperationType.DEFAULT);
        string path = gsh.Schema.GetFullPath_Default("_", true);
        Debug.Log("Returned Test Path: " + path);
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void SaveUtil_SerializeExternalDataJson()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaJson);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_", OperationType.EXTERNAL);
        string path = schemaXML.GetFullPath_External("_", true);
        Debug.Log(path);
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void SaveUtil_DeserializeDefaultDataJson()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true,
        };

        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaJson);
        gsh.Save(data, "_", OperationType.DEFAULT);
        TestDataRuntime dataDeserialized = gsh.Load("_", OperationType.DEFAULT);
        Assert.AreEqual(data, dataDeserialized);
    }

    [Test]
    public void SaveUtil_WriteDefaultToExternalJson()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test_Swap",
            index = 1,
            value = 1.5F,
            state = false
        };

        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaJson);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_S", OperationType.DEFAULT);
        string pathDefault = schemaXML.GetFullPath_Default("_S", true);
        Assert.IsTrue(File.Exists(pathDefault));

        gsh.GenerateNewPlayerDefault("_S");
        string pathPlayer = schemaXML.GetFullPath_External("_S", true);
        Assert.IsTrue(File.Exists(pathPlayer));

        TestDataRuntime deserialized = gsh.Load("_S", OperationType.EXTERNAL);
        Assert.AreEqual(data, deserialized);
    }

    [Test]
    public void SaveUtil_DeserializeExternalDataJson()
    {
        TestDataRuntime data = new TestDataRuntime()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestDataRuntime> gsh = new GenericSaveRuntime.GenericSaveHandler<TestDataRuntime>(schemaJson);
        gsh.Save(data, "_", OperationType.EXTERNAL);
        TestDataRuntime dataDeserialized = gsh.Load("_", OperationType.EXTERNAL);
        Assert.AreEqual(data, dataDeserialized);
    }
    #endregion
}
