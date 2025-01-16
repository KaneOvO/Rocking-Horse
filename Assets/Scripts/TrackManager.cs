using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public List<GameObject> tracks_ = new List<GameObject>();
    private Dictionary<int, SwitchTrakInfo> trackDict_ = new Dictionary<int, SwitchTrakInfo>();

    void Start()
    {
        init();
    }

    void init()
    {
        for (int i = 0; i < tracks_.Count; i++)
        {
            trackDict_.Add(i, new SwitchTrakInfo(tracks_[i].transform.position.x));
        }
    }

    public bool CanSwitchLeft(int currentTrackIndex)
    {
        return currentTrackIndex > 0;
    }

    public bool CanSwitchRight(int currentTrackIndex)
    {
        return currentTrackIndex < tracks_.Count - 1;
    }

    public float SwitchLeft(int currentTrackIndex)
    {
        if(CanSwitchLeft(currentTrackIndex))
        {
            return trackDict_[currentTrackIndex - 1].coordinate;
        }
        else
        {
            return trackDict_[currentTrackIndex].coordinate;
        }
    }

    public float SwitchRight(int currentTrackIndex)
    {
        if (CanSwitchRight(currentTrackIndex))
        {
            return trackDict_[currentTrackIndex + 1].coordinate;
        }
        else
        {
            return trackDict_[currentTrackIndex].coordinate;
        }
    }

    public int GetCurrentTrackIndex(Vector3 PlayerPosition)
    {
        float minDistance = float.MaxValue;
        int currentTrackIndex = 0;
        for (int i = 0; i < tracks_.Count; i++)
        {
            float distance = Mathf.Abs(PlayerPosition.x - trackDict_[i].coordinate);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentTrackIndex = i;
            }
        }
        return currentTrackIndex;
    }   
}


