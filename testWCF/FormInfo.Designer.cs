namespace testWCF
{
    partial class FormInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.countTask1 = new System.Windows.Forms.Label();
            this.countTask2 = new System.Windows.Forms.Label();
            this.Ресурсы = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Cpu = new System.Windows.Forms.Label();
            this.Ram = new System.Windows.Forms.Label();
            this.time1 = new System.Windows.Forms.Label();
            this.time2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.Ресурсы.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Вычислительных: ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ресурсоемких: ";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.countTask2);
            this.groupBox1.Controls.Add(this.countTask1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(195, 89);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Количество задач";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // countTask1
            // 
            this.countTask1.AutoSize = true;
            this.countTask1.Location = new System.Drawing.Point(122, 29);
            this.countTask1.Name = "countTask1";
            this.countTask1.Size = new System.Drawing.Size(13, 13);
            this.countTask1.TabIndex = 2;
            this.countTask1.Text = "0";
            // 
            // countTask2
            // 
            this.countTask2.AutoSize = true;
            this.countTask2.Location = new System.Drawing.Point(125, 58);
            this.countTask2.Name = "countTask2";
            this.countTask2.Size = new System.Drawing.Size(13, 13);
            this.countTask2.TabIndex = 3;
            this.countTask2.Text = "0";
            // 
            // Ресурсы
            // 
            this.Ресурсы.Controls.Add(this.Ram);
            this.Ресурсы.Controls.Add(this.Cpu);
            this.Ресурсы.Controls.Add(this.label4);
            this.Ресурсы.Controls.Add(this.label3);
            this.Ресурсы.Location = new System.Drawing.Point(223, 12);
            this.Ресурсы.Name = "Ресурсы";
            this.Ресурсы.Size = new System.Drawing.Size(108, 89);
            this.Ресурсы.TabIndex = 3;
            this.Ресурсы.TabStop = false;
            this.Ресурсы.Text = "Ресурсы";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "CPU: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "RAM: ";
            // 
            // Cpu
            // 
            this.Cpu.AutoSize = true;
            this.Cpu.Location = new System.Drawing.Point(64, 30);
            this.Cpu.Name = "Cpu";
            this.Cpu.Size = new System.Drawing.Size(36, 13);
            this.Cpu.TabIndex = 2;
            this.Cpu.Text = "0.00%";
            // 
            // Ram
            // 
            this.Ram.AutoSize = true;
            this.Ram.Location = new System.Drawing.Point(67, 58);
            this.Ram.Name = "Ram";
            this.Ram.Size = new System.Drawing.Size(32, 13);
            this.Ram.TabIndex = 3;
            this.Ram.Text = "0 MB";
            // 
            // time1
            // 
            this.time1.AutoSize = true;
            this.time1.Location = new System.Drawing.Point(121, 29);
            this.time1.Name = "time1";
            this.time1.Size = new System.Drawing.Size(13, 13);
            this.time1.TabIndex = 2;
            this.time1.Text = "0";
            // 
            // time2
            // 
            this.time2.AutoSize = true;
            this.time2.Location = new System.Drawing.Point(121, 55);
            this.time2.Name = "time2";
            this.time2.Size = new System.Drawing.Size(13, 13);
            this.time2.TabIndex = 3;
            this.time2.Text = "0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.time2);
            this.groupBox2.Controls.Add(this.time1);
            this.groupBox2.Location = new System.Drawing.Point(13, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(171, 85);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Среднее время выполнения";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Вычислительных: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Ресурсоемких: ";
            // 
            // FormInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 209);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Ресурсы);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormInfo";
            this.Text = "Информация о сервере";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Ресурсы.ResumeLayout(false);
            this.Ресурсы.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label countTask2;
        private System.Windows.Forms.Label countTask1;
        private System.Windows.Forms.GroupBox Ресурсы;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Ram;
        private System.Windows.Forms.Label Cpu;
        private System.Windows.Forms.Label time1;
        private System.Windows.Forms.Label time2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}