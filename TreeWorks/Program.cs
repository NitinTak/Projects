using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeWorks
{
    class node
    {
        public int value;
        public node left;
        public node right;
        public node()
        {
            left = null;
            right = null;
        }
        public node(int val)
        {
            value = val;
            left = null;
            right = null;
        }
    }
    class Program
    {
        static node ROOT;
        static void Main(string[] args)
        {
            try
            {
                AddNodeToBST(ref ROOT, 5);
                AddNodeToBST(ref ROOT, 3);
                AddNodeToBST(ref ROOT, 7);
                AddNodeToBST(ref ROOT, 9);
                AddNodeToBST(ref ROOT, 4);

                while (true)
                {
                    Console.WriteLine("1: Add Node To BST           2: Delete Node from BST");
                    Console.WriteLine("3: Search Node in BST        4: Get Size of BST");
                    Console.WriteLine("5: Print Preorder Traversal  6: Print Inorder Traversal");
                    Console.WriteLine("7: Print Postorder Traversal 8: Print Level Order Traversal");
                    Console.WriteLine("9: Print All Paths in BST    10: Check If BST Balanced");
                    Console.WriteLine("0: EXIT");

                    Dictionary<int, int> countTreeLookup = new Dictionary<int, int>();
                    CountTrees(3, ref countTreeLookup);

                    int choice = int.Parse(Console.ReadLine());
                    int value;
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("Enter node value in int: ");
                            Console.Write("-> ");
                            value = int.Parse(Console.ReadLine());
                            AddNodeToBST(ref ROOT, value);
                            break;
                        case 2:
                            Console.WriteLine("Enter node to delete from BST: ");
                            Console.Write("-> ");
                            value = int.Parse(Console.ReadLine());
                            ROOT = DeleteBSTNode(ROOT, value);
                            Console.WriteLine("DONE!");
                            break;
                        case 3:
                            Console.WriteLine("Enter node to Search in BST: ");
                            Console.Write("-> ");
                            value = int.Parse(Console.ReadLine());
                            Console.WriteLine(IsNodePresent(ref ROOT, value) ? "Present" : "Not Present");
                            break;
                        case 4:
                            Console.Write("-> Size of BST: ");
                            Console.WriteLine(GetBSTSize(ref ROOT));
                            break;
                        case 5:
                            Console.Write("-> Preorder: ");
                            PrintPreorder(ref ROOT);
                            Console.WriteLine();
                            break;
                        case 6:
                            Console.Write("-> Inorder: ");
                            PrintInorder(ref ROOT);
                            Console.WriteLine();
                            break;
                        case 7:
                            Console.Write("-> Postorder: ");
                            PrintPostorder(ref ROOT);
                            Console.WriteLine();
                            break;
                        case 8:
                            Console.Write("-> Level Order: ");
                            LevelOrderTraversal(ref ROOT);
                            Console.WriteLine();
                            break;
                        case 9:
                            Console.WriteLine("-> Paths: ");
                            PrintPathsToLeaves(ref ROOT);
                            break;
                        case 10:
                            Console.WriteLine("Enter maximum allowable difference for balance: ");
                            Console.Write("-> ");
                            value = int.Parse(Console.ReadLine());
                            Console.WriteLine(IsBSTBalanced(ROOT, value) ? "Balanced" : "Not Balanced");
                            break;
                        case 0:
                            System.Environment.Exit(0);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }

        static int min(int a, int b)
        {
            return a < b ? a : b;
        }
        static node AddNodeToBST(ref node root, int val)
        {
            try
            {
                if (root == null)
                {
                    root = new node(val);
                    return root;
                }
                if (val > root.value)
                {
                    root.right = AddNodeToBST(ref root.right, val);
                }
                else
                {
                    root.left = AddNodeToBST(ref root.left, val);
                }
                return root;
            }
            catch (Exception)
            {
                throw;
            }
        }
        static void PrintPreorder(ref node root)
        {
            if (root == null)
                return;
            Console.Write(root.value + " ");
            PrintPreorder(ref root.left);
            PrintPreorder(ref root.right);
        }
        static void PrintInorder(ref node root)
        {
            if (root == null)
                return;
            PrintInorder(ref root.left);
            Console.Write(root.value + " ");
            PrintInorder(ref root.right);
        }
        static void PrintPostorder(ref node root)
        {
            if (root == null)
                return;
            PrintPostorder(ref root.left);
            PrintPostorder(ref root.right);
            Console.Write(root.value + " ");
        }
        static bool IsNodePresent(ref node root, int val)
        {
            if (root == null)
                return false;
            if (root.value == val)
                return true;
            if (val > root.value)
                return IsNodePresent(ref root.right, val);
            else
                return IsNodePresent(ref root.right, val);
        }
        static node DeleteBSTNode(node root, int val)
        {
            node temp = root;
            node pred = null;
            if (root == null)
                return temp;
            while (temp != null)
            {
                if (temp.value == val)
                {
                    if (temp.left == null && temp.right == null)
                    {
                        if (pred != null)
                        {
                            if (pred.left == temp)
                                pred.left = null;
                            else
                                pred.right = null;
                        }
                        else
                        {
                            root = null;
                        }
                        break;
                    }
                    else if (temp.left != null && temp.right == null)
                    {
                        if (pred != null)
                        {
                            if (pred.left == temp)
                                pred.left = temp.left;
                            else
                                pred.right = temp.left;
                        }
                        else
                        {
                            root = temp.left;
                        }
                        break;
                    }
                    else if (temp.left == null && temp.right != null)
                    {
                        if (pred != null)
                        {
                            if (pred.left == temp)
                                pred.left = temp.right;
                            else
                                pred.right = temp.right;
                        }
                        else
                        {
                            root = temp.right;
                        }
                        break;
                    }
                    else if (temp.left != null && temp.right != null)
                    {
                        node rRightPred = temp.right;
                        node rLeft = rRightPred.left;
                        if (rLeft == null)
                        {
                            rRightPred.left = temp.left;
                            if (pred != null)
                            {
                                if (pred.left == temp)
                                {
                                    pred.left = rRightPred;
                                }
                                else if (pred.right == temp)
                                {
                                    pred.right = rRightPred;
                                }
                            }
                            else
                            {
                                root = rRightPred;
                            }
                            break;
                        }
                        else
                        {
                            while (rLeft.left != null)
                            {
                                rRightPred = rLeft;
                                rLeft = rLeft.left;
                            }
                            temp.value = rLeft.value;
                            rRightPred.left = rLeft.right;
                            //deleted = true;
                            break;
                        }
                    }
                }
                else if (val > temp.value)
                {
                    pred = temp;
                    temp = temp.right;
                }
                else
                {
                    pred = temp;
                    temp = temp.left;
                }
            }
            return root;
        }

        //line by line level order traversal..uses stack and queue..
        static void LevelOrderTraversal(ref node root)
        {
            Queue<node> nodeQ = new Queue<node>();
            Queue<node> secQ = new Queue<node>();
            if (root == null)
                return;
            secQ.Enqueue(root);
            while (secQ.Count > 0)
            {
                while (secQ.Count > 0)
                {
                    nodeQ.Enqueue(secQ.Dequeue());
                }
                while (nodeQ.Count > 0)
                {
                    node temp = nodeQ.Dequeue();
                    if (temp.left != null)
                        secQ.Enqueue(temp.left);
                    if (temp.right != null)
                        secQ.Enqueue(temp.right);
                    Console.Write(temp.value + " ");
                }
                Console.WriteLine();
            }
        }
        static int GetBSTSize(ref node root)
        {
            if (root == null)
                return 0;
            return (1 + GetBSTSize(ref root.left) + GetBSTSize(ref root.right));
        }
        static int GetLongestPathBST(ref node root)
        {
            if (root == null)
                return 0;
            return (max((1 + GetLongestPathBST(ref root.left)), (1 + GetLongestPathBST(ref root.right))));
        }
        static int max(int a, int b)
        {
            return a > b ? a : b;
        }
        static bool HasPathSum(ref node root, int n)
        {
            if (root == null || n < 0)
                return false;
            if ((n - root.value) == 0)
                return true;
            else
                return (HasPathSum(ref root.left, n - root.value) || HasPathSum(ref root.right, n - root.value));
        }
        static void PrintPathsToLeaves(ref node root)
        {
            int[] A = new int[GetBSTSize(ref root)];
            PathPrintHelper(root, A, 0);
        }
        static void PathPrintHelper(node root, int[] A, int n)
        {
            if (root == null)
                return;
            A[n] = root.value;
            if (root.left == null && root.right == null)
            {
                for (int i = 0; i <= n; i++)
                {
                    Console.Write(A[i] + " ");
                }
            }
            else
            {
                PathPrintHelper(root.left, A, n + 1);
                PathPrintHelper(root.right, A, n + 1);
            }
            n--;
            Console.WriteLine();
        }
        static void Mirror(ref node root)
        {
            if (root == null)
                return;
            node temp = root.right;
            root.right = root.left;
            root.left = temp;
            Mirror(ref root.left);
            Mirror(ref root.right);
        }
        static void DoubleTree(ref node root)
        {
            if (root == null)
                return;
            node temp = new node(root.value);
            temp.left = root.left;
            root.left = temp;
            DoubleTree(ref temp.left);
            DoubleTree(ref root.right);
        }
        static bool IsSameBST(node r1, node r2)
        {
            if (r1 == null && r2 == null)
                return true;
            if ((r1 == null && r2 != null) || (r1 != null && r2 == null))
                return false;
            if (r1.value == r2.value)
                return (IsSameBST(r1.left, r2.left) && IsSameBST(r1.right, r2.right));
            else
                return false;
        }
        static bool IsValidBST(node root)
        {
            return CheckValidBSTHelper(root, int.MinValue, int.MaxValue);
        }
        static bool CheckValidBSTHelper(node root, int min, int max)
        {
            if (root == null)
                return true;
            if (root.value < min || root.value > max)
                return false;
            else
                return (CheckValidBSTHelper(root.left, min, root.value) && CheckValidBSTHelper(root.right, root.value, max));
        }
        static bool IsBSTBalanced(node root, int maxDiff)
        {
            int n = 0;
            return CheckBSTBalancedHelper(root, maxDiff, ref n);
        }
        static bool CheckBSTBalancedHelper(node root, int maxDiff, ref int h)
        {
            if (root == null)
                return true;
            int l = 0, r = 0;
            bool leftSubTree, rightSubTree;
            leftSubTree = CheckBSTBalancedHelper(root.left, maxDiff, ref l);
            rightSubTree = CheckBSTBalancedHelper(root.right, maxDiff, ref r);

            if (Math.Abs(l - r) > maxDiff)
                return false;
            h = max(l, r) + 1;

            return leftSubTree && rightSubTree;
        }
        static int CountTrees(int N, ref Dictionary<int, int> lookup)
        {
            int ret = 0;
            if (N < 2)
            {
                if (N < 0)
                    return -1;
                else
                    return 1;
            }
            else if (lookup.ContainsKey(N))
            {
                return lookup[N];
            }
            else
            {
                for (int i = 1; i <= N; i++)
                {
                    int left = CountTrees(i - 1, ref lookup);
                    int right = CountTrees(N - i, ref lookup);
                    ret = ret + left * right;
                }
            }
            lookup.Add(N, ret);
            return ret;
        }
    }
}
