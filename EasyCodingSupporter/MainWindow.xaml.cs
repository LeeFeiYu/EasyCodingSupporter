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
        string PuaseWord = "``";
        string EndWord = "~~";
        string ProgramOver = "~~~~~";
        string ReadLineFromLoadWordFile = "";
        string[] words = new string [DigitNumber]; // 치환부분을 위한 변수
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
        {//

            int counter = 1;
            string coding; // 합성된 문자를 저장하기 위한 변수

            // Read the file and display it line by line. 
            string mainText = tbxMain.Text;

            //System.IO.StreamReader input = new System.IO.StreamReader(pathFile); // 본문의 내용을 컨텐츠에 담음
            StreamReader LoadedWordFile = new StreamReader(tbxSelectedFile.Text); // 치환파일의 내용을 파일에 담음

            string Phrase = tbxMain.Text;
            string[] Lines = Phrase.Split('`');
            foreach (var Line in Lines)
            //foreach (string Line in mainText.Split(new string[] {"`"}, StringSplitOptions.None); //Environment.NewLine.ToCharArray())) // ` 단위로 끊어서 읽어들임
            {
                StringBuilder sb = new StringBuilder();
                bool[] isReadTextString = new bool[DigitNumber];
                //int i_counter = 0;
                while ((ReadLineFromLoadWordFile = LoadedWordFile.ReadLine()) != ProgramOver)
                {
                    int i_counter = 0;
                    while ((ReadLineFromLoadWordFile = LoadedWordFile.ReadLine()) != EndWord)// 교체될 단어들을 읽어들임.
                    {
                        words[i_counter] = LoadedWordFile.ReadLine();
                        i_counter += i_counter;

                        //for (int i_ForWords = 0; i_ForWords < DigitNumber; i_ForWords++)
                        //{
                        //    words[i_ForWords] = ReadLineFromLoadWordFile; // 교체될 단어들을 대입함
                        //    counter += counter;
                        //}
                        //if (ProgramOver == ReadLineFromLoadWordFile)
                        //{ // ~~~~~ 를 만나면 중지
                        //    for (int i = 0; i < counter; i++)
                        //    {
                        //        //words[i] = null;
                        //    }
                        //    counter = 1;
                        //    continue; // 프로그램 끝냄.
                        //}
                        //
                        //else if (ReadTextString_[0] == ReadLineFromLoadWordFile)
                        //{
                        //    words[0] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[1] == ReadLineFromLoadWordFile)
                        //{
                        //    words[1] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[2] == ReadLineFromLoadWordFile)
                        //{
                        //    words[2] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[3] == ReadLineFromLoadWordFile)
                        //{
                        //    words[3] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[4] == ReadLineFromLoadWordFile)
                        //{
                        //    words[4] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[5] == ReadLineFromLoadWordFile)
                        //{
                        //    words[5] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[6] == ReadLineFromLoadWordFile)
                        //{
                        //    words[6] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[7] == ReadLineFromLoadWordFile)
                        //{
                        //    words[7] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[8] == ReadLineFromLoadWordFile)
                        //{
                        //    words[8] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[9] == ReadLineFromLoadWordFile)
                        //{
                        //    words[9] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[10] == ReadLineFromLoadWordFile)
                        //{
                        //    words[10] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}
                        //else if (ReadTextString_[11] == ReadLineFromLoadWordFile)
                        //{
                        //    words[11] = ReadLineFromLoadWordFile;
                        //    counter += counter;
                        //}


                    }

                    for (int i = 0; i < i_counter; i++)
                    {//테스트 코드 삭제가능
                        tbxMonitor.Text = words[i];// 테스트 코드
                        //sb.AppendLine(""); // 다음줄로 이동 코드
                        //tbxMonitor.Text = sb.ToString(); // 다음줄로 이동 실행
                    }


                    string[] TempLine = new string[i_counter]; // 배열 생성. 임시로 읽어들인 단어를 각 배열에 저장하기 위해 생성
                    for (int i = 0; i < i_counter; i++)
                    {
                        if (isReadTextString[i] && ((words[i] = ReadLineFromLoadWordFile) != null))
                        {// 불값의 참과 공백이 없을 때 실행
                            for (int i_ForTempLine = 0; i_ForTempLine < i_counter; i_ForTempLine++)
                            {
                                TempLine[i_ForTempLine] = Line.Replace(ReadTextString_[i_ForTempLine], words[i_ForTempLine]); // 교환된 단어를 임시 저장 배열에 저장
                                tbxMonitor.Text = words[i_ForTempLine];// 테스트 코드
                            }
                        }
                    }



                    for (int i_isReadTextString = 0; i_isReadTextString < i_counter; i_isReadTextString++) // ReadTextString_의 길이 만큼 반복할 수 있음
                    {// 해당 문자열이 있는지 없는지의 여부를 배열에 할당하여 불값으로 전달. 상수값을 전달
                        isReadTextString[i_isReadTextString] = Line.Contains(ReadTextString_[i_isReadTextString]);
                    }

                    if (isReadTextString[0])
                    {
                        sb.Append(TempLine[0]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[0] = false;
                    }
                    else if (isReadTextString[1])
                    {
                        sb.Append(TempLine[1]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[1] = false;
                    }
                    else if (isReadTextString[2])
                    {
                        sb.Append(TempLine[2]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[2] = false;
                    }
                    else if (isReadTextString[3])
                    {
                        sb.Append(TempLine[3]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[3] = false;
                    }
                    else if (isReadTextString[4])
                    {
                        sb.Append(TempLine[4]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[4] = false;
                    }
                    else if (isReadTextString[5])
                    {
                        sb.Append(TempLine[5]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[5] = false;
                    }
                    else if (isReadTextString[6])
                    {
                        sb.Append(TempLine[6]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[6] = false;
                    }
                    else if (isReadTextString[7])
                    {
                        sb.Append(TempLine[7]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[7] = false;
                    }
                    else if (isReadTextString[8])
                    {
                        sb.Append(TempLine[8]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[8] = false;
                    }
                    else if (isReadTextString[9])
                    {
                        sb.Append(TempLine[9]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[9] = false;
                    }
                    else if (isReadTextString[10])
                    {
                        sb.Append(TempLine[10]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[10] = false;
                    }
                    else if (isReadTextString[11])
                    {
                        sb.Append(TempLine[11]);
                        tbxOutput.Text = sb.ToString(); // 변환된 내용을 아웃풋 창에 출력
                        sb.AppendLine(""); // 다음줄로 이동 코드
                        tbxOutput.Text = sb.ToString(); // 다음줄로 이동 실행
                        isReadTextString[11] = false;
                    }
                    else
                    {
                        counter = 1;
                    }

                }





            }
            LoadedWordFile.Close();

        }

        #endregion

    }
}
