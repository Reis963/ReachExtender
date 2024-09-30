using EFT;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;

namespace ReachExtender.Patches {
    class InteractionRaycastPatch : ModulePatch {
        protected override MethodBase GetTargetMethod() => typeof(GameWorld).GetMethod("FindInteractable");

        [PatchPrefix]
        private static bool PatchPrefix(GameWorld __instance, ref GameObject __result, Ray ray, out RaycastHit hit) {
            __result = null;

            GameObject gameObject = EFTPhysicsClass.Raycast(ray, out hit, Mathf.Max(EFTHardSettings.Instance.LOOT_RAYCAST_DISTANCE, EFTHardSettings.Instance.PLAYER_RAYCAST_DISTANCE + EFTHardSettings.Instance.BEHIND_CAST), GameWorld.InteractiveLootMaskWPlayer) ? hit.collider.gameObject : null;
            if (gameObject) {
                // Get raycast to obstruction
                RaycastHit obstructionHit;
                if (!Physics.Linecast(ray.origin, hit.point, out obstructionHit, GameWorld.LootMaskObstruction)) {
                    // If theres no obstruction, return the gameobject of the loot item we found in the first raycast
                    __result = gameObject;
                } else {
                    // Distance between loot item and point that obstruction hit
                    float distSquared = (obstructionHit.point - gameObject.transform.position).sqrMagnitude;
                    if (distSquared <= ReachExtenderPlugin.DistanceFromWallSquared) {
                        __result = gameObject;
                    }
                }
            }

            return false;
        }
    }
}
