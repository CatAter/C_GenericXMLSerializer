using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GenericSaveEditor;
using System.IO;
using System.Collections.Generic;

public class SaveUtilTest_Editor
{
    public class TestDataEditor
    {
        public string name;
        public int index;
        public float value;
        public bool state;

        public override bool Equals(object obj)
        {
            if(obj is TestDataEditor)
            {
                TestDataEditor other = (TestDataEditor)obj;

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
        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaXML);
        Assert.IsNotNull(gsh);
        Assert.AreEqual(schemaXML, gsh.Schema);
    }
    #endregion

    #region Test Data Cases. 
    [Test]
    public void SaveUtil_TestData()
    {
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true,
        };

        TestDataEditor data2 = new TestDataEditor()
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
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaXML);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_", OperationType.DEFAULT);
        string path = schemaXML.GetFullPath_Default("_", true);
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void SaveUtil_SerializeExternalDataXML()
    {
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaXML);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_", OperationType.EXTERNAL);
        string path = schemaXML.GetFullPath_External("_", true);
        Debug.Log(path);
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void SaveUtil_DeserializeDefaultDataXML()
    {
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true,
        };

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaXML);
        gsh.Save(data, "_", OperationType.DEFAULT);
        TestDataEditor dataDeserialized = gsh.Load("_", OperationType.DEFAULT);
        Assert.AreEqual(data, dataDeserialized);
    }

    [Test]
    public void SaveUtil_DeserializeExternalDataXML()
    {
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaXML);
        gsh.Save(data, "_", OperationType.EXTERNAL);
        TestDataEditor dataDeserialized = gsh.Load("_", OperationType.EXTERNAL);
        Assert.AreEqual(data, dataDeserialized);
    }

    [Test]
    public void SaveUtil_WriteDefaultToExternalXML()
    {
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test_Swap",
            index = 1,
            value = 1.5F,
            state = false
        };

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaXML);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_S", OperationType.DEFAULT);
        string pathDefault = schemaXML.GetFullPath_Default("_S", true);
        Assert.IsTrue(File.Exists(pathDefault));

        gsh.GenerateNewPlayerDefault("_S");
        string pathPlayer = schemaXML.GetFullPath_External("_S", true);
        Assert.IsTrue(File.Exists(pathPlayer));

        TestDataEditor deserialized = gsh.Load("_S", OperationType.EXTERNAL);
        Assert.AreEqual(data, deserialized);
    }

    #endregion

    #region JSON. 
    [Test]
    public void SaveUtil_SerializeDefaultDataJson()
    {
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true,
        };

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaJson);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_", OperationType.DEFAULT);
        string path = gsh.Schema.GetFullPath_Default("_", true);
        Debug.Log("Returned Test Path: " + path);
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void SaveUtil_SerializeExternalDataJson()
    {
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaJson);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_", OperationType.EXTERNAL);
        string path = schemaXML.GetFullPath_External("_", true);
        Debug.Log(path);
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void SaveUtil_DeserializeDefaultDataJson()
    {
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true,
        };

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaJson);
        gsh.Save(data, "_", OperationType.DEFAULT);
        TestDataEditor dataDeserialized = gsh.Load("_", OperationType.DEFAULT);
        Assert.AreEqual(data, dataDeserialized);
    }

    [Test]
    public void SaveUtil_WriteDefaultToExternalJson()
    {
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test_Swap",
            index = 1,
            value = 1.5F,
            state = false
        };

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaJson);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_S", OperationType.DEFAULT);
        string pathDefault = schemaXML.GetFullPath_Default("_S", true);
        Assert.IsTrue(File.Exists(pathDefault));

        gsh.GenerateNewPlayerDefault("_S");
        string pathPlayer = schemaXML.GetFullPath_External("_S", true);
        Assert.IsTrue(File.Exists(pathPlayer));

        TestDataEditor deserialized = gsh.Load("_S", OperationType.EXTERNAL);
        Assert.AreEqual(data, deserialized);
    }

    [Test]
    public void SaveUtil_DeserializeExternalDataJson()
    {
        TestDataEditor data = new TestDataEditor()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestDataEditor> gsh = new GenericSaveEditor.GenericSaveHandler<TestDataEditor>(schemaJson);
        gsh.Save(data, "_", OperationType.EXTERNAL);
        TestDataEditor dataDeserialized = gsh.Load("_", OperationType.EXTERNAL);
        Assert.AreEqual(data, dataDeserialized);
    }
    #endregion

}
