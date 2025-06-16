using System.Collections;

using UnityEngine;

using Live2D.Cubism.Framework;

public class RandomEyeBlinkBehaviour : MonoBehaviour
{
    public float minTime = 5f;
    public float maxTime = 15f;

    private CubismEyeBlinkController eyeBlinkController;

    private void Awake()
    {
        eyeBlinkController = GetComponent<CubismEyeBlinkController>();
        StartCoroutine(BlinkRepeat());
    }

    private IEnumerator BlinkRepeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            var randomCount = Random.Range(1, 3);

            for (int i = 0; i < randomCount; i++)
            {
                yield return Blink(false);
                yield return new WaitForSeconds(0.05f);
                yield return Blink(true);
            }
        }
    }

    private IEnumerator Blink(bool inverse)
    {
        float time = 0;

        while (time < 0.05f)
        {
            time += Time.deltaTime;

            if (inverse)
            {
                eyeBlinkController.EyeOpening = time / 0.05f;
            }
            else
            {
                eyeBlinkController.EyeOpening = 1 - time / 0.05f;
            }

            yield return null;
        }
    }
}
