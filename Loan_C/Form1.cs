using AccpacCOMAPI;
using AccpacFinder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Loan_C
{
    public partial class Form1 : Form
    {
        private AccpacSession AccSession;
        private AccpacDBLink AccDBlink;
        dynamic person = new ExpandoObject();
        public Dictionary<string, string> errList;
        public Form1()
        {
            
            InitializeComponent();
        }

        private void btnBank_Click(object sender, EventArgs e)
        {
            btnfindinv();
        }
        private void btnfindinv()
        {
            AccSession = new AccpacSession();
            AccSession.Init("", "AS", "AS3001", "65");
             AccSession.Open("ADMIN", "ADMIN", "GSTMAS", DateTime.Today, 0, "");
            //AccSession.Open(USERNAME, PASSWORD, SAGEDB, DateTime.Today, 0, "");
            AccDBlink = AccSession.OpenDBLink(tagDBLinkTypeEnum.DBLINK_COMPANY, tagDBLinkFlagsEnum.DBLINK_FLG_READWRITE);// //OpenDBLink(DBLinkType.Company, DBLinkFlags.ReadWrite,"");
            ViewFinder afinder = new ViewFinder();
            int[] DispArr = new int[10] { 1, 2,3, 4, 5, 6, 7,8,9,10 };  // the array of field IDs that will be displayed in the finder’s columns.
            int[] SearchArr = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };  //the array of field IDs that will be used to search in the finder records.
           // int[] returnval = new int[1] { 1 };
            afinder.Session = AccDBlink.Session;
            afinder.AutoTabAway = true;
            afinder.ViewID = "BK0001";
            //afinder.InitKeyValue = "1200";
            //afinder.InitKeyType =1;
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
                txtbank.Text =  afinder.ReturnFieldValues;
                
            }
            AccSession.Close();
        }

        private void btnupload_Click(object sender, EventArgs e)
        {
            
                btnupload.Enabled = false;
                
                GLJAccountController();
                btnupload.Enabled = true;
            
        }
        public void GLJAccountController()
        {
            DataTable lndisbrh_tbl;
            string connectionstring = "Data Source=WIN-ICD2JD40HJ0\\SQLEXPRESS2012; Initial Catalog=CTLDAT; User ID=sa; Password=Erp#12345;";
            // string connectionstring = "Data Source=" + SERVERNAME + "; Initial Catalog=" + SAGEDB + "; User ID=" + SAA + "; Password=" + SAPSS + ";";
            //MessageBox.Show(connectionstring);
            //constr = "Provider=SQLOLEDB;Data Source=ERP-DATABASE; Initial Catalog=TSTDAT;User ID=sa; Password=Vspl@4321"
            //string connectionstring = "Data Source=ERP-DATABASE; Initial Catalog=TSTDAT; User ID=sa; Password=Vspl@4321;";
            System.Data.SqlClient.SqlConnection conn;
            System.Data.SqlClient.SqlCommand cmd;
            conn = new System.Data.SqlClient.SqlConnection(connectionstring);
            conn.Open();
            string Querystring = "SELECT * FROM CTLOAN.dbo.LNDISBH h where h.disburse_status=1  ";
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
                        foreach (DataRow row in lndisbrh_tbl.Rows)
                        {
                            bool sss = getrunseqNo();
                            if (sss == true)                       //SeqNo Genereated
                            {

                            }
                            else
                            { }                                    //SeqNo Not Genereated
                        }
                    }
                    else {
                        //Data Empty
                    }
                }
            }
            conn.Close();
        }

        public bool getrunseqNo()
        {
            System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
            bool retrn = false;
            DataTable dtRunSeq;
            string connectionstring1 = "Data Source=WIN-ICD2JD40HJ0\\SQLEXPRESS2012; Initial Catalog=CTLOAN; User ID=sa; Password=Erp#12345;";
            //MessageBox.Show(connectionstring);
            //constr = "Provider=SQLOLEDB;Data Source=ERP-DATABASE; Initial Catalog=TSTDAT;User ID=sa; Password=Vspl@4321"
            //string connectionstring = "Data Source=ERP-DATABASE; Initial Catalog=TSTDAT; User ID=sa; Password=Vspl@4321;";
            System.Data.SqlClient.SqlConnection connR;
            System.Data.SqlClient.SqlCommand cmdR;
            connR = new System.Data.SqlClient.SqlConnection(connectionstring1);
            connR.Open();
            string Querystring1 = " INSERT INTO CTLOAN.dbo.LNRUNPR (runseq_no,rundatetime,runtype,status, glbatch_no, glentry_no) VALUES  " +
             "((Select REPLICATE(0,10-LEN(max(convert(int, runseq_no)+1)))+CONVERT(CHAR,(max(convert(int, runseq_no)+1))) from LNRUNPR), " +
            "GETDATE(),1,1,'',''); SELECT * from CTLOAN.dbo.LNRUNPR where runid=SCOPE_IDENTITY() ;";
            int seqNo;
            cmdR = new System.Data.SqlClient.SqlCommand(Querystring1, connR);
            cmdR.CommandTimeout = 180;
            cmdR.CommandType = CommandType.Text;
          //cmdR.ExecuteNonQuery();
          // cmdR.ExecuteScalar();
            //cmdR.Connection = connR;

          //  seqNo = (int)cmdR.ExecuteNonQuery();

           
           // int ff= seqNo;

            sda.SelectCommand = cmdR;
             using (dtRunSeq = new DataTable())
            {
                 sda.Fill(dtRunSeq);
            //     if (dtRunSeq.Rows.Count > 0)
            //    {
            //        foreach (DataRow row in dtRunSeq.Rows)
            //         {
            //             retrn= true;
            //         }
            //     }
            //     else { retrn= false; }
             }
            if (connR.State == System.Data.ConnectionState.Open)
            connR.Close();
            return retrn;
        }
        public Boolean checkamtvalidation(String p_hdSeq, Double p_DefLoanDesc,String Defr)
        {
            Boolean ReturnVlidation;
            try
            {
               
                ReturnVlidation = false;
                System.Data.SqlClient.SqlConnection connR;
                System.Data.SqlClient.SqlCommand cmdR;
                Double strr;
                String sQueryDeff;
                String sQuerySame;
                DataTable dtCheckamt;
                sQueryDeff = "";
                sQuerySame = "";
                dtCheckamt = new DataTable();
                sQueryDeff = "SELECT SUM(TRANS_AMT) + SUM(IGST_AMT) AS TransD FROM CTLOAN.dbo.LNDISBD  WHERE HDSEQ_NO='" + p_hdSeq + "'";
                sQuerySame = "SELECT  SUM(TRANS_AMT) + SUM(SGST_AMT) + SUM(CGST_AMT) AS TransS FROM CTLOAN.dbo.LNDISBD WHERE HDSEQ_NO='" + p_hdSeq + "' ";
                System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter();
               
                string connectionstring1 = "Data Source=WIN-ICD2JD40HJ0\\SQLEXPRESS2012; Initial Catalog=CTLOAN; User ID=sa; Password=Erp#12345;";
               
                connR = new System.Data.SqlClient.SqlConnection(connectionstring1);
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
                { MessageBox.Show("Something Wrong!"); }
                if (dtCheckamt.Rows.Count > 0)
                {
                    if (p_DefLoanDesc == Convert.ToDouble(dtCheckamt.Rows[0]["TransS"])*-1)
                    {
                        ReturnVlidation = true;
                        MessageBox.Show("Amount validation passed...");
                    }
                    else
                    {
                        MessageBox.Show("Amount validation failed....");
                    }
                }
                else { MessageBox.Show("Something wrong..."); }
                
            }
            catch (Exception)
            {

                throw;
            }
            return ReturnVlidation;
        }

        public Boolean getaccountid_status(string sageACId)
        {
            Boolean returnvalue = false;
            return returnvalue;
        }
        public async Task<object> SendRequest(HttpMethod method, string requestUri, object payload = null)
        {
            HttpContent content = null;

            string responsePayload = "";
            // Serialize the payload if one is present
            if (payload != null)
            {
                var payloadString = JsonConvert.SerializeObject(payload);
                content = new StringContent(payloadString, Encoding.UTF8, "application/json");
            }

            // Create the Web API client with the appropriate authentication
            using (var httpClientHandler = new HttpClientHandler { Credentials = new NetworkCredential("ADMIN", "ADMIN") })
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                Console.WriteLine("\n{0} {1}", method.Method, requestUri);

                // Create the Web API request
                var request = new HttpRequestMessage(method, requestUri)
                {
                    Content = content
                };

                // Send the Web API request
                try
                {
                    var response = httpClient.SendAsync(request);
                    responsePayload = response.Result.ToString(); //response.Content.ReadAsStringAsync();

                    var statusNumber = (int)response.Result.StatusCode;
                    Console.WriteLine("\n{0} {1}", statusNumber, response.Result.StatusCode);
                    if (statusNumber == 201)
                    {
                        ///ClearControl();
                    }
                    if (statusNumber < 200 || statusNumber >= 300)
                    {
                        Console.WriteLine(responsePayload);
                        throw new ApplicationException(statusNumber.ToString());
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine("\n{0} Exception caught.", e);
                    Console.WriteLine("\n\nPlease ensure the service root URI entered is valid.");
                    Console.WriteLine("\n\nPress any key to end.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            return string.IsNullOrWhiteSpace(responsePayload) ? null : JsonConvert.DeserializeObject(responsePayload);
        }
    }
}
