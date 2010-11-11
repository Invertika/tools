<html>
<head>
	<?php 
		require 'config.php'; 	
		echo "<title>" . $title . "</title>";
		
		//TODO
		//MouseWheel
		//Drag & Drop mit Constrain -> oder Hintergrundbild
		//Sauberer Zoom (nicht an andere Stelle springen)
		//Positionierung der Infobox (nicht ganz oben rechts)
		//keine Scrollbalken nach rechts und unten
		//Abhängikeit von den Yahoo APis entfernen (das laden von yahoo apis.com
		//Anzeige von Informationen ob auf der Karte Musik vorhganden ist und welche
		//Code Bereinigung und Refactoring

		//Reagieren auf resizen der Viewarea (Fenster größer etc) -> gelöst
		//Problem mit dem Copy & Paste im Infofenster beheben -> gelöst
	?>
	
	<link rel="stylesheet" type="text/css" href="index.css">
    <script type="text/javascript" src="js/invertika.js"></script>
	<script type="text/javascript" src="js/sprintf.js"></script>
	<script type="text/javascript" src="js/yui-min.js"></script>
</head>
<body>

<div id="map_root" style="position: relative; border: 1px solid black; width: 100%; height: 99%; overflow: hidden;">		
        <div id="map_images" style="padding: 0; margin: 0; cursor: move; white-space: nowrap; position: relative; width: 100%; height: 100%;">
            <table border="0" cellspacing="0" cellpadding="0" id="map_table">
            </table>
        </div>
</div>

<div id="form_container">
   <div id="dragpoint"><h2>O</h2></div>
    <form class="yui3-widget-bd" id="theme_form" action="#" method="get">
        <fieldset>
            <h3>Invertika Weltkarte</h3>
			Mit der Maus kann die Karte bewegt werden. Doppelklick auf eine Kachel um detaillierte Informationen anzuzeigen.<br/>
			<br/>
			<div id="infotext">
			  <b>Information</b><br/>
			  keine Karte ausgewählt
			</div>
			
            <label for="zoom_value">Zoom:</label>
	    <div id="zoomlevel">Aktueller Zoom: 0</div>
            <input type="text" size="3" id="zoom_value">
        </fieldset>
        <input type="submit">
    </form>
</div>

<script>
var x = 0;
var y = 0;

var limX = <?php echo $map_x_max; ?>;
var limY = <?php echo $map_y_min; ?>;

var zoom = 100;

var rows = document.getElementById('map_table').getElementsByTagName('tr');
var el = document.getElementById('map_images');

var leftEdge = el.parentNode.clientWidth - el.clientWidth;
var topEdge = el.parentNode.clientHeight - el.clientHeight;

var TileCountXJS = 0;
var TileCountYJS = 0;

YUI().use("stylesheet", "overlay", "slider", "dd-plugin", "dd-constrain", function (Y) {
    var myStyleSheet = new Y.StyleSheet(),
        overlayContent = Y.one('#form_container'),
        overlay, slider, slider_container, fontSizeInput,

        // Create the Overlay, using the form container as the contentBox.
        // The form is assigned a class yui-widget-bd that will be automatically
        // discovered by Overlay to populate the Overlay's body section.
        // The overlay is positioned in the top right corner, but made draggable
        // using Y.Plugin.Drag, provided by the dd-plugin module.
        overlay = new Y.Overlay({
            srcNode: overlayContent,
            width: '225px',
            align: {
                //points: [Y.WidgetPositionAlign.TR, Y.WidgetPositionAlign.TR]
				points: [30, 30]
            },
            plugins: [Y.Plugin.Drag]
        }).render();	

		overlay.dd.addHandle('h2'); //Nur das H2 Element beachten

    // Slider needs a parent element to have the sam skin class for UI skinning
    overlayContent.addClass('yui3-skin-sam');

    // Progressively enhance the font-size input with a Slider
    fontSizeInput = Y.one('#zoom_value');
    fontSizeInput.set('type', 'hidden');
    fontSizeInput.get('parentNode').insertBefore(
    Y.Node.create('10 <span></span> 800'), fontSizeInput);

    slider_container = fontSizeInput.previous("span");

	//Berechne Initial Zoomstufe
	var initZoom=100;
	
	YUI().use('dom', function (Y) {
        viewWidth=Y.DOM.winWidth();	
		xDimension=Math.abs(<?php echo $map_x_max; ?>) + Math.abs(<?php echo $map_x_min; ?>);
		
		if((xDimension*10)>viewWidth) initZoom=10;
		else if((xDimension*20)>viewWidth) initZoom=20;
		else if((xDimension*30)>viewWidth) initZoom=30;
		else if((xDimension*40)>viewWidth) initZoom=40;
		else if((xDimension*50)>viewWidth) initZoom=50;
		else if((xDimension*100)>viewWidth) initZoom=100;
		else if((xDimension*200)>viewWidth) initZoom=200;
		else if((xDimension*400)>viewWidth) initZoom=400;
		else if((xDimension*800)>viewWidth) initZoom=800;
    })
	
    // Create a Slider to contain font size between 6px and 36px, using the
    // page's current font size as the initial value.
    // Set up an event subscriber during construction to update the replaced
    // input field's value and apply the change to the StyleSheet
    var slider = new Y.Slider({
        length: '125px',
        min: 10,
        max: 800,
        value: initZoom,
        after: {
            valueChange: function (e) {
                if (RoundToNextTileSize(e.newVal) != zoom) {
                    table = document.getElementById('map_table');
                    table.innerHTML = "";
                    zoom = RoundToNextTileSize(e.newVal);
                    document.getElementById('zoomlevel').innerHTML="Aktueller Zoom: " + zoom;
                    init();
                }
            }
        }
    }).render(slider_container);
	
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

    //Make map dragable
    YUI().use('dd-drag', function (Y) {
        var dd = new Y.DD.Drag({
            node: '#map_images'
        });
    });
});

