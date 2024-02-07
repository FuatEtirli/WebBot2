using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBot2
{
    public class ConvertToDataTable
    {
        static string path = "";
        public static string ConvertToDatatable(List<WebProduct> list, string projectName, string folderPath)
        {

            try
            {

                DataTable dt = new DataTable();

                DateTime date = DateTime.Now;

                dt.Columns.Add("StoreName");
                dt.Columns.Add("Category");
                dt.Columns.Add("SubCategory");
                dt.Columns.Add("Brand");
                dt.Columns.Add("SKU");
                dt.Columns.Add("SKUCode");
                dt.Columns.Add("Barcode");
                dt.Columns.Add("UnitCode");
                dt.Columns.Add("Supplier");
                dt.Columns.Add("SupplierMark");
                dt.Columns.Add("Supplier2");
                dt.Columns.Add("OldPrice");
                dt.Columns.Add("Price");
                dt.Columns.Add("Stock");
                dt.Columns.Add("IsStock");
                dt.Columns.Add("CargoDetail");
                dt.Columns.Add("CargoPrice");
                dt.Columns.Add("URL");
                dt.Columns.Add("DateTime");
                dt.Columns.Add("IsStar");

                string botName = "";

                foreach (var item in list.Distinct().ToList())
                {
                    var row = dt.NewRow();

                    botName = item.SupplierText;

                    row["StoreName"] = item.StoreName;
                    row["Category"] = item.Category;
                    row["SubCategory"] = item.SubCategory;
                    row["Brand"] = item.Brand;
                    row["SKU"] = item.Sku;
                    row["SKUCode"] = "";
                    row["Barcode"] = item.Barcode;
                    row["UnitCode"] = item.Unit;
                    row["Supplier"] = item.Supplier;
                    row["SupplierMark"] = "";
                    row["Supplier2"] = item.SubTitle;
                    row["OldPrice"] = item.OldPrice;
                    row["Price"] = item.Price;
                    row["IsStock"] = item.InStock;
                    row["CargoDetail"] = item.CargoDetail01;
                    row["URL"] = item.NewUrl;
                    row["DateTime"] = Convert.ToDateTime(date).ToString();
                    row["Stock"] = item.StockCount;
                    row["IsStar"] = 0;

                    if (item.SupplierMark == null)
                    {
                        row["SupplierMark"] = "";
                    }
                    dt.Rows.Add(row);
                }
                try
                {
                    Random r = new Random();

                    path = folderPath + projectName + " " + System.DateTime.Now.ToString("d MMMM yyyy dddd HH.mm.ss") + "-" + r.Next(999, 983948) + ".xlsx";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }
                bool isSuccess = false;

                
                try
                {
                    GC.Collect();
                    string filePath = Pum_Excel_Management.ExportToExcel(path, dt, true, false);
                    isSuccess = true;
                }
                catch (Exception)
                {
                    isSuccess = false;
                }
  
                System.Diagnostics.Process.Start(folderPath);
                Console.Clear();
                clearMemory();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return path;
        }

        private static void clearMemory()
        {
            //dcount.Clear();
            //list.Clear();
            GC.Collect();
            GC.RemoveMemoryPressure(200000);
        }
    }
}
