<!-- TODO - Es werden nur bilder auf der x achse nachgeladen... aber auch f�r die y achse sollte dies kein gro�er aufwand werden -->
<html>
<head>
	<?php 
	  require 'config.php'; 	
	  echo "<title>" . $title . "</title>";
    ?>
	
    <script type="text/javascript" src="js/drag.js"></script>
	<script type="text/javascript" src="js/sprintf.js"></script>
	
	<?php 

	$zoom = 100;
	if(!empty($_GET['zoom']) && is_numeric($_GET['zoom'])) $zoom = $_GET['zoom'];
	
	echo "<style type=\"text/css\">";
	echo "	img {";
	echo "		height:". $zoom ."px;";
	echo "		width:". $zoom ."px;";
	echo "		background-color:gray;";
	echo "	}";
	echo "</style>";
	?>
</head>
<body>
		<?php 
			function GetOuterWorldMapFilename($internalX, $internalY, $zoomLevel) 
			{
				$vX="";
				if($internalX==0) $vX="o";
				else if($internalX<0) $vX="n";
				else if($internalX>0) $vX="p";

				$vY="";
				if($internalY==0) $vY="o";
				else if($internalY<0) $vY="n";
				else if($internalY>0) $vY="p";

				$ret = sprintf("ow-%s%'04d-%s%'04d-o0000-%d.png", $vX, abs($internalX), $vY, abs($internalY), $zoomLevel);
				return $ret;
			}
		

			$controlWidth=500;
			$controlHeight=500;
			
			echo "<div style=\"position: relative; border: 1px solid black; width: ".$controlWidth."px; height: ".$controlHeight."px; overflow: hidden;\">";
			
			$zoom = 100;
			if(!empty($_GET['zoom']) && is_numeric($_GET['zoom'])) $zoom = $_GET['zoom'];
			
			$TileCountX = $controlWidth/$zoom;
			$TileCountY = $controlHeight/$zoom;
		
			$limX=$map_x_max-$map_x_min+1; // +1 0 Kachel
			$limY=$map_y_max-$map_y_min+1; // +1 0 Kachel
        ?>
		
        <div id="mapholder" style="padding: 0; margin: 0; cursor: move; white-space: nowrap;
            position: relative; width: <?php echo $controlWidth; ?>; height: <?php echo $controlHeight;
            ?>;">
            <table border="0" cellspacing="0" cellpadding="0" id="map_table">
                <?php
										
				
					for($y = 0; $y < $TileCountY; $y++) {
						echo '<tr>';
						
						for($x = 0; $x < $TileCountX; $x++) 
						{
							$fnImageFile=$mappath . GetOuterWorldMapFilename($x+$map_x_min, $map_y_max-$y, $zoom);
							echo '<td><img src="' . $fnImageFile . '" style="margin:0;padding:0;border:0 none;"/></td>';
						}
						
						echo '</tr>';
					}
				?>
            </table>
        </div>
    </div>
	
    <br />
    Zoom Level:  <a href="?zoom=10">10</a> <a href="?zoom=20">20</a> <a href="?zoom=30">30</a> <a href="?zoom=40">40</a> <a href="?zoom=50">50</a> 
	<a href="?zoom=100">100</a> <a href="?zoom=200">200</a> <a href="?zoom=400">400</a> <a href="?zoom=800">800</a> <a href="?zoom=1600">1600</a> <a href="?zoom=3200">3200</a>
	
    <script type="text/javascript">	
		var x = <?php echo $TileCountX+$map_x_min; ?>;
		var limX = <?php echo $map_x_max; ?>;
		var y = <?php echo $map_y_max-$TileCountY; ?>;
		var limY = <?php echo $map_y_min; ?>;
		var z = <?php echo $zoom; ?>;
		var rows = document.getElementById('map_table').getElementsByTagName('tr');
		var el = document.getElementById('mapholder');
		var leftEdge = el.parentNode.clientWidth - el.clientWidth;
		var topEdge = el.parentNode.clientHeight - el.clientHeight;
		var dragObj = new dragObject(el, null, new Position(leftEdge, topEdge), new Position(0, 0));
	   
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
			var ret='<img src="' + fn + '" style="margin:0;padding:0;border:0 none;"/>';
			return ret;
		}
	   
	  function reload() {
				if(el.offsetLeft + el.offsetWidth-500  <= 100 && x <= limX) {
						//neue felder nach rechts laden
						
						for(i = 0; i < rows.length; i++) {
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
				
				if(el.offsetTop + el.offsetHeight-500  <= 100 && y >= limY) {
						//neue felder nach unten laden
						t = document.createElement('tr');
								document.getElementById('map_table').appendChild(t);
						for(i = 0; i < x-(<?php echo $map_x_min; ?>); i++) {
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

		reload();
    </script>
</body>
</html>
