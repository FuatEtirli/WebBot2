using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebBot2
{
    public class Jobs
    {
        static string mainURL = "Site URL";
        public static List<string> GetCategoryList()
        {
            HtmlDocument doc = new HtmlDocument();
            List<string> pLinkList = new List<string>();
            List<string> catList = new List<string>();
            List<string> pageList = new List<string>();

            catList.Add("Kategori URL");
      
            foreach (var item in catList)
            {
                bool keepContinue = true;

                while (keepContinue)
                {
                    try
                    {
                        using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                        {
                            client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            var sonuc = client.DownloadString(item);
                            doc.LoadHtml(sonuc);
                        }
                        var pages = doc.DocumentNode.SelectNodes("//nav[@class='woocommerce-pagination']//ul[@class='page-numbers']//a[@class='page-numbers']").Last().InnerText.Trim();
                        if (pages != null)
                        {
                            int x = Convert.ToInt32(pages);

                            for (int i = 1; i <= x; i++)
                            {
                                using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                                {
                                    client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36";
                                    ServicePointManager.Expect100Continue = true;
                                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                                    var sonuc = client.DownloadString(item + "page/" + i);
                                    doc.LoadHtml(sonuc);
                                }

                                HtmlNodeCollection pURLList = doc.DocumentNode.SelectNodes("//ul[@class='products columns-3']//li");

                                foreach (var pLink in pURLList)
                                {


                                    string link = pLink.ChildNodes[1].Attributes[0].Value;
                                    pLinkList.Add(link);
                                    pLinkList = pLinkList.Distinct().ToList();


                                }
                                Console.Clear();
                                Console.WriteLine("{0} Kategorisi Taranıyor => {1} Adet Ürün Yakalandı", item, pLinkList.Count);
                                if (i >= x)
                                {
                                    keepContinue = false;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        keepContinue = false;

                    }

                }
            }
            return pLinkList.Distinct().ToList();

        }
        public static List<WebProduct> GetProductDetail(List<string> pLinkList)
        {
            List<WebProduct> wpList = new List<WebProduct>();
            HtmlDocument doc = new HtmlDocument();

            foreach (string pLink in pLinkList)
            {

                try
                {
                    using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                    {
                        client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        var response = client.DownloadString(pLink);
                        doc.LoadHtml(response);
                    }

                    WebProduct wp = new WebProduct()
                    {                       
                        RequestTime = DateTime.Now.Date,
                        NewUrl = pLink,

                    };
                    try
                    {
                        wp.Sku = doc.DocumentNode.SelectSingleNode("//div[@class='summary entry-summary']//h1[@class='product_title entry-title']").InnerText.Trim();
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        wp.Barcode = doc.DocumentNode.SelectNodes("//tr[@class='alt']//td[@class='attribute_value']//p")[1].InnerText.Trim();
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        var price = doc.DocumentNode.SelectNodes("//div[@class='summary entry-summary']//p[@class='price']//span[@class='woocommerce-Price-amount amount']")[1].InnerText.Trim().Replace("&nbsp;&#8378;", "");
                        wp.Price = Convert.ToDouble(price);
                    }
                    catch (Exception)
                    {
                        var price = doc.DocumentNode.SelectSingleNode("//div[@class='summary entry-summary']//p[@class='price']//span[@class='woocommerce-Price-amount amount']").InnerText.Trim().Replace("&nbsp;&#8378;", "");
                        wp.Price = Convert.ToDouble(price);

                    }

                    try
                    {
                        wp.Category = doc.DocumentNode.SelectNodes("//div[@class='product_meta']//span[@class='posted_in']//a")[1].InnerText.Trim();
                    }
                    catch (Exception)
                    {

                    }

                    if (wp.OldPrice > wp.Price)
                    {
                        wp.CargoDetail01 = "İndirim";
                    }
                    if (wp.Price != null)
                    {
                        wpList.Add(wp);
                        Console.Clear();
                        Console.WriteLine("{0} Adet Üründen => {1} adet Ürün Yakalandı => {2} Adet Ürün Kaldı", pLinkList.Count, wpList.Count, (pLinkList.Count - wpList.Count));
                    }


                }
                catch (Exception ex)
                {
                    string excep = ex.Message;
                    if (excep != "Uzak sunucu hata döndürdü: (404) Bulunamadı.")
                    {

                    }
                }
            }


            return wpList.Distinct().ToList();
        }

    }
}
