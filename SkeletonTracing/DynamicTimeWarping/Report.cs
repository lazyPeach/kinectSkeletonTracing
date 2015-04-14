using Helper;
using Microsoft.Office.Interop.Excel;
using SkeletonModel.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DynamicTimeWarping {
  public class Report {
    public Report() {
      computation = new Computation();
      CreateExcelFile();
    }

    public void CreateReport() {
      List<string> files = new List<string>();
      foreach (string s in Directory.EnumerateFiles(@".\..\..\..\..\testData\")) {
        files.Add(s);
      }

      int column = 0;
      int row = 1;

      string sample1FileName = files[0];
      string sample2FileName = files[1];

      Console.WriteLine(sample1FileName + " " + sample2FileName);

      FileStream sample1FileStream = new FileStream(sample1FileName, FileMode.Open, FileAccess.Read);
      FileStream sample2FileStream = new FileStream(sample2FileName, FileMode.Open, FileAccess.Read);

      BodyManager bodyManager = new BodyManager();
      bodyManager.LoadCollection(sample1FileStream);
      bodyManager.LoadSample(sample2FileStream);

      computation.ComputeDTW(bodyManager.BodyData, bodyManager.SampleData);

      float sum = 0;
      float max = 0;
      float nr = 0;
      foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
        xlWorkSheet.Cells[row, column * 3 + 1] = boneName.ToString();
        row++;
        
        for (int i = 0; i < 4; i++) {
          xlWorkSheet.Cells[row, column * 3 + 1] = i.ToString();
          xlWorkSheet.Cells[row, column * 3 + 2] = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost.ToString();
          sum += computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost;
          nr++;
          max = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost > max ? computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost : max;
          row++;
        }
      }

      xlWorkSheet.Cells[row, column * 3 + 1] = "max value";
      xlWorkSheet.Cells[row, column * 3 + 2] = max;
      row++;

      xlWorkSheet.Cells[row, column * 3 + 1] = "body cost as sum";
      xlWorkSheet.Cells[row, column * 3 + 2] = sum;
      row++;

      xlWorkSheet.Cells[row, column * 3 + 1] = "body cost as avg";
      xlWorkSheet.Cells[row, column * 3 + 2] = sum / nr;
      
      SaveReport();
      CloseExcelFile();


      
    }

    private void CreateExcelFile() {
      object misValue = Missing.Value;

      xlApp = new Microsoft.Office.Interop.Excel.Application();
      xlWorkBook = xlApp.Workbooks.Add(misValue);
      xlWorkSheet = xlWorkBook.ActiveSheet;

      xlWorkSheet.Name = "report";
      xlWorkBook.SaveAs("L:\\dummy\\KinectSkeletonTracing\\report\\report.xls"
                       , XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue
                       , XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue
                       , misValue);
    }

    private void SaveReport() {
      xlWorkBook.Save();
    }

    private void CloseExcelFile() {
      SaveReport();

      object misValue = Missing.Value;

      xlWorkBook.Close(true, misValue, misValue);
      xlApp.Quit();

      releaseObject(xlWorkSheet);
      releaseObject(xlWorkBook);
      releaseObject(xlApp);
    }

    private void releaseObject(object obj) {
      try {
        System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
        obj = null;
      } catch (Exception ex) {
        obj = null;
      } finally {
        GC.Collect();
      }
    }

    private Computation computation;
    private Microsoft.Office.Interop.Excel.Application xlApp;
    private Workbook xlWorkBook;
    private Worksheet xlWorkSheet;
  }
}
