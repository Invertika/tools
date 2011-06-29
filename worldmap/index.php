<html>
<head>
	<?php 
		require 'config.php'; 	
		echo "<title>" . $title . "</title>";
		
		//TODO
		//Drag & Drop mit Constrain -> oder Hintergrundbild
		//Abhängikeit von den Yahoo APis entfernen (das laden von yahooapis.com -> über http://developer.yahoo.com/yui/3/configurator/
		//Code Bereinigung und Refactoring
	?>
	
	<link rel="stylesheet" type="text/css" href="index.css">
    <script type="text/javascript" src="js/invertika.js"></script>
	<script type="text/javascript" src="js/sprintf.js"></script>
	<script type="text/javascript" src="js/yui-min.js"></script>
</head>
<body>

<div id="map_root" style="position: relative; border: 1px solid black; width: 100%; height: 99%; overflow: hidden; background: #d2d2d2;">		
        <div id="map_images" style="padding: 0; margin: 0; cursor: move; white-space: nowrap; position: relative; width: 100%; height: 100%;">
            <table border="0" cellspacing="0" cellpadding="0" id="map_table">
            </table>
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
			<br/>
			<div id="featuremaps">
			  <b>Kartenmodus</b><br/>
			  -
			</div>
			
            <label for="zoom_value">Zoom:</label>
	    <div id="zoomlevel">Aktueller Zoom: 0</div>
            <input type="text" size="3" id="zoom_value">
        </fieldset>
        <input type="submit">
    </form>
</div>
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

var currentMapPath = '';

YUI().use("stylesheet", "overlay", "slider", "dd-plugin", "node", function (Y) {
    var myStyleSheet = new Y.StyleSheet(),
        overlayContent = Y.one('#form_container'),
        overlay, slider, slider_container, zoomLevelInput,

    // Create the Overlay, using the form container as the contentBox.
    // The form is assigned a class yui-widget-bd that will be automatically
    // discovered by Overlay to populate the Overlay's body section.
    // The overlay is positioned in the top right corner, but made draggable
    // using Y.Plugin.Drag, provided by the dd-plugin module.
    overlay = new Y.Overlay({
        srcNode: overlayContent,
        width: '225px',
		xy: [15,15],
        plugins: [Y.Plugin.Drag]
    }).render();	
		
	overlay.dd.addHandle('h2'); //Nur das H2 Element beachten

    // Slider needs a parent element to have the sam skin class for UI skinning
    overlayContent.addClass('yui3-skin-sam');

    // Progressively enhance the zoom level input with a Slider
    zoomLevelInput = Y.one('#zoom_value');
    zoomLevelInput.set('type', 'hidden');
    zoomLevelInput.get('parentNode').insertBefore(
    Y.Node.create('10 <span></span> 6400'), zoomLevelInput);

    slider_container = zoomLevelInput.previous("span");

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
		else if((xDimension*1600)>viewWidth) initZoom=1600;
		else if((xDimension*3200)>viewWidth) initZoom=3200;
		else if((xDimension*6400)>viewWidth) initZoom=6400;
    })
	
    // Create a Slider to contain zoom level between 10 and 6400
    var slider = new Y.Slider({
        length: '125px',
        min: 10,
        max: 6400,
        value: initZoom,
        after: {
            valueChange: function (e) {
                if (RoundToNextTileSize(e.newVal) != zoom) {
                    table = document.getElementById('map_table');
                    table.innerHTML = "";

					newZoom=RoundToNextTileSize(e.newVal);
					
					posNewX=(Y.one("#map_images").getX()/zoom)*newZoom;
					posNewY=(Y.one("#map_images").getY()/zoom)*newZoom;					
				
					zoom = newZoom;
                    document.getElementById('zoomlevel').innerHTML="Aktueller Zoom: " + zoom;
                    init();
					
					Y.one("#map_images").setX(posNewX);
					Y.one("#map_images").setY(posNewY);
                }
            }

        }
    }).render(slider_container);
	
	document.getElementById('zoomlevel').innerHTML="Aktueller Zoom: " + zoom;

    //Mouse Wheel Events abfangen
    YUI().use('node', 'event', function(Y) {
       Y.on('mousewheel', function(e) {
         if(e.wheelDelta > 0) //Up
         {
           slider.setValue(GetNextHigherZoomlevel(slider.getValue()));
         }
         else //Down
         {
           slider.setValue(GetNextLowerZoomlevel(slider.getValue()));
         }
         e.halt();
         });
    });
	
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

function ReInitMap(modi) {
	currentMapPath=modi;

	YUI().use("node", function (Y) {
      table = document.getElementById('map_table');
      table.innerHTML = "";
				
      posNewX=Y.one("#map_images").getX();
      posNewY=Y.one("#map_images").getY();					
				
      document.getElementById('zoomlevel').innerHTML="Aktueller Zoom: " + zoom;
      init();
					
      Y.one("#map_images").setX(posNewX);
      Y.one("#map_images").setY(posNewY);
	});
}

function GetImgTag(internalX, internalY, zoomLevel) {
    var fn = GetOuterWorldMapFilename(internalX, internalY, zoomLevel);
    fn = '<?php echo $mappath; ?>' + currentMapPath + fn;
    return '<img src="' + fn + '" name="mapimg" style="margin:0;padding:0;border:0 none;" ondblclick="showLayer(' + internalX + ' , ' + internalY + ')" />';
}

function FillMapModi() {
    YUI().use('io', function(Y) {
		Y.on('io:complete', completeFillMapModi, Y); //Verknüpfe mit complete Event
		Y.io("fm.php");
    });
}

// Define a function to handle the response data.
function completeFillMapModi(id, o, args) {
	var data = o.responseText; // Response data.
	document.getElementById("featuremaps").innerHTML = data;
};

function showLayer(x, y) {
    var url = "info.php";
    url = url + "?x=" + x;
    url = url + "&y=" + y;
	
	// Create new YUI instance, and populate it with the required modules
    YUI().use('io', function(Y) {
		Y.on('io:complete', completeShowLayer, Y); //Verknüpfe mit complete Event
		Y.io(url );
    });
}

// Define a function to handle the response data.
function completeShowLayer(id, o, args) {
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

FillMapModi();
init();
reload();
</script>
</body>
</html>
