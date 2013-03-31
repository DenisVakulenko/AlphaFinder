using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestHelpers {
    public partial class frmShutDown : Form {
        public frmShutDown() {
            InitializeComponent();
        }

        private void ucOnAirRecognizer1_SmthRecognized(string[] text) {
            string s;
            for (int i = 0; i < Math.Min(text.Length, 2); i++) {
                s = text[i].ToLower();
                if (s == "да" || s == "yes" || s == "выключить" || s == "shut down")
                    System.Diagnostics.Process.Start("ShutDown", "-s -f -t 00");
                if (s == "нет" || s == "no" || s == "отмена" || s == "cancel")
                    this.Close();
            }
        }
        private void frmShutDown_Shown(object sender, EventArgs e) {
            ucOnAirRecognizer1.StartListening();
        }
        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("ShutDown", "-s -f -t 00");
        }
    }
}
