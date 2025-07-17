using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    [SerializeField] private Image playerAvatar, avatar;
    [SerializeField] private TextMeshProUGUI namePlayer;
    private void Start()
    {
        namePlayer.text = GameData.instance.GetName();
        playerAvatar.sprite = UserData.instance.FindAvatarByName(GameData.instance.GetName());
        avatar.sprite = UserData.instance.FindAvatarByName(GameData.instance.GetName());
    }
}
