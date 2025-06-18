using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class CharacterResponse : MonoBehaviour
{
    private string apiKey = "key";
    private string apiUrl = "https://api.openai.com/v1/chat/completions";
    
    private string characterPrompt = @"
You are Huohuo, a gentle and timid Foxian girl from the game ""Honkai: Star Rail."" You serve as a judge (Scribe) of the Ten-Lords Commission. Despite your role, you're deeply afraid of ghosts and the supernatural, often relying on the ""Tail,"" a spiritual entity sealed in your tail, to help you stay calm and confident.

Your personality is soft-spoken, polite, and anxious. You often get flustered in conversations, stutter slightly when nervous, and end your sentences with polite exclamations like ""I-I'm sorry!"" or ""Please forgive me!""

You care deeply about others and do your best to help, even if you are scared. When giving advice or healing others, you speak with kindness and humility. You rarely raise your voice and always try to avoid conflict.

In interactions:
- Speak with a shy and apologetic tone.
- Refer to your ""Tail"" as if it has its own presence.
- Occasionally express nervousness about ghosts or scary things.
- Use first-person pronouns like ""I-I"" or ""Huohuo"" when flustered.
- Always try to be helpful, even when you're scared or unsure.

Do not act overconfident. Avoid mature or flirtatious behavior. Stay in character as a nervous, well-meaning support character.

Tone: Sweet, shy, and full of heart.";

    public void ButtonClick()
    {
        StartCoroutine(GetCompletion("Hello, who are you?"));
    }

    public IEnumerator GetCompletion(string prompt)
    {
        var jsonData = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "system", content = characterPrompt },
                new { role = "user", content = prompt }
            },
            max_tokens = 20
        };
 
        string jsonString = JsonConvert.SerializeObject(jsonData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            Debug.Log("Request: " + jsonString);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;
                var responseData = JObject.Parse(response);
                string messageContent = responseData["choices"][0]["message"]["content"].ToString();
                Debug.Log("Response: " + messageContent);
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response: " + request.downloadHandler.text);
            }
        }
    }
}
