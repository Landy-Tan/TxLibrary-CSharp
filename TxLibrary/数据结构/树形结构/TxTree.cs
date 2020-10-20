/*
 * File:        TxTree.cs
 * Author:      Landy
 * Date:        2020年10月19日
 * Description：树型结构
 * History
 * <Date>           <Author>    <Description>
 * 2020年10月19日   Landy        创建
 */

using System;
using System.Collections.Generic;

namespace TxLibrary.DataStructure.Tree
{
    /// <summary>
    /// 树型结构
    /// </summary>
    public class TxTree
    {
        /// <summary>
        /// 节点数据
        /// </summary>
        public object Value { get; set; } = default;

        /// <summary>
        /// 父节点
        /// </summary>
        protected TxTree Father = default;

        /// <summary>
        /// 子节点
        /// </summary>
        protected List<TxTree> Childs = new List<TxTree>();

        /// <summary>
        /// 创建一个节点
        /// </summary>
        /// <param name="_value">节点数据</param>
        public TxTree(object _value = default)
        {
            Value = _value;
        }

        /// <summary>
        /// 获取子节点列表
        /// </summary>
        public IReadOnlyList<TxTree> GetChilds => Childs;
        
        /// <summary>
        /// 获取父节点
        /// </summary>
        public TxTree GetFather => Father;

        /// <summary>
        /// 判断当前节点是否为根节点
        /// </summary>
        public bool IsRoot => Father == null;

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="child">子节点数据</param>
        public void Add(object child)
        {
            TxTree node = new TxTree(child);
            Add(new TxTree(child));
        }

        /// <summary>
        /// 添加节点
        /// 新节点添加到Childs，并重置新节点的父亲为本节点
        /// </summary>
        /// <param name="tree">子节点</param>
        public void Add(TxTree tree)
        {
            if (tree == null) return;
            Childs.Add(tree);
            tree.Father = this;
        }

        /// <summary>
        /// 移除当前节点
        /// 将当前节点的所有子节点添加到父节点，并将所有子节点的父节点重置
        /// 将本节点从父节点中移除
        /// </summary>
        public void Remove()
        {
            foreach(var child in Childs)
            {
                child.Father = this.Father;
                this.Father?.Childs.Add(child);
            }
            this.Father?.Childs.Remove(this);
        }

        /// <summary>
        /// 转换到WPF框架的TreeViewItem类型
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static System.Windows.Controls.TreeViewItem ToTreeViewItem(TxTree tree)
        {
            if (tree == null) throw new ArgumentNullException(nameof(tree));
            System.Windows.Controls.TreeViewItem view = new System.Windows.Controls.TreeViewItem();
            view.Header = tree.Value;
            foreach (TxTree child in tree.Childs)
                view.Items.Add(ToTreeViewItem(child));
            return view;
        }

        /// <summary>
        /// 转换到WinFrom框架的TreeNode类型
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public static System.Windows.Forms.TreeNode ToTreeNode(TxTree tree)
        {
            if (tree == null) throw new ArgumentNullException(nameof(tree));
            System.Windows.Forms.TreeNode node = new System.Windows.Forms.TreeNode();
            node.Text = tree.Value?.ToString();
            foreach (TxTree child in tree.Childs)
                node.Nodes.Add(ToTreeNode(child));
            return node;
        }
    }
}
