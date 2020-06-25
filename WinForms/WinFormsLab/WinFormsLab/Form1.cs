using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Globalization;

namespace WinFormsLab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CreateInitialBitmap();
            //this.MouseWheel += pictureBox1_MouseWheel;
            //splitContainer.Panel1.MouseWheel += pictureBox1_MouseWheel;
        }

        public void CreateInitialBitmap()
        {
            Bitmap bmp = new Bitmap(splitContainer.Panel1.ClientSize.Width, splitContainer.Panel1.ClientSize.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            pictureBox1.Image = bmp;
            pictureBox1.Refresh();
            g.Dispose();
            listViewDisplay.Focus();
        }

        public void UpdateBitmap(Size s)
        {
            Bitmap newbmp = new Bitmap(s.Width, s.Height);
            Graphics g = Graphics.FromImage(newbmp);
            g.Clear(Color.White);
            g.DrawImage(pictureBox1.Image, 0, 0);
            pictureBox1.Image = newbmp;
            pictureBox1.Refresh();
            g.Dispose();
        }

        // ------------------- Resizing --------------------
        private void splitContainer_Panel1_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                if (splitContainer.Panel1.ClientSize.Width > pictureBox1.Image.Width ||
                    splitContainer.Panel1.ClientSize.Height > pictureBox1.Image.Height)
                {
                    UpdateBitmap(splitContainer.Panel1.ClientSize);
                }
            }
        }

        // -------------------- Buttons -------------------------
        bool tableFlag = false;
        bool bigtableFlag = false;
        bool sofaFlag = false;
        bool bedFlag = false;
        bool wallFlag = false;

        private void buttonTable_Click(object sender, EventArgs e)
        {
            if (!tableFlag) {
                tableFlag = true;
                buttonTable.BackColor = Color.Moccasin;
                bigtableFlag = false;
                sofaFlag = false;
                bedFlag = false;
                wallFlag = false;
                buttonBigTable.BackColor = Color.White;
                buttonSofa.BackColor = Color.White;
                buttonBed.BackColor = Color.White;
                wallButton.BackColor = Color.White;
                DeselectElement();
            }
            else
            {
                tableFlag = false;
                buttonTable.BackColor = Color.White;
            }
        }

        private void buttonBigTable_Click(object sender, EventArgs e)
        {
            if (!bigtableFlag)
            {
                bigtableFlag = true;
                buttonBigTable.BackColor = Color.Moccasin;
                sofaFlag = false;
                tableFlag = false;
                bedFlag = false;
                wallFlag = false;
                buttonTable.BackColor = Color.White;
                buttonSofa.BackColor = Color.White;
                buttonBed.BackColor = Color.White;
                wallButton.BackColor = Color.White;
                DeselectElement();
            }
            else
            {
                bigtableFlag = false;
                buttonBigTable.BackColor = Color.White;
            }
        }

        private void buttonSofa_Click(object sender, EventArgs e)
        {
            if (!sofaFlag)
            {
                sofaFlag = true;
                buttonSofa.BackColor = Color.Moccasin;
                bigtableFlag = false;
                tableFlag = false;
                bedFlag = false;
                wallFlag = false;
                buttonBigTable.BackColor = Color.White;
                buttonTable.BackColor = Color.White;
                buttonBed.BackColor = Color.White;
                wallButton.BackColor = Color.White;
                DeselectElement();
            }
            else
            {
                sofaFlag = false;
                buttonSofa.BackColor = Color.White;
            }
        }

        private void buttonBed_Click(object sender, EventArgs e)
        {
            if (!bedFlag)
            {
                bedFlag = true;
                buttonBed.BackColor = Color.Moccasin;
                bigtableFlag = false;
                sofaFlag = false;
                tableFlag = false;
                wallFlag = false;
                buttonBigTable.BackColor = Color.White;
                buttonSofa.BackColor = Color.White;
                buttonTable.BackColor = Color.White;
                wallButton.BackColor = Color.White;
                DeselectElement();
            }
            else
            {
                bedFlag = false;
                buttonBed.BackColor = Color.White;
            }
        }

        private void wallButton_Click(object sender, EventArgs e)
        {
            if (!wallFlag)
            {
                wallFlag = true;
                wallButton.BackColor = Color.Moccasin;
                bigtableFlag = false;
                sofaFlag = false;
                tableFlag = false;
                bedFlag = false;
                buttonBigTable.BackColor = Color.White;
                buttonSofa.BackColor = Color.White;
                buttonTable.BackColor = Color.White;
                buttonBed.BackColor = Color.White;
                DeselectElement();
            }
            else
            {
                painting = false;
                wallFlag = false;
                wallButton.BackColor = Color.White;
                currentWall.MousePosition = null;
                UpdatePictureBox();
            }
        }

        // ----------------- Menu buttons --------------------
        private void menuNewBlueprint_Click(object sender, EventArgs e)
        {
            CreateInitialBitmap();
            listViewDisplay.Clear();            bedFlag = false;            sofaFlag = false;            tableFlag = false;            bigtableFlag = false;            wallFlag = false;            painting = false;            buttonBed.BackColor = Color.White;            buttonBigTable.BackColor = Color.White;            buttonTable.BackColor = Color.White;            buttonSofa.BackColor = Color.White;            wallButton.BackColor = Color.White;            elements.Clear();
            listViewDisplay.Focus();        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Blueprints files(*.bp)|*.bp";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                SerializableClass opened = null;
                //Deserialization
                FileStream fs = new FileStream(fileDialog.FileName, FileMode.Open);
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    opened = (SerializableClass)bf.Deserialize(fs);
                }
                catch (SerializationException se)
                {
                    MessageBox.Show("Failed to load the blueprint" + se.Message);
                    return;
                    //throw;
                }
                MessageBox.Show("Blueprint loaded successfully!");
                fs.Close();
                
                CreateInitialBitmap();
                listViewDisplay.Items.Clear();
                elements.Clear();
                pictureBox1.Image = opened.pictureBoxImage;
                foreach(FurnitureClass f in opened.bitmapElements)
                {
                    elements.Add(f);
                }
                foreach(ListViewItem i in opened.listViewItems)
                {
                    listViewDisplay.Items.Add(i);
                }
                UpdatePictureBox();
            }
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Blueprints files(*.bp)|*.bp";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                List<ListViewItem> items = new List<ListViewItem>();
                foreach(ListViewItem i in listViewDisplay.Items)
                {
                    items.Add(i);
                }
                SerializableClass newfile = new SerializableClass(items, pictureBox1.Image, elements);
                //Serialization
                FileStream str = File.Create(saveDialog.FileName);
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(str, newfile);
                }
                catch (SerializationException se)
                {
                    MessageBox.Show("Failed to save the blueprint" + se.Message);
                    return;
                }
                finally
                {
                    str.Close();
                }
                MessageBox.Show("Blueprint saved successfully!");
            }
        }

        // -------------------- Adding/Editing furniture ---------------------
        bool painting;
        WallClass currentWall = null;
        Point MouseClickPos = new Point();

        List<FurnitureClass> elements = new List<FurnitureClass>();
        FurnitureClass selectedElement = null;
        ListViewItem selectedLVItem = null;
        bool movingElement = false;

        private void UpdatePictureBox()
        {
            //if (elements.Count == 0) return;
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            foreach (FurnitureClass f in elements)
            {
                pictureBox1.Image = f.ChangeImageColor(pictureBox1.Image);
            }
            pictureBox1.Refresh();
            g.Dispose();
        }

        private bool FindElement(Point mousePos)
        {
            foreach (FurnitureClass f in elements)
            {
                if (f.GetType() == typeof(WallClass))
                {
                    WallClass w = f as WallClass;
                    if (w.WallClicked(mousePos))
                    {
                        if (selectedElement != null && selectedLVItem != null)
                        {
                            selectedElement.Transparency = 1.00f;
                            UpdatePictureBox();
                            selectedLVItem.Selected = false;
                        }
                        selectedElement = w;
                        selectedElement.Transparency = 0.50f;
                        //UpdatePictureBox();
                        string s = selectedElement.myText + " {X=" + selectedElement.centerLocation.X;
                        if ((selectedLVItem = listViewDisplay.FindItemWithText(s)) != null)
                        {
                            selectedLVItem.Selected = true;
                        }
                        return true;
                    }
                }
                else
                {
                    int xLimit = f.furniture.Width / 2;
                    int yLimit = f.furniture.Height / 2;
                    if (mousePos.X >= f.centerLocation.X - xLimit && mousePos.X <= f.centerLocation.X + xLimit &&
                        mousePos.Y >= f.centerLocation.Y - yLimit && mousePos.Y <= f.centerLocation.Y + yLimit)
                    {
                        if (selectedElement != null && selectedLVItem != null)
                        {
                            selectedElement.Transparency = 1.00f;
                            UpdatePictureBox();
                            selectedLVItem.Selected = false;
                        }
                        selectedElement = f;
                        selectedElement.Transparency = 0.50f;
                        //UpdatePictureBox();
                        string s = selectedElement.myText + " {X=" + selectedElement.centerLocation.X;
                        if ((selectedLVItem = listViewDisplay.FindItemWithText(s)) != null)
                        {
                            selectedLVItem.Selected = true;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private void DeselectElement()
        {
            if (selectedElement != null && selectedLVItem != null)
            {
                selectedElement.Transparency = 1.00f;
                UpdatePictureBox();
                selectedLVItem.Selected = false;
                selectedElement = null;
                selectedLVItem = null;
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            listViewDisplay.Focus();
            if (e.Button == MouseButtons.Right && painting) {
                painting = false; currentWall.MousePosition = null;
                wallFlag = false; wallButton.BackColor = Color.White;
                UpdatePictureBox(); return;
            }
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle) return;
            if (tableFlag)
            {
                string name = "Coffe table " + "{X=" + e.Location.X + ", Y=" + e.Location.Y + "}";
                ListViewItem newItem = new ListViewItem(name);
                listViewDisplay.Items.Add(newItem);
                tableFlag = false;
                buttonTable.BackColor = Color.White;

                elements.Add(new FurnitureClass(global::WinFormsLab.Properties.Resources.coffee_table, e.Location, "Coffe table"));
                UpdatePictureBox();
            }
            else if (bigtableFlag)
            {
                string name = $"Table " + "{X=" + e.Location.X + ", Y=" + e.Location.Y + "}";
                ListViewItem newItem = new ListViewItem(name);
                listViewDisplay.Items.Add(newItem);
                bigtableFlag = false;
                buttonBigTable.BackColor = Color.White;

                elements.Add(new FurnitureClass(global::WinFormsLab.Properties.Resources.table, e.Location, "Table"));
                UpdatePictureBox();
            }
            else if (bedFlag)
            {
                string name = $"Bed " + "{X=" + e.Location.X + ", Y=" + e.Location.Y + "}";
                ListViewItem newItem = new ListViewItem(name);
                listViewDisplay.Items.Add(newItem);
                bedFlag = false;
                buttonBed.BackColor = Color.White;

                elements.Add(new FurnitureClass(global::WinFormsLab.Properties.Resources.double_bed, e.Location, "Bed"));
                UpdatePictureBox();
            }
            else if (sofaFlag)
            {
                string name = $"Sofa " + "{X=" + e.Location.X + ", Y=" + e.Location.Y + "}";
                ListViewItem newItem = new ListViewItem(name);
                listViewDisplay.Items.Add(newItem);
                sofaFlag = false;
                buttonSofa.BackColor = Color.White;

                elements.Add(new FurnitureClass(global::WinFormsLab.Properties.Resources.sofa, e.Location, "Sofa"));
                UpdatePictureBox();
            }
            else if (wallFlag)
            {
                if (!painting)
                {
                    string date = $"Wall " + "{X=" + e.Location.X + ", Y=" + e.Location.Y + "}";
                    ListViewItem newItem = new ListViewItem(date);
                    listViewDisplay.Items.Add(newItem);
                    painting = true;
                    WallClass newwall = new WallClass(new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height), e.Location, "Wall");
                    elements.Add(newwall);
                    currentWall = newwall;
                }
                else
                {
                    currentWall.clickPoints.Add(e.Location);
                }
            }
            else
            {
                if (FindElement(e.Location)) { UpdatePictureBox(); return; }
                DeselectElement();
            }
           
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            listViewDisplay.Focus();
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle) return;
            if (FindElement(e.Location))
            {
                UpdatePictureBox();
                movingElement = true;
                MouseClickPos = e.Location;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (painting == true)
            {
                currentWall.MousePosition = e.Location;
                UpdatePictureBox();
            }
            if (movingElement && selectedElement != null)
            {
                if (selectedElement.GetType() == typeof(WallClass))
                {
                    WallClass w = selectedElement as WallClass;
                    for (int i = 0; i < w.clickPoints.Count; i++)
                    {
                        float x = w.clickPoints[i].X + (e.Location.X - MouseClickPos.X);
                        float y = w.clickPoints[i].Y + (e.Location.Y - MouseClickPos.Y);
                        w.clickPoints[i] = new PointF(x, y);
                        if (i == 0) w.centerLocation = new Point((int)x, (int)y);
                    }
                }
                else selectedElement.centerLocation = e.Location;
                UpdatePictureBox();
                if (selectedLVItem != null)
                    selectedLVItem.Text = selectedElement.myText + " {X=" + selectedElement.centerLocation.X + ", Y=" + selectedElement.centerLocation.Y + "}";
                MouseClickPos = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (selectedElement != null)
            {
                movingElement = false;
            }
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs he = (HandledMouseEventArgs)e;
            he.Handled = true;
            if (selectedElement != null)
            {
                //MessageBox.Show("delta: " + e.Delta);
                if (selectedElement.GetType() == typeof(WallClass))
                {
                    selectedElement.Rotation = (e.Delta / 12);
                    WallClass w = selectedElement as WallClass;
                    for (int i = 0; i < w.clickPoints.Count; i++)
                    {
                        if (i == 0) continue;
                        else w.clickPoints[i] = w.RotatePoint(w.clickPoints[i], selectedElement.Rotation);
                    }
                }
                else selectedElement.Rotation += (e.Delta / 12);
                UpdatePictureBox();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (selectedElement != null && selectedLVItem != null)
                {
                    foreach (ListViewItem liv in listViewDisplay.Items)
                    {
                        if (liv == selectedLVItem)
                        {
                            listViewDisplay.Items.Remove(liv);
                            break;
                        }
                    }
                    foreach (FurnitureClass f in elements)
                    {
                        if (f == selectedElement)
                        {
                            elements.Remove(f);
                            break;
                        }
                    }
                    UpdatePictureBox();
                   
                    selectedElement = null;
                    selectedLVItem = null;
                }
            }
        }

        // ------------------ Language Localization ---------------
        private void language_Changed(object sender, EventArgs e)
        {
            MenuItem button = (MenuItem)sender;
            if (button == englishButton)
            {
                if (!polishButton.Checked)
                {
                    englishButton.Checked = true;
                    return;
                }
                polishButton.Checked = false;
                CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("en");
            }
            else
            {
                if (!englishButton.Checked)
                {
                    polishButton.Checked = true;
                    return;
                }
                englishButton.Checked = false;
                CultureInfo.CurrentUICulture = CultureInfo.CreateSpecificCulture("pl");
            }
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
            //applying resources too all items in Main menu
            foreach (MenuItem l in mainMenu1.MenuItems)
            {
                resources.ApplyResources(l, l.Name);
            }
            //applying resources for groupBox
            resources.ApplyResources(groupBoxAdd, groupBoxAdd.Name);
            resources.ApplyResources(groupBoxDisplay, groupBoxDisplay.Name);
            foreach (ListViewItem lv in listViewDisplay.Items)
            {
                resources.ApplyResources(lv, lv.Name);
            }
        }

        
    }
}
