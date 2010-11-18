<?php 
  require 'config.php';
  
  // Verbindung zum Datenbank Server herstellen
  $db = @mysql_connect ($host, $user, $password) or die ("Es konnte keine Verbindung zum Datenbankserver hergestellt werden");

  //Datenbank auf UTF8 setzen
  mysql_query("SET NAMES 'UTF8'");
  
  // Datenbank auswählen
  mysql_select_db ($name, $db) or die("Die Datenbank \"$name\" konnte nicht ausgew&auml;hlt werden");
  
  //Abfrage 
  $sql = "SELECT * FROM wmFeatureMaps";
  
  $result = mysql_query($sql);
  
  echo "<b>Kartenmodus</b><br/>";
  echo "- <a href=\"#\" onclick=\"ReInitMap('')\" title=\"Der normale Kartenmodus ohne zusätzliche Informationen.\">Normal</a><br/>";
  
      while ($row = mysql_fetch_array($result)) {
        echo "- <a href=\"#\" onclick=\"ReInitMap('fm-".$row['FeatureNameForFilename']."/')\" title=\"".$row['FeatureDescription']."\">".$row['FeatureName']."</a><br/>";
    }
  
  //Datenbank schließen
  mysql_close($db);
?>