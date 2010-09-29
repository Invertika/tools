<?php

// Example file for the whiteboard.
// (c) 2004 Michael Schierl.
// Licensed under GNU General Public License

error_reporting(E_ALL);

// config options - you may need to change these...

$mysql_host="localhost";
$mysql_user="whiteboard";
$mysql_password="whiteboard";
$mysql_db="whiteboard";
$mysql_table="wbdata";

// end of config options

// auth functions - you may need to change these as well...

// everyone may read and write all boards from 0 to 9 - one might add
// additional parameters (session ID...) for the script to
// authenticate someone

function may_read($id) {
	return $id >= 0 && $id < 10;
}

function may_write($id) {
	return $id >= 0 && $id < 10;
}

// end of auth functions

if (defined('WBDATA_ONLY_CONFIG')) return;

// set text/plain

header("Content-Type: text/plain");

// build connection

$connection = mysql_connect($mysql_host, $mysql_user, $mysql_password) 
     or die(mysql_error());
$db = mysql_select_db($mysql_db,$connection) 
     or die(mysql_error());

// do the work

if (!isset($_GET['id'])) die("Missing parameter");

$id=intval($_GET['id']);

if (isset($_GET['add'])) {
	// someone wants to add data to the whiteboard
	if (!may_write($id)) die("No permission");
	$add = mysql_real_escape_string($_GET['add'], $connection);
	$sql='INSERT INTO '.$mysql_table.' (board_id, data) 
	           VALUES ('.$id.", '".$add."')";
	if( !($result = mysql_query($sql, $connection)) ) {
		die("Could not add shape: ".mysql_error());
	}
	echo "Added";
} else if (isset($_GET['clearto'])) {
	// someone wants to clear the board and then return to where
	// he came from
	if (!may_write($id)) die("No permission");
	$sql="DELETE FROM ".$mysql_table."
	            WHERE board_id=".$id;
	if( !($result = mysql_query($sql)) ) {
		die("Could not clear whiteboard: ".mysql_error());
	}
	$sql='INSERT INTO '.$mysql_table.' (board_id, data) 
	           VALUES ('.$id.", '')";
	if( !($result = mysql_query($sql)) ) {
		die("Could not clear whiteboard: ".mysql_error());
	}
	header("Location: ".$_GET['clearto']);
} else if (isset($_GET['export'])) {
	// someone wants to export all the data to a file
	if (!may_read($id)) die("No permission");

	$sql= "SELECT data 
	         FROM ".$mysql_table."
	        WHERE board_id=".$id."
	        ORDER BY data_id ASC";
	
	if( !($result = mysql_query($sql)) ) {
		die("Could not query board:".mysql_error());
	}
	header("Content-Type: application/x-whiteboard-data");
	header('Content-Disposition: attachment; filename="whiteboard-'.$id.'.dat"');
	echo "WbAppletWhiteboardData1.0\1";
	while ($row=mysql_fetch_row($result)) {
		echo $row[0]."\1";
	}
	mysql_free_result($result);

} else if (isset($_POST['importto'])) {
	// someone wants to import a file
	if (!may_write($id)) die("No permission");

	if (!isset($_FILES['file']) || $_FILES['file']['error'] != 0) {
		die("Incorrect file specified");
	}

	if($_FILES['file']['size'] > 100000) {
		die("File is too large");
	}
	
	$txt=file_get_contents($_FILES['file']['tmp_name']);
	while(substr($txt, strlen($txt)-1)=="\1") {
		$txt=substr($txt, 0, strlen($txt)-1);
	}
	$entries = explode("\1", $txt);
	if ($entries[0] != 'WbAppletWhiteboardData1.0') {
		die("File format not supported");
	}
	$entries[0] = '';

	$sql="DELETE FROM ".$mysql_table."
	            WHERE board_id=".$id;
	if( !($result = mysql_query($sql)) ) {
		die("Could not clear whiteboard: ".mysql_error());
	}
	foreach($entries as $entry) {
		$sql='INSERT INTO '.$mysql_table.' (board_id, data) 
	                   VALUES ('.$id.", '".$entry."')";
		if( !($result = mysql_query($sql)) ) {
			die("Could not clear whiteboard: ".mysql_error());
		}
	}
	header("Location: ".$_POST['importto']);

} else {
	// someone wants to read the whiteboard
	if (!may_read($id)) die("No permission");

	$after=isset($_GET['after'])?intval($_GET['after']):0;
	
	$sql= "SELECT data_id, data 
	         FROM ".$mysql_table."
	        WHERE board_id=".$id." AND data_id>".$after."
	        ORDER BY data_id ASC";
	
	if( !($result = mysql_query($sql)) ) {
		die("Could not query board:".mysql_error());
	}
	while ($row=mysql_fetch_row($result)) {
		echo "::".$row[0].":".$row[1]."\n";
	}
	mysql_free_result($result);
}

mysql_close($connection);
?>
