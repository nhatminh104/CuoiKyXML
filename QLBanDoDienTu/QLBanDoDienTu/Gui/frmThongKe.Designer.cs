namespace QLBanDoDienTu.Gui
{
    partial class frmThongKe
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
            this.btnXuatFile = new System.Windows.Forms.Button();
            this.btnXemWeb = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboLoai = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(67, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "THỐNG KÊ HỆ THỐNG";
            // 
            // btnXuatFile
            // 
            this.btnXuatFile.BackColor = System.Drawing.Color.LightBlue;
            this.btnXuatFile.Location = new System.Drawing.Point(80, 129);
            this.btnXuatFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnXuatFile.Name = "btnXuatFile";
            this.btnXuatFile.Size = new System.Drawing.Size(98, 28);
            this.btnXuatFile.TabIndex = 13;
            this.btnXuatFile.Text = "Lưu file";
            this.btnXuatFile.UseVisualStyleBackColor = false;
            // 
            // btnXemWeb
            // 
            this.btnXemWeb.BackColor = System.Drawing.Color.LightCoral;
            this.btnXemWeb.Location = new System.Drawing.Point(202, 129);
            this.btnXemWeb.Margin = new System.Windows.Forms.Padding(2);
            this.btnXemWeb.Name = "btnXemWeb";
            this.btnXemWeb.Size = new System.Drawing.Size(98, 28);
            this.btnXemWeb.TabIndex = 14;
            this.btnXemWeb.Text = "Xem trên web";
            this.btnXemWeb.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 92);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Chọn loại thống kê: ";
            // 
            // comboLoai
            // 
            this.comboLoai.FormattingEnabled = true;
            this.comboLoai.Location = new System.Drawing.Point(193, 89);
            this.comboLoai.Margin = new System.Windows.Forms.Padding(2);
            this.comboLoai.Name = "comboLoai";
            this.comboLoai.Size = new System.Drawing.Size(92, 21);
            this.comboLoai.TabIndex = 16;
            this.comboLoai.SelectedIndexChanged += new System.EventHandler(this.comboLoai_SelectedIndexChanged);
            // 
            // frmThongKe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::QLBanDoDienTu.Properties.Resources.Cac_loai_bieu_do_duong_thuong_gap_cach_ve_bieu_do_duong;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(373, 256);
            this.Controls.Add(this.comboLoai);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnXemWeb);
            this.Controls.Add(this.btnXuatFile);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmThongKe";
            this.Text = "frmThongKe";
            this.Load += new System.EventHandler(this.frmThongKe_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnXuatFile;
        private System.Windows.Forms.Button btnXemWeb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboLoai;
    }
}