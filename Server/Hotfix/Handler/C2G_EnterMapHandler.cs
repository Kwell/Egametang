﻿using System;
using Model;

namespace Hotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_EnterMapHandler : AMRpcHandler<C2G_EnterMap, G2C_EnterMap>
	{
		protected override async void Run(Session session, C2G_EnterMap message, Action<G2C_EnterMap> reply)
		{
			G2C_EnterMap response = new G2C_EnterMap();
			try
			{
				Player player = session.GetComponent<SessionPlayerComponent>().Player;
				// 在map服务器上创建战斗Unit
				string mapAddress = Game.Scene.GetComponent<StartConfigComponent>().MapConfigs[0].GetComponent<InnerConfig>().Address;
				Session mapSession = Game.Scene.GetComponent<NetInnerComponent>().Get(mapAddress);
				M2G_CreateUnit createUnit = await mapSession.Call<M2G_CreateUnit>(new G2M_CreateUnit() { PlayerId = player.Id, GateSessionId = session.Id });
				player.UnitId = createUnit.UnitId;
				response.UnitId = createUnit.UnitId;
				response.Count = createUnit.Count;
				reply(response);
			}
			catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}
	}
}