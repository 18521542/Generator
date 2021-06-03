using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AllMyClass;

namespace Generator
{
    public class GeneratorMacine
    {
        List<Parameters> parameters;
        List<Condition> PostCons;
        List<Condition> PreCons;
        Name name;
        Parameters result;
        String Tab = "       ";

        public GeneratorMacine(List<Parameters> parameters, Name name, List<Condition> cons, List<Condition> PreCons)
        {
            result = parameters[parameters.Count - 1];
            parameters.Remove(result);
            this.parameters = parameters;
            this.name = name;
            this.PostCons = cons;
            this.PreCons = PreCons;

        }

        public void WriteFunc_Nhap(RichTextBox rtb)
        {
            String content = "";
            //Print first line
            content += "public void Nhap_"+ name.content + "(";
            int index = 0;
            foreach(Parameters pars in parameters)
            {
                content += "ref ";
                content += pars.Type + " " + pars.name;
                if (index != parameters.Count-1)
                {
                    content += ",";
                }
                else
                {
                    content += ")";
                }
                index++;
            }

            //Print second line
            content += "\n{";
            
            //Print the body
            foreach(Parameters par in parameters)
            {
                content += "\n" + Tab + "Console.WriteLine(" + "\"Nhap " + par.name +": \");";
                content += "\n" + Tab + par.name + " = " + par.Type + ".Parse(Console.ReadLine());";
            }

            //Print end-part
            content += "\n}";

            rtb.Text = content;
        }

        public void WriteFunc_Xuat(RichTextBox rtb)
        {
            String content = "";

            content += "public void Xuat_" + name.content + "("+result.Type + " "+result.name+")";
            content += "\n{";
            content += "\n"+ Tab + "Console.WriteLine(\"Ket qua la : {0} \", " + result.name + ");";
            content += "\n}";

            rtb.Text += content;
        }

        public void WriteFunc_KiemTra(RichTextBox rtb)
        {
            rtb.Text += "public bool KiemTra_" + name.content + "(";

            int index = 0;
            foreach (Parameters pars in parameters)
            {
                rtb.Text += pars.Type + " " + pars.name;
                if (index != parameters.Count - 1)
                {
                    rtb.Text += ",";
                }
                else
                {
                    rtb.Text += ")";
                }
                index++;
            }
            rtb.Text += "\n{";
            rtb.Text += "\n" + Tab + "return ";
            if (PreCons.Count != 0)
            {
                rtb.Text += PreCons[0].content + ";";
            }
            else
            {
                rtb.Text += "true;";
            }
            rtb.Text += "\n}";
        }

        public void WriteFunc_FormalSpecification(RichTextBox rtb)
        {
            //Print firstline of function: name, params, type of params
            rtb.Text += "public " + result.Type + " " + name.content + "(";
            int index = 0;
            foreach (Parameters pars in parameters)
            {   
                rtb.Text += pars.Type + " " + pars.name;
                if (index != parameters.Count - 1)
                {
                    rtb.Text += ",";
                }
                else
                {
                    rtb.Text += ")";
                }
                index++;
            }

            //Print body
            rtb.Text += "\n{";
            rtb.Text += "\n" + Tab + result.Type + " " + result.name + " = " + result.DefaultValue + ";";
            //foreach (Parameters pars in parameters)
            //{
            //    rtb.Text += "\n" +Tab + pars.Type + " " + pars.name +" = " +pars.DefaultValue + ";";
            //    index++;
            //}

            if (PostCons.Count != 1)
            {
                foreach (Condition con in PostCons)
                {
                    String ifCons = "if (";
                    for (int i = 1; i < con.SubCondition.Count; i++)
                    {
                        ifCons += con.SubCondition[i].content;
                        if (i != con.SubCondition.Count - 1)
                            ifCons += "&&";
                    }
                    ifCons += ")";
                    rtb.Text += "\n" + Tab + ifCons;
                    rtb.Text += "\n" + Tab + Tab + con.SubCondition[0].content + ";";
                }
            }
            else
            {
                rtb.Text += "\n" + Tab + PostCons[0].content + ";";
            }

            rtb.Text += "\n" + Tab + "return " + result.name + ";";
            rtb.Text += "\n}";
        }

