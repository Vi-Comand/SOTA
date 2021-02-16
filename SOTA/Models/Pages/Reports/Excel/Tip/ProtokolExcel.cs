using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;

namespace SOTA.Models.Pages.Reports.Excel.Tip
{
    public class ProtokolExcel
    {
        ProtokolProvedeniaMonitoringovoyRaboti Protokol;
        string Path;

        public ProtokolExcel(ProtokolProvedeniaMonitoringovoyRaboti Protokol,string Path)
        {
            this.Protokol =Protokol;
            this.Path = Path;
        }
        public void Create()
        {
            using (var stream = File.OpenRead(Directory.GetCurrentDirectory() + "/wwwroot/Reports/Templates/Протокол проведения мониторинговой работы.xlsx"))
            {
                ExcelPackage excelPack = new ExcelPackage();
                excelPack.Load(stream);
               
                Path = Path.Replace("\"", "").Replace("\'", "").Replace("?", "").Replace("*", "").Replace("|", "")
                    .Replace("<", "").Replace(">", "");
                FileInfo fileInfo=new FileInfo(Path);
                excelPack.SaveAs(fileInfo);
            }
            Fill();
        }





        private void Fill()
        {
            FileInfo fileInfo = new FileInfo(Path);

            ExcelPackage excelPack = new ExcelPackage(fileInfo);
               
            
            FillProtokolList(excelPack.Workbook.Worksheets[0]);
            FillAnalyticsList(excelPack.Workbook.Worksheets[1]);


            

            excelPack.Save();


            


        }
        private void FillProtokolList(ExcelWorksheet workSheet)
        {
            int KolZad = Protokol.Tables[1].Balls.Count;
            int kolUchen = Protokol.Tables.Count;
            AddNumberZadans(workSheet, KolZad);
            int rowKol = 12;
            workSheet.Cells[3, 2].Value = Protokol.NameR;
            workSheet.Cells[4, 2].Value = Protokol.Predmet;
            workSheet.Cells[5, 2].Value = Protokol.DateProved;
            workSheet.Cells[6, 2].Value = Protokol.KolVar;
            workSheet.Cells[7, 2].Value = Protokol.KolUch;
            foreach (var row in Protokol.Tables)
            {
                workSheet.Cells[rowKol, 1].Value = row.MO;
                workSheet.Cells[rowKol, 2].Value = row.OO;
                workSheet.Cells[rowKol, 3].Value = row.Klass;
                workSheet.Cells[rowKol, 4].Value = row.FIO;
                workSheet.Cells[rowKol, 5].Value = row.Var;
                int indexColumnsBalls = 6;
                foreach (var ball in row.Balls)
                {
                    if (ball != -1)
                        workSheet.Cells[rowKol, indexColumnsBalls].Value = ball;
                    else
                        workSheet.Cells[rowKol, indexColumnsBalls].Value = "-";
                    indexColumnsBalls++;
                }
                workSheet.Cells[rowKol, indexColumnsBalls].Value = Math.Round(row.ProcVipUch, 2);
                workSheet.Cells[rowKol, indexColumnsBalls + 1].Value = row.SumBall;
                rowKol++;
            }

            int indexColumnsProcVipolnen = 6;
            workSheet.Cells[rowKol, 4].Value = "Средний процент";
            foreach (var row in Protokol.ProcVipZad)
            {
                workSheet.Cells[rowKol, indexColumnsProcVipolnen].Value = Math.Round(row, 2);
                indexColumnsProcVipolnen++;
            }
            workSheet.Cells[rowKol, indexColumnsProcVipolnen].Value= Protokol.ProcVipZad.Sum()/KolZad;
            workSheet.Cells[rowKol, indexColumnsProcVipolnen].Style.Numberformat.Format = "0.0%";
            workSheet.Cells[rowKol, indexColumnsProcVipolnen].Style.Font.Bold=true;
            rowKol++;
            int indexColumnsSumVipolnen = 6;
            workSheet.Cells[rowKol, 4].Value = "Средний балл";
            foreach (var row in Protokol.SumVipZad)
            {
                workSheet.Cells[rowKol, indexColumnsSumVipolnen].Value = Math.Round(row, 2);
                indexColumnsSumVipolnen++;
            }
            workSheet.Cells[rowKol, indexColumnsSumVipolnen].Value = Protokol.SumVipZad.Sum() / KolZad;
            workSheet.Cells[rowKol, indexColumnsSumVipolnen].Style.Numberformat.Format = "0.00";

            Style(workSheet, kolUchen, KolZad);
            Chart(workSheet, kolUchen, KolZad);

        }

