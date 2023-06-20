using System;
using System.Collections;
using System.Collections.Generic;
using CommonScripts;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class TestLineRenderer : MonoBehaviour
{
    public LineRenderer _LineRenderer;

    [Range(0.05f, 1)] public float Width = 0.05f;
    
    [Range(10, 100)] public int SegmentNum = 10;
    
    [Range(1, 100)] public float Speed = 10f;
    [Range(0, 90)] public float Angle = 45f;

    private void Start()
    {
        

        GetParabolaLine();
    }

    private void Update()
    {
        _LineRenderer.startWidth = Width;
        _LineRenderer.endWidth = Width;
        
        GetParabolaLine();
    }

    private void GetParabolaLine()
    {
        float startSpeed = Speed;
        float startAngle = Angle;

        var aTr = math.PI / 180f;

        var vh = startSpeed * math.sin(startAngle * aTr);
        var vs = startSpeed * math.cos(startAngle * aTr);

        var tMax = vs / 9.81f;

        var forward = Vector3.right;

        var sh = vh * tMax - 9.81f * tMax * tMax / 2f;
        
        var p1 = new Vector3(vs * tMax, sh + 1.8f, 0);

        var t1 = 1.8f / 9.81f;
        
        var p2 = new Vector3(vs * (2 * tMax + t1), 0, 0);

        if (sh <= 0f)
        {
            var points = BezierUtils.GetLineBezierList(transform.position, p2, SegmentNum);
            _LineRenderer.positionCount = points.Length;
            _LineRenderer.SetPositions(points);
        }
        else
        {
            var points = BezierUtils.GetLineBezierList(transform.position, p1, p2, SegmentNum);
            _LineRenderer.positionCount = points.Length;
            _LineRenderer.SetPositions(points);
        }
    }
    
    
}
