using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picture_viewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string currentDirectory="";
        string currentMode = "single";
        ImageList imgs_slide = new ImageList();
        private void Button_chooseDirectory_Click(object sender, EventArgs e)
        {
            clear_elements();
            listBoxImages.Items.Clear();
            pictureBoxPreview.Image = null;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                foreach (var file in openFileDialog.FileNames)
                {
                    try
                    {
                        if (Path.GetExtension(file).ToLower() == ".png" || Path.GetExtension(file).ToLower() == ".jpg" || Path.GetExtension(file).ToLower() == ".jpeg")
                        {
                            listBoxImages.Items.Add(Path.GetFileName(file));
                        }
                        else
                        {
                            throw new Exception("error .. please select only images ");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        listBoxImages.Items.Clear();
                        return;
                    }
                    currentDirectory = Path.GetDirectoryName(file);
                }
                if (currentMode == "slide")
                    slide_mode();
            }
           
            
        }
        private void clear_elements()
        {
            listView1.Clear();
            listBoxImages.Items.Clear();
            imgs_slide.Images.Clear();
            pictureBoxPreview.Image = null;
            timer1.Enabled = false;
            statusStrip1.Visible = false;
        }
        private void single_mode()
        {
            listView1.Visible = false;
            try
            {
                String selectedImage = listBoxImages.SelectedItem.ToString();
                if (!string.IsNullOrEmpty(selectedImage) && !string.IsNullOrEmpty(currentDirectory))
                {
                    String fullPath = Path.Combine(currentDirectory, selectedImage);
                    pictureBoxPreview.Image = Image.FromFile(fullPath);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void multi_mode()
        {
            listView1.Visible = true;
            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.View = View.Details;
            listView1.Columns.Add("image");
            listView1.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
            ImageList imgs = new ImageList();
            imgs.ImageSize = new Size(250, 200);
            foreach (var img in listBoxImages.SelectedItems)
            {
                String fullPath = Path.Combine(currentDirectory, img.ToString());
                imgs.Images.Add(Image.FromFile(fullPath));
            }
            listView1.SmallImageList = imgs;
            int index = 0;
            foreach (var img in listBoxImages.SelectedItems)
            {
                listView1.Items.Add(img.ToString(), index++);
            }
            
        }
        private void slide_mode()
        {
            listView1.Visible = false;
            pictureBoxPreview.Visible = true;
            statusStrip1.Visible = true;
            toolStripStatusLabel1.Text = listBoxImages.Items[0].ToString();
            string fullpath = Path.Combine(currentDirectory, listBoxImages.Items[0].ToString());
            pictureBoxPreview.Image = Image.FromFile(fullpath);
            timer1.Interval = 100;
            timer1.Enabled = true;

        }

        private void ListBoxImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            
           if (currentMode=="single")
           {
               single_mode();
           }
           else if(currentMode=="multi")
           {
                multi_mode();
           }
           
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

        private void SinglePiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            clear_elements();
            currentMode = "single";
            listBoxImages.SelectionMode = SelectionMode.One;
        }

        private void MultiPictureModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear_elements();
            currentMode = "multi";
            listBoxImages.SelectionMode = SelectionMode.MultiSimple;
        }

        private void SlideShowModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear_elements();
            currentMode = "slide";
            listBoxImages.SelectionMode = SelectionMode.None;
        }
        int count = 1;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            count %= listBoxImages.Items.Count;
            toolStripStatusLabel1.Text = listBoxImages.Items[count].ToString();
            string fullpath = Path.Combine(currentDirectory, listBoxImages.Items[count].ToString());
            pictureBoxPreview.Image.Dispose();
            pictureBoxPreview.Image = Image.FromFile(fullpath);
            count++;
        }
    }
}
