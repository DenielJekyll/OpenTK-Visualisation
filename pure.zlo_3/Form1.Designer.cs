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
            this.normalsOptions_groupBox = new System.Windows.Forms.GroupBox();
            this.viewMode_groupBox = new System.Windows.Forms.GroupBox();
            this.ortografia_radioButton = new System.Windows.Forms.RadioButton();
            this.prospect_radioButton = new System.Windows.Forms.RadioButton();
            this.showNormals_checkBox = new System.Windows.Forms.CheckBox();
            this.texturing_checkBox = new System.Windows.Forms.CheckBox();
            this.surfaces_checkBox = new System.Windows.Forms.CheckBox();
            this.smoothing_checkBox = new System.Windows.Forms.CheckBox();
            this.fileManager_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.coordinateGrid_checkBox = new System.Windows.Forms.CheckBox();
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
            this.glV.Name = "glV";
            this.glV.Size = new System.Drawing.Size(531, 529);
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
            this.options_panel.Location = new System.Drawing.Point(-2, -2);
            this.options_panel.Name = "options_panel";
            this.options_panel.Size = new System.Drawing.Size(149, 257);
            this.options_panel.TabIndex = 1;
            // 
            // displayOptions_groupBox
            // 
            this.displayOptions_groupBox.Controls.Add(this.coordinateGrid_checkBox);
            this.displayOptions_groupBox.Controls.Add(this.texturing_checkBox);
            this.displayOptions_groupBox.Controls.Add(this.surfaces_checkBox);
            this.displayOptions_groupBox.Location = new System.Drawing.Point(14, 160);
            this.displayOptions_groupBox.Name = "displayOptions_groupBox";
            this.displayOptions_groupBox.Size = new System.Drawing.Size(121, 78);
            this.displayOptions_groupBox.TabIndex = 2;
            this.displayOptions_groupBox.TabStop = false;
            this.displayOptions_groupBox.Text = "Display options";
            // 
            // normalsOptions_groupBox
            // 
            this.normalsOptions_groupBox.Controls.Add(this.smoothing_checkBox);
            this.normalsOptions_groupBox.Controls.Add(this.showNormals_checkBox);
            this.normalsOptions_groupBox.Location = new System.Drawing.Point(14, 91);
            this.normalsOptions_groupBox.Name = "normalsOptions_groupBox";
            this.normalsOptions_groupBox.Size = new System.Drawing.Size(121, 63);
            this.normalsOptions_groupBox.TabIndex = 3;
            this.normalsOptions_groupBox.TabStop = false;
            this.normalsOptions_groupBox.Text = "Normals options";
            // 
            // viewMode_groupBox
            // 
            this.viewMode_groupBox.Controls.Add(this.ortografia_radioButton);
            this.viewMode_groupBox.Controls.Add(this.prospect_radioButton);
            this.viewMode_groupBox.Location = new System.Drawing.Point(14, 14);
            this.viewMode_groupBox.Name = "viewMode_groupBox";
            this.viewMode_groupBox.Size = new System.Drawing.Size(121, 71);
            this.viewMode_groupBox.TabIndex = 3;
            this.viewMode_groupBox.TabStop = false;
            this.viewMode_groupBox.Text = "View mode";
            // 
            // ortografia_radioButton
            // 
            this.ortografia_radioButton.AutoSize = true;
            this.ortografia_radioButton.Location = new System.Drawing.Point(6, 42);
            this.ortografia_radioButton.Name = "ortografia_radioButton";
            this.ortografia_radioButton.Size = new System.Drawing.Size(71, 17);
            this.ortografia_radioButton.TabIndex = 2;
            this.ortografia_radioButton.TabStop = true;
            this.ortografia_radioButton.Text = "Ortografia";
            this.ortografia_radioButton.UseVisualStyleBackColor = true;
            this.ortografia_radioButton.Click += new System.EventHandler(this.ortografia_radioButton_Click);
            // 
            // prospect_radioButton
            // 
            this.prospect_radioButton.AutoSize = true;
            this.prospect_radioButton.Location = new System.Drawing.Point(6, 19);
            this.prospect_radioButton.Name = "prospect_radioButton";
            this.prospect_radioButton.Size = new System.Drawing.Size(67, 17);
            this.prospect_radioButton.TabIndex = 3;
            this.prospect_radioButton.TabStop = true;
            this.prospect_radioButton.Text = "Prospect";
            this.prospect_radioButton.UseVisualStyleBackColor = true;
            this.prospect_radioButton.Click += new System.EventHandler(this.prospect_radioButton_Click);
            // 
            // showNormals_checkBox
            // 
            this.showNormals_checkBox.AutoSize = true;
            this.showNormals_checkBox.Location = new System.Drawing.Point(6, 19);
            this.showNormals_checkBox.Name = "showNormals_checkBox";
            this.showNormals_checkBox.Size = new System.Drawing.Size(92, 17);
            this.showNormals_checkBox.TabIndex = 2;
            this.showNormals_checkBox.Text = "Show normals";
            this.showNormals_checkBox.UseVisualStyleBackColor = true;
            this.showNormals_checkBox.CheckedChanged += new System.EventHandler(this.showNormals_checkBox_CheckedChanged);
            // 
            // texturing_checkBox
            // 
            this.texturing_checkBox.AutoSize = true;
            this.texturing_checkBox.Location = new System.Drawing.Point(6, 37);
            this.texturing_checkBox.Name = "texturing_checkBox";
            this.texturing_checkBox.Size = new System.Drawing.Size(70, 17);
            this.texturing_checkBox.TabIndex = 3;
            this.texturing_checkBox.Text = "Texturing";
            this.texturing_checkBox.UseVisualStyleBackColor = true;
            this.texturing_checkBox.CheckedChanged += new System.EventHandler(this.texturing_checkBox_CheckedChanged);
            // 
            // surfaces_checkBox
            // 
            this.surfaces_checkBox.AutoSize = true;
            this.surfaces_checkBox.Location = new System.Drawing.Point(6, 19);
            this.surfaces_checkBox.Name = "surfaces_checkBox";
            this.surfaces_checkBox.Size = new System.Drawing.Size(68, 17);
            this.surfaces_checkBox.TabIndex = 4;
            this.surfaces_checkBox.Text = "Surfaces";
            this.surfaces_checkBox.UseVisualStyleBackColor = true;
            this.surfaces_checkBox.CheckedChanged += new System.EventHandler(this.surfaces_checkBox_CheckedChanged);
            // 
            // smoothing_checkBox
            // 
            this.smoothing_checkBox.AutoSize = true;
            this.smoothing_checkBox.Location = new System.Drawing.Point(6, 40);
            this.smoothing_checkBox.Name = "smoothing_checkBox";
            this.smoothing_checkBox.Size = new System.Drawing.Size(76, 17);
            this.smoothing_checkBox.TabIndex = 5;
            this.smoothing_checkBox.Text = "Smoothing";
            this.smoothing_checkBox.UseVisualStyleBackColor = true;
            this.smoothing_checkBox.CheckedChanged += new System.EventHandler(this.smoothing_checkBox_CheckedChanged);
            // 
            // fileManager_openFileDialog
            // 
            this.fileManager_openFileDialog.FileName = "fileManager";
            // 
            // coordinateGrid_checkBox
            // 
            this.coordinateGrid_checkBox.AutoSize = true;
            this.coordinateGrid_checkBox.Location = new System.Drawing.Point(6, 56);
            this.coordinateGrid_checkBox.Name = "coordinateGrid_checkBox";
            this.coordinateGrid_checkBox.Size = new System.Drawing.Size(97, 17);
            this.coordinateGrid_checkBox.TabIndex = 2;
            this.coordinateGrid_checkBox.Text = "Сoordinate grid";
            this.coordinateGrid_checkBox.UseVisualStyleBackColor = true;
            this.coordinateGrid_checkBox.CheckedChanged += new System.EventHandler(this.coordinateGrid_checkBox_CheckedChanged);
            // 
            // Canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(531, 529);
            this.Controls.Add(this.options_panel);
            this.Controls.Add(this.glV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
    }
}

