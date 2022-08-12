using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusMessage : MonoBehaviour
{

    public TextMeshProUGUI messageText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetText(string t)
    {
        messageText.text = t;
    }

    public void Show()
    {
        ShowCoroutine(gameObject);
    }


    public void FadeIn(GameObject obj)
    {
        obj.SetActive(true);
        StartCoroutine(FadeInCoroutine(obj));
    }

    public void FadeOut(GameObject obj)
    {
        StartCoroutine(FadeOutCoroutine(obj));
    }



    private IEnumerator ShowCoroutine(GameObject obj)
    {
        //StartCoroutine(FadeInCoroutine(obj));

        Renderer r = obj.GetComponent<SpriteRenderer>();
        Color c = r.material.color;

        for (float alpha = 0f; alpha <= 1f; alpha += 0.05f)
        {
            c.a = alpha;
            yield return null;
        }


        yield return new WaitForSeconds(4f);


        for (float alpha = 1f; alpha >= 0f; alpha -= 0.05f)
        {
            c.a = alpha;
            yield return null;
        }


        //StartCoroutine(FadeOutCoroutine(obj));
        obj.SetActive(false);
    }

    private IEnumerator FadeInCoroutine(GameObject obj)
    {
        Renderer r = obj.GetComponent<SpriteRenderer>();
        Color c = r.material.color;

        for (float alpha = 0f; alpha <= 1f; alpha += 0.05f)
        {
            c.a = alpha;
            yield return null;
        }

        yield break;
    }

    private IEnumerator FadeOutCoroutine(GameObject obj)
    {
        Renderer r = obj.GetComponent<SpriteRenderer>();
        Color c = r.material.color;

        for (float alpha = 1f; alpha >= 0f; alpha -= 0.05f)
        {
            c.a = alpha;
            yield return null;
        }
        obj.SetActive(false);
        yield break;
    }
}
