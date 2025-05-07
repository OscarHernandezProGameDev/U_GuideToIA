using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree : Node
{
    public BehaviourTree()
    {
        this.name = "Tree";
    }

    public BehaviourTree(string n)
    {
        name = n;
    }

    struct NodeLevel
    {
        public int level;
        public Node node;
    }

    public void PrintTree()
    {
        string treePrintout = "";
        Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
        Node currentNode = this;
        nodeStack.Push(new NodeLevel { level = 0, node = currentNode });

        while (nodeStack.Count != 0)
        {
            NodeLevel nextNode = nodeStack.Pop();
            treePrintout += new string('-', nextNode.level) + nextNode.node.name + "\n";
            for (int i = nextNode.node.childrens.Count - 1; i >= 0; i--)
            {
                nodeStack.Push(new NodeLevel { level = nextNode.level + 1, node = nextNode.node.childrens[i] });
            }

            Debug.Log(treePrintout);
        }
    }
}
