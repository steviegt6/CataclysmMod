using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.ClickerClass
{
    /// <summary>
    /// Central file used for mod.Call wrappers.
    /// </summary>
    public static class ClickerCompatibilityCalls
    {
        internal static readonly Version APIVersion = new Version(1, 2, 6);

        internal static string VersionString;

        private static Mod _clickerClass;

        internal static Mod ClickerClass => _clickerClass ?? (_clickerClass = ModLoader.GetMod("ClickerClass"));

        internal static void Load()
        {
            VersionString = APIVersion.ToString();
        }

        internal static void Unload()
        {
            _clickerClass = null;
            VersionString = null;
        }

        //Here is a list of available calls you can do. Call them where required/recommended like this: ClickerCompat.SetClickerWeaponDefaults(item);
        //If they return something, they will try to default to a sensible value if Clicker Class is not loaded
        //Will throw an exception if something isn't right. CHECK THE LOGS!

        /// <summary>
        /// Call in <see cref="ModItem.SetDefaults"/> to set important default fields for a clicker weapon. Set fields:
        /// useTime, useAnimation, useStyle, holdStyle, noMelee, shoot, shootSpeed.
        /// Only change them afterwards if you know what you are doing!
        /// </summary>
        /// <param name="item">The <see cref="Item"/> to set the defaults for</param>
        internal static void SetClickerWeaponDefaults(Item item) => ClickerClass?.Call("SetClickerWeaponDefaults", VersionString, item);

        /// <summary>
        /// Call this in <see cref="ModProjectile.SetStaticDefaults"/> to register this projectile into the "clicker class" category
        /// </summary>
        /// <param name="modProj">The <see cref="ModProjectile"/> that is to be registered</param>
        internal static void RegisterClickerProjectile(ModProjectile modProj) => ClickerClass?.Call("RegisterClickerProjectile", VersionString, modProj);

        /// <summary>
        /// Call this in <see cref="ModItem.SetStaticDefaults"/> to register this item into the "clicker class" category
        /// </summary>
        /// <param name="modItem">The <see cref="ModItem"/> that is to be registered</param>
        internal static void RegisterClickerItem(ModItem modItem) => ClickerClass?.Call("RegisterClickerItem", VersionString, modItem);

        /// <summary>
        /// Call this in <see cref="ModItem.SetStaticDefaults"/> to register this weapon into the "clicker class" category as a "clicker".
        /// Do not call <see cref="RegisterClickerItem"/> with it as this method does this already by itself
        /// </summary>
        /// <param name="modItem">The <see cref="ModItem"/> that is to be registered</param>
        /// <param name="borderTexture">The path to the border texture (optional)</param>
        internal static void RegisterClickerWeapon(ModItem modItem, string borderTexture = null) => ClickerClass?.Call("RegisterClickerWeapon", VersionString, modItem, borderTexture);

        /// <summary>
        /// Call this in <see cref="Mod.PostSetupContent"/> or <see cref="ModItem.SetStaticDefaults"/> to register this click effect
        /// </summary>
        /// <param name="mod">The mod this effect belongs to. ONLY USE YOUR OWN MOD INSTANCE FOR THIS!</param>
        /// <param name="internalName">The internal name of the effect. Turns into the unique name combined with the associated mod</param>
        /// <param name="displayName">The name of the effect, null if you use lang keys (Defaults to ClickEffect.[internalName].Name)</param>
        /// <param name="description">The basic description of the effect, string.Empty for none, null if you use lang keys (Defaults to ClickEffect.[internalName].Description)</param>
        /// <param name="amount">The amount of clicks required to trigger the effect</param>
        /// <param name="action">The method that runs when the effect is triggered</param>
        /// <returns>The unique identifier, null if an exception occured. READ THE LOGS!</returns>
        internal static string RegisterClickEffect(Mod mod, string internalName, string displayName, string description, int amount, Color color, Action<Player, Vector2, int, int, float> action) => ClickerClass?.Call("RegisterClickEffect", VersionString, mod, internalName, displayName, description, amount, color, action) as string;

        /// <summary>
        /// Returns the border texture of the item of this type
        /// </summary>
        /// <param name="type">The item type</param>
        /// <returns>The path to the border texture, null if not found</returns>
        internal static string GetPathToBorderTexture(int type) => ClickerClass?.Call("GetPathToBorderTexture", VersionString, type) as string;

        /// <summary>
        /// Returns all existing effects' internal names
        /// </summary>
        /// <returns>IEnumerable[string]</returns>
        internal static List<string> GetAllEffectNames() => ClickerClass?.Call("GetAllEffectNames", VersionString) as List<string>;

        /// <summary>
        /// Access an effect's stats. <see cref="null"/> if not found.
        /// "Mod": The mod the effect belongs to (string).
        /// | "InternalName": The internal name (string).
        /// | "UniqueName": The unique name (string).
        /// | "DisplayName": The displayed name (string).
        /// | "Description": The description (string).
        /// | "Amount": The amount of clicks to trigger the effect (int).
        /// | "Color": The color (Color).
        /// | "Action": The method ran when triggered (Action[Player, Vector2, int, int, float]).
        /// </summary>
        /// <param name="effect">The unique effect name</param>
        /// <returns>Dictionary[string, object]</returns>
        internal static Dictionary<string, object> GetClickEffectAsDict(string effect) => ClickerClass?.Call("GetClickEffectAsDict", VersionString, effect) as Dictionary<string, object>;

        /// <summary>
        /// Checks if an effect of this name exists
        /// </summary>
        /// <param name="effect">The unique name</param>
        /// <returns><see langword="true"/> if valid</returns>
        internal static bool IsClickEffect(string effect) => ClickerClass?.Call("IsClickEffect", VersionString, effect) as bool? ?? false;

        /// <summary>
        /// Call this to check if a projectile type belongs to the "clicker class" category
        /// </summary>
        /// <param name="type">The item type to be checked</param>
        /// <returns><see langword="true"/> if that category</returns>
        internal static bool IsClickerProj(int type) => ClickerClass?.Call("IsClickerProj", VersionString, type) as bool? ?? false;

        /// <summary>
        /// Call this to check if a projectile belongs to the "clicker class" category
        /// </summary>
        /// <param name="proj">The <see cref="Projectile"/> to be checked</param>
        /// <returns><see langword="true"/> if that category</returns>
        internal static bool IsClickerProj(Projectile proj) => ClickerClass?.Call("IsClickerProj", VersionString, proj) as bool? ?? false;

        /// <summary>
        /// Call this to check if an item type belongs to the "clicker class" category
        /// </summary>
        /// <param name="type">The item type to be checked</param>
        /// <returns><see langword="true"/> if that category</returns>
        internal static bool IsClickerItem(int type) => ClickerClass?.Call("IsClickerItem", VersionString, type) as bool? ?? false;

        /// <summary>
        /// Call this to check if an item belongs to the "clicker class" category
        /// </summary>
        /// <param name="item">The <see cref="Item"/> to be checked</param>
        /// <returns><see langword="true"/> if a "clicker class" item</returns>
        internal static bool IsClickerItem(Item item) => ClickerClass?.Call("IsClickerItem", VersionString, item) as bool? ?? false;

        /// <summary>
        /// Call this to check if an item type is a "clicker"
        /// </summary>
        /// <param name="type">The item type to be checked</param>
        /// <returns><see langword="true"/> if a "clicker"</returns>
        internal static bool IsClickerWeapon(int type) => ClickerClass?.Call("IsClickerWeapon", VersionString, type) as bool? ?? false;

        /// <summary>
        /// Call this to check if an item is a "clicker"
        /// </summary>
        /// <param name="item">The <see cref="Item"/> to be checked</param>
        /// <returns><see langword="true"/> if a "clicker"</returns>
        internal static bool IsClickerWeapon(Item item) => ClickerClass?.Call("IsClickerWeapon", VersionString, item) as bool? ?? false;

        /// <summary>
        /// Call in <see cref="ModItem.SetDefaults"/> for a clicker weapon to set its radius color
        /// </summary>
        /// <param name="item">The clicker weapon</param>
        /// <param name="color">The color</param>
        internal static void SetColor(Item item, Color color) => ClickerClass?.Call("SetColor", VersionString, item, color);

        /// <summary>
        /// Call in <see cref="ModItem.SetDefaults"/> for a clicker weapon to set its specific radius increase (1f means 100 pixel)
        /// </summary>
        /// <param name="item">The clicker weapon</param>
        /// <param name="radius">The additional radius</param>
        internal static void SetRadius(Item item, float radius) => ClickerClass?.Call("SetRadius", VersionString, item, radius);

        /// <summary>
        /// Call in <see cref="ModItem.SetDefaults"/> for a clicker weapon to add an effect to it
        /// </summary>
        /// <param name="item">The clicker weapon</param>
        /// <param name="effect">the unique effect name</param>
        internal static void AddEffect(Item item, string effect) => ClickerClass?.Call("AddEffect", VersionString, item, effect);

        /// <summary>
        /// Call in <see cref="ModItem.SetDefaults"/> for a clicker weapon to to add effects to it
        /// </summary>
        /// <param name="item">The clicker weapon</param>
        /// <param name="effects">the effect names</param>
        internal static void AddEffect(Item item, IEnumerable<string> effects) => ClickerClass?.Call("AddEffect", VersionString, item, effects);

        /// <summary>
        /// Call in <see cref="ModItem.SetDefaults"/> for a clicker weapon to set its dust type when it's used
        /// </summary>
        /// <param name="item">The clicker weapon</param>
        /// <param name="type">the dust type</param>
        internal static void SetDust(Item item, int type) => ClickerClass?.Call("SetDust", VersionString, item, type);

        /// <summary>
        /// Call in <see cref="ModItem.SetDefaults"/> for a clicker item to make it display total click count in the tooltip
        /// </summary>
        /// <param name="item">The clicker class item</param>
        internal static void SetDisplayTotalClicks(Item item) => ClickerClass?.Call("SetDisplayTotalClicks", VersionString, item);

        /// <summary>
        /// Call to get the players' clicker radius (multiply by 100 for pixels)
        /// </summary>
        /// <param name="player">The player</param>
        internal static float GetClickerRadius(Player player) => ClickerClass?.Call("GetPlayerStat", VersionString, player, "clickerRadius") as float? ?? 1f;

        /// <summary>
        /// Call to get the players' click amount (how many clicks done)
        /// </summary>
        /// <param name="player">The player</param>
        internal static int GetClickAmount(Player player) => ClickerClass?.Call("GetPlayerStat", VersionString, player, "clickAmount") as int? ?? 0;

        /// <summary>
        /// Call to get the players' clicks per second
        /// </summary>
        /// <param name="player">The player</param>
        internal static int GetClickerPerSecond(Player player) => ClickerClass?.Call("GetPlayerStat", VersionString, player, "clickerPerSecond") as int? ?? 0;

        /// <summary>
        /// Call to get the players' total clicks required for the given effect to trigger on the item
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="item">The clicker item</param>
        /// <param name="effect">The unique effect name</param>
        internal static int GetClickerAmountTotal(Player player, Item item, string effect) => ClickerClass?.Call("GetPlayerStat", VersionString, player, "clickerAmountTotal", item, effect) as int? ?? 1;

        /// <summary>
        /// Call to check if the player is wearing a specific set. Supported sets:
        /// Motherboard, Overclock, Precursor, Mice
        /// </summary>
        /// <param name="player">The player</param>
        internal static bool GetArmorSet(Player player, string set) => ClickerClass?.Call("GetArmorSet", VersionString, player, set) as bool? ?? false;

        /// <summary>
        /// Call to check if a specific accessory effect is enabled (i.e. "Gamer Crate" will have multiple effects enabled). Supported accessories:
        /// ChocolateChip, EnchantedLED, HandCream, StickyKeychain, GlassOfMilk, Cookie, ClickingGlove, AncientClickingGlove, RegalClickingGlove.
        /// Visual variants (i.e. EnchantedLED2) are not gettable
        /// </summary>
        /// <param name="player">The player</param>
        internal static bool GetAccessory(Player player, string accessory) => ClickerClass?.Call("GetAccessory", VersionString, player, accessory) as bool? ?? false;

        /// <summary>
        /// Call to set a specific player accessory effect (i.e. to emulate "Gamer Crate" you need to have set multiple effects). Supported accessories:
        /// ChocolateChip, EnchantedLED, HandCream, StickyKeychain, GlassOfMilk, Cookie, ClickingGlove, AncientClickingGlove, RegalClickingGlove.
        /// EnchantedLED and Cookie have a variant with "2" added to them that is a visual variation.
        /// </summary>
        /// <param name="player">The player</param>
        internal static void SetAccessory(Player player, string accessory) => ClickerClass?.Call("SetAccessory", VersionString, player, accessory);

        /// <summary>
        /// Call to add to the players' clicker crit value
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="add">crit chance added</param>
        internal static void SetClickerCritAdd(Player player, int add) => ClickerClass?.Call("SetPlayerStat", VersionString, player, "clickerCritAdd", add);

        /// <summary>
        /// Call to add to the players' clicker flat damage value
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="add">flat damage added</param>
        internal static void SetDamageFlatAdd(Player player, int add) => ClickerClass?.Call("SetPlayerStat", VersionString, player, "clickerDamageFlatAdd", add);

        /// <summary>
        /// Call to add to the players' clicker damage value in %
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="add">damage added in %</param>
        internal static void SetDamageAdd(Player player, float add) => ClickerClass?.Call("SetPlayerStat", VersionString, player, "clickerDamageAdd", add);

        /// <summary>
        /// Call to modify the players' effect threshold
        /// (2 will mean 2 less clicks required to reach the effect trigger threshold)
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="add">amount of clicks to reduce</param>
        internal static void SetClickerBonusAdd(Player player, int add) => ClickerClass?.Call("SetPlayerStat", VersionString, player, "clickerBonusAdd", add);

        /// <summary>
        /// Call to modify the players' effect threshold
        /// (-0.20f will mean 20% less clicks required to reach the effect trigger threshold)
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="add">% of total clicks</param>
        internal static void SetClickerBonusPercentAdd(Player player, float add) => ClickerClass?.Call("SetPlayerStat", VersionString, player, "clickerBonusPercentAdd", add);

        /// <summary>
        /// Call to add to the players' clicker radius (default 1f)
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="add">distance added in 100 pixels (1f = 100 pixel)</param>
        internal static void SetClickerRadiusAdd(Player player, float add) => ClickerClass?.Call("SetPlayerStat", VersionString, player, "clickerRadiusAdd", add);

        /// <summary>
        /// Enables the use of a click effect for this player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="effect">The unique effect name</param>
        internal static void EnableClickEffect(Player player, string effect) => ClickerClass?.Call("EnableClickEffect", VersionString, player, effect);

        /// <summary>
        /// Enables the use of click effects for this player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="effects">The unique effect names</param>
        internal static void EnableClickEffect(Player player, IEnumerable<string> effects) => ClickerClass?.Call("EnableClickEffect", VersionString, player, effects);

        /// <summary>
        /// Checks if the player has a click effect enabled
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="effect">The unique effect name</param>
        /// <returns><see langword="true"/> if enabled</returns>
        internal static bool HasClickEffect(Player player, string effect) => ClickerClass?.Call("HasClickEffect", VersionString, player, effect) as bool? ?? false;
    }
}