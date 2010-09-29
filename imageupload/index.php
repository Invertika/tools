<?php

error_reporting( 0 );
session_start();
ob_start();

// Passwort fuer den Upload und die Uebersicht
$passwort = "123456";
// Adresse zum Bilderordner MIT dem abschliessenden Slash
$ImageURL = "http://image.invertika.org/";
$ImageURLRelative = "data/";
// Automatisches Login mit Cookie (0 = ohne Cookie | 1 = Cookie benutzen)
// HINWEIS: sollte nicht verwendet werden, wenn fremde Personen Zugang zum PC haben!!
$CookieLogin = 0;

/* ************************************************************************** */
/* *****     A B   H I E R   N I C H T S   M E H R   A E N D E R N     ****** */
/* ************************************************************************** */


// ---------------------------------------------------
// ----------          G L O B A L          ----------
// ---------------------------------------------------
// Startseite
if (!isset( $_GET['seite'] )) $_GET['seite'] = 'upload';

// Variablen initialisieren
$DateiListe = $DateiName = "";

// Erlaubte Dateiendungen und Sonderzeichen-Array
$Dateiendung_Whitelist = array( "jpg", "jpeg", "gif", "png" );
$ersetzen = array( 'ä' => 'ae', 'ö' => 'oe', 'ü' => 'ue', 'ß' => 'ss', ' ' => '_', '\\' => '-', '/' => '-' );

// Cookie pruefen
if ($CookieLogin == 1)
{
	if (!isset( $_COOKIE['ImageUpload'] ))
	{
		if (!isset( $_SESSION['approved'] ) &&
			$_POST['uebersichtpwd'] == $passwort)
		{
			$_SESSION['approved'] = 1;
			setcookie( "ImageUpload", "approved", time()+60*60*24*30 );
			$_SESSION['passwort_ok'] = true;
			header( 'location:' .$_SERVER['PHP_SELF']. '?seite=uebersicht' );
		}
	}
	else if (isset( $_COOKIE['ImageUpload'] ))
	{
		$_SESSION['approved'] = 1;
		$_SESSION['passwort_ok'] = true;
	}
}

// ---------------------------------------------------
// ----------          U P L O A D          ----------
// ---------------------------------------------------

/**
 * Funktion zum erstellen der Thumbnails
 *
 * @param string $OriginalBildName Bildname vom Original
 * @param integer $ThumbMaxBreite Maximale Breite des Thumbnail
 * @param integer $ThumbMaxHoehe Maximale Hoehe des Thumbnail
 * @param integer $JPGQualitaet JPEG Qualitaet
 * @param boolean $ThumbInfo Bildinformationen auf dem Thumbnail anziegen
 */
