using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Microsoft.Win32;

namespace EasyCodingSupporter.Class
{
    public class LoadFileClass
    {
        public void LoadFile()
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog(); // 파일 열기 코드
            fileDialog.Multiselect = false;
            fileDialog.Filter = "EasyCodingSupporter|*.txt";
            Nullable<bool> dialogOK = fileDialog.ShowDialog();

            if(dialogOK == true)
            {
                foreach (string strFilename in fileDialog.FileNames)
                {
                    
                }
            }
        }

    }   
}
