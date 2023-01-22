using System;
using System.Windows.Forms;
using HAPI;
using h_number_t = System.Double;
using size_t = System.UInt64;

namespace WinFormsApp2.Domains
{
    /// <summary>
    /// Класс для создания домена
    /// </summary>
    public class SimpleBN
    {
        protected Domain domain;
        
        public SimpleBN(Domain domain)
        {
            this.domain = domain;
            BuildNetwork();
            domain.SaveAsNet("simpleBn.net");
        }
        
        public SimpleBN()
        {
            domain = new Domain();
            BuildNetwork();
            domain.SaveAsNet("simpleBn.net");
        }

        /// <summary>
        /// Инициализация сети
        /// </summary>
        public void BuildNetwork()
        {
            //размер блоков
            domain.SetNodeSize(new Size(120, 100));

            //вершины графов
            NumberedDCNode Sick = ConstructNDC("sick", "sick", new string[] { "sick", "no" });
            NumberedDCNode Dry = ConstructNDC("dry", "dry", new string[] { "dry", "no" });
            NumberedDCNode Loses = ConstructNDC("loses", "loses", new string[] { "yes", "no" });

            //построение структуры
            BuildStructure(Sick, Dry, Loses);

            //заполнение таблиц вероятностей
            SpecifyDistributions(Sick, Dry, Loses);

        }

        /// <summary>
        /// Начальная настройка вершины
        /// </summary>
        /// <param name="label">название вершины</param>
        /// <param name="name">внутренее имя веришины</param>
        /// <param name="nameState"> имя состояния</param>
        /// <returns>ссылка на вершину </returns>
        protected NumberedDCNode ConstructNDC(string label, string name, string[] nameState)
        {
            NumberedDCNode node = new NumberedDCNode(domain);
            //количесво состояний
            node.SetNumberOfStates((size_t)nameState.Length);

            //добавление значений и наименование вершин
            for (int i = 0; i < nameState.Length; i++)
            {
                node.SetStateLabel((size_t)i, nameState[i]);
            }
            //навзание вершины графа
            node.SetLabel(label);
            node.SetName(name);

            return node;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sick"></param>
        /// <param name="Dry"></param>
        /// <param name="Loses"></param>
        protected void BuildStructure(NumberedDCNode Sick, NumberedDCNode Dry, NumberedDCNode Loses)
        {
            // Установление причинно-следственных связей между вершинами БСД
            Loses.AddParent(Dry);
            Loses.AddParent(Sick);

            //Координаты вершин графов при визуализации  
            Sick.SetPosition(new Point(350, 200));
            Dry.SetPosition(new Point(150, 200));
            Loses.SetPosition(new Point(250, 50));
        }


        /// <summary>
        /// Заполнение ТУВ
        /// </summary>
        /// <param name="Dry"></param>
        /// <param name="Sick"></param>
        /// <param name="Loses"></param>
        protected void SpecifyDistributions(NumberedDCNode Dry, NumberedDCNode Sick, NumberedDCNode Loses)
        {
            FillTable(new h_number_t[] { 0.1, 0.9 }, Dry);
            FillTable(new h_number_t[] { 0.1, 0.9 }, Sick);
            FillTable(new h_number_t[] { 0.95, 0.05, 0.85, 0.15, 0.9, 0.1, 0.02, 0.98 }, Loses);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataProbability">массив вероятностей</param>
        /// <param name="Node">веришина домен</param>
        private void FillTable(h_number_t[] dataProbability, NumberedDCNode Node)
        {
            Table table = Node.GetTable(); //таблица узла
            h_number_t[] data = table.GetData(); //сами значения узла

            for (size_t i = 0; i < (size_t)dataProbability.Length; i++)
            {
                data[i] = dataProbability[i];
            }

            table.SetData(data);
        }

    }
}
