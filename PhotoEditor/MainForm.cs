using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoEditor {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();

            listView.Columns.Add("Name").Width = 250;
            listView.Columns.Add("Last Modified").Width = 150;
            listView.Columns.Add("File Size").Width = 75;

            ListDirectory(treeView, Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        }

        private void ListDirectory(TreeView treeView, string path) {
            treeView.Nodes.Clear();

            var stack = new Stack<TreeNode>();
            var rootDirectory = new DirectoryInfo(path);
            var node = new TreeNode(rootDirectory.Name) { Tag = rootDirectory };
            stack.Push(node);

            ImageList thumbs = new ImageList();

            while (stack.Count > 0) {
                var currentNode = stack.Pop();
                var directoryInfo = (DirectoryInfo)currentNode.Tag;
                foreach (var directory in directoryInfo.GetDirectories()) {
                    var childDirectoryNode = new TreeNode(directory.Name) { Tag = directory };
                    currentNode.Nodes.Add(childDirectoryNode);
                    stack.Push(childDirectoryNode);
                }

                foreach (var file in directoryInfo.GetFiles()) {
                    string fileExtension = file.Extension;
                    fileExtension = fileExtension.ToLower();
                    var images = new ImageList();

                    if (file.Extension == ".jpg" || file.Extension == ".jpeg") {
                        ListViewItem item = new ListViewItem {
                            Text = file.Name,
                            Name = file.Name,
                            ToolTipText = file.FullName,
                            ImageKey = file.FullName
                        };

                        listView.Items.Add(item);
                    }
                }
            }

            treeView.Nodes.Add(node);
            treeView.Nodes[0].Expand();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutDialog d = new AboutDialog();
            d.ShowDialog();
        }

        private void switchViewMode(int v) {
            detailsToolStripMenuItem.Checked = false;
            smallToolStripMenuItem.Checked = false;
            largeToolStripMenuItem.Checked = false;

            if (v == 0) {
                largeToolStripMenuItem.Checked = true;
                listView.View = View.LargeIcon;
            } else if (v == 1) {
                smallToolStripMenuItem.Checked = true;
                listView.View = View.SmallIcon;
            } else {
                detailsToolStripMenuItem.Checked = true;
                listView.View = View.Details;
            }
        }

        private void largeToolStripMenuItem_Click(object sender, EventArgs e) {
            switchViewMode(0);
        }

        private void smallToolStripMenuItem_Click(object sender, EventArgs e) {
            switchViewMode(1);
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e) {
            switchViewMode(2);
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {

        }

        private void listView_ItemActivate(object sender, EventArgs e) {
            string filePath = listView.SelectedItems[0].ImageKey.ToString();

            Console.WriteLine("Opening file: " + filePath);

            EditorForm editorForm = new EditorForm(filePath);
            editorForm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void locateOnDiskToolStripMenuItem_Click(object sender, EventArgs e) {
            string filePath = null;

            try {
                filePath = listView.SelectedItems[0].ImageKey.ToString();
                filePath = filePath.Replace(listView.SelectedItems[0].Name.ToString(), "");
                Process.Start(filePath);
            } catch (ArgumentOutOfRangeException) {
                MessageBox.Show("No file selected.");
            }
        }
    }
}
