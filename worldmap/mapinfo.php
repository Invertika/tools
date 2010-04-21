<?php 
  require 'config.php';
  
	function GetOuterWorldMapFilename($internalX, $internalY) 
	{
		$vX="";
		if($internalX==0) $vX="o";
		else if($internalX<0) $vX="n";
		else if($internalX>0) $vX="p";

		$vY="";
		if($internalY==0) $vY="o";
		else if($internalY<0) $vY="n";
		else if($internalY>0) $vY="p";

		$ret = sprintf("ow-%s%'04d-%s%'04d-o0000", $vX, abs($internalX), $vY, abs($internalY));
		return $ret;
	}

  //Variablen abfragen
  $x = 0;
  if(!empty($_GET['x']) && is_numeric($_GET['x'])) $x = $_GET['x'];
  
  $y = 0;
  if(!empty($_GET['y']) && is_numeric($_GET['y'])) $y = $_GET['y'];
  
  // Verbindung zum Datenbank Server herstellen
  $db = @mysql_connect ($host, $user, $password) or die ("Es konnte keine Verbindung zum Datenbankserver hergestellt werden");

  //Datenbank auf UTF8 setzen
  mysql_query("SET NAMES 'UTF8'");
  
  // Datenbank auswählen
  mysql_select_db ($name, $db) or die("Die Datenbank \"$name\" konnte nicht ausgew&auml;hlt werden");
  
  //Abfrage
  $filename = GetOuterWorldMapFilename($x, $y);
  $wikilink = "http://wiki.invertika.org/" . $filename;
  
  $sql = "SELECT * FROM wmInformation WHERE FileName LIKE '%" . $filename . "%'";
  
  $result = mysql_query($sql);
  
  while ($row = mysql_fetch_array($result)) {
      echo "<b>" . $row['Title'] . "</b><br/><br/>";
      echo "Map ID: " . $row['MapID'] . "<br/>";
	  echo "Dateiname: " . $row['FileName'] . ".tmx<br/><br/>";
	  echo "- <a href=\"" . $wikilink . "\" target=\"_blank\">Wiki</a><br/>";
	  echo "- <a href=\"" . $mappath . $row['FileName'] . "-800.png\">Großansicht</a>";
  }
  
  //Datenbank schließen
  mysql_close($db);
?>