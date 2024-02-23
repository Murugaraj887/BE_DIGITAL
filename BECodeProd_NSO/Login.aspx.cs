using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BEData.BusinessEntity;
using System.IO;
using BEData;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


    public partial class Login : BasePage
    {

        private BEDL service = new BEDL();
        public string fileName = "BEData.Login";
        Logger logger = new Logger();
        DataSet dsusers = new DataSet();
     
        //bool isCurrentQTobeShown = true;
        //bool isNextQTobeShown = true;

        public class EntityEqualityComparer : IEqualityComparer<KeyValues>
        {

            public bool Equals(KeyValues x, KeyValues y)
            {
                return x.Key == y.Key;
            }

            public int GetHashCode(KeyValues obj)
            {
                return 0;
            }
        }

         protected void Page_Load(object sender, EventArgs e)
        {
       


            try
            {
                string status = service.CheckStatus();

                if (status != "Active")
                {
                  // Response.Redirect("Process.htm");
                }

                if (!Page.IsPostBack)
                {
                   
                    //freezing logic
                    Session.Clear();
                    Session["Login"] = "1";
                    string userid = HttpContext.Current.User.Identity.Name;

                    
                    string[] userids = userid.Split('\\');
                    if (userids.Length == 2)
                    {
                        userid = userids[1];
                    }

                   
                   
                    Session["UserID"] = userid;
                    Session["LoggedInUserID"] = userid;

                    string role = service.GetUserRole(userid);
                   
                    Session["LoginRole"] = role + "";
                    Session["Role"] = role;
                    Session["IsAdmin"] = "N";

                    string isAdmin = service.GetIsAdmin(userid);

                    string SDMorDM = "SELECT [txtDMorSDM] FROM [BEUserAccess_NSO] WHERE [txtUserId]='" + userid + "'";
                    DataSet dssdmordm = service.GetDataSet(SDMorDM);
                    DataTable dtsdmordm = dssdmordm.Tables[0];
                    
                    if (dtsdmordm.Rows.Count > 0)
                    {
                        Session["SDMorDM"] = dtsdmordm.Rows[0][0].ToString();
                    }
                    else
                    {
                        Response.Redirect("noaccess.html");
                    }

                    if (Request.QueryString["Subcon"] != null)
                    {
                        Response.Redirect("SubConHome.aspx");
                    }
                    string machineUser = string.Empty;
                    string[] machineUsers = HttpContext.Current.User.Identity.Name.Split('\\');
                    if (machineUsers.Length == 2)
                        machineUser = machineUsers[1];
                    string machineRole = service.GetUserRole(machineUser); //Machine User
                    if (Request.QueryString["Report"] == "1")
                    {
                        
                       
                            string txtDMorSDM = Session["SDMorDM"].ToString();

                            if (Session["SDMorDM"].ToString() == "All")
                            {
                                Session["RadioButtonSelected"] = "0";
                            }
                            if (Request.QueryString["tab"] == "menu")
                            {
                                string Report = ConfigurationManager.AppSettings["Report"].ToString();
                                Response.Redirect(Report);
                                //Response.Redirect("/beapp/Summary/BeReportSummary.aspx?#menu1");
                                //Response.Redirect("/Summary/BeReportSummary.aspx?#menu1");
                            }
                            else if (Request.QueryString["tab"] == "summary")
                            {
                                string Summary = ConfigurationManager.AppSettings["Summary"].ToString();
                                Response.Redirect(Summary);
                                //Response.Redirect("/beapp/Summary/BeReportSummary.aspx?#home");
                                //Response.Redirect("/Summary/BeReportSummary.aspx?#home");
                            }
                       
                    }
                    else if (Request.QueryString["Report"] == "2")
                    {
                            Session["MachineRole"] = machineRole;

                            string PageName = Request.QueryString["Page"].ToString();

                            Response.Redirect(PageName);
                        
                    }
                    else if (Request.QueryString["Report"] == "3")
                    {
                        Session["MachineUser"] = machineUser;
                        Response.Redirect("ExchangeRates.aspx?flag=N");
                    }
                    else if (Request.QueryString["Report"] == "4")
                    {
                        Response.Redirect("Beadmin.aspx");
                    }

                    if (role.ToLower() == "admin" || role.ToLower() == "pna")
                    {
                        //TRRole.Style.Add("visibility", "hidden");
                        //TRDel.Style.Add("visibility", "hidden");

                        dsusers = service.GetAllUsers(userid);
                        //lstUsers = lstUsers.OrderByDescending(k => k.Value).ToList().Distinct(new EntityEqualityComparer()).ToList();
                        //lstUsers = lstUsers.OrderBy(k => k.Key).ToList();
                        //ddlEmpList.DataTextField = "Key";
                        //ddlEmpList.DataValueField = "Value";
                        DataTable dtusers = dsusers.Tables[0];
                        ddlEmpList.Items.Clear();
                        for (int i = 0; i < dtusers.Rows.Count; i++)
                        {
                            ddlEmpList.Items.Add(dtusers.Rows[i][0].ToString());
                        }
                        ddlEmpList.Items.Insert(0, new ListItem(userid, "0"));
                        hndValue.Value = userid;

                    }


                    else if (role.ToLower() == "anchor")
                    {
                        if (Session["SDMorDM"].ToString() == "DM")
                        {
                            Response.Redirect("DMView.aspx");
                        }
                        if (Session["SDMorDM"].ToString() == "SDM")
                        {
                            Response.Redirect("SDMView.aspx");
                        }
                        if (Session["SDMorDM"].ToString() == "All")
                        {
                            Response.Redirect("AnchorLogin.aspx");
                        }
                    }

                    if (role.ToLower() == "dm")
                    {
                        Response.Redirect("DMView.aspx");
                    }
                    else if (role.ToLower() == "sdm" || role.ToLower() == "soh" || role.ToLower() == "dh")
                    {
                        Response.Redirect("SDMView.aspx");
                    }

                    else if (role.ToLower() == "uh")
                    {
                        //Server.Execute("Reports.aspx");
                        Response.Redirect("Reports.aspx");
                    }
                    //else
                    //    Response.Redirect("UnAuthorised.aspx");
                }
            }


            catch (Exception ex)
            {

                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }
           

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

             
            try
            {
                string userid = HttpContext.Current.User.Identity.Name;
            
                string[] userids = userid.Split('\\');
                if (userids.Length == 2)
                {
                    userid = userids[1];
                }

                // Session["LoginId"] = userid +"";
                string isAdminLogin = service.GetIsAdmin(userid);

                //string delegatedAnchor = service.GetDelegatedUserRole(userid);
                //string delegatedDMSDM = service.GetDelegatedUserRoleDMSDM(userid);
                string selectedItem = null;
                string RoleForAnchor = null;
                string isAdmin = null;

                //if (delegatedAnchor.ToLower().ToString().Trim() == "delegatedanchor" || delegatedDMSDM.ToLower().ToString().Trim() == "delegateddmsdm")
                //{
                //    selectedItem = ddlEmpList.SelectedItem.ToString();
                //}

                selectedItem = hndValue.Value;
                Session["UserID"] = selectedItem;
                Session["RoleSDM"] = "";
                RoleForAnchor = service.GetUserRole(selectedItem);
                Session["Role"] = RoleForAnchor;
                //    isAdmin = service.GetIsAdmin(selectedItem);

                string role = service.GetUserRole(selectedItem);
                string SDMorDM = "SELECT [txtDMorSDM] FROM [BEUserAccess_NSO] WHERE [txtUserId]='" + selectedItem + "'";
                DataSet dssdmordm = service.GetDataSet(SDMorDM);
                DataTable dtsdmordm = dssdmordm.Tables[0];
                if (dtsdmordm.Rows.Count > 0)
                {
                    Session["SDMorDM"] = dtsdmordm.Rows[0][0].ToString();
                }
              

               
                if (Session["UserID"].ToString().ToLower() == Session["LoggedInUserID"].ToString().ToLower())
                {
                    if (role.ToLower().Trim() == "pna")
                    {
                        Response.Redirect("SDMView.aspx");
                    }
                   Response.Redirect("AnchorLogin.aspx");
                }

                if (role.ToLower().Trim() == "admin" || role.ToLower().Trim() == "pna" || role.ToLower().Trim() == "dh")
                {
                    Response.Redirect("SDMView.aspx");
                }
              
                if (role.ToLower().Trim() == "uh")
                {
                    Response.Redirect("Reports.aspx");
                }

                if (role.ToLower().Trim() == "dm")
                    Response.Redirect("DMView.aspx");
                else if (role.ToLower().Trim() == "sdm" || role.ToLower().Trim() == "soh")
                    Response.Redirect("SDMView.aspx");



                else if (role.ToLower().Trim() == "anchor")
                {
                    if (Session["SDMorDM"].ToString() == "DM")
                    {
                        Response.Redirect("DMView.aspx");
                    }
                    if (Session["SDMorDM"].ToString() == "SDM")
                    {
                        Response.Redirect("SDMView.aspx");
                    }
                    if (Session["SDMorDM"].ToString() == "All")
                    {
                        Response.Redirect("AnchorLogin.aspx");
                    }
                }





               // Response.Redirect("DMVolume.aspx");
            }
            catch (Exception ex)
            {
                if ((ex.Message + "").Contains("Thread was being aborted."))
                    logger.LogErrorToServer(Logger.LoggerType.Info, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                else
                {
                    logger.LogErrorToServer(Logger.LoggerType.Error, fileName, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
                    throw ex;
                }
            }


        }




    }
