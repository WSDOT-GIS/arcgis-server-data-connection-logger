/// <reference path="C:\Users\jacobsj\Documents\GitHub\arcgis-server-data-connection-logger\ArcGisServerDataConnectionsWebsite\Script/ServerInfo.js" />

/*global ServerInfo*/
(function () {

	var reviver = function (k, v) {
		if (v && v.hasOwnProperty("Directory")) {
			return new ServerInfo(v);
		}
		else {
			return v;
		}
	};

	var loadData = function (e) {
		// Remove the button.
		this.parentElement.removeChild(this);

		var loadedDataSection = document.getElementById("loadeddata");
		loadedDataSection.hidden = false;

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
				loadedDataSection.appendChild(docFrag);
			} else {
				alert("Data load error");
				console.log("Request error. See console for details", { request: this, event: e });
			}


		};
		request.send();
	};

	document.getElementById("loadDataButton").addEventListener("click", loadData);
}());