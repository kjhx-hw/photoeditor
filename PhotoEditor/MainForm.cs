using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            ListDirectory(treeView, Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
        }

        private void ListDirectory(TreeView treeView, string path) {
            treeView.Nodes.Clear();

            var stack = new Stack<TreeNode>();
            var rootDirectory = new DirectoryInfo(path);
            var node = new TreeNode(rootDirectory.Name) { Tag = rootDirectory };
            stack.Push(node);

            while (stack.Count > 0) {
                var currentNode = stack.Pop();
                var directoryInfo = (DirectoryInfo)currentNode.Tag;
                foreach (var directory in directoryInfo.GetDirectories()) {
                    var childDirectoryNode = new TreeNode(directory.Name) { Tag = directory };
                    currentNode.Nodes.Add(childDirectoryNode);
                    stack.Push(childDirectoryNode);
                }
                //foreach (var file in directoryInfo.GetFiles())
                  //  currentNode.Nodes.Add(new TreeNode(file.Name));
            }

            treeView.Nodes.Add(node);
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
            } else if (v == 1) {
                smallToolStripMenuItem.Checked = true;
            } else {
                detailsToolStripMenuItem.Checked = true;
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

        private void openEditorFormToolStripMenuItem_Click(object sender, EventArgs e) {
            EditorForm eF = new EditorForm();
            eF.ShowDialog();
        }
    }
}
