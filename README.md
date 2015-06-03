Listing datasets used by ArcGIS Server
======================================

This project provides utilities (written in C#) for creating a list of datasets currently in use by ArcGIS Server.

GetArcGisConnections application
--------------------------------

Pass in directories as parameters to `GetArcGisConnections.exe`, as shown in the example below. Use the `>` operator to redirect the output to a text file.

        GetArcGisConnections "\\myserver1\share$\arcgisserver\directories\arcgissystem\arcgisinput" "\\myserver2\share$\arcgisserver\directories\arcgissystem\arcgisinput" > output.md

The output from this program is in [Markdown] format.


Technical Info
--------------

* Use the [ArcGIS Admin Service] endpoint to get path to `msd` files. The response will contain the following information.

        {
          ...
          "properties": {
            ...
            "filePath": "C:\\arcgisserver\\directories\\arcgissystem\\arcgisinput\\Folder\\ServiceName.MapServer\\extracted\\v101\\ServiceName.msd",
            ...
          }
          ...
        }
    
    The drive letter (`C:` in this case) refers to a drive on the server.

* You can also use the [ArcGIS Server Admin system > directories] endpoint to get the `arcgisinput` folder. E.g, `http://myserver:6080/arcgis/admin/system/directories/arcgisinput`
 
* The `mxd` files used for generating the `msd` files are in the same directory. (The `msd` files should suffice for this purpose, though.)

* The `msd` files are zip files containing XML files. The `layers` folder contains XML files that contain connection information: connection string and dataset name.

### MSD File Structure ###

The `*.msd` file is actually an archive containing XML files. For the needs of this application, only the contents of the `layers` folder will need to be examined.

Each of the XML files in the `layers` directory represents a layer in the map service. Some are group layers and others are data layers. 

For each XML file...

1. Look for the `CIMDEGeographicFeatureLayer/FeatureTable/DataConnection` element. If not present, skip to the next XML file. 
2. Get the contents of the `WorkspaceConnectionString` and `DataSet` elements of the `DataConnection` element. The `DataConnection/WorkspaceFactory` element will indicate the type of dataset (e.g., *SDE*). 


[ArcGIS Admin Service]:http://resources.arcgis.com/en/help/arcgis-rest-api/index.html#/Service/02r300000200000000/
[ArcGIS Server Admin system > directories]:http://resources.arcgis.com/en/help/arcgis-rest-api/index.html#/Server_Directory/02r3000001q2000000/
[Markdown]:http://commonmark.org/