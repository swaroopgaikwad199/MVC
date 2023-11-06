using System;
using System.Collections.Generic;
using REDTR.DB.BLL;
using REDTR.DB.BusinessObjects;
using REDTR.UTILS;
using REDTR.UTILS.SystemIntegrity;
using RedUtils;
//using RPM.Base;

namespace RED.Database.Install
{
    public class InitDB
    {
        BLLManager BllMgr;

        // TBD: Get JobTypeAs.DGFT/Etc from Installer. At the time of Installation or Configuration from EXE, user will provide this.
        public InitDB()
        {
            MainFun(false, JobTypeAs.DGFTOnly);
        }
        public InitDB(string StartUpPath)//string provider, string connectionString
        {
            MainFun(false, JobTypeAs.DGFTOnly);
        }
        public InitDB(bool onlyJobTypeOper, JobTypeAs addAs)
        {
            MainFun(onlyJobTypeOper, addAs);
        }
        private bool MainFun(bool onlyJobTypeOper, JobTypeAs addAs)
        {

            if (string.IsNullOrEmpty(REDTR.UTILS.DbConnectionConfig.mDbConfig.Database))
                REDTR.UTILS.DbConnectionConfig.LoadConection();

            if (Globals.CheckSQLDB() == false)
            {
                Console.WriteLine("DATABASE MISSING! \r\nPLEASE CONTACT ADMINISTRATOR");
                Console.ReadLine();
                return false;
            }
            else
            {
                Console.WriteLine("Processing.....");
                //Console.ReadLine();
            }

            BllMgr = new BLLManager(); //provider,connectionString);
            if (onlyJobTypeOper == false)
            {
                loadInfoRoles();

                CreateSetting();
                CreateUser();
                CreatJobType(addAs);
                CreatPackageTypeCode();
                CreatMLNO();
            }
            else
            {
                CreatJobType(addAs);
            }
            return true;
        }
        private void CreateSetting()
        {
            List<Settings> mLst = BllMgr.SettingsBLL.GetSettingss();
            if (mLst.Count > 0)
                return;

            Settings mSetting = new Settings();
            try
            {
                mSetting.Address = RedSysIntegrity.CompanyAddress;
                mSetting.CompanyCode = RedSysIntegrity.CompanyCode; //"8900001";
                mSetting.CompanyName = RedSysIntegrity.GetCompName;
                mSetting.LineCode = "001";
                mSetting.Logo = "";
                mSetting.Remarks = "PHARMA INDUSTRY";
                BllMgr.SettingsBLL.AddSettings(mSetting);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void loadInfoRoles()
        {
            List<Permissions> p = BllMgr.PermissionsBLL.GetPermissionss();
            if (p.Count == 0)
            {
                BllMgr.PermissionsBLL.savePermssion();
            }
            List<Roles> r = BllMgr.RolesBLL.GetRoles();
            if (r.Count == 0)
            {
                BllMgr.RolesBLL.SaveDefaultRoles();
                r = BllMgr.RolesBLL.GetRoles();
            }
            p = BllMgr.PermissionsBLL.GetPermissionss();
            List<ROLESPermission> RP = BllMgr.ROLESPermissionBLL.GetROLESPermissions(ROLESPermissionBLL.ROLESPermissionOp.GetAllROLESPermissions, null);
            if (RP.Count == 0)
            {
                foreach (Roles role in r)
                {
                    foreach (Permissions per in p)
                    {
                        ROLESPermission Rp = null;
                        if (role.Roles_Name.Equals(RoleName.Admin))
                        {
                            Rp = new ROLESPermission();
                            Rp.Roles_Id = (int)role.ID;
                            Rp.Permission_Id = (int)per.ID;
                        }
                        if (role.Roles_Name.Equals(RoleName.Operator)) { }
                        if (role.Roles_Name.Equals(RoleName.Supervisor))
                        {
                            //Permissions.PermissionsList p_list = new Permissions.PermissionsList();
                            //1-Add Project,5-Edit Project,6-print report,7-view report
                            if (per.ID == (int)Permissions.PermissionsList.CanAddProject || per.ID == (int)Permissions.PermissionsList.CanEditProject || per.ID == (int)Permissions.PermissionsList.CanPrintReport || per.ID == (int)Permissions.PermissionsList.CanViewReport)
                            {
                                Rp = new ROLESPermission();
                                Rp.Roles_Id = (int)role.ID;
                                Rp.Permission_Id = (int)per.ID;
                            }
                        }
                        if (role.Roles_Name.Equals(RoleName.QA))
                        {
                            if (per.ID == (int)Permissions.PermissionsList.CanByPassJobs)
                            {
                                Rp = new ROLESPermission();
                                Rp.Roles_Id = (int)role.ID;
                                Rp.Permission_Id = (int)per.ID;
                            }
                        }
                        if (Rp != null)
                            BllMgr.ROLESPermissionBLL.AddROLESPermission(Rp);
                    }
                }
            }
        }
        private void CreateUser()
        {
            List<Roles> mLstrole = BllMgr.RolesBLL.GetRoles();
            Users mUser = null;

            Roles Rl = mLstrole.Find(delegate(Roles o) { return o.Roles_Name == RoleName.Admin; });
            if (Rl != null)
            {
                mUser = new Users();
                mUser.UserName = "ADMINISTRATOR";
                mUser.Password = "a1234567";
                mUser.Active = true;
                mUser.RoleID = Rl.ID;
                BllMgr.UsersBLL.InsertOrUpdateUsers(UsersBLL.UsersOp.AddUser, mUser);
            }

            //Rl = mLstrole.Find(delegate(Roles o) { return o.Roles_Name == RoleName.Supervisor; });
            //if (Rl != null)
            //{
            //    mUser = new Users();
            //    mUser.UserName = "SUPERVISOR";
            //    mUser.Password = "s1234567";
            //    mUser.Active = true;
            //    mUser.RoleID = Rl.ID;
            //    BllMgr.UsersBLL.InsertOrUpdateUsers(UsersBLL.UsersOp.AddUser, mUser);
            //}

            Rl = mLstrole.Find(delegate(Roles o) { return o.Roles_Name == RoleName.QA; });
            if (Rl != null)
            {
                mUser = new Users();
                mUser.UserName = "QA";
                mUser.Password = "q1234567";
                mUser.Active = true;
                mUser.RoleID = Rl.ID;
                BllMgr.UsersBLL.InsertOrUpdateUsers(UsersBLL.UsersOp.AddUser, mUser);
            }
        }

        public enum JobTypeAs
        {
            DGFTOnly,
            DGFTAll,
            Customised_JNJ
        }
        const string XML_Dgft = "";

        private void CreatJobType(JobTypeAs addAs)
        {
            if (addAs == JobTypeAs.DGFTOnly)
            {
                JOBType jbtype = new JOBType();
                jbtype.Job_Type = "DGFT";
                jbtype.Action = JobTypeCodeFormat.GetDefaultXml(JobTypeCodeFormat.JobType.mDGFT);
                BllMgr.JOBTypeBLL.AddJOBType(jbtype);
            }
            else if (addAs == JobTypeAs.DGFTAll)
            {
                JOBType jbtype = new JOBType();
                jbtype.Job_Type = "DGFT";
                jbtype.Action = JobTypeCodeFormat.GetDefaultXml(JobTypeCodeFormat.JobType.mDGFT);
                BllMgr.JOBTypeBLL.AddJOBType(jbtype);

                jbtype = new JOBType();
                jbtype.Job_Type = "CIP";
                jbtype.Action = JobTypeCodeFormat.GetDefaultXml(JobTypeCodeFormat.JobType.mDGFT_Ex);
                BllMgr.JOBTypeBLL.AddJOBType(jbtype);

                jbtype = new JOBType();
                jbtype.Job_Type = "STADA";
                jbtype.Action = JobTypeCodeFormat.GetDefaultXml(JobTypeCodeFormat.JobType.mDGFT_Ex);
                BllMgr.JOBTypeBLL.AddJOBType(jbtype);

            }
            else if (addAs == JobTypeAs.Customised_JNJ)
            {
                JOBType jbtype = new JOBType();
                jbtype.Job_Type = "Domestic";
                jbtype.Action = JobTypeCodeFormat.GetDefaultXml(JobTypeCodeFormat.JobType.mDomestic);
                BllMgr.JOBTypeBLL.AddJOBType(jbtype);

                jbtype = new JOBType();
                jbtype.Job_Type = "Export";
                jbtype.Action = JobTypeCodeFormat.GetDefaultXml(JobTypeCodeFormat.JobType.mExport);
                BllMgr.JOBTypeBLL.AddJOBType(jbtype);

                jbtype = new JOBType();
                jbtype.Job_Type = "Tender";
                jbtype.Action = JobTypeCodeFormat.GetDefaultXml(JobTypeCodeFormat.JobType.mTender);
                BllMgr.JOBTypeBLL.AddJOBType(jbtype);

                jbtype = new JOBType();
                jbtype.Job_Type = "Export-China-PG";
                jbtype.Action = JobTypeCodeFormat.GetDefaultXml(JobTypeCodeFormat.JobType.mExportChina);
                BllMgr.JOBTypeBLL.AddJOBType(jbtype);

                jbtype = new JOBType();
                jbtype.Job_Type = "DuelMRP";
                jbtype.Action = JobTypeCodeFormat.GetDefaultXml(JobTypeCodeFormat.JobType.mDuelMRP);
                BllMgr.JOBTypeBLL.AddJOBType(jbtype);
            }
        }
        private void CreatPackageTypeCode()
        {
            PackageTypeCode PackTypeCode = new PackageTypeCode();
            PackTypeCode.Code = DECKs.PPB.ToString();
            PackTypeCode.Name = "PrimaryPCK";
            BllMgr.PackageTypeCodeBLL.AddPackageTypeCode(PackTypeCode);

            PackTypeCode = new PackageTypeCode();
            PackTypeCode.Code = DECKs.MOC.ToString();
            PackTypeCode.Name = "MonoCARTON";
            BllMgr.PackageTypeCodeBLL.AddPackageTypeCode(PackTypeCode);

            PackTypeCode = new PackageTypeCode();
            PackTypeCode.Code = DECKs.OBX.ToString();
            PackTypeCode.Name = "OuterBOX";
            BllMgr.PackageTypeCodeBLL.AddPackageTypeCode(PackTypeCode);

            PackTypeCode = new PackageTypeCode();
            PackTypeCode.Code = DECKs.ISH.ToString();
            PackTypeCode.Name = "ISHIPPER";
            BllMgr.PackageTypeCodeBLL.AddPackageTypeCode(PackTypeCode);

            PackTypeCode = new PackageTypeCode();
            PackTypeCode.Code = DECKs.OSH.ToString();
            PackTypeCode.Name = "OSHIPPER";
            BllMgr.PackageTypeCodeBLL.AddPackageTypeCode(PackTypeCode);

            PackTypeCode = new PackageTypeCode();
            PackTypeCode.Code = DECKs.PAL.ToString();
            PackTypeCode.Name = "PALLET";
            BllMgr.PackageTypeCodeBLL.AddPackageTypeCode(PackTypeCode);
        }
        private void CreatMLNO()
        {
            string[] MlNo = new string[] { "MLNO1111111" };

            for (int i = 0; i <= MlNo.Length - 1; i++)
            {
                MLNO m_mlno = new MLNO();
                m_mlno.ML_NO = MlNo[i];
                m_mlno.LastUpdatedDate = DateTime.Now;
                BllMgr.MLNOBLL.InsertOrUpdateMLNO(m_mlno);
            }
        }
    }
}
