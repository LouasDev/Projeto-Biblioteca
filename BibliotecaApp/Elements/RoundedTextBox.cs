using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class RoundedTextBox : UserControl
{
    private TextBox innerTextBox;
    private Label placeholderLabel;
    private int _placeholderMarginLeft = 12;
    private bool useSystemPasswordChar = false;
    private Color _placeholderColor = Color.Gray;
    private Color currentBackColor;
    public bool internalUpdate = false;
    private bool isMouseOver = false;
    private bool isFocused = false;

    public event EventHandler TextChanged
    {
        add { innerTextBox.TextChanged += value; }
        remove { innerTextBox.TextChanged -= value; }
    }

    public RoundedTextBox()
    {
        this.DoubleBuffered = true;
        this.BackColor = Color.White;
        currentBackColor = this.BackColor;

        innerTextBox = new TextBox
        {
            BorderStyle = BorderStyle.None,
            BackColor = this.BackColor,
            ForeColor = Color.Black,
            Location = new Point(10, 7),
            Width = this.Width - 20,
            UseSystemPasswordChar = useSystemPasswordChar
        };

        innerTextBox.TextChanged += (s, e) => UpdatePlaceholder();
        innerTextBox.GotFocus += (s, e) => { isFocused = true; UpdatePlaceholder(); this.Invalidate(); };
        innerTextBox.LostFocus += (s, e) => { isFocused = false; UpdatePlaceholder(); this.Invalidate(); };

        // Redirecionar eventos de hover para o textbox
        innerTextBox.MouseEnter += (s, e) => { isMouseOver = true; this.Invalidate(); };
        innerTextBox.MouseLeave += (s, e) => { isMouseOver = false; this.Invalidate(); };

        placeholderLabel = new Label
        {
            Text = "Placeholder",
            ForeColor = _placeholderColor,
            BackColor = Color.Transparent,
            AutoSize = false,
            Font = new Font("Segoe UI", 9F, FontStyle.Italic),
            TextAlign = ContentAlignment.MiddleLeft
        };

        // Também propagar hover no label
        placeholderLabel.MouseEnter += (s, e) => { isMouseOver = true; this.Invalidate(); };
        placeholderLabel.MouseLeave += (s, e) => { isMouseOver = false; this.Invalidate(); };
        placeholderLabel.Click += (s, e) => innerTextBox.Focus();

        this.Controls.Add(innerTextBox);
        this.Controls.Add(placeholderLabel);
        this.Size = new Size(200, 35);

        // Hover geral do controle
        this.MouseEnter += (s, e) => { isMouseOver = true; this.Invalidate(); };
        this.MouseLeave += (s, e) => { isMouseOver = false; this.Invalidate(); };

        this.Resize += (s, e) => AdjustLayout();

        AdjustLayout();
        UpdatePlaceholder();
    }

    private void AdjustLayout()
    {
        innerTextBox.Location = new Point(10, (this.Height - innerTextBox.Font.Height) / 2 - 1);
        innerTextBox.Width = this.Width - 20;
        innerTextBox.Height = innerTextBox.Font.Height + 4;

        placeholderLabel.Location = new Point(_placeholderMarginLeft, innerTextBox.Top);
        placeholderLabel.Size = new Size(this.Width - _placeholderMarginLeft - 8, innerTextBox.Height + 2);
        placeholderLabel.BringToFront();
    }

    private void UpdatePlaceholder()
    {
        placeholderLabel.Visible = string.IsNullOrEmpty(innerTextBox.Text) && !innerTextBox.Focused;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

        using (GraphicsPath path = GetRoundedRectanglePath(rect, BorderRadius))
        {
            Color backColorToUse = isMouseOver ? HoverBackColor : this.BackColor;
            using (SolidBrush brush = new SolidBrush(backColorToUse))
            {
                e.Graphics.FillPath(brush, path);
            }

            Color borderToUse = isFocused ? BorderFocusColor : BorderColor;
            using (Pen pen = new Pen(borderToUse, BorderThickness))
            {
                e.Graphics.DrawPath(pen, path);
            }

            innerTextBox.BackColor = backColorToUse;
        }
    }

    private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int diameter = radius * 2;

        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();

        return path;
    }

    // ----------- MÉTODOS PÚBLICOS -----------

    [Category("Comportamento")]
    [Browsable(true)]
    [Description("Seleciona todo o texto no controle.")]
    public void SelectAll()
    {
        innerTextBox.SelectAll();
    }

    [Category("Comportamento")]
    [Browsable(true)]
    [Description("Define o foco para o controle.")]
    public new void Focus()
    {
        innerTextBox.Focus();
    }



    // ----------- PROPRIEDADES EXPONÍVEIS NO DESIGNER -----------

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Cor da borda quando o controle não está em foco.")]
    public Color BorderColor { get; set; } = Color.Black;

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Cor da borda quando o controle está em foco.")]
    public Color BorderFocusColor { get; set; } = Color.Blue;

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Cor de fundo ao passar o mouse sobre o controle.")]
    public Color HoverBackColor { get; set; } = Color.LightGray;

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Raio da borda arredondada.")]
    public int BorderRadius { get; set; } = 10;

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Espessura da borda.")]
    public int BorderThickness { get; set; } = 2;

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Texto exibido como placeholder.")]
    public string PlaceholderText
    {
        get => placeholderLabel.Text;
        set => placeholderLabel.Text = value;
    }

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Fonte do placeholder.")]
    public Font PlaceholderFont
    {
        get => placeholderLabel.Font;
        set
        {
            placeholderLabel.Font = value;
            AdjustLayout();
        }
    }

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Margem à esquerda do placeholder.")]
    public int PlaceholderMarginLeft
    {
        get => _placeholderMarginLeft;
        set
        {
            _placeholderMarginLeft = value;
            AdjustLayout();
        }
    }

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Cor do texto do placeholder.")]
    public Color PlaceholderColor
    {
        get => _placeholderColor;
        set
        {
            _placeholderColor = value;
            placeholderLabel.ForeColor = value;
            this.Invalidate();
        }
    }

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Cor do texto digitado.")]
    public Color TextColor
    {
        get => innerTextBox.ForeColor;
        set => innerTextBox.ForeColor = value;
    }

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Texto digitado no controle.")]
    public override string Text
    {
        get => innerTextBox.Text;
        set => innerTextBox.Text = value;
    }

    [Category("Comportamento")]
    [Browsable(true)]
    [Description("Define se o texto será mascarado (estilo senha).")]
    public bool UseSystemPasswordChar
    {
        get => useSystemPasswordChar;
        set
        {
            useSystemPasswordChar = value;
            innerTextBox.UseSystemPasswordChar = value;
            this.Invalidate();
        }
    }

    [Category("Aparência")]
    [Browsable(true)]
    [Description("Cor de fundo do controle.")]
    public override Color BackColor
    {
        get => base.BackColor;
        set
        {
            base.BackColor = value;
            currentBackColor = value;
            this.Invalidate();
        }
    }

    [Category("Comportamento")]
    [Browsable(true)]
    [Description("Fonte de dados usada para completar automaticamente a digitação.")]
    public AutoCompleteSource AutoCompleteSource
    {
        get => innerTextBox.AutoCompleteSource;
        set => innerTextBox.AutoCompleteSource = value;
    }

    [Category("Comportamento")]
    [Browsable(true)]
    [Description("Modo de autocompletar usado para sugerir e anexar texto.")]
    public AutoCompleteMode AutoCompleteMode
    {
        get => innerTextBox.AutoCompleteMode;
        set => innerTextBox.AutoCompleteMode = value;
    }

    [Category("Comportamento")]
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Description("Lista personalizada de sugestões de autocompletar.")]
    public AutoCompleteStringCollection AutoCompleteCustomSource
    {
        get => innerTextBox.AutoCompleteCustomSource;
        set => innerTextBox.AutoCompleteCustomSource = value;
    }

    [Category("Comportamento")]
    [Browsable(true)]
    [Description("Posição do cursor no texto.")]
    public int SelectionStart
    {
        get => innerTextBox.SelectionStart;
        set => innerTextBox.SelectionStart = value;
    }

    [Category("Comportamento")]
    [Browsable(true)]
    [Description("Comprimento do texto selecionado.")]
    public int SelectionLength
    {
        get => innerTextBox.SelectionLength;
        set => innerTextBox.SelectionLength = value;
    }

    [Category("Comportamento")]
    [Browsable(true)]
    [Description("Texto selecionado no controle.")]
    public string SelectedText
    {
        get => innerTextBox.SelectedText;
        set => innerTextBox.SelectedText = value;
    }

    [Category("Comportamento")]
    [Browsable(true)]
    [Description("Define a seleção de texto no controle.")]
    public void Select(int start, int length)
    {
        innerTextBox.Select(start, length);
    }
}