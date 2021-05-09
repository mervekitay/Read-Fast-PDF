using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

public class Okuyucu : MonoBehaviour
{
    
    string[] sayfalarım;
    public Text AnaEkran, anaSafaSayısı, toplamSayfaSayısı;
    string kitap_yolu;
    bool basıyorum=false;
    float okuma_hızı = 0.5f;
    float secilen_hiz=0.8f;
    public Text ekrandaki_kelimeler,ekranSayfaSayısı;
    string current_text;
    int baslangıç = 0;
    int i=1;
    PdfReader okuyucu;
    List<string> kelimelerim = new List<string>();
    public Transform sayfapozisyonu;
    public GameObject panel2, hızlıoku_tusu, onceki_sayfa_tusu, sonraki_sayfa_tusu,slider,hızpaneli, okuTusu, birgeri,hızıayarla, sayfayadontusu;
    

    private string pdfFileType;
    // Start is called before the first frame update
    void Start()
    {
        hızlıoku_tusu.GetComponent<Button>().interactable = false;
        onceki_sayfa_tusu.GetComponent<Button>().interactable = false;
        sonraki_sayfa_tusu.GetComponent<Button>().interactable = false;
        pdfFileType = NativeFilePicker.ConvertExtensionToFileType("pdf");
        string kitap_yolum = PlayerPrefs.GetString("en_son_actıgım");
        if (kitap_yolum != null)
        {
            kitap_yolu = PlayerPrefs.GetString("en_son_actıgım");
            try
            {
                okuyucu = new PdfReader(kitap_yolu);
                sayfalarım = new string[okuyucu.NumberOfPages];
                KitabiAc();
            }
            catch
            {

            }
        }

        if (PlayerPrefs.GetFloat("hız") != 0)
        {
            slider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("hız");
            secilen_hiz = PlayerPrefs.GetFloat("hız") * 0.16f;

        }
        else
        {
            slider.GetComponent<Slider>().value = 5;
        }

        okuma_hızı = secilen_hiz;


    }
    void FixedUpdate()
    {
        if (basıyorum)
        {
            if (baslangıç <= (kelimelerim.Count - 3))
            {
                current_text = "";
                if (kelimelerim[baslangıç].EndsWith(",") || kelimelerim[baslangıç].EndsWith(".") || kelimelerim[baslangıç].EndsWith("?") || kelimelerim[baslangıç].EndsWith("!") || kelimelerim[baslangıç].EndsWith("…"))
                {
                    current_text = string.Format("{0}", kelimelerim[baslangıç]);
                    ekrandaki_kelimeler.text = current_text;
                    okuma_hızı -= Time.deltaTime;
                    if (okuma_hızı < 0)
                    {
                        baslangıç++;

                        okuma_hızı = secilen_hiz;
                    }

                }
                else if (kelimelerim[baslangıç + 1].EndsWith(",") || kelimelerim[baslangıç + 1].EndsWith(".") || kelimelerim[baslangıç + 1].EndsWith("?") || kelimelerim[baslangıç + 1].EndsWith("!") || kelimelerim[baslangıç + 1].EndsWith("…"))
                {
                    current_text = string.Format("{0} {1}", kelimelerim[baslangıç], kelimelerim[baslangıç + 1]);
                    ekrandaki_kelimeler.text = current_text;
                    okuma_hızı -= Time.deltaTime;
                    if (okuma_hızı < 0)
                    {
                        baslangıç++;
                        baslangıç++;

                        okuma_hızı = secilen_hiz;
                    }

                }
                else
                {
                    current_text = string.Format("{0} {1} {2}", kelimelerim[baslangıç], kelimelerim[baslangıç + 1], kelimelerim[baslangıç + 2]);



                    ekrandaki_kelimeler.text = current_text;
                    okuma_hızı -= Time.deltaTime;
                    if (okuma_hızı < 0)
                    {
                        baslangıç++;
                        baslangıç++;
                        baslangıç++;
                        okuma_hızı = secilen_hiz;
                    }

                }
            }
            else if (baslangıç == (kelimelerim.Count - 2))
            {
                if (kelimelerim[baslangıç].EndsWith(",") || kelimelerim[baslangıç].EndsWith(".") || kelimelerim[baslangıç].EndsWith("?") || kelimelerim[baslangıç].EndsWith("!") || kelimelerim[baslangıç].EndsWith("…"))
                {
                    current_text = string.Format("{0}", kelimelerim[baslangıç]);
                    ekrandaki_kelimeler.text = current_text;
                    okuma_hızı -= Time.deltaTime;
                    if (okuma_hızı < 0)
                    {
                        baslangıç++;

                        okuma_hızı = secilen_hiz;
                    }

                }
                else
                {
                    current_text = string.Format("{0} {1}", kelimelerim[baslangıç], kelimelerim[baslangıç + 1]);
                    ekrandaki_kelimeler.text = current_text;
                    okuma_hızı -= Time.deltaTime;
                    if (okuma_hızı < 0)
                    {
                        baslangıç++;
                        baslangıç++;

                        okuma_hızı = secilen_hiz;
                    }

                }
            }
            else if (baslangıç == (kelimelerim.Count - 1))
            {
                current_text = string.Format("{0}", kelimelerim[baslangıç]);
                ekrandaki_kelimeler.text = current_text;
                okuma_hızı -= Time.deltaTime;
                if (okuma_hızı < 0)
                {
                    baslangıç++;

                    okuma_hızı = secilen_hiz;
                }
            }

            else if (baslangıç == (kelimelerim.Count))
            {
                if (i + 1 <= okuyucu.NumberOfPages)
                {
                    sonrakiSayfayıAc();
                }
                else
                {
                    current_text = "kitabın sonuna geldiniz";
                    ekrandaki_kelimeler.text = current_text;
                }



            }

        }
    }
    public void Filepick()
    {

        NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
                Debug.Log("Operation cancelled");
            else
                kitap_yolu = path;
                PlayerPrefs.SetString("en_son_actıgım", path);
                PlayerPrefs.SetInt("kaldıgım_sayfa", 1);
                okuyucu = new PdfReader(kitap_yolu);
                sayfalarım = new string[okuyucu.NumberOfPages];
                KitabiAc();
        }, new string[] { pdfFileType });

        
    }
    //List<string> kelimelerim = new List<string>();
    //string current_text;
    //int current_page = 2;
    //int baslangıç = 0;
    //bool basıyorum = false;
    //float okuma_hızı = 0.5f;
    //public float secilen_hiz = 0.5f;

    // Start is called before the first frame update
    void sayfaninKelimeleriniAl()
    {
        
        kelimelerim.Clear();
        string[] kelimeler = sayfalarım[i - 1].Split(' ', '\n');
        for (int y = 0; y <= kelimeler.Length - 1; y++)
        {
            kelimelerim.Add(kelimeler[y].Trim(' '));

        }
    }

    public void KitabiAc()
    {
        i = PlayerPrefs.GetInt("kaldıgım_sayfa");
        if (i == 1)
        {
            if (okuyucu.NumberOfPages == 1)
            {
                sayfalarım[0] = PdfTextExtractor.GetTextFromPage(okuyucu, 1).TrimEnd(' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
            }
            else if (okuyucu.NumberOfPages == 2)
            {
                for (int x = 0; x < 2; x++)
                {
                    sayfalarım[x] = PdfTextExtractor.GetTextFromPage(okuyucu, x + 1).TrimEnd(' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
                    AnaEkran.text = sayfalarım[i - 1];
                }
            }
            else if (okuyucu.NumberOfPages >= 3)
            {
                for (int x = 0; x < 3; x++)
                {
                    sayfalarım[x] = PdfTextExtractor.GetTextFromPage(okuyucu, x + 1).TrimEnd(' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
                    AnaEkran.text = sayfalarım[i - 1];
                }
            }
        }

        else
        {
            sayfalarım[i-2]= PdfTextExtractor.GetTextFromPage(okuyucu, i-1).TrimEnd(' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
            sayfalarım[i - 1] = PdfTextExtractor.GetTextFromPage(okuyucu, i).TrimEnd(' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');
            if (i + 1 <= okuyucu.NumberOfPages)
            {
                sayfalarım[i] = PdfTextExtractor.GetTextFromPage(okuyucu, i +1).TrimEnd(' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');

            }
            AnaEkran.text = sayfalarım[i - 1];
        }
        sayfapozisyonu.position = new Vector3(-8.5f, -629.981f, 0);
        sayfaninKelimeleriniAl();
        hızlıoku_tusu.GetComponent<Button>().interactable = true;
        onceki_sayfa_tusu.GetComponent<Button>().interactable = true;
        sonraki_sayfa_tusu.GetComponent<Button>().interactable = true;
        anaSafaSayısı.text = i.ToString();
        ekranSayfaSayısı.text = i.ToString();
        toplamSayfaSayısı.text = okuyucu.NumberOfPages.ToString();
    }

    public void sonrakiSayfayıAc()
    {      
        if(i+1 <= okuyucu.NumberOfPages)
        {
            
            if (i == 1)
            {
                i++;
                AnaEkran.text = sayfalarım[i-1];
            }
            else
            {
                i++;
                if (i + 1 <= okuyucu.NumberOfPages)
                {
                    sayfalarım[i] = PdfTextExtractor.GetTextFromPage(okuyucu, i + 1).TrimEnd(' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');

                } 
                sayfalarım[i - 3] = "";
                AnaEkran.text = sayfalarım[i - 1];
                
            }
            sayfaninKelimeleriniAl();
            baslangıç = 0;
            current_text = "";
            ekrandaki_kelimeler.text = current_text;
            PlayerPrefs.SetInt("kaldıgım_sayfa", i);
            sayfapozisyonu.position = new Vector3(-8.5f, -629.981f, 0);
            anaSafaSayısı.text = i.ToString();
            ekranSayfaSayısı.text = i.ToString();
        }
    }

    public void OncekiSayfayıAc()
    {
        if (i > 1 )
        {
            
            if (i == 2)
            {
                i--;
                AnaEkran.text = sayfalarım[i - 1];
            }
            else
            {
                i--;
                 
                sayfalarım[i - 2] = PdfTextExtractor.GetTextFromPage(okuyucu, i - 1).TrimEnd(' ', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0');

                if (i+2<=okuyucu.NumberOfPages)
                {
                    sayfalarım[i + 1] = "";
                }  
                AnaEkran.text = sayfalarım[i - 1];
                
            }
            sayfaninKelimeleriniAl();
            baslangıç = 0;
            current_text = "";
            ekrandaki_kelimeler.text = current_text;
            PlayerPrefs.SetInt("kaldıgım_sayfa", i);
            sayfapozisyonu.position = new Vector3(-8.5f, -629.981f, 0);
            anaSafaSayısı.text = i.ToString();
            ekranSayfaSayısı.text = i.ToString();
        }
    }
    public void oku()
    {
        basıyorum = true;

    }

    public void dur()
    {
        basıyorum = false;
    }
    public void hizliokumayagec()
    {
        panel2.SetActive(true);
    }

    public void sayfayadon()
    {
        panel2.SetActive(false);
    }

    public void Bir_önceki()
    {
        if (baslangıç >= 3)
        {
            current_text = "";

            current_text = string.Format("{0} {1} {2}", kelimelerim[baslangıç - 3], kelimelerim[baslangıç - 2], kelimelerim[baslangıç - 1]);
            ekrandaki_kelimeler.text = current_text;
            baslangıç--;
            baslangıç--;
            baslangıç--;

        }
        else if (baslangıç == 2)
        {
            current_text = "";

            current_text = string.Format("{0} {1}", kelimelerim[baslangıç - 2], kelimelerim[baslangıç - 1]);
            ekrandaki_kelimeler.text = current_text;
            baslangıç--;
            baslangıç--;

        }
        else if (baslangıç == 1)
        {
            current_text = "";

            current_text = string.Format("{0}", kelimelerim[baslangıç - 1]);
            ekrandaki_kelimeler.text = current_text;
            baslangıç--;
        }
        else if (baslangıç == 0)
        {
            if (i-1 > 0)
            {
                current_text = "";
                OncekiSayfayıAc();
                current_text = "sayfanın başına geldiniz";
                ekrandaki_kelimeler.text = current_text;
                baslangıç--;
            }
            else
            {
                current_text = "kitabın başına geldiniz";
                ekrandaki_kelimeler.text = current_text;
            }


        }
        else if (baslangıç < 0)
        {

            baslangıç = kelimelerim.Count - 1;
            current_text = "";

            current_text = string.Format("{0}", kelimelerim[baslangıç]);
            ekrandaki_kelimeler.text = current_text;


        }


    }

    public void hizPaneliniAc()
    {
        hızpaneli.SetActive(true);
    }
    public void hizPaneliniKapat()
    {
        PlayerPrefs.SetFloat("hız", slider.GetComponent<Slider>().value);
        secilen_hiz = PlayerPrefs.GetFloat("hız") * 0.16f;
        hızpaneli.SetActive(false);
    }

    public void cıkıs()
    {
        Application.Quit();
    }

    public void otomatik()
    {
        if (basıyorum)
        {
            dur();
            okuTusu.GetComponent<Button>().interactable = true;
            birgeri.GetComponent<Button>().interactable = true;
            hızıayarla.GetComponent<Button>().interactable = true;
            sayfayadontusu.GetComponent<Button>().interactable = true;
        }
        else
        {
            oku();
            okuTusu.GetComponent<Button>().interactable = false;
            birgeri.GetComponent<Button>().interactable = false;
            hızıayarla.GetComponent<Button>().interactable = false;
            sayfayadontusu.GetComponent<Button>().interactable = false;
        }
    }
}
