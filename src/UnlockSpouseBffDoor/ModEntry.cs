using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Diagnostics;
using System.Reflection;
using UnlockBffHousesDoor.Events;


namespace UnlockBffHousesDoor
{
  internal sealed class ModEntry : Mod
  {
    private List<EventsManipulator> _eventsManipulators;
    public override void Entry(IModHelper helper)
    {
      SetupEvents(helper);
      SetupHarmonyPatches();
    }

    private void SetupHarmonyPatches()
    {
      HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(Utils.Utils.ModUniqueId);
      harmony.PatchAll();
    }

    private void SetupEvents(IModHelper helper)
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      List<Type> typesOfEventsManipulators =  assembly.GetTypes().Where(t => typeof(EventsManipulator).IsAssignableFrom(t) && !t.IsAbstract).ToList();
      foreach (Type typeOfEventsManipulator in typesOfEventsManipulators)
      {
        EventsManipulator? instance = Activator.CreateInstance(typeOfEventsManipulator, helper) as EventsManipulator;
        if (instance != null) _eventsManipulators.Add(instance);
      }
      
    }
  }
}
