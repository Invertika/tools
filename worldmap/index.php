<html>
<head>
	<?php 
		require 'config.php'; 	
		echo "<title>" . $title . "</title>";
		
		$zoom = $initial_zoom;
		if(!empty($_GET['zoom']) && is_numeric($_GET['zoom'])) $zoom = $_GET['zoom'];
		
		echo "<style type=\"text/css\">";
		echo "	img {";
		echo "		height:". $zoom ."px;";
		echo "		width:". $zoom ."px;";
		echo "		background-color:gray;";
		echo "	}";
		echo "</style>";
	?>
	
	<link rel="stylesheet" href="index.css" type="text/css" />
	
	<script type="text/javascript" src="tooltip.js"></script>
	<script type="text/javascript" src="drag.js"></script>
	<script type="text/javascript" src="sprintf.js"></script>
</head>
<body>
		<?php 
			$controlWidth="100%";
			$controlHeight="100%";
			
			echo "<div style=\"position: relative; border: 1px solid black; width: ".$controlWidth."; height: ".$controlHeight."; overflow: hidden;\">";
        ?>
		
        <div id="mapholder" style="padding: 0; margin: 0; cursor: move; white-space: nowrap; position: relative; width: <?php echo $controlWidth; ?>; height: <?php echo $controlHeight; ?>;">
            <table border="0" cellspacing="0" cellpadding="0" id="map_table">
            </table>
        </div>
    </div>
	
    <br />
	
	<div class="divboxes" id="InfoBox" style="position:absolute; left:15px; top:20px; width: 200px; z-index:+1">
	<b>Invertika Weltkarte</b><br/>
	<br/>
	- Mit der Maus kann die Karte bewegt werden<br/>
	- Doppelklick auf eine Kachel um detaillierte Informationen anzuzeigen
	</div>
	
	<div class="divboxes" id="zoombox" style="position:absolute; left: 230px; top:20px; z-index:+1;">
	Zoom Level: <a href="?zoom=10">10</a> <a href="?zoom=20">20</a> <a href="?zoom=30">30</a> <a href="?zoom=40">40</a> <a href="?zoom=50">50</a> 
	<a href="?zoom=100">100</a> <a href="?zoom=200">200</a> <a href="?zoom=400">400</a> <a href="?zoom=800">800</a>
	</div>
	
    <script type="text/javascript">	
		var x = 0;
		var limX = <?php echo $map_x_max; ?>;
		var y =0;
		var limY = <?php echo $map_y_min; ?>;
		var z = <?php echo $zoom; ?>;
		var rows = document.getElementById('map_table').getElementsByTagName('tr');
		var el = document.getElementById('mapholder');
		var leftEdge = el.parentNode.clientWidth - el.clientWidth;
		var topEdge = el.parentNode.clientHeight - el.clientHeight;
		var dragObj = new dragObject(el, null, new Position(leftEdge, topEdge), new Position(0, 0));
		
		var TileCountXJS=0;
		var TileCountYJS=0;
		
		function GetOuterWorldMapFilename(internalX, internalY, zoomLevel) 
		{
			var vX="";
			if(internalX==0) vX="o";
			else if(internalX<0) vX="n";
			else if(internalX>0) vX="p";

			var vY="";
			if(internalY==0) vY="o";
			else if(internalY<0) vY="n";
			else if(internalY>0) vY="p";

			var ret = sprintf("ow-%s%'04d-%s%'04d-o0000-%d.png", vX, Math.abs(internalX), vY, Math.abs(internalY), zoomLevel);
			return ret;
		}
		
		function GetImgTag(internalX, internalY, zoomLevel)
		{
			var fn=GetOuterWorldMapFilename(internalX, internalY, zoomLevel);
			fn='<?php echo $mappath; ?>' + fn;
			var ret='<img src="' + fn + '" style="margin:0;padding:0;border:0 none;" ondblclick="showLayer(' + internalX + ' , ' + internalY + ')" />';
			return ret;
		}
		
		function init() {		
					TileCountXJS=Math.ceil(el.clientWidth/z);
					TileCountYJS=Math.ceil(el.clientHeight/z);
					
					var MaxTileCountX=<?php echo $map_x_max; ?>-(<?php echo $map_x_min; ?>)+1;
					var MaxTileCountY=<?php echo $map_y_max; ?>-(<?php echo $map_y_min; ?>)+1;
					
					if(TileCountXJS>MaxTileCountX)
					{
						TileCountXJS=MaxTileCountX;
					}
					
					if(TileCountYJS>MaxTileCountY)
					{
						TileCountYJS=MaxTileCountY;
					}

					table = document.getElementById('map_table');
					
					for(yz = 0; yz < TileCountYJS; yz++) 
					{
						t = document.createElement('tr');
						table.appendChild(t);
						
						for(xz = 0; xz < TileCountXJS; xz++)
						{
							var xv=xz+<?php echo $map_x_min; ?>;
							var yv=<?php echo $map_y_max; ?>-yz;
							
							ctd = document.createElement('td');
							ctd.innerHTML = GetImgTag(xv, yv, z);
							
							t.appendChild(ctd);
						
						}
					}       

			x = TileCountXJS+<?php echo $map_x_min; ?>;
			y = <?php echo $map_y_max ?>-TileCountYJS;
		}
	   
	  function reload() {
				if(el.offsetLeft + el.offsetWidth-el.clientWidth  <= 100 && x <= limX) {
						//neue felder nach rechts laden
						
						for(i = 0; i < rows.length; i++) 
						{
								t = document.createElement('td');
								rows[i].appendChild(t);
								ty=<?php echo $map_y_max; ?>-i;
								t.innerHTML = GetImgTag(x, ty, z);
						}
						
						x++;
						el.style.width = (x+<?php echo $map_x_max; ?>)*z+"px";
						leftEdge = el.parentNode.clientWidth - el.clientWidth;
						topEdge = el.parentNode.clientHeight - el.clientHeight;
						dragObj = new dragObject(el, null, new Position(leftEdge, topEdge), new Position(0, 0));
				}
				
				if(el.offsetTop + el.offsetHeight-el.clientHeight  <= 100 && y >= limY) {
						//neue felder nach unten laden
						
						t = document.createElement('tr');
						document.getElementById('map_table').appendChild(t);
								
						for(i = 0; i < x-(<?php echo $map_x_min; ?>); i++) 
						{
							tx=i+(<?php echo $map_x_min; ?>);	
								t.innerHTML += '<td>' + GetImgTag(tx, y, z) + '</td>';
						}
						
						y--;
						el.style.height = (<?php echo $map_y_max; ?>-y)*z+"px";
						rows = document.getElementById('map_table').getElementsByTagName('tr');
						leftEdge = el.parentNode.clientWidth - el.clientWidth;
						topEdge = el.parentNode.clientHeight - el.clientHeight;
						dragObj = new dragObject(el, null, new Position(leftEdge, topEdge), new Position(0, 0));                                
				}
				 
				 window.setTimeout('reload()',150);
		}

		init();
		reload();
    </script>
</body>
</html>
