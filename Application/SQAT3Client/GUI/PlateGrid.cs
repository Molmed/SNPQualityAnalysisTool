using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;


namespace Molmed.SQAT.GUI
{
	/// <summary>
	/// Summary description for PlateGrid.
	/// </summary>
	public class PlateGrid : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		private Cell [,] MyCellMatrix;
		private Size MyInnerCellSize;
		private Size MyOuterCellSize;
		private int MyCellNumberX, MyCellNumberY;
		private Point MyGridOffset;  //Offset of grid from origo.
		private Point MySelectionStartIndex;  //Indices of first cell in a range selection.
		private bool MySelectionActiveFlag = false;  //True when a range selection has been started.

		public PlateGrid()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// PlateGrid
			// 
			this.Name = "PlateGrid";
			this.Size = new System.Drawing.Size(248, 160);

		}
		#endregion

		public void ShowGrid(object [,] content, string [] colHeaders, string [] rowHeaders, 
							Size outerCellSize, Size innerCellSize)
		{
			Header [] rowHeader, colHeader;
			Point labelOffset;	//Offset of labels from origo.
			double labelOffsetFraction;  //The fraction of the outer cell size at which the label starts. 

			//Check that the number of column headers matches the first dimension of the content matrix,
			//and that the number of row headers matches the second dimension of the content matrix.
			if (content.GetLength(0) != colHeaders.GetLength(0)
				|| content.GetLength(1) != rowHeaders.GetLength(0))
			{
				throw new Exception("Non-matching content and header dimensions.");
			}

			//Hide control while updating.
			this.Visible = false;

			//Remove any already existing cells etc.
			this.ClearGrid();

			MyCellNumberX = content.GetLength(0);
			MyCellNumberY = content.GetLength(1);
			MyOuterCellSize = outerCellSize;
			MyInnerCellSize = innerCellSize;

			labelOffsetFraction = 0.2;

			MyGridOffset = new Point(MyOuterCellSize.Width + 2, MyOuterCellSize.Height + 2);
			labelOffset = new Point((int)(MyOuterCellSize.Width * labelOffsetFraction), 
									(int)(MyOuterCellSize.Height * labelOffsetFraction));

			//Make sure the inner size is smaller than the outer size.
			MyInnerCellSize.Height = MyInnerCellSize.Height > MyOuterCellSize.Height ? 
										MyOuterCellSize.Height : MyInnerCellSize.Height;
			MyInnerCellSize.Width = MyInnerCellSize.Width  > MyOuterCellSize.Width  ? 
										MyOuterCellSize.Width  : MyInnerCellSize.Width ;

			MyCellMatrix = new Cell[MyCellNumberX, MyCellNumberY];
			colHeader = new Header[MyCellNumberX];
			rowHeader = new Header[MyCellNumberY];

			for (int x = 0 ; x < MyCellNumberX ; x++)
			{
				colHeader[x] = new Header();
				colHeader[x].Location = new Point(MyOuterCellSize.Width * (x + 1) + labelOffset.X, 1);
				colHeader[x].Size = MyOuterCellSize;
				colHeader[x].Text = colHeaders[x];
				this.Controls.Add(colHeader[x]);

				for (int y = 0 ; y < MyCellNumberY ; y++)
				{
					if (x == 0)
					{
						rowHeader[y] = new Header();
						rowHeader[y].Location = new Point(1, MyOuterCellSize.Height * (y + 1) + labelOffset.Y );
						rowHeader[y].Size = MyOuterCellSize;
						rowHeader[y].Text = rowHeaders[y];
						this.Controls.Add(rowHeader[y]);					
					}
					MyCellMatrix[x, y] = new Cell(content[x, y]);
					MyCellMatrix[x, y].Location = new Point(MyOuterCellSize.Width * x + MyGridOffset.X, 
															MyOuterCellSize.Height * y + MyGridOffset.Y);
					MyCellMatrix[x, y].Size = MyInnerCellSize;
					MyCellMatrix[x, y].MouseDown += new MouseEventHandler(Cell_MouseDown);
					this.Controls.Add(MyCellMatrix[x, y]);
				}
			}
			this.Visible = true;
		}

		public void ClearGrid()
		{
			this.Controls.Clear();
		}

		public object [] GetSelectedObjects()
		{
			ArrayList values;
			object [] returnObjects;

			values = new ArrayList(MyCellNumberX * MyCellNumberY);

			for (int x = 0 ; x < MyCellNumberX ; x++)
			{
				for (int y = 0 ; y < MyCellNumberY ; y++)
				{
					if (MyCellMatrix[x, y].IsMarkSelected)
					{
						values.Add(MyCellMatrix[x, y].Content);
					}
				}
			}
			
			returnObjects = new object[values.Count];
			values.CopyTo(returnObjects);

			return returnObjects;
		}

		public Point [] GetSelectedIndices()
		{
			ArrayList indices;
			Point [] returnIndices;

			indices = new ArrayList(MyCellNumberX * MyCellNumberY);

			for (int x = 0 ; x < MyCellNumberX ; x++)
			{
				for (int y = 0 ; y < MyCellNumberY ; y++)
				{
					if (MyCellMatrix[x, y].IsMarkSelected)
					{
						indices.Add(new Point(x + 1, y + 1));
					}
				}
			}
			returnIndices = new Point[indices.Count];
			indices.CopyTo(returnIndices);

			return returnIndices;
		}

		private Point IndexFromRaster (Point rasterPoint)
		{
			Point relGridTopPoint;
			int indexX, indexY;

			relGridTopPoint = new Point(rasterPoint.X - MyGridOffset.X, rasterPoint.Y - MyGridOffset.Y);
			indexX = relGridTopPoint.X / MyOuterCellSize.Width;
			indexY = relGridTopPoint.Y / MyOuterCellSize.Height;

			indexX = indexX > (MyCellNumberX - 1) ? MyCellNumberX - 1 : indexX;
			indexY = indexY > (MyCellNumberY - 1) ? MyCellNumberY - 1 : indexY;

			return new Point(indexX, indexY);
		}

		private void SelectCells(Point startIndex, Point endIndex)
		{
			int tempVal;

			if (startIndex.X > endIndex.X)
			{
				tempVal = endIndex.X;
				endIndex.X = startIndex.X;
				startIndex.X = tempVal;
			}
			if (startIndex.Y > endIndex.Y)
			{
				tempVal = endIndex.Y;
				endIndex.Y = startIndex.Y;
				startIndex.Y = tempVal;
			}

			for (int x = startIndex.X ; x <= endIndex.X ; x++)
			{
				for (int y = startIndex.Y ; y <= endIndex.Y ; y++)
				{
					MyCellMatrix[x, y].MarkSelected();
				}
			}
		}

		private void ClearSelection()
		{
			for (int x = 0 ; x < MyCellNumberX ; x++)
			{
				for (int y = 0 ; y < MyCellNumberY ; y++)
				{

					MyCellMatrix[x, y].UnmarkSelected();
				}
			}
		}

		private void SetStartSelectionCell(Point index)
		{
			MyCellMatrix[index.X, index.Y].MarkStartRange();
			MySelectionStartIndex = index;
		}


		private void Cell_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Cell tempCell;
			Point endSelectionIndex;

			try
			{
				tempCell = (Cell)sender;
			
				if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
				{
					if (!((Control.ModifierKeys & Keys.Control) == Keys.Control))
					{
						ClearSelection();
					}
					if (MySelectionActiveFlag)
					{
						endSelectionIndex = IndexFromRaster(new Point(tempCell.Location.X, tempCell.Location.Y));					
						SelectCells(MySelectionStartIndex, endSelectionIndex);
					}
					else
					{
						MySelectionStartIndex = IndexFromRaster(new Point(tempCell.Location.X, tempCell.Location.Y));
						SetStartSelectionCell(MySelectionStartIndex);
					}
					MySelectionActiveFlag = !MySelectionActiveFlag;
				}
				else
				{
					if (!((Control.ModifierKeys & Keys.Control) == Keys.Control))
					{
						ClearSelection();
					}
					endSelectionIndex = IndexFromRaster(new Point(tempCell.Location.X, tempCell.Location.Y));
					SelectCells(endSelectionIndex, endSelectionIndex);
				}
			}
			catch (Exception ex)
			{
				MessageManager.ShowError(ex, "Error when selecting cell.", this.FindForm());
			}
		}




		private class Cell : PictureBox
		{
			Color EmptyColor = Color.White;
			Color FullColor = Color.CornflowerBlue;
			Color SelectColor = Color.Blue;
			Color StartSelectionColor = Color.AntiqueWhite;

			private bool MySelectedFlag = false;

			public Cell(object content) : base()
			{
				this.Content = content;
			}
			
			public void MarkSelected()
			{
				MySelectedFlag = true;
				this.BackColor = SelectColor;
			}

			public void UnmarkSelected()
			{
				MySelectedFlag = false;
				this.BackColor = (this.Content != null ? FullColor : EmptyColor);
			}

			public void MarkStartRange()
			{
				this.BackColor = StartSelectionColor;
			}

			public bool IsMarkSelected
			{
				get
				{
					return MySelectedFlag;
				}
			}

			public object Content
			{
				get
				{
					return this.Tag;
				}
				set
				{
					this.Tag = value;
					this.BackColor = (value != null ? FullColor : EmptyColor);
				}
			}

		}

		private class Header : Label
		{

			public Header() : base()
			{
				this.TextAlign = ContentAlignment.TopLeft;
			}
		}
	}
}
