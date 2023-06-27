using HAPI;
using Microsoft.Win32.SafeHandles;
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

            /*this.Height = 5000;
            this.Width = 1500;*/
        }

        public void SetZoom(float scale)
        {
            zoom = scale;
            RawDomain();
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
                Domain dom = new Domain(path, new DefaultClassParseListener());


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
            Controls.Clear();
            NodeList list = domain.GetNodes();
         
            if (view)
            {//рисуем таблицы
                foreach (Node node in list)
                    if (node is DiscreteChanceNode)
                        //для дискретных вершины
                        Controls.Add(new DCMonitor((DiscreteChanceNode)node, zoom, zoom));
                    else if (node is ContinuousChanceNode)
                        //для непрерывных вершины
                        Controls.Add(new CCMonitor((ContinuousChanceNode)node));
            }
            else
            { //рисуем вершины
                foreach (Node node in list)
                    if (node is DiscreteChanceNode)
                        //для  дискретных вершины
                        Controls.Add(new VisibleDCNode((DiscreteChanceNode)node,zoom,zoom));
                    else
                        //для интервальных
                        Controls.Add(new VisibleCCNode(node));

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

                            Pen pen = new Pen(Color.CornflowerBlue, 1);//перо для рисования
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Debug.WriteLine(e.X.ToString() + " " + e.Y.ToString());
            if (e.Button == MouseButtons.Left)
            {
                Location = new Point(Cursor.Position.X + e.X, Cursor.Position.Y + e.Y);
            }
        }


        /// <summary>
        /// метод для определения координат точек пересечения линии и прямоугольника
        /// </summary>
        /// <param name="to">координаты начала линии</param>
        /// <param name="from">координаты конца линии</param>
        /// <returns>координаты пересечения прямоугольника и линии</returns>
        /// <exception cref="ArgumentException"></exception>
        private PointF pointOnRect(Node to, Point from)
        {
            float x, y, XMinimum, Yminimum, XMaximum, YMaximum;
            x = from.X; y = from.Y;

            XMinimum = to.GetPosition().X;
            Yminimum = to.GetPosition().Y;

            XMaximum = to.GetPosition().X + domain.GetNodeSize().Width;
            YMaximum = to.GetPosition().Y + domain.GetNodeSize().Height;

            if (XMinimum < x && x < XMaximum && Yminimum < y && y < YMaximum)
                throw new ArgumentException("Point " + x + y + "cannot be inside "
                    + "the rectangle: " + XMinimum + Yminimum + " - " + XMaximum + YMaximum + ".");
            float midX = (XMinimum + XMaximum) / 2;
            float midY = (Yminimum + YMaximum) / 2;

            float mid = (midY - y) / (midX - x);

            if (x <= midX)
            { // проверка левой строны
                var minXy = mid * (XMinimum - x) + y;
                if (Yminimum <= minXy && minXy <= YMaximum)
                    return new PointF(XMinimum, minXy);
            }

            if (x >= midX)
            { // проверка правой стороны
                var maxXy = mid * (XMaximum - x) + y;
                if (Yminimum <= maxXy && maxXy <= YMaximum)
                    return new PointF(XMaximum, maxXy);
            }

            if (y <= midY)
            { // проверка верхней строны
                var minYx = (Yminimum - y) / mid + x;
                if (XMinimum <= minYx && minYx <= XMaximum)
                    return new PointF(minYx, Yminimum);
            }

            if (y >= midY)
            { // проверка нижней стороны
                var maxYx = (YMaximum - y) / mid + x;
                if (XMinimum <= maxYx && maxYx <= XMaximum)
                    return new PointF(maxYx, YMaximum);
            }

            if (x == midX && y == midY) return new PointF(x, y);

            // если точки не попали в набор случай
            throw new ArgumentException("Не удается найти пересечение для " + x + y
                + " внутри прямоугольника " + XMinimum + Yminimum + " - " + XMaximum + YMaximum + ".");
        }


        /// <summary>
        /// Метод преобразования координат
        /// для отрисовки связей (линий) между узлами в графической библиотеке
        /// вычисление координат для стрелки 
        /// </summary>
        /// <param name="to">координаты начала линии</param>
        /// <param name="from">координаты коца линии</param>
        /// <returns>координаты точки пересечния овлов</returns>
        private PointF pointOnCycle(Point to, Point from)
        {
            //определение масшаба
            float scaleX = domain.GetNodeSize().Width / 2;
            float scaleY = domain.GetNodeSize().Height / 2;
            float scaleCos = (to.X - from.X) * ((float)domain.GetNodeSize().Height
                              / (float)domain.GetNodeSize().Width);

            //sin - разность координат точек
            float scaleSin = to.Y - from.Y;
            //Нормализация cos и sin,
            //равный обратной величине квадратного корня от суммы квадратов
            float norm1 = 1f / (float)Math.Sqrt(scaleCos * scaleCos + scaleSin * scaleSin);
            scaleCos *= norm1;
            scaleSin *= norm1;
            //сos - разность координат точек
            float cos = to.X - from.X;
            float sin = to.Y - from.Y;
            //Нормализация cos и sin,
            //равный обратной величине квадратного корня от суммы квадратов
            float norm2 = 1f / (float)Math.Sqrt(cos * cos + sin * sin);
            cos *= norm2;
            sin *= norm2;

            //Координаты X и Y конечной точки (to) вычитаются из вычисленных
            //преобразований для масштаба и направления связи
            return new PointF(to.X - scaleCos * scaleX - cos - sin,
                to.Y - scaleSin * scaleY - sin + cos);
        }

        public class DCMonitor : FlowLayoutPanel
        {
            private DiscreteChanceNode node;
            public float zoomX;
            public float zoomY;


            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x20;
                    return cp;
                }
            }
            public DCMonitor(DiscreteChanceNode dcNode, float zoomX, float zoomY)
            {
                node = dcNode;
                this.zoomX = zoomX;
                this.zoomY = zoomY;

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
                    Controls.Add(new State(node, i,zoomX,zoomY));
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
                private float zoomX;
                private float zoomY;

                DiscreteChanceNode node;

                public State(DiscreteChanceNode dcNode, size_t state, float zoomX, float zoomY)
                {

                    Width = 150;
                    Height = 20;
                    BackColor = Color.White;
                    this.zoomY = zoomY;
                    this.zoomX = zoomX;
                    stateNumber = state;
                    node = dcNode;
                    MouseDoubleClick += MouseDoubleClickHandler;

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
                        g.ScaleTransform(zoomX, zoomY);
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
                                return "Поле не может быть пустым!";
                            }
                            if (!(new Regex(@"^\d+,\d+|^\d+$")).IsMatch(val))
                                return "Ошибка!Неправильный ввод! (X,XX)";
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
                        lbl.Text = "Значение вершины: ";
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

            public VisibleDCNode(DiscreteChanceNode dcNode,float zoomX, float zoomY)
            {
                node = dcNode;
                Size = node.GetHome().GetNodeSize();
                Location = node.GetPosition();
                monitor = new DCMonitor(node,zoomX,zoomY);

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

        public class VisibleCCNode : UserControl
        {
            private Node node;

            public VisibleCCNode(Node theNode)
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

    }
}
