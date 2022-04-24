using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
        }

        private void sendMessage_Click(object sender, EventArgs e)
        {
            TextBox.Text += $"{DateTime.Now:t} Вы: " + messageBox.Text + "\n";
           
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            

            //TextBox.Text =  $"{null,-35}Соединение установленно!\n\n";
        }


    }
}
