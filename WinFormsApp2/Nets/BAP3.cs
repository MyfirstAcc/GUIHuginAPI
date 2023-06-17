using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIHugin.Nets
{
    public class BAP3
    {

        //Auto-generated C# code from HUGIN 9300 (double precision, table generator: false)

        public static HAPI.ClassCollection CreateClassCollection()
        {
            //string constants
            String[] s = new String[] { "Species", "SepalWidth", "PetalLength", "SepalLength", "PetalWidth", "Iris-setosa", "Iris-versicolor", "Iris-virginica", "iric_classifie" };
            //some numbers
            double[] data_node_iric_classifie_SepalWidth = new double[] { 3.418, 0.14517959, 2.77, 0.098469388, 2.974, 0.10400408 };
            double[] data_node_iric_classifie_PetalLength = new double[] { 1.464, 0.030106122, 4.26, 0.22081633, 5.552, 0.30458776 };
            double[] data_node_iric_classifie_SepalLength = new double[] { 5.006, 0.12424898, 5.936, 0.26643265, 6.588, 0.40434286 };
            double[] data_node_iric_classifie_PetalWidth = new double[] { 0.244, 0.011493878, 1.326, 0.039106122, 2.026, 0.075432653 };
            //temporary variables
            HAPI.Table t;
            HAPI.Model model;
            HAPI.ParseListener p = new HAPI.DefaultClassParseListener();
            HAPI.NodeList nodeList;

            HAPI.ClassCollection cc = new HAPI.ClassCollection();

            //create class iric_classifie
            HAPI.Class cls_iric_classifie = new HAPI.Class(cc, s[8]);

            HAPI.LabelledDCNode node_iric_classifie_Species = new HAPI.LabelledDCNode(cls_iric_classifie);
            node_iric_classifie_Species.SetName(s[0]);
            node_iric_classifie_Species.SetNumberOfStates(3);
            {
                int[] label = new int[] { 5, 6, 7 };
                for (uint i = 0; i < 3; ++i)
                {
                    node_iric_classifie_Species.SetStateLabel(i, s[label[i]]);
                }
            }

            HAPI.ContinuousChanceNode node_iric_classifie_SepalWidth = new HAPI.ContinuousChanceNode(cls_iric_classifie);
            node_iric_classifie_SepalWidth.SetName(s[1]);

            HAPI.ContinuousChanceNode node_iric_classifie_PetalLength = new HAPI.ContinuousChanceNode(cls_iric_classifie);
            node_iric_classifie_PetalLength.SetName(s[2]);

            HAPI.ContinuousChanceNode node_iric_classifie_SepalLength = new HAPI.ContinuousChanceNode(cls_iric_classifie);
            node_iric_classifie_SepalLength.SetName(s[3]);

            HAPI.ContinuousChanceNode node_iric_classifie_PetalWidth = new HAPI.ContinuousChanceNode(cls_iric_classifie);
            node_iric_classifie_PetalWidth.SetName(s[4]);

            //structure class iric_classifie
            node_iric_classifie_SepalWidth.AddParent(node_iric_classifie_Species);
            node_iric_classifie_PetalLength.AddParent(node_iric_classifie_Species);
            node_iric_classifie_SepalLength.AddParent(node_iric_classifie_Species);
            node_iric_classifie_PetalWidth.AddParent(node_iric_classifie_Species);

            //parameters class iric_classifie
            t = node_iric_classifie_Species.GetTable();
            {
                double[] data = new double[] { 0.333333, 0.335524, 0.331143 };
                t.SetData(data, 0, 3);
            }

            for (uint i = 0, j = 0; i < 3; ++i)
            {
                node_iric_classifie_SepalWidth.SetAlpha(data_node_iric_classifie_SepalWidth[j++], i);
                node_iric_classifie_SepalWidth.SetGamma(data_node_iric_classifie_SepalWidth[j++], i);
            }

            for (uint i = 0, j = 0; i < 3; ++i)
            {
                node_iric_classifie_PetalLength.SetAlpha(data_node_iric_classifie_PetalLength[j++], i);
                node_iric_classifie_PetalLength.SetGamma(data_node_iric_classifie_PetalLength[j++], i);
            }

            for (uint i = 0, j = 0; i < 3; ++i)
            {
                node_iric_classifie_SepalLength.SetAlpha(data_node_iric_classifie_SepalLength[j++], i);
                node_iric_classifie_SepalLength.SetGamma(data_node_iric_classifie_SepalLength[j++], i);
            }

            for (uint i = 0, j = 0; i < 3; ++i)
            {
                node_iric_classifie_PetalWidth.SetAlpha(data_node_iric_classifie_PetalWidth[j++], i);
                node_iric_classifie_PetalWidth.SetGamma(data_node_iric_classifie_PetalWidth[j++], i);
            }

            return cc;
        }

    }
}
