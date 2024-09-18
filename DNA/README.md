# DNA
## Web Application Template for ASP.NET Core

## Introduction
Dot Net Admin or DNA for short is a Web application Template for ASP.NET Core consisting of the parent website and three subordinate projects, though a more robust solution could have more than a hundred, these four projects represent a complete, basic solution. With the exception of the Utilities Class Library, each module is defined as an Assembly Part and provides its own Models, Views, and Controllers within a separate Area defined for the module. Assembly Parts are Class Libraries and cannot stand-alone, instead they must be included as part of a larger, parent application that provides the environment in which they operate.

### DNA (Parent Website)
The Parent Website defines the program configuration and settings for the entire solution. Each module is loaded as a separate Assembly Part and is contained within its own Area. In this fashion, many different applications can make use of the same modules while maintaining a single source code tree. Furthermore, since each module is defined as an Assembly Part in its own Area, it can have its own Home Controller and associated Views. Only two Routes are required to support the Parent Website and an unlimited number of modules.

### DNA3 (Dashboard Assembly Part)
The Dashboard Assembly Part provides Client, User, and User Identity Management, Authorization and Authentication Services for four different Schemes, Content Management Services, and a single Migration file to create all the tables for the various modules. Due to limitations in Migration Scripting capability, tables are created for every known module but are configured only when the system detects that its associated Assembly Part is installed. Based upon its presence or absence, each Assembly Part will be added or removed from the dashboard menu automatically.

### SMO (SQL Server Management Objects)
The SQL Server Management Objects module provides Upload, Download, Backup, Restore, and Database Scripting capabilities to facilitate migration of data from one version of SQL Server to another. Automated backups can be scheduled and completed backups can be downloaded to authorized user desktop systems on demand.

### Utilities Class Library
As its name suggests, the Utilities Class Library provides functionality common to all modules ranging from simple string manipulation to functionality specific to ADO.NET and Entity Framework. Classes intended for system wide use should be included in this project.

## Installation
Configuration settings are broken down into two parts: Common settings contained within the source code tree and application secrets defined in a separate file in a location of your choosing. There are only two secrets required for basic operation: A Database Connection String, and a random string for JWT encryption.

## Customization
The parent website and dashboard modules can be themed separately. Themes consist of a Layout file, associated views, and the CSS and Javascript required for your template. Just about any standard template can be used with the only caveat being that your must break down pages into MVC compatible format. To function with the Content Management Features, each view will require a trivial amount of code to be inserted.

## Known Issues
- Documentation is not yet complete