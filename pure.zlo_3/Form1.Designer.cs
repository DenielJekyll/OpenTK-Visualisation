namespace pure.zlo_3
{
    partial class Canvas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Canvas));
            this.glV = new OpenTK.GLControl();
            this.options_panel = new System.Windows.Forms.Panel();
            this.displayOptions_groupBox = new System.Windows.Forms.GroupBox();
            this.coordinateGrid_checkBox = new System.Windows.Forms.CheckBox();
            this.texturing_checkBox = new System.Windows.Forms.CheckBox();
            this.surfaces_checkBox = new System.Windows.Forms.CheckBox();
            this.normalsOptions_groupBox = new System.Windows.Forms.GroupBox();
            this.smoothing_checkBox = new System.Windows.Forms.CheckBox();
            this.showNormals_checkBox = new System.Windows.Forms.CheckBox();
            this.viewMode_groupBox = new System.Windows.Forms.GroupBox();
            this.ortografia_radioButton = new System.Windows.Forms.RadioButton();
            this.prospect_radioButton = new System.Windows.Forms.RadioButton();
            this.fileManager_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.lightColorPicker = new System.Windows.Forms.ColorDialog();
            this.lightColor_button = new System.Windows.Forms.Button();
            this.options_panel.SuspendLayout();
            this.displayOptions_groupBox.SuspendLayout();
            this.normalsOptions_groupBox.SuspendLayout();
            this.viewMode_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // glV
            // 
            this.glV.AutoSize = true;
            this.glV.BackColor = System.Drawing.Color.Black;
            this.glV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glV.Location = new System.Drawing.Point(0, 0);
            this.glV.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.glV.Name = "glV";
            this.glV.Size = new System.Drawing.Size(796, 814);
            this.glV.TabIndex = 0;
            this.glV.VSync = false;
            this.glV.Load += new System.EventHandler(this.glV_Load);
            this.glV.Paint += new System.Windows.Forms.PaintEventHandler(this.glV_Paint);
            this.glV.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glV_KeyDown);
            this.glV.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glV_MouseDown);
            this.glV.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glV_MouseMove);
            this.glV.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glV_MouseUp);
            this.glV.Resize += new System.EventHandler(this.glV_Resize);
            // 
            // options_panel
            // 
            this.options_panel.Controls.Add(this.displayOptions_groupBox);
            this.options_panel.Controls.Add(this.normalsOptions_groupBox);
            this.options_panel.Controls.Add(this.viewMode_groupBox);
            this.options_panel.Location = new System.Drawing.Point(-3, -3);
            this.options_panel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.options_panel.Name = "options_panel";
            this.options_panel.Size = new System.Drawing.Size(224, 426);
            this.options_panel.TabIndex = 1;
            // 
            // displayOptions_groupBox
            // 
            this.displayOptions_groupBox.Controls.Add(this.lightColor_button);
            this.displayOptions_groupBox.Controls.Add(this.coordinateGrid_checkBox);
            this.displayOptions_groupBox.Controls.Add(this.texturing_checkBox);
            this.displayOptions_groupBox.Controls.Add(this.surfaces_checkBox);
            this.displayOptions_groupBox.Location = new System.Drawing.Point(21, 246);
            this.displayOptions_groupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.displayOptions_groupBox.Name = "displayOptions_groupBox";
            this.displayOptions_groupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.displayOptions_groupBox.Size = new System.Drawing.Size(182, 165);
            this.displayOptions_groupBox.TabIndex = 2;
            this.displayOptions_groupBox.TabStop = false;
            this.displayOptions_groupBox.Text = "Display options";
            // 
            // coordinateGrid_checkBox
            // 
            this.coordinateGrid_checkBox.AutoSize = true;
            this.coordinateGrid_checkBox.Checked = true;
            this.coordinateGrid_checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.coordinateGrid_checkBox.Location = new System.Drawing.Point(9, 86);
            this.coordinateGrid_checkBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.coordinateGrid_checkBox.Name = "coordinateGrid_checkBox";
            this.coordinateGrid_checkBox.Size = new System.Drawing.Size(143, 24);
            this.coordinateGrid_checkBox.TabIndex = 2;
            this.coordinateGrid_checkBox.Text = "Сoordinate grid";
            this.coordinateGrid_checkBox.UseVisualStyleBackColor = true;
            this.coordinateGrid_checkBox.CheckedChanged += new System.EventHandler(this.coordinateGrid_checkBox_CheckedChanged);
            // 
            // texturing_checkBox
            // 
            this.texturing_checkBox.AutoSize = true;
            this.texturing_checkBox.Location = new System.Drawing.Point(9, 57);
            this.texturing_checkBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.texturing_checkBox.Name = "texturing_checkBox";
            this.texturing_checkBox.Size = new System.Drawing.Size(100, 24);
            this.texturing_checkBox.TabIndex = 3;
            this.texturing_checkBox.Text = "Texturing";
            this.texturing_checkBox.UseVisualStyleBackColor = true;
            this.texturing_checkBox.CheckedChanged += new System.EventHandler(this.texturing_checkBox_CheckedChanged);
            // 
            // surfaces_checkBox
            // 
            this.surfaces_checkBox.AutoSize = true;
            this.surfaces_checkBox.Location = new System.Drawing.Point(9, 29);
            this.surfaces_checkBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.surfaces_checkBox.Name = "surfaces_checkBox";
            this.surfaces_checkBox.Size = new System.Drawing.Size(99, 24);
            this.surfaces_checkBox.TabIndex = 4;
            this.surfaces_checkBox.Text = "Surfaces";
            this.surfaces_checkBox.UseVisualStyleBackColor = true;
            this.surfaces_checkBox.CheckedChanged += new System.EventHandler(this.surfaces_checkBox_CheckedChanged);
            // 
            // normalsOptions_groupBox
            // 
            this.normalsOptions_groupBox.Controls.Add(this.smoothing_checkBox);
            this.normalsOptions_groupBox.Controls.Add(this.showNormals_checkBox);
            this.normalsOptions_groupBox.Location = new System.Drawing.Point(21, 140);
            this.normalsOptions_groupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.normalsOptions_groupBox.Name = "normalsOptions_groupBox";
            this.normalsOptions_groupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.normalsOptions_groupBox.Size = new System.Drawing.Size(182, 97);
            this.normalsOptions_groupBox.TabIndex = 3;
            this.normalsOptions_groupBox.TabStop = false;
            this.normalsOptions_groupBox.Text = "Normals options";
            // 
            // smoothing_checkBox
            // 
            this.smoothing_checkBox.AutoSize = true;
            this.smoothing_checkBox.Location = new System.Drawing.Point(9, 62);
            this.smoothing_checkBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.smoothing_checkBox.Name = "smoothing_checkBox";
            this.smoothing_checkBox.Size = new System.Drawing.Size(112, 24);
            this.smoothing_checkBox.TabIndex = 5;
            this.smoothing_checkBox.Text = "Smoothing";
            this.smoothing_checkBox.UseVisualStyleBackColor = true;
            this.smoothing_checkBox.CheckedChanged += new System.EventHandler(this.smoothing_checkBox_CheckedChanged);
            // 
            // showNormals_checkBox
            // 
            this.showNormals_checkBox.AutoSize = true;
            this.showNormals_checkBox.Location = new System.Drawing.Point(9, 29);
            this.showNormals_checkBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.showNormals_checkBox.Name = "showNormals_checkBox";
            this.showNormals_checkBox.Size = new System.Drawing.Size(135, 24);
            this.showNormals_checkBox.TabIndex = 2;
            this.showNormals_checkBox.Text = "Show normals";
            this.showNormals_checkBox.UseVisualStyleBackColor = true;
            this.showNormals_checkBox.CheckedChanged += new System.EventHandler(this.showNormals_checkBox_CheckedChanged);
            // 
            // viewMode_groupBox
            // 
            this.viewMode_groupBox.Controls.Add(this.ortografia_radioButton);
            this.viewMode_groupBox.Controls.Add(this.prospect_radioButton);
            this.viewMode_groupBox.Location = new System.Drawing.Point(21, 22);
            this.viewMode_groupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.viewMode_groupBox.Name = "viewMode_groupBox";
            this.viewMode_groupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.viewMode_groupBox.Size = new System.Drawing.Size(182, 109);
            this.viewMode_groupBox.TabIndex = 3;
            this.viewMode_groupBox.TabStop = false;
            this.viewMode_groupBox.Text = "View mode";
            // 
            // ortografia_radioButton
            // 
            this.ortografia_radioButton.AutoSize = true;
            this.ortografia_radioButton.Location = new System.Drawing.Point(9, 65);
            this.ortografia_radioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ortografia_radioButton.Name = "ortografia_radioButton";
            this.ortografia_radioButton.Size = new System.Drawing.Size(105, 24);
            this.ortografia_radioButton.TabIndex = 2;
            this.ortografia_radioButton.Text = "Ortografia";
            this.ortografia_radioButton.UseVisualStyleBackColor = true;
            this.ortografia_radioButton.Click += new System.EventHandler(this.ortografia_radioButton_Click);
            // 
            // prospect_radioButton
            // 
            this.prospect_radioButton.AutoSize = true;
            this.prospect_radioButton.Checked = true;
            this.prospect_radioButton.Location = new System.Drawing.Point(9, 29);
            this.prospect_radioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.prospect_radioButton.Name = "prospect_radioButton";
            this.prospect_radioButton.Size = new System.Drawing.Size(97, 24);
            this.prospect_radioButton.TabIndex = 3;
            this.prospect_radioButton.TabStop = true;
            this.prospect_radioButton.Text = "Prospect";
            this.prospect_radioButton.UseVisualStyleBackColor = true;
            this.prospect_radioButton.Click += new System.EventHandler(this.prospect_radioButton_Click);
            // 
            // fileManager_openFileDialog
            // 
            this.fileManager_openFileDialog.FileName = "fileManager";
            // 
            // lightColor_button
            // 
            this.lightColor_button.Location = new System.Drawing.Point(9, 121);
            this.lightColor_button.Name = "lightColor_button";
            this.lightColor_button.Size = new System.Drawing.Size(166, 36);
            this.lightColor_button.TabIndex = 5;
            this.lightColor_button.Text = "Light Color";
            this.lightColor_button.UseVisualStyleBackColor = true;
            this.lightColor_button.Click += new System.EventHandler(this.lightColor_button_Click);
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(796, 814);
            this.Controls.Add(this.options_panel);
            this.Controls.Add(this.glV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Canvas";
            this.Text = "pure.zlo_3";
            this.options_panel.ResumeLayout(false);
            this.displayOptions_groupBox.ResumeLayout(false);
            this.displayOptions_groupBox.PerformLayout();
            this.normalsOptions_groupBox.ResumeLayout(false);
            this.normalsOptions_groupBox.PerformLayout();
            this.viewMode_groupBox.ResumeLayout(false);
            this.viewMode_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glV;
        private System.Windows.Forms.Panel options_panel;
        private System.Windows.Forms.GroupBox displayOptions_groupBox;
        private System.Windows.Forms.CheckBox texturing_checkBox;
        private System.Windows.Forms.CheckBox surfaces_checkBox;
        private System.Windows.Forms.GroupBox normalsOptions_groupBox;
        private System.Windows.Forms.CheckBox smoothing_checkBox;
        private System.Windows.Forms.CheckBox showNormals_checkBox;
        private System.Windows.Forms.GroupBox viewMode_groupBox;
        private System.Windows.Forms.RadioButton ortografia_radioButton;
        private System.Windows.Forms.RadioButton prospect_radioButton;
        private System.Windows.Forms.OpenFileDialog fileManager_openFileDialog;
        private System.Windows.Forms.CheckBox coordinateGrid_checkBox;
        private System.Windows.Forms.Button lightColor_button;
        private System.Windows.Forms.ColorDialog lightColorPicker;
    }
}

