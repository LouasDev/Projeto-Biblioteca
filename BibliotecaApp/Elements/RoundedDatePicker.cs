using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

/// <summary>
/// RoundedDatePicker — versão ajustada: formatação ao digitar + hover customizável do ícone/área.
/// </summary>
public class RoundedDatePicker : UserControl
{
    private TextBox innerTextBox;
    private Label placeholderLabel;
    private PictureBox calendarIcon;
    private MonthCalendar calendarPopup;
    private bool isMouseOver = false;
    private bool isFocused = false;
    private bool isIconHover = false;
    private int _placeholderMarginLeft = 12;
    private Color _placeholderColor = Color.Gray;
    private DateTime? previousValidDate = null;
    private Form parentFormCached = null;

    // novos campos para customização do ícone/área
    private Color _iconColor;
    private Color _iconHoverColor;
    private Color _iconHoverAreaColor;

    // formatação em tempo real
    private bool isFormattingText = false;

    public RoundedDatePicker()
    {
        // ---------- DEFAULTS MÍNIMOS (não chamam AdjustLayout imediatamente) ----------
        this.DoubleBuffered = true;
        base.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold);
        this.ForeColor = Color.FromArgb(20, 42, 60);
        this.Size = new Size(199, 35);
        this.TabStop = false;

        // defaults do ícone (guardados em campos simples)
        _iconColor = Color.FromArgb(80, 80, 80);
        _iconHoverColor = Color.FromArgb(30, 61, 88);
        _iconHoverAreaColor = Color.FromArgb(220, 220, 220);

        // ---------- CRIA CONTROLES INTERNOS PRIMEIRO ----------
        innerTextBox = new TextBox
        {
            BorderStyle = BorderStyle.None,
            BackColor = this.BackColor,
            ForeColor = this.ForeColor,
            ReadOnly = false,
            Location = new Point(10, 7),
            Width = Math.Max(40, this.Width - 54),
            TabStop = true,
            Font = base.Font
        };

        innerTextBox.GotFocus += (s, e) => { isFocused = true; Invalidate(); };
        innerTextBox.LostFocus += (s, e) =>
        {
            isFocused = false;
            ValidateAndApplyTextDate();
            Invalidate();

            // fechar calendário se o foco saiu do controle e não foi pro calendário
            this.BeginInvoke((Action)(() =>
            {
                bool calendarHasFocus = calendarPopup != null && calendarPopup.ContainsFocus;
                bool controlHasFocus = this.ContainsFocus;
                if (!calendarHasFocus && !controlHasFocus)
                    HideCalendar();
            }));
        };

