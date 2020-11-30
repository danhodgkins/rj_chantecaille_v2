using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class translationText : MonoBehaviour
{
    public bool translationsReady;
    public List<singletranslation> translations = new List<singletranslation>();
    public bool hummingbirdMode;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        TextAsset translationDoc;
        if (hummingbirdMode) 
            translationDoc = Resources.Load<TextAsset>("hummingbird");
        else
            translationDoc = Resources.Load<TextAsset>("chantecailleTranslationTab");

        string[] data = translationDoc.text.Split(new char[] { '\n' });
        
        for (int i = 1; i < data.Length; i++)
        {
            yield return null;
            
            string[] row = data[i].Split(new char[] { '\t' });
            
            singletranslation t = new singletranslation
            {
                id = row[2],
                EnglishTranslation = row[4],
                ChineseTranslation = row[5]
            };

            translations.Add(t);
        }

        while (ChantecailleARManager.Instance.language == "") {
            yield return null;
        }

        translationsReady = true;
    }

    public void selectLanguage(string language)
    {

    }
}
