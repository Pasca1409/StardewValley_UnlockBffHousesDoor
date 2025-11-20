using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnlockBffHousesDoor.Utils;

namespace UnlockBffHousesDoor.Events
{
  internal class WorldEvents : EventsManipulator
  {
    public WorldEvents(IModHelper helper) : base(helper)
    {
      RegisterEvents();
    }

    private void RegisterEvents()
    {
      Helper.Events.GameLoop.DayStarted += OnDayStarted;
    }

    private void OnDayStarted(object? sender, DayStartedEventArgs e)
    {
      RegisterFriendsKeys();
    }

    private void RegisterFriendsKeys()
    {
      IEnumerable<Farmer> farmers = Game1.getAllFarmers();
      foreach (Farmer farmer in farmers)
      {
        string farmerName = farmer.displayName;
        string keyWorldSaveKeyNamePrefix = $"{Utils.Utils.UserKeyGivedFromSaveData}";
        NetStringDictionary<Friendship, Netcode.NetRef<Friendship>> friendships = farmer.friendshipData;
        StringfiedList givedKeys = new StringfiedList(",", farmer.modData[keyWorldSaveKeyNamePrefix] ?? "");
        foreach (KeyValuePair<string, Friendship> friendship in friendships.Pairs)
        {
          string friendName = friendship.Key;
          int friendshipHearts = farmer.getFriendshipHeartLevelForNPC(friendName);
          if (friendshipHearts >= 8) givedKeys.Add(friendName);
        }
        farmer.modData.OverwriteData(keyWorldSaveKeyNamePrefix, givedKeys.ToString());
      }
    }
  }
}
