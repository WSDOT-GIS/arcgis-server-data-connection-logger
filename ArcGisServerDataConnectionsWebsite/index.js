﻿(function () {

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

	function ConnectionInfo(data) {
		this.connectionString = data.ConnectionString || null;
		this.workspaceFactory = data.WorkspaceFactory || null;
	}

	function Connection(data) {
		this.connectionInfo = new ConnectionInfo(data.ConnectionInfo) || null;
		this.dataSet = data.DataSet || null;
		this.layerName = data.LayerName || null;
	}

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

	function MsdInfo(data) {
		this.path = data.Path;
		this.connections = data.Connections.map(function (c) {
			return new Connection(c);
		});
	}

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

	function ServerInfo(data) {
		this.directory = data.Directory;
		this.msdInfos = data.MsdInfos.map(function (m) {
			return new MsdInfo(m);
		});
	}

	var reviver = function (k, v) {
		if (v && v.hasOwnProperty("Directory")) {
			return new ServerInfo(v);
		}
		else {
			return v;
		}
	};

	var request = new XMLHttpRequest();
	request.open("get", "api/connections");
	request.onloadend = function () {
		// Remove the progress element.
		var prog = document.getElementById("dataProgress");
		prog.parentElement.removeChild(prog);

		var docFrag;

		var serverInfos;
		if (this.status === 200) {
			serverInfos = JSON.parse(this.responseText, reviver);
			console.log("serverInfos", serverInfos);

			docFrag = document.createDocumentFragment();

			serverInfos.forEach(function (serverInfo) {
				var section = document.createElement("section");

				var h1 = document.createElement("h1");
				h1.textContent = serverInfo.directory;
				section.appendChild(h1);

				var table, msdInfo, caption;

				for (var i = 0; i < serverInfo.msdInfos.length; i++) {
					msdInfo = serverInfo.msdInfos[i];
					caption = document.createElement("caption");
					caption.textContent = msdInfo.path;
					table = msdInfo.createTable();
					table.appendChild(caption);
					section.appendChild(table);
				}

				docFrag.appendChild(section);
			});
		}

		document.body.appendChild(docFrag);

	};
	request.send();
}());