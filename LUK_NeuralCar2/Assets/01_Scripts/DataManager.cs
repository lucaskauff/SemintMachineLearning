using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;


[XmlRoot("Data")]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public string path;

    private XmlSerializer serializer = new XmlSerializer(typeof(Data));
    private Encoding encoding = Encoding.GetEncoding("UTF-8");

    void Awake()
    {
        instance = this;
        SetPath();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPath()
    {
        path = Path.Combine(Application.persistentDataPath, "Data.xml");
    }

    public Data Load()
    {
        if(File.Exists(path))
        {
            return serializer.Deserialize(new FileStream(path, FileMode.Open)) as Data;
        }

        return null;
    }

    public void Save(List<NeuralNetwork> _nets)
    {
        serializer.Serialize(new StreamWriter(path, false, encoding), new Data{nets = _nets});
    }
}
