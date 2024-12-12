using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GenericSaveEditor;
using System.IO;

public class SaveUtilTest
{
    public class TestData
    {
        public string name;
        public int index;
        public float value;
        public bool state;

        public bool AreEqual(TestData other)
        {
            return (name == other.name && index == other.index && value == other.value && state == other.state);
        }
    }

    public SerializationSchema schema = new SerializationSchema()
    {
        defaultFilename = "DefaultTest_{0}.xml",
        externalFileName = "PTest_{0}.xml",
        folderName = "/TestData/"
    };

    [Test]
    public void SaveUtilTest_AutoPass()
    {
        // Should always succeed. 
        Assert.IsTrue(true);
    }

    [Test]
    public void SaveUtil_GenerateInstance()
    {
        GenericSaveHandler<TestData> gsh = new GenericSaveEditor.GenericSaveHandler<TestData>(schema);
        Assert.IsNotNull(gsh);
        Assert.AreEqual(schema, gsh.Schema);
    }

    [Test]
    public void SaveUtil_SerializeDefaultData()
    {
        TestData data = new TestData() {
            name = "Test", 
            index = 0,
            value = 0.5F, 
            state = true
        };

        GenericSaveHandler<TestData> gsh = new GenericSaveEditor.GenericSaveHandler<TestData>(schema);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_", GenericSaveHandler<TestData>.OperationType.DEFAULT);
        string path = schema.GetFullPath_Default("_");
        Debug.Log(path);
        Assert.AreEqual("C:/Users/GRINLESS/Tool_LevelSelectionEditor/Assets/StreamingAssets/TestData/DefaultTest__.xml", path);
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void SaveUtil_DeserializeDefaultData()
    {
        TestData data = new TestData()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestData> gsh = new GenericSaveEditor.GenericSaveHandler<TestData>(schema);
        gsh.Save(data, "_", GenericSaveHandler<TestData>.OperationType.DEFAULT);
        TestData dataDeserialized = gsh.Load("_", GenericSaveHandler<TestData>.OperationType.DEFAULT);
        Assert.IsTrue(data.AreEqual(dataDeserialized));
    }

    [Test]
    public void SaveUtil_SerializePlayerData()
    {
        TestData data = new TestData()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestData> gsh = new GenericSaveEditor.GenericSaveHandler<TestData>(schema);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_", GenericSaveHandler<TestData>.OperationType.PLAYER);
        string path = schema.GetFullPath_Player("_");
        Debug.Log(path);
        Assert.AreEqual("C:/Users/GRINLESS/AppData/LocalLow/DefaultCompany/Tool_LevelSelectionEditor/TestData/PTest__.xml", path);
        Assert.IsTrue(File.Exists(path));
    }

    [Test]
    public void SaveUtil_DeserializePlayerData()
    {
        TestData data = new TestData()
        {
            name = "Test",
            index = 0,
            value = 0.5F,
            state = true
        };

        GenericSaveHandler<TestData> gsh = new GenericSaveEditor.GenericSaveHandler<TestData>(schema);
        gsh.Save(data, "_", GenericSaveHandler<TestData>.OperationType.PLAYER);
        TestData dataDeserialized = gsh.Load("_", GenericSaveHandler<TestData>.OperationType.PLAYER);
        Assert.IsTrue(data.AreEqual(dataDeserialized));
    }

    [Test]
    public void SaveUtil_WriteDefaultToPlayer()
    {
        TestData data = new TestData()
        {
            name = "Test_Swap",
            index = 1,
            value = 1.5F,
            state = false
        };

        GenericSaveHandler<TestData> gsh = new GenericSaveEditor.GenericSaveHandler<TestData>(schema);
        Assert.IsNotNull(gsh);
        gsh.Save(data, "_S", GenericSaveHandler<TestData>.OperationType.DEFAULT);
        string pathDefault = schema.GetFullPath_Default("_S");
        Assert.IsTrue(File.Exists(pathDefault));

        gsh.GenerateNewPlayerDefault("_S");
        string pathPlayer = schema.GetFullPath_Player("_S");
        Assert.IsTrue(File.Exists(pathPlayer));

        TestData deserialized = gsh.Load("_S", GenericSaveHandler<TestData>.OperationType.PLAYER);
        Assert.IsTrue(data.AreEqual(deserialized));
    }
}
