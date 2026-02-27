
using UnityEngine;
using UnityEngine.UI;
public class DamageCanvas : MonoBehaviour
{
    [SerializeField] Image DamagePanel;
    [SerializeField] float alpha = 0f;
    [SerializeField] float fadeSpeed = 10f;
    bool isFading = false;
    bool isGameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.isGameOver&&!isGameOver)
        {
            GameManager.Instance.isDamage = false;
            isGameOver = true;
            alpha = 0f;
            DamagePanel.color = new Color(1f, 1f, 1f, alpha);
            isFading = false;
            gameObject.SetActive(false);
            return;
        }
        else if(isGameOver&&!GameManager.Instance.isGameOver)
        {
            isGameOver = false;
            alpha = 0f;
            DamagePanel.color = new Color(1f, 1f, 1f, alpha);
            isFading = false;
        }
        if (GameManager.Instance.isDamage)
        {
            if (!isFading)
            {
                alpha = 1f; // ダメージを受けたときの初期アルファ値
                isFading = true;
            }
            alpha -= fadeSpeed * Time.deltaTime;
            if (alpha <= 0f)
            {
                alpha = 0f;
                GameManager.Instance.isDamage = false;
                isFading = false;
            }
            DamagePanel.color = new Color(1f, 1f, 1f, alpha);
        }

    }
}