        innerTextBox.KeyDown += (s, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                ValidateAndApplyTextDate();
                e.SuppressKeyPress = true;
            }
        };

        innerTextBox.TextChanged += InnerTextBox_TextChanged;

        placeholderLabel = new Label
        {
            Text = "Selecione uma data...",
            ForeColor = _placeholderColor,
            BackColor = Color.Transparent,
            AutoSize = false,
            Font = new Font("Segoe UI", 9F, FontStyle.Italic),
            TextAlign = ContentAlignment.MiddleLeft
        };
        placeholderLabel.Click += (s, e) => ToggleCalendar();

        calendarIcon = new PictureBox
        {
            Size = new Size(24, 24),
            Anchor = AnchorStyles.Right,
            Cursor = Cursors.Hand,
            BackColor = Color.Transparent
        };
        calendarIcon.Paint += DrawCalendarIcon;
        calendarIcon.MouseEnter += (s, e) => { isIconHover = true; Invalidate(); };
        calendarIcon.MouseLeave += (s, e) => { isIconHover = false; Invalidate(); };

        calendarIcon.Click += (s, e) =>
        {
            // solicita foco para o TextBox
            innerTextBox.Focus();

            // executa depois da troca de foco para abrir o calendário sem roubar o foco
            this.BeginInvoke((Action)(() =>
            {
                if (calendarPopup != null)
                {
                    if (!calendarPopup.Visible)
                    {
                        ToggleCalendar(); // abre o calendário
                    }
                    // NÃO traze o foco para o calendarPopup (assim o innerTextBox mantém o foco)
                }
                else
                {
                    ToggleCalendar();
                }

                // posiciona o cursor no final do texto
                try
                {
                    innerTextBox.SelectionStart = innerTextBox.Text?.Length ?? 0;
                    innerTextBox.SelectionLength = 0;
                    innerTextBox.Focus(); // reforça o foco no TextBox
                }
                catch { /* ignore */ }
            }));
        };


        calendarPopup = new MonthCalendar
        {
            Visible = false,
            MaxSelectionCount = 1,
            ShowTodayCircle = true,
            BackColor = Color.White,
            ForeColor = Color.Black,
            TitleBackColor = Color.WhiteSmoke,
            TitleForeColor = Color.Black
        };
        calendarPopup.DateSelected += (s, e) =>
        {
            SelectedDate = e.Start;
            HideCalendar();
        };

        // adiciona controles à coleção do UserControl
        Controls.Add(innerTextBox);
        Controls.Add(placeholderLabel);
        Controls.Add(calendarIcon);

        // ---------- AGORA DEFINIMOS AS PROPRIEDADES QUE PODEM CHAMAR AdjustLayout/Invalidate ----------
        this.BorderColor = Color.FromArgb(204, 204, 204);
        this.BorderFocusColor = Color.FromArgb(30, 61, 88);
        this.BorderRadius = 10;
        this.BorderThickness = 1;
        this.HoverBackColor = Color.LightGray;
        this.PlaceholderColor = Color.Gray;
        this.PlaceholderText = "Selecione uma data...";
        this.PlaceholderFont = new Font("Segoe UI", 12.2F, FontStyle.Regular);

        // eventos gerais (após controles)
        this.MouseEnter += (s, e) => { isMouseOver = true; Invalidate(); };
        this.MouseLeave += (s, e) => { isMouseOver = false; isIconHover = false; Invalidate(); };
        this.MouseMove += RoundedDatePicker_MouseMove;
        this.Resize += (s, e) => AdjustLayout();

        // layout inicial
        AdjustLayout();
        UpdatePlaceholderVisibility();
    }


    private void RoundedDatePicker_MouseMove(object sender, MouseEventArgs e)
    {
        var iconArea = new Rectangle(this.Width - calendarIcon.Width - 16, 0, calendarIcon.Width + 16, this.Height);
        bool hover = iconArea.Contains(e.Location);
        if (hover != isIconHover)
        {
            isIconHover = hover;
            Invalidate();
        }
    }

    private void AdjustLayout()
    {
        if (innerTextBox == null || placeholderLabel == null || calendarIcon == null)
            return;

        innerTextBox.Location = new Point(10, (Height - innerTextBox.Font.Height) / 2 - 1);
        innerTextBox.Width = Math.Max(40, Width - 54);
        innerTextBox.Height = innerTextBox.Font.Height + 4;

        placeholderLabel.Location = new Point(_placeholderMarginLeft, innerTextBox.Top);
        placeholderLabel.Size = new Size(Width - _placeholderMarginLeft - 54, innerTextBox.Height + 2);

        calendarIcon.Location = new Point(Width - calendarIcon.Width - 8, (Height - calendarIcon.Height) / 2);
        placeholderLabel.BringToFront();
    }

    private void ToggleCalendar()
    {
        if (!calendarPopup.Visible)
            ShowCalendar();
        else
            HideCalendar();
    }

    private void ShowCalendar()
    {
        var parentForm = this.FindForm();
        if (parentForm == null) return;

        if (parentFormCached != parentForm)
        {
            if (parentFormCached != null)
            {
                parentFormCached.MouseDown -= ParentForm_MouseDown;
                parentFormCached.LocationChanged -= ParentForm_LocationOrSizeChanged;
                parentFormCached.SizeChanged -= ParentForm_LocationOrSizeChanged;
                parentFormCached.Deactivate -= ParentForm_Deactivate;
            }

            parentFormCached = parentForm;
            parentFormCached.MouseDown += ParentForm_MouseDown;
            parentFormCached.LocationChanged += ParentForm_LocationOrSizeChanged;
            parentFormCached.SizeChanged += ParentForm_LocationOrSizeChanged;
            parentFormCached.Deactivate += ParentForm_Deactivate;
        }

        if (!parentForm.Controls.Contains(calendarPopup))
            parentForm.Controls.Add(calendarPopup);

        Point screenBelow = this.PointToScreen(new Point(0, this.Height));
        Point clientBelow = parentForm.PointToClient(screenBelow);

        int x = clientBelow.X;
        int y = clientBelow.Y;

        if (y + calendarPopup.Height > parentForm.ClientSize.Height)
        {
            int aboveY = parentForm.PointToClient(this.PointToScreen(new Point(0, 0))).Y - calendarPopup.Height - 2;
            if (aboveY >= 0) y = aboveY;
            else y = Math.Max(0, parentForm.ClientSize.Height - calendarPopup.Height);
        }

        if (x + calendarPopup.Width > parentForm.ClientSize.Width)
            x = Math.Max(0, parentForm.ClientSize.Width - calendarPopup.Width);

        calendarPopup.Location = new Point(x, y);
        calendarPopup.Visible = true;
        calendarPopup.BringToFront();
    }

    private void ParentForm_Deactivate(object sender, EventArgs e) => HideCalendar();
    private void ParentForm_LocationOrSizeChanged(object sender, EventArgs e) => HideCalendar();

    private void ParentForm_MouseDown(object sender, MouseEventArgs e)
    {
        var parent = sender as Form;
        if (parent == null) return;

        Point clicked = e.Location;
        Rectangle calRect = calendarPopup.Visible ? calendarPopup.Bounds : Rectangle.Empty;
        Point ctrlTopLeft = parent.PointToClient(this.PointToScreen(Point.Empty));
        Rectangle ctrlRect = new Rectangle(ctrlTopLeft, this.Size);

        if (!calRect.Contains(clicked) && !ctrlRect.Contains(clicked))
            HideCalendar();
    }

    private void HideCalendar()
    {
        if (parentFormCached != null)
        {
            parentFormCached.MouseDown -= ParentForm_MouseDown;
            parentFormCached.LocationChanged -= ParentForm_LocationOrSizeChanged;
            parentFormCached.SizeChanged -= ParentForm_LocationOrSizeChanged;
            parentFormCached.Deactivate -= ParentForm_Deactivate;

            if (parentFormCached.Controls.Contains(calendarPopup))
                parentFormCached.Controls.Remove(calendarPopup);

            parentFormCached = null;
        }

        calendarPopup.Visible = false;
    }

    private void DrawCalendarIcon(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Color baseColor = isIconHover ? IconHoverColor : IconColor;

        // Hover do ícone (círculo atrás)
        if (isIconHover)
        {
            Color fill = Color.FromArgb(30, IconHoverColor.R, IconHoverColor.G, IconHoverColor.B);
            using (SolidBrush bb = new SolidBrush(fill))
            {
                g.FillEllipse(bb, 3, 3, calendarIcon.Width - 6, calendarIcon.Height - 6);
            }
        }

        // Desenha o ícone do calendário
        using (Pen pen = new Pen(baseColor, 2.0f))
        {
            int pad = 6;
            int w = calendarIcon.Width - pad;
            int h = calendarIcon.Height - pad;
            int x = pad / 2;
            int y = pad / 2;

            g.DrawRectangle(pen, x, y + 4, w - 1, h - 6);
            g.DrawLine(pen, x, y + 8, x + w - 1, y + 8);
            g.DrawLine(pen, x + 5, y, x + 5, y + 4);
            g.DrawLine(pen, x + w - 6, y, x + w - 6, y + 4);
        }

        // Linha separadora entre a data e o ícone (desenhada no controle principal OnPaint para melhor alinhamento)
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
                e.Graphics.FillPath(brush, path);

            Color borderToUse = isFocused ? BorderFocusColor : BorderColor;
            using (Pen pen = new Pen(borderToUse, BorderThickness))
                e.Graphics.DrawPath(pen, path);
        }

        // Hover do ícone (fundo da área)
        if (isIconHover)
        {
            Rectangle iconArea = new Rectangle(this.Width - calendarIcon.Width - 16, 0, calendarIcon.Width + 16, this.Height);
            using (SolidBrush b = new SolidBrush(IconHoverAreaColor))
                e.Graphics.FillRectangle(b, iconArea);
        }

        // desenhar linha separadora
        using (Pen sep = new Pen(Color.FromArgb(200,200,200), 1f))
        {
            int sepX = this.Width - calendarIcon.Width - 16; // posição da linha
            int sepTop = 4;
            int sepBottom = this.Height - 4;
            e.Graphics.DrawLine(sep, sepX, sepTop, sepX, sepBottom);
        }

        if (innerTextBox != null)
            innerTextBox.BackColor = isMouseOver ? HoverBackColor : this.BackColor;

        UpdatePlaceholderVisibility();
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

    private void UpdatePlaceholderVisibility()
    {
        if (placeholderLabel == null || innerTextBox == null) return;
        placeholderLabel.Visible = string.IsNullOrEmpty(innerTextBox.Text) && !innerTextBox.Focused;
    }

    private void ValidateAndApplyTextDate()
    {
        if (innerTextBox == null) return;

        string txt = innerTextBox.Text?.Trim();
        if (string.IsNullOrEmpty(txt))
        {
            previousValidDate = null;
            placeholderLabel.Visible = true;
            return;
        }

        DateTime parsed = DateTime.MinValue;
        bool ok = false;
        string[] formats = new[] { "d", "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "M/d/yyyy" };

        foreach (var fmt in formats)
        {
            if (DateTime.TryParseExact(txt, fmt, CultureInfo.CurrentCulture,
                DateTimeStyles.None, out parsed))
            {
                ok = true;
                break;
            }
        }

        if (!ok)
            ok = DateTime.TryParse(txt, CultureInfo.CurrentCulture, DateTimeStyles.None, out parsed);

        if (ok)
        {
           
            SelectedDate = parsed; 
        }
        else
        {
            
            if (previousValidDate.HasValue)
                SelectedDate = previousValidDate.Value; 
            else
                SelectedDate = null;

            FlashBorder(Color.FromArgb(200, 60, 60));
        }
    }

    private async void FlashBorder(Color errorColor)
    {
        var original = this.BorderColor;
        this.BorderColor = errorColor;
        Invalidate();
        await System.Threading.Tasks.Task.Delay(900);
        this.BorderColor = original;
        Invalidate();
    }

    // ----------------- PROPRIEDADES EXPONÍVEIS -----------------

    [Category("Aparência")]
    public Color BorderColor { get; set; }

    [Category("Aparência")]
    public Color BorderFocusColor { get; set; }

    [Category("Aparência")]
    public Color HoverBackColor { get; set; }

    [Category("Aparência")]
    public int BorderRadius { get; set; }

    [Category("Aparência")]
    public int BorderThickness { get; set; }

    [Category("Aparência")]
    public string PlaceholderText
    {
        get => placeholderLabel?.Text ?? "";
        set
        {
            if (placeholderLabel != null) placeholderLabel.Text = value;
            Invalidate();
        }
    }

    [Category("Aparência")]
    public Font PlaceholderFont
    {
        get => placeholderLabel?.Font;
        set
        {
            if (placeholderLabel != null) placeholderLabel.Font = value;
            AdjustLayout();
            Invalidate();
        }
    }

    [Category("Aparência")]
    public Color PlaceholderColor
    {
        get => _placeholderColor;
        set
        {
            _placeholderColor = value;
            if (placeholderLabel != null) placeholderLabel.ForeColor = value;
            Invalidate();
        }
    }

    [Category("Aparência")]
    public DateTime? SelectedDate
    {
        get
        {
            if (innerTextBox != null && DateTime.TryParse(innerTextBox.Text, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime dt))
                return dt;
            return null;
        }
        set
        {
            if (innerTextBox == null) return;

            if (value.HasValue)
            {
                previousValidDate = value;
                // atualiza texto com formato dd/MM/yyyy
                innerTextBox.Text = value.Value.ToString("dd/MM/yyyy");
                placeholderLabel.Visible = false;
                // atualiza o MonthCalendar (se existir)
                if (calendarPopup != null)
                    calendarPopup.SetDate(value.Value);
            }
            else
            {
                previousValidDate = null;
                innerTextBox.Text = "";
                placeholderLabel.Visible = true;
            }

            Invalidate();

            // Dispara ValueChanged para compatibilidade com DateTimePicker
            OnValueChanged(EventArgs.Empty);
        }
    }

    // limites compatíveis com DateTimePicker
    private DateTime _minDate = new DateTime(1753, 1, 1);
    private DateTime _maxDate = new DateTime(9998, 12, 31);

    [Category("Comportamento")]
    public DateTime MinDate
    {
        get => _minDate;
        set
        {
            _minDate = value;
            if (calendarPopup != null) calendarPopup.MinDate = _minDate;
            // se seleção atual estiver abaixo, corrige
            if (SelectedDate.HasValue && SelectedDate.Value < _minDate)
                SelectedDate = _minDate;
        }
    }

    [Category("Comportamento")]
    public DateTime MaxDate
    {
        get => _maxDate;
        set
        {
            _maxDate = value;
            if (calendarPopup != null) calendarPopup.MaxDate = _maxDate;
            // se seleção atual estiver acima, corrige
            if (SelectedDate.HasValue && SelectedDate.Value > _maxDate)
                SelectedDate = _maxDate;
        }
    }


    public override string Text
    {
        get => innerTextBox?.Text ?? "";
        set
        {
            if (innerTextBox != null) innerTextBox.Text = value;
            UpdatePlaceholderVisibility();
            Invalidate();
        }
    }


    /// <summary>
    /// Evento compatível com DateTimePicker.ValueChanged do WinForms.
    /// </summary>
    [Category("Comportamento")]
    public event EventHandler ValueChanged;

    protected virtual void OnValueChanged(EventArgs e)
    {
        ValueChanged?.Invoke(this, e);
    }

    /// <summary>
    /// Propriedade Value compatível com DateTimePicker.Value (facilita troca sem mexer no Form).
    /// Retorna SelectedDate ou DateTime.Today se null (comportamento configurável).
    /// </summary>
    [Browsable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public DateTime Value
    {
        get => SelectedDate ?? DateTime.Today;
        set => SelectedDate = value;
    }



    public override Font Font
    {
        get => base.Font;
        set
        {
            base.Font = value;
            if (innerTextBox != null)
            {
                innerTextBox.Font = value;
                AdjustLayout();
            }
        }
    }

    public void SelectAll()
    {
        innerTextBox?.SelectAll();
    }

    public new void Focus()
    {
        innerTextBox?.Focus();
    }

    // ----------------- PROPRIEDADES DO ÍCONE (NOVAS) -----------------

    [Category("Ícone")]
    public Color IconColor
    {
        get => _iconColor;
        set { _iconColor = value; calendarIcon?.Invalidate(); Invalidate(); }
    }

    [Category("Ícone")]
    public Color IconHoverColor
    {
        get => _iconHoverColor;
        set { _iconHoverColor = value; calendarIcon?.Invalidate(); Invalidate(); }
    }

    [Category("Ícone")]
    public Color IconHoverAreaColor
    {
        get => _iconHoverAreaColor;
        set { _iconHoverAreaColor = value; Invalidate(); }
    }

    // ----------------- FORMATAÇÃO AO DIGITAR -----------------
    private void InnerTextBox_TextChanged(object sender, EventArgs e)
    {
        if (isFormattingText) return;

        try
        {
            isFormattingText = true;

            string original = innerTextBox.Text ?? "";
            int selStart = innerTextBox.SelectionStart;

            // quantidade de dígitos antes do cursor no texto original
            int digitsBeforeCursor = CountDigits(original.Substring(0, Math.Max(0, Math.Min(selStart, original.Length))));

            // extrai apenas dígitos
            string digits = Regex.Replace(original, @"\D", "");
            if (digits.Length > 8) digits = digits.Substring(0, 8); // ddMMyyyy máximo

            string formatted = FormatDigitsToDate(digits);

            // calcula nova posição do cursor com base em dígitosBeforeCursor
            int newPos = MapDigitsToFormattedPosition(digitsBeforeCursor, formatted);

            innerTextBox.Text = formatted;
            innerTextBox.SelectionStart = Math.Max(0, Math.Min(newPos, innerTextBox.Text.Length));
        }
        finally
        {
            isFormattingText = false;
        }
    }

    private static int CountDigits(string s)
    {
        int c = 0;
        foreach (char ch in s) if (char.IsDigit(ch)) c++;
        return c;
    }

    private static string FormatDigitsToDate(string digits)
    {
        if (string.IsNullOrEmpty(digits)) return "";

        if (digits.Length <= 2) return digits;
        if (digits.Length <= 4) return digits.Insert(2, "/");
        // >4
        if (digits.Length <= 8)
        {
            string part1 = digits.Substring(0, 2);
            string part2 = digits.Substring(2, Math.Min(2, digits.Length - 2));
            string part3 = digits.Length > 4 ? digits.Substring(4) : "";
            if (part3.Length > 0)
                return $"{part1}/{part2}/{part3}";
            else
                return $"{part1}/{part2}";
        }
        return digits;
    }

    private static int MapDigitsToFormattedPosition(int digitsBeforeCursor, string formatted)
    {
        if (digitsBeforeCursor <= 2) return digitsBeforeCursor;
        if (digitsBeforeCursor <= 4) return digitsBeforeCursor + 1; // uma barra
        return digitsBeforeCursor + 2; // duas barras
    }
}
