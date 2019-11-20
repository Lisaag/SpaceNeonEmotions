using UnityEngine;

public class XYDemo : MonoBehaviour
{
    int baseline = 70;
    int oldHeartrate;
    public int heartrate;
    XYStreamReader reader;
    public GameObject example;
    Material nonPermanentMaterial;
    Renderer rend;

    void Start()
    {
        rend = example.GetComponent<Renderer>();
        InvokeRepeating("Read", 1.0f, 1.0f);
        //material.EnableKeyword("_EMISSION")
    }

    void OnDestroy()
    {
        reader.Dispose();
    }

    void Read()
    {
        reader = XYStreamReader.FromFile("C:\\Users\\User\\Desktop\\Heartrate\\sample.txt");
        print("Read...");
        reader.Read();
        if (reader.heartrate != heartrate)
        {
            heartrate = reader.heartrate;
            float newRed = (heartrate - baseline) * 5;
            if (newRed > 255)
            {
                newRed = 255;
            }
            Color c = new Color(100, 0, 0);
            rend.sharedMaterial.SetColor("_EmissionColor", new Vector4(c.r,c.g,c.b, 1));
        }
        reader.Dispose();
        Debug.Log(reader.heartrate);
    }
}