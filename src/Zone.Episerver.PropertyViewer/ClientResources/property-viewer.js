$(function () {
    $("#jstree").jstree({
        "core": {
            "data": {
                'url': "/plugins/propertyviewer/getcontenttree/",
                "dataType": "json",
                "data": function (node) {
                    // jstree requests # for root of tree
                    return { "id": node.id === "#" ? "1" : node.id };
                }
            }
        },
        "root": "1"
    });

    $("#jstree").on("select_node.jstree",
        function (e, data) {
            $("#PageId").val(data.node.id);
            $.ajax("/plugins/propertyviewer/getproperties/?pageId=" + data.node.id).done(
                function (data) {
                    $("#blockPropertyList").html("");
                    $("#results").html("");
                    $("#propertyList").html(data);
                });
        });

    $("#propertyList").on("change", "select",
        function () {
            $.ajax("/plugins/propertyviewer/getpropertyvalues/?pageId="
                + $("#PageId").val() + "&propertyname=" + this.value).done(
                    function (data) {
                        if (data.indexOf("BlockPropertyName") > -1) {
                            $("#blockPropertyList").html(data);
                            $("#results").html("");
                        } else {
                            $("#blockPropertyList").html("");
                            $("#results").html(data);
                        }
                    });
        });

    $("#blockPropertyList").on("change", "select",
        function () {
            $.ajax("/plugins/propertyviewer/getblockpropertyvalues/?pageId=" + $("#PageId").val()
                + "&propertyname=" + $("#propertyList select").val()
                + "&blockpropertyname=" + this.value).done(
                    function (data) {
                        $("#results").html(data);
                    });
        });
});
