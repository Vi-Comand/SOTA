using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
                FileInfo fileInfo=new FileInfo(Path);
                excelPack.SaveAs(fileInfo);
            }
            Fill();
        }





        private void Fill()
        {
            FileInfo fileInfo = new FileInfo(Path);

            ExcelPackage excelPack = new ExcelPackage(fileInfo);
               
                var workSheet = excelPack.Workbook.Worksheets[0];
            int KolZad = Protokol.Tables[1].Balls.Count;
            int kolUchen = Protokol.Tables.Count;
                AddNumberZadans(workSheet, KolZad);
            int rowKol = 12;
            workSheet.Cells[3, 2].Value = Protokol.NameR;
            workSheet.Cells[4, 2].Value = Protokol.Predmet;
            workSheet.Cells[5, 2].Value = Protokol.DateProved;
            workSheet.Cells[6, 2].Value = Protokol.KolVar;
            workSheet.Cells[7, 2].Value = Protokol.KolUch;
            foreach (var row in Protokol.Tables )
                {
                workSheet.Cells[rowKol, 1].Value = row.MO;
                workSheet.Cells[rowKol, 2].Value = row.OO;
                workSheet.Cells[rowKol, 3].Value = row.Klass;
                workSheet.Cells[rowKol, 4].Value = row.FIO;
                workSheet.Cells[rowKol, 5].Value = row.Var;
                int indexColumnsBalls = 6;
              foreach(var ball in row.Balls)
                {if (ball != -1)
                        workSheet.Cells[rowKol, indexColumnsBalls].Value = ball; 
                else
                        workSheet.Cells[rowKol, indexColumnsBalls].Value = "-";
                    indexColumnsBalls++;
                }
                workSheet.Cells[rowKol, indexColumnsBalls].Value = Math.Round(row.ProcVipUch, 2);
                workSheet.Cells[rowKol, indexColumnsBalls+1].Value = row.SumBall;
                rowKol++;
            }
           int  indexColumnsProcVipolnen = 6;
            foreach (var row in Protokol.ProcVipZad)
            {
                workSheet.Cells[rowKol, indexColumnsProcVipolnen].Value = Math.Round(row, 2);
                indexColumnsProcVipolnen++;
            }



            Style(workSheet, kolUchen, KolZad);
            Chart(workSheet, kolUchen, KolZad);

            excelPack.Save();


            


        }

        private void Chart(ExcelWorksheet workSheet, int kolRow,int kolZad)
        {
            double top=0;
            for (int i = 1; i < kolRow+13 ; i++)
                top += workSheet.Row(i).Height;
            top =  top / 0.75;
            var ws = workSheet;
            
            

           
            //Create the chart
            var chart = (ExcelBarChart)ws.Drawings.AddChart("barChart", eChartType.ColumnClustered);
            
            chart.Legend.Remove();
            chart.SetSize(40*kolZad, 400);
            chart.SetPosition((int)top, 20);
            chart.Title.Text = "Процент выполнения заданий";
            //chart.Direction = eDirection.Column; // error: Property or indexer 'OfficeOpenXml.Drawing.Chart.ExcelBarChart.Direction' cannot be assigned to -- it is read only

            chart.Series.Add(ExcelRange.GetAddress(12+kolRow, 6, 12 + kolRow, kolZad + 5), ExcelRange.GetAddress(11, 6, 11,kolZad+5 ));

        }
        private void Style(ExcelWorksheet workSheet, int kolRow, int KolZad)
        {
            ExcelBorderStyle Style= ExcelBorderStyle.Thin;
            workSheet.Cells[12, 1, 12 + kolRow, 7 + KolZad].Style.Border.Left.Style = Style;
            workSheet.Cells[12, 1, 12 + kolRow, 7 + KolZad].Style.Border.Right.Style = Style;
            workSheet.Cells[12, 1, 12 + kolRow, 7 + KolZad].Style.Border.Top.Style = Style;
            workSheet.Cells[12, 1, 12 + kolRow, 7 + KolZad].Style.Border.Bottom.Style = Style;
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
