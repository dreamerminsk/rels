using rels.Workers;
using System.Windows.Forms;

namespace rels.UI
{
    public partial class MainView : Form
    {

        private Updater updater = new Updater();

        public MainView()
        {
            InitializeComponent();
        }

        private void MainView_Load(object sender, System.EventArgs e)
        {

        }
    }
}
