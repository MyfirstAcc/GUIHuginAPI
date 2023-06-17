﻿using HAPI;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text.RegularExpressions;
using h_number_t = System.Double;
using size_t = System.UInt64;

namespace WinFormsApp2.Controls
{
    /// <summary>
    /// класс для отрисовки модели
    /// </summary>
    public class HuginControl : Panel
    {
        private Domain domain = null;
        protected float zoom = 1f;


        //режим отражения 
        private bool view = true;

        public HuginControl()
        {
            BackColor = Color.White;
            BorderStyle = BorderStyle.Fixed3D;
            AutoScroll = true;
        }



        /// <summary>
        /// взятие домена из файла
        /// </summary>
        /// <param name="path">путь к домену</param>
        /// <param name="view">метод представления: таблица или общая структура</param>
        public void GetDomainFromFile(string path, bool view)
        {
            this.view = view;
            try
            {
                //создание домена, парсинг файла парсером по-умолчанию
                Domain dom = new Domain("iric_classifie.net", new DefaultClassParseListener());


                if (domain != null && domain.IsAlive() == true)
                    domain.Delete();
                domain = dom;
                domain.Compile();//компиляция домена
                RawDomain();//метод отрисовки домена
            }
            catch (ExceptionHugin eh)
            {
                MessageBox.Show(eh.Message);
            }
        }

        private void RawDomain()
        {
            Controls.Clear();//очистка области для рисования
            NodeList list = domain.GetNodes();
            //в заиимости от выбранного вида 
            if (view)
            {//рисуем прямоугольники
                foreach (Node node in list)
                    if (node is DiscreteChanceNode)
                        //для дискретных вершины
                        Controls.Add(new DCMonitor((DiscreteChanceNode)node));
                    else if (node is ContinuousChanceNode)
                        //для непрерывных вершины
                        Controls.Add(new CCMonitor((ContinuousChanceNode)node));
            }
            else
            { //рисуем овалы
                foreach (Node node in list)
                    if (node is DiscreteChanceNode)
                        //для  дискретных вершины
                        Controls.Add(new VisibleDCNode((DiscreteChanceNode)node));
                    else
                        //для остальных
                        Controls.Add(new VisibleNode(node));

            }
            Refresh();//вызов базового метода для перересовки элемента
        }

        public void GetDomain(Domain dom, bool view)
        {
            this.view = view;
            try
            {
                if (domain != null && domain.IsAlive() == true) //существует ли домен
                    domain.Delete();
                domain = dom;
                domain.Compile(); //компиляция домена
                RawDomain();
            }
            catch (ExceptionHugin eh)
            {
                MessageBox.Show(eh.Message);
            }
        }

        /// <summary>
        /// переопределяем метод отрисовки
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {

            e.Graphics.ScaleTransform(zoom, zoom);
            try
            {
                base.OnPaintBackground(e);
                if (domain != null && domain.IsAlive())
                {
                    NodeList list = domain.GetNodes();
                    foreach (Node node in list)
                        foreach (Node n in node.GetChildren())
                        {
                            Point locationOffset = new Point(domain.GetNodeSize().Width / 2,
                                                              domain.GetNodeSize().Height / 2);
                            //получение координат начала линии
                            Point from = node.GetPosition();
                            Point to = n.GetPosition();

                            //сдвиги координат
                            from.Offset(AutoScrollPosition);
                            from.Offset(locationOffset);
                            to.Offset(AutoScrollPosition);
                            to.Offset(locationOffset);

                            //получение координат для окончания линии

                            Pen pen = new Pen(Color.Black, 1);//перо для рисования
                            pen.CustomEndCap = new AdjustableArrowCap(7, 7); //рисование стрелки на конце линии
                            PointF interectPoint;

                            if (!view)
                            {
                                interectPoint = pointOnCycle(to, from); //вычисление точек пересечения овала и линии
                            }
                            else
                            {
                                interectPoint = pointOnRect(n, from); //вычисление
                                                                      //точек пересесчения прямоугольника и линии
                            }

                            e.Graphics.DrawLine(pen, from, interectPoint); //рисование стрелки от одного узла до другого
                        }
                }
            }
            catch (Exception eh)
            {
                MessageBox.Show(eh.Message);
            }
        }
        /// <summary>
        /// метод для определения координат точек пересечения линии и прямоугольника
        /// </summary>
        /// <param name="x">точка X начала линии</param>
        /// <param name="y">точка Y начала линии</param>
        /// <param name="minX">верхняя X точка прямоугольника </param>
        /// <param name="minY">верхняя Y точка прямоугольника</param>
        /// <param name="maxX">нижняя X точка прямоугольника</param>
        /// <param name="maxY">нижняя Y точка прямоугольника</param>
        /// <param name="validate">проверка на то, что пересечение есть</param>
        /// <returns>координаты пересечения прямоугольника и линии</returns>
        /// <exception cref="ArgumentException"></exception>
        private PointF pointOnRect(Node rect, Point from)
        {
            float x, y, minX, minY, maxX, maxY;
            x = from.X; y = from.Y;

            minX = rect.GetPosition().X;
            minY = rect.GetPosition().Y;

            maxX = rect.GetPosition().X + domain.GetNodeSize().Width;
            maxY = rect.GetPosition().Y + domain.GetNodeSize().Height;

            if (minX < x && x < maxX && minY < y && y < maxY)
                throw new ArgumentException("Point " + x + y + "cannot be inside "
                    + "the rectangle: " + minX + minY + " - " + maxX + maxY + ".");
            float midX = (minX + maxX) / 2;
            float midY = (minY + maxY) / 2;

            float m = (midY - y) / (midX - x);

            if (x <= midX)
            { // проверка левой строны
                var minXy = m * (minX - x) + y;
                if (minY <= minXy && minXy <= maxY)
                    return new PointF(minX, minXy);
            }

            if (x >= midX)
            { // проверка правой стороны
                var maxXy = m * (maxX - x) + y;
                if (minY <= maxXy && maxXy <= maxY)
                    return new PointF(maxX, maxXy);
            }

            if (y <= midY)
            { // проверка верха
                var minYx = (minY - y) / m + x;
                if (minX <= minYx && minYx <= maxX)
                    return new PointF(minYx, minY);
            }

            if (y >= midY)
            { // проверка низа
                var maxYx = (maxY - y) / m + x;
                if (minX <= maxYx && maxYx <= maxX)
                    return new PointF(maxYx, maxY);
            }

            if (x == midX && y == midY) return new PointF(x, y);

            // навсякий случай
            throw new ArgumentException("Cannot find intersection for " + x + y
                + " inside rectangle " + minX + minY + " - " + maxX + maxY + ".");


        }

