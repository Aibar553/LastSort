public enum Color { Red, Black }

public class Node
{
    public int Value;
    public Color Color;
    public Node Left;
    public Node Right;
    public Node Parent;

    public Node(int value)
    {
        Value = value;
        Color = Color.Red; // Новые узлы всегда красные
        Left = null;
        Right = null;
        Parent = null;
    }

    public Node Grandparent() => Parent?.Parent;

    public Node Uncle() => Parent?.Sibling();

    public Node Sibling()
    {
        if (this == Parent?.Left) return Parent?.Right;
        return Parent?.Left;
    }
}

public class RedBlackTree
{
    private Node root;

    private void RotateRight(Node node)
    {
        Node pivot = node.Left;
        pivot.Parent = node.Parent;

        if (node.Parent != null)
        {
            if (node.Parent.Left == node)
                node.Parent.Left = pivot;
            else
                node.Parent.Right = pivot;
        }

        node.Left = pivot.Right;
        if (pivot.Right != null)
            pivot.Right.Parent = node;

        node.Parent = pivot;
        pivot.Right = node;
    }

    private void Insert(Node node, Node newNode)
    {
        if (root == null)
        {
            root = newNode;
            newNode.Color = Color.Black;
        }
        else
        {
            if (newNode.Value < node.Value)
            {
                if (node.Left == null)
                {
                    node.Left = newNode;
                    newNode.Parent = node;
                }
                else
                {
                    Insert(node.Left, newNode);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = newNode;
                    newNode.Parent = node;
                }
                else
                {
                    Insert(node.Right, newNode);
                }
            }
        }
    }

    private void Rebalance(Node node)
    {
        if (node.Parent == null)
        {
            node.Color = Color.Black;
            return;
        }

        if (node.Parent.Color == Color.Black)
            return;

        if (node.Uncle() != null && node.Uncle().Color == Color.Red)
        {
            node.Parent.Color = node.Uncle().Color = Color.Black;
            node.Grandparent().Color = Color.Red;
            Rebalance(node.Grandparent());
        }
        else
        {
            if (node.Parent.Right == node && node.Grandparent().Left == node.Parent)
            {
                RotateRight(node.Parent);
                node = node.Left;
            }

            node.Parent.Color = Color.Black;
            node.Grandparent().Color = Color.Red;
            RotateRight(node.Grandparent());
        }
    }

    public void Insert(int value)
    {
        Node newNode = new Node(value);
        Insert(root, newNode);
        Rebalance(newNode);
    }
    public void DisplayTree()
    {
        if (root == null)
        {
            Console.WriteLine("Tree is empty");
            return;
        }

        DisplayNode(root, string.Empty);
    }

    private void DisplayNode(Node node, string indent)
    {
        if (node == null) return;

        Console.WriteLine(indent + node.Value + " (" + node.Color + ")");
        DisplayNode(node.Left, indent + "  ");
        DisplayNode(node.Right, indent + "  ");
    }

    public bool ValidateTree()
    {
        if (root == null) return true;

        // корень должен быть черным
        if (root.Color != Color.Black) return false;

        // Проверка на корректность красно-черного дерева
        return ValidateNode(root);
    }

    private bool ValidateNode(Node node)
    {
        // Красный узел должен иметь черных родителей и детей
        if (node.Color == Color.Red)
        {
            if ((node.Left != null && node.Left.Color != Color.Black) ||
                (node.Right != null && node.Right.Color != Color.Black) ||
                (node.Parent != null && node.Parent.Color != Color.Black))
            {
                return false;
            }
        }

        // Проверяем корректность узлов-потомков
        if (node.Left != null && !ValidateNode(node.Left)) return false;
        if (node.Right != null && !ValidateNode(node.Right)) return false;

        return true;
    }
}

    class Program
{
    static void Main(string[] args)
    {
        RedBlackTree tree = new RedBlackTree();

        // Добавляем значения в дерево
        tree.Insert(10);
        tree.Insert(5);
        tree.Insert(15);
        tree.Insert(20);

        // Здесь вы можете добавить методы для вывода или проверки дерева
        // Например, вы можете реализовать метод обхода дерева и вывода его на экран
        tree.DisplayTree();

        // Проверяем корректность дерева
        if (tree.ValidateTree())
        {
            Console.WriteLine("The tree is a valid red-black tree.");
        }
        else
        {
            Console.WriteLine("The tree is NOT a valid red-black tree.");
        }
    }
}

