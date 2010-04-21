    function GetXmlHttpObject() {
        var xmlHttp = null;
        try {
            xmlHttp = new XMLHttpRequest;
        } catch (e) {
            try {
                xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
            } catch (e) {
				try	{
                    xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
				}
				catch (e)
				{
				    alert("Kein AJAX vorhanden");
				}
            }
        }
        return xmlHttp;
    }
	
	function showInformations(x, y) {
        xmlHttp = GetXmlHttpObject();
		
        if (xmlHttp == null) {
            alert("Browser does not support HTTP Request");
            return;
        }
		
        var url = "mapinfo.php";
        url = url + "?x=" + x;
        url = url + "&y=" + y;

        xmlHttp.onreadystatechange = stateInformations;
        xmlHttp.open("GET", url, true);
        xmlHttp.send(null);
    }
	
	
	function stateInformations() {
        if (xmlHttp.readyState == 4 || xmlHttp.readyState == "complete") {
            document.getElementById("InfoBox").innerHTML = xmlHttp.responseText;
        }
    }

    function showLayer(x, y){
		showInformations(x, y);
    }