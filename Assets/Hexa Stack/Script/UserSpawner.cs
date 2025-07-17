using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserSpawner : MonoBehaviour
{
    [SerializeField] private GameObject UserPrefab;
    Dictionary<string, int[]> data = new Dictionary<string, int[]>();
    [SerializeField] UserData userData;

    [Header("Player")]
    [SerializeField] private TextMeshProUGUI namePlayerText, scorePlayerText, idPlayerText;
    [SerializeField] private Image avatarPlayerImage, rankPlayerImage;
    [Header("Top1")]
    [SerializeField] private TextMeshProUGUI nameText1, scoreText1, idText1;
    [SerializeField] private Image avatarImage1, rankImage1;
    [Header("Top2")]
    [SerializeField] private TextMeshProUGUI nameText2, scoreText2, idText2;
    [SerializeField] private Image avatarImage2, rankImage2;
    [Header("Top3")]
    [SerializeField] private TextMeshProUGUI nameText3, scoreText3, idText3;
    [SerializeField] private Image avatarImage3, rankImage3;
    private void Start()
    {
        data = UserData.instance.LoadData();

        GenerateUseObject();
    }
    private void GenerateUseObject()
    {
        for (int i=0; i<3; i++) {
            switch (i)
            {
                case 0:
                    nameText1.text = (i + 1).ToString();
                    nameText1.text = data.ElementAt(i).Key;
                    scoreText1.text = data.ElementAt(i).Value[2].ToString();
                    avatarImage1.sprite = userData.avataSpriteList[data.ElementAt(i).Value[1]];
                    rankImage1.sprite = userData.rankSpriteList[data.ElementAt(i).Value[0]];
                    break;
                case 1:
                    nameText2.text = (i + 1).ToString();
                    nameText2.text = data.ElementAt(i).Key;
                    scoreText2.text = data.ElementAt(i).Value[2].ToString();
                    avatarImage2.sprite = userData.avataSpriteList[data.ElementAt(i).Value[1]];
                    rankImage2.sprite = userData.rankSpriteList[data.ElementAt(i).Value[0]];
                    break;
                case 2:
                    nameText3.text = (i + 1).ToString();
                    nameText3.text = data.ElementAt(i).Key;
                    scoreText3.text = data.ElementAt(i).Value[2].ToString();
                    avatarImage3.sprite = userData.avataSpriteList[data.ElementAt(i).Value[1]];
                    rankImage3.sprite = userData.rankSpriteList[data.ElementAt(i).Value[0]];
                    break;
            }
                
        }
        for (int i = 3; i <31; i++)
        {
            GameObject newUser= Instantiate(UserPrefab, Vector3.zero,Quaternion.identity);
            newUser.transform.localScale= Vector3.one;
            newUser.transform.SetParent(transform);
            

            UserSetup user = newUser.GetComponent<UserSetup>();

            if (data.ElementAt(i).Key == GameData.instance.GetName())
                SetPlayerRank((i + 1).ToString(), data.ElementAt(i).Key, data.ElementAt(i).Value[2].ToString()
                    , userData.avataSpriteList[data.ElementAt(i).Value[1]], userData.rankSpriteList[data.ElementAt(i).Value[0]]);

            user.Initialize((i+1).ToString(), data.ElementAt(i).Key, data.ElementAt(i).Value[2].ToString()
                , userData.avataSpriteList[data.ElementAt(i).Value[1]], userData.rankSpriteList[data.ElementAt(i).Value[0]]);
        }
    }
    private void SetPlayerRank(string _id, string _name, string _score, Sprite avatar, Sprite rank)
    {
        idPlayerText.text = _id;
        namePlayerText.text = _name;
        scorePlayerText.text = _score;
        avatarPlayerImage.sprite = avatar;
        rankPlayerImage.sprite = rank;
    }
    


}
