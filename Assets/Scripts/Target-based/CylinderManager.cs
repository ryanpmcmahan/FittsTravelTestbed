using System.Collections;
using UnityEngine;

public enum CYLINDERSTATE { DEFAULT, TARGET, INSIDE };

public class CylinderManager : MonoBehaviour
{
    [Header("Task variables:")]
    public int cylinderCount;
    public float fieldDiameter;

    [Header("Cylinder variables:")]
    public float cylinderDiameter;

    public GameObject cylinderModel;
    public Material targetMaterial;
    public Material insideMaterial;
    public Material defaultMaterial;

    private const float CYLINDER_HEIGHT = 6.0f;
    private ArrayList cylinderLst;
    private ArrayList cylinderStateLst;

    void Awake()
    {
        cylinderLst = new ArrayList();
        cylinderStateLst = new ArrayList();

        LayoutCylinder();

        UpdateCylinderMaterial();
    }

    private void LayoutCylinder()
    {
        float angleBetweenCylinder = 2.0f * Mathf.PI / cylinderCount;
        float currAngle = 0.0f;

        for (int i = 0; i < cylinderCount; i++)
        {
            if (cylinderCount % 2 != 0)
                currAngle = cylinderCount / 2 * angleBetweenCylinder * i;
            else
                currAngle = cylinderCount / 2 * angleBetweenCylinder * i + angleBetweenCylinder * (i / 2);

            float x = Mathf.Cos(currAngle) * (fieldDiameter + cylinderDiameter) / 2.0f;
            float z = Mathf.Sin(currAngle) * (fieldDiameter + cylinderDiameter) / 2.0f;
            float y = CYLINDER_HEIGHT / 2.0f;

            GameObject cylinder = Instantiate(cylinderModel) as GameObject;
            cylinder.transform.position = new Vector3(z, y, x);
            cylinder.transform.localScale = new Vector3(cylinderDiameter, y, cylinderDiameter);
            cylinderStateLst.Add(CYLINDERSTATE.DEFAULT);
            cylinderLst.Add(cylinder);
        }
    }

    public void UpdateCylinderMaterial()
    {
        for (int i = 0; i < cylinderCount; i++)
        {
            switch ((CYLINDERSTATE)cylinderStateLst[i])
            {
                case CYLINDERSTATE.DEFAULT:
                    (cylinderLst[i] as GameObject).GetComponent<Renderer>().material = defaultMaterial;
                    break;
                case CYLINDERSTATE.TARGET:
                    (cylinderLst[i] as GameObject).GetComponent<Renderer>().material = targetMaterial;
                    break;
                case CYLINDERSTATE.INSIDE:
                    (cylinderLst[i] as GameObject).GetComponent<Renderer>().material = insideMaterial;
                    break;
                default:
                    (cylinderLst[i] as GameObject).GetComponent<Renderer>().material = defaultMaterial;
                    break;
            }
        }
    }

    public Vector3 GetCylinderPosition(int id)
    {
        if (id < cylinderCount && id >= 0)
            return (cylinderLst[id] as GameObject).transform.position;
        else
            return Vector3.zero;
    }

    public bool SetCylinderState(int id, CYLINDERSTATE state)
    {
        if (id < cylinderCount && id >= 0)
        {
            cylinderStateLst[id] = state;
            return true;
        }
        else
            return false;
    }
}
