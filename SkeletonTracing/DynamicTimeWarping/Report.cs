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

    private List<string> files;

    public void CreateReport() {
      files = new List<string>();
      
      foreach (string s in Directory.EnumerateFiles(@".\..\..\..\..\testData\")) {
        files.Add(s);
      }

      for (int i = 0; i < files.Count; i++) {
        CreateReport(i);
      }

      SaveReport();
      CloseExcelFile();
    }

    private void CreateReport(int templateIndex) {
      string templateFileName = files[templateIndex];
      Worksheet ws = xlWorkBook.Worksheets.Add();
      ws.Name = templateIndex.ToString();//last in files[templateIndex];
     
      for (int sampleIndex = 0; sampleIndex < files.Count; sampleIndex++) {
        string sampleFileName = files[sampleIndex];
        Console.WriteLine("template " + templateIndex.ToString() + " - computing for" + sampleFileName);
        int column = sampleIndex;
        int row = 1;

        FileStream templateFileStream = new FileStream(templateFileName, FileMode.Open, FileAccess.Read);
        FileStream sampleFileStream = new FileStream(sampleFileName, FileMode.Open, FileAccess.Read);

        BodyManager bodyManager = new BodyManager();
        bodyManager.LoadCollection(templateFileStream);
        bodyManager.LoadSample(sampleFileStream);

        computation.ComputeDTW(bodyManager.BodyData, bodyManager.SampleData);

        float sum = 0;
        float max = 0;
        float nr = 0;
        // populate the worksheet with bones and resulting value of quaternions
        foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
          ws.Cells[row, column * 3 + 1] = boneName.ToString();
          row++;

          for (int i = 0; i < 4; i++) {
            ws.Cells[row, column * 3 + 1] = i.ToString();
            ws.Cells[row, column * 3 + 2] = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost.ToString();
            sum += computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost;
            nr++;
            max = computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost > max ? computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost : max;
            row++;
          }
        }

        ws.Cells[row, column * 3 + 1] = "max value";
        ws.Cells[row, column * 3 + 2] = max;
        row++;

        ws.Cells[row, column * 3 + 1] = "body cost as sum";
        ws.Cells[row, column * 3 + 2] = sum;
        row++;

        ws.Cells[row, column * 3 + 1] = "body cost as avg";
        ws.Cells[row, column * 3 + 2] = sum / nr;
      }
        releaseObject(ws);
    }



    private void CreateExcelFile() {
      object misValue = Missing.Value;

      xlApp = new Microsoft.Office.Interop.Excel.Application();
      xlWorkBook = xlApp.Workbooks.Add(misValue);
      xlWorkSheet = xlWorkBook.ActiveSheet;

      //xlWorkSheet.Name = "report";
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