function makeThumbWithStamp( $OriginalBildName, $ThumbMaxBreite, $ThumbMaxHoehe, $JPGQualitaet, $ThumbInfo )
{
	$OriginalBildInfo         = getimagesize( $OriginalBildName );
	$OriginalBildInfo['size'] = ceil( filesize( $OriginalBildName ) / 1024 );
	
	// Dateiname ohne Endung
	$DateiNameTemp = explode( ".", strtolower( $OriginalBildName ) );
	array_pop( $DateiNameTemp );
	$DateiNameOhneEndung = implode( "_", $DateiNameTemp );

	// Bilddaten auslesen
	$OriginalBild       = imagecreatefromjpeg( $OriginalBildName );
	$OriginalBildBreite = imagesx( $OriginalBild );
	$OriginalBildHoehe  = imagesy( $OriginalBild );

	// Höhe und Breite berechnen
	if ($OriginalBildBreite > $ThumbMaxBreite || $OriginalBildHoehe > $ThumbMaxHoehe)
	{
		$ThumbnailBreite = $ThumbMaxBreite;
		$ThumbnailHoehe  = $ThumbMaxHoehe;
		if ($ThumbnailBreite / $OriginalBildBreite * $OriginalBildHoehe > $ThumbnailHoehe)
		{
			$ThumbnailBreite = round( $ThumbnailHoehe * $OriginalBildBreite / $OriginalBildHoehe );
		}
		else
		{
			$ThumbnailHoehe = round( $ThumbnailBreite * $OriginalBildHoehe / $OriginalBildBreite );
		}
	}
	else
	{
		$ThumbnailBreite = $OriginalBildBreite;
		$ThumbnailHoehe  = $OriginalBildHoehe;
	}

	// Info auf Thumbnail anzeigen
	if ($ThumbInfo)
	{
		// Schriftgroesse festlegen
		switch (true)
		{
			case ($ThumbnailBreite > 69 && $ThumbnailBreite < 89):
				$SchriftGroesse = 1;
				$Rahmen         = 16;
				$Padding        = 4;
			break;
			
			case ($ThumbnailBreite > 89):
				$SchriftGroesse = 2;
				$Rahmen         = 18;
				$Padding        = 2;
			break;
		}
	}
	// Thumbnail erstellen
	$Thumbnail = imagecreatetruecolor( $ThumbnailBreite, $ThumbnailHoehe+$Rahmen );
	imagecopyresampled( $Thumbnail, $OriginalBild, 0, 0, 0, 0, $ThumbnailBreite, $ThumbnailHoehe, $OriginalBildBreite, $OriginalBildHoehe );
	$TextFarbe = imagecolorallocate( $Thumbnail, 255, 255, 255 );
	$BildInfoString = $OriginalBildInfo[0]. "x" .$OriginalBildInfo[1]. " " .$OriginalBildInfo['size']. "kb";
	imagestring( $Thumbnail, $SchriftGroesse, ($ThumbnailBreite / 2 - strlen( $BildInfoString ) * imagefontwidth( $SchriftGroesse ) / 2), $ThumbnailHoehe+$Padding, $BildInfoString, $TextFarbe );
	// Thumbnail speichern
	imagejpeg( $Thumbnail, $DateiNameOhneEndung. "_t.jpg", $JPGQualitaet );
	@chmod( $DateiNameOhneEndung. "_t.jpg", 0755 );
	imagedestroy( $Thumbnail );
}

/**
 * Abschnitt zur Verarbeitung der hochgeladenen Image Datei
 */
if (isset( $_POST['submit'] ) &&
	$_GET['seite'] == 'upload')
{
	if ($_POST['uploadpasswort'] == $passwort)
	{
		if ($_FILES['upload']['size'] > 0 &&
			substr( strtolower( $_FILES['upload']['name'] ), -5 ) == '.jpeg' ||
			substr( strtolower( $_FILES['upload']['name'] ), -4 ) == '.jpg' ||
			substr( strtolower( $_FILES['upload']['name'] ), -4 ) == '.gif' ||
			substr( strtolower( $_FILES['upload']['name'] ), -4 ) == '.png' )
		{
			$umaskold = umask( 0 );
			$DateiName = $ImageURLRelative.strtr( strtolower( $_FILES['upload']['name'] ), $ersetzen );
			
			// Falls Datei bereits existiert
			if (file_exists( $DateiName ))
			{
				// Dateiname ohne Endung
				$DateiNameTemp = explode( ".", strtolower( $DateiName ) );
				$DateiEndungTmp = array_pop( $DateiNameTemp );
				$DateiNameOhneEndung = implode( "_", $DateiNameTemp );
				$NameZusatz = "_" .substr( md5( time() ), 0, 3 );
				$DateiName = $DateiNameOhneEndung.$NameZusatz. "." .$DateiEndungTmp;
			}			
			if (!file_exists( $DateiName ))
			{
				// Datei kopieren
				if (@move_uploaded_file( $_FILES['upload']['tmp_name'], $DateiName ))
				{
					$BildUpload = true;
					// Berechtigung setzen fuer weitere Uploads und die Uebersicht
					$_SESSION['passwort_ok'] = true;
					$_SESSION['approved'] = 1;
					// Thumbnail erstellen
					if (isset( $_POST['makethumb'] ) &&
						substr( strtolower( $DateiName ), -4 ) == '.jpg' )
					{
						switch (true)
						{
							case (isset( $_POST['thumbinfo'] )):
								makeThumbWithStamp( $DateiName, (int) $_POST['thumbsize'], (int) $_POST['thumbsize'], 80, true );					
							break;
							
							case (!isset( $_POST['thumbinfo'] )):
								makeThumbWithStamp( $DateiName, (int) $_POST['thumbsize'], (int) $_POST['thumbsize'], 80, false );
							break;
						}
						$ThumbNameTmp = substr( $DateiName, 0, -4 );
						$ThumbName = $ThumbNameTmp. "_t.jpg";
					}
					// Link Ausgabe
					if (substr( strtolower( $DateiName ), -4 ) == '.jpg' )
					{
						$ThumbNameTmp = substr( $DateiName, 0, -4 );
						$Vorschau = $ThumbNameTmp. "_t.jpg";
					}
					$UploadOutput  = '<div id="output" class="clearfix">';
					$UploadOutput .= file_exists( $Vorschau ) ? '<div class="thumbimg"><img src="' .$Vorschau. '" /></div>' : '<div class="thumbimg">&nbsp;</div>';
					$UploadOutput .= '<ul>';
					$UploadOutput .= '<li><strong>HTML Link:</strong> <input type="text" id="1" onclick="hl(1);" value="' .htmlspecialchars( "<a href=\"{$ImageURL}{$DateiName}\">Image Datei</a>" ). '" class="hl" /></li>';
					$UploadOutput .= '<li><strong>BBCode Link:</strong> <input type="text" id="2" onclick="hl(2);" value="' .htmlspecialchars( "[URL={$ImageURL}{$DateiName}]Image Datei[/URL]" ). '" class="bl" /></li>';
					$UploadOutput .= '<li><strong>BBCode Bild:</strong> <input type="text" id="3" onclick="hl(3);" value="' .htmlspecialchars( "[IMG]{$ImageURL}{$DateiName}[/IMG]" ). '" class="bb" /></li>';
					if (file_exists( $Vorschau ))
					{
						$UploadOutput .= '<li><strong>BBCode Thumb: </strong><input type="text" id="4" onclick="hl(4);" value="' .htmlspecialchars( "[URL={$ImageURL}{$DateiName}][IMG]{$ImageURL}{$ThumbName}[/IMG][/URL]" ). '" class="bt" /></li>';
					}
					$UploadOutput .=  '</ul>';
					$UploadOutput .=  '</div>';
					@chmod( $DateiName, 0755 );
				}
				else
				{
					$BildUploadFehler = true;
				}
			}
			@umask( $umaskold );
		}
	}
	else if ($_POST['uploadpasswort'] != $passwort &&
			 $_POST['uploadpasswort'] != "")
	{
		$PWFehler = true;
	}
	else
	{
		$PrivatScript = true;
	}
}


