using UnityEngine;

public class SoundEffector : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpSound, coinSound, winSound, loseSound, hitSound, gemSound, moveSound, powerUpSound;

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }
    public void PlayCoinSound()
    {
        audioSource.PlayOneShot(coinSound);
    }
    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }
    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }
    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitSound);
    }
    public void PlayGemSound()
    {
        audioSource.PlayOneShot(gemSound);
    }
    public void PlayMoveSound()
    {
        audioSource.PlayOneShot(moveSound);
    }
    
    public void PlayPowerUpSound()
    {
        audioSource.PlayOneShot(powerUpSound);
    }
}
