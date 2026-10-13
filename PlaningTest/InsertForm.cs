using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaningTest
{
    public enum InsertMode
    {
        Front,
        Rear,
        Cancel
    }
    public partial class InsertForm : Form
    {
        public InsertMode Result { get; private set; } = InsertMode.Cancel;
        public InsertForm()
        {
            InitializeComponent();
        }

        private void InsertBackBtn_Click(object sender, EventArgs e)
        {
            Result = InsertMode.Rear;
            DialogResult = DialogResult.OK;
        }

        private void InsertFrontBtn_Click(object sender, EventArgs e)
        {
            Result = InsertMode.Front;
            DialogResult = DialogResult.OK;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
