namespace Tree
{
    using System;
    using System.Collections.Generic;

    public class Tree<T> : IAbstractTree<T>
    {
        private T value;
        private List<Tree<T>> children;
        private Tree<T> parent;
        public Tree(T value)
        {
            this.value = value;
            children = new List<Tree<T>>();
        }

        public Tree(T value, params Tree<T>[] children)
            : this(value)
        {
            foreach (Tree<T> child in children)
            {
                this.children.Add(child);
                child.parent = this;
            }
        }

        public void AddChild(T parentKey, Tree<T> child)
        {
            Tree<T> parent = FindNode(parentKey);
            if (IsNull(parent))
            {
                throw new ArgumentNullException();
            }
            parent.children.Add(child);
            child.parent = parent;
        }

        public IEnumerable<T> OrderBfs()
        {
            List<T> list = new List<T>();

            Queue<Tree<T>> queue = new Queue<Tree<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                Tree<T> current = queue.Dequeue();
                list.Add(current.value);

                foreach (var child in current.children)
                {
                    queue.Enqueue(child);
                }
            }

            return list;
        }

        public IEnumerable<T> OrderDfs()
        {
            List<T> result = new List<T>();
            OrderDfs(result, this);

            return result;
        }

        public void RemoveNode(T nodeKey)
        {
            Tree<T> nodeToRemove = FindNode(nodeKey);
            if (IsNull(nodeToRemove))
            {
                throw new ArgumentNullException();
            }
            Tree<T> parent = nodeToRemove.parent;
            if (IsNull(parent))
            {
                throw new ArgumentException();
            }
            nodeToRemove.parent = null;
            parent.children.Remove(nodeToRemove);
        }

        public void Swap(T firstKey, T secondKey)
        {
            Tree<T> firstNode = FindNode(firstKey);
            Tree<T> secondNode = FindNode(secondKey);

            if (IsNull(firstNode) || IsNull(secondNode))
            {
                throw new ArgumentNullException();
            }
            Tree<T> firstNodeParent = firstNode.parent;
            Tree<T> secondNodeParent = secondNode.parent;

            if (IsNull(firstNodeParent) || IsNull(secondNodeParent))
            {
                throw new ArgumentException();
            }

            int firstNodeIndex = firstNodeParent.children.IndexOf(firstNode);
            int secondNodeIndex = secondNodeParent.children.IndexOf(secondNode);

            firstNodeParent.children[firstNodeIndex] = secondNode;
            secondNode.parent = firstNodeParent;

            secondNodeParent.children[secondNodeIndex] = firstNode;
            firstNode.parent = secondNodeParent;
        }

        private void OrderDfs(List<T> list, Tree<T> node)
        {
            foreach (var child in node.children)
            {
                OrderDfs(list, child);
            }
            list.Add(node.value);
        }
        private Tree<T> FindNode(T key)
        {
            Queue<Tree<T>> queue = new Queue<Tree<T>>();

            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                Tree<T> currentNode = queue.Dequeue();
                if (currentNode.value.Equals(key))
                {
                    return currentNode;
                }

                foreach (var child in currentNode.children)
                {
                    queue.Enqueue(child);
                }
            }
            return null;
        }

        private bool IsNull(Tree<T> node)
        {
            return node == null;
        }
    }
}
