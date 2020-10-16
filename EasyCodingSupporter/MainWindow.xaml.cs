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
using System.Windows.Threading;

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
        int PuaseWord = 8888;
        string EndWord = "~~";
        string ProgramOver = "~~~~~";
        string ReadLineFromLoadWordFile = "";
        string[] words = new string[DigitNumber]; // 치환부분을 위한 변수
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
            if (saveOK == true)
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
        private void tbxMonitor_KeyUp(object sender, KeyEventArgs e)
        {
            //아래의 세 줄을 주석을 제거하면 가장 최근에 쓴 글이 맨 위로 오게 정렬이 됨.
            //tbxMain.Text += line + Environment.NewLine;
            //tbxMain.ScrollToEnd();
            //tbxMain.Focus();
            tbxOutput.AcceptsReturn = true;
        }

        #endregion

        #region TranslateFile 파일 변환 부분

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            int counter = 0;
            
            StringBuilder sb = new StringBuilder();
            bool[] isReadTextString = new bool[DigitNumber];

            StreamReader LoadedWordFile = new StreamReader(tbxSelectedFile.Text); // 치환파일의 내용을 파일에 담음
            string BufferWords;
            sb.AppendLine(""); // 다음줄로 이동 코드
            bool isContinue = true; // 문장이 계속 되는 한 계속 반복하게

            
            string[] strarray = tbxMain.Text.Split(Environment.NewLine.ToCharArray());
            List<string> strList = new List<string>();
            foreach (var item in strarray)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    strList.Add(item);
                }
            }

            //while (isContinue==true)
            //{
                while ((BufferWords = LoadedWordFile.ReadLine()) != null)
                {
                    //tbxOutput.Text += BufferWords;
                    int BoolEndWord = string.Compare(EndWord, BufferWords); // 읽어들인 문장과 엔드워드가 같으면 0을 반환. 다르면 앞쪽 글자가 우선일 경우 0이하, 뒷쪽 글자가 우선일 경우 0보다 큰 수 반환
                    if (BoolEndWord != 0)
                    {
                        words[counter] = BufferWords;
                        tbxMonitor.Text += words[counter]; // 테스트 코드
                        tbxMonitor.Text += sb.ToString(); // 다음줄로 이동 실행
                        counter++;
                    }
                    else
                    {
                        break;
                    }

                }

                foreach (var Lines in strList)
                {
                    int BoolProgramOver = string.Compare(ProgramOver, words[0].ToString()); // 읽어들인 문장과 엔드워드가 같으면 0을 반환. 다르면 앞쪽 글자가 우선일 경우 0이하, 뒷쪽 글자가 우선일 경우 0보다 큰 수 반환
                    if (BoolProgramOver == 0)
                    {// 프로그램 종료
                        tbxMonitor.Text = "프로그램 종료"; // 테스트 코드
                        counter = 0;
                        continue;
                    }
                    else if (BoolProgramOver != 0)
                    {
                        tbxMonitor.Text += "첫 번째"; // 테스트 코드

                        for (int i_isReadTextString = 0; i_isReadTextString < counter; i_isReadTextString++) // ReadTextString_의 길이 만큼 반복할 수 있음
                        {// 해당 문자열이 있는지 없는지의 여부를 배열에 할당하여 불값으로 전달. 상수값을 전달
                            isReadTextString[i_isReadTextString] = Lines.Contains(ReadTextString_[i_isReadTextString]);
                            //tbxOutput.Text += ReadTextString_[i_isReadTextString];// 테스트 코드
                        }


                        tbxOutput.Text += Lines.Replace("#0", words[0])
                            .Replace("#1", words[1])
                            .Replace("#2", words[2])
                            .Replace("#3", words[3])
                            .Replace("#4", words[4])
                            .Replace("#5", words[5])
                            .Replace("#6", words[6])
                            .Replace("#7", words[7])
                            .Replace("#8", words[8])
                            .Replace("#9", words[9])
                            .Replace("#10", words[10])
                            .Replace("#11", words[11]);
                        tbxOutput.Text += Environment.NewLine;
                    }
                }


                for (int i = 0; i < counter; i++)
                {// 프로그램 단어의 끝 단어가 나오지 않으면 계속 반복하게 설정.
                    int BoolProgramOver = string.Compare(ProgramOver, words[i]); // 읽어들인 문장과 엔드워드가 같으면 0을 반환. 다르면 앞쪽 글자가 우선일 경우 0이하, 뒷쪽 글자가 우선일 경우 0보다 큰 수 반환
                    if (BoolProgramOver == 0)
                    {
                        isContinue = false;
                    }
                    else
                    {
                        isContinue = true;
                    }
                }

            //}

            
            
            //LoadedWordFile.Close();

        }

        #endregion

    }
}
