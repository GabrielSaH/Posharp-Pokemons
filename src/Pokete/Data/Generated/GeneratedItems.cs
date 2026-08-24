// AUTO-GENERATED from src/pokete/data/items.py - do not edit by hand.
using System.Collections.Generic;
using Pokete.Models;

namespace Pokete.Data.Generated;

public static class GeneratedItems
{
    public static readonly Dictionary<string, ItemInfo> All = new()
    {
        ["poketeball"] = new ItemInfo("poketeball", "Poketeball", "A ball you can use to catch Poketes", 2, "poketeball"),
        ["superball"] = new ItemInfo("superball", "Superball", "A ball you can use to catch Poketes with an increased chance", 10, "superball"),
        ["hyperball"] = new ItemInfo("hyperball", "Hyperball", "For catching Poketes with a waaay higher chance", null, "hyperball"),
        ["healing_potion"] = new ItemInfo("healing_potion", "Healing potion", "Heals a Pokete with 5 HP", 15, "heal_potion"),
        ["super_potion"] = new ItemInfo("super_potion", "Super potion", "Heals a Pokete with 15 HP", 25, "super_potion"),
        ["ap_potion"] = new ItemInfo("ap_potion", "AP potion", "Refills the Poketes attack APs.", 100, "ap_potion"),
        ["treat"] = new ItemInfo("treat", "Treat", "Upgrades a Pokete by a whole level.", null, null),
        ["shut_the_fuck_up_stone"] = new ItemInfo("shut_the_fuck_up_stone", "'Shut the fuck up' stone", "Makes trainer leaving you alone", null, null),
    };
}