using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CSCL.Controls;
using CSCL;
using System.IO;

namespace Invertika_Editor.Controls
{
	public partial class QuestEditorPanel : Panel
	{
		Timer timer;

		ShapeControl SelectedShapeControl=null;

		public enum ElementType
		{
			Start,
			Message,
			If,
			Choice,
			End
		}

		public List<Pair<ShapeControl>> Connections
		{
			get;
			private set;
		}

		private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
		{
			Invalidate();
		}

		public QuestEditorPanel()
		{
			InitializeComponent();
			Connections=new List<Pair<ShapeControl>>();

			timer=new Timer();
			timer.Tick+=new EventHandler(TimerEventProcessor);

			timer.Interval=100;
			timer.Start();

			ShapeControl sc=CreateNewElement(ElementType.Start, null);
			//shapeControl_Click(sc, null);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);

			foreach(Pair<ShapeControl> i in Connections)
			{
				Point p1=new Point();
				p1.X=i.First.Location.X+i.First.Size.Width/2;
				p1.Y=i.First.Location.Y+i.First.Size.Height;

				Point p2=new Point();
				p2.X=i.Second.Location.X+i.Second.Size.Width/2;
				p2.Y=i.Second.Location.Y;

				pe.Graphics.DrawLine(Pens.Blue, p1, p2);
			}
		}

		private void shapeControl_Click(object sender, EventArgs e)
		{
			foreach(ShapeControl i in Controls)
			{
				i.BorderWidth=0;
			}

			ShapeControl c=(ShapeControl)sender;
			c.BorderWidth=3;
			SelectedShapeControl=c;
		}

		private void shapeControl_DoubleClick(object sender, EventArgs e)
		{
			ShapeControl c=(ShapeControl)sender;
			Parameters param=(Parameters)c.Tag;

			switch(param.GetString("Type"))
			{
				case "Start":
					{
						MessageBox.Show("Der Startknoten verfügt über keinerlei Eigenschaften.", "Hinweis", MessageBoxButtons.OK, MessageBoxIcon.Information);
						break;
					}
				case "Message":
					{
						QuestEditorMessageEditor InstQuestEditorMessageEditor=new QuestEditorMessageEditor();
						InstQuestEditorMessageEditor.Data=param;
						InstQuestEditorMessageEditor.ShowDialog();
						break;
					}
				default:
					{
						throw new NotImplementedException();
					}
			}
		}

		private ShapeControl CreateNewElement(ElementType element, ShapeControl parent)
		{
			ShapeControl shapeControl=new ShapeControl();

			foreach(Pair<ShapeControl> psc in Connections)
			{
				if(psc.First==parent) return null;
			}

			switch(element)
			{
				case ElementType.Start:
					{
						shapeControl.BackColor=System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(141)))), ((int)(((byte)(148)))), ((int)(((byte)(152)))));
						shapeControl.BackgroundImageLayout=System.Windows.Forms.ImageLayout.None;
						shapeControl.BorderColor=System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
						shapeControl.BorderStyle=System.Drawing.Drawing2D.DashStyle.Solid;
						shapeControl.BorderWidth=0;
						shapeControl.CenterColor=System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
						shapeControl.Font=new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
						shapeControl.ForeColor=System.Drawing.Color.Black;
						shapeControl.Location=new System.Drawing.Point(15, 15);
						shapeControl.Name="scStart";
						shapeControl.Shape=CSCL.Controls.ShapeType.RoundedRectangle;
						shapeControl.Size=new System.Drawing.Size(144, 51);
						shapeControl.SurroundColor=System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
						shapeControl.TabIndex=1;
						shapeControl.Text="Start";
						shapeControl.UseGradient=true;

						Parameters param=new Parameters();
						param.Add("Type", "Start");
						shapeControl.Tag=param;

