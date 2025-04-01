using UnityEngine;

public class CubeColor : MonoBehaviour
{
    [SerializeField]
    public Material whiteMaterial;

    [SerializeField]
    public Material redMaterial;

    [SerializeField]
    public Material blackMaterial;

    [SerializeField]
    public Material blueMaterial;

    [SerializeField]
    public MeshRenderer meshRenderer;

    public void SetColor(Color color)
    {
        if (color == Color.red)
        {
            meshRenderer.material = redMaterial;
            gameObject.name = "Cube Red";
        }
        else if(color == Color.black)
        {
            meshRenderer.material = blackMaterial;
            gameObject.name = "Cube Black";
        }
        else if (color == Color.blue)
        {
            meshRenderer.material = blueMaterial;
            gameObject.name = "Cube Blue";
        }
        else
        {
            meshRenderer.material = whiteMaterial;
            gameObject.name = "Cube White";
        }
    }
}
