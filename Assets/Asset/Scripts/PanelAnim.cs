using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnim : MonoBehaviour
{
    public Transform mainContent;

    private void OnEnable()
    {
        mainContent.localScale = Vector3.zero;

        Sequence popupAnim = DOTween.Sequence();
        popupAnim.Append(mainContent.DOScale(1, 0.25f));
        //popupAnim.Append(mainContent.DOShakeScale(0.5f, 0.2f, 10, 45));
        popupAnim.Play();

    }



    public void DestroySelf()
    {
        mainContent.DOScale(0, 0.15f).OnComplete(() => {
            gameObject.SetActive(false);
        });
    }
}
