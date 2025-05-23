namespace lab5;

partial class DirectedForm
{
    private System.ComponentModel.IContainer components = null;
    private Button _stepButton;


    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.ClientSize = new System.Drawing.Size(800, 600);
        this.Text = "Напрямлений граф";

        _stepButton = new Button
        {
            Text = "Наступний крок",
            Location = new Point(10, 10),
            Size = new Size(200, 30)
        };
        _stepButton.Click += StepButton_Click;
        Controls.Add(_stepButton);
    }

    private void StepButton_Click(object sender, EventArgs e)
    {
        if (_steps != null && _steps.MoveNext())
        {
            _steps.Current.Invoke();
        }
    }

    #endregion
}
