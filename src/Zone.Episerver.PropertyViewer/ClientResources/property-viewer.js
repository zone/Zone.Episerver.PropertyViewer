$(function() {

	var loader = document.getElementById("loading");
	var results = document.getElementById("results");
	var pageId = document.getElementById("PageId");
	var $jsTree = $("#jsTree");
	var $propertyList = $("#propertyList");
	var $blockPropertyList = $("#blockPropertyList");
    var config = window.propertyViewerConfig;

	$jsTree.jstree({
		"core": {
			"data": {
				"url": config.getContentTreeUrl,
				"dataType": "json",
				"data": function(node) {
					// jstree requests # for root of tree
					return { "pageId": node.id === "#" ? "1" : node.id };
				}
			}
		},
		"root": "1"
	});

	$jsTree.on("select_node.jstree",
		function(e, data) {
			clearAll();
			showLoader(true);
			pageId.value = data.node.id;

			$.ajax(config.getPropertiesUrl + "?pageId=" + data.node.id)
				.done(function(data) {
					$propertyList.html(data);
					hideLoader(false);
				});
		});

	$propertyList.on("change",
		"select",
		function() {
			clearResults();
			clearBlockProperties();
			showLoader(true);
			$.ajax(config.getPropertyValuesUrl + "?pageId=" +
					pageId.value +
					"&propertyname=" +
					this.value)
				.done(function(data) {
					if (isBlock(data)) {
						$blockPropertyList.html(data);
					} else {
						results.innerHTML = data;
					}
					hideLoader(false);
				});
		});

	$blockPropertyList.on("change",
		"select",
		function() {
			clearResults();
			showLoader(true);
			$.ajax(config.getBlockPropertyValuesUrl + "?pageId=" +
					pageId.value +
					"&propertyname=" +
					$propertyList.find("select").val() +
					"&blockpropertyname=" +
					this.value)
				.done(function(data) {
					results.innerHTML = data;
					hideLoader(false);
				});
		});

	function clearAll() {
		clearProperties();
		clearBlockProperties();
		clearResults();
	}

	function clearResults() {
		results.innerHTML = "";
	}

	function clearProperties() {
		$propertyList.html("");
	}

	function clearBlockProperties() {
		$blockPropertyList.html("");
	}

	function showLoader() {
		loader.style.cssText = "display: block;";
	}

	function hideLoader() {
		loader.style.cssText = "display: none;";
	}

    function isBlock(data) {
        return data.indexOf("BlockPropertyName") > -1;
    }
});
