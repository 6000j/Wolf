﻿using Celtic_Guardian;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wolf
{
    public static class ContextMenuFunctions
    {
        public static void ExtractFile(ListViewItem Item, string ExportPath = "")
        {
            Task.Run(() =>
            {

                var FileToExport = Form1.Data.First(File => File.Item3.Contains(Item.Text));

                Directory.CreateDirectory("Exported");
                var BytesToRead = 0L;
                foreach (var File in Form1.Data)
                {
                    if (File.Item3 == FileToExport.Item3)
                        break;

                    var AligneSize = Utilities.IsAligned(File.Item1);
                    BytesToRead = AligneSize + BytesToRead;

                    if (File.Item3 == FileToExport.Item3)
                        break;
                }
                using (var BReader = new BinaryReader(File.Open($"{Form1.InstallDir}\\YGO_DATA.dat", FileMode.Open,
                    FileAccess.Read)))
                {
                    if (ExportPath == "")
                        ExportPath = FileToExport.Item3;

                    new FileInfo("YGO_DATA\\" + FileToExport.Item3).Directory?.Create();
                    using (var Writer = new BinaryWriter(File.Open("YGO_DATA\\" + ExportPath, FileMode.Create, FileAccess.Write)))
                    {
                        BReader.BaseStream.Position = BytesToRead;
                        Writer.Write(BReader.ReadBytes(FileToExport.Item1));
                        BReader.Dispose();
                    }
                }
            });
        }

        public static void ViewImage(ListViewItem Item)
        {
            var RandFileName = new Random(DateTime.Now.Millisecond).Next();
            ExtractFile(Item, $"{Path.GetTempPath()}\\{RandFileName}");
            var Viewer = new ImageViewer($"{Path.GetTempPath()}\\{RandFileName}");
            Viewer.ShowDialog();
        }
    }
}