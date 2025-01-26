using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStepSound : MonoBehaviour
{
    public void PlayStepAudio() => AudioManager.instance.PlaySFX(5);
}
