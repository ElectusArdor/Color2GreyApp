using UnityEngine;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using UI = UnityEngine.UI;

public class Converter : MonoBehaviour
{
    [SerializeField] private UI.Image OrigImg, ConvImg;
    [SerializeField] private UI.Text TextInfo;
    [SerializeField] private UI.Button ConvertBtn;

    private Sprite newSprite;
    private Bitmap origImgBitmap, convImgBitmap;
    private Image convImg;
    private string fileName;

    private void Start()
    {
        Screen.SetResolution(Mathf.RoundToInt(Screen.width * 0.8f), Mathf.RoundToInt(Screen.height * 0.8f), false, 0);
    }

    public void LoadImg()
    {
        FileSelector.GetFile(GotFile);
    }

    public void ConvertImg()
    {
        SaveGrayImg(CreateGreyImage());
        ConvImg.sprite = CreateNewSprite("Gray_" + fileName + ".png");
        TextInfo.text = "Изображение " + fileName + ".png сохранено.";
        ConvertBtn.interactable = false;
    }

    private Bitmap CreateGreyImage()
    {
        if (origImgBitmap != null)
        {
            convImgBitmap = new Bitmap(origImgBitmap.Width, origImgBitmap.Height);

            for (int i = 0; i < origImgBitmap.Width; i++)
            {
                for (int j = 0; j < origImgBitmap.Height; j++)
                {
                    uint pixel = (uint)origImgBitmap.GetPixel(i, j).ToArgb();

                    float A = (float)(pixel & 0xFF000000);
                    float R = (float)((pixel & 0x00FF0000) >> 16);
                    float G = (float)((pixel & 0x0000FF00) >> 8);
                    float B = (float)(pixel & 0x000000FF);

                    R = G = B = (R + G + B) / 3f;
                    uint newPixel = (uint)A | ((uint)R << 16) | ((uint)G << 8) | (uint)B;

                    convImgBitmap.SetPixel(i, j, System.Drawing.Color.FromArgb((int)newPixel));
                }
            }
        }
        return convImgBitmap;
    }

    private Image SaveGrayImg(Bitmap convImgBitmap)
    {
        convImg = convImgBitmap;
        convImg.Save("Gray_" + fileName + ".png", ImageFormat.Png);
        
        return convImg;
    }

    private void GotFile(FileSelector.Status status, string path)
    {
        if (status == FileSelector.Status.Successful)
        {
            try
            {
                origImgBitmap = new Bitmap(path);
                OrigImg.sprite = CreateNewSprite(path);
                ConvertBtn.interactable = true;
            }
            catch
            {
                TextInfo.text = "Не получается открыть файл";
            }
        }
    }

    private Sprite CreateNewSprite(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
        texture.LoadImage(data);
        fileName = Path.GetFileNameWithoutExtension(path);
        texture.name = fileName;
        newSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        Fit2Sprite(OrigImg, newSprite);
        Fit2Sprite(ConvImg, newSprite);
        return newSprite;
    }

    public void Fit2Sprite(UI.Image img, Sprite sprite)
    {
        if (sprite.textureRect.width > sprite.textureRect.height)
            img.transform.localScale = new Vector2(1f, sprite.textureRect.height / sprite.textureRect.width);
        else
            img.transform.localScale = new Vector2(sprite.textureRect.width / sprite.textureRect.height, 1f);
    }
}
