using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Audio;
using StardewValley.GameData.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnlockBffHousesDoor.Utils;


namespace UnlockBffHousesDoor.Harmony
{
  [HarmonyPatch(typeof(StardewValley.GameLocation))]
  public static class GameLocationHarmony
  {
    [HarmonyPatch("lockedDoorWarp")]
    [HarmonyPrefix]
    public static bool lockedDoorWarp(GameLocation __instance, Point tile, string locationName, int openTime, int closeTime, string npcName, int minFriendship)
    {

      if (Game1.player.modData.TryGetValue($"{Utils.Utils.UserKeyGivedFromSaveData}", out string? givedKeysString))
      {
        StringfiedList givedKeys = new StringfiedList(",", givedKeysString);

        List<string> npcKeysList = givedKeys.Get().ToList();

        bool farmerContainsKey = false;

        foreach (string keyName in npcKeysList)
        {
          if (NPC.TryGetData(keyName, out CharacterData? characterData)
          && NPC.ReadNpcHomeData(characterData, __instance, out string locName, out _, out _)
          && locName.Equals(locationName))
          {
            farmerContainsKey = true;
            break;
          }
        }

        if (farmerContainsKey)
        {
          Rumble.rumble(0.15f, 200f);
          Game1.player.completelyStopAnimatingOrDoingAction();
          __instance.playSound("doorClose", new Vector2?(Game1.player.Tile), null, SoundContext.Default);
          Game1.warpFarmer(locationName, tile.X, tile.Y, false);
          return false;
        }
      }
      return true;
    }
  }
}
