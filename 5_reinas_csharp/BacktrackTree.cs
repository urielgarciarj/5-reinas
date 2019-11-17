using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5_reinas_csharp
{
    public class BacktrackTree
    {
        Node root;

        public int Depth
        {
            get
            {
                if (root != null)
                    return root.nodes.Length;
                else
                    return 0;
            }
        }

        public BacktrackTree(int depth)
        {
            root = new Node(depth, depth);
        }

        public bool InsertSolution(int[] solution)
        {
            if (solution.Length != Depth)
                return false;

            Node temp = root;
            for (int i = 0; i < Depth; i++)
            {
                temp = temp.nodes[solution[i]];
                temp.hasQueen = true;
            }

            return true;
        }

        class Node
        {
            public bool hasQueen = false;
            public Node[] nodes;

            public Node(int width, int depth)
            {
                hasQueen = false;

                if (depth > 0)
                {
                    nodes = new Node[width];
                    for (int i = 0; i < width; i++)
                    {
                        nodes[i] = new Node(width, depth - 1);
                    }
                }
                else
                {
                    nodes = new Node[0];
                }
            }
        }
    }
}