namespace WinFormsLab
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxAdd = new System.Windows.Forms.GroupBox();
            this.furnitureFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonTable = new System.Windows.Forms.Button();
            this.buttonBigTable = new System.Windows.Forms.Button();
            this.buttonSofa = new System.Windows.Forms.Button();
            this.buttonBed = new System.Windows.Forms.Button();
            this.wallButton = new System.Windows.Forms.Button();
            this.groupBoxDisplay = new System.Windows.Forms.GroupBox();
            this.listViewDisplay = new System.Windows.Forms.ListView();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuNewBlueprint = new System.Windows.Forms.MenuItem();
            this.openMenuItem = new System.Windows.Forms.MenuItem();
            this.saveMenuItem = new System.Windows.Forms.MenuItem();
            this.languageMenuItem = new System.Windows.Forms.MenuItem();
            this.polishButton = new System.Windows.Forms.MenuItem();
            this.englishButton = new System.Windows.Forms.MenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.groupBoxAdd.SuspendLayout();
            this.furnitureFlowLayoutPanel.SuspendLayout();
            this.groupBoxDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.splitContainer, "splitContainer");
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            resources.ApplyResources(this.splitContainer.Panel1, "splitContainer.Panel1");
            this.splitContainer.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer.Panel1.SizeChanged += new System.EventHandler(this.splitContainer_Panel1_SizeChanged);
            this.splitContainer.Panel1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanel);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // tableLayoutPanel
            // 
            resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
            this.tableLayoutPanel.Controls.Add(this.groupBoxAdd, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.groupBoxDisplay, 0, 1);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            // 
            // groupBoxAdd
            // 
            this.groupBoxAdd.Controls.Add(this.furnitureFlowLayoutPanel);
            resources.ApplyResources(this.groupBoxAdd, "groupBoxAdd");
            this.groupBoxAdd.Name = "groupBoxAdd";
            this.groupBoxAdd.TabStop = false;
            // 
            // furnitureFlowLayoutPanel
            // 
            this.furnitureFlowLayoutPanel.Controls.Add(this.buttonTable);
            this.furnitureFlowLayoutPanel.Controls.Add(this.buttonBigTable);
            this.furnitureFlowLayoutPanel.Controls.Add(this.buttonSofa);
            this.furnitureFlowLayoutPanel.Controls.Add(this.buttonBed);
            this.furnitureFlowLayoutPanel.Controls.Add(this.wallButton);
            resources.ApplyResources(this.furnitureFlowLayoutPanel, "furnitureFlowLayoutPanel");
            this.furnitureFlowLayoutPanel.Name = "furnitureFlowLayoutPanel";
            // 
            // buttonTable
            // 
            this.buttonTable.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonTable, "buttonTable");
            this.buttonTable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonTable.Name = "buttonTable";
            this.buttonTable.UseVisualStyleBackColor = false;
            this.buttonTable.Click += new System.EventHandler(this.buttonTable_Click);
            // 
            // buttonBigTable
            // 
            this.buttonBigTable.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonBigTable, "buttonBigTable");
            this.buttonBigTable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonBigTable.Name = "buttonBigTable";
            this.buttonBigTable.UseVisualStyleBackColor = false;
            this.buttonBigTable.Click += new System.EventHandler(this.buttonBigTable_Click);
            // 
            // buttonSofa
            // 
            this.buttonSofa.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonSofa, "buttonSofa");
            this.buttonSofa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSofa.Name = "buttonSofa";
            this.buttonSofa.UseVisualStyleBackColor = false;
            this.buttonSofa.Click += new System.EventHandler(this.buttonSofa_Click);
            // 
            // buttonBed
            // 
            this.buttonBed.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.buttonBed, "buttonBed");
            this.buttonBed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonBed.Name = "buttonBed";
            this.buttonBed.UseVisualStyleBackColor = false;
            this.buttonBed.Click += new System.EventHandler(this.buttonBed_Click);
            // 
            // wallButton
            // 
            this.wallButton.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.wallButton, "wallButton");
            this.wallButton.Name = "wallButton";
            this.wallButton.UseVisualStyleBackColor = false;
            this.wallButton.Click += new System.EventHandler(this.wallButton_Click);
            // 
            // groupBoxDisplay
            // 
            this.groupBoxDisplay.Controls.Add(this.listViewDisplay);
            resources.ApplyResources(this.groupBoxDisplay, "groupBoxDisplay");
            this.groupBoxDisplay.Name = "groupBoxDisplay";
            this.groupBoxDisplay.TabStop = false;
            // 
            // listViewDisplay
            // 
            resources.ApplyResources(this.listViewDisplay, "listViewDisplay");
            this.listViewDisplay.FullRowSelect = true;
            this.listViewDisplay.HideSelection = false;
            this.listViewDisplay.Name = "listViewDisplay";
            this.listViewDisplay.UseCompatibleStateImageBehavior = false;
            this.listViewDisplay.View = System.Windows.Forms.View.List;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile});
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuNewBlueprint,
            this.openMenuItem,
            this.saveMenuItem,
            this.languageMenuItem});
            resources.ApplyResources(this.menuFile, "menuFile");
            // 
            // menuNewBlueprint
            // 
            this.menuNewBlueprint.Index = 0;
            resources.ApplyResources(this.menuNewBlueprint, "menuNewBlueprint");
            this.menuNewBlueprint.Click += new System.EventHandler(this.menuNewBlueprint_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Index = 1;
            resources.ApplyResources(this.openMenuItem, "openMenuItem");
            this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
            // 
            // saveMenuItem
            // 
            this.saveMenuItem.Index = 2;
            resources.ApplyResources(this.saveMenuItem, "saveMenuItem");
            this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
            // 
            // languageMenuItem
            // 
            this.languageMenuItem.Index = 3;
            this.languageMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.polishButton,
            this.englishButton});
            resources.ApplyResources(this.languageMenuItem, "languageMenuItem");
            // 
            // polishButton
            // 
            this.polishButton.Index = 0;
            resources.ApplyResources(this.polishButton, "polishButton");
            this.polishButton.Click += new System.EventHandler(this.language_Changed);
            // 
            // englishButton
            // 
            this.englishButton.Index = 1;
            resources.ApplyResources(this.englishButton, "englishButton");
            this.englishButton.Click += new System.EventHandler(this.language_Changed);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.groupBoxAdd.ResumeLayout(false);
            this.furnitureFlowLayoutPanel.ResumeLayout(false);
            this.groupBoxDisplay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.GroupBox groupBoxAdd;
        private System.Windows.Forms.FlowLayoutPanel furnitureFlowLayoutPanel;
        private System.Windows.Forms.Button buttonTable;
        private System.Windows.Forms.Button buttonBigTable;
        private System.Windows.Forms.Button buttonSofa;
        private System.Windows.Forms.Button buttonBed;
        private System.Windows.Forms.GroupBox groupBoxDisplay;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuFile;
        private System.Windows.Forms.MenuItem menuNewBlueprint;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListView listViewDisplay;
        private System.Windows.Forms.MenuItem openMenuItem;
        private System.Windows.Forms.MenuItem saveMenuItem;
        private System.Windows.Forms.MenuItem languageMenuItem;
        private System.Windows.Forms.MenuItem polishButton;
        private System.Windows.Forms.MenuItem englishButton;
        private System.Windows.Forms.Button wallButton;
    }
}

