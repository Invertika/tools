<html>
<head>
	<?php 
		require 'config.php'; 	
		echo "<title>" . $title . "</title>";
	?>
	
	<link rel="stylesheet" type="text/css" href="css/index.css">
    <link rel="stylesheet" type="text/css" href="css/theme.css">
    <script type="text/javascript" src="js/invertika.js"></script>
	<script type="text/javascript" src="js/sprintf.js"></script>
	<script type="text/javascript" src="js/yui-min.js"></script>
</head>
<body>

<div align="center">
  <div id="map_container" style="position: relative; border: 1px solid black; width: 99%; height: 10px; overflow: hidden;">
    <div id="map_images">
      <table border="0" cellspacing="0" cellpadding="0" id="map_table">
      </table>
    </div>
  </div>
</div>

<div id="form_container">
    <form class="yui3-widget-bd" id="theme_form" action="#" method="get">
        <fieldset>
            <h3>Invertika Weltkarte</h3>
			Mit der Maus kann die Karte bewegt werden. Doppelklick auf eine Kachel um detaillierte Informationen anzuzeigen.<br />
			
			<label for="heading_color">Information:</label>
			<div id="infotext"></div>
			
            <label for="font_size">Zoom:</label>
            <input type="text" size="3" id="font_size" value="16px">
        </fieldset>
        <input type="submit">
    </form>
</div>

<script>
var x = 0;
var limX = <?php echo $map_x_max; ?> ;
var y = 0;
var limY = <?php echo $map_y_min; ?> ;
var z = 100;
var rows = document.getElementById('map_table').getElementsByTagName('tr');
var el = document.getElementById('map_images');
var leftEdge = el.parentNode.clientWidth - el.clientWidth;
var topEdge = el.parentNode.clientHeight - el.clientHeight;
//var dragObj = new dragObject(el, null, new Position(leftEdge, topEdge), new Position(0, 0));
var TileCountXJS = 0;
var TileCountYJS = 0;

YUI().use("stylesheet", "overlay", "slider", "dd-plugin", function (Y) {

    var myStyleSheet = new Y.StyleSheet(),
        overlayContent = Y.one('#form_container'),
        overlay, slider, slider_container, fontSizeInput,

        //<div id="map_container" style="position: relative; border: 1px solid black; width: 900px; height: 700px; overflow: hidden;">
        // Create the Overlay, using the form container as the contentBox.
        // The form is assigned a class yui-widget-bd that will be automatically
        // discovered by Overlay to populate the Overlay's body section.
        // The overlay is positioned in the top right corner, but made draggable
        // using Y.Plugin.Drag, provided by the dd-plugin module.
        overlay = new Y.Overlay({
            srcNode: overlayContent,
            width: '225px',
            align: {
                points: [Y.WidgetPositionAlign.TR, Y.WidgetPositionAlign.TR]
            },
            plugins: [Y.Plugin.Drag]
        }).render();

    // Slider needs a parent element to have the sam skin class for UI skinning
    overlayContent.addClass('yui3-skin-sam');

    // Progressively enhance the font-size input with a Slider
    fontSizeInput = Y.one('#font_size');
    fontSizeInput.set('type', 'hidden');
    fontSizeInput.get('parentNode').insertBefore(
    Y.Node.create('10 <span></span> 800'), fontSizeInput);

    slider_container = fontSizeInput.previous("span");

    // Create a Slider to contain font size between 6px and 36px, using the
    // page's current font size as the initial value.
    // Set up an event subscriber during construction to update the replaced
    // input field's value and apply the change to the StyleSheet
    slider = new Y.Slider({
        length: '135px',
        min: 10,
        max: 800,
        value: parseInt(Y.one('body').getStyle('fontSize')) || 13,
        after: {
            valueChange: function (e) {
                //var size = e.newVal + 'px';
                if (RoundToNextTileSize(e.newVal) != z) {
                    //Methode 1
                    table = document.getElementById('map_table');
                    table.innerHTML = "";
                    z = RoundToNextTileSize(e.newVal);
                    init();

                    //Methode2
                    //				  elements = document.getElementsByTagName("img");
                    //for (x=0;x<elements.length;x++)
                    //{
                    //if(elements[x].name=="mapimg")
                    //{
                    //z=RoundToNextTileSize(e.newVal);
                    //elements[x].src=ChangeZoomLevelOfMapName(elements[x].src, z);
                    //}
                    //}
                }

                //this.thumb.set('title', size);
                //fontSizeInput.set('value', size);
                //myStyleSheet.set('body', { fontSize: size });
            }
        }
    }).render(slider_container);

    // The color inputs are assigned keyup listeners that will update the
    // StyleSheet if the current input value is a valid CSS color value
    // The heading input affects all h1s, h2, and h3s
    Y.on('keyup', function (e) {
        var color = this.get('value');

        if (isValidColor(color)) {
            myStyleSheet.set('h1, h2, h3', {
                color: color
            });
        }
    }, '#heading_color');

    // The link hover affects the background color of links when they are
    // hovered.  There is no way other than via stylesheet modification to
    // change pseudo-class styles.
    Y.on('keyup', function (e) {
        var color = this.get('value');

        if (isValidColor(color)) {
            myStyleSheet.set('a:hover', {
                backgroundColor: color
            });
        }
    }, '#link_hover');

    // Progressive form enhancement complete, now prevent the form from
    // submitting normally.
    Y.one('#theme_form input[type=submit]').remove();

    Y.on('submit', function (e) {
        e.halt();
    }, '#theme_form');

    //Make dragable
    YUI().use('dd-drag', function (Y) {
        var dd = new Y.DD.Drag({
            node: '#map_images'
        });
    });


});

