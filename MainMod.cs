using Il2Cpp;
using Il2CppPhoton.Pun;
using MelonLoader;
using PortaBoard;
using UnityEngine.XR;
using UnityEngine;
using Il2CppLocomotion;

[assembly: MelonInfo(typeof(MainMod), ModInfo.NAME, ModInfo.VERSION, ModInfo.AUTHOR)]

namespace PortaBoard
{
    public class MainMod : MelonMod
    {
        private GameObject playerBoard;
        private bool enabled, canPress = true;
        private bool hasInit;

        public override void OnUpdate()
        {
            if (!hasInit && Player.Instance != null)
            {
                hasInit = true;
                playerBoard = UnityEngine.Object.Instantiate(
                    original: GameObject.Find("Global/Levels/Forest/Playerboard"),
                    parent: Player.Instance.playerCam.transform);
                playerBoard.transform.localPosition = new Vector3(0, 0, 0.6f);
                playerBoard.transform.localEulerAngles = new Vector3(0, 90, 0);
                playerBoard.transform.localScale =
                    new Vector3(0.1604897f, 0.6411243f, 1.252927f); //idk it just looks good enough
                playerBoard.SetActive(false);
            }
            else if (!hasInit) return;

            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand)
                .TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimary);
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand)
                .TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimary);

            if (canPress && leftPrimary && rightPrimary)
            {
                if (enabled)
                {
                    canPress = false;
                    playerBoard.SetActive(false);

                    Player.Instance.LeftCollider.isTrigger = false;
                    Player.Instance.RightCollider.isTrigger = false;

                    if (PhotonNetwork.InRoom)
                    {
                        FusionPlayer.Instance.LeftHandLocation.gameObject.SetActive(true);
                        FusionPlayer.Instance.RightHandLocation.gameObject.SetActive(true);
                    }

                    enabled = !enabled;
                }
                else
                {
                    canPress = false;
                    playerBoard.SetActive(true);

                    Player.Instance.LeftCollider.isTrigger = true;
                    Player.Instance.RightCollider.isTrigger = true;

                    if (PhotonNetwork.InRoom)
                    {
                        FusionPlayer.Instance.LeftHandLocation.gameObject.SetActive(false);
                        FusionPlayer.Instance.RightHandLocation.gameObject.SetActive(false);
                    }

                    enabled = !enabled;
                }
            }
            else if (!canPress && !leftPrimary && !rightPrimary)
            {
                canPress = true;
            }
        }
    }
}
