/*
 * File:        MainWindow.xaml.cs
 * Author:      Landy
 * Date:        2020年10月19日
 * Description：测试树型数据结构
 * History
 * <Date>           <Author>    <Description>
 * 2020年10月19日   Landy        创建
 */

using System;
using System.Windows;
using TxLibrary.DataStructure.Tree;

namespace TestTree
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        TxTree Tree = new TxTree("Father");
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            tree.Items.Add(TxTree.ToTreeViewItem(Tree));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tree.Items.Clear();

            {// Test ok
                //string value = DateTime.Now.ToString();
                //Tree.Add(value);
                //var t = new TxTree("1");
                //t.Add("2");
                //t.Add(new TxTree("3"));
                //Tree.Add(t);
            }

            {// Test ok
                Tree.Add(Create(10));
            }

            {// Test ok
                //Tree.GetChilds[0].Add("temp");
            }

            {// Test ok
                //Tree.GetChilds[0].GetChilds[0].Remove();
            }

            {// Test ok
                //foreach(var node in Tree.GetChilds)
                //{
                //    node.Remove();
                //    break;
                //}
            }

            {
                
            }

            tree.Items.Add(TxTree.ToTreeViewItem(Tree));
        }

        private TxTree Create(int i)
        {
            TxTree tree = new TxTree(i);
            if (i > 0)
                tree.Add(Create(i - 1));
            return tree;
        }
    }
}