        private PointF pointOnCycle(Point to, Point from)
        {
            float scaleX = domain.GetNodeSize().Width / 2;
            float scaleY = domain.GetNodeSize().Height / 2;
            float scaleCos = (to.X - from.X) * ((float)domain.GetNodeSize().Height
                              / (float)domain.GetNodeSize().Width);
            float scaleSin = to.Y - from.Y;
            float norm1 = 1f / (float)Math.Sqrt(scaleCos * scaleCos + scaleSin * scaleSin);
            scaleCos *= norm1;
            scaleSin *= norm1;
            float cos = to.X - from.X;
            float sin = to.Y - from.Y;
            float norm2 = 1f / (float)Math.Sqrt(cos * cos + sin * sin);
            cos *= norm2;
            sin *= norm2;
            return new PointF(to.X - scaleCos * scaleX - cos - sin,
                to.Y - scaleSin * scaleY - sin + cos);
        }

        public class DCMonitor : FlowLayoutPanel
        {
            private DiscreteChanceNode node;
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x20;
                    return cp;
                }
            }
            public DCMonitor(DiscreteChanceNode dcNode)
            {
                node = dcNode;
                MouseMove += MouseMoveHandler;
                MouseClick += MouseClickHandler;
                BackColor = Color.White;
                BorderStyle = BorderStyle.FixedSingle;
                AutoScroll = true;
                Width = 160;
                Height = 130;
                Label lbl = new Label();
                lbl.Text = node.GetName() + ": " + node.GetLabel();
                lbl.MouseClick += MouseClickHandler;
                lbl.MouseMove += MouseMoveHandler;
                Controls.Add(lbl);
                for (size_t i = 0; i < node.GetNumberOfStates(); i++)
                    Controls.Add(new State(node, i));
                Location = node.GetPosition();
            }

            private void MouseClickHandler(object sender, MouseEventArgs e)
            {
                BringToFront();
            }

            private void MouseMoveHandler(object sender, MouseEventArgs e)
            {

            }

            private void RedrawNet()
            {
                Parent.Refresh();
            }

            private class State : UserControl
            {
                size_t stateNumber = 0;
                DiscreteChanceNode node;

                public State(DiscreteChanceNode dcNode, size_t state)
                {

                    Width = 150;
                    Height = 20;
                    BackColor = Color.White;
                    stateNumber = state;
                    node = dcNode;
                    MouseDoubleClick += MouseDoubleClickHandler;
                    MouseClick += MouseClickHandler;


                }