						break;
					}
				case ElementType.Message:
					{
						shapeControl.BackColor=System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(141)))), ((int)(((byte)(148)))), ((int)(((byte)(152)))));
						shapeControl.BackgroundImageLayout=System.Windows.Forms.ImageLayout.None;
						shapeControl.BorderColor=System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
						shapeControl.BorderStyle=System.Drawing.Drawing2D.DashStyle.Solid;
						shapeControl.BorderWidth=0;
						shapeControl.CenterColor=System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
						shapeControl.Font=new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
						shapeControl.ForeColor=System.Drawing.Color.Yellow;
						shapeControl.Name="scMessage";
						shapeControl.Shape=CSCL.Controls.ShapeType.Rectangle;
						shapeControl.Size=new System.Drawing.Size(144, 51);
						shapeControl.SurroundColor=System.Drawing.Color.FromArgb(((int)(((byte)(129)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
						shapeControl.TabIndex=1;
						shapeControl.Text="Nachricht";
						shapeControl.UseGradient=true;

						Point NewLocation=new Point();
						NewLocation.X=parent.Location.X;
						NewLocation.Y=parent.Location.Y+parent.Size.Height+15;
						shapeControl.Location=NewLocation;

						Pair<ShapeControl> pair=new Pair<ShapeControl>(parent, shapeControl);
						Connections.Add(pair);

						Parameters param=new Parameters();
						param.Add("Type", "Message");
						param.Add("Message/MultibleValues", false);
						param.Add("Message/Values", "");
						shapeControl.Tag=param;

						break;
					}
				case ElementType.If:
					{
						shapeControl.BackColor=System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(141)))), ((int)(((byte)(148)))), ((int)(((byte)(152)))));
						shapeControl.BackgroundImageLayout=System.Windows.Forms.ImageLayout.None;
						shapeControl.BorderColor=System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
						shapeControl.BorderStyle=System.Drawing.Drawing2D.DashStyle.Solid;
						shapeControl.BorderWidth=0;
						shapeControl.CenterColor=System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
						shapeControl.Font=new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
						shapeControl.ForeColor=System.Drawing.Color.Yellow;
						shapeControl.Name="scIf";
						shapeControl.Shape=CSCL.Controls.ShapeType.Diamond;
						shapeControl.Size=new System.Drawing.Size(144, 51);
						shapeControl.SurroundColor=System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
						shapeControl.TabIndex=1;
						shapeControl.Text="If";
						shapeControl.UseGradient=true;

						Point NewLocation=new Point();
						NewLocation.X=parent.Location.X;
						NewLocation.Y=parent.Location.Y+parent.Size.Height+15;
						shapeControl.Location=NewLocation;

						Pair<ShapeControl> pair=new Pair<ShapeControl>(parent, shapeControl);
						Connections.Add(pair);

						Parameters param=new Parameters();
						param.Add("Type", "If");
						param.Add("If/Value1", "");
						param.Add("If/Value2", "");
						param.Add("If/Operator", "");
						shapeControl.Tag=param;

						break;
					}
				default:
					{
						throw new NotImplementedException();
					}
			}

			shapeControl.Click+=new EventHandler(shapeControl_Click);
			shapeControl.DoubleClick+=new EventHandler(shapeControl_DoubleClick);

			//Debug
			//shapeControl.Dragable=true;

			this.Controls.Add(shapeControl);
			shapeControl_Click(shapeControl, null);

			return shapeControl;
		}

		public bool AddElementToSelected(ElementType et)
		{
			if(SelectedShapeControl!=null)
			{
				CreateNewElement(et, SelectedShapeControl);
				return true;
			}

			return false;
		}

		public void ExportToLuaFile(string filename)
		{
			DTreeNode<Parameters> root=new DTreeNode<Parameters>();
			DTreeNode<Parameters> start=null;

			List<string> lines=new List<string>();

			lines.Add("function xxx_talk(npc, ch)");

			foreach(Pair<ShapeControl> psc in Connections)
			{
				Parameters param=(Parameters)psc.First.Tag;

				switch(param.GetString("Type"))
				{
					case "Start":
						{
							start=root.Nodes.Add(param);
							break;
						}
					case "Message":
						{
							start=start.Nodes.Add(param);

							List<string> value=param.GetStringList("Message/Values");

							if(value==null)
							{
								value=new List<string>();
								value.Add("Keine Nachricht definiert!");
							}

							if(param.GetBool("Message/MultibleValues")==true)
							{
								string output="do_message(npc, ch, invertika.get_random_element(";

								foreach(string line in value)
								{
									if(output[output.Length-1]=='(')
									{
										output+=String.Format("\"{0}\",\n", line);
									}
									else
									{
										output+=String.Format("	  \"{0}\",\n", line);
									}
								}

								output=output.TrimEnd('\n');
								output=output.TrimEnd(',');

								output+="))";

								output+="\n";

								lines.Add(output);
							}
							else
							{
								lines.Add(String.Format("do_message(npc, ch, \"{0}\")", value[0]));
							}

							break;
						}
					default:
						{
							throw new NotImplementedException();
						}
				}
			}

			lines.Add("do_npc_close(npc, ch)");
			lines.Add("end");
			File.WriteAllLines(filename, lines.ToArray());
		}
	}
}
