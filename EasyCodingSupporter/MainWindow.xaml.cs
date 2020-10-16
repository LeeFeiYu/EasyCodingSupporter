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
            string coding; // 합성된 문자를 저장하기 위한 변수

            // Read the file and display it line by line. 

            //System.IO.StreamReader input = new System.IO.StreamReader(pathFile); // 본문의 내용을 컨텐츠에 담음

            StringBuilder sb = new StringBuilder();
            bool[] isReadTextString = new bool[DigitNumber];


        Load_word:

            StreamReader LoadedWordFile = new StreamReader(tbxSelectedFile.Text); // 치환파일의 내용을 파일에 담음
            string BufferWords;
            while ((BufferWords = LoadedWordFile.ReadLine()) != null)
            {
                tbxOutput.Text = BufferWords;
                //    int BoolEndWord = string.Compare(EndWord, BufferWords); // 읽어들인 문장과 엔드워드가 같으면 0을 반환. 다르면 앞쪽 글자가 우선일 경우 0이하, 뒷쪽 글자가 우선일 경우 0보다 큰 수 반환
                //    if (BoolEndWord != 0)
                //    {
                //        //int BoolEndWord = string.Compare(EndWord, buffer); // 읽어들인 문장과 엔드워드가 같으면 0을 반환. 다르면 앞쪽 글자가 우선일 경우 0이하, 뒷쪽 글자가 우선일 경우 0보다 큰 수 반환
                //        words[counter] = BufferWords;
                //        sb.AppendLine(""); // 다음줄로 이동 코드
                //        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                //        tbxOutput.Text = words[counter]; // 테스트 코드
                //        counter += counter;
                //    }
                //    else
                //    {
                //        break;
                //    }

            }

            //string Phrase = tbxMain.Text;
            //string[] Lines = Phrase.Split('`');
            //foreach (var Line in Lines)
            ////foreach (string Line in mainText.Split(new string[] {"`"}, StringSplitOptions.None); //Environment.NewLine.ToCharArray())) // ` 단위로 끊어서 읽어들임
            //{
            //    //string Loadedwords = buffer;
            //    //string[] TempLine = Phrase.Split('`'); // 배열 생성. 임시로 읽어들인 단어를 각 배열에 저장하기 위해 생성
            //    //foreach (var LoadedWord in Loadedwords)

            //    int BoolProgramOver = string.Compare(ProgramOver, words[0].ToString()); // 읽어들인 문장과 엔드워드가 같으면 0을 반환. 다르면 앞쪽 글자가 우선일 경우 0이하, 뒷쪽 글자가 우선일 경우 0보다 큰 수 반환
            //    if (BoolProgramOver == 0)
            //    {// 프로그램 종료
            //        tbxMonitor.Text = "프로그램 종료"; // 테스트 코드
            //        counter = 0;
            //        continue;
            //    }
                
                
            //    else if (BoolProgramOver != 0)
            //    {
            //        tbxMonitor.Text = "else if"; // 테스트 코드

            //        for (int i_isReadTextString = 0; i_isReadTextString < counter; i_isReadTextString++) // ReadTextString_의 길이 만큼 반복할 수 있음
            //        {// 해당 문자열이 있는지 없는지의 여부를 배열에 할당하여 불값으로 전달. 상수값을 전달
            //            isReadTextString[i_isReadTextString] = Line.Contains(ReadTextString_[i_isReadTextString]);
            //        }
            //        while (Line != null)
            //        {
            //            if (isReadTextString[0])
            //            {
            //                sb.Append(Line[0]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[0] = false;
            //            }
            //            else if (isReadTextString[1])
            //            {
            //                sb.Append(Line[1]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[1] = false;
            //            }
            //            else if (isReadTextString[2])
            //            {
            //                sb.Append(Line[2]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[2] = false;
            //            }
            //            else if (isReadTextString[3])
            //            {
            //                sb.Append(Line[3]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[3] = false;
            //            }
            //            else if (isReadTextString[4])
            //            {
            //                sb.Append(Line[4]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[4] = false;
            //            }
            //            else if (isReadTextString[5])
            //            {
            //                sb.Append(Line[5]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[5] = false;
            //            }
            //            else if (isReadTextString[6])
            //            {
            //                sb.Append(Line[6]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[6] = false;
            //            }
            //            else if (isReadTextString[7])
            //            {
            //                sb.Append(Line[7]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[7] = false;
            //            }
            //            else if (isReadTextString[8])
            //            {
            //                sb.Append(Line[8]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[8] = false;
            //            }
            //            else if (isReadTextString[9])
            //            {
            //                sb.Append(Line[9]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[9] = false;
            //            }
            //            else if (isReadTextString[10])
            //            {
            //                sb.Append(Line[10]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[10] = false;
            //            }
            //            else if (isReadTextString[11])
            //            {
            //                sb.Append(Line[11]);
            //                tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
            //                sb.AppendLine(""); // 다음줄로 이동 코드
            //                tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
            //                isReadTextString[11] = false;
            //            }
            //            else
            //            {
            //                counter = 0;
            //                tbxMonitor.Text = "else";
            //            }
            //        }

            //        counter = 0;

            //        goto Load_word;
            //    }








            //}





            //LoadedWordFile.Close();

        }

        #endregion

    }
}
