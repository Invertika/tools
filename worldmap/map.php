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
	
	require 'config.php';
	
	$zoom = 100;
	$fieldX  = 0;
	$fieldY = 0;

	if(!empty($_GET['zoom']) && is_numeric($_GET['zoom'])) $zoom = $_GET['zoom'];
	if(!empty($_GET['fieldX']) && is_numeric($_GET['fieldX'])) $fieldX= $_GET['fieldX'];
	if(!empty($_GET['fieldY']) && is_numeric($_GET['fieldY'])) $fieldY = $_GET['fieldY'];
		
	$fnImageFile=$mappath . GetOuterWorldMapFilename($fieldX, $fieldY, $zoom);
	
	header("Content-Type: image/png"); 
	$source = imagecreatefrompng($fnImageFile);
	imagepng($source);
	imagedestroy($source);
?>