using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBot2;

namespace WebBot2
{
    public static class Program
    {
        static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static string projectName = "WebBot2";
        public static void Main(string[] args)
        {
            List<string> catList = Jobs.GetCategoryList();
            List<WebProduct> mainWpList = Jobs.GetProductDetail(catList);
            ConvertToDataTable.ConvertToDatatable(mainWpList, projectName, folderPath);
        }
    }
}