        public void WriteFunc_Main(RichTextBox rtb)
        {
            rtb.Text += "public static void Main(string[] args)";
            rtb.Text += "\n{";

            // init all params
            foreach (Parameters pars in parameters)
            {
                rtb.Text += "\n" + Tab + pars.Type + " " + pars.name + " = " + pars.DefaultValue + ";";
            }
            rtb.Text += "\n" + Tab + result.Type + " " + result.name + " = " + result.DefaultValue + ";";
            //==============================
            rtb.Text += "\n" + Tab + "Program p = new Program();";

            //========p.Nhap_Ten(a,b,c,...);
            rtb.Text += "\n" + Tab + "p.Nhap_" + name.content  + "(";
            String ParamsLine = "";
            int index = 0;
            foreach (Parameters pars in parameters)
            {
                ParamsLine += "ref ";
                ParamsLine += pars.name;
                if (index != parameters.Count - 1)
                {
                    ParamsLine += ",";
                }
                else
                {
                    ParamsLine += ");";
                }
                index++;
            }
            rtb.Text += ParamsLine;

            //======== if(p.KiemTra(a,b,c,...))
            rtb.Text += "\n" + Tab + "if (p.KiemTra_" + name.content + "(";
            index = 0;
            foreach(Parameters pars in parameters)
            {
                rtb.Text += pars.name;
                if (index != parameters.Count - 1)
                {
                    rtb.Text += ",";
                }
                else
                {
                    rtb.Text += "))";
                }
                index++;
            }
            rtb.Text += "\n"+ Tab + "{";

            //======== result = p.Name(a,b,c);
            rtb.Text += "\n" + Tab + Tab + result.name + " = p." + name.content + "(";
            index = 0;
            foreach (Parameters pars in parameters)
            {
                rtb.Text += pars.name;
                if (index != parameters.Count - 1)
                {
                    rtb.Text += ",";
                }
                else
                {
                    rtb.Text += ");";
                }
                index++;
            }
            rtb.Text += "\n" + Tab + Tab + "p.Xuat_" + name.content + "(" + result.name + ");"; 
            rtb.Text += "\n"+ Tab + "}";

            //================ else
            rtb.Text += "\n" + Tab + "else";
            rtb.Text += "\n" + Tab + Tab + "Console.WriteLine(\"Thong tin nhap khong hop le\");";
            rtb.Text += "\n" + Tab + "Console.ReadLine();";
            rtb.Text += "\n}";
        }

        public void Reformat(RichTextBox rtb)
        {
            List<String> mainContent = new List<String>();
            foreach(String line in rtb.Lines)
            {
                String newLine = Tab + Tab + line + "\n";
                mainContent.Add(newLine);
            }

            rtb.Text = "";

            String newContent = "";
            newContent += "using System;\n";
            newContent += "namespace FormalSpecification\n";
            newContent += "{\n";
            newContent += Tab + "public class Program\n";
            newContent += Tab + "{\n";
            foreach (var line in mainContent)
            {
                newContent += line;
            }

            newContent += Tab + "}";
            newContent += "\n}";

            rtb.Text = newContent;

        }

        public void WriteEndLine(RichTextBox rtb)
        {
            rtb.Text += "\n";
        }

        public void Build(RichTextBox richTextBox, String name)
        {
            string compiledOutput = name;

            //COMPILATION WORK
            String[] referenceAssemblies = { "System.dll", "System.Drawing.dll", "System.Windows.Forms.dll" };

            CodeDomProvider _CodeCompiler = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters _CompilerParameters = new CompilerParameters(referenceAssemblies, "")
            {
                OutputAssembly = compiledOutput,
                GenerateExecutable = true,
                GenerateInMemory = false,
                WarningLevel = 3,
                TreatWarningsAsErrors = true,
                CompilerOptions = "/optimize+ /platform:x86 /target:exe /unsafe"//!! HERE IS THE SOLUTION !!
            };

            string _Errors = null;
            try
            {
                // Invoke compilation
                CompilerResults _CompilerResults = null;
                _CompilerResults = _CodeCompiler.CompileAssemblyFromSource(_CompilerParameters, richTextBox.Text);

                if (_CompilerResults.Errors.Count > 0)
                {
                    // Return compilation errors
                    _Errors = "";
                    foreach (System.CodeDom.Compiler.CompilerError CompErr in _CompilerResults.Errors)
                    {
                        _Errors += "Line number " + CompErr.Line +
                        ", Error Number: " + CompErr.ErrorNumber +
                        ", '" + CompErr.ErrorText + ";\r\n\r\n";
                    }
                }
            }
            catch (Exception _Exception)
            {
                // Error occurred when trying to compile the code
                _Errors = _Exception.Message;
            }



            //AFTER WORK
            if (_Errors == null)
            {
                // lets run the program
                MessageBox.Show(compiledOutput + " Compiled !");

                //Start .exe
                Process.Start(compiledOutput);
            }
            else
            {
                MessageBox.Show("Error occurred during compilation : \r\n" + _Errors);
            }
        }

    }
}
