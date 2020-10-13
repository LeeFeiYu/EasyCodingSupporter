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
    /// </summary>Text_Monitor
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


        #region public const string 상수 및 전역 변수 선언 부분
        //전역변수 부분
        string EndWord = "```";
        //int counter = 0;
        //string line;
        // 전역 변수 배열에 값을 할당.
        public string[] ReadTextString_ = new string[12] { "#0", "#1", "#2", "#3", "#4", "#5", "#6", "#7", "#8", "#9", "#10", "#11" };

        #endregion

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
                streamwriterName.Write(tbxOutput.Text); // xaml의 텍스트 박스를 지정.
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

        #region TranslateFile 파일 변환 부분

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {//
            //TranslateFile path = new TranslateFile();
            //MessageBox.Show(tbxSelectedFile.Text);
            int counter = 0;
            string line; // 본문을 위한 변수
            string[] words = new string[12]; // 치환부분을 위한 변수
            string coding; // 합성된 문자를 저장하기 위한 변수

            // Read the file and display it line by line.  
            System.IO.StreamReader contents = new System.IO.StreamReader(tbxMain.Text); // 본문의 내용을 컨텐츠에 담음
            System.IO.StreamReader file = new System.IO.StreamReader(@tbxSelectedFile.Text); // 치환파일의 내용을 파일에 담음

            while ((line = contents.ReadLine()) != null) // 본문의 한 줄이 공백인지 아닌지 검사
            {
                StringBuilder sb = new StringBuilder();
                bool[] isReadTextString = new bool[12];

                for (int j = 0; j < 12; j++)
                {// 해당 문자열이 있는지 없는지의 여부를 배열에 할당하여 불값으로 전달. 
                    isReadTextString[j] = line.Contains(ReadTextString_[j]);
                }
                
                for (int i = 0; i < 12; i++)
                {
                    //WordsProcess wp = new WordsProcess();
                    if ( EndWord == file.ReadLine())
                    {
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        return;
                    }

                    else if(isReadTextString[i] && ((words[i] = file.ReadLine()) != null))
                    {//불값의 참과 공백이 없을 때 실행
                        line = line.Replace(ReadTextString_[i], words[i]);
                        sb.Append(line);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                    }
                    
                }
                

            }


            file.Close();
            // System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.  
            // System.Console.ReadLine();
        }

        

        //System.IO.StreamReader file = new System.IO.StreamReader();
        //public static string TranslateFile(IEnumerable<TextContentsPart> Parts)
        //{
        //    StringBuilder textContents = new StringBuilder();
        //    [DefaultMember("Sentence")]
        //}
        #endregion

    }
}
