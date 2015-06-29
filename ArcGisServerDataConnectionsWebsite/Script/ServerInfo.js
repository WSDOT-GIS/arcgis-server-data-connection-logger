/*global define,module*/

/**
 * ServerInfo module
 * @module ServerInfo
 * @see module:ServerInfo
 */

(function (root, factory) {
	if (typeof define === 'function' && define.amd) {
		// AMD. Register as an anonymous module.
		define([], factory);
	} else if (typeof exports === 'object') {
		// Node. Does not work with strict CommonJS, but
		// only CommonJS-like environments that support module.exports,
		// like Node.
		module.exports = factory();
	} else {
		// Browser globals (root is window)
		root.ServerInfo = factory();
	}
}(this, function () {

	/**
	 * Converts an object into a definition list.
	 * @param {Object} dict
	 * @returns {HTMLDListElement}
	 */
	function toDL(dict) {
		var dl = document.createElement("dl");
		var dt, dd;

		for (var k in dict) {
			if (dict.hasOwnProperty(k)) {
				dt = document.createElement("dt");
				dt.textContent = k;
				dd = document.createElement("dd");
				dd.textContent = dict[k];
				dl.appendChild(dt);
				dl.appendChild(dd);
			}
		}

		return dl;
	}

	/**
	 * Creates a new ConnectionInfo.
	 * @param {object} data
	 * @param {?string} [data.ConnectionString]
	 * @param {string} data.WorkspaceFactory
	 * @class
	 */
	function ConnectionInfo(data) {
		/**@member {string}*/
		this.connectionString = data.ConnectionString || null;
		/**@member {string}*/
		this.workspaceFactory = data.WorkspaceFactory || null;
	}

	/**
	 * Creates a new Connection
	 * @class
	 */
	function Connection(data) {
		/**@member {ConnectionInfo}*/
		this.connectionInfo = new ConnectionInfo(data.ConnectionInfo) || null;
		/**@member {string}*/
		this.dataSet = data.DataSet || null;
		/**@member {string}*/
		this.layerName = data.LayerName || null;
	}

	/**
	 * Returns a table row with connection data.
	 * @returns {HTMLTableRowElement}
	 */
	Connection.prototype.toTR = function () {
		var tr = document.createElement("tr");
		var cell = tr.insertCell(-1);
		cell.textContent = this.connectionInfo ? this.connectionInfo.workspaceFactory : null;
		cell = tr.insertCell(-1);
		if (this.connectionInfo) {
			cell.appendChild(toDL(this.connectionInfo.connectionString));
		}
		cell = tr.insertCell(-1);
		cell.textContent = this.dataSet;
		cell = tr.insertCell(-1);
		cell.textContent = this.layerName;
		return tr;
	};

	/**
	 * Creates a new MsdInfo
	 * @param {Object} data
	 * @param {string} data.Path
	 * @param {Object[]} data.Connections
	 * @class
	 */
	function MsdInfo(data) {
		/** @member {string} - Path to MSD file */
		this.path = data.Path;
		/** @member {Connection[]} */
		this.connections = data.Connections ? data.Connections.map(function (c) {
			return new Connection(c);
		}) : null;
	}

	/**
	 * Creates a table.
	 * @returns {HTMLTableElement}
	 */
	MsdInfo.prototype.createTable = function () {
		var table = document.createElement("table");
		var thead = table.createTHead();
		var tbody = table.createTBody();

		function createHeaderRow() {
			var row = document.createElement("tr");
			var th;

			th = document.createElement("th");
			th.textContent = "Workspace Factory";
			row.appendChild(th);

			th = document.createElement("th");
			th.textContent = "Connection String";
			row.appendChild(th);

			th = document.createElement("th");
			th.textContent = "Dataset";
			row.appendChild(th);

			th = document.createElement("th");
			th.textContent = "Layer Name";
			row.appendChild(th);

			return row;
		}

		thead.appendChild(createHeaderRow());

		this.connections.forEach(function (c) {
			var row = c.toTR();
			tbody.appendChild(row);
		});

		return table;
	};

	/**
	 * 
	 * @param {Object} data
	 * @param {string} data.Directory
	 * @param {Object[]} data.MsdInfos
	 * @class
	 */
	function ServerInfo(data) {
		/** @member {string} */
		this.directory = data.Directory;
		/** @member {MsdInfo[]} */
		this.msdInfos = data.MsdInfos.map(function (m) {
			return new MsdInfo(m);
		});
	}

	// Just return a value to define the module export.
	// This example returns an object, but the module
	// can return a function as the exported value.
	return ServerInfo;
}));

