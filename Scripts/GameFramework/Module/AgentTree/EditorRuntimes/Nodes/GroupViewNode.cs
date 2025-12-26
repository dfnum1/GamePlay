/********************************************************************
生成日期:	06:30:2025
类    名: 	VectorOpNode
作    者:	HappLI
描    述:	向量运算节点
*********************************************************************/
#if UNITY_EDITOR
using Framework.AT.Runtime;
using UnityEngine.UIElements;

namespace Framework.AT.Editor
{
    [EditorBindNode(typeof(GroupNode))]
    public class GroupViewNode : GraphNode
    {
        public GroupViewNode(AgentTreeGraphView pAgent, BaseNode pNode, bool bUpdatePos = true) : base(pAgent, pNode, bUpdatePos)
        {
            RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button == 0)
                {
                    groupViewNode.userData = evt.mousePosition;
                }
            });
            RegisterCallback<MouseMoveEvent>(evt =>
            {
                if (evt.pressedButtons == 1 && groupViewNode.userData is Vector2 startPos)
                {
                    Vector2 delta = evt.mousePosition - startPos;
                    groupViewNode.userData = evt.mousePosition;
                    // 移动所有子节点
                    foreach (var node in selectedNodes)
                    {
                        var pos = node.GetPosition();
                        node.SetPosition(new Rect(pos.x + delta.x, pos.y + delta.y, pos.width, pos.height));
                    }
                    // 组自身也移动
                    var groupPos = groupViewNode.GetPosition();
                    groupViewNode.SetPosition(new Rect(groupPos.x + delta.x, groupPos.y + delta.y, groupPos.width, groupPos.height));
                }
            });
        }
    }
}
#endif