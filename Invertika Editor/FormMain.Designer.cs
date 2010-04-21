﻿namespace Invertika_Editor
{
	partial class FormMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components=null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing&&(components!=null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources=new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.menuStrip=new System.Windows.Forms.MenuStrip();
			this.toolStrip=new System.Windows.Forms.ToolStrip();
			this.statusStrip=new System.Windows.Forms.StatusStrip();
			this.dateiToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.hilfeToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.überToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.beendenToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.mappingToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.höhlengeneratorToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.automatismenToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.optionenToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1=new System.Windows.Forms.ToolStripSeparator();
			this.kartenthumbnailsUndMinimapsErzeugenToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.clientUpdateErstellenToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.datenOrdnerErstellenToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2=new System.Windows.Forms.ToolStripSeparator();
			this.skriptingToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.nPCGeneratorToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.xMLÖffnenToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3=new System.Windows.Forms.ToolStripSeparator();
			this.xMLSpeichernToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4=new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem5=new System.Windows.Forms.ToolStripSeparator();
			this.mapsInDieMapsxmlToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.monsterxmlMediaWikiToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.itemsxmlMediaWikiToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.mapsAusEinerBitmapErzeugenToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6=new System.Windows.Forms.ToolStripSeparator();
			this.weltkartenErzeugenToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.mapskripteErzeugenUndEintragenToolStripMenuItem=new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7=new System.Windows.Forms.ToolStripSeparator();
			this.openFileDialog=new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog=new System.Windows.Forms.SaveFileDialog();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.mappingToolStripMenuItem,
            this.skriptingToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.automatismenToolStripMenuItem,
            this.hilfeToolStripMenuItem});
			this.menuStrip.Location=new System.Drawing.Point(0, 0);
			this.menuStrip.Name="menuStrip";
			this.menuStrip.Size=new System.Drawing.Size(1006, 24);
			this.menuStrip.TabIndex=0;
			this.menuStrip.Text="menuStrip1";
			// 
			// toolStrip
			// 
			this.toolStrip.Location=new System.Drawing.Point(0, 24);
			this.toolStrip.Name="toolStrip";
			this.toolStrip.Size=new System.Drawing.Size(1006, 25);
			this.toolStrip.TabIndex=1;
			this.toolStrip.Text="toolStrip1";
			// 
			// statusStrip
			// 
			this.statusStrip.Location=new System.Drawing.Point(0, 451);
			this.statusStrip.Name="statusStrip";
			this.statusStrip.Size=new System.Drawing.Size(1006, 22);
			this.statusStrip.TabIndex=2;
			this.statusStrip.Text="statusStrip1";
			// 
			// dateiToolStripMenuItem
			// 
			this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xMLÖffnenToolStripMenuItem,
            this.toolStripMenuItem3,
            this.xMLSpeichernToolStripMenuItem,
            this.toolStripMenuItem4,
            this.optionenToolStripMenuItem,
            this.toolStripMenuItem1,
            this.beendenToolStripMenuItem});
			this.dateiToolStripMenuItem.Name="dateiToolStripMenuItem";
			this.dateiToolStripMenuItem.Size=new System.Drawing.Size(44, 20);
			this.dateiToolStripMenuItem.Text="&Datei";
			// 
			// hilfeToolStripMenuItem
			// 
			this.hilfeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.überToolStripMenuItem});
			this.hilfeToolStripMenuItem.Name="hilfeToolStripMenuItem";
			this.hilfeToolStripMenuItem.Size=new System.Drawing.Size(40, 20);
			this.hilfeToolStripMenuItem.Text="&Hilfe";
			// 
			// überToolStripMenuItem
			// 
			this.überToolStripMenuItem.Name="überToolStripMenuItem";
			this.überToolStripMenuItem.Size=new System.Drawing.Size(152, 22);
			this.überToolStripMenuItem.Text="Ü&ber...";
			// 
			// beendenToolStripMenuItem
			// 
			this.beendenToolStripMenuItem.Name="beendenToolStripMenuItem";
			this.beendenToolStripMenuItem.Size=new System.Drawing.Size(165, 22);
			this.beendenToolStripMenuItem.Text="B&eenden";
			this.beendenToolStripMenuItem.Click+=new System.EventHandler(this.beendenToolStripMenuItem_Click);
			// 
			// mappingToolStripMenuItem
			// 
			this.mappingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.höhlengeneratorToolStripMenuItem,
            this.toolStripMenuItem5,
            this.mapsInDieMapsxmlToolStripMenuItem,
            this.mapsAusEinerBitmapErzeugenToolStripMenuItem});
			this.mappingToolStripMenuItem.Name="mappingToolStripMenuItem";
			this.mappingToolStripMenuItem.Size=new System.Drawing.Size(59, 20);
			this.mappingToolStripMenuItem.Text="&Mapping";
			// 
			// höhlengeneratorToolStripMenuItem
			// 
			this.höhlengeneratorToolStripMenuItem.Name="höhlengeneratorToolStripMenuItem";
			this.höhlengeneratorToolStripMenuItem.Size=new System.Drawing.Size(252, 22);
			this.höhlengeneratorToolStripMenuItem.Text="Höhlengenerator...";
			// 
			// automatismenToolStripMenuItem
			// 
			this.automatismenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kartenthumbnailsUndMinimapsErzeugenToolStripMenuItem,
            this.toolStripMenuItem2,
            this.clientUpdateErstellenToolStripMenuItem,
            this.datenOrdnerErstellenToolStripMenuItem,
            this.toolStripMenuItem6,
            this.weltkartenErzeugenToolStripMenuItem});
			this.automatismenToolStripMenuItem.Name="automatismenToolStripMenuItem";
			this.automatismenToolStripMenuItem.Size=new System.Drawing.Size(65, 20);
			this.automatismenToolStripMenuItem.Text="S&onstiges";
			// 
			// optionenToolStripMenuItem
			// 
			this.optionenToolStripMenuItem.Name="optionenToolStripMenuItem";
			this.optionenToolStripMenuItem.Size=new System.Drawing.Size(165, 22);
			this.optionenToolStripMenuItem.Text="Optionen...";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name="toolStripMenuItem1";
			this.toolStripMenuItem1.Size=new System.Drawing.Size(162, 6);
			// 
			// kartenthumbnailsUndMinimapsErzeugenToolStripMenuItem
			// 
			this.kartenthumbnailsUndMinimapsErzeugenToolStripMenuItem.Name="kartenthumbnailsUndMinimapsErzeugenToolStripMenuItem";
			this.kartenthumbnailsUndMinimapsErzeugenToolStripMenuItem.Size=new System.Drawing.Size(295, 22);
			this.kartenthumbnailsUndMinimapsErzeugenToolStripMenuItem.Text="Kartenthumbnails und Minimaps erzeugen...";
			// 
			// clientUpdateErstellenToolStripMenuItem
			// 
			this.clientUpdateErstellenToolStripMenuItem.Name="clientUpdateErstellenToolStripMenuItem";
			this.clientUpdateErstellenToolStripMenuItem.Size=new System.Drawing.Size(295, 22);
			this.clientUpdateErstellenToolStripMenuItem.Text="Client Update erstellen...";
			// 
			// datenOrdnerErstellenToolStripMenuItem
			// 
			this.datenOrdnerErstellenToolStripMenuItem.Name="datenOrdnerErstellenToolStripMenuItem";
			this.datenOrdnerErstellenToolStripMenuItem.Size=new System.Drawing.Size(295, 22);
			this.datenOrdnerErstellenToolStripMenuItem.Text="Daten Ordner erstellen...";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name="toolStripMenuItem2";
			this.toolStripMenuItem2.Size=new System.Drawing.Size(292, 6);
			// 
			// skriptingToolStripMenuItem
			// 
			this.skriptingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nPCGeneratorToolStripMenuItem,
            this.toolStripMenuItem7,
            this.mapskripteErzeugenUndEintragenToolStripMenuItem});
			this.skriptingToolStripMenuItem.Name="skriptingToolStripMenuItem";
			this.skriptingToolStripMenuItem.Size=new System.Drawing.Size(60, 20);
			this.skriptingToolStripMenuItem.Text="&Skripting";
			// 
			// nPCGeneratorToolStripMenuItem
			// 
			this.nPCGeneratorToolStripMenuItem.Name="nPCGeneratorToolStripMenuItem";
			this.nPCGeneratorToolStripMenuItem.Size=new System.Drawing.Size(267, 22);
			this.nPCGeneratorToolStripMenuItem.Text="NPC Generator...";
			// 
			// xMLÖffnenToolStripMenuItem
			// 
			this.xMLÖffnenToolStripMenuItem.Name="xMLÖffnenToolStripMenuItem";
			this.xMLÖffnenToolStripMenuItem.Size=new System.Drawing.Size(165, 22);
			this.xMLÖffnenToolStripMenuItem.Text="XML öffnen...";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name="toolStripMenuItem3";
			this.toolStripMenuItem3.Size=new System.Drawing.Size(162, 6);
			// 
			// xMLSpeichernToolStripMenuItem
			// 
			this.xMLSpeichernToolStripMenuItem.Name="xMLSpeichernToolStripMenuItem";
			this.xMLSpeichernToolStripMenuItem.Size=new System.Drawing.Size(165, 22);
			this.xMLSpeichernToolStripMenuItem.Text="XML speichern...";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name="toolStripMenuItem4";
			this.toolStripMenuItem4.Size=new System.Drawing.Size(162, 6);
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name="toolStripMenuItem5";
			this.toolStripMenuItem5.Size=new System.Drawing.Size(249, 6);
			// 
			// mapsInDieMapsxmlToolStripMenuItem
			// 
			this.mapsInDieMapsxmlToolStripMenuItem.Name="mapsInDieMapsxmlToolStripMenuItem";
			this.mapsInDieMapsxmlToolStripMenuItem.Size=new System.Drawing.Size(252, 22);
			this.mapsInDieMapsxmlToolStripMenuItem.Text="Maps in die maps.xml eintragen...";
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemsxmlMediaWikiToolStripMenuItem,
            this.monsterxmlMediaWikiToolStripMenuItem});
			this.exportToolStripMenuItem.Name="exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size=new System.Drawing.Size(51, 20);
			this.exportToolStripMenuItem.Text="E&xport";
			// 
			// monsterxmlMediaWikiToolStripMenuItem
			// 
			this.monsterxmlMediaWikiToolStripMenuItem.Name="monsterxmlMediaWikiToolStripMenuItem";
			this.monsterxmlMediaWikiToolStripMenuItem.Size=new System.Drawing.Size(221, 22);
			this.monsterxmlMediaWikiToolStripMenuItem.Text="monster.xml -> MediaWiki...";
			// 
			// itemsxmlMediaWikiToolStripMenuItem
			// 
			this.itemsxmlMediaWikiToolStripMenuItem.Name="itemsxmlMediaWikiToolStripMenuItem";
			this.itemsxmlMediaWikiToolStripMenuItem.Size=new System.Drawing.Size(221, 22);
			this.itemsxmlMediaWikiToolStripMenuItem.Text="items.xml -> MediaWiki...";
			// 
			// mapsAusEinerBitmapErzeugenToolStripMenuItem
			// 
			this.mapsAusEinerBitmapErzeugenToolStripMenuItem.Name="mapsAusEinerBitmapErzeugenToolStripMenuItem";
			this.mapsAusEinerBitmapErzeugenToolStripMenuItem.Size=new System.Drawing.Size(252, 22);
			this.mapsAusEinerBitmapErzeugenToolStripMenuItem.Text="Maps aus einer Bitmap erzeugen...";
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name="toolStripMenuItem6";
			this.toolStripMenuItem6.Size=new System.Drawing.Size(292, 6);
			// 
			// weltkartenErzeugenToolStripMenuItem
			// 
			this.weltkartenErzeugenToolStripMenuItem.Name="weltkartenErzeugenToolStripMenuItem";
			this.weltkartenErzeugenToolStripMenuItem.Size=new System.Drawing.Size(295, 22);
			this.weltkartenErzeugenToolStripMenuItem.Text="Weltkarten erzeugen...";
			// 
			// mapskripteErzeugenUndEintragenToolStripMenuItem
			// 
			this.mapskripteErzeugenUndEintragenToolStripMenuItem.Name="mapskripteErzeugenUndEintragenToolStripMenuItem";
			this.mapskripteErzeugenUndEintragenToolStripMenuItem.Size=new System.Drawing.Size(267, 22);
			this.mapskripteErzeugenUndEintragenToolStripMenuItem.Text="Mapskripte erzeugen und eintragen...";
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name="toolStripMenuItem7";
			this.toolStripMenuItem7.Size=new System.Drawing.Size(264, 6);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(1006, 473);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.menuStrip);
			this.Icon=((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip=this.menuStrip;
			this.Name="FormMain";
			this.StartPosition=System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text="Invertika Editor";
			this.WindowState=System.Windows.Forms.FormWindowState.Maximized;
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hilfeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem überToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mappingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem höhlengeneratorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem automatismenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionenToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem skriptingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nPCGeneratorToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem kartenthumbnailsUndMinimapsErzeugenToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem clientUpdateErstellenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem datenOrdnerErstellenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem xMLÖffnenToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem xMLSpeichernToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.ToolStripMenuItem mapsInDieMapsxmlToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem monsterxmlMediaWikiToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem itemsxmlMediaWikiToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mapsAusEinerBitmapErzeugenToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem mapskripteErzeugenUndEintragenToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem weltkartenErzeugenToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
	}
}
