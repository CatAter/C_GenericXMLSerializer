# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

- N/A

### Changed

- N/A

### Removed

- N/A

## 14.12.2024 [1.0.1]

### Added

- JSON based serialization. 
- Added runtime unit tests.
- SerializationSchema now contains Enum for representing base serialization format (XML, JSON).
- Better file path and file name construction functions. 

### Changed

- Renamed the main editor file [GenericXMLSerializer.cs -> GenericSaveEditor.cs]
- Renamed the main runtime file [GenericXMLSerializer.cs -> GenericSaveRuntime.cs]
- Added JSON test cases to editor tests. 
- Replaced a naming convention.
	- Player -> External, better reflects actual use case. 
- Added documentation. 
	- API Reference. 
	- General Use Guide. 
	- Custom Serialization Guide. 
	- Documentation. 
- Fixed some naming errata. 

### Removed

- N/A

## 12.12.2024 [1.0.0]

### Added

- v1.0.0 Added XML Generic XML parser.
- v1.0.0 Added Generic Save Handler.
- v1.0.0 Added Actions/Functions for custom handling data serialization.
- v1.0.0 Added SerializationSchema class for handling serialization strings.

### Changed

- N/A

### Removed

- N/A


[unreleased]: https://github.com/olivierlacan/keep-a-changelog/compare/v1.1.1...HEAD
[1.0.0]: https://github.com/olivierlacan/keep-a-changelog/releases/tag/v1.0.0