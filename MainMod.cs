using Il2Cpp;
using Il2CppPhoton.Pun;
using MelonLoader;
using PortaBoard;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine;
using Il2CppLocomotion;

[assembly: MelonInfo(typeof(MainMod), ModInfo.NAME, ModInfo.VERSION, ModInfo.AUTHOR)]

namespace PortaBoard
{
    public class MainMod : MelonMod
    {
        GameObject playerBoard;
        bool enabled = false;
        bool canPress = true;
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (SceneManager.GetActiveScene().name == "CapuchinCopy")
            {
                if (playerBoard == null)
                {
                    playerBoard = UnityEngine.Object.Instantiate(GameObject.Find("Global/Levels/Forest/Playerboard"));
                    playerBoard.transform.parent = Player.Instance.playerCam.transform;
                    playerBoard.SetActive(false);
                }
                playerBoard.transform.localPosition = new Vector3(0, 0, 0.6f);
                playerBoard.transform.localEulerAngles = new Vector3(0, 90, 0);
                playerBoard.transform.localScale = new Vector3(0.1604897f, 0.6411243f, 1.252927f); //idk it just looks good enough
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimary);
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimary);
                if (canPress)
                {
                    if (leftPrimary && rightPrimary && enabled && canPress)
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
                    if (leftPrimary && rightPrimary && !enabled && canPress)
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
                else if (!leftPrimary && !rightPrimary)
                {
                    canPress = true;
                }

            }
        }
    }
}