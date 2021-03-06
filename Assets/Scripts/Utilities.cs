using System;
using System.IO;
using System.Reflection;
using UnityEngine;

public static class Utilities {
    public static MethodInfo GetMethodInfo(Action a) => a.Method;

    public static Texture2D LoadPNG(string filePath) {
        if (!File.Exists(filePath)) {
            Debug.Log($"None: {new FileInfo(filePath).Directory?.FullName}");
            return null;
        }
        var fileData = File.ReadAllBytes(filePath);
        var tex = new Texture2D(2, 2, TextureFormat.BGRA32, false);
        tex.LoadImage(fileData);
        return tex;
    }

    public static Texture2D RoundCrop(Texture2D sourceTex) {
        var h = Math.Min(sourceTex.height, sourceTex.width);
        var r = h / 2;
        var c = sourceTex.GetPixels(0, 0, sourceTex.width, sourceTex.height);
        var b = new Texture2D(h, h);
        for (var i = 0; i < h * h; i++) {
            var y = Mathf.FloorToInt(i / (float) h);
            var x = Mathf.FloorToInt(i - (float) (y * h));
            if (r * r >= (x - r) * (x - r) + (y - r) * (y - r)) {
                b.SetPixel(x, y, c[i]);
            } else {
                b.SetPixel(x, y, Color.clear);
            }
        }
        b.Apply();
        return b;
    }
}