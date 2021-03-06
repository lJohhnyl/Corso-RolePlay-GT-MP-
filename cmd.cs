﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using System;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

using MySQL;
using player.funcs;

namespace RP.cmd
{
    public class GlobalCommands : Script
    {
		[Command("me", GreedyArg = true)]
		public void MeCommand(Client player, string action)
		{
			var msg = player.name + " " + action;
			var players = API.getPlayersInRadiusOfPlayer(30, player);

			foreach (Client c in players)
			{
				API.sendChatMessageToPlayer(c, msg);
			}
		}
		[Command("save")]
		public void GetPosition(Client player)
		{
			Vector3 PlayerPos = API.getEntityPosition(player);
			Vector3 clientRotation = API.getEntityRotation(player.handle);
			API.sendChatMessageToPlayer(player, "X: " + PlayerPos.X + " Y: " + PlayerPos.Y + " Z: " + PlayerPos.Z + " R: " + clientRotation.Z);
		}
		[Command("vehicle")]
        public void CommandGetVehicle(Client player, string model)
        {
            VehicleHash veh;
            try
            {
                veh = (VehicleHash)System.Enum.Parse(typeof(VehicleHash), model);
            }
            catch
            {
                API.sendChatMessageToPlayer(player, "~r~[ERROR] Vehicle with the name of " + model + " was not found!");
                return;
            }
			Vector3 PlayerPos = API.getEntityPosition(player);
            Vector3 rot = API.getEntityRotation(player.handle);
            API.createVehicle(veh, new Vector3(PlayerPos.X+2, PlayerPos.Y+2, PlayerPos.Z), new Vector3(0, 0, rot.Z), 0, 0);
            API.sendChatMessageToPlayer(player, "~g~ [SPAWNED] " + model + " has spawned!");
        }
        [Command("getweapon")]
        public void CommandGetWeapon(Client player, string model, int ammo = 1000)
        {

            WeaponHash weapon;
            try
            {
                weapon = (WeaponHash)System.Enum.Parse(typeof(WeaponHash), model);
            }
            catch
            {
                API.sendChatMessageToPlayer(player, "~r~[ERROR] Weapon with the name of " + model + " was not found!");
                return;
            }
            API.givePlayerWeapon(player, weapon, ammo, true, true);
            API.sendChatMessageToPlayer(player, "~g~ [SUCCESS] " + model + " was given to the player!");
        }
        [Command("weaponnames")]
        public void CommandWeaponNames(Client player, bool randomColor = true)
        {
            int count = 0;
            // Don't put more than 7 names per line, more than that some names will not appear
            int maxNamesPerLine = 5;
            string displayString = "";
            List<string> rndColor = new List<string>
            {
                "~r~",
                "~b~",
                "~g~",
                "~y~"
            };
            Random r = new Random();
            foreach (string w in Enum.GetNames(typeof(WeaponHash)))
            {
                if (randomColor) displayString += rndColor[r.Next(0, rndColor.Count)];
                displayString += w + " ";
                count++;
                if (count >= maxNamesPerLine)
                {
                    API.sendChatMessageToPlayer(player, displayString);
                    displayString = "";
                    count = 0;
                }
            }
        }
		[Command("getmoney")]
		public void Get_Money(Client player){
			API.sendChatMessageToPlayer(player, ""+player_function.GetMoney(player));
		}
		[Command("changemoney")]
		public void Change_Money(Client player,int amount){
			player_function.ChangeMoney(player, amount);
		}
		[Command("setclothes")]
		public void clothes(Client sender, int slot, int drawable, int texture)
		{
			API.setPlayerClothes(sender, slot, drawable, texture);
		}
    }
}
