// <author>Xavier Morera & Maikol Barrantes</author>
// <date>05/30/2016</date>
// <summary>App to write text while you talk</summary>
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutomaticWriter
{
    public partial class Form1 : Form
    {
        public Process p;
        public IntPtr h;
            
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("User32.dll")]
        static extern IntPtr GetForegroundWindow();

        public Form1()
        {
            InitializeComponent();

            try
            {
                Process[] devenvs = Process.GetProcessesByName("devenv");
                //Process[] devenvs = Process.GetProcessesByName("notepad");
                if (devenvs.Length == 0) return;
                if (devenvs[0] != null)
                {
                    //p = Process.Start("notepad.exe");
                    p = devenvs[0];
                    p.WaitForInputIdle();
                    h = p.MainWindowHandle;
                    SetForegroundWindow(h);
                }
            }
            catch
            {
                MessageBox.Show("There is no target open");
                this.Close();
            }
                        
            birth = 0;
            dead = 1;
            active = false;
            try
            {
                timer1.Interval = Int32.Parse(textBox2.Text);
            }
            catch
            {
                timer1.Interval = 200;
            }

        }

        public bool active;

        private void btnStart_Click(object sender, EventArgs e)
        {
            active = true;
            try
            {
                timer1.Interval = Int32.Parse(textBox2.Text);
            }
            catch { }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            active = false;
            timer1.Enabled = false;
        }

        int birth;
        int dead;

        public string[] specialChars = { "{", "}", "(", ")", "+", "^", "[", "]", "*", ";", "=", "_", ".", "&", "|", ",\"", "\'" };

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (birth < textBox1.Text.Length)
            {
                bool _specialCharFound = false;

                string arr = textBox1.Text.Substring(birth++, dead);
                
                for (int i = 0; i < specialChars.Length; i++)
                {
                    if (arr == specialChars[i])
                    {
                        _specialCharFound = true;
                        break;
                    }
                }

                if (arr != "\n")
                    if (_specialCharFound)
                        SendKeys.Send("{" + arr + "}");
                    else 
                        SendKeys.Send(arr);

                /*
                if (birth > 2 && last_word == textBox1.Text.Substring(birth-2, dead))
                    SendKeys.SendWait(textBox1.Text.Substring(birth-1, dead));
                SendKeys.SendWait(textBox1.Text.Substring(birth, dead));
                last_word = textBox1.Text.Substring(birth++, dead);
                */
            }
            else
            {
                timer1.Enabled = false;
                birth = 0;
            }            
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            birth = 0;
            active = false;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (birth > 0)
            {
                textBox2.ReadOnly = true;
            }
            else
            {
                textBox2.ReadOnly = false;
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (active)
            {
                System.Threading.Thread.Sleep(1500);
                timer1.Enabled = true;
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