function GetImgTag(internalX, internalY, zoomLevel) {
    var fn = GetOuterWorldMapFilename(internalX, internalY, zoomLevel);
    fn = '<?php echo $mappath; ?>' + fn;
    return '<img src="' + fn + '" name="mapimg" style="margin:0;padding:0;border:0 none;" ondblclick="showLayer(' + internalX + ' , ' + internalY + ')" />';
}

function showLayer(x, y) {
    var url = "info.php";
    url = url + "?x=" + x;
    url = url + "&y=" + y;
	
	// Create new YUI instance, and populate it with the required modules
    YUI().use('io', function(Y) {
		Y.on('io:complete', complete, Y); //Verknüpfe mit complete Event
		Y.io(url );
    });
}

// Define a function to handle the response data.
function complete(id, o, args) {
	var data = o.responseText; // Response data.
	document.getElementById("infotext").innerHTML = data;
};

function init() {
    TileCountXJS = Math.ceil(el.clientWidth / zoom);
    TileCountYJS = Math.ceil(el.clientHeight / zoom);

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
            ctd.innerHTML = GetImgTag(xv, yv, zoom);

            t.appendChild(ctd);
        }
    }

    x = TileCountXJS+<?php echo $map_x_min; ?>;
    y = <?php echo $map_y_max ?>-TileCountYJS;
}

function reload() {
    if (el.offsetLeft + el.offsetWidth - el.clientWidth <= 100 && x <= limX) {
        //neue felder nach rechts laden
        for (i = 0; i < rows.length; i++) {
            t = document.createElement('td');
            rows[i].appendChild(t);
            ty = <?php echo $map_y_max; ?> -i;
            t.innerHTML = GetImgTag(x, ty, zoom);
        }

        x++;
        el.style.width = (x + <?php echo $map_x_max; ?> ) * zoom + "px";
        leftEdge = el.parentNode.clientWidth - el.clientWidth;
        topEdge = el.parentNode.clientHeight - el.clientHeight;
    }

    if (el.offsetTop + el.offsetHeight - el.clientHeight <= 100 && y >= limY) {
        //neue felder nach unten laden
        t = document.createElement('tr');
        document.getElementById('map_table').appendChild(t);

        for (i = 0; i < x - ( <?php echo $map_x_min; ?> ); i++) {
            tx = i + ( <?php echo $map_x_min; ?> );
            t.innerHTML += '<td>' + GetImgTag(tx, y, zoom) + '</td>';
        }

        y--;
        el.style.height = ( <?php echo $map_y_max; ?> -y) * zoom + "px";
        rows = document.getElementById('map_table').getElementsByTagName('tr');
        leftEdge = el.parentNode.clientWidth - el.clientWidth;
        topEdge = el.parentNode.clientHeight - el.clientHeight;                         
    }

    window.setTimeout('reload()', 150);
}

init();
reload();
</script>
</body>
</html>
