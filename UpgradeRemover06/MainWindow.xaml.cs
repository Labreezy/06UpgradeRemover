using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UpgradeRemover06
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
            
    public partial class MainWindow
    {
        private int[][] UPGRADEFLAGS = new[] {new[]{ 6000,6001,6002,6004,6005,6006,6007,6008,6009,6010},new[] {6012,6013,6014}, new[]{ 6016,6017,6018,6019}};
        private static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null) return null;

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }

        private MemoryUtils mu;
        public MainWindow()
        {
            InitializeComponent();
            mu = new MemoryUtils();
            bool success = mu.tryOpenXenia();
            Debug.WriteLineIf(success, "Process opened!");
            Debug.WriteLine("hello o/");
            
            
        }

        

        private void UpgradeOnClick(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            TreeViewItem tvi = FindParent<TreeViewItem>(box);
            TreeViewItem chartvi = FindParent<TreeViewItem>(tvi);
            int upgradeidx = chartvi.Items.IndexOf(tvi);
            int charidx = MainTreeView.Items.IndexOf(chartvi);
            Debug.Print($"upgrade index: {upgradeidx}\n char index: {charidx}");
            if (box.IsChecked.Value)
            {
                mu.writeFlag(UPGRADEFLAGS[charidx][upgradeidx], 1);
            }
            else
            {
                mu.writeFlag(UPGRADEFLAGS[charidx][upgradeidx], 0);
            }
            
        }

        private void RefreshBtn_OnClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem[] charnodes = this.MainTreeView.Items.Cast<TreeViewItem>().ToArray();
            for (int i = 0; i < charnodes.Count(); i++)
            {
                TreeViewItem[] upgradenodes = charnodes[i].Items.Cast<TreeViewItem>().ToArray();
                int[] currentflagbatch = UPGRADEFLAGS[i];
                for (var j = 0; j < currentflagbatch.Length; j++)
                {
                    DockPanel dp = upgradenodes[j].Header as DockPanel;
                    CheckBox cb = dp.Children[0] as CheckBox;
                    cb.IsChecked = mu.readFlag(UPGRADEFLAGS[i][j]);
                }

            }
        }
    }
}