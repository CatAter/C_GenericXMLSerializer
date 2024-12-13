# Documentation: C_GenericGameSave

### What is C_GenericGameSave? 
C_GenericGameSave is a custom save implementation designed with Unity workflows in mind. 
More generally, its designed to provide an API which eases the complexity of having to write multiple serializers for custom tasks. 
Generics enable this with ease, and thus generating data files is no harder than reading a smidge of documentation and then defining a structure not unlike a List.
C_GenericGameSave also handles both default (internal data file creation) and external data file creation (player save data, level states). 
Thus hopefully C_GenericGameSave will help remove the complexity of data setup from prototyping/small games. 

### Feature List
- A generic .Net based serializer. 
- A generic .Net based save handler.
- Offers XML and JSON based serialization. 
- Allows for cloning default save data to the player data folder.
- Supports custom serialization for other types. 

### Future possible additions
- Adding version control directly to file naming to allow fallbacks and extra control. 
- Built in encryption. 
- Custom encryption system overrides. 

## Setup
- Install package from repo. 
- Import into project. 
    - To import the package to your project post direct download:
        - Go to [Project Name]\Packages\
        - Add new folder named - com.cater.cgenericgamesave
        - Drag and drop download contents into the folder. 
        - Congrats should now run as an embedded package within your project. 

### Note versions are identical between both editor and runtime version currently. 
However, the separation is kept because of possible future divergence between the two. 
This is why there is a specified GenericSaveEditor.cs and GenericSaveRuntime.cs and their respective tests. 

## General Usage Guide:
https://github.com/CatAter/C_GenericXMLSerializer/blob/main/Documentation/Doc_GeneralUseGuide.md

## Custom Serialization Guide:
https://github.com/CatAter/C_GenericXMLSerializer/blob/main/Documentation/Doc_CustomSerialization.md

## API Documentation:
https://github.com/CatAter/C_GenericXMLSerializer/blob/main/Documentation/APIReference.md