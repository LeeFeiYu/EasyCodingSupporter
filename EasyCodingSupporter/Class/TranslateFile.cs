using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyCodingSupporter;

namespace EasyCodingSupporter.Class
{

    class TranslateFile
    {
        public object importPath { get; set; }
        
        public void ImportText()
        {
            LoadFileClass calling = new LoadFileClass();
            importPath = calling.loadedFile;
        }
    }
}
