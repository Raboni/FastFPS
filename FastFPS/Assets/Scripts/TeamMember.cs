﻿using UnityEngine;
using System.Collections;

public class TeamMember : MonoBehaviour
{
    public string Name = "New Player";
    public int Team = 0;
    public int Kills = 0;
    public int Deaths = 0;

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(Name);
            stream.SendNext(Team);
            stream.SendNext(Kills);
            stream.SendNext(Deaths);
        }
        else
        {
            Name = (string)stream.ReceiveNext();
            Team = (int)stream.ReceiveNext();
            Kills = (int)stream.ReceiveNext();
            Deaths = (int)stream.ReceiveNext();
        }
    }
}
