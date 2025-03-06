using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, scoreText, idText;
    [SerializeField] private Image avatarImage, rankImage;


    public void Initialize(string _id, string _name, string _score, Sprite avatar, Sprite rank)
    {
        idText.text = _id;
        nameText.text = _name;
        scoreText.text = _score;
        avatarImage.sprite = avatar;
        rankImage.sprite = rank;
    }
}
