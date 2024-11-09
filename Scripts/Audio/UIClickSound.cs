using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickSound : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.uiClickClip);
    }
}
