using UnityEngine;
using UI = UnityEngine.UI;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Text;

public class ConverterView : MonoBehaviour
{
    [SerializeField] private UI.Image[] images;    //  Images on the scene

    public void RenewImage(Image img, int i)
    {
        ChangeSprite(img, images[i]);
        Fit2Sprite(images[i]);
    }

    /// <summary>
    /// Open file with default program
    /// </summary>
    /// <param name="path"> Path to file</param>
    public void ViewImg(string path)
    {
        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "");
        psi.CreateNoWindow = true;
        psi.UseShellExecute = false;
        psi.StandardOutputEncoding = Encoding.GetEncoding("utf-8");
        psi.RedirectStandardInput = true;
        psi.RedirectStandardOutput = true;

        Process process = Process.Start(psi);
        process.StandardInput.WriteLine("cd " + Path.GetDirectoryName(path));
        process.StandardInput.WriteLine(Path.GetFileName(path));
        process.StandardInput.WriteLine("exit");
    }

    /// <summary>
    /// Take System.Drawing.Image ("img") and UnityEngine.UI.Image ("imgUI"), create Sprite from "img" and set it for "imgUI"
    /// </summary>
    /// <param name="img"> System.Drawing.Image </param>
    /// <param name="imgUI"> UnityEngine.UI.Image </param>
    private void ChangeSprite(Image img, UI.Image imgUI)
    {
        MemoryStream ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false); ;
        texture.LoadImage(ms.ToArray());
        Sprite newSprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        imgUI.sprite = newSprite;
    }

    /// <summary>
    ///  Resize Image to sprite's ratio
    /// </summary>
    private void Fit2Sprite(UI.Image img)
    {
        if (img.sprite.textureRect.width > img.sprite.textureRect.height)
            img.transform.localScale = new Vector2(1f, img.sprite.textureRect.height / img.sprite.textureRect.width);
        else
            img.transform.localScale = new Vector2(img.sprite.textureRect.width / img.sprite.textureRect.height, 1f);
    }
}