                private void MouseClickHandler(object sender, MouseEventArgs e)
                {

                    if (node.GetName() == "loses")
                    {
                        Table table = node.GetTable();

                        table.SetDataItem(1, 0.9);

                        node.GetHomeDomain().Propagate(Domain.Equilibrium.H_EQUILIBRIUM_SUM,
                                                     Domain.EvidenceMode.H_EVIDENCE_MODE_NORMAL);

                        ((DCMonitor)Parent).RedrawNet();
                    }



                    /*Debug.WriteLine("n");
                    InputBoxValidation valid = delegate (string val)
                       {
                           if (val == "")
                           {
                               return "Value cannot empty.";
                           }
                           if (!(new Regex(@"^\d+,\d+|^\d+$")).IsMatch(val))
                               return "Error!Numbers only";
                           return "";
                       };


                    double value = node.GetBelief(stateNumber);
                    if (InputBox.Show("Введите Mean!", "Введите Mean:", ref value, valid) == DialogResult.OK)
                    {
                        node.RetractFindings();
                        Table table = node.GetTable();
                        

                        //double t = 1 - value;
                       h_number_t[] d = new h_number_t[table.GetSize()];

                        for (size_t i = 0; i < table.GetSize(); i++)
                        {
                            if (i == stateNumber)
                            {
                                d[i] = value;
                            }
                            else
                            {
                                d[i] = table[i];
                            }
                        }

                        table.SetData(d);

                        node.GetHomeDomain().Propagate(Domain.Equilibrium.H_EQUILIBRIUM_SUM,
                                                       Domain.EvidenceMode.H_EVIDENCE_MODE_NORMAL);

                        ((DCMonitor)Parent).RedrawNet();

                    }
                    */
                }

                private void MouseDoubleClickHandler(object sender, MouseEventArgs e)
                {
                    try
                    {
                        if (node.EvidenceIsEntered() && node.GetBelief(stateNumber) > 0)
                            node.RetractFindings();
                        else
                            node.SelectState(stateNumber);
                        node.GetHomeDomain().Propagate(Domain.Equilibrium.H_EQUILIBRIUM_SUM,
                                                       Domain.EvidenceMode.H_EVIDENCE_MODE_NORMAL);
                        ((DCMonitor)Parent).RedrawNet();

                    }
                    catch (ExceptionHugin eh)
                    {
                        MessageBox.Show(eh.Message);
                    }
                }

