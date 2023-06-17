using HAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using h_number_t = System.Double;
using size_t = System.UInt64;

namespace WinFormsApp2.Domains
{
    public class BAPCCN2
    {
        protected Domain domain;

        public BAPCCN2(Domain domain)
        {
            this.domain = domain;
            BuildNetwork();
        }

        // Construct numbered discrete chance node.
        protected NumberedDCNode ConstructNDC(string label, string name, size_t n, string[] nameState)
        {
            NumberedDCNode node = new NumberedDCNode(domain);

            node.SetNumberOfStates(n);

            for (size_t i = 0; i < n; i++)
            {
                node.SetStateValue(i, i);
                node.SetStateLabel(i, nameState[i]);
            }
            node.SetLabel(label);
            node.SetName(name);

            return node;
        }

        protected void BuildStructure(NumberedDCNode Area, NumberedDCNode Room, NumberedDCNode TimeMetro, ContinuousChanceNode Price)
        {
            Price.AddParent(Area);
            Price.AddParent(Room);
            Price.AddParent(TimeMetro);


            Area.SetPosition(new Point(350, 50));
            Price.SetPosition(new Point(225, 225));
            Room.SetPosition(new Point(50, 50));
            TimeMetro.SetPosition(new Point(50, 300));


        }

        protected void SpecifyDistributions(NumberedDCNode Area, NumberedDCNode Room, NumberedDCNode TimeMetro, ContinuousChanceNode Price)
        {
            Table table = Room.GetTable();
            h_number_t[] data = table.GetData();

            data[0] = 0.33;
            data[1] = 0.33;
            data[2] = 0.33;
            table.SetData(data);

            table = Area.GetTable();
            data = table.GetData();
            data[0] = 0.33;
            data[1] = 0.33;
            data[2] = 0.33;
            table.SetData(data);


            table = TimeMetro.GetTable();
            data = table.GetData();
            data[0] = 0.5;
            data[1] = 0.5;
            table.SetData(data);


            Dictionary<string, float[]>hashtable = new Dictionary<string, float[]>();
            hashtable.Add("Mean", new float[] { 3210, 3450, 2460, 4280, 4600, 3280, 5350, 4140, 3000, 5100, 5520, 3999, 6400, 6900, 4920 });
            hashtable.Add("Varience", new float[] { 25760, 29756, 15129, 45796, 52900, 29896, 71556, 182656, 42025, 37094, 42849, 21785.0f, 665946.0f, 76176.0f, 38730.0f });

            foreach (var elem in hashtable)
            {
                if (elem.Key == "Mean")
                {
                    for (int i = 0; i < elem.Value.Length; i++)
                    {
                        Price.SetAlpha(elem.Value[i], (size_t)i);
                    }
                }
                else
                {
                    for (int i = 0; i < elem.Value.Length; i++)
                    {
                        Price.SetGamma(elem.Value[i], (size_t)i);
                    }
                }

            }
        }

        // Build the Bayesian network.
        public void BuildNetwork()
        {
            domain.SetNodeSize(new Size(150, 120));

            NumberedDCNode Area = ConstructNDC("Area", "Area", 3, new string[] { "Nevskiy", "Moskovskiy", "Pushkinskiy" });

            ContinuousChanceNode Price = ConstructNCC("Price", "Price");
            NumberedDCNode Room = ConstructNDC("Romm", "Room", 3, new string[] { "Room 1", "Room 2", "Room 3" });
            NumberedDCNode TimeMetro = ConstructNDC("Time_Metro", "Time_Metro", 2, new string[] { ">= 20 min", "<=20" });

            BuildStructure(Area, Room, TimeMetro, Price);
            SpecifyDistributions(Area, Room, TimeMetro, Price);
        }

        private ContinuousChanceNode ConstructNCC(string name, string label)
        {
            ContinuousChanceNode node = new ContinuousChanceNode(domain);

            node.SetLabel(label);
            node.SetName(name);


            return node;
        }
    }
}
