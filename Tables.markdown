Tables
======

Proposed schema

WorkspaceFactories
------------------

The type of database connection. E.g., SDE, SQL, File GDB.

* *WorkspaceFactoryId* (Integer, PK)
* WorkspaceFactoryName (String) (or should this just be the primary key?)
	* `'FileGDB'`
	* `'SDE'`
	* `'SQL'`

Connections
-----------

Contains the parts of a connection string.

* *ConnectionId* (Integer, PK)
* AuthenticationMode (String)
* Database (String)
* DBClient (String)
* DBConnectionProperties (String)
* Instance (String)
* Server (String)
* ServerInstance (String)
* User (String)
* Version (String)
* WorkspaceFactoryId (Integer, FK)

MapServices
-----------

* MapServiceId (Integer, PK)
* MapServicePath (String)

MapServiceConnections
---------------------
Many-to-many relationship table between *MapServices* and *Connections*.

* MapServiceId (FK)
* ConnectionId (FK)

Datasets
--------

* DatasetId (Integer, PK)
* DatasetName (String)
* Connection
 
MapServiceLayers
----------------

* LayerId (Integer, PK)
* LayerName (String)
* MapServiceId (Integer, FK)
* DatasetId 
   
ArcGisServers
-------------

* *ServerId* (Integer, PK)
* ServerName (String)
* MsdPath (String)

ToolRuns
--------

* *RunId* (Integer, PK)
* RunDateTime (DateTime)

ToolRunMapService
-----------------

* RunId (FK)
* MapServiceId (FK)
