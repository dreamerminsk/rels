using rels.Workers;
using System;
using System.Reactive.Linq;
using System.Threading;
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

        private void MainView_Load(object sender, EventArgs e)
        {
            updater.WikiData.ObserveOn(SynchronizationContext.Current)
               .Subscribe(o =>
            {
                treeView1.BeginUpdate();
                treeView1.Nodes.Add(o);
                treeView1.EndUpdate();
            });
            updater.Start();
        }
    }
}
