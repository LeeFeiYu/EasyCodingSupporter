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
        string EndWord = "~~";
        //int counter = 0;
        //string line;
        // 전역 변수 배열에 값을 할당.
        public const int DigitNumber = 12; // ☆☆치환 단어의 개수를 설정할 수 있음☆☆

        public string[] ReadTextString_ = new string[DigitNumber] { "#0", "#1", "#2", "#3", "#4", "#5", "#6", "#7", "#8", "#9", "#10", "#11" };//☆☆ 치환 단어의 개수에 따라 증감 시켜야 함 ☆☆

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
            if(saveOK == true)
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

        #region new line 입력 공간에 엔터 입력시 개행 코드
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
        private void tbxOutput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //아래의 세 줄을 주석을 제거하면 가장 최근에 쓴 글이 맨 위로 오게 정렬이 됨.
                //tbxMain.Text += line + Environment.NewLine;
                //tbxMain.ScrollToEnd();
                //tbxMain.Focus();
                tbxOutput.AcceptsReturn = true;
            }
        }

        #endregion

        #region TranslateFile 파일 변환 부분

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {//
         
            int counter = 0;
            string[] words = new string[DigitNumber]; // 치환부분을 위한 변수
            string coding; // 합성된 문자를 저장하기 위한 변수

            // Read the file and display it line by line. 
            string mainText = tbxMain.Text;

            //System.IO.StreamReader input = new System.IO.StreamReader(pathFile); // 본문의 내용을 컨텐츠에 담음
            System.IO.StreamReader output = new System.IO.StreamReader(tbxSelectedFile.Text); // 치환파일의 내용을 파일에 담음

            // 할 일★★★★★★★★★★★★★★★이하 부분의 수정으로 파일의 아래 문장을 계속 읽어들이게 해야함.★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★★

            string Phrase = tbxMain.Text;
            string[] Lines = Phrase.Split('`');
            foreach (var Line in Lines)
            //foreach (string Line in mainText.Split(new string[] {"`"}, StringSplitOptions.None); //Environment.NewLine.ToCharArray())) // ` 단위로 끊어서 읽어들임
            {
                StringBuilder sb = new StringBuilder();
                bool[] isReadTextString = new bool[DigitNumber];

                for (int j = 0; j < ReadTextString_.Length; j++) // ReadTextString_의 길이 만큼 반복할 수 있음
                {// 해당 문자열이 있는지 없는지의 여부를 배열에 할당하여 불값으로 전달. 
                    isReadTextString[j] = Line.Contains(ReadTextString_[j]);
                }

                for (int i = 0; i < ReadTextString_.Length; i++)// ReadTextString_의 길이 만큼 반복할 수 있음
                {
                    var readLine = output.ReadLine();
                    //WordsProcess wp = new WordsProcess();
                    if (readLine == EndWord)
                    {
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        return;
                    }

                    else if (isReadTextString[i] && ((words[i] = readLine) != null))
                    {//불값의 참과 공백이 없을 때 실행
                        var tempLine = Line.Replace(ReadTextString_[i], words[i]);
                        sb.Append(tempLine);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                    }
                    else
                    {
                        return;
                    }

                }


            }


            output.Close();
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
