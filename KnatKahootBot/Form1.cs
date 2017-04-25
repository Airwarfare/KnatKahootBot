using MetroFramework.Controls;
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
        public List<string> testinput = new List<string>();
        public int count = 0;
        public Form1()
        {
            InitializeComponent();
            metroTextBox1.Text = "Username";
            metroTextBox2.Text = "Password";
            metroTextBox1.Enter += MetroTextBox1_GotFocus;
            metroTextBox2.Enter += MetroTextBox2_GotFocus;
            metroTextBox1.Visible = false;
            metroTextBox2.Visible = false;
            metroButton1.Visible = false;
        }

        private void MetroTextBox2_GotFocus(object sender, EventArgs e)
        {
            MetroTextBox tb = (MetroTextBox)sender;
            tb.Text = string.Empty;
            tb.UseSystemPasswordChar = true;
            metroTextBox2.Enter -= MetroTextBox2_GotFocus;
        }

        private void MetroTextBox1_GotFocus(object sender, EventArgs e)
        {
            MetroTextBox tb = (MetroTextBox)sender;
            tb.Text = string.Empty;
            metroTextBox1.Enter -= MetroTextBox1_GotFocus;
        }

        public void Login(string username, string password)
        {
            cd = new ChromeDriver(@"C:\Users\jorda\Desktop\");
            cd.Navigate().GoToUrl("https://create.kahoot.it/login");
            var LoginBox = cd.FindElement(By.CssSelector("#username-input-field__input"));
            var PassBox = cd.FindElement(By.CssSelector("#password-input-field__input"));
            var Button = cd.FindElement(By.CssSelector("#app > div > div > main > div > div > form > button"));
            LoginBox.SendKeys(username);
            PassBox.SendKeys(password);
            Button.Click();
            CreateQuiz();
            for (int i = 0; i < testinput.Count; i++)
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
                        QuizTitle.SendKeys("Temp Title");
                        QuizDesc.SendKeys("Temp Desc");
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
            File.Delete(@"C:\Users\jorda\Downloads\test.jpg");
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
                    TextInput.SendKeys("What Animal Is this?");
                    var Fakes = GetFakes(testinput[count]);
                    foreach (var s in Fakes)
                        Console.WriteLine("Fakes: " + s);
                    bool answer = false;
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
                        Random r = new Random(590);
                        if (answer)
                        {
                            Input.SendKeys(Fakes[0]);
                            Fakes.RemoveAt(0);
                        }
                        else
                        {
                            if (r.Next(0, 4) == 2)
                            {
                                Input.SendKeys(testinput[count]);
                                var CheckBox = cd.FindElement(By.CssSelector(cssSelect));
                                CheckBox.Click();
                                answer = true;
                            }
                            else if (Fakes.Count == 0)
                            { 
                                Input.SendKeys(testinput[count]);
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
                    string html = GetHtmlCode();
                    List<string> urls = GetUrls(html);
                    byte[] image = GetImage(urls[0]);
                    using (var ms = new MemoryStream(image))
                    {
                        Image.FromStream(ms).Save(@"C:\Users\jorda\Downloads\test.jpg");
                    }
                    var Test = cd.FindElement(By.CssSelector("#image-uploader"));
                    Test.SendKeys(@"C:\Users\jorda\Downloads\test.jpg");
                    break;
                }
                catch(Exception ex)
                {

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
            string url = "https://www.google.com/search?q=" + testinput[count] + "&tbm=isch";
            string data = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
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
            int ndx = html.IndexOf("class=\"images_table\"", StringComparison.Ordinal);
            ndx = html.IndexOf("<img", ndx, StringComparison.Ordinal);

            while (ndx >= 0)
            {
                ndx = html.IndexOf("src=\"", ndx, StringComparison.Ordinal);
                ndx = ndx + 5;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                string url = html.Substring(ndx, ndx2 - ndx);
                urls.Add(url);
                ndx = html.IndexOf("<img", ndx, StringComparison.Ordinal);
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
                    byte[] bytes = sr.ReadBytes(100000);

                    return bytes;
                }
            }
        }

        public List<string> GetFakes(string real)
        {
            string[] split = real.Split(' ');
            List<string> fakes = new List<string>();
            foreach(string s in testinput)
                for (int i = 0; i < split.Length; i++)
                    if (s.Contains(split[i]) && s != real)
                        fakes.Add(s);
            while(!(fakes.Count >= 3))
            {
                Random r = new Random();
                string hold = testinput[r.Next(0, testinput.Count)];
                Console.WriteLine("Loop: " + hold);
                if(!fakes.Contains(hold))
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
                    testinput.Add(line);
                }

                openFile.Close();
            }
            metroButton1.Visible = true;
            metroTextBox1.Visible = true;
            metroTextBox2.Visible = true;
            foreach(var s in testinput)
            {
                Console.WriteLine(s);
            }
        }
    }
}
