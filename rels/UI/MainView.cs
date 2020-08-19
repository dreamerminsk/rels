using LinqToDB.Common;
using rels.Model;
using rels.Wiki;
using rels.Workers;
using System;
using System.Linq;
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
            updater.Log.ObserveOn(SynchronizationContext.Current).Subscribe(o =>
            {
                logTextBox.AppendText(o);
            });
            Web.Log.ObserveOn(SynchronizationContext.Current).Subscribe(o =>
            {
                webTextBox.AppendText(o);
            });
            Web.Stats.ObserveOn(SynchronizationContext.Current).Subscribe(o =>
            {
                webStatsView.BeginUpdate();
                var find = webStatsView.Items.Find(o.Name, false);
                if (find.IsNullOrEmpty())
                {
                    var statItem = webStatsView.Items.Add(o.Name);
                    statItem.SubItems.Add(o.Requests.ToString());
                    statItem.SubItems.Add(o.Bytes.ToString());
                }
                else
                {
                    find.ToList().ForEach(lvi =>
                    {
                        lvi.SubItems.Clear();
                        lvi.SubItems.Add(o.Requests.ToString());
                        lvi.SubItems.Add(o.Bytes.ToString());
                    });
                }
                webStatsView.EndUpdate();
            });
            updater.Start();
        }

        private void TreeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

        }

        private async void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = treeView1.SelectedNode;
            if (node == null) return;
            if (!node.Nodes.IsNullOrEmpty()) return;
            if (!node.Text.StartsWith("Q")) return;
            var p = await Humans.GetByWikiDataIDAsync(node.Text).ConfigureAwait(true);
            treeView1.BeginUpdate();
            var labelNode = node.Nodes.Add("Labels");
            if (!p.Labels.IsNullOrEmpty())
            {
                p.Labels.ForEach(l => labelNode.Nodes.Add(string.Format("{0}: {1}", l.Language, l.Value)));
            }
            var lifeNode = node.Nodes.Add("Life");
            if (!p.DateOfBirth.IsNullOrEmpty())
            {
                var birthNode = lifeNode.Nodes.Add(string.Format("{0}", p.DateOfBirth.Substring(0, 11)));
                birthNode.ImageIndex = 4;
                birthNode.SelectedImageIndex = 4;
            }
            if (!p.DateOfDeath.IsNullOrEmpty())
            {
                var deathNode = lifeNode.Nodes.Add(string.Format("{0}", p.DateOfDeath.Substring(0, 11)));
                deathNode.ImageIndex = 6;
                deathNode.SelectedImageIndex = 6;
            }
            var parentsNode = node.Nodes.Add("Parents");
            if (p.Father != null)
            {
                var fatherNode = parentsNode.Nodes.Add(p.Father);
                fatherNode.ImageIndex = 1;
                fatherNode.SelectedImageIndex = 1;
            }
            if (p.Mother != null)
            {
                var motherNode = parentsNode.Nodes.Add(p.Mother);
                motherNode.ImageIndex = 2;
                motherNode.SelectedImageIndex = 2;
            }
            treeView1.EndUpdate();
            node.ExpandAll();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