// ---------------------------------------------------
// ----------      U E B E R S I C H T      ----------
// ---------------------------------------------------
// Login pruefen
if (!isset( $_SESSION['approved'] ) &&
	$_POST['uebersichtpwd'] == $passwort)
{
	$_SESSION['approved'] = 1;
	$_SESSION['passwort_ok'] = true;
	header( 'location:' .$_SERVER['PHP_SELF']. '?seite=uebersicht&liste=1' );
}
// Funktion zum loeschen von Grafiken
if ($_SESSION['approved'] == 1 &&
	$_GET['liste'] == 1 &&
	$_GET['aktion'] == 'loeschen' &&
	$_GET['datei'] != "")
{
	/**
	 * Loescht eine Image Datei
	 *
	 * @param string $Datei Der zu loeschende Dateiname
	 * @global array $Dateiendung_Whitelist Liste der erlaubten Dateiendungen
	 */
	function loescheDatei( $Datei )
	{
		global $Dateiendung_Whitelist;
		// Auf Verzeichniswechsel im Dateiname pruefen
		if (preg_match( "|/|", $Datei )) die( 'Operation kann nicht ausfuehrt werden!' );
		// Dateiendung pruefen
		$DateiEndung = array_pop( explode( ".", $Datei ) );
		if (!in_array( $DateiEndung, $Dateiendung_Whitelist ) ||
			$Datei == 'imgdel.gif') die( 'Operation kann nicht ausfuehrt werden!' );
		// Datei loeschen
		if (file_exists( $Datei ))
		{		
			// Vorhandenes Thumbnail loeschen
			if (array_pop( explode( ".", strtolower( $Datei ) ) ) == 'jpg')
			{
				$ThumbNameTmp = substr( $Datei, 0, -4 );
				$ThumbName = $ThumbNameTmp. "_t.jpg";
				if (file_exists( $ThumbName ) ) @unlink( $ThumbName );
			}
			@unlink( $Datei );
			header( 'location:' .$_SERVER['PHP_SELF']. '?seite=uebersicht&liste=1' );
		}
		else
		{
			// Datei existiert nicht
			echo "Die Datei {$Datei} konnte nicht gefunden werden!";
		}
	}
	// Funktion zum loeschena ufrufen
	loescheDatei( $_GET['datei'] );
}
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>image.invertika.org</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<style type="text/css">
<!--
/* *** Global *** */
* {
	margin  : 0;
	padding : 0;
	border  : 0 solid;
}
html, body {
	height: 100%;
}
body {
    font       : 62.5% Verdana, Arial, Helvetica, sans-serif;
    text-align : center;
	background : #DDDDFF;
}
#frame {
	width        : 770px;
	margin       : 0 auto;
	padding      : 1em;
	min-height   : 100%;
	height       : auto !important;  /* für moderne Browser */
	height       : 100%;  /*für den IE */
	background   : #F0F0F8;
	border-left  : solid 2px #D2D2E6;
	border-right : solid 2px #D2D2E6;
	text-align   : left;
}
p, li { font-size: 1.2em; }
h2 { font-size: 1.8em; }
hr { margin: 1.4em auto; height: 1px; background: #FF0000; color: #FF0000; border: 0; }
a { color: #0000FF; text-decoration: none; }
a:hover { color: #FF0000; text-decoration: underline; }
.headline { margin-bottom: .5em; }
.center { text-align: center; }
.navlinks { margin-bottom: 2em; }
.aktiv { color: red; }

/* *** Clearfix *** */
.clearfix:after {
    content     : "."; 
    display     : block; 
    height      : 0;
    font-size   : 0;
    clear       : both; 
    visibility  : hidden;
}
.clearfix { display: inline-block; }
/* Hides from IE-mac \*/
* html .clearfix { height: 1%; }
.clearfix { display: block; }
/* End hide from IE-mac */
<?php
// CSS fuer Uebersicht
if ($_GET['seite'] == 'uebersicht')
{
?>

/* *** Login  *** */
#uebersicht #loginform { margin: 5em auto; width: 202px; font-size: 1.2em; }
#uebersicht #loginform label { display: block; font-weight: bold; }
#uebersicht #loginform input { width: 200px; height: 1.4em; }
#uebersicht #loginform .loginbutton { margin-top: 1em; width: 208px; height: 1.6em; }

/* *** Uebersicht *** */
#uebersicht input { width: 490px; padding: 1px 3px; border: 1px solid #D2D2E6; }
#uebersicht ul { float: left; }
#uebersicht .thumbimg { width: 130px; float: left; overflow: auto; margin: 10px 5px 0 0; }
#uebersicht li { line-height: 2em; list-style-type: none; }
#uebersicht .hl { margin-left: 38px; }
#uebersicht .bl { margin-left: 24px; }
#uebersicht .bb { margin-left: 27px; }
#uebersicht .bt { margin-left: 8px; }
<?php
}
else if ($_GET['seite'] == 'upload')
{
?>
#upload #uploadform fieldset { width: 404px; margin: 5em auto 0; padding: 1em; border: 1px solid #D2D2E6; }
#upload #uploadform legend { font: normal bold 1.3em Verdana, Arial, Helvetica, sans-serif; color: #666666; }
#upload #uploadform label { cursor: pointer; font-size: 1.1em; }
#upload #uploadform input.dateifeld { width: 400px; border: 1px solid #999999; }
#upload #uploadform input.passwortfeld { width: 200px; border: 1px solid #999999; }
#upload #uploadform input.checkbox { width: 25px; }
#upload #uploadform input.button { width: 150px; background: #FFFFFF; border: 1px solid #CCCCCC; margin: 15px 0 0 120px; }
#upload #uploadform #jpgoptions fieldset { width: 380px; margin-top: 1em; }
#upload #uploadform .block { display: block; font-weight: bold; margin-top: 12px; }

#upload #output input { width: 490px; padding: 1px 3px; border: 1px solid #D2D2E6; }
#upload #output ul { float: left; }
#upload #output .thumbimg { width: 130px; float: left; overflow: auto; margin: 10px 5px 0 0; }
#upload #output li { line-height: 2em; list-style-type: none; }
#upload #output .hl { margin-left: 38px; }
#upload #output .bl { margin-left: 24px; }
#upload #output .bb { margin-left: 27px; }
#upload #output .bt { margin-left: 8px; }

<?php
}
?>
-->
</style>

<script type="text/javascript">
function hl(id)
{
    document.getElementById(id).focus();
    document.getElementById(id).select();
}

function del(dateiname)
{
	return confirm('Soll die Datei <'+dateiname+'> wirklich gelöscht werden?\nDas löschen ist endgültig und kann nicht rückgängig gemacht werden!');
}

function checkupload(Bild)
{
    if(Bild)
    {
        Dateiendung = Bild.substring(Bild.lastIndexOf(".")+1);
        if(Dateiendung.toLowerCase() != "jpg")
        {
            document.getElementById("makethumb").checked = false;
			document.getElementById("jpgoptions").style.display = 'none';
            return false;
        }
        else if(Dateiendung.toLowerCase() == "jpg")
        {
            document.getElementById("makethumb").checked = true;
			document.getElementById("jpgoptions").style.display = 'block';
        }
    }
}
</script>

</head>
<body>
<div id="frame">
<?php
// ---------------------------------------------------
// ----------          U P L O A D          ----------
// ---------------------------------------------------
if ($_GET['seite'] == 'upload')
{
?>
    <div id="upload">
    <h2 class="headline center">Upload</h2>
    <p class="center navlinks"><span class="aktiv">Upload</span> &nbsp;&bull;&nbsp; <a href="<?php echo $_SERVER['PHP_SELF']; ?>?seite=uebersicht">Übersicht</a></p>
    <hr />
    <?php
	if ($BildUpload)
	{
		echo $UploadOutput;
		echo '<hr />';
	}
	if ($PWFehler)
	{
		echo '<h3 style="color: red; text-align: center;"><strong>Falsches Passwort!</strong></h3>';
	}
	if ($PrivatScript)
	{
		echo '<h3 style="color: red; text-align: center;">Sorry, kein Passwort - kein Upload!</h3>';
	}
	if ($BildUploadFehler)
	{
		echo '<p style="color: red; text-align: center;"><strong>Das Bild konnte nicht hochgeladen werden!</strong></p>';
	}
	?>
    <form action="<?php echo $_SERVER['PHP_SELF']. "?seite=upload"; ?>" method="post" enctype="multipart/form-data" name="uploadform" id="uploadform">
    <fieldset><legend>Neue Datei hochladen</legend>
    <label for="uploadpasswort" class="block">Passwort für den Upload:</label>
    <input type="password" name="uploadpasswort" id="uploadpasswort"<?php echo ($_SESSION['passwort_ok'] === true) ? ' value="' .$passwort. '"' : ''; ?> class="passwortfeld" />
    <label for="upload" class="block">Datei auswählen:</label>
    <input type="file" name="upload" id="upload" onchange="checkupload(this.value);" size="50" class="dateifeld" />
    <br />
    <div id="jpgoptions" style="display: none;">
        <fieldset><legend>JPG Optionen</legend>
        <input type="checkbox" name="makethumb" id="makethumb" class="checkbox" /><label for="makethumb"> Thumbnail erstellen</label>
        <br /><br />
        <input type="checkbox" name="thumbinfo" id="thumbinfo" class="checkbox" checked="checked" /><label for="thumbinfo"> Bildinfo auf Thumbnail anzeigen</label>
        <br /><br />
        <select name="thumbsize" id="thumbsize">
        <option value="70">70px</option>
        <option value="80">80px</option>
        <option value="90">90px</option>
        <option value="100" selected="selected">100px</option>
        <option value="110">110px</option>
        <option value="120">120px</option>
        </select>
        <label for="thumbsize">Maximale Kantenlänge des Thumbnail</label>
        </fieldset>
    </div>
    <input type="submit" name="submit" value="&raquo; Datei hochladen &laquo;" class="button" />
    </fieldset>
    </form>
    </div><!-- Ende upload -->
<?php
}
// ---------------------------------------------------
// ----------      U E B E R S I C H T      ----------
// ---------------------------------------------------
else if ($_GET['seite'] == 'uebersicht')
{
?>
    <div id="uebersicht">
    <h2 class="headline center">Übersicht</h2>
    <p class="center navlinks"><a href="<?php echo $_SERVER['PHP_SELF']; ?>?seite=upload">Upload</a> &nbsp;&bull;&nbsp; <span class="aktiv">Übersicht</span></p>
    <hr />
<?php
if ($_GET['seite'] == 'uebersicht' &&
	$_SESSION['approved'] == 1)
{
	$DateiListe = glob( "{".$ImageURLRelative."*.jpg,".$ImageURLRelative."*.gif,".$ImageURLRelative."*.png}", GLOB_BRACE );
	sort( $DateiListe );
	$bid = 1;
	foreach ( $DateiListe as $DateiName)
	{
		if ($DateiName == "imgdel.gif") continue;
		if (substr( strtolower( $DateiName ), -6 ) != '_t.jpg' )
		{
			// Preview ermitteln
			switch (array_pop( explode( ".", strtolower( $DateiName ) ) ) )
			{
				case 'gif':
					$gifbreite = getimagesize( $DateiName );
					$preview = ($gifbreite[0] < 125 && $gifbreite[1] < 125) ? $DateiName : 'nopreview';
				break;
				
				case 'jpg':
					$ThumbNameTmp = substr( $DateiName, 0, -4 );
					$ThumbName = $preview = $ThumbNameTmp. "_t.jpg";
				break;
				
				case 'png':
					$pngbreite = getimagesize( $DateiName );
					$preview = ($pngbreite[0] < 125 && $pngbreite[0] < 125) ? $DateiName : 'nopreview';
				break;
			}

			// Links ausgeben
			echo '<p><a href="?seite=uebersicht&liste=1&aktion=loeschen&datei=' .$DateiName. '" onClick="return del(\'' .$DateiName. '\');"><img src="imgdel.gif" width="16" height="16" alt="" align="right" /></a><strong><a href="' .$DateiName. '">' .$DateiName. '</a></strong></p><br />';
			echo "\n";
			echo '<div class="clearfix">';
			echo file_exists( $preview ) ? '<div class="thumbimg"><img src="' .$preview. '" /></div>' : '<div class="thumbimg">&nbsp;</div>';
			echo '<ul>';
			echo "\n";
			echo '<li><strong>HTML Link:</strong> <input type="text" id="' .$bid. '" onclick="hl(' .$bid. ');" value="' .htmlspecialchars( "<a href=\"{$ImageURL}{$DateiName}\">Image Datei</a>" ). '" class="hl" /></li>';
			echo "\n";
			echo '<li><strong>BBCode Link:</strong> <input type="text" id="' .++$bid. '" onclick="hl(' .$bid. ');" value="' .htmlspecialchars( "[URL={$ImageURL}{$DateiName}]Image Datei[/URL]" ). '" class="bl" /></li>';
			echo "\n";
			echo '<li><strong>BBCode Bild:</strong> <input type="text" id="' .++$bid. '" onclick="hl(' .$bid. ');" value="' .htmlspecialchars( "[IMG]{$ImageURL}{$DateiName}[/IMG]" ). '" class="bb" /></li>';
			if (substr( strtolower( $DateiName ), -4 ) == '.jpg' )
			{
				$ThumbNameTmp = substr( $DateiName, 0, -4 );
				$ThumbName = $ThumbNameTmp. "_t.jpg";
				if (file_exists( $ThumbName ))
				{
					echo "\n";
					echo '<li><strong>BBCode Thumb: </strong><input type="text" id="' .++$bid. '" onclick="hl(' .$bid. ');" value="' .htmlspecialchars( "[URL={$ImageURL}{$DateiName}][IMG]{$ImageURL}{$ThumbName}[/IMG][/URL]" ). '" class="bt" /></li>';
					echo "\n";
				}
			}
			echo '</ul>';
			echo '</div>';
			echo '<hr />';
			echo "\n\n";
		}
		// Bild ID Zaehler erhoehen
		++$bid;
	}
}
else
{
?>
<div id="loginform">
    <form action="<?php echo $_SERVER['PHP_SELF']; ?>" method="post" id="login">
    <label for="uebersichtpwd">Passwort: </label><input type="password" name="uebersichtpwd" id="uebersichtpwd" size="20" />
    <input type="submit" name="uebersichtlogin" value="&raquo; Login &laquo;" class="loginbutton" />
    </form>
</div>
<?php
}
?>
    </div><!-- Ende uebersicht -->
<?php
}
?>
</div><!-- Ende frame -->
</body>
</html>