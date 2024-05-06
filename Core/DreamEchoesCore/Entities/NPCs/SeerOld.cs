namespace DreamEchoes.Entities.NPCs;

public class SeerOld : MonoBehaviour
{
    void Start()
    {
        var template = Preloader.Get("RestingGrounds_07/Dream Moth");
        var instance = Instantiate(template, transform.position, transform.rotation, transform);
        instance.SetActive(true);
    }
}
