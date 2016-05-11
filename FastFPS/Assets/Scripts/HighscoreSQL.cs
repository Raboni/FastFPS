using UnityEngine;
using System.Collections;

public class HighscoreSQL : MonoBehaviour
{
    string url = "localhost/Highscore/unity.php";
    string player = "Robin2";
    int score = 120;

	// Use this for initialization
	IEnumerator Start ()
    {
	    //create webform to send to server
        WWWForm form = new WWWForm();

        //add name and score
        form.AddField("user", player);
        form.AddField("score", score);

        //encrypt
        form.AddField("hash", Md5Sum(player + score + "TheSecretWord"));

        //connect and send
        WWW send = new WWW(url, form);
        
        //wait until complete
        yield return send;

        if (!string.IsNullOrEmpty(send.error))
        {
            Debug.Log("highscore error " + send.error);
        }
        else
        {
            Debug.Log(send.text);
        }
	}

    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}
