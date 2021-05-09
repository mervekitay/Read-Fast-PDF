using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;


//using iText.Kernel.Pdf;
//using iText.Kernel.Pdf.Canvas.Parser;

public class file_pick : MonoBehaviour
{
    
    public Text AnaEkran;
    
    private string pdfFileType;
    // Start is called before the first frame update
    void Start()
    {
        
        pdfFileType = NativeFilePicker.ConvertExtensionToFileType("pdf");


    }
    public void Filepick()
    {
        
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
                Debug.Log("Operation cancelled");
            else
                AnaEkran.text= path;
        }, new string[] { pdfFileType });

        Debug.Log("Permission result: " + permission);
    }
    //public void oku()
    //{
    //    PdfReader okuyucu = new PdfReader(AnaEkran.text);
    //    PdfDocument sourcePdf = new PdfDocument(okuyucu);
    //    int sayfaSayisi = sourcePdf.GetNumberOfPages();
    //    PdfPage a = sourcePdf.GetPage(1);
    //    string b = PdfTextExtractor.GetTextFromPage(a);




    //}

}
