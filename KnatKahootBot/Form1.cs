﻿using MetroFramework.Controls;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KnatKahootBot
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {

        public ChromeDriver cd;
        public List<string> AnswerInput = new List<string>();
        public List<string> Questions = new List<string>();
        public List<string> Answers = new List<string>();
        public int count = 0;
        public Form1()
        {
            InitializeComponent();
            metroTextBox1.Text = "Username";
            metroTextBox2.Text = "Password";
            metroTextBox3.Text = "Quiz Title";
            metroTextBox4.Text = "Quiz Description";
            metroTextBox1.Enter += RemoveText;
            metroTextBox2.Enter += RemoveText;
            metroTextBox3.Enter += RemoveText;
            metroTextBox4.Enter += RemoveText;
            metroTextBox1.Visible = false;
            metroTextBox2.Visible = false;
            metroButton1.Visible = false;
            metroTextBox3.Visible = false;
            metroTextBox4.Visible = false;
            metroButton2.Visible = false;
            metroCheckBox1.Visible = false;
            metroCheckBox2.Visible = false;
            metroComboBox1.SelectedValueChanged += MetroComboBox1_SelectedValueChanged;
        }

        private void MetroComboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            metroTextBox3.Visible = true;
            metroTextBox4.Visible = true;
            metroButton2.Visible = true;
            metroCheckBox1.Visible = true;
            metroCheckBox2.Visible = true;
        }

        private void RemoveText(object sender, EventArgs e)
        {
            MetroTextBox tb = (MetroTextBox)sender;
            if(tb.Text == "Password")
                tb.UseSystemPasswordChar = true;
            tb.Text = string.Empty;
            tb.Enter -= RemoveText;
        }

        public void Login(string username, string password)
        {
            cd = new ChromeDriver(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            cd.Navigate().GoToUrl("https://create.kahoot.it/login");
            var LoginBox = cd.FindElement(By.CssSelector("#username-input-field__input"));
            var PassBox = cd.FindElement(By.CssSelector("#password-input-field__input"));
            var Button = cd.FindElement(By.CssSelector("#app > div > div > main > div > div > form > button"));
            LoginBox.SendKeys(username);
            PassBox.SendKeys(password);
            Button.Click();
            CreateQuiz();
            for (int i = 0; i < AnswerInput.Count; i++)
            {
                CreateQuestion();
            }
        }

        public void CreateQuiz()
        {
            while (true)
            {
                try
                {
                    var QuizButton = cd.FindElement(By.CssSelector("#create-kahoot-type-selector-container > div > ul > li:nth-child(1) > a > div.create-kahoot-type-selector__menu-item-icon.create-kahoot-type-selector__menu-item-icon--gradient-purple"));
                    QuizButton.Click();
                    while (true)
                    {
                        var QuizTitle = cd.FindElement(By.CssSelector("#kahoot-title-input-field__input"));
                        var QuizDesc = cd.FindElement(By.CssSelector("#kahoot-description-input-field__input"));
                        var ComboBox = new SelectElement(cd.FindElement(By.CssSelector("#kahoot-audience-dropdown-list__select")));
                        QuizTitle.SendKeys(metroTextBox3.Text);
                        QuizDesc.SendKeys(metroTextBox4.Text);
                        ComboBox.SelectByIndex(1);
                        break;
                    }
                    var Final = cd.FindElement(By.CssSelector("#app > div > div > span > section > div > div > div > div > button"));
                    Final.Click();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Button not present");
                }
            }
            while(true)
            {
                try
                {
                    var Final = cd.FindElement(By.CssSelector("#app > div > div > div > div > header > div.top-bar__right > button"));
                    Final.Click();
                    break;
                }
                catch(Exception ex)
                {

                }
            }
            while(true)
            {
                try
                {
                    var Dialog = cd.FindElement(By.CssSelector("#app > div > div > span > section > div > div > div > div > button.button.takeover-tip__button.takeover-tip__button--dismiss.button--flat"));
                    Dialog.Click();
                    break;
                }
                catch(Exception ex)
                {

                }
            }
        }

        public void CreateQuestion()
        {
            File.Delete(System.Environment.GetEnvironmentVariable("USERPROFILE") + "/Downloads/test.jpg");
            while (true)
            {
                try
                {
                    var QuestionButton = cd.FindElement(By.CssSelector("#app > div > div > div > main > section:nth-child(4) > div.content-block.content-actions > div > button"));
                    QuestionButton.Click();
                    break;
                }
                catch(Exception ex)
                {
                    //Console.WriteLine(ex);
                }
            }
            while (true)
            {
                try
                {

                    var TextInput = cd.FindElement(By.CssSelector("#ql-editor-1"));
                    TextInput.SendKeys("What animal is this?");
                    List<string> Fakes = new List<string>();
                    if (metroCheckBox2.Checked)
                        Fakes = GetFakes(AnswerInput[count]);
                    else
                    {
                        Fakes = Answers;
                        Fakes.RemoveAt(count);
                    }
                    foreach (var s in Fakes)
                        Console.WriteLine("Fakes: " + s);
                    bool answer = false;
                    Random r = new Random();
                    int randomnumber = r.Next(0, 4);
                    for (int i = 0; i < 4; i++)
                    {
                        string cssSelect = "";
                        if (i == 0)
                            cssSelect = "#app > div > div > div > main > form > div:nth-child(2) > div:nth-child(1) > div:nth-child(1) > div > div > div.icon-toggle.answer-input-field__icon-toggle > label";
                        else if (i == 1)
                            cssSelect = "#app > div > div > div > main > form > div:nth-child(2) > div:nth-child(1) > div:nth-child(2) > div > div > div.icon-toggle.answer-input-field__icon-toggle > label";
                        else if (i == 2)
                            cssSelect = "#app > div > div > div > main > form > div:nth-child(2) > div:nth-child(2) > div:nth-child(1) > div > div > div.icon-toggle.answer-input-field__icon-toggle > label";
                        else if (i == 3)
                            cssSelect = "#app > div > div > div > main > form > div:nth-child(2) > div:nth-child(2) > div:nth-child(2) > div > div > div.icon-toggle.answer-input-field__icon-toggle > label";
                        var Input = cd.FindElement(By.CssSelector("#ql-editor-" + (i + 2)));            
                        Console.WriteLine("Fakes Count: " + Fakes.Count);

                        if (answer)
                        {
                            Input.SendKeys(Fakes[0]);
                            Fakes.RemoveAt(0);
                        }
                        else
                        {
                            if (i == randomnumber)
                            {
                                Input.SendKeys(AnswerInput[count]);
                                var CheckBox = cd.FindElement(By.CssSelector(cssSelect));
                                CheckBox.Click();
                                answer = true;
                            }
                            else if (Fakes.Count == 0)
                            { 
                                Input.SendKeys(AnswerInput[count]);
                                var CheckBox = cd.FindElement(By.CssSelector(cssSelect));
                                CheckBox.Click();
                                answer = true;
                            }
                            else
                            {
                                Input.SendKeys(Fakes[0]);
                                Fakes.RemoveAt(0);
                            }
                        }
                    }
                    if (metroCheckBox1.Checked)
                    {
                        while (true)
                        {
                            try
                            {
                                string html = GetHtmlCode();
                                List<string> urls = GetUrls(html);
                                byte[] image = GetImage(urls[r.Next(0, 4)]);
                                using (var ms = new MemoryStream(image))
                                {
                                    Image.FromStream(ms).Save(System.Environment.GetEnvironmentVariable("USERPROFILE") + "/Downloads/test.jpg");
                                }
                                var Test = cd.FindElement(By.CssSelector("#image-uploader"));
                                Test.SendKeys(System.Environment.GetEnvironmentVariable("USERPROFILE") + "/Downloads/test.jpg");
                                var holdImage = cd.FindElement(By.CssSelector("#app > div > div > div > main > form > div.grid.grid--gutter-offset > div:nth-child(2) > div > div > div.media-uploader__wrap > div > div > figure"));
                                break;
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine(ex);
                            }
                        }
                    }
                    break;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            count++;
            var End = cd.FindElement(By.CssSelector("#app > div > div > div > div > header > div.top-bar__right > button"));
            End.Click();
        }

        public void GenerateAnswers()
        {
            
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Login(metroTextBox1.Text, metroTextBox2.Text);
        }

        private string GetHtmlCode()
        {

            string url = "https://www.google.com/search?q=" + AnswerInput[count] + "&tbm=isch";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }

        private List<string> GetUrls(string html)
        {
            var urls = new List<string>();

            int ndx = html.IndexOf("\"ou\"", StringComparison.Ordinal);

            while (ndx >= 0)
            {
                ndx = html.IndexOf("\"", ndx + 4, StringComparison.Ordinal);
                ndx++;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                string url = html.Substring(ndx, ndx2 - ndx);
                urls.Add(url);
                ndx = html.IndexOf("\"ou\"", ndx2, StringComparison.Ordinal);
            }
            return urls;
        }

        private byte[] GetImage(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return null;
                using (var sr = new BinaryReader(dataStream))
                {
                    byte[] bytes = sr.ReadBytes(100000000);

                    return bytes;
                }
            }

            return null;
        }

        public List<string> GetFakes(string real)
        {
            string[] split = real.Split(' ');
            List<string> fakes = new List<string>();
            foreach(string s in AnswerInput)
                for (int i = 0; i < split.Length; i++)
                    if (s.Contains(split[i]) && s != real)
                        fakes.Add(s);
            while(!(fakes.Count >= 3))
            {
                Random r = new Random();
                string hold = AnswerInput[r.Next(0, AnswerInput.Count)];
                Console.WriteLine("Loop: " + hold);
                if(hold != real && !fakes.Contains(hold))
                {
                    fakes.Add(hold);
                    Console.WriteLine("Add");
                }
                Console.WriteLine(fakes.Count);
            }
            return new List<string>(new string[] { fakes[0], fakes[1], fakes[2] });
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowserDialog1 = new OpenFileDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader openFile = new StreamReader(folderBrowserDialog1.FileName, Encoding.UTF8);
                string line;
                while ((line = openFile.ReadLine()) != null)
                {
                    bool foundChar = false;
                    List<char> answer = new List<char>();
                    List<char> question = new List<char>();
                    if(metroComboBox1.SelectedIndex == 0)
                        AnswerInput.Add(line);
                    else if(metroComboBox1.SelectedIndex == 1)
                    {
                        foreach(char c in line)
                        {
                            if (c == '-')
                            {
                                foundChar = true;
                                continue;
                            }
                            if (!foundChar)
                                question.Add(c);
                            else
                                answer.Add(c);
                        }
                    }
                    else if(metroComboBox1.SelectedIndex == 2)
                    {
                        foreach(char c in line)
                        {
                            if(c == '-')
                            {
                                foundChar = true;
                                continue;
                            }
                            if (!foundChar)
                                answer.Add(c);
                            else
                                question.Add(c);
                        }
                    }
                }
                openFile.Close();
            }
            metroButton1.Visible = true;
            metroTextBox1.Visible = true;
            metroTextBox2.Visible = true;
            foreach(var s in AnswerInput)
            {
                Console.WriteLine(s);
            }
        }
    }
}
