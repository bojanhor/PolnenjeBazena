using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.Administration;

namespace PrepareIIS
{
    public partial class Form1 : Form
    {
        // Settings
        public string Name = "testPool";

        // //

        ServerManager s = new ServerManager();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        public void AddSiteToIIS()
        {
            
            for (int i = 0; i < s.ApplicationPools.Count; i++)
            {
                if (s.ApplicationPools[i].Name == Name)
                {
                    var pool = s.ApplicationPools[i];
                    pool.Stop();
                    ApplicationPoolCollection appColl = s.ApplicationPools;
                    appColl.Remove(s.ApplicationPools[i]);
                    

                }
            }

            s.ApplicationPools.Add(Name);
            
        }
    }
}
