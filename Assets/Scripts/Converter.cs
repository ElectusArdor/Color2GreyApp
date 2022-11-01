using System;
using UnityEngine;
using System.Drawing;
using System.Drawing.Imaging;
using UI = UnityEngine.UI;
using System.Runtime.InteropServices;
using System.IO;

public class Converter : MonoBehaviour
{
    [SerializeField] private UI.Text TextInfo;
    [SerializeField] private UI.Button ConvertBtn, SaveBtn, OpenImgBtn;

    private ConverterView CV;
    private Bitmap imgBitmap;
    private Image convImg;
    private string fileName;

    private void Start()
    {
        CV = Camera.main.GetComponent<ConverterView>();   //  View part of project
    }

    public void LoadImg()
    {
        TextInfo.text = "";
        OpenImgBtn.interactable = false;
        FileSelector.GetFile(GotFile);
    }

    public void ConvertImg()
    {
        ConvertBtn.interactable = false;
        convImg = CreateGreyImage(imgBitmap);
        CV.RenewImage(convImg, 1);
        SaveBtn.interactable = true;
    }

    public void Save()
    {
        SaveBtn.interactable = false;
        SaveGrayImg(convImg);
        OpenImgBtn.interactable = true;
    }

    public void OpenFile()
    {
        CV.ViewImg(fileName);
    }

    private Image CreateGreyImage(Bitmap bmp)
    {
        if (bmp != null)
        {
            PixelFormat pxf = PixelFormat.Format32bppArgb;
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            BitmapData imgBitmapData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);  // Locking an image dataset in memory

            IntPtr ptr = imgBitmapData.Scan0;

            int numBytes = imgBitmapData.Stride * imgBitmapData.Height;
            byte[] rgbValues = new byte[numBytes];  //  Create data array

            Marshal.Copy(ptr, rgbValues, 0, numBytes);  //    Copying dataset to array

            // Loop through pixels 4 bytes each and change values
            for (int counter = 0; counter < rgbValues.Length; counter += 4)
            {
                int value = rgbValues[counter] + rgbValues[counter + 1] + rgbValues[counter + 2];

                byte color_b = Convert.ToByte(value / 3);

                rgbValues[counter] = color_b;
                rgbValues[counter + 1] = color_b;
                rgbValues[counter + 2] = color_b;
            }

            Marshal.Copy(rgbValues, 0, ptr, numBytes);  // Copying the dataset back into the image

            bmp.UnlockBits(imgBitmapData);    //  Unlocking an image dataset in memory
        }
        return bmp;
    }

    private void SaveGrayImg(Image img)
    {
        try
        {
            img.Save(fileName, ImageFormat.Png);
            TextInfo.text = "Файл сохранён " + fileName;
        }
        catch
        {
            TextInfo.text = "Не удалось сохранить файл";
        }
    }

    private void GotFile(FileSelector.Status status, string path)
    {
        if (status == FileSelector.Status.Successful)
        {
            try
            {
                imgBitmap = new Bitmap(path);
                ConvertBtn.interactable = true;
                CV.RenewImage(imgBitmap, 0);
                fileName = Path.GetDirectoryName(path) + "\\Gray_" + Path.GetFileName(path);
            }
            catch
            {
                TextInfo.text = "Не удалось открыть файл";
            }
        }
    }
}
