using System.Reflection;
using BepInEx;
using Harmony;
using static CustomKeybindings;

namespace HideBackpacks
{
    [BepInPlugin("com.gnivler.HideBackpacks", "HideBackpacks", "1.3")]
    public class HideBackpacks : BaseUnityPlugin
    {
        public void Awake()
        {
            var harmony = HarmonyInstance.Create("com.gnivler.HideBackpacks.Outward");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            AddAction(
                "Backpack Visible (Toggle)",
                KeybindingsCategory.Actions,
                ControlType.Both, 5);
        }

        [HarmonyPatch(typeof(PlayerSystem), "Update", MethodType.Normal)]
        public class UpdatePatch
        {
            private static bool bagVisible = true;

            public static void Postfix(PlayerSystem __instance)
            {
                if (!__instance.ControlledCharacter.Inventory.EquippedBag) return;
                
                var playerID = __instance.ControlledCharacter.OwnerPlayerSys.PlayerID;
                if (m_playerInputManager[playerID]
                    .GetButtonDown("Backpack Visible (Toggle)"))
                {
                    bagVisible = !bagVisible;
                }

                var itemVisual = Traverse.Create(__instance.ControlledCharacter.Inventory.EquippedBag)
                    .Field("m_loadedVisual").GetValue<ItemVisual>();
                if (bagVisible && !itemVisual.IsVisible)
                {
                    itemVisual.Show();
                }

                if (!bagVisible && itemVisual.IsVisible)
                {
                    itemVisual.Hide();
                }
            }
        }
    }
}