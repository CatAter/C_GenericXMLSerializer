# Custom Serialization Setup

To set up custom serialization, anonymous functions are required. 
For further reading on anonymous functions in C#: 
- Func: https://learn.microsoft.com/en-us/dotnet/api/system.func-4?view=net-9.0
 - Action: https://learn.microsoft.com/en-us/dotnet/api/system.action-1?view=net-9.0

## General Serialization Flow: 

GenericGameSave implements the following serialization process: 
- -> Data/additional name data is sent to serializer
    - -> Serializer determines formatting style (XML/SON)
    - -> Serializer checks to see if GameSerializerEditor.useCustomSerialization/GameSerializerRuntime.useCustomSerialization is flagged.
        - ->  If flagged, then the system defaults to using anonymous functions to parse that data into the provided anonymous function.
        - -> Else the generic parser for (XML/JSON) is used for serialization. 
- -> Exit serialization process. 

*Note* that there is no fallback in this system currently, and if the anonymous function is not defined the data will not be saved. 

### Setting Custom Serialization Setup: 

Setting up custom serialization isn't too difficult, as the process just takes defining 4 required anonymous functions.
* GameSaveRuntime.SaveDefaultAction
* GameSaveRuntime.SavePlayerAction
* GameSaveRuntime.LoadDefaultFunc
* GameSaveRuntime.LoadPlayerFunc

Or equivalent functions in GameSaveEditor: 
* GameSaveEditor.SaveDefaultAction
* GameSaveEditor.SavePlayerAction
* GameSaveEditor.LoadDefaultFunc
* GameSaveRuntime.LoadPlayerFunc


---

- Actions

        public void SerializeDefault(T data, SerializationSchema schema, string additionalNameData)
        {
            //Handle serialization logic here. 
        }

Then when assigning the action:
        
        GameSaveEditor.SaveDefaultAction = SerializeDefault;
        GameSaveRuntime.SaveDefaultAction = SerializeDefault;
___

- Func

        public T DeserializeDefault(SerializationSchema schema, string additionalNameData)
        {
            //Handle deserialization logic here. 
        }

Then when assigning the action:
        
        GameSaveEditor.SaveDefaultAction = DeserializeDefault;
        GameSaveRuntime.SaveDefaultAction = DeserializeDefault;
___
The final step is to set the flag in GameSaveEditor/GameSaveRuntime to use the custom serialization. 

        GameSaveEditor.useCustomSerialization
        GameSaveRuntime.useCustomSerialization

Congratulations, you are now running your own custom serialization through GenericGameSave.