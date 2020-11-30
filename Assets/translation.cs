using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class translation : MonoBehaviour
{
    public string ID;
    public string[] additionalIDs;
    [TextArea]
    public string EnglishString;
    [TextArea]
    public string ChineseString;
    public bool twoNewLines = true;
    TextMeshProUGUI tm;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (ID != null && ID != "")
        {
            tm = GetComponent<TextMeshProUGUI>();
            tm.text = "";
            translationText centralTranslation = ChantecailleARManager.Instance.gameObject.GetComponent<translationText>();

            while (! centralTranslation.translationsReady)
            {
                yield return null;
            }

            string language = ChantecailleARManager.Instance.language;

            string textToUse = null;

            foreach (singletranslation t in centralTranslation.translations)
            {
                if (t.id == ID)
                {
                    if (language == "chi")
                    {
                        tm.font = ChantecailleARManager.Instance.chineseFont;
                        textToUse += t.ChineseTranslation;
                    }
                    else
                    {
                        textToUse += t.EnglishTranslation;
                    }
                }
            }

            if (additionalIDs != null)
            {
                for (int i = 0; i < additionalIDs.Length; i++)
                {
                    foreach (singletranslation t in ChantecailleARManager.Instance.gameObject.GetComponent<translationText>().translations)
                    {
                        if (t.id == additionalIDs[i])
                        {
                            if (twoNewLines)
                                textToUse += "\n";
                            if (language == "chi")
                            {
                                textToUse += "\n" + t.ChineseTranslation;
                            }
                            else
                            {
                                textToUse += "\n" + t.EnglishTranslation;
                            }
                            
                        }
                    }
                }
            }
            if (textToUse.Length > 0)
                ReplaceText(textToUse);
        }
        //ChantecailleARManager.Instance.gameObject.GetComponent<translationText>().

        /*if (ChantecailleARManager.Instance.language == "eng")
        {
            ReplaceText(EnglishString);
        }
            
        else if (ChantecailleARManager.Instance.language == "chi")
        {
            ReplaceText(ChineseString);
            tm.font = ChantecailleARManager.Instance.chineseFont;
        }
        
        else
        {
            ReplaceText(EnglishString);
        }
        */

    }

    void ReplaceText(string newText)
    {
        if (newText.Length > 0 && newText != null)
            tm.text = newText;
    }

    
}