        private void FillAnalyticsList(ExcelWorksheet workSheet)
        {
            int rowIndex = 2;
            foreach (var row in Protokol.AnalyticsTable)
            {
                
                    workSheet.Cells[rowIndex, 1].Value = row.Number;
                workSheet.Cells[rowIndex, 2].Value = row.CheckElementContent;
                workSheet.Cells[rowIndex, 3].Value = row.KodPes;
                workSheet.Cells[rowIndex, 4].Value = row.LevelOfComplexity;
                workSheet.Cells[rowIndex, 5].Value = row.MaxScore;
                workSheet.Cells[rowIndex, 6].Value = row.AverageScore;
                workSheet.Cells[rowIndex, 7].Value = row.LevelSuccess;
                workSheet.Cells[rowIndex, 8].Value = row.Conclusion;
                if (row.LevelSuccess <= 0.50) { workSheet.Cells[rowIndex, 9].Value = row.Recomend;}


                rowIndex++;


            }
            rowIndex--;
            ExcelBorderStyle Style = ExcelBorderStyle.Thin;
            workSheet.Cells[1, 1, rowIndex, 9].Style.Border.Left.Style = Style;
            workSheet.Cells[1, 1, rowIndex, 9].Style.Border.Right.Style = Style;
            workSheet.Cells[1, 1, rowIndex, 9].Style.Border.Top.Style = Style;
            workSheet.Cells[1, 1, rowIndex, 9].Style.Border.Bottom.Style = Style;
            workSheet.Cells[2 , 6, rowIndex, 6].Style.Numberformat.Format = "0.00";
        }

            private void Chart(ExcelWorksheet workSheet, int kolRow,int kolZad)
        {
            double top=0;
            for (int i = 1; i < kolRow+20 ; i++)
                top += workSheet.Row(i).Height;
            top =  top / 0.75;
            var ws = workSheet;
            
            

           
            //Create the chart
            var chart = (ExcelBarChart)ws.Drawings.AddChart("barChart", eChartType.ColumnClustered);
            
            chart.Legend.Remove();
            chart.SetSize(100*kolZad, 400);
            chart.SetPosition((int)top, 20);
            chart.Title.Text = "Процент выполнения заданий";
            //chart.Direction = eDirection.Column; // error: Property or indexer 'OfficeOpenXml.Drawing.Chart.ExcelBarChart.Direction' cannot be assigned to -- it is read only

            chart.Series.Add(ExcelRange.GetAddress(12+kolRow, 6, 12 + kolRow, kolZad + 5), ExcelRange.GetAddress(11, 6, 11,kolZad+5 ));



            var chart2 = (ExcelBarChart)ws.Drawings.AddChart("barChart2", eChartType.ColumnClustered);

            chart2.Legend.Remove();
            chart2.SetSize(100 * kolZad, 400);
            chart2.SetPosition((int)top, 100+100*kolZad);
            chart2.Title.Text = "Средний балл по заданиям";
            //chart.Direction = eDirection.Column; // error: Property or indexer 'OfficeOpenXml.Drawing.Chart.ExcelBarChart.Direction' cannot be assigned to -- it is read only

            chart2.Series.Add(ExcelRange.GetAddress(13 + kolRow, 6, 13 + kolRow, kolZad + 5), ExcelRange.GetAddress(11, 6, 11, kolZad + 5));

        }
        private void Style(ExcelWorksheet workSheet, int kolRow, int KolZad)
        {
            ExcelBorderStyle Style= ExcelBorderStyle.Thin;
            workSheet.Cells[12, 1, 13 + kolRow, 7 + KolZad].Style.Border.Left.Style = Style;
            workSheet.Cells[12, 1, 13 + kolRow, 7 + KolZad].Style.Border.Right.Style = Style;
            workSheet.Cells[12, 1, 13 + kolRow, 7 + KolZad].Style.Border.Top.Style = Style;
            workSheet.Cells[12, 1, 13 + kolRow, 7 + KolZad].Style.Border.Bottom.Style = Style;
            workSheet.Cells[12 + kolRow, 6, 12 + kolRow, 6 + KolZad].Style.Numberformat.Format = "0.0%";
            workSheet.Cells[12 + kolRow, 4, 13 + kolRow, 6 + KolZad].Style.Font.Bold = true;
        }

            private void AddNumberZadans(ExcelWorksheet workSheet, int KolZad)
        {
            
            workSheet.Cells[10,7 , 13, 8].Copy(workSheet.Cells[10, KolZad+6,   13,KolZad+7]);
            for (int i = 2; i <= KolZad; i++)
            {
                workSheet.Cells[10, 6, 13, 6].Copy(workSheet.Cells[10, 5+i, 13, 5 + i]);
                workSheet.Cells[11, 5 + i].Value = i;

            }
        }

    }
}
