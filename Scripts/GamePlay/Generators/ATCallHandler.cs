//auto generated
namespace Framework.AT.Runtime
{
	public class ATCallHandler
	{
		public static bool DoAction(AgentTree pAgentTree, BaseNode pNode)
		{
			if(pNode == null || pNode.GetInportCount()<=0) return true;
			var pUserClasser = pAgentTree.GetInportUserData(pNode, 0);
			switch(pUserClasser.value)
			{
			case -1:return Framework.ActorSystem.Runtime.Framework_ActorSystem_Runtime_Actor.DoAction(pUserClasser,pAgentTree, pNode);//Framework.ActorSystem.Runtime.Actor
			}
			return false;
		}
	}
}
