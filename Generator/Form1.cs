using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;  
using AllMyClass;
using SoTest;

namespace Generator
{
    public partial class Form1 : Form
    {
        public Name FunctionName;
        public Controller ctrl;
        private string InstancePath = "";
        //private bool isNew = true;
        private List<Parameters> parameters;
        private List<Condition> conditions;
        private List<Condition> Preconditions;
        private GeneratorMacine generator;


        public Form1()
        {
            ctrl = new Controller();
            FunctionName = new Name();

            parameters = new List<Parameters>();
            conditions = new List<Condition>();
            Preconditions = new List<Condition>();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.label1.Text = MyTest.connected;
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                Open();
                Generate(richTextBox1.Lines[0], richTextBox1.Lines[1], richTextBox1.Lines[2]);
            }
            catch(Exception ex)
            {
                Console.Write(ex);
            }
        }

        private void Open()
        {
            using (OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter =
                "Text File|*.txt",
                ValidateNames = true,
                Multiselect = false
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string text = File.ReadAllText(ofd.FileName);
                    //this.contentOfInputFile = File.ReadAllLines(ofd.FileName);

                    //string FirstLine = contentOfInputFile[0];
                    //string SecondLine = contentOfInputFile[1];
                    //string ThirdLine = contentOfInputFile[2];

                    //ctrl.HandleFirstLine(FirstLine, ref this.parameters, ref this.FunctionName);
                    //ctrl.HandleSecondLine(SecondLine, ref this.Preconditions);
                    //ctrl.HandleThirdLine(ThirdLine, ref this.conditions);
                    //this.MyDebug();
                    richTextBox1.Text = text;

                    this.InstancePath = ofd.FileName.ToString();
                    //this.isNew = false;
                }
            }
        }

        private void Generate(String Line1, String Line2, String Line3)
        {
            ctrl.HandleFirstLine(Line1, ref this.parameters, ref this.FunctionName);
            ctrl.HandleSecondLine(Line2, ref this.Preconditions);
            ctrl.HandleThirdLine(Line3, ref this.conditions);

            generator = new GeneratorMacine(this.parameters, this.FunctionName, this.conditions, this.Preconditions);

            generator.WriteFunc_Nhap(richTextBox2);
            generator.WriteEndLine(richTextBox2);
            generator.WriteEndLine(richTextBox2);

            generator.WriteFunc_Xuat(richTextBox2);
            generator.WriteEndLine(richTextBox2);
            generator.WriteEndLine(richTextBox2);

            generator.WriteFunc_KiemTra(richTextBox2);
            generator.WriteEndLine(richTextBox2);
            generator.WriteEndLine(richTextBox2);

            generator.WriteFunc_FormalSpecification(richTextBox2);
            generator.WriteEndLine(richTextBox2);
            generator.WriteEndLine(richTextBox2);

            generator.WriteFunc_Main(richTextBox2);

            generator.Reformat(richTextBox2);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            clear();
        }

        public void clear()
        {
            this.parameters.Clear();
            this.conditions.Clear();
            this.Preconditions.Clear();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";

            clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
            Generate(richTextBox1.Lines[0], richTextBox1.Lines[1], richTextBox1.Lines[2]);
        }

        private void btnBuildClick(object sender, EventArgs e)
        {
            generator.Build(richTextBox2,tbExename.Text);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            CheckKeyword("&&", Color.Brown, 0, richTextBox1);
            CheckKeyword("||", Color.SandyBrown, 0, richTextBox1);
            CheckKeyword("pre", Color.Blue, 0, richTextBox1);
            CheckKeyword("post", Color.Blue, 0, richTextBox1);
            CheckKeyword(":", Color.Red, 1, richTextBox1);
            CheckKeyword("har", Color.Red, 0, richTextBox1);
        }

        private void CheckKeyword(string word, Color color, int startIndex, RichTextBox Rchtxt)
        {
            if (Rchtxt.Text.Contains(word))
            {
                int index = -1;
                int selectStart = Rchtxt.SelectionStart;

                while ((index = Rchtxt.Text.IndexOf(word, (index + 1))) != -1)
                {
                    Rchtxt.Select((index + startIndex), word.Length);
                    Rchtxt.SelectionColor = color;
                    Rchtxt.Select(selectStart, 0);
                    Rchtxt.SelectionColor = Color.Black;
                }
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            String[] blueChars = { "using", "namespace", "public", "class", "void", "ref", "float",
            "string","bool","return","if","else","static", "new"};
            foreach(String word in blueChars)
            {
                CheckKeyword(word, Color.Blue, 0, richTextBox2);
            }

            CheckKeyword("Console", Color.BlueViolet,0,richTextBox2);
           
            CheckKeyword("Program", Color.Brown, 0, richTextBox2);
        }

        private void generatingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear();
            Generate(richTextBox1.Lines[0], richTextBox1.Lines[1], richTextBox1.Lines[2]);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Win32.FreeConsole();
        }

    }
}
