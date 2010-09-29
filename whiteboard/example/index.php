<?php

// Example file for the whiteboard.
// (c) 2004 Michael Schierl.
// Licensed under GNU General Public License

error_reporting(E_ALL);

/*
 * Generates links for 10 whiteboards.
 */
function generateLinks($readonly) {
	for($i=0;$i<10;$i++) {
		echo '<td><a href="index.php?id='.$i.
			'&amp;readonly='.$readonly.'">'.$i.'</a></td>';
	}
}

/*
 * Prepare settings
 */
$id=isset($_GET['id'])?intval($_GET['id']):0;
$readonly=(isset($_GET['readonly']) && $_GET['readonly'] == 1)?"true":"false";
$comment=($readonly=="true")?"This board is read only":('<a href="wbdata.php?id='.$id.'&amp;clearto=index.php%3Fid='.$id.'%26readonly=0">Clear</a>');

// Note that this will not work on all systems. However, I do not know
// any better alternatives...
$url="http://".$_SERVER['HTTP_HOST'].
	preg_replace("~/[^/]*$~", "", $_SERVER['PHP_SELF']).
	"/wbdata.php?id=".$id;

/*
 * Print the site
 */
?>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html><head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<title>whiteboard.invertika.org</title>
</head>
<body>
<table border=1 align="center">
<tr>
<th>&nbsp;</th>
<th colspan=10>Whiteboards available</th>
</tr>
<tr>
<td>Read/Write:</td>
<?generateLinks(0)?>
</tr>
<tr>
<td>Read only:</td>
<?generateLinks(1)?>
</tr>
</table>
<p></p>
<table border=1 align="center">
<tr><th>Whiteboard #<?=$id?> (<?=$comment?>)</th></tr>
<tr><td>
<applet width=800 HEIGHT=590 ARCHIVE="whiteboard.jar" CODE="WhiteboardApplet">
<param name="url" value="<?=$url?>">
<param name="readonly" value="<?=$readonly?>">
</APPLET>
</td></tr>
</table>
<p></p>
</body>
</html>