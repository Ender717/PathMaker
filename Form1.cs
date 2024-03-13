namespace PathMaker
{
    public partial class Form1 : Form
    {
        private PointCanvas pointCanvas;

        public Form1()
        {
            InitializeComponent();

            pointCanvas = new PointCanvas();
            pointCanvas.BackgroundImage = Properties.Resources.Over_Under_Field;
            pointCanvas.BackgroundImageLayout = ImageLayout.Stretch;
            pointCanvas.Location = new Point(59, 21);
            pointCanvas.Name = "fieldPanel";
            pointCanvas.Size = new Size(886, 886);
            pointCanvas.TabIndex = 0;
            Controls.Add(pointCanvas);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            pointCanvas.AddPoint(new PathPoint(e.X, e.Y));
        }
    }
}
