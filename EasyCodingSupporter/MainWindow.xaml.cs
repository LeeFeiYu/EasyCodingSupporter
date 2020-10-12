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
using EasyCodingSupporter.Class;




namespace EasyCodingSupporter
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private string fileName;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Run버튼을 눌렀을 때 MainTextBox에서 문자를 읽어오고 문자를 분석하여 로드된 파일에서 값들을 대입함.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        //전역변수 부분
        //int counter = 0;
        //string line;

        #region LoadFile 파일 로드 부분
        public void LoadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(); // 파일 열기 코드
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "EasyCodingSupporter|*.txt";
            openFileDialog.DefaultExt = ".txt";
            Nullable<bool> dialogOK = openFileDialog.ShowDialog();
            //string fileName = openFileDialog.FileName;
            

            if (dialogOK == true)
            {
                string strFilenames = "";
                foreach (string strFilename in openFileDialog.FileNames)
                {
                    strFilenames += ";" + strFilename;
                }
                strFilenames = strFilenames.Substring(1);
                tbxSelectedFile.Text = strFilenames;
                LoadFileClass setting = new LoadFileClass();
                setting.loadedFile = tbxSelectedFile.Text;
            }
        }
        #endregion

        #region TranslateFile 파일 변환 부분
        //System.IO.StreamReader file = new System.IO.StreamReader();
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
            var saveOK = saveFileDialog1.ShowDialog();


            //if (saveOK == true)
            if(saveFileDialog1.ShowDialog().GetValueOrDefault())
            {// txt파일로 저장.
                fileName = saveFileDialog1.FileName;
                this.Title = fileName;
                //File.WriteAllText(saveFileDialog1.FileName, txtConverter.Convert(_subtitleParts));
            }
            else return;
            using (StreamWriter streamwriterName = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                streamwriterName.Write(tbxMain.Text); // xaml의 텍스트 박스를 지정.
            }
        }

        #endregion

        #region MainPart 주요 부분
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

        #region MainTextBox new line 주 입력 공간에 엔터 입력시 개행 코드
        private void tbxMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //아래의 세 줄을 주석을 제거하면 가장 최근에 쓴 글이 맨 위로 오게 정렬이 됨.
                //tbxMain.Text += line + Environment.NewLine;
                //tbxMain.ScrollToEnd();
                //tbxMain.Focus();
                tbxMain.AcceptsReturn = true;
            }
        }
        #endregion

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //TranslateFile path = new TranslateFile();
            //MessageBox.Show(tbxSelectedFile.Text);
            int counter = 0;
            string line;
            string words;

            // Read the file and display it line by line.  
            System.IO.StreamReader contents = new System.IO.StreamReader(tbxMain.Text);
            System.IO.StreamReader file = new System.IO.StreamReader(@tbxSelectedFile.Text);

            while((line = contents.ReadLine()) != null)
            {

                while ((words = file.ReadLine()) != null)
                {
                    //System.Console.WriteLine(line);
                    tbxMain.Text = words;
                    counter++;
                }
            }


            file.Close();
            //System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            System.Console.ReadLine();
        }
       
    }
}
