Logging datasets used by ArcGIS Server
======================================


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
 
* The `mxd` files used for generating the `msd` files are in the same directory. (The `msd` files should suffice for this purpose.)

* The `msd` files are zip files containing XML files. The `layers` folder contains XML files that contain connection information: connection string and dataset name.

[ArcGIS Admin Service]:http://resources.arcgis.com/en/help/arcgis-rest-api/index.html#/Service/02r300000200000000/
[ArcGIS Server Admin system > directories]:http://resources.arcgis.com/en/help/arcgis-rest-api/index.html#/Server_Directory/02r3000001q2000000/