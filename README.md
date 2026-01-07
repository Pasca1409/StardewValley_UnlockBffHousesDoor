# 🔑 Stardew NPC Keys Mod

A **Stardew Valley (SMAPI)** mod that allows NPCs to grant the player a key to their house when friendship reaches **8 or more hearts**, unlocking access outside normal hours.

This project was designed to be **educational, extensible, and a solid technical base** for developers who want to learn how to create clean, well-structured mods using C#, SMAPI and Harmony.

---

## 📌 Features

- ✅ NPCs grant their house key at 8+ hearts
- ✅ Free access to NPC homes at any time
- ✅ Persistent storage via `modData`
- ✅ Precise patch on `GameLocation.lockedDoorWarp`
- ✅ Modular architecture using `EventsManipulator`
- ✅ Native multiplayer support
- ✅ Compatible with custom NPCs from other mods
- ✅ Optional debug logging

---

## 🧠 How it works

### 1. Key Grant System
At the start of each in-game day (`GameLoop.DayStarted`), the mod:

1. Iterates through all farmers
2. Checks friendship with each NPC
3. If friendship >= 8 hearts, the NPC key is registered
4. The key is stored in `farmer.modData`

```csharp
if (hearts >= 8 && !givedKeys.Contains(npcName))
    givedKeys.Add(npcName);
```

---

### 2. Door Unlock (Harmony Patch)
The default game behavior is intercepted using Harmony:

```csharp
[HarmonyPatch(typeof(GameLocation), "lockedDoorWarp")]
[HarmonyPrefix]
public static bool Prefix(...)
```

If the player owns the NPC key, the method manually performs the door opening logic and skips the original:

```csharp
Game1.warpFarmer(locationName, tile.X, tile.Y, false);
return false;
```

---

## 🏗️ Project Architecture

This project follows a clean and modular structure designed for maintainability and easy learning:

```
ModEntry.cs

Events/
 ├── EventsManipulator.cs
 └── WorldEvents.cs

Harmony/
 └── GameLocationHarmony.cs

Utils/
 ├── Utils.cs
 └── StringfiedList.cs
```

### Structure Explanation

- **ModEntry.cs**  
  Main entry point of the mod. It initializes Harmony patches and dynamically registers all event modules.

- **Events/**  
  Contains all classes responsible for managing game events. Each module inherits from `EventsManipulator` and is automatically discovered via reflection.

- **Harmony/**  
  All Harmony patches are isolated here, including the interception of `lockedDoorWarp` to control door access.

- **Utils/**  
  Shared helper utilities and data processing tools used across the mod.

---

## 🚀 How to use

1. Build the project
2. Copy the generated DLL into:

```
Stardew Valley/Mods/
```

3. Launch the game with SMAPI
4. Reach 8 hearts of friendship with any NPC
5. The key will be automatically granted

---

## 🎓 Who is this for?

- Stardew modding beginners
- Developers wanting clean architecture references
- Harmony learners
- Developers building systems based on relationships

---

## 🧩 Technologies Used

- .NET 6
- C#
- SMAPI
- Harmony
- Pathoschild ModBuildConfig

---

## 📝 License

This project is licensed under the MIT License, allowing free use, modification and redistribution with proper credit.

```
MIT License

Copyright (c) 2025

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files...
```

---

## 🤝 Contributing

Feel free to:
- open issues
- suggest improvements
- submit pull requests
- use this project as a base for your own mods

---

## 📷 Screenshots
(Add screenshots demonstrating the mod behavior here)

---

## 📚 Useful Links

- SMAPI Docs → https://smapi.io
- Stardew Modding Wiki → https://stardewvalleywiki.com/Modding
- Harmony Docs → https://harmony.pardeike.net

---

## ✨ Author

Created by a developer passionate about clean architecture, strong design patterns, and educational modding for the Stardew Valley community.

If you're reading this to learn: explore the code, modify it, test it, and build your own systems on top of it.

---

Happy modding! 🌱💻

