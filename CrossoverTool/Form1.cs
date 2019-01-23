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
        System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        private VIAHalImageDisplay display;
        private string path = System.IO.Directory.GetCurrentDirectory() + "\\img";
        public Form1()
        {
            InitializeComponent();
            display = new VIAHalImageDisplay();
            myTimer.Tick += new EventHandler(timer1_Tick);
            myTimer.Enabled = true;
            myTimer.Interval = 100;
            myTimer.Start();
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
                    {
                        int indexA= 0;
                        for (int i = 0; i < names.Length; i++)
                        {
                            indexA++;
                            HImage image = null;
                            labal = names[i];
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

        private void button1_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog finder = new OpenFileDialog();
            finder.Filter= "(点云文件)|*.asc;*.ply";
            finder.Multiselect = true;
            if (DialogResult.OK==finder.ShowDialog())
            {
                string pathdir = finder.FileName;
                string[] filename = pathdir.Split('.');
                string[] dirarr = filename[0].Split('\\');
                int dirindex = dirarr.Length-1;
                path = @"D:\" + dirarr[dirindex];
                DirectoryInfo DirInfo = new DirectoryInfo(path);
                if (!DirInfo.Exists)
                    DirInfo.Create();
                Thread thread = new Thread(() =>
                {
                    int index = 0;
                    string[] names = finder.FileNames;
                    for (int i = 0; i <names.Length; i++)
                    {
                        index++;
                        HImage img;
                        labal = names[i];
                        ConvertToHalImg.ConvertFunc(out img, names[i]);
                        img.WriteImage("tiff", 0,path+"\\"+index+".tiff");
                    }
                    MessageBox.Show("sucess" + filename[0]);
                });
                thread.Start();              
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.ShowNewFolderButton = true;
            string parhp = @"D:\图片\1.21重复性32组原始数据\1.21\产品";
            fb.SelectedPath = parhp;
            if (DialogResult.OK == fb.ShowDialog())
            {
                string pathdir = fb.SelectedPath;
                DirectoryInfo dir = new DirectoryInfo(pathdir);
                Thread thread = new Thread(() =>
                {
                    FindFile(dir);
                    MessageBox.Show("Sucess");
                });
                thread.Start();
            }            
            
         }
      static   string parhp = @"D:\图片\1.21重复性32组原始数据\1.21\产品";
       static  int Dirindex = 0;
        static string  indexA = "1";
        static string labal = string.Empty;
        static void FindFile(DirectoryInfo di)
        {

                FileInfo[] fis = di.GetFiles();
                int index = 0;
                parhp = @"D:\转换图片" + "\\" + indexA;
                DirectoryInfo dir = new DirectoryInfo(parhp);
                if (!dir.Exists)
                    dir.Create();
                for (int i = 0; i < fis.Length; i++)
                {
                    index++;
                    HImage img;
                    labal = fis[i].FullName;
                    ConvertToHalImg.ConvertFunc(out img, fis[i].FullName);
                    img.WriteImage("tiff", 0, parhp + "\\" + index + ".tiff");
                    Console.WriteLine("文件：" + fis[i].FullName);
                }
                DirectoryInfo[] dis = di.GetDirectories();

                for (; Dirindex < dis.Length; Dirindex++)
                {
                    Console.WriteLine("目录：" + dis[Dirindex].FullName);
                    if (Dirindex != 0)
                    {
                        indexA = dis[Dirindex].FullName;
                        string[] indexaa = indexA.Split('\\');
                        indexA = indexaa[indexaa.Length - 1];
                    }
                    else
                    {
                        indexA = "1";
                    }
                    FindFile(dis[Dirindex]);
                }           
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label1.Text = "正在转换的图片："+labal;
        }

    }
}
