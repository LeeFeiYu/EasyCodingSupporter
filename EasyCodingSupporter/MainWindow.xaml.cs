using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasyCodingSupporter.Class;
using System.IO;
using Microsoft.Win32;
using System.Reflection;

namespace EasyCodingSupporter
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Run버튼을 눌렀을 때 MainTextBox에서 문자를 읽어오고 문자를 분석하여 로드된 파일에서 값들을 대입함.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        #region LoadFile 파일 로드 부분
        public void LoadFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog(); // 파일 열기 코드
            fileDialog.Multiselect = true;
            fileDialog.Filter = "EasyCodingSupporter|*.txt";
            fileDialog.DefaultExt = ".txt";
            Nullable<bool> dialogOK = fileDialog.ShowDialog();
            string filename = fileDialog.FileName;
            

            if (dialogOK == true)
            {
                string strFilenames = "";
                foreach (string strFilename in fileDialog.FileNames)
                {
                    strFilenames += ";" + strFilename;
                }
                strFilenames = strFilenames.Substring(1);
                tbxSelectedFile.Text = strFilenames;
            }
        }
        #endregion

        #region TranslateFile 파일 변환 부분
        //public static string TranslateFile(IEnumerable<TextContentsPart> Parts)
        //{
        //    StringBuilder textContents = new StringBuilder();
        //    [DefaultMember("Sentence")]
        //}
        #endregion

        #region SaveFile 파일 저장 부분
        public void SaveFile()
        {
            //File.WriteAllText("test.txt", txtConverter.Convert(_subtitleParts));
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt file|*.txt";
            saveFileDialog1.Title = "txt저장";
            var result = saveFileDialog1.ShowDialog();


            //if (result == true)
            //{// txt파일로 저장. 

            //    File.WriteAllText(saveFileDialog1.FileName, txtConverter.Convert(_subtitleParts));
            //}
        }

        #endregion

        #region MainProgrammingPart 주요 부분
        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            //LoadFileClass loadFile = new LoadFileClass();
            LoadFile();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbxMain.Text) == false)
            {
                //RunCodingClass File = new RunCodingClass();
                //TranslateFile();
                SaveFile();
            }
            else
            {
                MessageBox.Show("텍스트 박스에 내용이 없습니다", "입력에러");
            }
        }
        #endregion
    }
}