function GetImgTag(internalX, internalY, zoomLevel) {
    var fn = GetOuterWorldMapFilename(internalX, internalY, zoomLevel);
    fn = '<?php echo $mappath; ?>' + fn;
    var ret = '<img src="' + fn + '" name="mapimg" style="margin:0;padding:0;border:0 none;" ondblclick="showLayer(' + internalX + ' , ' + internalY + ')" />';
    return ret;
}

function GetXmlHttpObject() {
    var xmlHttp = null;
    try {
        xmlHttp = new XMLHttpRequest;
    } catch (e) {
        try {
            xmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
            }
            catch (e) {
                alert("Kein AJAX vorhanden");
            }
        }
    }
    return xmlHttp;
}


function showLayer(x, y) {
    xmlHttp = GetXmlHttpObject();

    if (xmlHttp == null) {
        alert("Browser does not support HTTP Request");
        return;
    }

    var url = "info.php";
    url = url + "?x=" + x;
    url = url + "&y=" + y;

    xmlHttp.onreadystatechange = stateInformations;
    xmlHttp.open("GET", url, true);
    xmlHttp.send(null);

    //alert("showLayer Erfolgreich ausgeführt");
}


function stateInformations() {
    if (xmlHttp.readyState == 4 || xmlHttp.readyState == "complete") {
        //alert("stateInformations Erfolgreich ausgeführt");
        document.getElementById("infotext").innerHTML = xmlHttp.responseText;
    }
}

function init() {
    //document.getElementById("map_container").style.width = window.innerWidth + "px";
    //document.getElementById("map_container").style.height = winHeight + "px";
    YUI().use('dom', function (Y) {
        document.getElementById("map_container").style.height = Y.DOM.winHeight() - 20 + "px";
    })

    TileCountXJS = Math.ceil(el.clientWidth / z);
    TileCountYJS = Math.ceil(el.clientHeight / z);

    var MaxTileCountX = <?php echo $map_x_max; ?> -( <?php echo $map_x_min; ?> ) + 1;
    var MaxTileCountY = <?php echo $map_y_max; ?> -( <?php echo $map_y_min; ?> ) + 1;

    if (TileCountXJS > MaxTileCountX) {
        TileCountXJS = MaxTileCountX;
    }

    if (TileCountYJS > MaxTileCountY) {
        TileCountYJS = MaxTileCountY;
    }

    table = document.getElementById('map_table');

    for (yz = 0; yz < TileCountYJS; yz++) {
        t = document.createElement('tr');
        table.appendChild(t);

        for (xz = 0; xz < TileCountXJS; xz++) {
            var xv = xz + <?php echo $map_x_min; ?> ;
            var yv = <?php echo $map_y_max; ?> -yz;

            ctd = document.createElement('td');
            ctd.innerHTML = GetImgTag(xv, yv, z);

            t.appendChild(ctd);

        }
    }

    x = TileCountXJS + <?php echo $map_x_min; ?> ;
    y = <?php echo $map_y_max ?> -TileCountYJS;
}

function reload() {
    if (el.offsetLeft + el.offsetWidth - el.clientWidth <= 100 && x <= limX) {
        //neue felder nach rechts laden
        for (i = 0; i < rows.length; i++) {
            t = document.createElement('td');
            rows[i].appendChild(t);
            ty = <?php echo $map_y_max; ?> -i;
            t.innerHTML = GetImgTag(x, ty, z);
        }

        x++;
        el.style.width = (x + <?php echo $map_x_max; ?> ) * z + "px";
        leftEdge = el.parentNode.clientWidth - el.clientWidth;
        topEdge = el.parentNode.clientHeight - el.clientHeight;
        //dragObj = new dragObject(el, null, new Position(leftEdge, topEdge), new Position(0, 0));
    }

    if (el.offsetTop + el.offsetHeight - el.clientHeight <= 100 && y >= limY) {
        //neue felder nach unten laden
        t = document.createElement('tr');
        document.getElementById('map_table').appendChild(t);

        for (i = 0; i < x - ( <?php echo $map_x_min; ?> ); i++) {
            tx = i + ( <?php echo $map_x_min; ?> );
            t.innerHTML += '<td>' + GetImgTag(tx, y, z) + '</td>';
        }

        y--;
        el.style.height = ( <?php echo $map_y_max; ?> -y) * z + "px";
        rows = document.getElementById('map_table').getElementsByTagName('tr');
        leftEdge = el.parentNode.clientWidth - el.clientWidth;
        topEdge = el.parentNode.clientHeight - el.clientHeight;
        //dragObj = new dragObject(el, null, new Position(leftEdge, topEdge), new Position(0, 0));                                
    }

    window.setTimeout('reload()', 150);
}

init();
reload();
</script>
</body>
</html>
