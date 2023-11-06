
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TnT.DataLayer
{
    public class ExcellHelper
    {

        /// <param name="ds"></param>
        public void ExportDataSetToExcel(DataSet ds, string FPath)
        {
            //Creae an Excel application instance
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            excelApp.Visible = false;
            //Create an Excel workbook instance and open it from the predefined location
            Microsoft.Office.Interop.Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();
            //excelWorkBook = excelApp.Workbooks.
            foreach (DataTable table in ds.Tables)
            {
                //Add a new worksheet to workbook with the Datatable name
                Microsoft.Office.Interop.Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = table.TableName;

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;

                }

                int colCnt = table.Columns.Count;
                int rowCnt = table.Rows.Count + 1;

               // excelWorkSheet.Cells[1, colCnt].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
                //excelWorkSheet.Cells[1, colCnt].EntireRow.Font.Bold = true;

                //excelWorkSheet.Range[excelWorkSheet.Cells[1, 1], excelWorkSheet.Cells[rowCnt, colCnt]].EntireColumn.AutoFit();
                excelWorkSheet.Range[excelWorkSheet.Cells[1, 1], excelWorkSheet.Cells[1, colCnt]].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSkyBlue);
                excelWorkSheet.Range[excelWorkSheet.Cells[1, 1], excelWorkSheet.Cells[1, colCnt]].Font.Bold = true;
                excelWorkSheet.Range[excelWorkSheet.Cells[1, 1], excelWorkSheet.Cells[1, colCnt]].Font.Size = 13;


                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                    }
                }
            }

            excelWorkBook.SaveAs(FPath);
            excelWorkBook.Close();
            excelApp.Quit();

        }


    }
}