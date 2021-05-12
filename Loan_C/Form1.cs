using AccpacCOMAPI;
using AccpacFinder;
using CTL_MSTR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Loan_C
{
    public partial class Form1 : Form
    {
        protected internal string SERVERNAME = null;
        protected internal string USERNAME = null;
        protected internal string PASSWORD = null;
        protected internal string SAGEDB = null;
        protected internal string SAA = null;
        protected internal string SAPSS = null;
        protected internal string THSERVERMSTR = null;
        protected internal string THSERVERDETS = null;
        protected internal string THSERVERCOLL = null;
        protected internal string THSERVERDEP = null;
        private AccpacSession AccSession;
        private AccpacDBLink AccDBlink;
        string BankEntryNumber;
        LogGenerator ojbLog;
        int slotSize;
        int slot = 200;
        dynamic person = new ExpandoObject();
        string borrowerName;
        String DisSageAccId;
        String strLoanID, hdSeqNo, gstStateCode, placeSupcode, ifscCode, benBankName, benBankAC, benBranchName, benName;
        Double Def_LNDes;
        String NetAmount;
        decimal TotalAmt_c;
        String sage_accid_c;
        ent_GLJBtch Jbatch_c;
        DataTable tbdisTocmb;
        string SageACC_Dis_List;
        List<ent_GLHeader> lstHeader_c;
        List<ent_GLJBtch> JList;
        List<string> update_JList;
        DataTable tbGRD_deposit;
        DataSet tbGRD_col;
        string strS_var;
        string str_batchno;
        public Dictionary<string, string> oldacct;
        public Dictionary<string, string> errList;       
        string connectionstring;
        string logfilename;
        ListBox mstrlist;
        String Disb_LogFile;
        String Disb_LogFile_failed;
        String Coll_LogFile;
        String Coll_LogFile_failed;
        String ColDiposit_LogFile;
        String ColDiposit_LogFile_failed;
        public Form1()
        {
            InitializeComponent();
            ojbLog = new LogGenerator();
            CredentialsXml();
            connectionstring = "Data Source=" + SERVERNAME + "; Initial Catalog=" + SAGEDB + "; User ID=" + SAA + "; Password=" + SAPSS + ";";
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(@"LoanTypeList.xml");
            cmbBx_loanType.DisplayMember = "LoanType";
            cmbBx_loanType.ValueMember = "LoanType";
            cmbBx_loanType.DataSource = dataSet.Tables[0];
        }        

        #region Disbursment --------------------------------------------
        private void btnBank_Click(object sender, EventArgs e)
        {
            btnfindinv();
        }
        private void btnfindinv()
        {
            AccSession = new AccpacSession();
            AccSession.Init("", "AS", "AS3001", "65");
            AccSession.Open(USERNAME, PASSWORD, SAGEDB, DateTime.Today, 0, "");
            AccDBlink = AccSession.OpenDBLink(tagDBLinkTypeEnum.DBLINK_COMPANY, tagDBLinkFlagsEnum.DBLINK_FLG_READWRITE);// //OpenDBLink(DBLinkType.Company, DBLinkFlags.ReadWrite,"");
            ViewFinder afinder = new ViewFinder();
            int[] DispArr = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };  // the array of field IDs that will be displayed in the finder’s columns.
            int[] SearchArr = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };  //the array of field IDs that will be used to search in the finder records.
                                                                              // int[] returnval = new int[1] { 1 };
            afinder.Session = AccDBlink.Session;
            afinder.AutoTabAway = true;
            afinder.ViewID = "BK0001";
            afinder.ReturnFieldIDs = 1;
            afinder.DisplayFieldIDs = DispArr;
            afinder.SearchFieldIDs = SearchArr;
            afinder.Finder();
            if (afinder.ReturnFieldValues == null)
            {
                txtbank.Text = "";
            }
            else
            {
                txtbank.Text = afinder.ReturnFieldValues;

            }
            AccSession.Close();
        }
        private void txtGo_Click(object sender, EventArgs e)
        {

            DisSageAccId = "";
            lblTotalrowcount.Text = "";
            lblslotcount.Text = "";
            lblValFailCount.Text = "";
            lblValPassCount.Text = "";
            lblDisburs_mess.Text = "Please wait.......";
            getList();
            lblDisburs_mess.Text = "";
        }
        public void getList()
        {
            DataTable tbGRD;
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlCommand cmd;
            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
            conn.Open();
            string Querystring = "SELECT * FROM " + THSERVERMSTR + " h where h.loantype='"+cmbBx_loanType.Text+"' and h.sage_gL_BATCHno is null and h.sage_gl_bank_entryno is null  and  Convert(date,h.disbdate,100) between '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "'";
            cmd = new System.Data.SqlClient.SqlCommand(Querystring, conn);
            cmd.CommandTimeout = 180;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
            {
                var objerrList = (IDictionary<string, object>)person;
                cmd.Connection = conn;
                sda.SelectCommand = cmd;
                using (tbGRD = new DataTable())
                {
                    sda.Fill(tbGRD);
                    if (tbGRD.Rows.Count > 0)
                    {
                        slotSize = (tbGRD.Rows.Count) / slot;
                        int md = tbGRD.Rows.Count % slot;
                        if (md > 0)
                            slotSize = slotSize + 1;

                        lblTotalrowcount.Text = tbGRD.Rows.Count.ToString();


                        dgv_LNDISBH.DataSource = tbGRD;
                        dgv_LNDISBH.AutoGenerateColumns = false;
                    }
                    else {
                        dgv_LNDISBH.DataSource = tbGRD;
                        MessageBox.Show("Data not found!"); }
                }
            }
        }
        private void btnupload_Click(object sender, EventArgs e)
        {
            DisSageAccId = "";
            logfilename = System.DateTime.Now.ToString();
            logfilename = logfilename.Replace(" ", "-");
            logfilename = logfilename.Replace(":", "");
            logfilename = logfilename.Replace("/", "-");
           // Disb_LogFile = @"Logs/Log- " + logfilename + ".txt";
            //Disb_LogFile = @"Logs/Log- " + logfilename + ".txt";
            Disb_LogFile_failed = "DIS-FAILED-" + logfilename;
            Disb_LogFile = @"Logs/Log- " + Disb_LogFile_failed + ".txt";
            ojbLog.WriteLog(logfilename,"---------------Start Date Time " + System.DateTime.Now.ToString() + "---------------");
            btnupload.Enabled = false;
            GLJAccountController();
            btnupload.Enabled = true;
            ojbLog.WriteLog(logfilename,"---------------End Date Time " + System.DateTime.Now.ToString() + "---------------");
            lblDisburs_mess.Text = "";
        }
        public void GLJAccountController()
        {
            lblDisburs_mess.Text = "Please wait, checking validations.......";
            int strcount = 0;
            int FailCount = 0;

            int TOCount = 0;
            int FromCount = 0;
            // for (int i = 1; i <= slotSize; i++)//
            //{               
            try
            {
                Boolean valid_return = false;
                string Loan_Dis_List00 = "";
                string Loan_Dis_List = "";
                Boolean taxType_entry;
                Double taxbase;
                Double igstamt;
                Double cgstamt;
                Double sgstamt;
                string strbc_id;
                string strbc_name;

                string taxgroupcode;
                string taxliabilityaccount = "";
                dynamic GL = new ExpandoObject();
                var obj = (IDictionary<string, object>)GL;
                string dloanid = "";
                string sageloanacct;
                DataTable dtgjbatch;
                ent_GLHeader header;

                ent_GLJBtch Jbatch = new ent_GLJBtch();
                List<ent_GLHeader> lstHeader;
                List<ent_GLDetail> lstDet;

                //''''''''''''''''''''''''''''''
                DataTable lndisbrh_tbl;
                string branchid;
                System.Data.SqlClient.SqlConnection conn;
                System.Data.SqlClient.SqlCommand cmd;
                conn = new System.Data.SqlClient.SqlConnection(connectionstring);
                conn.Open();
                string Querystring = "SELECT  *,ROW_NUMBER() OVER (ORDER BY id) as RN FROM " + THSERVERMSTR + " h where h.loantype='" + cmbBx_loanType.Text + "' and h.sage_gL_BATCHno is null and h.sage_gl_bank_entryno is null  and Convert(date,h.disbdate,100) between '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "'";
                //  string Querystring = "SELECT *,ROW_NUMBER() OVER (ORDER BY id) as RN FROM " + THSERVERMSTR + " h where loanid=740597";

                cmd = new System.Data.SqlClient.SqlCommand(Querystring, conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                {
                    var objerrList = (IDictionary<string, object>)person;
                    cmd.Connection = conn;
                    sda.SelectCommand = cmd;
                    using (lndisbrh_tbl = new DataTable())
                    {
                        sda.Fill(lndisbrh_tbl);
                        //ojbLog.WriteLog(logfilename,"Total header row count = " + lndisbrh_tbl.Rows.Count);
                        ojbLog.WriteLog(logfilename,"Total header row count = " + lndisbrh_tbl.Rows.Count);
                        //if(i>1)
                        //{
                        //    TOCount =FromCount;
                        //}
                        //FromCount = FromCount + slot;
                        // DataTable tb_rows = lndisbrh_tbl.Select("RN>=" + TOCount + "  AND  RN<=" + FromCount).CopyToDataTable();
                        if (lndisbrh_tbl.Rows.Count > 0)
                        {
                            int disburscount = 0;
                            int ss1 = 0;
                            lstHeader = new List<ent_GLHeader>();
                            foreach (DataRow row in lndisbrh_tbl.Rows)
                            {
                                valid_return = false;
                                header = new ent_GLHeader();
                                lstDet = new List<ent_GLDetail>();
                                ent_GLDetail detail;
                                strLoanID = row["loanid"].ToString().Trim();
                                hdSeqNo = row["loanid"].ToString().Trim();
                                ifscCode = row["ifsc_code"].ToString().Trim();
                                benBankName = row["bank_name"].ToString().Trim();
                                benBankAC = row["account_number"].ToString().Trim();
                                benName = row["beneficiary_name"].ToString().Trim();
                                benBranchName = row["bank_branch"].ToString().Trim();
                                Def_LNDes = Convert.ToDouble(row["financeAmt"].ToString().Trim()) - Convert.ToDouble(row["net_amount"].ToString().Trim());
                                branchid = row["branchid"].ToString().Trim();
                                placeSupcode = row["stateid"].ToString().Trim();
                                if (placeSupcode.Length == 1)
                                    placeSupcode = "0"+placeSupcode;
                                gstStateCode = "07";
                                strbc_id = row["bc_id"].ToString().Trim();
                                strbc_name = row["bc_name"].ToString().Trim();

                                NetAmount = row["net_amount"].ToString().Trim();
                              
                                //'To check if igst or cgst is applicable
                                if (gstStateCode == placeSupcode)
                                    strS_var = "SAME";
                                else
                                    strS_var = "DEFF";

                                lblDisburs_mess.Text = "Please wait, In proccess Row Number= " + row["RN"].ToString().Trim() + " LoanId-" + strLoanID;
                                ojbLog.WriteLog(logfilename,"Row Number= " + row["RN"].ToString().Trim());
                                //if (strbc_id != "" && strbc_id != null && strbc_id != "NULL")
                                //{
                                //    ojbLog.WriteLog(logfilename,"BC_ID not null inserting Loan_Id = " + strLoanID +"   BC_id="+strbc_id+"-"+strbc_name);
                                //    Insert_LoanIdMStr_BC(strLoanID,strbc_id,strbc_name);
                                //    ojbLog.WriteLog(logfilename,"BC_ID not null Inserted Loan_Id");
                                //}
                                //else
                                //{
                                    ojbLog.WriteLog(logfilename,"Calling function to check if loanid already dusbursed....Loan_Id = " + strLoanID);
                                    // ojbLog.WriteLog(logfilename,"Calling function to check if loanid already dusbursed....Loan_Id=" + strLoanID);
                                    if (CHK_LOAN_Disb(strLoanID) == true)
                                    {
                                        ojbLog.WriteLog(logfilename,"Calling function to check if loanid exist in loan master....");
                                        //check existing loanid in loan master
                                        if (CHK_LNID_LNLOAN(strLoanID,0,"") == true)
                                        {
                                            // check existing dis in sage
                                            ojbLog.WriteLog(logfilename,"Calling function to Check net amount of disbursement....");
                                            //'Check net amount of disbursement
                                            if (checkamtvalidation(hdSeqNo, Def_LNDes, strS_var) == true)
                                            {
                                                ojbLog.WriteLog(logfilename,"Calling function to Check and insert loan account in sage database....");
                                                //'Check and insert loan account
                                                if (getaccountid_status(hdSeqNo) == true)
                                                {
                                                    ojbLog.WriteLog(logfilename,"Calling function to Validate loan disbursement details before GL entry....");
                                                    //'Validate loan disbursement details before GL entry
                                                    if (loandisbdetails(hdSeqNo, placeSupcode, branchid) == true)
                                                    {
                                                        ojbLog.WriteLog(logfilename,"Loan disbusment detail GL Account validation successful....");
                                                        valid_return = true;
                                                        ss1 = ss1 + 1;
                                                        lblValPassCount.Text = ss1.ToString();
                                                    //lblValPassCount.Text = strcount.ToString();
                                                    // Checking detail line account successful...."
                                                    //if (loandisdetails_oldloan(hdSeqNo) == true) ///Validate old loan account exist or not
                                                    //{
                                                    //    ojbLog.WriteLog(logfilename,"Checking detail line old loan account successful....");
                                                    //    //GLDesmtDetail.ListBox1.AddItem " Checking detail line old loan account successful...."
                                                    //    //GLBatchEntry(hdSeqNo);
                                                    //    strreturn = true;
                                                    //}
                                                    //else { ojbLog.WriteLog(logfilename,"loandisdetails_oldloan Validation Failed...."+strLoanID);  }
                                                }
                                                else { ojbLog.WriteLog(Disb_LogFile_failed, "Loan disbusment detail GL Account validation Failed...."+ strLoanID); }
                                                }
                                                else { ojbLog.WriteLog(Disb_LogFile_failed, "getaccountid_status Validation Failed...."+ strLoanID); }
                                            }
                                            else { ojbLog.WriteLog(Disb_LogFile_failed, "Amount Validation Failed...."+ strLoanID); }
                                        }
                                        else
                                        { ojbLog.WriteLog(Disb_LogFile_failed, "Some thing error for inserting Loan Master...."+strLoanID); }
                                    }
                                    else
                                    {
                                        disburscount = disburscount + 1;
                                        lblslotcount.Text = disburscount.ToString();
                                    FailCount = FailCount - 1;
                                    }
                                    if (valid_return == true)
                                    {
                                        try
                                        {
                                            strcount = strcount + 1;
                                            ojbLog.WriteLog(logfilename,"Going to insert GLBatch Entry..........");
                                            // System.Data.SqlClient.SqlConnection conn;
                                            System.Data.SqlClient.SqlCommand cmd1;
                                            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
                                            conn.Open();
                                            // Qry to check it is tax type entry or not
                                            string Querystring1 = " Select (sum(ISNULL(AMOUNT, 0)) * -1),SUM(CONVERT(DECIMAL, igst_amt)),SUM(CONVERT(DECIMAL, cgst_amt)),SUM(CONVERT(DECIMAL, sgst_amt))" +
                                               "  from " + THSERVERDETS + " where LOANID = " + hdSeqNo + "  and(ISNULL(igst_amt, 0) <> 0 or ISNULL(cgst_amt, 0) <> 0)";
                                            cmd1 = new System.Data.SqlClient.SqlCommand(Querystring1, conn);
                                            cmd1.CommandTimeout = 180;
                                            cmd1.CommandType = CommandType.Text;
                                            cmd1.ExecuteNonQuery();
                                            using (System.Data.SqlClient.SqlDataAdapter glsda = new System.Data.SqlClient.SqlDataAdapter())
                                            {
                                                cmd1.Connection = conn;
                                                glsda.SelectCommand = cmd1;
                                                using (dtgjbatch = new DataTable())
                                                {
                                                    glsda.Fill(dtgjbatch);
                                                    if (dtgjbatch.Rows.Count > 0)  // if it is tax tupe entry count will be more than 0
                                                    {
                                                        taxType_entry = true;
                                                        taxbase = Convert.ToDouble(dtgjbatch.Rows[0][0]);
                                                        igstamt = Convert.ToDouble(dtgjbatch.Rows[0][1]);
                                                        cgstamt = Convert.ToDouble(dtgjbatch.Rows[0][2]);
                                                        sgstamt = Convert.ToDouble(dtgjbatch.Rows[0][3]);
                                                    }
                                                    else
                                                    {
                                                        taxType_entry = false;
                                                        taxbase = 0;
                                                        igstamt = 0;
                                                        cgstamt = 0;
                                                        sgstamt = 0;
                                                    }
                                                    // to determine igst or cgst depending on the state codes and deriving tax group
                                                    if (strS_var == "DEFF")
                                                        taxgroupcode = gstStateCode + "IGN";
                                                    else
                                                        taxgroupcode = gstStateCode + "CGN";

                                                    DataSet tblibr;
                                                    string Querystring11 = " Select LIABILITY from [" + SAGEDB + "].[dbo].TXAUTH where AUTHORITY='" + taxgroupcode + "' ; " +   // deriving tax liability account
                                                                    " select a.id, a.loanid, '" + borrowerName + "' as borrowerName, " +
                                                                    " case when RTRIM(a.type)= 'Processing Fee' then RTRIM(a.sage_id)+'-' + Cast(right('0'+left(b.stateid,2),2) as varchar) + '-' + Cast(right('0'+left(b.branchid,4),4) as varchar)  " +
                                                                    " when RTRIM(a.type)= 'Verification' then RTRIM(a.sage_id)+'-00'  when RTRIM(a.type)= 'NIP' then RTRIM(a.sage_id)+Cast(right('0'+left(b.stateid,2),2) as varchar) + '-' + Cast(right('0'+left(b.branchid,4),4) as varchar)  " +
                                                                    " when RTRIM(a.type)= 'Risk Fund' then RTRIM(a.sage_id)+'-00'  when RTRIM(a.type)= 'Hospi' then  RTRIM(a.sage_id)+'-00' " +
                                                                    " when RTRIM(a.type)= 'Penydrop' then RTRIM(a.sage_id)   else '0'  end,  " +
                                                                    " a.[type], a.oldloanid, a.amount , a.gst_rate, ISNULL(a.igst_amt, 0), ISNULL(a.cgst_amt, 0), ISNULL(a.sgst_amt, 0),  " +
                                                                    " '" + DisSageAccId + "' sage_acctid from " + THSERVERDETS + " a  " +
                                                                    " left join " + THSERVERMSTR + " b on b.loanid = a.loanid  where  a.amount <> 0 and a.loanid = '" + hdSeqNo + "'"; // selecting detail line items for journal entry

                                                    cmd1 = new System.Data.SqlClient.SqlCommand(Querystring11, conn);
                                                    cmd1.CommandTimeout = 180;
                                                    cmd1.CommandType = CommandType.Text;
                                                    cmd1.ExecuteNonQuery();
                                                    using (System.Data.SqlClient.SqlDataAdapter glsds = new System.Data.SqlClient.SqlDataAdapter())
                                                    {
                                                        using (tblibr = new DataSet())
                                                        {
                                                            glsds.SelectCommand = cmd1;
                                                            glsds.Fill(tblibr);
                                                            if (tblibr.Tables[0].Rows.Count > 0)
                                                            {
                                                                taxliabilityaccount = tblibr.Tables[0].Rows[0][0].ToString().Trim();
                                                            }
                                                            else
                                                            {
                                                                ojbLog.WriteLog(logfilename,"Tax liability GL account not found  - aborting");
                                                            }
                                                        }
                                                        dloanid = tblibr.Tables[1].Rows[0][1].ToString();
                                                        sageloanacct = tblibr.Tables[1].Rows[0][11].ToString().Trim();
                                                    }
                                                    // creating header and detail line 
                                                    if (taxType_entry == true)
                                                    {
                                                        //Entering header details
                                                        header.SourceType = "LD";
                                                        header.Description = sageloanacct;
                                                        header.JournalDetails = lstDet;
                                                    header.PostingDate = dateTimePicker2.Text + "T00:00:00Z";
                                                    header.DocumentDate = dateTimePicker2.Text + "T00:00:00Z";
                                                    lstHeader.Add(header);

                                                        // Entering Journal details for taxes 
                                                        detail = new ent_GLDetail();
                                                        detail.SourceType = "TI";   // Detail source type T1 for crediting Tax liability IGST account
                                                        detail.Reference = hdSeqNo.Trim();
                                                        // detail.TaxAuthority = "07IGN";
                                                        detail.Description = sageloanacct.Trim();
                                                        detail.AccountNumber = taxliabilityaccount.Trim();
                                                        detail.Amount = igstamt;
                                                        lstDet.Add(detail);

                                                        detail = new ent_GLDetail();
                                                        detail.SourceType = "LC";   // Detail source type LC for debiting loan account for gst
                                                        detail.Reference = hdSeqNo.Trim();
                                                        detail.AccountNumber = sageloanacct.Trim();
                                                        detail.Description = "GST";
                                                        detail.Amount = igstamt * -1;
                                                        lstDet.Add(detail);

                                                        // ojbLog.WriteLog(logfilename,detail.ToString());
                                                    }
                                                    foreach (DataRow rowR in tblibr.Tables[1].Rows)
                                                    {
                                                        //'Creating entries for detail line which are not adjustment of old loan account
                                                        // ' Checking & processing for charges which are subject to gst
                                                        if (rowR[3].ToString() != "0" && rowR[6].ToString() != "0" && rowR[7].ToString() != "0")
                                                        {
                                                            detail = new ent_GLDetail();
                                                            detail.AccountNumber = rowR[3].ToString().Trim();
                                                            detail.Amount = rowR[6];
                                                            detail.Description = rowR[1].ToString().Trim() + "-" + rowR[2].ToString().Trim();
                                                            detail.Reference = hdSeqNo + "-" + rowR[0].ToString().Trim();
                                                            detail.SourceType = "X5";                                    //Taxable charge item -  5 means tax class 5 ie 18%
                                                                                                                         //detail.TaxAuthority = "07IGN";
                                                            lstDet.Add(detail);
                                                            ojbLog.WriteLog(logfilename,"X5=>" + rowR[11].ToString().Trim());
                                                            detail = new ent_GLDetail();
                                                            detail.AccountNumber = rowR[11].ToString().Trim();
                                                            detail.Amount = (Convert.ToDouble(rowR[6]) * -1);
                                                            detail.Description = rowR[3].ToString().Trim() + "-" + rowR[4].ToString().Trim();
                                                            detail.Reference = hdSeqNo + "-" + rowR[0].ToString().Trim();
                                                            detail.SourceType = "LC";  // Detail source type LC for debiting loan account for gst
                                                                                       // detail.TaxAuthority = "07IGN";
                                                            lstDet.Add(detail);
                                                        }
                                                        //'Checking & processing for charges which are NOT subject to gst
                                                        if (rowR[3].ToString() != "0" && rowR[6].ToString() != "0" && rowR[7].ToString() == "0")
                                                        {
                                                            detail = new ent_GLDetail();
                                                            detail.AccountNumber = rowR[3].ToString().Trim();
                                                            detail.Amount = rowR[6];
                                                            detail.Description = rowR[1].ToString().Trim() + "-" + rowR[2].ToString().Trim();
                                                            detail.Reference = hdSeqNo + "-" + rowR[0].ToString().Trim();
                                                            detail.SourceType = "X1";                 //Non taxable charge  items
                                                                                                      //detail.TaxAuthority = "07IGN";
                                                            lstDet.Add(detail);
                                                            ojbLog.WriteLog(logfilename,"X1=>" + rowR[11].ToString().Trim());
                                                            detail = new ent_GLDetail();
                                                            detail.AccountNumber = rowR[11].ToString().Trim();
                                                            detail.Amount = (Convert.ToDouble(rowR[6]) * -1);
                                                            detail.Description = rowR[3].ToString().Trim() + "-" + rowR[4].ToString().Trim();
                                                            detail.Reference = hdSeqNo + "-" + rowR[0].ToString().Trim();
                                                            detail.SourceType = "LC";
                                                            //detail.TaxAuthority = "07IGN";
                                                            lstDet.Add(detail);
                                                        }

                                                        //'Checking & processing for old loan adjustment
                                                        //'For crediting old loan account - credit amount
                                                        if (rowR[3].ToString() == "0" && rowR[6].ToString() != "0" && rowR[5].ToString() != "0")
                                                        {
                                                            detail = new ent_GLDetail();
                                                            detail.AccountNumber = oldacct[rowR[0].ToString().Trim()];
                                                            detail.Amount = rowR[6];
                                                            detail.Description = rowR[1].ToString().Trim() + "-" + rowR[2].ToString().Trim();
                                                            detail.Reference = hdSeqNo + "-" + rowR[0].ToString().Trim();
                                                            detail.SourceType = "OL";
                                                            //detail.TaxAuthority = "07IGN";
                                                            lstDet.Add(detail);
                                                            ojbLog.WriteLog(logfilename,"ol =>" + rowR[11].ToString().Trim());
                                                            detail = new ent_GLDetail();
                                                            detail.AccountNumber = rowR[11].ToString().Trim();
                                                            detail.Amount = (Convert.ToDouble(rowR[6]) * -1);
                                                            detail.Description = oldacct[rowR[0].ToString().Trim()];
                                                            detail.Reference = hdSeqNo + "-" + rowR[0].ToString().Trim();
                                                            detail.SourceType = "LO";
                                                            // detail.TaxAuthority = "07IGN";
                                                            lstDet.Add(detail);
                                                        }
                                                        ojbLog.WriteLog(logfilename,"Json Created for GL Detail line item.." + rowR[3].ToString().Trim());
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ojbLog.WriteLog(Disb_LogFile_failed, "GLBatchEntry Inseting fail........." + ex.Message.ToString());
                                        }
                                        Jbatch.UpdateOperation = "Unspecified";
                                        Jbatch.Description = "LD";
                                        Jbatch.JournalHeaders = lstHeader;
                                        var payloadString7 = JsonConvert.SerializeObject(Jbatch);
                                        Loan_Dis_List = strLoanID + "," + Loan_Dis_List;
                                        string ss = "'00" + strLoanID + "'";
                                        Loan_Dis_List00 = ss + "," + Loan_Dis_List00;
                                    }
                                    else
                                    {
                                        FailCount = FailCount + 1;
                                        lblValFailCount.Text = FailCount.ToString();
                                    }
                                
                                    ojbLog.WriteLog(logfilename,"                                                              ");
                                    ojbLog.WriteLog(logfilename,"--------------------------------------------------------------------------------------------");
                                    ojbLog.WriteLog(logfilename,"                                                             ");

                            }
                            if (strcount > 0)
                                {
                                    var payloadString = JsonConvert.SerializeObject(Jbatch);

                                    lblDisburs_mess.Text = "Please wait for Batch Entry API Response......";
                                    ojbLog.WriteLog(logfilename,payloadString.ToString());
                                    ojbLog.WriteLog(logfilename,"JSON creater for GLBatchEntry  going for inserting ..");

                                    dynamic newCustomer = POSTData(Jbatch, "http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/GL/GLJournalBatches");
                                    dynamic deserialized = null;
                                    try
                                    {
                                        if (newCustomer != "ERROR")
                                        {
                                        mstrlist = new ListBox();
                                            deserialized = JsonConvert.DeserializeObject(newCustomer.ToString());
                                            var rESPONSE = deserialized["JournalHeaders"];
                                            dynamic des = JsonConvert.DeserializeObject(rESPONSE.ToString());
                                            str_batchno = des[0].BatchNumber.ToString();
                                            string EntryNumber = des[0].EntryNumber.ToString();
                                            ojbLog.WriteLog(logfilename,"GL Entry creation successful....GLBatchNumbr=" + str_batchno);
                                            Loan_Dis_List = Loan_Dis_List.Remove(Loan_Dis_List.Length - 1, 1);
                                            Loan_Dis_List00 = Loan_Dis_List00.Remove(Loan_Dis_List00.Length - 1, 1);
                                            lblDisburs_mess.Text = "Please wait for Bank API Response......";
                                            Boolean bk = BankEntry(Loan_Dis_List, str_batchno);

                                            if (bk == true)
                                            {
                                                Update_RTGS_MSTR(mstrlist);

                                                lblDisburs_mess.Text = "Updating loan master in proccess......";
                                                if (UpdateLoanMaster(str_batchno, BankEntryNumber, Loan_Dis_List00) == true)
                                                {
                                                    MessageBox.Show("All Proccess successfully posted! " + Environment.NewLine + "  Batch Number=" + str_batchno + "; " + Environment.NewLine + "Last Bank Entry Number=--" + BankEntryNumber);
                                                getList();
                                            }
                                                else ojbLog.WriteLog(Disb_LogFile_failed, "UpdateLoanMaster function failed......");
                                            }
                                            else
                                            { }
                                        }
                                        else
                                            ojbLog.WriteLog(Disb_LogFile_failed, "GLBatchEntry API failed......");
                                    }
                                    catch (Exception Err)
                                    {
                                        //rntGLBatchEntry = false;
                                        MessageBox.Show(deserialized);
                                        ojbLog.WriteLog(Disb_LogFile_failed, "GLBatchEntry function failed......" + Err.Message.ToString());
                                    }
                                }
                           // }

                          
                        }
                        else
                        {
                            //Data Empty
                            ojbLog.WriteLog(logfilename,"No records to process.......");
                            // MessageBox.Show("Empty table 'LNDISBH'...");
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ojbLog.WriteLog(Disb_LogFile_failed, "GLJAccountController function failed...." + ex.Message);
            }
            // }

        }
        public Boolean Insert_LoanIdMStr_BC(string sLoanid,string bc_id,string bc_name)
        {
            
            Boolean lnReturn = false;
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlCommand cmdR;
            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
            conn.Open();
            using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
            {
              // ojbLog.WriteLog(logfilename,"Loan Id does not exist in loan master..");
                try
                {
                    ojbLog.WriteLog(logfilename,"Going to insert Loan Id in loan master..");
                    String SS = "CTLOAN.[dbo].InsertLoanMaster_ByLoanID";
                    cmdR = new System.Data.SqlClient.SqlCommand(SS, conn);
                    cmdR.CommandTimeout = 180;
                    cmdR.CommandType = CommandType.StoredProcedure;
                    cmdR.Parameters.AddWithValue("@loan_id", SqlDbType.VarChar).Value = "00"+sLoanid;
                    cmdR.Parameters.AddWithValue("@bc_id", SqlDbType.VarChar).Value = bc_id;
                    cmdR.Parameters.AddWithValue("@bc_name", SqlDbType.VarChar).Value = bc_name;
                    sda.SelectCommand = cmdR;
                    cmdR.ExecuteNonQuery();
                    DataTable dtRunSeq;
                    using (dtRunSeq = new DataTable())
                    {
                        sda.Fill(dtRunSeq);
                        if (dtRunSeq.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtRunSeq.Rows)
                            {
                               // if (row["EXIST"].ToString() == "EXIST")
                              //  {
                                    borrowerName = row["borrower_name"].ToString();
                                    DisSageAccId = row["sage_acctid"].ToString();
                                   // ojbLog.WriteLog(logfilename,"Loan Id already Exist!");
                               // }
                               // else {
                                  //  borrowerName = row["borrower_name"].ToString();
                                  //  DisSageAccId = row["sage_acctid"].ToString();
                                    ojbLog.WriteLog(logfilename,"Loan Id successfully inserted in loan master..");
                               // }
                                lnReturn = true;
                            }

                        }
                        else
                        {
                            ojbLog.WriteLog(Disb_LogFile_failed, "Loan Id could not be inserted in loan master!");

                        }
                    }
                    lnReturn = true;
                }
                catch (Exception ex)
                {
                    ojbLog.WriteLog(Disb_LogFile_failed, "Error  Query execution time -!" + ex);
                }
            }   
            conn.Close();
            return lnReturn;
        }
        public Boolean CHK_LOAN_Disb(string sLoanid)
        {
            DataTable lndisbrh_tbl;
            Boolean lnReturn = true;
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlCommand cmd;
            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
            conn.Open();
            string Querystring = "SELECT h.borrower_name,h.sage_acctid FROM [CTLOAN].dbo.LNMSTR h where dis_GLBatch is not NULL and  h.loan_id= '00" + sLoanid + "'";
            cmd = new System.Data.SqlClient.SqlCommand(Querystring, conn);
            cmd.CommandTimeout = 180;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
            {
                var objerrList = (IDictionary<string, object>)person;
                cmd.Connection = conn;
                sda.SelectCommand = cmd;
                using (lndisbrh_tbl = new DataTable())
                {
                    sda.Fill(lndisbrh_tbl);
                    if (lndisbrh_tbl.Rows.Count > 0)
                    {
                        ojbLog.WriteLog(logfilename,"Loan Id already disbursed...");
                        lnReturn = false;
                    }
                    else
                    {
                        ojbLog.WriteLog(logfilename,"Loan Id  to be disburse...");
                    }
                }
            }
            conn.Close();
            return lnReturn;
        }
        public Boolean CHK_LNID_LNLOAN(string sLoanid, int bc_id, string bc_name)
        {
            DataTable lndisbrh_tbl;
            Boolean lnReturn = false;
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlCommand cmd;
            System.Data.SqlClient.SqlCommand cmdR;
            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
            conn.Open();
            string Querystring = "SELECT h.borrower_name,h.sage_acctid FROM [CTLOAN].dbo.LNMSTR h where h.loan_id= '00" + sLoanid + "'";
            cmd = new System.Data.SqlClient.SqlCommand(Querystring, conn);
            cmd.CommandTimeout = 180;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
            {
                var objerrList = (IDictionary<string, object>)person;
                cmd.Connection = conn;
                sda.SelectCommand = cmd;
                using (lndisbrh_tbl = new DataTable())
                {
                    sda.Fill(lndisbrh_tbl);
                    if (lndisbrh_tbl.Rows.Count > 0 )
                    {
                        ojbLog.WriteLog(logfilename,"Loan Id exist in loan master..");
                        borrowerName = lndisbrh_tbl.Rows[0]["borrower_name"].ToString();
                        DisSageAccId = lndisbrh_tbl.Rows[0]["sage_acctid"].ToString();
                        lnReturn = true;

                    }
                    else
                    {
                        ojbLog.WriteLog(logfilename,"Loan Id does not exist in loan master..");
                        try
                        {
                            ojbLog.WriteLog(logfilename,"Going to insert Loan Id in loan master..");
                            String SS = "CTLOAN.[dbo].InsertLoanMaster_ByLoanID";
                            cmdR = new System.Data.SqlClient.SqlCommand(SS, conn);
                            cmdR.CommandTimeout = 180;
                            cmdR.CommandType = CommandType.StoredProcedure;
                            cmdR.Parameters.AddWithValue("@loan_id", SqlDbType.Int).Value = sLoanid;
                            cmdR.Parameters.AddWithValue("@bc_id", SqlDbType.Int).Value = bc_id;
                            cmdR.Parameters.AddWithValue("@bc_name", SqlDbType.VarChar).Value = bc_name;
                            sda.SelectCommand = cmdR;
                            cmdR.ExecuteNonQuery();
                            DataTable dtRunSeq;
                            using (dtRunSeq = new DataTable())
                            {
                                sda.Fill(dtRunSeq);
                                if (dtRunSeq.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtRunSeq.Rows)
                                    {
                                        borrowerName = row["borrower_name"].ToString();
                                        DisSageAccId = row["sage_acctid"].ToString();
                                        lnReturn = true;
                                        ojbLog.WriteLog(logfilename,"Loan Id successfully inserted in loan master..");
                                    }

                                }
                                else
                                {
                                    ojbLog.WriteLog(Disb_LogFile_failed, "Loan Id could not be inserted in loan master!");

                                }
                            }
                            lnReturn = true;
                        }
                        catch (Exception ex)
                        {
                            ojbLog.WriteLog(Disb_LogFile_failed, "Error  Query execution time -!" + ex);
                        }
                    }
                }
            }
            conn.Close();
            return lnReturn;
        }
        public Boolean checkamtvalidation(String p_hdSeq, Double p_DefLoanDesc, String Defr)
        {
            Boolean ReturnVlidation = false;
            try
            {
                ReturnVlidation = false;
                System.Data.SqlClient.SqlConnection connR;
                System.Data.SqlClient.SqlCommand cmdR;

                String sQueryDeff;
                String sQuerySame;
                DataTable dtCheckamt;
                sQueryDeff = "";
                sQuerySame = "";
                dtCheckamt = new DataTable();
                sQueryDeff = "SELECT SUM(AMOUNT) + SUM(Convert(decimal,IGST_AMT)) AS TransD FROM " + THSERVERDETS + "  WHERE loanid='" + p_hdSeq + "'";
                sQuerySame = "SELECT  SUM(AMOUNT) + SUM(Convert(decimal,SGST_AMT)) + SUM(Convert(decimal,CGST_AMT)) AS TransD FROM " + THSERVERDETS + " WHERE loanid='" + p_hdSeq + "' ";

                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();

                connR = new System.Data.SqlClient.SqlConnection(connectionstring);
                connR.Open();
                // choose sql qry for igst or cgst
                if (Defr == "DEFF")
                {
                    cmdR = new System.Data.SqlClient.SqlCommand(sQueryDeff, connR);
                    cmdR.CommandTimeout = 180;
                    cmdR.CommandType = CommandType.Text;
                    sda.SelectCommand = cmdR;
                    sda.Fill(dtCheckamt);
                }
                else if (Defr == "SAME")
                {
                    cmdR = new System.Data.SqlClient.SqlCommand(sQuerySame, connR);
                    cmdR.CommandTimeout = 180;
                    cmdR.CommandType = CommandType.Text;
                    sda.SelectCommand = cmdR;
                    sda.Fill(dtCheckamt);
                }
                else
                { ojbLog.WriteLog(Disb_LogFile_failed, "Amount validation failed SQL Query.."); }
                if (dtCheckamt.Rows.Count > 0)
                {
                    if (p_DefLoanDesc == Convert.ToDouble(dtCheckamt.Rows[0]["TransD"]) * -1)
                    {
                        ReturnVlidation = true;
                        ojbLog.WriteLog(logfilename,"Amount validation passed...");
                    }
                    else
                    {
                        ReturnVlidation = false;
                        ojbLog.WriteLog(Disb_LogFile_failed, "Amount validation failed....");
                    }
                }
                else { ojbLog.WriteLog(Disb_LogFile_failed, "Amount validation failed else condition...."); }
            }
            catch (Exception Err)
            {

                ojbLog.WriteLog(Disb_LogFile_failed, "checkamtvalidation function failed......" + Err.Message.ToString());
            }

            return ReturnVlidation;
        }
        public Boolean getaccountid_status(string LoanId)
        {
            Boolean returnvalue = false;
            try
            {
                System.Data.SqlClient.SqlConnection connR;
                System.Data.SqlClient.SqlCommand cmdR;
                String sQueryDeff;
                DataTable dtCheckamt;
                sQueryDeff = "";
                dtCheckamt = new DataTable();
                sQueryDeff = "Select * from [" + SAGEDB + "].dbo.GLAMF where ACCTFMTTD = '" + DisSageAccId.Trim() + "'";
                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
                connR = new System.Data.SqlClient.SqlConnection(connectionstring);
                connR.Open();
                cmdR = new System.Data.SqlClient.SqlCommand(sQueryDeff, connR);
                cmdR.CommandTimeout = 180;
                cmdR.CommandType = CommandType.Text;
                sda.SelectCommand = cmdR;
                sda.Fill(dtCheckamt);
                if (dtCheckamt.Rows.Count > 0)
                {
                    returnvalue = true;
                    ojbLog.WriteLog(logfilename,"Loan GL Account exist in Sage.....");
                }
                else
                {
                    ojbLog.WriteLog(logfilename,"Loan GL Account does not exist in Sage.....");
                    if (CheckGLSegments(LoanId) == true)
                    {
                        if (CreateGLAccount(LoanId) == true)
                        {
                            ojbLog.WriteLog(logfilename,"GL account successfuly inserted....." + LoanId);
                            returnvalue = true;
                        }
                        else { returnvalue = false; }
                    }
                }
            }
            catch (Exception ex)
            {
                returnvalue = false;
                ojbLog.WriteLog(Disb_LogFile_failed, "Getaccountid_status function failed..." + ex.Message);
            }
            return returnvalue;
        }
      
        public Boolean CheckGLSegments(String loanid)
        {
            Boolean chkSegment;
            chkSegment = false;
            try
            {
                // CHK_LNID_LNLOAN(loanid);
                ojbLog.WriteLog(logfilename,"Checking GL Segment value before inserting into Sage.....");
                Boolean apReturn03 = false;
                Boolean apReturn04 = false;
                Boolean apReturn05 = false;
                Boolean apReturn06 = false;
                Boolean apReturn08 = false;
                apReturn03 = SegmentAPI("http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/GL/GLSegmentCodes?$filter=SegmentNumber eq '000003' and  SegmentCodeKey eq '" + DisSageAccId.Trim().Substring(6, 2) + "'", "000003 This state code segment=>" + DisSageAccId.Trim().Substring(6, 2));
                apReturn04 = SegmentAPI("http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/GL/GLSegmentCodes?$filter=SegmentNumber eq '000004' and  SegmentCodeKey eq '" + DisSageAccId.Trim().Substring(9, 3) + "'", "000004 This Cluster segment=>" + DisSageAccId.Trim().Substring(9, 3));
                apReturn05 = SegmentAPI("http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/GL/GLSegmentCodes?$filter=SegmentNumber eq '000005' and  SegmentCodeKey eq '" + DisSageAccId.Trim().Substring(13, 3) + "'", "000005 This District no. segment=>" + DisSageAccId.Trim().Substring(13, 3));
                apReturn06 = SegmentAPI("http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/GL/GLSegmentCodes?$filter=SegmentNumber eq '000006' and  SegmentCodeKey eq '" + DisSageAccId.Trim().Substring(17, 4) + "'", "000006 This Branch no. segment=>" + DisSageAccId.Trim().Substring(17, 4));
                apReturn08 = SegmentAPI("http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/GL/GLSegmentCodes?$filter=SegmentNumber eq '000008' and  SegmentCodeKey eq '" + DisSageAccId.Trim().Substring(22, 8) + "'", "000008 Loan ID=>" + DisSageAccId.Trim().Substring(22, 8));

                if (apReturn03 == apReturn04 == apReturn05 == apReturn06 == true && apReturn08 == false)
                {
                    ojbLog.WriteLog(logfilename,"state, cluster, District and branch segement exist in Sage.....");
                    var detail = new
                    {
                        SegmentNumber = "000008",
                        SegmentCodeKey = DisSageAccId.Trim().Substring(22, 8),
                        SegmentCodeDescription = borrowerName
                    };
                    ojbLog.WriteLog(logfilename,"Going to insert loan segment.........");

                    dynamic SegmentResponse = POSTData(detail, "http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/" + @"GL/GLSegmentCodes");
                    dynamic deserialized = null;
                    try
                    {
                        deserialized = JsonConvert.DeserializeObject(SegmentResponse.ToString());
                        string SegmentCodeKey = deserialized.SegmentCodeKey;
                        chkSegment = true;
                        ojbLog.WriteLog(logfilename,"Loan segment successfully inserted.........");
                    }
                    catch (Exception err)
                    {
                        ojbLog.WriteLog(Disb_LogFile_failed, "Loan SegmentResponse API Failed " + err.Message);
                    }

                    //chkSegment = true;
                }
                else if (apReturn03 == false || apReturn04 == false || apReturn05 == false || apReturn06 == false)
                {
                    ojbLog.WriteLog(logfilename,"One of the segement in state, cluster, District and branch does not exist in Sage.....");
                }
                else { ojbLog.WriteLog(logfilename,"Loan id  exist in Sage....."); chkSegment = true; }
            }
            catch (Exception Err)
            {
                ojbLog.WriteLog(Disb_LogFile_failed, "CheckGLSegments function failed......" + Err.Message.ToString());
            }
            return chkSegment;
        }
        public Boolean SegmentAPI(string url, string segNumber)
        {
            Boolean apiReturn;
            try
            {
                using (var client = new HttpClient())
                {
                    apiReturn = false;
                    var authenticationBytes = Encoding.ASCII.GetBytes(USERNAME + ":" + PASSWORD);
                    using (HttpClient confClient = new HttpClient())
                    {
                        confClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                               Convert.ToBase64String(authenticationBytes));
                        confClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage message = confClient.GetAsync(url).Result;
                        if (message.IsSuccessStatusCode)
                        {
                            var inter = message.Content.ReadAsStringAsync();
                            JObject googleSearch = JObject.Parse(inter.Result.ToString());

                            // get JSON result objects into a list
                            IList<JToken> results12 = googleSearch["value"].ToList();
                            if (results12.Count > 0)
                            {
                                apiReturn = true;
                                ojbLog.WriteLog(logfilename,"SegmentNumber= " + segNumber + "  validated!");
                            }
                            else
                            {
                                ojbLog.WriteLog(logfilename,"SegmentNumber= " + segNumber + " Not validated!");
                                apiReturn = false;
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                ojbLog.WriteLog(logfilename,"Some thing wrong in SegmentAPI Method");
                apiReturn = false;
            }

            return apiReturn;
        }
        public Boolean CreateGLAccount(string LoanId)
        {
            Boolean GlReturn;
            GlReturn = false;
            try
            {
                var GLAccount = new
                {
                    UnformattedAccount = DisSageAccId.Replace("-", ""), //SageAccId.Replace("-", ""),
                    Description = borrowerName,
                    AccountType = "BalanceSheet",
                    NormalBalanceDRCR = "1",
                    Status = "1",
                    StructureCode = "ASCDBL",
                    AccountGroupCode = "07"
                };
                ojbLog.WriteLog(logfilename,"GL Account Inserting in sage.......");
                dynamic newGLAccount = POSTData(GLAccount, "http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/" + @"GL/GLAccounts");
                dynamic deserialized = null;
                try
                {
                    deserialized = JsonConvert.DeserializeObject(newGLAccount.ToString());
                    string AccountNumber = deserialized.AccountNumber;
                    GlReturn = true;
                    ojbLog.WriteLog(logfilename,"Gl Account inserted successfully.........");
                }
                catch (Exception err)
                {
                    ojbLog.WriteLog(Disb_LogFile_failed, "Loan SegmentResponse API Failed " + err.Message);
                }
                ojbLog.WriteLog(logfilename,"GL Account Inserted in sage");

            }
            catch (Exception Err)
            {
                GlReturn = false;
                ojbLog.WriteLog(Disb_LogFile_failed, "GL Account Insert failed....." + Err.Message.ToString());
            }
            return GlReturn;
        }
        public Boolean loandisbdetails(string loanId, string strstate, string strbranchId)
        {
            int count_dis = 0;
            int count_dis_row = 0;
            Boolean loandisbdetails = false;
            oldacct = new Dictionary<string, string>();
            DataTable dtlnDetail;
            try
            {
                System.Data.SqlClient.SqlConnection conn;
                System.Data.SqlClient.SqlCommand cmd;
                System.Data.SqlClient.SqlCommand cmd1;
                conn = new System.Data.SqlClient.SqlConnection(connectionstring);
                conn.Open();
                ojbLog.WriteLog(logfilename,"Checking GL Accounts of detail line.......");

                string Querystring = "Select * from " + THSERVERDETS + " where amount<>0 And loanid = " + loanId;
                cmd = new System.Data.SqlClient.SqlCommand(Querystring, conn);
                cmd.CommandTimeout = 180;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                {
                    var objerrList = (IDictionary<string, object>)person;
                    cmd.Connection = conn;
                    sda.SelectCommand = cmd;
                    using (dtlnDetail = new DataTable())
                    {
                        sda.Fill(dtlnDetail);
                        if (dtlnDetail.Rows.Count > 0)
                        {
                            count_dis_row = dtlnDetail.Rows.Count;
                            foreach (DataRow row in dtlnDetail.Rows)
                            {
                                string TransDesc = row["type"].ToString().Trim();
                                string sageid = row["sage_id"].ToString();
                                string oldloanid = row["oldloanid"].ToString();
                                if (sageid != "0" && oldloanid == "0")
                                {
                                    string sqlQry = "";
                                    DataTable dtln;
                                    if (strbranchId.Trim().Length == 3)
                                        strbranchId = "0" + strbranchId;

                                    if (strstate.Trim().Length == 1)
                                        strstate = "0" + strstate;

                                    string id1 = "";
                                    if (TransDesc == "NIP")
                                    {

                                        //id1 = "30103-07-9999";
                                        id1 = sageid + "-" + strstate + "-" + strbranchId;
                                        ojbLog.WriteLog(logfilename,"Checking transaction type...." + row[2] + "-" + id1);
                                        sqlQry = "SELECT * FROM [" + SAGEDB + "].dbo.GLAMF  where ACCTFMTTD='" + id1 + "'";
                                    }
                                    if (TransDesc == "Interest")
                                    {
                                        //id1 = "30103-07-9999";
                                        id1 = sageid + "-" + strstate + "-" + strbranchId;
                                        ojbLog.WriteLog(logfilename,"Checking transaction type...." + row[2] + "-" + id1);
                                        sqlQry = "SELECT * FROM [" + SAGEDB + "].dbo.GLAMF  where ACCTFMTTD='" + id1 + "'";
                                    }
                                    if (TransDesc == "Processing Fee")
                                    {
                                        id1 = sageid + "-" + strstate + "-" + strbranchId;
                                        ojbLog.WriteLog(logfilename,"Checking transaction type...." + row[2] + "-" + id1);
                                        sqlQry = "SELECT * FROM [" + SAGEDB + "].dbo.GLAMF  where ACCTFMTTD='" + id1 + "'";
                                    }
                                    if (TransDesc == "Verification")
                                    {
                                        id1 = sageid + "-00";
                                        ojbLog.WriteLog(logfilename,"Checking transaction type...." + row[2] + "-" + id1);
                                        sqlQry = "SELECT * FROM [" + SAGEDB + "].dbo.GLAMF  where ACCTFMTTD='" + id1 + "'";
                                    }
                                    if (TransDesc == "Risk Fund")
                                    {
                                        id1 = sageid + "-00";
                                        ojbLog.WriteLog(logfilename,"Checking transaction type...." + row[2] + "-" + id1);
                                        sqlQry = "SELECT * FROM [" + SAGEDB + "].dbo.GLAMF  where ACCTFMTTD='" + id1 + "'";
                                    }
                                    if (TransDesc == "Hospi")
                                    {
                                        id1 = sageid + "-00";
                                        ojbLog.WriteLog(logfilename,"Checking transaction type...." + row[2] + "-" + id1);
                                        sqlQry = "SELECT * FROM [" + SAGEDB + "].dbo.GLAMF  where ACCTFMTTD='" + id1 + "'";
                                    }

                                    if (TransDesc == "Penydrop")
                                    {
                                        id1 = sageid;
                                        ojbLog.WriteLog(logfilename, "Checking transaction type...." + row[2] + "-" + id1);
                                        sqlQry = "SELECT * FROM [" + SAGEDB + "].dbo.GLAMF  where ACCTFMTTD='" + id1 + "'";
                                    }
                                    cmd1 = new System.Data.SqlClient.SqlCommand(sqlQry, conn);
                                    cmd1.CommandTimeout = 180;
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.ExecuteNonQuery();
                                    using (System.Data.SqlClient.SqlDataAdapter sda1 = new System.Data.SqlClient.SqlDataAdapter())
                                    {
                                        cmd1.Connection = conn;
                                        sda1.SelectCommand = cmd1;
                                        using (dtln = new DataTable())
                                        {
                                            sda1.Fill(dtln);
                                            if (dtln.Rows.Count > 0)
                                            {
                                                count_dis = count_dis + 1;
                                                loandisbdetails = true;
                                                ojbLog.WriteLog(logfilename,"Exist transaction type...." + row[2] + "-" + id1);
                                            }
                                            else
                                            {
                                                ojbLog.WriteLog(logfilename,"Does not exist transaction type...." + row[2] + "-" + id1);
                                            }
                                        }
                                    }
                                }
                                else if (oldloanid != "0")
                                {
                                    try
                                    {

                                    
                                    ojbLog.WriteLog(logfilename, "Old loan id "+oldloanid+ " in process");
                                    DataTable dtOldLN;
                                    DataTable dtOldLN_Sg;
                                    string sqlQry = " Select loan_id, sage_acctid from [CTLOAN].[DBO].LNMSTR where loan_id = '00" + oldloanid + "' ";
                                    cmd1 = new System.Data.SqlClient.SqlCommand(sqlQry, conn);
                                    cmd1.CommandTimeout = 180;
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.ExecuteNonQuery();
                                    using (System.Data.SqlClient.SqlDataAdapter sda1 = new System.Data.SqlClient.SqlDataAdapter())
                                    {
                                        cmd1.Connection = conn;
                                        sda1.SelectCommand = cmd1;
                                        using (dtOldLN = new DataTable())
                                        {
                                            sda1.Fill(dtOldLN);
                                            if (dtOldLN.Rows.Count > 0)
                                            {

                                                string Acc_Mstr = dtOldLN.Rows[0]["sage_acctid"].ToString();
                                                ojbLog.WriteLog(logfilename,row[0].ToString() + " 000000 " + Acc_Mstr);
                                                //loandisbdetails = false;
                                                // ojbLog.WriteLog(logfilename,"Account exist........." + row[0]);
                                                string strQry = " Select * from [" + SAGEDB + "].dbo.GLAMF where ACCTFMTTD = '" + Acc_Mstr + "' ";
                                                cmd1 = new System.Data.SqlClient.SqlCommand(strQry, conn);
                                                cmd1.CommandTimeout = 180;
                                                cmd1.CommandType = CommandType.Text;
                                                cmd1.ExecuteNonQuery();
                                                using (System.Data.SqlClient.SqlDataAdapter sda2 = new System.Data.SqlClient.SqlDataAdapter())
                                                {
                                                    cmd1.Connection = conn;
                                                    sda2.SelectCommand = cmd1;
                                                    using (dtOldLN_Sg = new DataTable())
                                                    {
                                                        sda2.Fill(dtOldLN_Sg);
                                                        if (dtOldLN_Sg.Rows.Count > 0)
                                                        {
                                                            count_dis = count_dis + 1;
                                                            loandisbdetails = true;
                                                            ojbLog.WriteLog(logfilename,row[0].ToString() + " 000000 " + Acc_Mstr);
                                                            oldacct.Add(row[0].ToString(), Acc_Mstr);
                                                            ojbLog.WriteLog(logfilename,"old loan Account exist........." + row[0]);
                                                        }
                                                        else
                                                        {

                                                            ojbLog.WriteLog(logfilename,"old loan Account does not exist in Sage ......" + row[0]);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ojbLog.WriteLog(logfilename,"Old Loan Account does not exist in Loan Master........." + row[0]);
                                            }
                                        }
                                    }
                                        //Select loan_id, sage_acctid from LNMSTR where loan_id = 'oldloanid';

                                        //Select* from GLAMF where ACCTFMTTD = '11103-08-009-043-0166-00891226'
                                    }
                                    catch (Exception Err)
                                    {
                                        ojbLog.WriteLog(Disb_LogFile_failed, "OldLoan id function fail....." + Err.Message.ToString());
                                    }
                                }

                            }
                            if (count_dis_row == count_dis)
                                loandisbdetails = true;
                            else
                                loandisbdetails = false;
                        }
                    }
                }
            }
            catch (Exception Err)
            {

                ojbLog.WriteLog(Disb_LogFile_failed, "loandisbdetails function failed......" + Err.Message.ToString());
            }
            return loandisbdetails;
        }
        public Boolean UpdateLoanMaster(string batchEntryNo, string bkEntry, string hdSeqNo)
        {
            Boolean retrnBatchEntryNo = false;
            try
            {
                ojbLog.WriteLog(logfilename,"UpdateLoanMaster  Loan Id List" + hdSeqNo);
                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.SqlClient.SqlConnection connR;
                System.Data.SqlClient.SqlCommand cmdR;
                connR = new System.Data.SqlClient.SqlConnection(connectionstring); //,dis_BKEntry='" + bkEntry + "'
                connR.Open();
                string Querystring1 = " update CTLOAN.dbo.LNMSTR set dis_GLBatch='" + batchEntryNo + "'   where loan_id in (" + hdSeqNo + ")";
                cmdR = new System.Data.SqlClient.SqlCommand(Querystring1, connR);
                cmdR.CommandTimeout = 180;
                cmdR.CommandType = CommandType.Text;
                int res = cmdR.ExecuteNonQuery();
                ojbLog.WriteLog(logfilename,"Update loan master Status:" + res);
                if (res >= 1)
                    retrnBatchEntryNo = true;
                if (connR.State == System.Data.ConnectionState.Open)
                    connR.Close();
            }
            catch (Exception Err)
            {
                ojbLog.WriteLog(logfilename,"UpdateLoanMaster function failed......" + Err.Message.ToString());
            }
            return retrnBatchEntryNo;
        }
        public Boolean BankEntry(string str_Listloanid, string str_batchno)
        {
            Boolean strRtrn = false;
            int counter9;
            DataTable tbBank;
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlCommand cmd;
            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
            conn.Open();
            ojbLog.WriteLog(logfilename,"Loan Id List for bk entry" + str_Listloanid);
            //string Querystring = "Select sage_acctid, benf_acctno, Ifsc_code, benfbank_name, benbank_branch, borrower_name, disbursement_amt " +
            //                  "from LNDSDB.dbo.disbursement_master where glbatch_no='" + str_batchno + "'";
            string Querystring = "Select lm.sage_acctid, dm.account_number benf_acctno, dm.Ifsc_code, dm.bank_name benfbank_name,dm.bank_branch benbank_branch,lm.borrower_name, dm.net_amount, lm.loan_id " +
            "from " + THSERVERMSTR + " dm  left join [CTLOAN].dbo.LNMSTR lm on dm.loanid = lm.loan_id  where lm.loan_id in (" + str_Listloanid + ")";
            cmd = new System.Data.SqlClient.SqlCommand(Querystring, conn);
            cmd.CommandTimeout = 180;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            counter9 = 1;
           
            using (System.Data.SqlClient.SqlDataAdapter glsda = new System.Data.SqlClient.SqlDataAdapter())
            {
                cmd.Connection = conn;
                glsda.SelectCommand = cmd;
                using (tbBank = new DataTable())
                {
                    glsda.Fill(tbBank);
                    if (tbBank.Rows.Count > 0)
                    {
                        ojbLog.WriteLog(logfilename,"Loan Id countor bk entry=>" + tbBank.Rows.Count);
                        foreach (DataRow rw in tbBank.Rows)
                        {
                            ojbLog.WriteLog(logfilename,"Bank entry create for Loan Id= " + "LD-" + str_batchno + "-" + rw["loan_id"].ToString());
                            dynamic BK = new ExpandoObject();
                            var obj = (IDictionary<string, object>)BK;
                            var objerrList = (IDictionary<string, object>)person;
                            ent_BKdetln bk;
                            List<ent_BKdetln> objBn = new List<ent_BKdetln>();
                            //Header DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") '2021-03-21T00:00:00Z'
                            obj.Add("BankCode", txtbank.Text);
                            obj.Add("BankEntryDate", dateTimePicker2.Text+ "T00:00:00Z");
                            obj.Add("DateCreated", dateTimePicker2.Text + "T00:00:00Z");
                            obj.Add("BankEntryType", "Withdrawals");
                            obj.Add("EntryDescription", "LD-"+str_batchno + "-" + rw["loan_id"].ToString());
                            //Detail Line
                            bk = new ent_BKdetln();
                            bk.LineNumber = counter9 * -1;
                            bk.Reference = (rw[1].ToString().Trim() + "/" + rw[2].ToString().Trim());
                            bk.Description = rw[5].ToString().Trim();
                            bk.Comments = (rw[3].ToString().Trim() + "/" + rw[4].ToString().Trim());
                            bk.SourceAmount = rw[6];
                            bk.GLAccount = rw[0].ToString().Trim();
                            objBn.Add(bk);
                            //counter9 = counter9 + 1;

                            obj.Add("BankEntryDetail", objBn);
                            ojbLog.WriteLog(logfilename,JsonConvert.SerializeObject(obj));
                            string ss = "";
                            dynamic newCustomer = POSTData(obj, "http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/BK/BKBankEntries");
                            dynamic deserialized = JsonConvert.DeserializeObject(newCustomer.ToString());
                            if (newCustomer != "Conflict")
                            {
                                try
                                {
                                    string loanid = Convert.ToUInt32(rw["loan_id"].ToString()).ToString();
                                        string qry = "";
                                    ss = deserialized.BankEntryNumber;
                                    qry = "UPDATE " + THSERVERMSTR + "  SET SAGE_GL_BATCHNO='" + str_batchno + "'  ,SAGE_GL_BANK_ENTRYNO='" + ss + "' WHERE Convert(date,disbdate,100)='" + dateTimePicker2.Text + "' AND LOANID='" + loanid + "'";
                                    mstrlist.Items.Add(qry);
                                    ojbLog.WriteLog(logfilename, "Bank entry created-Entry No- " + ss);
                                    strRtrn = true;
                                }
                                catch (Exception ex)
                                {
                                    strRtrn = false;
                                    ojbLog.WriteLog(logfilename, "Bank Method, Some thing wrong......." + ex.Message.ToString());
                                }
                                BankEntryNumber = ss;
                            }
                            else ojbLog.WriteLog(logfilename, "Bank api response conflict..");

                        }
                    }
                    else { strRtrn = false; }
                }
            }
            return strRtrn;
        }

        public Boolean Update_RTGS_MSTR(ListBox lmbx)
        {
            Boolean retrnBatchEntryNo = false;
            try
            {
                ojbLog.WriteLog(logfilename, "UpdateLoanMaster  Loan Id List" + hdSeqNo);
                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.SqlClient.SqlConnection connR;
                System.Data.SqlClient.SqlCommand cmdR;
                connR = new System.Data.SqlClient.SqlConnection(connectionstring); //,dis_BKEntry='" + bkEntry + "'
                connR.Open();
                foreach(var item in lmbx.Items)
                { 
                                  // MessageBox.Show(item.ToString());
                cmdR = new System.Data.SqlClient.SqlCommand(item.ToString(), connR);
                cmdR.CommandTimeout = 180;
                cmdR.CommandType = CommandType.Text;
                int res = cmdR.ExecuteNonQuery();
                ojbLog.WriteLog(logfilename, "Update loan master Status:" + res);
                if (res >= 1)
                    retrnBatchEntryNo = true;
                }
                if (connR.State == System.Data.ConnectionState.Open)
                    connR.Close();
            }
            catch (Exception Err)
            {
                ojbLog.WriteLog(logfilename, "UpdateLoanMaster function failed......" + Err.Message.ToString());
            }
            return retrnBatchEntryNo;
        }

        #endregion

        #region Collection--------------------------------------

        public void colList()
        {
            #region  New Code ---Branch wise--------


            dynamic GL = new ExpandoObject();
            var obj = (IDictionary<string, object>)GL;

            JList = new List<ent_GLJBtch>();
            update_JList = new List<string>();

            DataTable dtnew = new DataTable();
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlCommand cmd;
            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
            conn.Open();
            string Querystring = " select count(*) TotalCount from " + THSERVERCOLL + "  where Received_Amt<>0  and sageacct_branchcash IS NOT NULL and GLBatch_No is null and " +
                   " Convert(date, Received_Date, 100) = '" + dateTimePicker3.Text + "' ;" +
                   " Select id, l.branch_id, l.loan_id,sage_acctid,d.sageacct_branchcash,d.received_Amt,d.Received_Date,d.ClientName,d.Receiptid,d.sage_credit_gl,d.[type] from [CTLOAN].dbo.LNMSTR l  " +
                   " inner join " + SAGEDB + ".dbo.GLAMF s on l.sage_acctid = s.ACCTFMTTD " +
                   " inner join " + THSERVERCOLL + " d on d.loanid=right('0000000'+left(l.loan_id,8),8) " +
                   " inner join " + SAGEDB + ".dbo.GLAMF s1 on d.sageacct_branchcash = s1.ACCTFMTTD " +
                   " where  s.ACTIVESW=1 and l.bc_id is null and d.Received_Amt <>0  and d.sageacct_branchcash IS NOT NULL and d.GLBatch_No is null and Convert(date,d.Received_Date,100)='" + dateTimePicker3.Text + "' ";
            cmd = new System.Data.SqlClient.SqlCommand(Querystring, conn);
            cmd.CommandTimeout = 180;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
            {
                var objerrList = (IDictionary<string, object>)person;
                cmd.Connection = conn;
                sda.SelectCommand = cmd;
                using (tbGRD_col = new DataSet())
                {
                    sda.Fill(tbGRD_col);
                    if (tbGRD_col.Tables[1].Rows.Count > 0)
                    {
                        lblCollTotal.Text = tbGRD_col.Tables[0].Rows[0]["TotalCount"].ToString();
                        dataGridView1.DataSource = tbGRD_col.Tables[1];
                        dataGridView1.EnableHeadersVisualStyles = false;
                        dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
                        dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                        // dgw_deposit.ColumnHeadersHeight = 50;
                        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                        dataGridView1.ColumnHeadersHeight = 30;
                        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                        dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
                        DataView viewSlot = new DataView(tbGRD_col.Tables[1]);

                        if (tbGRD_col.Tables[1].Rows.Count > 0)
                        {

                            DataView view1 = new DataView(tbGRD_col.Tables[1]);
                            view1.Sort = "branch_id asc";
                            DataTable tb_distinctValues = view1.ToTable(true, "branch_id");
                            tbdisTocmb = view1.ToTable(true, "branch_id");

                            cmb_col_to.DisplayMember = "branch_id";
                            cmb_col_to.ValueMember = "branch_id";
                            cmb_col_to.DataSource = tbdisTocmb;

                            cmb_col_from.DisplayMember = "branch_id";
                            cmb_col_from.ValueMember = "branch_id";
                            cmb_col_from.DataSource = tb_distinctValues;

                            lblSuccess_cc.Text = Convert.ToString(tbGRD_col.Tables[1].Rows.Count);
                        }
                        //JList.Add(Jbatch_c);
                        //update_JList.Add(SageACC_Dis_List.Remove(SageACC_Dis_List.Length - 1));

                    }
                    else { MessageBox.Show("Data not found!"); }
                }
            }
            #endregion
        }

        public void colListError()
        {
            #region  New Code ---Branch wise--------


            dynamic GL = new ExpandoObject();
            var obj = (IDictionary<string, object>)GL;

            JList = new List<ent_GLJBtch>();
            update_JList = new List<string>();

            DataTable dtnew = new DataTable();
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlCommand cmd;
            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
            conn.Open();
            string Querystring = " select count(*) TotalCount from " + THSERVERCOLL + "  where  GLBatch_No is null and " +
                   " Convert(date, Received_Date, 100) = '" + dateTimePicker3.Text + "' ;" +
                   " Select id, d.branch_id, d.loanid,'LNMSTR' sage_acctid,d.sageacct_branchcash,d.received_Amt,d.Received_Date,d.ClientName,d.Receiptid,d.sage_credit_gl,d.[type] from " + THSERVERCOLL + " d  " +
                  // " inner join " + THSERVERCOLL + " d on d.loanid=right('0000000'+left(l.loan_id,8),8) " +
                   " where d.GLBatch_No is null and Convert(date,d.Received_Date,100)='" + dateTimePicker3.Text + "' ";
            cmd = new System.Data.SqlClient.SqlCommand(Querystring, conn);
            cmd.CommandTimeout = 180;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
            {
                var objerrList = (IDictionary<string, object>)person;
                cmd.Connection = conn;
                sda.SelectCommand = cmd;
                using (tbGRD_col = new DataSet())
                {
                    sda.Fill(tbGRD_col);
                    if (tbGRD_col.Tables[1].Rows.Count > 0)
                    {
                        lblCollTotal.Text = tbGRD_col.Tables[0].Rows[0]["TotalCount"].ToString();
                        dataGridView1.DataSource = tbGRD_col.Tables[1];
                        dataGridView1.EnableHeadersVisualStyles = false;
                        dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
                        dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                       // dataGridView1.Columns[].DefaultCellStyle.BackColor = Color.Red;
                        // dgw_deposit.ColumnHeadersHeight = 50;
                        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                        dataGridView1.ColumnHeadersHeight = 30;
                        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                        dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
                        DataView viewSlot = new DataView(tbGRD_col.Tables[1]);

                        if (tbGRD_col.Tables[1].Rows.Count > 0)
                        {

                            DataView view1 = new DataView(tbGRD_col.Tables[1]);
                            view1.Sort = "branch_id asc";
                            DataTable tb_distinctValues = view1.ToTable(true, "branch_id");
                            tbdisTocmb = view1.ToTable(true, "branch_id");

                            cmb_col_to.DisplayMember = "branch_id";
                            cmb_col_to.ValueMember = "branch_id";
                            cmb_col_to.DataSource = tbdisTocmb;

                            cmb_col_from.DisplayMember = "branch_id";
                            cmb_col_from.ValueMember = "branch_id";
                            cmb_col_from.DataSource = tb_distinctValues;

                            lblSuccess_cc.Text = Convert.ToString(tbGRD_col.Tables[1].Rows.Count);
                        }
                        //JList.Add(Jbatch_c);
                        //update_JList.Add(SageACC_Dis_List.Remove(SageACC_Dis_List.Length - 1));

                    }
                    else { MessageBox.Show("Data not found!"); }
                }
            }
            #endregion
        }

        private void btnGoC_Click(object sender, EventArgs e)
        {
            ojbLog.WriteLog(logfilename,"---------------Start Validation Date Time " + System.DateTime.Now.ToString() + "---------------");
            btnGoC.Enabled = false;
            colList();
            btnGoC.Enabled = true;
             lblCol_messege.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)    ///Collection upload controlll.......
        {
            logfilename = "";
            logfilename = System.DateTime.Now.ToString();
            logfilename = logfilename.Replace(" ", "-");
            logfilename = logfilename.Replace(":", "");
            logfilename = logfilename.Replace("/", "-");
            Coll_LogFile = @"Logs/Log- " + logfilename + ".txt";
            lblCol_messege.Text = "Please wait .....";

            button1.Enabled = false;
            string strid = "";
            string SageACid_c = "";
            ent_GLHeader header_c;
            ent_GLDetail detail_c;
            List<ent_GLDetail> lstDet_c;
            lblCol_messege.Text = "Please wait .....";

            
            #region
            string strFilter = "branch_id >=" + cmb_col_from.SelectedValue.ToString() + " And  branch_id<= " + cmb_col_to.SelectedValue.ToString() + "";
            DataTable tb_slot = tbdisTocmb.Select(strFilter).CopyToDataTable();
            Jbatch_c = new ent_GLJBtch();
            lstHeader_c = new List<ent_GLHeader>();
            Jbatch_c.UpdateOperation = "Unspecified";
            Jbatch_c.Description ="CC-"+DateTime.Now.ToString("MM/dd/yyyy") ;
            SageACC_Dis_List = "";
            int totalpasscount = 0;
            if (tb_slot.Rows.Count > 0)
            {
                foreach (DataRow rw in tb_slot.Rows)
                {
                    string BranchId_c = rw["branch_id"].ToString();
                    ojbLog.WriteLog(logfilename, "In Process Branch_ID=" + BranchId_c);
                    string fff = "branch_id='" + BranchId_c + "'";
                    DataRow[] rowsFilteredSorting = tbGRD_col.Tables[1].Select(fff);
                    if (rowsFilteredSorting.Count() > 0)
                    {
                        Boolean Debit_Entry = false;
                        header_c = new ent_GLHeader();
                        TotalAmt_c = 0;
                        int delCount = 0;
                        string loanid_cl = "";
                        lstDet_c = new List<ent_GLDetail>();
                        decimal amt_c;
                        amt_c = 0;
                        string branchid_c = "";
                        string TransType_c = "";
                        string ClientName_c = "";
                        string ReceiptId_c = "";
                        string sage_credit_gl = "";
                        string sage_credit_gl_status = "TRUE";
                        foreach (DataRow VDrw in rowsFilteredSorting)
                        {                            
                            strid = VDrw["id"].ToString();
                            branchid_c = VDrw["branch_id"].ToString();
                            TransType_c = VDrw["tYPE"].ToString();
                            ClientName_c = VDrw["ClientName"].ToString();
                            sage_accid_c = VDrw["sage_acctid"].ToString();  //gl AC of loanid
                            sage_credit_gl = VDrw["sage_credit_gl"].ToString();  //credit gl account in case loan account is not credited

                          //  MessageBox.Show(string.IsNullOrEmpty(sage_credit_gl).ToString() + sage_credit_gl);
                            if (string.IsNullOrEmpty(sage_credit_gl) == false)
                            {
                                if (getaccountid_statusCol(sage_credit_gl) == false)
                                {
                                    ojbLog.WriteLog(logfilename, "Not Exist in Sage! sage_credit_gl -" + sage_credit_gl);
                                    sage_credit_gl_status = "False";
                                }
                                else
                                {
                                    sage_credit_gl_status = "TRUE";
                                    sage_accid_c = sage_credit_gl;
                                    ojbLog.WriteLog(logfilename, "sage_credit_gl -" + sage_credit_gl);
                                }

                            }

                            // ojbLog.WriteLog(logfilename,"validating sage_sageAccid id-" + SageACid_c);
                            if (sage_credit_gl_status == "TRUE")
                            {
                                loanid_cl = VDrw["loan_id"].ToString();
                                ojbLog.WriteLog(logfilename, "Cash collection for Loanid -" + loanid_cl);
                                SageACid_c = VDrw["sageacct_branchcash"].ToString().Trim();
                                if (delCount == 0)
                                {
                                    //Entering header details
                                    header_c.SourceType = "CC";
                                    header_c.Description = "CC-" + VDrw["Received_Date"].ToString() + "-" + VDrw["branch_id"].ToString();
                                    header_c.JournalDetails = lstDet_c;
                                    header_c.PostingDate = dateTimePicker3.Text + "T00:00:00Z";
                                    header_c.DocumentDate = dateTimePicker3.Text + "T00:00:00Z"; 
                                    lstHeader_c.Add(header_c);
                                }
                                delCount++;

                                ojbLog.WriteLog(logfilename, "json created for loan account -" + sage_accid_c);
                                amt_c = Convert.ToDecimal(VDrw["Received_Amt"].ToString());
                                detail_c = new ent_GLDetail();
                                detail_c.SourceType = "CC";   // Detail source type T1 for crediting Tax liability IGST account
                                detail_c.Reference = VDrw["id"].ToString() + "-" + loanid_cl;
                                // detail.TaxAuthority = "07IGN";
                                detail_c.Description = TransType_c;//VDrw["ClientName"].ToString();
                                detail_c.AccountNumber = sage_accid_c.Trim();
                                detail_c.Amount = amt_c * -1;
                               // detail_c.DateCreated= dateTimePicker3.Text + "T00:00:00Z";
                                //Receiptid
                                TotalAmt_c = TotalAmt_c + amt_c;
                                lstDet_c.Add(detail_c);
                                Debit_Entry = true;
                                string ss = "'" + strid + "'";
                                SageACC_Dis_List = ss + "," + SageACC_Dis_List;
                                //ReceiptId_c = VDrw["Receiptid"].ToString() +"-"+ReceiptId_c;
                                totalpasscount++;
                                //}
                                // else
                                // {
                                // ojbLog.WriteLog(logfilename,"Sage  account does not exist in loan master or in sage" + sage_sageAccid);                        
                            }
                        }//End foreach

                        if (Debit_Entry == true)
                        {

                            detail_c = new ent_GLDetail();
                            detail_c.SourceType = "CC";   // Detail source type T1 for crediting Tax liability IGST account
                            detail_c.Reference = strid;
                            // detail.TaxAuthority = "07IGN";
                            detail_c.Description = "Cash Collection";
                            detail_c.AccountNumber = SageACid_c.Trim();
                            detail_c.Amount = TotalAmt_c;
                          //  detail_c.DateCreated = dateTimePicker3.Text + "T00:00:00Z";
                            lstDet_c.Add(detail_c);
                        }
                        //GLBatchEntry_c(SageACid_c);
                        Jbatch_c.JournalHeaders = lstHeader_c;
                    }
                }
                lblfilter_c.Text = Convert.ToString(totalpasscount);
                #endregion

                ojbLog.WriteLog(logfilename, "---------------Start API Date Time " + System.DateTime.Now.ToString() + "---------------");
                lblCol_messege.Text = "Waiting for API response....";
                dynamic newCustomer = POSTData(Jbatch_c, "http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/GL/GLJournalBatches");
                if (newCustomer == "ERROR" || newCustomer == "conflic" || newCustomer == "Bad Request")
                {
                    MessageBox.Show("GL Entry creation API Response failed.... " + newCustomer);
                }
                else
                {
                    dynamic deserialized = JsonConvert.DeserializeObject(newCustomer.ToString());
                    try
                    {
                        var rESPONSE = deserialized["JournalHeaders"];
                        dynamic des = JsonConvert.DeserializeObject(rESPONSE.ToString());
                        str_batchno = des[0].BatchNumber.ToString();
                        lblCol_messege.Text = "Batch Number:" + str_batchno;
                        string EntryNumber = des[0].EntryNumber.ToString();
                        ojbLog.WriteLog(logfilename, "GL Entry creation successfull....GLBatchNumbr=" + str_batchno);
                        ojbLog.WriteLog(logfilename, "GLBatchEntry Updating......" + SageACC_Dis_List);
                        SageACC_Dis_List = SageACC_Dis_List.Remove(SageACC_Dis_List.Length - 1);
                        //string strupList = update_JList[s].ToString();
                        UpdateCollMaster(str_batchno, SageACC_Dis_List);
                        MessageBox.Show("GL Entry creation successful....GLBatchNumbr = " + str_batchno);
                        ojbLog.WriteLog(logfilename, "GLBatch Entry No updated in disbursement header...GLBatchNumbr=" + str_batchno);



                    }
                    catch (Exception Err)
                    {
                        MessageBox.Show(deserialized);
                        ojbLog.WriteLog(logfilename, "GLBatchEntry function failed......" + Err.Message.ToString());
                    }
                }
            }
            ojbLog.WriteLog(logfilename,"---------------End API Date Time " + System.DateTime.Now.ToString() + "---------------");

            ojbLog.WriteLog(logfilename, "---------------End  Date Time " + System.DateTime.Now.ToString() + "---------------");
            colList();
            button1.Enabled = true;
            lblCol_messege.Text = "";
        }

        public Boolean UpdateCollMaster(string batchEntryNo, string sageDisAccid)
        {
            Boolean retrnBatchEntryNo = false;
            try
            {
                // ojbLog.WriteLog(logfilename,"UpdateLoanMaster  Loan Id List" + sageDisAccid);
                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.SqlClient.SqlConnection connR;
                System.Data.SqlClient.SqlCommand cmdR;
                connR = new System.Data.SqlClient.SqlConnection(connectionstring);
                connR.Open();
                string Querystring1 = " update " + THSERVERCOLL + " set GLBatch_No='" + batchEntryNo + "'   where Convert(date,Received_Date,100) ='" + dateTimePicker3.Text + "' and  id in (" + sageDisAccid + ")";
                cmdR = new System.Data.SqlClient.SqlCommand(Querystring1, connR);
                cmdR.CommandTimeout = 180;
                cmdR.CommandType = CommandType.Text;
                int res = cmdR.ExecuteNonQuery();
                ojbLog.WriteLog(logfilename,"Update loan master Status:" + res);
                if (res >= 1)
                    retrnBatchEntryNo = true;
                if (connR.State == System.Data.ConnectionState.Open)
                    connR.Close();
            }
            catch (Exception Err)
            {
                ojbLog.WriteLog(logfilename,"UpdateLoanMaster function failed......" + Err.Message.ToString());
            }
            return retrnBatchEntryNo;
        }

        public Boolean getaccountid_statusCol(string sageAC_gl)
        {
            Boolean returnvalue = false;
            try
            {
                System.Data.SqlClient.SqlConnection connR;
                System.Data.SqlClient.SqlCommand cmdR;
                String sQueryDeff;
                DataTable dtCheckamt;
                sQueryDeff = "";
                dtCheckamt = new DataTable();
                sQueryDeff = "Select * from [" + SAGEDB + "].dbo.GLAMF where ACCTFMTTD = '" + sageAC_gl.Trim() + "'";
                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
                connR = new System.Data.SqlClient.SqlConnection(connectionstring);
                connR.Open();
                cmdR = new System.Data.SqlClient.SqlCommand(sQueryDeff, connR);
                cmdR.CommandTimeout = 180;
                cmdR.CommandType = CommandType.Text;
                sda.SelectCommand = cmdR;
                sda.Fill(dtCheckamt);
                if (dtCheckamt.Rows.Count > 0)
                {
                    returnvalue = true;
                    ojbLog.WriteLog(logfilename, "GL Credit Account exist in Sage.....");
                }
                else
                 ojbLog.WriteLog(logfilename, "GL Credit Account not exist in Sage.....");

            }
            catch (Exception ex)
            {
                returnvalue = false;
                ojbLog.WriteLog(logfilename, "Getaccountid_status_col function failed..." + ex.Message);
            }
            return returnvalue;
        }


        #endregion

        #region Branch cash Deposit in bank--------------------------------------
        private void btnDeposit_Click(object sender, EventArgs e)
        {
            ojbLog.WriteLog(logfilename,"---------------Start Deposit Date Time " + System.DateTime.Now.ToString() + "---------------");
            dipositList();
            ojbLog.WriteLog(logfilename,"---------------End Deposit Date Time " + System.DateTime.Now.ToString() + "---------------");
            lblDep_messege.Text = "";
        }
        public void dipositList()
        {
            #region  New Code     
            lblDep_messege.Text = "Please wait .......";

            DataTable dtnew = new DataTable();
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlCommand cmd;
            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
            conn.Open();
            string Querystring = " select id ,RTRIM(ds.sageacct_branchcash) as sageacct_branchcash,RTRIM(ds.sageid_bank) as sageid_bank,RTRIM(ds.Bank_name) as Bank_name,RTRIM(ds.Amount) as Amount,RTRIM(ds.DepositDate) as DepositDate,RTRIM(ds.branch_id) as branch_id,RTRIM(ds.sageid_branchcash) as sageid_branchcash ,RTRIM(ds.sageacct_bank) as sageacct_bank ,RTRIM(ds.branchName) as  branchName from  " + THSERVERDEP + "   ds  " +
                   " inner join " + SAGEDB + ".dbo.GLAMF s on ds.sageacct_branchcash = s.ACCTFMTTD   inner join   " + SAGEDB + ".dbo.BKACCT bk on bk.BANK=ds.sageid_bank " +
                   " where s.ACTIVESW=1 and ds.Amount <>0  and ds.sageacct_branchcash IS NOT NULL and ds.BKEntry_no is null and ds.depositDate='" + dtp_deposit.Text + "' ";

            cmd = new System.Data.SqlClient.SqlCommand(Querystring, conn);
            cmd.CommandTimeout = 180;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
            {
                var objerrList = (IDictionary<string, object>)person;
                cmd.Connection = conn;
                sda.SelectCommand = cmd;
                using (tbGRD_deposit = new DataTable())
                {
                    sda.Fill(tbGRD_deposit);
                    if (tbGRD_deposit.Rows.Count > 0)
                    {
                        lblDepTotal.Text = Convert.ToString(tbGRD_deposit.Rows.Count);
                        //lblTotalrowcount.Text = tbGRD.Rows.Count.ToString();
                        //lblslotcount.Text = slotSize.ToString();
                        // dgw_deposit.AutoGenerateColumns = false;
                        dgw_deposit.DataSource = tbGRD_deposit;
                        dgw_deposit.EnableHeadersVisualStyles = false;
                        dgw_deposit.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
                        dgw_deposit.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                        // dgw_deposit.ColumnHeadersHeight = 50;
                        dgw_deposit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                        dgw_deposit.ColumnHeadersHeight = 30;
                        dgw_deposit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                        dgw_deposit.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
                    }
                    else { MessageBox.Show("Data not found!"); }
                }
            }


            #endregion
        }

        public void dipositErrorList()
        {
            #region  New Code     
            lblDep_messege.Text = "Please wait .......";

            DataTable dtnew = new DataTable();
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlCommand cmd;
            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
            conn.Open();
            string Querystring = " select id ,RTRIM(ds.sageacct_branchcash) as sageacct_branchcash,RTRIM(ds.sageid_bank) as sageid_bank,RTRIM(ds.Bank_name) as Bank_name,RTRIM(ds.Amount) as Amount,RTRIM(ds.DepositDate) as DepositDate,RTRIM(ds.branch_id) as branch_id,RTRIM(ds.sageid_branchcash) as sageid_branchcash ,RTRIM(ds.sageacct_bank) as sageacct_bank ,RTRIM(ds.branchName) as  branchName from  " + THSERVERDEP + "   ds  " +
                               " where ds.BKEntry_no is null and ds.depositDate='" + dtp_deposit.Text + "' ";

            cmd = new System.Data.SqlClient.SqlCommand(Querystring, conn);
            cmd.CommandTimeout = 180;
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();
            using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
            {
                var objerrList = (IDictionary<string, object>)person;
                cmd.Connection = conn;
                sda.SelectCommand = cmd;
                using (tbGRD_deposit = new DataTable())
                {
                    sda.Fill(tbGRD_deposit);
                    if (tbGRD_deposit.Rows.Count > 0)
                    {
                        lblDepTotal.Text = Convert.ToString(tbGRD_deposit.Rows.Count);
                        //lblTotalrowcount.Text = tbGRD.Rows.Count.ToString();
                        //lblslotcount.Text = slotSize.ToString();
                        // dgw_deposit.AutoGenerateColumns = false;
                        dgw_deposit.DataSource = tbGRD_deposit;
                        dgw_deposit.EnableHeadersVisualStyles = false;
                        dgw_deposit.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
                        dgw_deposit.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                        // dgw_deposit.ColumnHeadersHeight = 50;
                        dgw_deposit.Columns[1].DefaultCellStyle.BackColor = Color.Red;
                        dgw_deposit.Columns[2].DefaultCellStyle.BackColor = Color.Red;
                        dgw_deposit.Columns[8].DefaultCellStyle.BackColor = Color.Red;
                        dgw_deposit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                        dgw_deposit.ColumnHeadersHeight = 30;
                        dgw_deposit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                        dgw_deposit.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
                    }
                    else { MessageBox.Show("Data not found!"); }
                }
            }


            #endregion
        }

        private void btnUploaddeposit_Click(object sender, EventArgs e)
        {
            logfilename = "";
            logfilename = System.DateTime.Now.ToString();
            logfilename = logfilename.Replace(" ", "-");
            logfilename = logfilename.Replace(":", "");
            logfilename = logfilename.Replace("/", "-");
            ColDiposit_LogFile = @"Logs/Log- " + logfilename + ".txt";
            ojbLog.WriteLog(logfilename,"---------------Start Deposit API Process Date Time " + System.DateTime.Now.ToString() + "---------------");
            lblDep_messege.Text = "Please wait .......";
            SageACC_Dis_List = "";
            DataView view = new DataView(tbGRD_deposit);
            DataTable tb_distinctValues = view.ToTable(true, "sageid_bank"); //grouping with sageid_bank
            ojbLog.WriteLog(logfilename,tbGRD_deposit.Rows.Count.ToString());
            string successResult = "";
            int totSucCount = 0;
            foreach (DataRow rw in tb_distinctValues.Rows)
            {
               
                string sageacct_branchcash_list = "";
                string sageid_bank = rw["sageid_bank"].ToString();

                ojbLog.WriteLog(logfilename,"In Process Account number=" + sageid_bank);
                string BankCode = "sageid_bank='" + sageid_bank + "'";
                DataRow[] rowsFilteredSorting = tbGRD_deposit.Select(BankCode);

                ent_BKdetln bk;
                List<ent_BKdetln> objBn = new List<ent_BKdetln>();
                int lineNumber;
                int lineNumberH=1;
                lineNumber = 1;
                //obj.Add("BankCode", sageid_bank);
                //obj.Add("BankEntryDate", DateTime.Now);
                //obj.Add("BankEntryType", "Deposits");
                //obj.Add("EntryDescription", rw["Received_Date"].ToString() + "-BCHID-" + sageid_bank);
                string strBK_entry = "";
                dynamic BK = new ExpandoObject();
                var obj = (IDictionary<string, object>)BK;
                foreach (DataRow BKrw in rowsFilteredSorting)
                {
                    totSucCount++;
                    ojbLog.WriteLog(logfilename, "Row Number-"+ totSucCount);
                    if (lineNumberH==1)
                    {
                        obj.Add("BankCode", sageid_bank);
                        obj.Add("BankEntryDate", dtp_deposit.Text+ "T00:00:00Z");
                        obj.Add("DateCreated", dtp_deposit.Text + "T00:00:00Z");
                        obj.Add("BankEntryType", "Deposits");
                        obj.Add("EntryDescription", "CD-"+BKrw["DepositDate"].ToString() + "-" + BKrw["branch_id"].ToString());
                       
                    }
                    lineNumberH++;
                    bk = new ent_BKdetln();
                    bk.LineNumber = lineNumber * -1;
                    bk.Reference = (BKrw["sageid_branchcash"].ToString().Trim() + "/" + BKrw["sageid_bank"].ToString().Trim());
                    bk.Description = BKrw["Bank_name"].ToString().Trim();
                    bk.Comments = (BKrw["Bank_name"].ToString().Trim() + "/" + BKrw["branch_id"].ToString().Trim());
                    bk.SourceAmount = Convert.ToDecimal(BKrw["Amount"]);
                    bk.GLAccount = BKrw["sageacct_branchcash"].ToString().Trim();
                    objBn.Add(bk);
                   // lineNumber = lineNumber + 1;
                    string ss = "'" + BKrw["id"].ToString().Trim() + "'";
                    sageacct_branchcash_list = ss + "," + sageacct_branchcash_list;
                    
                }
                obj.Add("BankEntryDetail", objBn);
                dynamic newCustomer = POSTData(obj, "http://localhost/Sage300WebApi/v1.0/-/" + SAGEDB + "/BK/BKBankEntries");
                dynamic deserialized = JsonConvert.DeserializeObject(newCustomer.ToString());
                try
                {
                    BankEntryNumber = deserialized.BankEntryNumber;
                    lblDep_messege.Text = "Proccess BankEntryNumber="+ BankEntryNumber;
                    UpdateDepositMaster(BankEntryNumber, sageid_bank, sageacct_branchcash_list);
                    ojbLog.WriteLog(logfilename,"Bank entry created-Entry No......." + BankEntryNumber);
                    string sss = " BankEntry Number: " + BankEntryNumber + "  Line Number: " + lineNumber ;

                    strBK_entry = sss;//sss + Environment.NewLine + strBK_entry;


                }
                catch (Exception)
                {
                    ojbLog.WriteLog(logfilename,"Bank Method, Some thing wrong......." + deserialized);
                }
                
                successResult = strBK_entry + Environment.NewLine + "Total Success Count-"+ totSucCount;
            }
            lblDep_messege.Text = "";
            if(successResult=="")
            MessageBox.Show("Validated Data not found. ", "Error Details..!!!");
            else
                MessageBox.Show(successResult, "Successfully Inserted Bank Entry!!!");
            dipositErrorList();
            lblDep_messege.Text = "";
            ojbLog.WriteLog(logfilename,"---------------End Deposit API Process Date Time " + System.DateTime.Now.ToString() + "---------------");
        }

        private void button2_Click(object sender, EventArgs e)  //without validated list
        {
            colListError();
        }

        public Boolean UpdateDepositMaster(string bankEntryNo, string sageid_bank, string sageacct_branchcash_List)
        {
            Boolean retrnBatchEntryNo = false;
            try
            {
                sageacct_branchcash_List = sageacct_branchcash_List.Remove(sageacct_branchcash_List.Length - 1);
                ojbLog.WriteLog(logfilename,"UpdateLoanMaster  Loan Id List" + hdSeqNo);
                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
                System.Data.SqlClient.SqlConnection connR;
                System.Data.SqlClient.SqlCommand cmdR;
                connR = new System.Data.SqlClient.SqlConnection(connectionstring);
                connR.Open();
                ojbLog.WriteLog(logfilename,"sageacct_branchcash_List-:" + sageacct_branchcash_List);
                string Querystring1 = " update " + THSERVERDEP + " set BKEntry_no='" + bankEntryNo + "'   where  id in (" + sageacct_branchcash_List + ") and  Convert(date,DepositDate,100) ='" + dtp_deposit.Text + "' and  sageid_bank ='" + sageid_bank + "'";
                cmdR = new System.Data.SqlClient.SqlCommand(Querystring1, connR);
                cmdR.CommandTimeout = 180;
                cmdR.CommandType = CommandType.Text;
                int res = cmdR.ExecuteNonQuery();
                ojbLog.WriteLog(logfilename,"Update loan master Status:" + res);
                if (res >= 1)
                    retrnBatchEntryNo = true;
                if (connR.State == System.Data.ConnectionState.Open)
                    connR.Close();
            }
            catch (Exception Err)
            {
                ojbLog.WriteLog(logfilename,"UpdateLoanMaster function failed......" + Err.Message.ToString());
            }
            return retrnBatchEntryNo;
        }
        #endregion

        #region Common Method--------------------------------------------------------
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dd = lblsuccess_c.SelectedIndex.ToString();
            if (dd == "0")
            {
                dataGridView1.DataSource = null;
                dgw_deposit.DataSource = null;
            }
            else
                if (dd == "1")
            {
                dgv_LNDISBH.DataSource = null;
                dgw_deposit.DataSource = null;
            }
            else if (dd == "2")
            {
                dataGridView1.DataSource = null;
                dgv_LNDISBH.DataSource = null;
            }
        }
        public void CredentialsXml()
        {
            if (!System.IO.File.Exists("LOANCRD.xml"))
            {
                XmlTextWriter writer = new XmlTextWriter(@"ValocisDetCRD.xml", System.Text.Encoding.UTF8);
                writer.WriteStartDocument(false);
                writer.Formatting = System.Xml.Formatting.Indented;
                writer.Indentation = 2;
                writer.WriteStartElement("dbconfig");
                writer.WriteStartElement("SERVERNAME");
                writer.WriteString(".");
                writer.WriteEndElement();
                writer.WriteStartElement("USERNAME");
                writer.WriteString("ADMIN");
                writer.WriteEndElement();
                writer.WriteStartElement("PASSWORD");
                writer.WriteString("ADMIN");
                writer.WriteEndElement();
                writer.WriteStartElement("SAGEDB");
                writer.WriteString("GSTMAS");
                writer.WriteEndElement();
                writer.WriteStartElement("SAA");
                writer.WriteString("sa");
                writer.WriteEndElement();
                writer.WriteStartElement("SAPSS");
                writer.WriteString("Erp#12345");
                writer.WriteEndElement();
                writer.WriteStartElement("THSERVERMSTR");
                writer.WriteString("[LMSSERVER].MICROFINANCE.dbo.rtgs_disbursement_master");
                writer.WriteEndElement();
                writer.WriteStartElement("THSERVERDETS");
                writer.WriteString("[LMSSERVER].MICROFINANCE.dbo.rtgs_disbursement_Details");
                writer.WriteEndElement();
                writer.WriteStartElement("THSERVERCOLL");
                writer.WriteString("[LMSSERVER].MICROFINANCE.dbo.rtgs_disbursement_Details");
                writer.WriteEndElement();
                writer.WriteStartElement("THSERVERDEP");
                writer.WriteString("[LMSSERVER].MICROFINANCE.dbo.rtgs_disbursement_Details");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();

            }

            ReadWriteXML xml1 = new ReadWriteXML();
            bool conStatus = xml1.ReadXML();
            if (conStatus == true)
            {
                SERVERNAME = xml1.SERVERNAME;
                USERNAME = xml1.USERNAME;
                PASSWORD = xml1.PASSWORD;
                SAGEDB = xml1.SAGEDB;
                SAA = xml1.SAA;
                SAPSS = xml1.SAPSS;
                THSERVERMSTR = xml1.THSERVERMSTR;
                THSERVERDETS = xml1.THSERVERDETS;
                THSERVERCOLL = xml1.THSERVERCOLL;
                THSERVERDEP = xml1.THSERVERDEP;
                if (THSERVERMSTR == "EMPTY")
                    THSERVERMSTR = "LNDSDB.dbo.disbursement_Master";
                if (THSERVERDETS == "EMPTY")
                    THSERVERDETS = "LNDSDB.dbo.disbursement_details";
                // if (THSERVERCOLL == "EMPTY")
                //THSERVERCOLL = "LNDSDB.dbo.disbursement_details";
            }
        }

        private void btnlogview_Click(object sender, EventArgs e)
        {
            string path = System.IO.Path.GetFullPath(Disb_LogFile);
            if (System.IO.File.Exists(path)==true)
            System.Diagnostics.Process.Start(path);
           
        }

        private void btnDepositLogview_Click(object sender, EventArgs e)
        {
            string path = System.IO.Path.GetFullPath(ColDiposit_LogFile);
            if (System.IO.File.Exists(path) == true)
                System.Diagnostics.Process.Start(path);
        }

        private void btnCollLogView_Click(object sender, EventArgs e)
        {
            string path = System.IO.Path.GetFullPath(Coll_LogFile);
            if (System.IO.File.Exists(path) == true)
                System.Diagnostics.Process.Start(path);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        public object POSTData(object json, string url)
        {
            object returnValue = null;
            try
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8, "application/json"))
                {
                  ojbLog.WriteLog(logfilename,JsonConvert.SerializeObject(json));
                    using (var httpClientHandler = new HttpClientHandler { Credentials = new NetworkCredential(USERNAME, PASSWORD) })

                    using (var _httpClient = new HttpClient(httpClientHandler))
                    {
                        _httpClient.BaseAddress = new Uri(url);
                        HttpResponseMessage result = _httpClient.PostAsync(url, content).Result;
                        if (result.StatusCode == System.Net.HttpStatusCode.OK || result.StatusCode == System.Net.HttpStatusCode.Created)
                        {
                           // ojbLog.WriteLog(logfilename,result.ToString());
                            returnValue = result.Content.ReadAsStringAsync().Result;
                            dynamic deserialized = JsonConvert.DeserializeObject(returnValue.ToString());
                        }
                        else if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            returnValue = "Conflict";
                            ojbLog.WriteLog(logfilename,result.ToString()); ;
                        }
                        else
                        {
                            returnValue = "ERROR";
                            ojbLog.WriteLog(logfilename,result.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            { ojbLog.WriteLog(logfilename,"POSTData  Date" + System.DateTime.Now.ToString("MM-dd-yyyy") + ex.Message); }
            return returnValue;
        }
        #endregion


    }
}
