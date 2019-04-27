using System.Reflection;
using BepInEx;
using Harmony;
using static CustomKeybindings;

namespace HideBackpacks
{
    [BepInPlugin("com.gnivler.HideBackpacks", "HideBackpacks", "1.0")]
    public class HideBackpacks : BaseUnityPlugin
    {
        public void Awake()
        {
            var harmony = HarmonyInstance.Create("com.gnivler.HideBackpacks");
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
                var playerID = __instance.ControlledCharacter.OwnerPlayerSys.PlayerID;
                if (m_playerInputManager[playerID]
                    .GetButtonDown("Backpack Visible (Toggle)"))
                {
                    bagVisible = !bagVisible;
                }

                if (!__instance.ControlledCharacter.Inventory.EquippedBag) return;
                if (bagVisible)
                {
                    __instance.ControlledCharacter.Inventory.EquippedBag.m_loadedVisual.Show();
                }
                else
                {
                    __instance.ControlledCharacter.Inventory.EquippedBag.m_loadedVisual.Hide();
                }
            }
        }
    }
}