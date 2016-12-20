using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Draw : MonoBehaviour {

    public Texture2D customBrush;

    private string targetTexture = "_MainTex"; // target texture for this material shader (usually _MainTex)
    private FilterMode filterMode = FilterMode.Point;
    private Texture2D tex; // texture that we paint into (it gets updated from pixels[] array when painted)
    private int texWidth = 1920;
    private int texHeight = 1080;
    private byte[] pixels; // byte array for texture painting, this is the image that we paint into.

    private Dictionary<string, byte[]> dicBrush = new Dictionary<string, byte[]>();
    private byte[] customBrushBytes;
    private int customBrushWidth;
    private int customBrushHeight;
    private int customBrushWidthHalf;
    private int texWidthMinusCustomBrushWidth;
    private int texHeightMinusCustomBrushHeight;
    private float brushAlphaStrength = 1f; // multiplier to soften brush additive alpha, 0.1f is nice & smooth, 1 = faster

    private bool textureNeedsUpdate = true; // if we have modified texture

    void Start() {
        texWidth = GeneralDefine.Instance.defaultSceenWidth;
        texHeight = GeneralDefine.Instance.defaultSceenHeight;
        InitializeEverything();

        //SetBrush("branches1");
        //DrawBrush(500, 500);
        //SetBrush("branches2");
        //DrawBrush(500, 600);
    }

    void Update() {
        UpdateTexture();
    }

    private void UpdateTexture() {
        if (textureNeedsUpdate) {
            textureNeedsUpdate = false;
            tex.LoadRawTextureData(pixels);
            tex.Apply(false);
        }
    }

    private void InitializeEverything() {
        CreateFullScreenQuad();
        if (!GetComponent<Renderer>().material.HasProperty(targetTexture)) 
            Debug.LogError("Fatal error: Current shader doesn't have a property: '" + targetTexture + "'");

        if (GetComponent<Renderer>().material.GetTexture(targetTexture) == null) {
            tex = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
            GetComponent<Renderer>().material.SetTexture(targetTexture, tex);
            pixels = new byte[texWidth * texHeight * 4];
        }
        GetComponent<Renderer>().sortingOrder = -950;

        tex.filterMode = filterMode;
        tex.wrapMode = TextureWrapMode.Clamp;
    }

    private void CreateFullScreenQuad() {
        float halfRoomWidth = GeneralDefine.Instance.roomSizeWidth / 2;
        float halfRoomHeight = GeneralDefine.Instance.roomSizeHeight / 2;
        Mesh go_Mesh = GetComponent<MeshFilter>().mesh;
        go_Mesh.Clear();
        go_Mesh.vertices = new[] {
				new Vector3(-halfRoomWidth, -halfRoomHeight, 0),
				new Vector3(-halfRoomWidth, halfRoomHeight, 0),
				new Vector3(halfRoomWidth, halfRoomHeight, 0),
				new Vector3(halfRoomWidth, -halfRoomHeight, 0),
			};
        go_Mesh.uv = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
        go_Mesh.triangles = new[] { 0, 1, 2, 0, 2, 3 };
        go_Mesh.RecalculateNormals();
        go_Mesh.tangents = new[] { new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f), new Vector4(1.0f, 0.0f, 0.0f, -1.0f) };
    }

    public void DrawTexture(Vector3 _pos, Texture2D _tex) {
        if (_tex == null) {
            Debug.LogError("tex is null");
            return;
        }
        SetBrush(_tex);
        DrawBrush(1920 / 2 + _pos.x * 1920 / 46, 1080 / 2 +_pos.y * 1080 / 26);
    }

    public void DrawTexture(Vector3 _pos, string _name) {
        SetBrush(_name);
        DrawBrush(1920 / 2 + _pos.x * 1920 / 46, 1080 / 2 + _pos.y * 1080 / 26);
    }

    public void SetBrush(string spriteName) {
        Sprite sprite = LocalAssetMgr.Instance.Load_UISprite("Branches", spriteName);
        SetBrush(sprite.texture);
    }

    public void SetBrush(Texture2D _tex) {
        string brushName = _tex.name;
        customBrush = _tex;
        customBrushWidth = customBrush.width;
        customBrushHeight = customBrush.height;

        if (dicBrush.ContainsKey(brushName)) {
            customBrushBytes = dicBrush[brushName];
        }
        else {
            customBrushBytes = new byte[customBrushWidth * customBrushHeight * 4];

            int pixel = 0;
            for (int y = 0; y < customBrushHeight; y++) {
                for (int x = 0; x < customBrushWidth; x++) {
                    Color brushPixel = customBrush.GetPixel(x, y);
                    customBrushBytes[pixel] = (byte)(brushPixel.r * 255);
                    customBrushBytes[pixel + 1] = (byte)(brushPixel.g * 255);
                    customBrushBytes[pixel + 2] = (byte)(brushPixel.b * 255);
                    customBrushBytes[pixel + 3] = (byte)(brushPixel.a * 255);
                    pixel += 4;
                }
            }

            dicBrush.Add(brushName, customBrushBytes);
        }

        customBrushWidthHalf = (int)(customBrushWidth * 0.5f);
        texWidthMinusCustomBrushWidth = texWidth - customBrushWidth;
        texHeightMinusCustomBrushHeight = texHeight - customBrushHeight;
    }

    public void DrawBrush(float px, float py) {
        textureNeedsUpdate = true;
        int startX = (int)(px - customBrushWidthHalf);
        int startY = (int)(py - customBrushWidthHalf);
        if (startX < 0) {
            startX = 0;
        }
        else {
            if (startX + customBrushWidth >= texWidth) startX = texWidthMinusCustomBrushWidth;
        }
        if (startY < 1)
			{
            startY = 1;
        }
        else {
            if (startY + customBrushHeight >= texHeight) startY = texHeightMinusCustomBrushHeight;
        }
        int pixel = (texWidth * startY + startX) * 4;
        int brushPixel = 0;
        for (int y = 0; y < customBrushHeight; y++) {
            for (int x = 0; x < customBrushWidth; x++) {
                brushPixel = (customBrushWidth * (y) + x) * 4;
                if (customBrushBytes[brushPixel + 3] > 0) {
                    // no additive colors
                    pixels[pixel] = customBrushBytes[brushPixel];
                    pixels[pixel + 1] = customBrushBytes[brushPixel + 1];
                    pixels[pixel + 2] = customBrushBytes[brushPixel + 2];
                    pixels[pixel + 3] = customBrushBytes[brushPixel + 3];

                    // additive over white also
                    //pixels[pixel] = (byte)Mathf.Lerp(pixels[pixel], customBrushBytes[brushPixel], customBrushBytes[brushPixel + 3] * brushAlphaStrength);
                    //pixels[pixel + 1] = (byte)Mathf.Lerp(pixels[pixel + 1], customBrushBytes[brushPixel + 1], customBrushBytes[brushPixel + 3] * brushAlphaStrength);
                    //pixels[pixel + 2] = (byte)Mathf.Lerp(pixels[pixel + 2], customBrushBytes[brushPixel + 2], customBrushBytes[brushPixel + 3] * brushAlphaStrength);
                    //pixels[pixel + 3] = (byte)Mathf.Lerp(pixels[pixel + 3], customBrushBytes[brushPixel + 3], customBrushBytes[brushPixel + 3] * brushAlphaStrength);

                    //pixels[pixel] = (byte)Mathf.Lerp(pixels[pixel], customBrushBytes[brushPixel], brushAlphaStrength);
                    //pixels[pixel + 1] = (byte)Mathf.Lerp(pixels[pixel + 1], customBrushBytes[brushPixel + 1], brushAlphaStrength);
                    //pixels[pixel + 2] = (byte)Mathf.Lerp(pixels[pixel + 2], customBrushBytes[brushPixel + 2], brushAlphaStrength);
                    //pixels[pixel + 3] = (byte)Mathf.Lerp(pixels[pixel + 3], customBrushBytes[brushPixel + 3], brushAlphaStrength);
                }
                pixel += 4;
            }
            pixel = (texWidth * (startY == 0 ? 1 : startY + y) + startX + 1) * 4;
        }
    }
}
