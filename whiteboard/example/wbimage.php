<?php

// Example file how to generate images from the whiteboard
// (c) 2004 Michael Schierl.
// Licensed under GNU General Public License

error_reporting(E_ALL);

// import config options

define('WBDATA_ONLY_CONFIG', true);
include("./wbdata.php");

// some more config options - you may need to change these...

$local_path='/var/www/wb';
$java_path='/usr/local/bin/java';
$temp_prefix='/tmp/wb_';

// end of config options

// build connection

$connection = mysql_connect($mysql_host, $mysql_user, $mysql_password) 
     or die(mysql_error());
$db = mysql_select_db($mysql_db,$connection) 
     or die(mysql_error());

// do the work

if (!isset($_GET['id'])) 
     die("Missing parameter. use <b>wbimage.php?id=0</b>");

$id=intval($_GET['id']);

if (!may_read($id)) die("No permission.");

$sql = 'SELECT data 
          FROM '.$mysql_table.'
         WHERE board_id='.$id.'
         ORDER BY data_id ASC';
$filename=$temp_prefix.md5(uniqid(rand())).".png";
		
$handle=popen($java_path.' -Djava.awt.headless=true -cp '.$local_path.'/whiteboard.jar WhiteboardSaver '.$filename, "w") or die("Could not start process");
		
if( !($result = mysql_query($sql)) ) {
	die("Could not query board:".mysql_error());
}
while ($row=mysql_fetch_row($result)) {
	fwrite($handle, $row[0]."\n");
}
fclose($handle);
mysql_close($connection);

// sleep(1);

$handle=fopen($filename, "rb") or die ("Could not open file");
header("Content-Type: image/png");

fpassthru($handle);
fclose($handle);
unlink($filename);
?>