using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Typewrite : MonoBehaviour
{
    private TMP_Text _tmpProText;
    private string writer;

    [SerializeField] float delayBeforeStart = 0f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] string leadingChar = "";
    [SerializeField] bool leadingCharBeforeDelay = false;

    void Start()
    {
        _tmpProText = GetComponent<TMP_Text>();

        if (_tmpProText != null)
        {
            writer = _tmpProText.text;
            _tmpProText.text = "";
            StartCoroutine(TypeWrite());
        }
    }

    IEnumerator TypeWrite()
    {
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";
        yield return new WaitForSeconds(delayBeforeStart);

        foreach (char c in writer)
        {
            if (_tmpProText.text.Length > 0)
            {
                _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
            }
            _tmpProText.text += c + leadingChar;
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (leadingChar != "")
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
        }
    }
}
