using LinqToDB.Common;
using rels.Model;
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

        private async void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

        }

        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = treeView1.SelectedNode;
            if (node == null) return;
            if (!node.Nodes.IsNullOrEmpty()) return;
            if (!node.Text.StartsWith("Q")) return;
            var p = await People.GetByWikiDataIDAsync(node.Text).ConfigureAwait(true);
            treeView1.BeginUpdate();
            if (!p.Labels.IsNullOrEmpty())
            {
                var labelNode = node.Nodes.Add("Labels");
                p.Labels.ForEach(l => labelNode.Nodes.Add(string.Format("{0}: {1}", l.Language, l.Value)));
            }
            if (!p.DateOfBirth.IsNullOrEmpty())
            {
                node.Nodes.Add(string.Format("b. {0}", p.DateOfBirth.Substring(0, 11)));
            }
            if (!p.DateOfDeath.IsNullOrEmpty())
            {
                node.Nodes.Add(string.Format("d. {0}", p.DateOfDeath.Substring(0, 11)));
            }
            if (p.Father!=null)
            {
                node.Nodes.Add(p.Father);
            }
            if (p.Mother != null)
            {
                node.Nodes.Add(p.Mother);
            }
            treeView1.EndUpdate();
            node.ExpandAll();
        }
    }
}
