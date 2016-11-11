namespace MCX_Basic
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.commandWindow = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // commandWindow
            // 
            this.commandWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandWindow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(54)))), ((int)(((byte)(160)))));
            this.commandWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.commandWindow.Location = new System.Drawing.Point(7, 7);
            this.commandWindow.Margin = new System.Windows.Forms.Padding(10);
            this.commandWindow.Multiline = true;
            this.commandWindow.Name = "commandWindow";
            this.commandWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.commandWindow.Size = new System.Drawing.Size(270, 249);
            this.commandWindow.TabIndex = 0;
            this.commandWindow.TabStop = false;
            this.commandWindow.KeyDown += new System.Windows.Forms.KeyEventHandler(this.commandWindow_KeyDown);
            this.commandWindow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.commandWindow_KeyPress);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.commandWindow);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox commandWindow;
    }
}

