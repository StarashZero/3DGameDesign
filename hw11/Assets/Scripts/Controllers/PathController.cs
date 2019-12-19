using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController
{
    protected PathNode currentNode;      // 路径的当前状态节点

    private Hashtable solutionPath;  // 解路径：用以保存从起始状态到达目标状态的移动路径中的每一个状态节点

    private Queue<PathNode> exploreList;  // 用以保存已发现但未访问的节点
    private HashSet<int> visitedList;    // 用以保存已发现的节点

    /**
     * 构造函数
     */
    public PathController()
    {
        currentNode = null;
        solutionPath = null;
        exploreList = null;
        visitedList = null;
    }

    /**
     * 获得路径
     */
    public void GetPath()
    {
        if (solutionPath == null && currentNode != null)
        {
            solutionPath = new Hashtable();
            PathNode cNode, lNode;
            cNode = currentNode;
            lNode = null;
            while (cNode != null)
            {
                solutionPath.Add(cNode.GetHashCode(), cNode);
                cNode.SetNext(lNode);
                lNode = cNode;
                cNode = cNode.GetParent();
            }
        }
    }

    public PathNode Search(int[] bState)
    {
        PathNode bNode = new PathNode(bState);
        //如果当前路径已被求得，则直接返回结果
        if (solutionPath != null && solutionPath.Contains(bNode.GetHashCode()))
            return (PathNode)solutionPath[bNode.GetHashCode()];
        currentNode = null;
        solutionPath = null;
        exploreList = new Queue<PathNode>();
        visitedList = new HashSet<int>();
        exploreList.Enqueue(bNode);
        visitedList.Add(bNode.GetHashCode());
        //广度优先搜索
        while (exploreList.Count != 0)
        {
            currentNode = exploreList.Peek();
            if (currentNode.GetState()==1)
            {
                GetPath();
                break;
            }
            exploreList.Dequeue();

            PathNode[] nextNodes = new PathNode[]{
                    new PathNode(currentNode), new PathNode(currentNode),
                    new PathNode(currentNode), new PathNode(currentNode),
                    new PathNode(currentNode), new PathNode(currentNode),
                    new PathNode(currentNode)
            };

            // 寻找所有与currentJNode邻接且未曾被发现的节点，将它们插入exploreList中
            // 并加入visitedList中，表示已发现
            for (int i = 0; i < 7; i++)
            {
                if (nextNodes[i].Move(i))
                {
                    if (nextNodes[i].GetState() <= 1 && !visitedList.Contains(nextNodes[i].GetHashCode()))
                    {
                        exploreList.Enqueue(nextNodes[i]);
                        visitedList.Add(nextNodes[i].GetHashCode());
                    }
                }
            }
        }

        return bNode;
    }
}