                protected override void OnPaint(PaintEventArgs e)
                {
                    try
                    {
                        base.OnPaint(e);
                        Graphics g = e.Graphics;
                        Brush fillBrush = node.EvidenceIsEntered() ? Brushes.Red : Brushes.Green;
                        g.FillRectangle(fillBrush, 0, 0, (float)((Width - 1) * node.GetBelief(stateNumber)),
                                        Height - 1);
                        string text = node.GetStateLabel(stateNumber) + " " + node.GetBelief(stateNumber);
                        g.DrawString(text, new Font("Microsoft Sans Serif", 8.25f),
                                      Brushes.Black, 10f, Height / 2 - 5);
                        g.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
                    }
                    catch (ExceptionHugin eh)
                    {
                        MessageBox.Show(eh.Message);
                    }
                }
            }
        }

        public class CCMonitor : FlowLayoutPanel
        {


            private ContinuousChanceNode node;
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x20;
                    return cp;
                }
            }
            public CCMonitor(ContinuousChanceNode dcNode)
            {
                node = dcNode;
                MouseMove += MouseMoveHandler;
                MouseClick += MouseClickHandler;
                BackColor = Color.White;
                Width = 130;
                Height = 100;
                BorderStyle = BorderStyle.FixedSingle;
                AutoScroll = false;

                Label lbl = new Label();
                lbl.Text = node.GetName() + ": " + node.GetLabel();
                lbl.MouseClick += MouseClickHandler;
                lbl.MouseMove += MouseMoveHandler;
                Controls.Add(lbl);
                var rows = new string[] { string.Format("{0, -10} {1, 5}", "Mean", node.GetMean()),
                    string.Format("{0, -10} {1, 5}", "Variance", node.GetVariance()) };


               
                    Controls.Add(new State(node));
                
                
            

                Location = node.GetPosition();
            }

            private void MouseClickHandler(object sender, MouseEventArgs e)
            {
                /*BringToFront();
                InputBoxValidation validation = delegate (string val)
                {
                    if (val == "")
                        return "Value cannot be empty.";
                    if (!(new Regex(@"[0-9].[0-9]")).IsMatch(val))
                        return "Невверные данные! Повторите ввод!";
                    return "";
                };

                double value = double.MaxValue;
                if (InputBox.Show("Непрерывное событие", "Матожидание:", ref value, validation) == DialogResult.OK)
                {
                    MessageBox.Show(value.ToString("G", CultureInfo.CreateSpecificCulture("eu-ES")));
                }*/

            }

            private void MouseMoveHandler(object sender, MouseEventArgs e)
            {

            }

            private void RedrawNet()
            {
                Parent.Refresh();
            }

            private class State : UserControl
            {
                string row;
                ContinuousChanceNode node;
                TextBox txtbox;

                public State(ContinuousChanceNode ccNode)
                {
                    Width = 150;
                    Height = 45;
                    node = ccNode;
                    BackColor = Color.White;
                    this.row = row;
                    MouseDoubleClick += MouseDoubleClickHandler;
                    txtbox = new TextBox();
                    txtbox.BackColor = Color.Thistle;
                    txtbox.KeyPress += KeyPressHandler;
                }


                private void KeyPressHandler(object sender, KeyPressEventArgs e)
                {
                    if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 44)
                    {
                        e.Handled = true;
                    }

                    if (e.KeyChar == (char)Keys.Return)
                    {
                        Debug.WriteLine(txtbox.Text);
                        node.RetractFindings();
                        node.EnterValue(h_number_t.Parse(txtbox.Text));
                        node.GetHomeDomain().Propagate(Domain.Equilibrium.H_EQUILIBRIUM_SUM,
                                                       Domain.EvidenceMode.H_EVIDENCE_MODE_NORMAL);

                        ((CCMonitor)Parent).RedrawNet();
                    }

                    
                }
                private void MouseDoubleClickHandler(object sender, MouseEventArgs e)
                {
                    try
                    {
                        InputBoxValidation valid = delegate (string val)
                        {
                            if (val == "")
                            {
                                return "Value cannot empty.";
                            }
                            if (!(new Regex(@"^\d+,\d+|^\d+$")).IsMatch(val))
                                return "Error!Numbers only";
                            return "";
                        };
                      
                        double value = node.GetMean();
                        if (InputBox.Show("Введите значение "+ node.GetName(), "Введите Mean:", ref value, valid) == DialogResult.OK)
                        {
                            node.RetractFindings();
                            node.EnterValue(value);
                            node.GetHomeDomain().Propagate(Domain.Equilibrium.H_EQUILIBRIUM_SUM,
                                                           Domain.EvidenceMode.H_EVIDENCE_MODE_NORMAL);

                            ((CCMonitor)Parent).RedrawNet();
                        }
                    }
                    catch (ExceptionHugin eh)
                    {
                        MessageBox.Show(eh.Message);
                    }
                }



                protected override void OnPaint(PaintEventArgs e)
                {
                    try
                    {
                        base.OnPaint(e);
                        Graphics g = e.Graphics;
                        Brush fillBrush = node.EvidenceIsEntered() ? Brushes.Red : Brushes.Azure;
                        Label lbl = new Label();
                        lbl.Text = "Мат. ожид: ";
                        txtbox.Location = new Point(0, 20);
                        Debug.WriteLine(row);
                        txtbox.Text = string.Format("{0}",node.GetMean());
                        Controls.Add(lbl);
                        Controls.Add(txtbox);                    

                    }
                    catch (ExceptionHugin eh)
                    {
                        MessageBox.Show(eh.Message);
                    }
                }
            }
        }

        public class VisibleDCNode : UserControl
        {
            private DCMonitor monitor;
            private DiscreteChanceNode node;

            public VisibleDCNode(DiscreteChanceNode dcNode)
            {
                node = dcNode;
                Size = node.GetHome().GetNodeSize();
                Location = node.GetPosition();
                monitor = new DCMonitor(node);

            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x20;
                    return cp;
                }
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                pevent.Graphics.FillEllipse(Brushes.LightYellow, 0, 0, Width - 1, Height - 1);
                pevent.Graphics.DrawEllipse(Pens.Black, 0, 0, Width - 1, Height - 1);
                pevent.Graphics.DrawString(node.GetName() + ": " + node.GetLabel(),
                      new Font("Microsoft Sans Serif", 8.25f), Brushes.Black, 10f, Height / 2 - 5);
            }
        }

        public class VisibleNode : UserControl
        {
            private Node node;

            public VisibleNode(Node theNode)
            {
                node = theNode;
                Size = node.GetHome().GetNodeSize();
                Location = node.GetPosition();

            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x20;
                    return cp;
                }
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                //empty
            }

            protected override void OnPaint(PaintEventArgs pevent)
            {
                pevent.Graphics.FillEllipse(Brushes.Yellow, 0, 0, Width - 1, Height - 1);
                pevent.Graphics.DrawEllipse(Pens.Black, 2, 2, Width-6, Height - 6);

                pevent.Graphics.DrawEllipse(Pens.Black, 0, 0, Width - 1, Height - 1);
                pevent.Graphics.DrawString(node.GetName() + ": " + node.GetLabel(),
                      new Font("Microsoft Sans Serif", 8.25f), Brushes.Black, 10f, Height / 2 - 5);
                pevent.Graphics.DrawString(node.GetName() + ": " + node.GetAttribute("Alpha"),
                     new Font("Microsoft Sans Serif", 8.25f), Brushes.Black, 10f, Height / 2 - 5);
            }
        }


        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.ScaleTransform(2, 2);
        }

    }
}
