namespace Loan_C
{
    partial class Form1
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
            this.btnBank = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbank = new System.Windows.Forms.TextBox();
            this.btnupload = new System.Windows.Forms.Button();
            this.dgv_LNDISBH = new System.Windows.Forms.DataGridView();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGo = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblValPassCount = new System.Windows.Forms.Label();
            this.lblValFailCount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblslotcount = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblTotalrowcount = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblsuccess_c = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDisburs_mess = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblfilter_c = new System.Windows.Forms.Label();
            this.lblSuccess_cc = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblCollTotal = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmb_col_to = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmb_col_from = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnGoC = new System.Windows.Forms.Button();
            this.lblCol_messege = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Deposit = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.lblDep_messege = new System.Windows.Forms.Label();
            this.btnUploaddeposit = new System.Windows.Forms.Button();
            this.btnDeposit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dtp_deposit = new System.Windows.Forms.DateTimePicker();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lblDepTotal = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.dgw_deposit = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_LNDISBH)).BeginInit();
            this.lblsuccess_c.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.Deposit.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgw_deposit)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBank
            // 
            this.btnBank.Location = new System.Drawing.Point(241, 8);
            this.btnBank.Name = "btnBank";
            this.btnBank.Size = new System.Drawing.Size(44, 23);
            this.btnBank.TabIndex = 0;
            this.btnBank.Text = "Bank";
            this.btnBank.UseVisualStyleBackColor = true;
            this.btnBank.Click += new System.EventHandler(this.btnBank_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Bank Ac";
            // 
            // txtbank
            // 
            this.txtbank.Location = new System.Drawing.Point(68, 9);
            this.txtbank.Name = "txtbank";
            this.txtbank.Size = new System.Drawing.Size(176, 20);
            this.txtbank.TabIndex = 2;
            // 
            // btnupload
            // 
            this.btnupload.Location = new System.Drawing.Point(604, 6);
            this.btnupload.Name = "btnupload";
            this.btnupload.Size = new System.Drawing.Size(75, 23);
            this.btnupload.TabIndex = 3;
            this.btnupload.Text = "Upload";
            this.btnupload.UseVisualStyleBackColor = true;
            this.btnupload.Click += new System.EventHandler(this.btnupload_Click);
            // 
            // dgv_LNDISBH
            // 
            this.dgv_LNDISBH.AllowUserToAddRows = false;
            this.dgv_LNDISBH.AllowUserToDeleteRows = false;
            this.dgv_LNDISBH.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_LNDISBH.Location = new System.Drawing.Point(0, 41);
            this.dgv_LNDISBH.Name = "dgv_LNDISBH";
            this.dgv_LNDISBH.ReadOnly = true;
            this.dgv_LNDISBH.Size = new System.Drawing.Size(1008, 384);
            this.dgv_LNDISBH.TabIndex = 4;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(913, 3);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(92, 20);
            this.dateTimePicker1.TabIndex = 8;
            this.dateTimePicker1.Value = new System.DateTime(2021, 2, 6, 0, 0, 0, 0);
            this.dateTimePicker1.Visible = false;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(412, 9);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(96, 20);
            this.dateTimePicker2.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(291, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Disbursement Date:";
            // 
            // txtGo
            // 
            this.txtGo.Location = new System.Drawing.Point(520, 6);
            this.txtGo.Name = "txtGo";
            this.txtGo.Size = new System.Drawing.Size(75, 23);
            this.txtGo.TabIndex = 12;
            this.txtGo.Text = "GO";
            this.txtGo.UseVisualStyleBackColor = true;
            this.txtGo.Click += new System.EventHandler(this.txtGo_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(520, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Validation Passed Count:";
            // 
            // lblValPassCount
            // 
            this.lblValPassCount.AutoSize = true;
            this.lblValPassCount.Location = new System.Drawing.Point(668, 11);
            this.lblValPassCount.Name = "lblValPassCount";
            this.lblValPassCount.Size = new System.Drawing.Size(13, 13);
            this.lblValPassCount.TabIndex = 14;
            this.lblValPassCount.Text = "0";
            // 
            // lblValFailCount
            // 
            this.lblValFailCount.AutoSize = true;
            this.lblValFailCount.Location = new System.Drawing.Point(465, 11);
            this.lblValFailCount.Name = "lblValFailCount";
            this.lblValFailCount.Size = new System.Drawing.Size(13, 13);
            this.lblValFailCount.TabIndex = 16;
            this.lblValFailCount.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(318, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = " Validation Failed Count:";
            // 
            // lblslotcount
            // 
            this.lblslotcount.AutoSize = true;
            this.lblslotcount.Location = new System.Drawing.Point(272, 11);
            this.lblslotcount.Name = "lblslotcount";
            this.lblslotcount.Size = new System.Drawing.Size(13, 13);
            this.lblslotcount.TabIndex = 20;
            this.lblslotcount.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(169, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(104, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Disbursed Count:";
            // 
            // lblTotalrowcount
            // 
            this.lblTotalrowcount.AutoSize = true;
            this.lblTotalrowcount.Location = new System.Drawing.Point(109, 11);
            this.lblTotalrowcount.Name = "lblTotalrowcount";
            this.lblTotalrowcount.Size = new System.Drawing.Size(13, 13);
            this.lblTotalrowcount.TabIndex = 22;
            this.lblTotalrowcount.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(4, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(106, 13);
            this.label13.TabIndex = 21;
            this.label13.Text = "Total Row Count:";
            // 
            // lblsuccess_c
            // 
            this.lblsuccess_c.Controls.Add(this.tabPage1);
            this.lblsuccess_c.Controls.Add(this.tabPage2);
            this.lblsuccess_c.Controls.Add(this.Deposit);
            this.lblsuccess_c.Location = new System.Drawing.Point(3, 8);
            this.lblsuccess_c.Name = "lblsuccess_c";
            this.lblsuccess_c.SelectedIndex = 0;
            this.lblsuccess_c.Size = new System.Drawing.Size(1019, 487);
            this.lblsuccess_c.TabIndex = 23;
            this.lblsuccess_c.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.dgv_LNDISBH);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1011, 461);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Disbursement";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.lblValPassCount);
            this.panel2.Controls.Add(this.lblTotalrowcount);
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.lblValFailCount);
            this.panel2.Controls.Add(this.lblslotcount);
            this.panel2.Location = new System.Drawing.Point(1, 428);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 35);
            this.panel2.TabIndex = 24;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Controls.Add(this.lblDisburs_mess);
            this.panel1.Controls.Add(this.btnupload);
            this.panel1.Controls.Add(this.txtGo);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnBank);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtbank);
            this.panel1.Location = new System.Drawing.Point(1, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 35);
            this.panel1.TabIndex = 23;
            // 
            // lblDisburs_mess
            // 
            this.lblDisburs_mess.AutoSize = true;
            this.lblDisburs_mess.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.lblDisburs_mess.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblDisburs_mess.Location = new System.Drawing.Point(701, 11);
            this.lblDisburs_mess.Name = "lblDisburs_mess";
            this.lblDisburs_mess.Size = new System.Drawing.Size(0, 16);
            this.lblDisburs_mess.TabIndex = 13;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Controls.Add(this.panel3);
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1011, 461);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Collection";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel4.Controls.Add(this.lblfilter_c);
            this.panel4.Controls.Add(this.lblSuccess_cc);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.lblCollTotal);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Location = new System.Drawing.Point(1, 427);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1008, 35);
            this.panel4.TabIndex = 25;
            // 
            // lblfilter_c
            // 
            this.lblfilter_c.AutoSize = true;
            this.lblfilter_c.Location = new System.Drawing.Point(455, 12);
            this.lblfilter_c.Name = "lblfilter_c";
            this.lblfilter_c.Size = new System.Drawing.Size(13, 13);
            this.lblfilter_c.TabIndex = 29;
            this.lblfilter_c.Text = "0";
            // 
            // lblSuccess_cc
            // 
            this.lblSuccess_cc.AutoSize = true;
            this.lblSuccess_cc.Location = new System.Drawing.Point(281, 13);
            this.lblSuccess_cc.Name = "lblSuccess_cc";
            this.lblSuccess_cc.Size = new System.Drawing.Size(13, 13);
            this.lblSuccess_cc.TabIndex = 28;
            this.lblSuccess_cc.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(366, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 13);
            this.label10.TabIndex = 27;
            this.label10.Text = "Filter Count:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(178, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Validated Count:";
            // 
            // lblCollTotal
            // 
            this.lblCollTotal.AutoSize = true;
            this.lblCollTotal.Location = new System.Drawing.Point(128, 13);
            this.lblCollTotal.Name = "lblCollTotal";
            this.lblCollTotal.Size = new System.Drawing.Size(13, 13);
            this.lblCollTotal.TabIndex = 24;
            this.lblCollTotal.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(18, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "Total Row Count:";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel3.Controls.Add(this.cmb_col_to);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.cmb_col_from);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.btnGoC);
            this.panel3.Controls.Add(this.lblCol_messege);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.dateTimePicker3);
            this.panel3.Location = new System.Drawing.Point(1, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 35);
            this.panel3.TabIndex = 24;
            // 
            // cmb_col_to
            // 
            this.cmb_col_to.FormattingEnabled = true;
            this.cmb_col_to.Location = new System.Drawing.Point(463, 4);
            this.cmb_col_to.Name = "cmb_col_to";
            this.cmb_col_to.Size = new System.Drawing.Size(104, 21);
            this.cmb_col_to.TabIndex = 30;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(435, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(22, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "To";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(267, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "From Branch:";
            // 
            // cmb_col_from
            // 
            this.cmb_col_from.FormattingEnabled = true;
            this.cmb_col_from.Location = new System.Drawing.Point(349, 4);
            this.cmb_col_from.Name = "cmb_col_from";
            this.cmb_col_from.Size = new System.Drawing.Size(84, 21);
            this.cmb_col_from.TabIndex = 26;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(570, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Upload";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnGoC
            // 
            this.btnGoC.Location = new System.Drawing.Point(180, 5);
            this.btnGoC.Name = "btnGoC";
            this.btnGoC.Size = new System.Drawing.Size(83, 23);
            this.btnGoC.TabIndex = 17;
            this.btnGoC.Text = "GO";
            this.btnGoC.UseVisualStyleBackColor = true;
            this.btnGoC.Click += new System.EventHandler(this.btnGoC_Click);
            // 
            // lblCol_messege
            // 
            this.lblCol_messege.AutoSize = true;
            this.lblCol_messege.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCol_messege.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblCol_messege.Location = new System.Drawing.Point(694, 10);
            this.lblCol_messege.Name = "lblCol_messege";
            this.lblCol_messege.Size = new System.Drawing.Size(0, 16);
            this.lblCol_messege.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "From Date:";
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker3.Location = new System.Drawing.Point(75, 7);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(97, 20);
            this.dateTimePicker3.TabIndex = 15;
            this.dateTimePicker3.Value = new System.DateTime(2021, 3, 2, 15, 11, 28, 0);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1008, 384);
            this.dataGridView1.TabIndex = 14;
            // 
            // Deposit
            // 
            this.Deposit.Controls.Add(this.panel6);
            this.Deposit.Controls.Add(this.panel5);
            this.Deposit.Controls.Add(this.dgw_deposit);
            this.Deposit.Location = new System.Drawing.Point(4, 22);
            this.Deposit.Name = "Deposit";
            this.Deposit.Size = new System.Drawing.Size(1011, 461);
            this.Deposit.TabIndex = 2;
            this.Deposit.Text = "Deposit";
            this.Deposit.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel6.Controls.Add(this.lblDep_messege);
            this.panel6.Controls.Add(this.btnUploaddeposit);
            this.panel6.Controls.Add(this.btnDeposit);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.dtp_deposit);
            this.panel6.Location = new System.Drawing.Point(1, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1008, 35);
            this.panel6.TabIndex = 27;
            // 
            // lblDep_messege
            // 
            this.lblDep_messege.AutoSize = true;
            this.lblDep_messege.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDep_messege.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblDep_messege.Location = new System.Drawing.Point(455, 10);
            this.lblDep_messege.Name = "lblDep_messege";
            this.lblDep_messege.Size = new System.Drawing.Size(0, 16);
            this.lblDep_messege.TabIndex = 25;
            // 
            // btnUploaddeposit
            // 
            this.btnUploaddeposit.Location = new System.Drawing.Point(273, 7);
            this.btnUploaddeposit.Name = "btnUploaddeposit";
            this.btnUploaddeposit.Size = new System.Drawing.Size(75, 23);
            this.btnUploaddeposit.TabIndex = 24;
            this.btnUploaddeposit.Text = "Upload";
            this.btnUploaddeposit.UseVisualStyleBackColor = true;
            this.btnUploaddeposit.Click += new System.EventHandler(this.btnUploaddeposit_Click);
            // 
            // btnDeposit
            // 
            this.btnDeposit.Location = new System.Drawing.Point(181, 8);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.Size = new System.Drawing.Size(87, 23);
            this.btnDeposit.TabIndex = 23;
            this.btnDeposit.Text = "GO";
            this.btnDeposit.UseVisualStyleBackColor = true;
            this.btnDeposit.Click += new System.EventHandler(this.btnDeposit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-1, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "From Date:";
            // 
            // dtp_deposit
            // 
            this.dtp_deposit.CustomFormat = "yyyy-MM-dd";
            this.dtp_deposit.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_deposit.Location = new System.Drawing.Point(71, 8);
            this.dtp_deposit.Name = "dtp_deposit";
            this.dtp_deposit.Size = new System.Drawing.Size(93, 20);
            this.dtp_deposit.TabIndex = 21;
            this.dtp_deposit.Value = new System.DateTime(2021, 3, 2, 15, 11, 28, 0);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel5.Controls.Add(this.lblDepTotal);
            this.panel5.Controls.Add(this.label18);
            this.panel5.Location = new System.Drawing.Point(1, 426);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1008, 35);
            this.panel5.TabIndex = 26;
            // 
            // lblDepTotal
            // 
            this.lblDepTotal.AutoSize = true;
            this.lblDepTotal.Location = new System.Drawing.Point(122, 12);
            this.lblDepTotal.Name = "lblDepTotal";
            this.lblDepTotal.Size = new System.Drawing.Size(13, 13);
            this.lblDepTotal.TabIndex = 31;
            this.lblDepTotal.Text = "0";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(12, 12);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(106, 13);
            this.label18.TabIndex = 30;
            this.label18.Text = "Total Row Count:";
            // 
            // dgw_deposit
            // 
            this.dgw_deposit.AllowUserToAddRows = false;
            this.dgw_deposit.AllowUserToDeleteRows = false;
            this.dgw_deposit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgw_deposit.Location = new System.Drawing.Point(0, 41);
            this.dgw_deposit.Name = "dgw_deposit";
            this.dgw_deposit.ReadOnly = true;
            this.dgw_deposit.Size = new System.Drawing.Size(1008, 384);
            this.dgw_deposit.TabIndex = 20;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 496);
            this.Controls.Add(this.lblsuccess_c);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Capital  Trust Limited";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_LNDISBH)).EndInit();
            this.lblsuccess_c.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.Deposit.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgw_deposit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBank;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtbank;
        private System.Windows.Forms.Button btnupload;
        private System.Windows.Forms.DataGridView dgv_LNDISBH;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button txtGo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblValPassCount;
        private System.Windows.Forms.Label lblValFailCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblslotcount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblTotalrowcount;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabControl lblsuccess_c;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnGoC;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage Deposit;
        private System.Windows.Forms.Button btnUploaddeposit;
        private System.Windows.Forms.DataGridView dgw_deposit;
        private System.Windows.Forms.DateTimePicker dtp_deposit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDeposit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lblCollTotal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblCol_messege;
        private System.Windows.Forms.Label lblfilter_c;
        private System.Windows.Forms.Label lblSuccess_cc;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblDepTotal;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblDep_messege;
        private System.Windows.Forms.Label lblDisburs_mess;
        private System.Windows.Forms.ComboBox cmb_col_from;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmb_col_to;
    }
}

