using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using Molmed.SQAT.ClientObjects;
using Molmed.SQAT.DBObjects;
using Molmed.SQAT.ServiceObjects;

namespace Molmed.SQAT.GUI
{
	/// <summary>
	/// Summary description for ConnectionManager.
	/// </summary>
	public class ConnectionManager : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.Label PromtLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ComboBox ConnectionList;
         
		private const string ConnectionFileName = "SQATconnect.XML";
		private bool MyOKFlag;
        private struct ConnectionData
        {
            public const string CONNECTION = "Connection";
            public const string CONNECTIONS = "Connections";
            public const string NAME = "Name";
            public const string CONNECTION_STRING = "ConnectionString";
            public const string CHIASMA_CONNECTION_STRING = "ChiasmaConnectionString";
        }

		public ConnectionManager()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionManager));
            this.OKButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.PromtLabel = new System.Windows.Forms.Label();
            this.ConnectionList = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OKButton.Location = new System.Drawing.Point(8, 96);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(72, 24);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "&OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(176, 96);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(72, 24);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "&Cancel";
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // PromtLabel
            // 
            this.PromtLabel.Location = new System.Drawing.Point(24, 16);
            this.PromtLabel.Name = "PromtLabel";
            this.PromtLabel.Size = new System.Drawing.Size(200, 24);
            this.PromtLabel.TabIndex = 2;
            this.PromtLabel.Text = "Please select database connection:";
            this.PromtLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConnectionList
            // 
            this.ConnectionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ConnectionList.Location = new System.Drawing.Point(24, 48);
            this.ConnectionList.Name = "ConnectionList";
            this.ConnectionList.Size = new System.Drawing.Size(216, 21);
            this.ConnectionList.TabIndex = 0;
            // 
            // ConnectionManager
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(258, 126);
            this.Controls.Add(this.ConnectionList);
            this.Controls.Add(this.PromtLabel);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.OKButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connections";
            this.TopMost = true;
            this.ResumeLayout(false);

		}
		#endregion

		public string GetConnectionString()
		{
			StringDictionary connections;
			IEnumerator conEnumerator;

			MyOKFlag = false;
			connections = this.ReadConnections();
			if (connections == null)
			{
				throw new Exception("Unable to read connection file.");
			}
			if (connections.Count < 1)
			{
				throw new Exception("No connections defined in connection file.");
			}
			//If there was only one connection defined, return it.
			if (connections.Count == 1)
			{
				conEnumerator = connections.Values.GetEnumerator();
				conEnumerator.MoveNext();
				return (conEnumerator.Current.ToString());
			}

			//More than one connection, let the user select.
			foreach(string tempName in connections.Keys)
			{
				ConnectionList.Items.Add(tempName.ToUpper());
			}

			ConnectionList.Sorted = true;
			ConnectionList.SelectedIndex = 0;
			this.ShowDialog();

			return (MyOKFlag ? connections[ConnectionList.SelectedItem.ToString()] : "");
		}

        public string GetConnectionString2(out string chiasmaConnectionString)
        {
            DataTable connections;
            DataRow row;
            chiasmaConnectionString = "";
            MyOKFlag = false;
            connections = this.ReadConnections2();
            if (connections == null)
            {
                throw new Exception("Unable to read connection file.");
            }
            if (connections.Rows.Count < 1)
            {
                throw new Exception("No connections defined in connection file.");
            }
            //If there was only one connection defined, return it.
            if (connections.Rows.Count == 1)
            {
                row = connections.Rows[0];
                chiasmaConnectionString = row[ConnectionData.CHIASMA_CONNECTION_STRING].ToString();
                return row[ConnectionData.CONNECTION_STRING].ToString();
            }

            //More than one connection, let the user select.

            foreach (DataRow dataRow in connections.Rows)
            {
                ConnectionList.Items.Add(dataRow[ConnectionData.NAME].ToString().ToUpper());
            }

            ConnectionList.Sorted = true;
            ConnectionList.SelectedIndex = 0;
            this.ShowDialog();

            if (MyOKFlag)
            {
                foreach (DataRow dataRow in connections.Rows)
                {
                    if (dataRow[ConnectionData.NAME].ToString().ToUpper() == ConnectionList.SelectedItem.ToString())
                    {
                        chiasmaConnectionString = dataRow[ConnectionData.CHIASMA_CONNECTION_STRING].ToString();
                        return dataRow[ConnectionData.CONNECTION_STRING].ToString();
                    }
                }
                throw new Exception("Unable to match connection database name!");
            }
            else
            {
                chiasmaConnectionString = "";
                return "";
            }
        }

		private StringDictionary ReadConnections()
		{
			DataSet dSet;
			StringDictionary connections;
            string connectionFilePath;

            connectionFilePath = Application.StartupPath + "\\" + ConnectionFileName;

			if (!File.Exists(connectionFilePath))
			{
				throw new Exception("Unable to find connection file " + connectionFilePath + ".");
			}

			dSet = this.CreateSerializeDataSet();

			dSet.ReadXml(connectionFilePath, XmlReadMode.IgnoreSchema);

			connections = new StringDictionary();
			foreach(DataRow tempRow in dSet.Tables[ConnectionData.CONNECTION].Rows)
			{
				connections.Add(tempRow[ConnectionData.NAME].ToString(), tempRow[ConnectionData.CONNECTION_STRING].ToString());
			}

			return connections;
		}

        private DataTable ReadConnections2()
        {
            DataSet dSet;
            DataTable connections = null;
            DataRow row;
            string connectionFilePath;

            connectionFilePath = Application.StartupPath + "\\" + ConnectionFileName;

            if (!File.Exists(connectionFilePath))
            {
                throw new Exception("Unable to find connection file " + connectionFilePath + ".");
            }

            dSet = this.CreateSerializeDataSet();

            dSet.ReadXml(connectionFilePath, XmlReadMode.IgnoreSchema);

            connections = GetConnectionsDataTableFrame();
            foreach (DataRow tempRow in dSet.Tables[ConnectionData.CONNECTION].Rows)
            {
                row = connections.NewRow();
                row[ConnectionData.NAME] = tempRow[ConnectionData.NAME];
                row[ConnectionData.CONNECTION_STRING] = tempRow[ConnectionData.CONNECTION_STRING];
                row[ConnectionData.CHIASMA_CONNECTION_STRING] = tempRow[ConnectionData.CHIASMA_CONNECTION_STRING];
                connections.Rows.Add(row);
            }

            return connections;
        }

        private DataTable GetConnectionsDataTableFrame()
        {
            DataTable connectionDataTable;
            DataColumn column;
            connectionDataTable = new DataTable();
            column = new DataColumn(ConnectionData.NAME, typeof(string));
            column.AllowDBNull = false;
            column.Unique = true;
            connectionDataTable.Columns.Add(column);

            column = new DataColumn(ConnectionData.CONNECTION_STRING, typeof(string));
            column.AllowDBNull = false;
            column.Unique = true;
            connectionDataTable.Columns.Add(column);

            column = new DataColumn(ConnectionData.CHIASMA_CONNECTION_STRING, typeof(string));
            column.AllowDBNull = false;
            column.Unique = true;
            connectionDataTable.Columns.Add(column);
            return connectionDataTable;
        }

		private DataSet CreateSerializeDataSet()
		{
			DataSet dSet;
			DataTable table;

			table = new DataTable(ConnectionData.CONNECTION);
			table.Columns.Add(ConnectionData.NAME, typeof(System.String));
            table.Columns.Add(ConnectionData.CONNECTION_STRING, typeof(System.String));
            table.Columns.Add(ConnectionData.CHIASMA_CONNECTION_STRING, typeof(System.String));
			
			dSet = new DataSet(ConnectionData.CONNECTIONS);
			dSet.Tables.Add(table);

			return dSet;
		}

		private void OKButton_Click(object sender, System.EventArgs e)
		{
			MyOKFlag = true;
			this.Close();
		}

		private void CloseButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
