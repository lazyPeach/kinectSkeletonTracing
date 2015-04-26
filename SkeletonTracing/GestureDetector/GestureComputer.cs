using DynamicTimeWarping;
using Helper;
using SkeletonModel.Managers;
using SkeletonModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureDetector {
  public class GestureComputer {
    public bool IsBothHandsRaise(Body[] record) {
      Computation computation = new Computation();
      List<string> files = new List<string>();

      foreach (string s in Directory.EnumerateFiles(@".\..\..\..\..\testData\")) {
        files.Add(s);
      }

      //iterate only through the first 5 files which we know are both hans rise serialization
      for (int sampleIndex = 0; sampleIndex < 5; sampleIndex++) {
        string sampleFileName = files[sampleIndex];

        FileStream sampleFileStream = new FileStream(sampleFileName, FileMode.Open, FileAccess.Read);

        BodyManager bodyManager = new BodyManager();
        bodyManager.LoadCollection(sampleFileStream);
        // now we have the template, compare crt body to each template

        computation.ComputeDTW(bodyManager.BodyData, record);

        float sum = 0;
        foreach (BoneName boneName in Enum.GetValues(typeof(BoneName))) {
          for (int i = 0; i < 4; i++) {
            sum += computation.FilteredResult.Data[Mapper.BoneIndexMap[boneName]].BestCost[i].Cost;
          }
        }

        //Console.WriteLine("for sample " + sampleIndex.ToString() + " cost is: " + sum.ToString());

        if (sum < 150)
          return true;

      }

      return false;
    }



  }
}
