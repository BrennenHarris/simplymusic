using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simplymusic
{
    public class BinarySearchTree<T> where T : IComparable<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            public Node(T data)
            {
                Data = data;
                Left = null;
                Right = null;
            }
        }

        private Node root;

        public BinarySearchTree()
        {
            root = null;
        }

        public void Insert(T data)
        {
            root = Insert(root, data);
        }

        private Node Insert(Node node, T data)
        {
            if (node == null)
            {
                node = new Node(data);
            }
            else if (data.CompareTo(node.Data) < 0)
            {
                node.Left = Insert(node.Left, data);
            }
            else if (data.CompareTo(node.Data) > 0)
            {
                node.Right = Insert(node.Right, data);
            }

            return node;
        }

        public IEnumerable<T> InOrderTraversal()
        {
            List<T> result = new List<T>();
            InOrderTraversal(root, result);
            return result;
        }

        private void InOrderTraversal(Node node, List<T> result)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, result);
                result.Add(node.Data);
                InOrderTraversal(node.Right, result);
            }
        }
    }
}
