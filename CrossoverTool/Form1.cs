using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VIA.SmartVision.DisplayHalImage;
using VIA.SmartVision.HalconFunc;

namespace CrossoverTool
{
    public partial class Form1 : Form
    {
        private VIAHalImageDisplay display;
        private string path = System.IO.Directory.GetCurrentDirectory() + "\\img";
        public Form1()
        {
            InitializeComponent();
            display = new VIAHalImageDisplay();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowNewFolderButton = true;
            //OpenFileDialog finder = new OpenFileDialog();
            //finder.Filter = "(点云文件)|*.asc;*.ply";
            //finder.Multiselect = true;
            string parhp = @"D:\图片\1.21重复性32组原始数据\1.21\产品";
            fb.SelectedPath = parhp;
            if (DialogResult.OK == fb.ShowDialog())
            {
                string pathdir = fb.SelectedPath;
                string[] dirarr = pathdir.Split('\\');
                             Thread thread = new Thread(() =>
                {             
                    int fileIndex = 0;
                    string[] safeNames=new string[8] ;
                    string[] names = new string[8];
                    string dirName = fb.SelectedPath;                  
                    DirectoryInfo directory = new DirectoryInfo(dirName);
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        safeNames[fileIndex] = file.Name;
                        names[fileIndex] =file.FullName ;
                        fileIndex++;
                    }
                    DirectoryInfo directory1 = new DirectoryInfo(path);
                    if (!directory1.Exists)//不存在
                        directory1.Create();

                    // string[] safeNames = finder.SafeFileNames;
                    //string[] names = finder.FileNames;
                    {
                        int indexA= 0;
                        for (int i = 0; i < names.Length; i++)
                        {
                            indexA++;
                            HImage image = null;
                            ConvertToHalImg.ConvertFunc(out image, names[i]);
                            //HalPosition index = display.GetIndex(safeNames[i]);
                            //if (HalPosition.None != index)
                            //{          
                            string pathaa = path + "\\" + dirarr[5];
                            DirectoryInfo directory2 = new DirectoryInfo(pathaa);
                            if (!directory2.Exists)//不存在
                                directory2.Create();
                            image.WriteImage("tiff", 0,  pathaa+"\\"+ indexA + ".tiff");
                            //}
                        }
                    }
                    MessageBox.Show("sucess"+ dirarr[5]);
                });
                thread.Start();
            }
        }
    }
}
