using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] int dotsNumber;
    [SerializeField] GameObject DotsParent;
    [SerializeField] GameObject DotsPrefab;
    [SerializeField] float dotSpacing;
    [SerializeField] [Range(0.5f, 1f)] float dotMinScale;
    [SerializeField] [Range(1f, 2f)] float dotMaxScale;

    Transform[] dotsList;
    Vector2 pos;
    float TimeStamp;

    private void Start()
    {
        Hide();
        prepareDots();
    }

    void prepareDots()
    {
        dotsList = new Transform[dotsNumber];
        DotsPrefab.transform.localScale = Vector3.one * dotMaxScale;

        float scale = dotMaxScale;
        float scalefactor = scale / dotsNumber;

        for(int i = 0; i < dotsNumber; i++)
        {
            dotsList[i] = Instantiate(DotsPrefab, null).transform;
            dotsList[i].parent = DotsParent.transform;

            dotsList[i].localScale = Vector3.one * scale;
            if(scale > dotMinScale)
            {
                scale -= scalefactor;
            }
        }
    }

    public void ApplyTrajectoryColor(Color color)
    {
        foreach (Transform dot in dotsList)
        {
            if (dot.TryGetComponent<SpriteRenderer>(out SpriteRenderer renderer))
            {
                renderer.color = color;
            }
        }
    }

    public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
    {
        TimeStamp = dotSpacing;
        for(int i = 0; i < dotsNumber; i++)
        {
            pos.x = (ballPos.x + forceApplied.x * TimeStamp);
            pos.y = (ballPos.y + forceApplied.y * TimeStamp) - (Physics2D.gravity.magnitude * TimeStamp * TimeStamp) / 2f;

            dotsList[i].position = pos;
            TimeStamp += dotSpacing;
        }
    }

    public void Show()
    {
        DotsParent.SetActive(true);
    }
    public void Hide()
    {
        DotsParent.SetActive(false);
    }
}
