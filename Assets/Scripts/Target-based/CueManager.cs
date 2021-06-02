using UnityEngine;
using UnityEngine.UI;

public class CueManager : MonoBehaviour
{
    public Material UIMaterial;
    public Sprite[] letterSprite;

    private GameObject canvas;
    private GameObject cueObj;
    private GameObject countdownTimerObj;
    private GameObject countdownLoadingBar;
    private GameObject center;
    private GameObject countdownValue;

    private System.Random rSeed;

    private void Awake()
    {
        canvas = new GameObject("Canvas");
        RectTransform rtCanvas = canvas.AddComponent<RectTransform>();
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.transform.SetParent(Camera.main.transform);

        cueObj = new GameObject("Cue");
        cueObj.transform.SetParent(canvas.transform);
        RectTransform rtCue = cueObj.AddComponent<RectTransform>();
        Image imgCue = cueObj.AddComponent<Image>();

        countdownTimerObj = new GameObject("CountdownTimer");
        countdownTimerObj.transform.SetParent(canvas.transform);
        RectTransform rtRadialProgressBar = countdownTimerObj.AddComponent<RectTransform>();
        Image imgRadialProgressBar = countdownTimerObj.AddComponent<Image>();

        countdownLoadingBar = new GameObject("LoadingBar");
        countdownLoadingBar.transform.SetParent(countdownTimerObj.transform);
        RectTransform rtLoadingBar = countdownLoadingBar.AddComponent<RectTransform>();
        Image imgLoadingBar = countdownLoadingBar.AddComponent<Image>();

        center = new GameObject("Center");
        center.transform.SetParent(countdownTimerObj.transform);
        RectTransform rtCenter = center.AddComponent<RectTransform>();
        Image imgCenter = center.AddComponent<Image>();

        countdownValue = new GameObject("CountdownValue");
        countdownValue.transform.SetParent(center.transform);
        RectTransform rtCountdownValue = countdownValue.AddComponent<RectTransform>();
        Text txtCountdownValue = countdownValue.AddComponent<Text>();

        rtCanvas.sizeDelta = new Vector2(200.0f, 200.0f);
        rtCanvas.anchorMax = new Vector2(0.0f, 0.0f);
        rtCanvas.anchorMin = new Vector2(0.0f, 0.0f);
        rtCanvas.pivot = new Vector2(0.5f, 0.5f);
        rtCanvas.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        rtCue.sizeDelta = new Vector2(200.0f, 200.0f);
        rtCue.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        imgCue.material = UIMaterial;
        imgCue.raycastTarget = false;

    //    cueObj.transform.localPosition = new Vector3(0.0f, -50.0f, 0.0f);

        countdownTimerObj.transform.localPosition = new Vector3(0.0f, 170.0f, 0.0f);
        countdownTimerObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        rtRadialProgressBar.sizeDelta = new Vector2(250.0f, 250.0f);
        imgRadialProgressBar.sprite = Resources.Load<Sprite>("Images/Circle");
        imgRadialProgressBar.color = new Color(188.0f / 255.0f, 188.0f / 255.0f, 188.0f / 255.0f);
        imgRadialProgressBar.material = UIMaterial;
        imgRadialProgressBar.raycastTarget = false;

        rtLoadingBar.sizeDelta = new Vector2(250.0f, 250.0f);
        imgLoadingBar.sprite = Resources.Load<Sprite>("Images/Circle");
        imgLoadingBar.color = new Color(120.0f / 255.0f, 120.0f / 255.0f, 120.0f / 255.0f);
        imgLoadingBar.material = UIMaterial;
        imgLoadingBar.raycastTarget = false;
        imgLoadingBar.type = Image.Type.Filled;
        imgLoadingBar.fillMethod = Image.FillMethod.Radial360;
        imgLoadingBar.fillOrigin = (int)Image.Origin360.Top;
        imgLoadingBar.fillAmount = 0.0f;
        imgLoadingBar.fillClockwise = false;

        rtCenter.sizeDelta = new Vector2(200.0f, 200.0f);
        imgCenter.sprite = Resources.Load<Sprite>("Images/Circle");
        imgCenter.material = UIMaterial;
        imgCenter.raycastTarget = false;

        rtCountdownValue.anchorMin = new Vector2(0.0f, 0.0f);
        rtCountdownValue.anchorMax = new Vector2(1.0f, 1.0f);
        rtCountdownValue.pivot = new Vector2(0.5f, 0.5f);
        rtCountdownValue.offsetMax = new Vector2(0.0f, 0.0f);
        rtCountdownValue.offsetMin = new Vector2(0.0f, 0.0f);
        txtCountdownValue.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        txtCountdownValue.fontSize = 100;
        txtCountdownValue.alignment = TextAnchor.MiddleCenter;
        txtCountdownValue.color = new Color(50.0f / 255.0f, 50.0f / 255.0f, 50.0f / 255.0f);
        txtCountdownValue.material = UIMaterial;
        txtCountdownValue.raycastTarget = false;
        txtCountdownValue.text = "2s";

        ShowCountdown(false);
        ShowCue(false);
        rSeed = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rtCanvas = canvas.GetComponent<RectTransform>();
        rtCanvas.localPosition = new Vector3(0.0f, 0.0f, 8.0f);
        rtCanvas.localRotation = Quaternion.identity;
    }

    public string GetCue()
    {
        int seed = rSeed.Next();
 
        System.Random rdm = new System.Random(seed);
        int typeId = rdm.Next(4);
        cueObj.GetComponent<Image>().sprite = letterSprite[typeId];
        return "" + letterSprite[typeId].name.ToCharArray()[0];
    }

    public void ShowCue(bool b)
    {
        cueObj.GetComponent<Image>().enabled = b;
    }

    public void ShowCountdown(bool b)
    {
        countdownTimerObj.GetComponent<Image>().enabled = b;
        countdownLoadingBar.GetComponent<Image>().enabled = b;
        center.GetComponent<Image>().enabled = b;
        countdownValue.GetComponent<Text>().enabled = b;
    }

    public void SetCountdownValue(float val, float percentage)
    {
        countdownLoadingBar.GetComponent<Image>().fillAmount = percentage;
        countdownValue.GetComponent<Text>().text = ((int)val + 1).ToString("N0") + "s";
    }
}
